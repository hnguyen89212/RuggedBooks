using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RuggedBooksDAL.Repository.IRepository;
using RuggedBooksModels;
using RuggedBooksModels.ViewModels;
using RuggedBooksUtilities;

namespace RuggedBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Administrator)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // We need to upload the image into the wwwroot/img later so web host env is necessary.
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(each => new SelectListItem
                {
                    Text = each.CategoryName,
                    Value = each.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(each => new SelectListItem
                {
                    Text = each.Name,
                    Value = each.Id.ToString()
                })
            };

            // Id is null when we create/add a new cateogry
            if (Id == null)
            {
                return View(productVM);
            }

            // Otherwise, we are trying to update an existing? category
            // For an integer, the default value is 0.
            productVM.Product = _unitOfWork.Product.Get(Id.GetValueOrDefault());

            // The Id does not associate with any category.
            if (productVM == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        // User updates a product and its image as well. We need to remove the old image.
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extenstion;
                }
                else
                {
                    // User updates a product but leaves the image url unchanged. So we grab the existing image url.
                    if (productVM.Product.Id != 0)
                    {
                        Product fetchedProduct = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = fetchedProduct.ImageUrl;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                });
                productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }

            return View(productVM);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new
            {
                data = allProducts
            });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            Product product = _unitOfWork.Product.Get(Id);

            if (product == null)
            {
                return Json(new { success = false, message = "Error removing product. Please try again." });
            }
            
            // Deletes the associated image
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product is successfully removed." });
        }

        #endregion
    }
}