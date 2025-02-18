namespace moje_filmy_api.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public required string Title { get; set; }
        public string ?Director { get; set; } = null;
        public int ?Year { get; set; } = null;
        public int ?Rate { get; set; } = null;
    }
}
