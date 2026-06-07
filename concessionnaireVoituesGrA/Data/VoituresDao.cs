using concessionnaireVoituesGrA.Domains;
using Microsoft.Data.SqlClient;
using Dapper;
using concessionnaireVoituesGrA.Entities;
using concessionnaireVoituesGrA.Services;
using System.Data.Common;
namespace concessionnaireVoituesGrA.Data
{
    public class VoituresDao
    {
        InterfaceDbFactory factory;
        public VoituresDao(InterfaceDbFactory factory) //DI
        {
            this.factory = factory;
        }
        public void Ajouter(Voiture voiture)
        {
            VoitureEntity voitureEntity = new VoitureEntity();
            AutoMapping<Voiture, VoitureEntity>.Map(voiture, voitureEntity);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = @"INSERT INTO Voitures(Matricule, Marque, Modele, Categorie, Annee, PrixLocation, EtatDisponibilite, ImageUrl) 
                        VALUES (@Matricule, @Marque, @Modele, @Categorie, @Annee, @PrixLocation, @EtatDisponibilite, @ImageUrl)";
                connection.Execute(sql, voitureEntity);
            }
        }
        public Voiture GetVoiture(string matricule)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM Voitures WHERE Matricule = @Matricule";
                VoitureEntity voitureEntity = connection.QuerySingleOrDefault<VoitureEntity>(sql, new { Matricule = matricule });
                if (voitureEntity == null)
                    return null;
                Voiture voiture = new Voiture();
                AutoMapping<VoitureEntity, Voiture>.Map(voitureEntity, voiture);
                return voiture;
            }
        }
        public List<Voiture> GetAllVoitures()
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM Voitures";
                List<VoitureEntity> liste1 = connection.Query<VoitureEntity>(sql).ToList();
                List<Voiture> liste2 = new List<Voiture>();
                foreach (var item in liste1)
                {
                    Voiture voiture = new Voiture();
                    AutoMapping<VoitureEntity, Voiture>.Map(item, voiture);
                    liste2.Add(voiture);
                }
                return liste2;
            }
        }

        public bool Modifier(string matricule, Voiture voiture)
        {
            VoitureEntity voitureEntity = new VoitureEntity();
            AutoMapping<Voiture, VoitureEntity>.Map(voiture, voitureEntity);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = @$"UPDATE Voitures SET Marque=@Marque, Modele=@Modele, Categorie=@Categorie,
                            Annee=@Annee, PrixLocation=@PrixLocation, EtatDisponibilite=@EtatDisponibilite, ImageUrl=@ImageUrl 
                            WHERE Matricule='{matricule}'";
                connection.Execute(sql, voitureEntity);
            }
            return true;
        }
        public bool Supprimer(string matricule)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "DELETE FROM Voitures WHERE Matricule = @Matricule";
                connection.Execute(sql, new { Matricule = matricule });
            }
            return true;
        }
    }
}
