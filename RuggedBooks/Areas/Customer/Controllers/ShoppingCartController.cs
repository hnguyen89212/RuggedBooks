using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using RuggedBooksModels.ViewModels;
using RuggedBooksUtilities;

namespace RuggedBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IEmailSender _emailSender;

        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCarts = _unitOfWork.ShoppingCart
                    .GetAll(cart => cart.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Id == claim.Value, includeProperties: "Company");

            foreach (ShoppingCart cart in ShoppingCartVM.ShoppingCarts)
            {
                cart.Price = Utilities.GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);

                cart.Product.Description = Utilities.ConvertToRawHtml(cart.Product.Description);
                if (cart.Product.Description.Length > 100)
                {
                    cart.Product.Description = cart.Product.Description.Substring(0, 99) + "...";
                }
            }

            return View(ShoppingCartVM);
        }

        public IActionResult IncrementQuantity(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");
            cart.Count++;

            // Re-determines the price since the quantity varies.
            cart.Price = Utilities.GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DecrementQuantity(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");

            if (cart.Count == 1)
            {
                var totalItemsInCart = _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cart.ApplicationUserId)
                    .ToList()
                    .Count;

                _unitOfWork.ShoppingCart.Remove(cart);

                _unitOfWork.Save();
                
                HttpContext.Session.SetInt32(SD.Shopping_Cart_Session, totalItemsInCart - 1);
            }
            else
            {
                cart.Count--;
                cart.Price = Utilities.GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                                    cart.Product.Price50, cart.Product.Price100);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveItem(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");

            var totalItemsInCart = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cart.ApplicationUserId)
                .ToList()
                .Count;

            _unitOfWork.ShoppingCart.Remove(cart);
            
            _unitOfWork.Save();
            
            HttpContext.Session.SetInt32(SD.Shopping_Cart_Session, --totalItemsInCart);

            return RedirectToAction(nameof(Index));
        }
    }
}