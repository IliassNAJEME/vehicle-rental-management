using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public interface InterfaceComptes
    {
        void Creer(CompteDto compte);
        bool Modifier(string username, CompteDto compte);
        CompteDto Authentifier(CompteDto compte);
        CompteDto Rechercher(string username);
        List<CompteDto> GetComptes();
        bool Supprimer(string username);
    }
}
