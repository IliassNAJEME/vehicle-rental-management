using concessionnaireVoituesGrA.Data;
using concessionnaireVoituesGrA.Domains;
using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public class GestionVoitures : InterfaceVoitures
    {
        VoituresDao dao ;
        public GestionVoitures(VoituresDao dao)
        {
            this.dao = dao;
        }
        public bool AjouterVoiture(VoitureDto voitureDto)
        {
           VoitureDto voitureDto1= GetVoiture(voitureDto.Matricule); //pour vérifier si la voiture existe ou pas
           if(voitureDto1 != null) 
                return false;
            Voiture voiture=new Voiture();
            AutoMapping<VoitureDto,Voiture>.Map(voitureDto, voiture);
            dao.Ajouter(voiture);
            return true;
        }

        public List<VoitureDto> GetAllVoitures()
        {
            List<Voiture> liste1=dao.GetAllVoitures();
            List<VoitureDto> liste2=new List<VoitureDto>();
            foreach (var item in liste1)
            {
                VoitureDto voitureDto = new VoitureDto();
                AutoMapping<Voiture,VoitureDto>.Map(item, voitureDto);
                liste2.Add(voitureDto);
            }
            return liste2;
        }

        public VoitureDto GetVoiture(string matricule)
        {
           Voiture voiture= dao.GetVoiture(matricule);
           if (voiture == null)
               return null;
           VoitureDto voitureDto = new VoitureDto();
           AutoMapping<Voiture,VoitureDto>.Map(voiture, voitureDto);
           return voitureDto;
        }

        public bool ModifierVoiture(string matricule, VoitureDto voitureDto)
        {
            Voiture voiture = new Voiture();
            AutoMapping<VoitureDto,Voiture>.Map(voitureDto, voiture);
           return dao.Modifier(matricule, voiture);
        }

        public bool SupprimerVoiture(string matricule)
        {
            VoitureDto voitureDto = GetVoiture(matricule); //pour vérifier si la voiture existe ou pas
            if(voitureDto == null) 
                return false;
            return dao.Supprimer(matricule);
        }
    }
}
