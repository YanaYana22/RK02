using Newtonsoft.Json;

namespace RK02.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }
    }
}
