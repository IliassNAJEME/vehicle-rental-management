namespace concessionnaireVoituesGrA.Domains
{
    public class Voiture
    {
        public string Matricule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Categorie { get; set; }
        public int Annee { get; set; }
        public double PrixLocation { get; set; }
        public string EtatDisponibilite { get; set; } // Disponible, Louee, EnMaintenance
        public string ImageUrl { get; set; }
    }
}
