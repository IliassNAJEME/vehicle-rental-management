using BCrypt.Net;
using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Entities;
using concessionnaireVoituesGrA.Services;
using Dapper;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Data
{
    /// <summary>
    /// DAO Comptes — mots de passe haches avec BCrypt (work factor 12)
    /// Migration automatique des anciens mots de passe en clair
    /// </summary>
    public class ComptesDao
    {
        private readonly InterfaceDbFactory _factory;

        public ComptesDao(InterfaceDbFactory factory) => _factory = factory;

        public void Ajouter(Compte compte)
        {
            compte.Password = BCrypt.Net.BCrypt.HashPassword(compte.Password, workFactor: 12);
            var entity = new CompteEntity();
            AutoMapping<Compte, CompteEntity>.Map(compte, entity);
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            conn.Execute("INSERT INTO Comptes (Username, Password, Role) VALUES (@Username, @Password, @Role)", entity);
        }

        public List<Compte> GetAllComptes()
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            return conn.Query<CompteEntity>("SELECT * FROM Comptes").Select(e => {
                var c = new Compte(); AutoMapping<CompteEntity, Compte>.Map(e, c); return c;
            }).ToList();
        }

        internal Compte Authentifier(Compte c)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            var entity = conn.QueryFirstOrDefault<CompteEntity>(
                "SELECT * FROM Comptes WHERE Username = @Username", new { c.Username });
            if (entity == null) return null;

            bool valid;
            if (entity.Password.StartsWith("$2"))
            {
                valid = BCrypt.Net.BCrypt.Verify(c.Password, entity.Password);
            }
            else
            {
                // Migration automatique des mots de passe en clair
                valid = entity.Password == c.Password;
                if (valid)
                {
                    string hashed = BCrypt.Net.BCrypt.HashPassword(c.Password, workFactor: 12);
                    conn.Execute("UPDATE Comptes SET Password = @Password WHERE Username = @Username",
                        new { Password = hashed, entity.Username });
                }
            }

            if (!valid) return null;
            var result = new Compte();
            AutoMapping<CompteEntity, Compte>.Map(entity, result);
            return result;
        }

        public Compte Rechercher(string username)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            var entity = conn.QueryFirstOrDefault<CompteEntity>(
                "SELECT * FROM Comptes WHERE Username = @Username", new { Username = username });
            if (entity == null) return null;
            var compte = new Compte();
            AutoMapping<CompteEntity, Compte>.Map(entity, compte);
            return compte;
        }

        public bool Modifier(string username, Compte compte)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(compte.Password, workFactor: 12);
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            return conn.Execute("UPDATE Comptes SET Password = @Password WHERE Username = @Username",
                new { Password = hashed, Username = username }) > 0;
        }

        public bool Supprimer(string username)
        {
            using DbConnection conn = _factory.CreateConnection();
            conn.Open();
            return conn.Execute("DELETE FROM Comptes WHERE Username = @Username", new { Username = username }) > 0;
        }
    }
}
