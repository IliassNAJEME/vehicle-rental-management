namespace concessionnaireVoituesGrA.Domains
{
    public class Location
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Voiture Voiture { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public double MontantTotal { get; set; }
        public string Statut { get; set; } // En attente, Validée, Refusée
    }
}
