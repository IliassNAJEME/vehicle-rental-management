using concessionnaireVoituesGrA.Models;

namespace concessionnaireVoituesGrA.Services
{
    public interface InterfaceVoitures
    {
        bool AjouterVoiture(VoitureDto voitureDto);
        VoitureDto GetVoiture(string matricule);
        List<VoitureDto> GetAllVoitures();
        bool SupprimerVoiture(string matricule);
        bool ModifierVoiture(string matricule, VoitureDto voitureDto);
    }
}
