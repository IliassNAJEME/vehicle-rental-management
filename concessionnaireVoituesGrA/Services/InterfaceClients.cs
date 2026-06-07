using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public interface InterfaceClients
    {
        void Ajouter(ClientDto clientDto);
        bool Modifier(string cine, ClientDto clientDto);
        bool Supprimer(string cine);
        ClientDto GetClient(string cine);
        List<ClientDto> GetAllClients();
    }
}
