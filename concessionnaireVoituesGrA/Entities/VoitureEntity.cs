namespace concessionnaireVoituesGrA.Entities
{
    public class VoitureEntity
    {
        public int Id { get; set; }
        public string Matricule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Categorie { get; set; }
        public int Annee { get; set; }
        public double PrixLocation { get; set; }
        public string EtatDisponibilite { get; set; }
        public string ImageUrl { get; set; }
    }
}
