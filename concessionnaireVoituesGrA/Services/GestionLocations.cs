using concessionnaireVoituesGrA.Data;
using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Entities;
using Dapper;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Services
{
    public class GestionLocations : InterfaceLocations
    {
        private readonly DemandesLocationDao _demandesDao;
        private readonly VoituresDao _voituresDao;
        private readonly InterfaceDbFactory _factory;

        public GestionLocations(DemandesLocationDao demandesLocationDao, VoituresDao voituresDao, InterfaceDbFactory factory)
        {
            _demandesDao = demandesLocationDao;
            _voituresDao = voituresDao;
            _factory = factory;
        }

        public void AjouterDemande(Location location)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                // Une voiture peut être réservée si elle est 'Disponible' OU 'Louee' (on va vérifier les dates ensuite)
                // On empêche seulement la réservation si elle est 'Indisponible' ou supprimée.
                var voiture = conn.QueryFirstOrDefault<VoitureEntity>(
                    "SELECT * FROM Voitures WHERE Matricule = @Matricule AND EtatDisponibilite IN ('Disponible', 'Louee', 'Louée')",
                    new { location.Voiture.Matricule }, transaction);

                if (voiture == null)
                    throw new InvalidOperationException("Cette voiture n'est plus disponible à la location.");

                int conflicts = conn.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM DemandesLocation
                    WHERE MatriculeVoiture = @Matricule
                      AND Statut IN ('En attente', 'Validee', 'Validée')
                      AND DateDebut <= @DateFin
                      AND DateFin >= @DateDebut",
                    new { location.Voiture.Matricule, location.DateDebut, location.DateFin }, transaction);

                if (conflicts > 0)
                    throw new InvalidOperationException("Cette voiture est deja reservee sur ces dates. Choisissez d'autres dates.");

                var days = (location.DateFin - location.DateDebut).TotalDays;
                if (days < 1) days = 1;
                location.MontantTotal = Math.Round(days * voiture.PrixLocation, 2);
                location.Statut = "En attente";

                conn.Execute(@"
                    INSERT INTO DemandesLocation (UsernameClient, MatriculeVoiture, DateDebut, DateFin, MontantTotal, Statut)
                    VALUES (@UsernameClient, @MatriculeVoiture, @DateDebut, @DateFin, @MontantTotal, @Statut)",
                    new {
                        UsernameClient = location.Client.Nom,
                        MatriculeVoiture = location.Voiture.Matricule,
                        location.DateDebut,
                        location.DateFin,
                        location.MontantTotal,
                        location.Statut
                    }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private List<Location> MapDemandes(IEnumerable<dynamic> rows) =>
            rows.Select(r => new Location {
                Id = (int)r.Id,
                DateDebut = (DateTime)r.DateDebut,
                DateFin = (DateTime)r.DateFin,
                MontantTotal = (double)r.MontantTotal,
                Statut = (string)r.Statut,
                Client = new Client { Nom = (string)r.UsernameClient },
                Voiture = new Voiture {
                    Matricule = (string)r.Matricule,
                    Marque = (string)(r.Marque ?? ""),
                    Modele = (string)(r.Modele ?? ""),
                    PrixLocation = (double)r.PrixLocation,
                    EtatDisponibilite = (string)(r.EtatDisponibilite ?? "")
                }
            }).ToList();

        public List<Location> GetAllDemandes()
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            const string sql = @"
                SELECT d.Id, d.UsernameClient, d.MatriculeVoiture, d.DateDebut, d.DateFin, d.MontantTotal, d.Statut,
                       v.Matricule, v.Marque, v.Modele, v.Categorie, v.Annee, v.PrixLocation, v.EtatDisponibilite, v.ImageUrl
                FROM DemandesLocation d
                JOIN Voitures v ON d.MatriculeVoiture = v.Matricule
                ORDER BY d.Id DESC";

            return conn.Query<LocationEntity, VoitureEntity, Location>(sql, (locEntity, voitEntity) => {
                var voiture = new Voiture();
                AutoMapping<VoitureEntity, Voiture>.Map(voitEntity, voiture);
                return new Location {
                    Id = locEntity.Id,
                    DateDebut = locEntity.DateDebut,
                    DateFin = locEntity.DateFin,
                    MontantTotal = locEntity.MontantTotal,
                    Statut = locEntity.Statut,
                    Client = new Client { Nom = locEntity.UsernameClient },
                    Voiture = voiture
                };
            }, splitOn: "Matricule").ToList();
        }

        public List<Location> GetDemandesByClient(string usernameClient)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            const string sql = @"
                SELECT d.Id, d.UsernameClient, d.MatriculeVoiture, d.DateDebut, d.DateFin, d.MontantTotal, d.Statut,
                       v.Matricule, v.Marque, v.Modele, v.Categorie, v.Annee, v.PrixLocation, v.EtatDisponibilite, v.ImageUrl
                FROM DemandesLocation d
                JOIN Voitures v ON d.MatriculeVoiture = v.Matricule
                WHERE d.UsernameClient = @UsernameClient
                ORDER BY d.Id DESC";

            return conn.Query<LocationEntity, VoitureEntity, Location>(sql, (locEntity, voitEntity) => {
                var voiture = new Voiture();
                AutoMapping<VoitureEntity, Voiture>.Map(voitEntity, voiture);
                return new Location {
                    Id = locEntity.Id,
                    DateDebut = locEntity.DateDebut,
                    DateFin = locEntity.DateFin,
                    MontantTotal = locEntity.MontantTotal,
                    Statut = locEntity.Statut,
                    Client = new Client { Nom = locEntity.UsernameClient },
                    Voiture = voiture
                };
            }, new { UsernameClient = usernameClient }, splitOn: "Matricule").ToList();
        }

        public void ValiderDemande(int id)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                var demande = conn.QuerySingleOrDefault<LocationEntity>(
                    "SELECT * FROM DemandesLocation WHERE Id = @Id", new { Id = id }, transaction);
                if (demande == null) throw new InvalidOperationException("Demande introuvable.");

                conn.Execute("UPDATE DemandesLocation SET Statut = 'Validee' WHERE Id = @Id", new { Id = id }, transaction);
                
                // La voiture prend le statut "Louee" pour indiquer qu'elle a au moins une réservation en cours/future
                conn.Execute("UPDATE Voitures SET EtatDisponibilite = 'Louee' WHERE Matricule = @Matricule",
                    new { Matricule = demande.MatriculeVoiture }, transaction);
                
                // On refuse AUTOMATIQUEMENT uniquement les autres demandes en attente qui CHEVAUCHENT les mêmes dates
                conn.Execute(@"
                    UPDATE DemandesLocation 
                    SET Statut = 'Refusee' 
                    WHERE MatriculeVoiture = @Matricule 
                      AND Id != @Id 
                      AND Statut = 'En attente'
                      AND DateDebut <= @DateFin 
                      AND DateFin >= @DateDebut",
                    new { Matricule = demande.MatriculeVoiture, Id = id, DateDebut = demande.DateDebut, DateFin = demande.DateFin }, transaction);
                
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }

        public void RefuserDemande(int id)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                var demande = conn.QuerySingleOrDefault<LocationEntity>(
                    "SELECT * FROM DemandesLocation WHERE Id = @Id", new { Id = id }, transaction);
                if (demande == null) throw new InvalidOperationException("Demande introuvable.");

                conn.Execute("UPDATE DemandesLocation SET Statut = 'Refusee' WHERE Id = @Id", new { Id = id }, transaction);

                int validees = conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM DemandesLocation WHERE MatriculeVoiture = @Matricule AND Statut = 'Validee'",
                    new { Matricule = demande.MatriculeVoiture }, transaction);

                if (validees == 0)
                    conn.Execute("UPDATE Voitures SET EtatDisponibilite = 'Disponible' WHERE Matricule = @Matricule",
                        new { Matricule = demande.MatriculeVoiture }, transaction);

                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }
    }
}
