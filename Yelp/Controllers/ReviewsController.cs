using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Yelp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yelp.Controllers
{
  public class ReviewsController : Controller
  {
    private readonly YelpContext _db;

    public ReviewsController(YelpContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Review> model = _db.Reviews
                            .Include(review => review.Restaurant)
                            .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.RestaurantId = new SelectList(_db.Restaurants, "RestaurantId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Review review)
    {
      if (review.RestaurantId == 0)
      {
        return RedirectToAction("Create");
      }
      _db.Reviews.Add(review);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Review thisReview = _db.Reviews
                          .Include(review => review.Restaurant)
                          .FirstOrDefault(review => review.ReviewId == id);
      return View(thisReview);
    }

    public ActionResult Edit(int id)
    {
      Review thisReview = _db.Reviews.FirstOrDefault(review => review.ReviewId == id);
      ViewBag.RestaurantId = new SelectList(_db.Restaurants, "RestaurantId", "Name");
      return View(thisReview);
    }

    [HttpPost]
    public ActionResult Edit(Review review)
    {
      _db.Reviews.Update(review);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Review thisReview = _db.Reviews.FirstOrDefault(review => review.ReviewId == id);
      return View(thisReview);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Review thisReview = _db.Reviews.FirstOrDefault(review => review.ReviewId == id);
      _db.Reviews.Remove(thisReview);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Filter()
    {
      List<Review> model = _db.Reviews
                            .Include(review => review.Restaurant)
                            .ToList();
      return View();
    }


    private async Task<List<Review>> Search(int searchQuery)
    {
      IQueryable<Review> reviewsList = _db.Set<Review>();

      if (searchQuery != 0)
      {
        return await reviewsList?
                                .Include(review => review.Restaurant)
                                .Where(review => review.Rating >= searchQuery)
                                .OrderByDescending(review => review.Rating)
                                .ToListAsync();
      }
      else
      {
        return await reviewsList.ToListAsync();
      }
    }

    public async Task<IActionResult> Display(int searchQuery)
    {
      List<Review> resultList = await Search(searchQuery);
      ViewBag.Query = searchQuery.ToString();
      return View(resultList);
    }
  }
}