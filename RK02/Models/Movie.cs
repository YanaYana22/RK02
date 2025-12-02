using Newtonsoft.Json;

namespace RK02.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public double Rating { get; set; }

        [JsonIgnore]
        public Genre Genre { get; set; }
    }
}
