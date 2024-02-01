using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Yelp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Yelp.Controllers
{
  public class RestaurantsController : Controller
  {
    private readonly YelpContext _db;

    public RestaurantsController(YelpContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Restaurant> model = _db.Restaurants
                            .Include(restaurant => restaurant.Cuisine)
                            .ToList();
      return View(model);
    }

    private async Task<List<Restaurant>> SearchMethod(string searchQuery)
  {
    IQueryable<Restaurant> restaurantsList = _db.Set<Restaurant>();

    if (searchQuery != null)
    {
      return await restaurantsList?
                                .Include(restaurant => restaurant.Cuisine)
                                .Where(restaurant => restaurant.Name.Contains(searchQuery) 
                                || restaurant.Cuisine.Type.Contains(searchQuery))
                                .ToListAsync();
    }
    else
    {
      return await restaurantsList.ToListAsync();
    }
  }
    public async Task<IActionResult> Results(string searchQuery)
    {
      List<Restaurant> resultList = await SearchMethod(searchQuery);
      // ViewBag.PageTitle = "View Restaurants";
      return View(resultList);
    }

    public ActionResult Create()
    {
      ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Restaurant restaurant)
    {
      if (restaurant.CuisineId == 0)
      {
        return RedirectToAction("Create");
      }
      _db.Restaurants.Add(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants
                          .Include(restaurant => restaurant.Reviews)
                          .Include(restaurant => restaurant.Cuisine)
                          .FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    public ActionResult Edit(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
      return View(thisRestaurant);
    }

    [HttpPost]
    public ActionResult Edit(Restaurant restaurant)
    {
      _db.Restaurants.Update(restaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      return View(thisRestaurant);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Restaurant thisRestaurant = _db.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == id);
      _db.Restaurants.Remove(thisRestaurant);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }


  }
}