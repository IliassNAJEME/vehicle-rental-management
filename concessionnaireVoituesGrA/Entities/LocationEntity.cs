using System;

namespace concessionnaireVoituesGrA.Entities
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public string UsernameClient { get; set; }
        public string MatriculeVoiture { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public double MontantTotal { get; set; }
        public string Statut { get; set; }
    }
}
