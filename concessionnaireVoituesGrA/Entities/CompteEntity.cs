using concessionnaireVoituesGrA.Domains;

namespace concessionnaireVoituesGrA.Entities
{
    public class CompteEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int  IdClient { get; set; }
    }
}
