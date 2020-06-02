using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RuggedBooksDAL.Repository;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using RuggedBooksModels.ViewModels;

namespace RuggedBooks.Area.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // Retrieves all the products in db and render them in the homepage.
        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int productId)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(each => each.Id == productId, includeProperties: "Category,CoverType");

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;

            if (ModelState.IsValid)
            {
                // fetches the current user
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                // Sets the cart owner
                shoppingCart.ApplicationUserId = claim.Value;

                //
                ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    each => each.ApplicationUserId == shoppingCart.ApplicationUserId && each.ProductId == shoppingCart.ProductId
                );

                if (shoppingCartFromDb == null)
                {
                    // there is no record of this user buying this product.
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    // updates
                    shoppingCartFromDb.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
                }
                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // We re-render the Details page with the same old product

                var product = _unitOfWork.Product.GetFirstOrDefault(each => each.Id == shoppingCart.ProductId, includeProperties: "Category,CoverType");

                shoppingCart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };

                return View(shoppingCart);
            }
        }
    }
}
