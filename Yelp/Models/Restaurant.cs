using System.Collections.Generic;

namespace Yelp.Models
{
  public class Restaurant
  {
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Hours { get; set; }
    public string Address { get; set; }
    public int CuisineId { get; set; }
    public Cuisine Cuisine { get; set; }
    public List<Review> Reviews { get; set; }
  }
}