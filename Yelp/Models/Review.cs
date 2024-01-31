namespace Yelp.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public string Critique { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}