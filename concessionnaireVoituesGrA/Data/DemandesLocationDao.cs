using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Entities;
using concessionnaireVoituesGrA.Services;
using Dapper;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Data
{
    public class DemandesLocationDao
    {
        InterfaceDbFactory factory;

        public DemandesLocationDao(InterfaceDbFactory factory)
        {
            this.factory = factory;
        }

        public void Ajouter(Location location)
        {
            LocationEntity entity = new LocationEntity
            {
                UsernameClient = location.Client.Nom, // Nom contient le Username du compte connecté
                MatriculeVoiture = location.Voiture.Matricule,
                DateDebut = location.DateDebut,
                DateFin = location.DateFin,
                MontantTotal = location.MontantTotal,
                Statut = location.Statut
            };

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = @"INSERT INTO DemandesLocation(UsernameClient, MatriculeVoiture, DateDebut, DateFin, MontantTotal, Statut) 
                               VALUES (@UsernameClient, @MatriculeVoiture, @DateDebut, @DateFin, @MontantTotal, @Statut)";
                connection.Execute(sql, entity);
            }
        }

        public List<LocationEntity> GetAllDemandes()
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM DemandesLocation";
                return connection.Query<LocationEntity>(sql).ToList();
            }
        }

        public List<LocationEntity> GetDemandesByClient(string usernameClient)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM DemandesLocation WHERE UsernameClient = @UsernameClient";
                return connection.Query<LocationEntity>(sql, new { UsernameClient = usernameClient }).ToList();
            }
        }

        public bool ModifierStatut(int id, string statut)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "UPDATE DemandesLocation SET Statut = @Statut WHERE Id = @Id";
                connection.Execute(sql, new { Statut = statut, Id = id });
            }
            return true;
        }

        public LocationEntity GetDemande(int id)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM DemandesLocation WHERE Id = @Id";
                return connection.QuerySingleOrDefault<LocationEntity>(sql, new { Id = id });
            }
        }
    }
}
