using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessObject.Models;
using DataAccess.Repository;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace eStore.Controllers
{
    public class ProductsController : Controller
    {
        IProductRepository productsRepository;
        ICategoryRepository categoryRepository;
        public ProductsController()
        {
            productsRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }
        // GET: ProductsController

        public ActionResult Index(string SearchValue, decimal Max, decimal Min)
        {

            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Search"] = SearchValue;
            ViewData["min"] = Min;
            ViewData["max"] = Max;

            var productList = productsRepository.SearchProduct(SearchValue, Min, Max).ToList();
            return View(productList);

        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = productsRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            var List = categoryRepository.GetAllCategoriesForList();
            ViewBag.List = new SelectList(List, nameof(Category.CategoryId), nameof(Category.CategoryName));
            Product product = new Product();
            product.ProductId = productsRepository.GetMaxId() + 1;
            return View(product);
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    productsRepository.Add(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(product);
            }

        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = productsRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            var list = categoryRepository.GetAllCategoriesForList();
            ViewBag.CategoryId = new SelectList(list, nameof(Category.CategoryId), nameof(Category.CategoryName));

            return View(product);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    productsRepository.Update(product);
                }
                ViewBag.CategoryId = new SelectList(categoryRepository.GetAllCategoriesForList(), nameof(Category.CategoryId), nameof(Category.CategoryName));
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(product);
            }

        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = productsRepository.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                productsRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}
