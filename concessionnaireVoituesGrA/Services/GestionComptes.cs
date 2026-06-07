using concessionnaireVoituesGrA.Data;
using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public class GestionComptes : InterfaceComptes
    {
        ComptesDao dao;
        public GestionComptes(ComptesDao dao)
        {
            this.dao = dao;
        }

        public CompteDto Authentifier(CompteDto compte)
        {
           Compte c = new Compte();
            AutoMapping<CompteDto, Compte>.Map(compte, c);
            var result = dao.Authentifier(c);
            if (result == null) return null;
            CompteDto dto = new CompteDto();
            AutoMapping<Compte, CompteDto>.Map(result, dto);
            return dto;
        }

        public void Creer(CompteDto compte)
        {
            Compte c=new Compte();
            AutoMapping<CompteDto, Compte>.Map(compte, c);
            c.Role="Client";
            dao.Ajouter(c);
        }

        public List<CompteDto> GetComptes()
        {
           List<Compte> liste1=  dao.GetAllComptes();
              List<CompteDto> liste2=new List<CompteDto>();
                foreach (var item in liste1)
                {
                 CompteDto compteDto = new CompteDto();
                 AutoMapping<Compte,CompteDto>.Map(item, compteDto);
                 liste2.Add(compteDto);
                }
                return liste2;
        }

        public bool Modifier(string username, CompteDto compte)
        {
            Compte c = new Compte();
            AutoMapping<CompteDto, Compte>.Map(compte, c);
            return dao.Modifier(username, c);
        }

        public CompteDto Rechercher(string username)
        {
            Compte c = dao.Rechercher(username);
            if (c == null) return null;
            CompteDto dto = new CompteDto();
            AutoMapping<Compte, CompteDto>.Map(c, dto);
            return dto;
        }

        public bool Supprimer(string username)
        {
            return dao.Supprimer(username);
        }
    }
}
