using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    //service pour faire les tests:
    public class ServiceComptesV0 : InterfaceComptes
    {
        static List<CompteDto> comptes = new List<CompteDto>();
        public CompteDto Authentifier(CompteDto compte)
        {
            var found = comptes.Find(c => c.Username == compte.Username && c.Password == compte.Password);
            return found;
        }

        public void Creer(CompteDto compte)
        {
            comptes.Add(compte);
        }

        public List<CompteDto> GetComptes()
        {
            return comptes;
        }

        public bool Modifier(string username, CompteDto compte)
        {
            var found = Rechercher(username);
            if (found != null)
            {
                found.Password = compte.Password;
                found.Role = compte.Role;
                return true;
            }
            return false;
        }

        public CompteDto Rechercher(string username)
        {
            return comptes.Find(c => c.Username == username);
        }

        public bool Supprimer(string username)
        {
            var found = Rechercher(username);
            if (found != null)
            {
                comptes.Remove(found);
                return true;
            }
            return false;
        }
    }
}
