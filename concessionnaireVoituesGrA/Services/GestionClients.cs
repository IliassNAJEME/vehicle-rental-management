using concessionnaireVoituesGrA.Data;
using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public class GestionClients : InterfaceClients
    {
        ClientsDao dao;
        public GestionClients(ClientsDao dao)
        {
            this.dao = dao;
        }
        public void Ajouter(ClientDto clientDto)
        {
            Client client = new Client();
           AutoMapping<ClientDto, Client>.Map(clientDto, client);
            dao.Ajouter(client);
        }

        public List<ClientDto> GetAllClients()
        {
            List<Client> clients = dao.GetAllClients();
            List<ClientDto> liste2 = new List<ClientDto>();
            foreach (var client in clients)
            {
                ClientDto clientDto = new ClientDto();
                AutoMapping<Client, ClientDto>.Map(client, clientDto);
                liste2.Add(clientDto);
            }
            return liste2;
        }

        public ClientDto GetClient(string cine)
        {
           Client client= dao.GetClient(cine);
            if(client==null)
            {
                return null;
            }
            ClientDto clientDto = new ClientDto();
            AutoMapping<Client, ClientDto>.Map(client, clientDto);
            return clientDto;
        }

        public bool Modifier(string cine, ClientDto clientDto)
        {
            
            Client client = new Client();
            AutoMapping<ClientDto, Client>.Map(clientDto, client);
            return dao.Modifier(cine, client);
        }

        public bool Supprimer(string cine)
        {
           
            return dao.Supprimer(cine);
        }
    }
}
