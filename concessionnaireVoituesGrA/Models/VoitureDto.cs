namespace concessionnaireVoituesGrA.Models
{
    public class VoitureDto
    {
        public string Matricule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Categorie { get; set; }
        public int Annee { get; set; }
        public double PrixLocation { get; set; }
        public string EtatDisponibilite { get; set; }
        public string ImageUrl { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }
    }
}
