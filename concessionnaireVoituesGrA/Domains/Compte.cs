namespace concessionnaireVoituesGrA.Domains
{
    public class Compte
    {
        public string Username { get; set; }    
        public string Password { get; set; }
        public string Role { get; set; }   
        public Client Client { get; set; }
    }
}
