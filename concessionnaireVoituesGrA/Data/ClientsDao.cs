using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Entities;
using concessionnaireVoituesGrA.Services;
using Dapper;
using System.Data.Common;

namespace concessionnaireVoituesGrA.Data
{
    public class ClientsDao
    {
        InterfaceDbFactory factory;
        public ClientsDao(InterfaceDbFactory factory) //DI
        {
            this.factory = factory;
        }

        public void Ajouter(Client client)
        {
            ClientEntity clientEntity = new ClientEntity();
            AutoMapping<Client, ClientEntity>.Map(client, clientEntity);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "INSERT INTO Clients (CINE, Nom, Prenom, Tel, Adresse) VALUES (@CINE, @Nom, @Prenom, @Tel, @Adresse)";
                connection.Execute(sql, clientEntity);
            }
        }

        public Client GetClient(string cine)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM Clients WHERE CINE = @CINE";
                ClientEntity clientEntity = connection.QuerySingleOrDefault<ClientEntity>(sql, new { CINE = cine });
                if (clientEntity == null) return null;
                Client client = new Client();
                AutoMapping<ClientEntity, Client>.Map(clientEntity, client);
                return client;
            }
        }

        public List<Client> GetAllClients()
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "SELECT * FROM Clients";
                List<ClientEntity> liste1 = connection.Query<ClientEntity>(sql).ToList();
                List<Client> liste2 = new List<Client>();
                foreach (var item in liste1)
                {
                    Client client = new Client();
                    AutoMapping<ClientEntity, Client>.Map(item, client);
                    liste2.Add(client);
                }
                return liste2;
            }
        }

        public bool Modifier(string cine, Client client)
        {
            ClientEntity clientEntity = new ClientEntity();
            AutoMapping<Client, ClientEntity>.Map(client, clientEntity);

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = $"UPDATE Clients SET Nom=@Nom, Prenom=@Prenom, Tel=@Tel, Adresse=@Adresse WHERE CINE='{cine}'";
                connection.Execute(sql, clientEntity);
            }
            return true;
        }

        public bool Supprimer(string cine)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.Open();
                string sql = "DELETE FROM Clients WHERE CINE = @CINE";
                connection.Execute(sql, new { CINE = cine });
            }
            return true;
        }
    }
}
