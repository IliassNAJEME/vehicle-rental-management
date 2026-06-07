using concessionnaireVoituesGrA.Domains;
using System.Collections.Generic;

namespace concessionnaireVoituesGrA.Services
{
    public interface InterfaceLocations
    {
        void AjouterDemande(Location location);
        List<Location> GetAllDemandes();
        List<Location> GetDemandesByClient(string usernameClient);
        void ValiderDemande(int id);
        void RefuserDemande(int id);
    }
}
