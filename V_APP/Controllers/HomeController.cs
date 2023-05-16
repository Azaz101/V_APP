using V_APP.Data;
using V_APP.DTOs;
using V_APP.Helper;
using V_APP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Mail;
using static Humanizer.In;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;
using System.Linq;

namespace V_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly v_appContext _dbContext;
        private readonly IWebHostEnvironment _he;

        public HomeController(ILogger<HomeController> logger, v_appContext dbContext, IWebHostEnvironment he)
        {
            _logger = logger;
            _dbContext = dbContext;
            _he = he;
        }
        #region Static
        
        public IActionResult Index()
        {
            ViewBag.TopProduct = _dbContext.Products.Take(10).ToList();
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Notfound()
        {
            return View();
        }
		public IActionResult CantAccess()
		{
			return View();
		}
		public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(LoginModel U)
		{
			var User = _dbContext.SystemUsers.Where(a => a.UserName == U.Username && a.Password == U.Password).FirstOrDefault();
			if (User == null)
			{
				ViewBag.Error = "Invalid user name or password";
				return View();
			}
			//Success Login
			HttpContext.Session.SetInt32("UserId", User.Id);
			HttpContext.Session.SetString("UserName", User.UserName);
			HttpContext.Session.SetInt32("UserType", User.Type ?? 0);

			if (User.Type == Constants.Type.Customer)
			{
				var customer = _dbContext.Customers.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
				if (customer != null && customer.Image != null)
				{
					HttpContext.Session.SetString("Pic", customer.Image);
					HttpContext.Session.SetString("FirstName", customer.FirstName);
					HttpContext.Session.SetString("LastName", customer.LastName);
					HttpContext.Session.SetInt32("Id", customer.Id);
				}
			}
			else if (User.Type == Constants.Type.Seller)
			{
				var seller = _dbContext.Sellers.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
				if (seller != null && seller.Image != null)
				{
					HttpContext.Session.SetString("Pic", seller.Image);
					HttpContext.Session.SetString("FirstName", seller.FirstName);
					HttpContext.Session.SetString("LastName", seller.LastName);
					HttpContext.Session.SetInt32("Id", seller.Id);
				}
			}
			else
			{
				var staff = _dbContext.staff.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
				if (staff != null && staff.Image != null)
				{
					HttpContext.Session.SetString("Pic", staff.Image);
					HttpContext.Session.SetString("FirstName", staff.FirstName);
					HttpContext.Session.SetString("LastName", staff.LastName);
					HttpContext.Session.SetInt32("Id", staff.Id);
				}
			}
			return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		#endregion


		#region Customer
		[HttpGet]
		public IActionResult SignupCustomer()
		{
			return View();
		}
		[HttpPost]
		public IActionResult SignupCustomer(SignupCustomer U)
		{
			SystemUser systemuser = new SystemUser();
			systemuser.UserName = U.Username;
			systemuser.Password = U.Password;
			systemuser.Status = Constants.Status.Active;
			systemuser.Type = 1;
			_dbContext.SystemUsers.Add(systemuser);
			_dbContext.SaveChanges();

			//to add image and its address
			//if (img != null)
			//{
			//	string FinalFilePathVirtual = "/data/student/pics/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

			//	using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
			//	{
			//		img.CopyTo(FS);
			//	}
			//	student.Images = FinalFilePathVirtual;
			//}
			Customer customer = new Customer();
			customer.SystemUserId = systemuser.Id;
			customer.FirstName = U.FirstName;
			customer.LastName = U.LastName;
			customer.Gender = U.Gender;
			customer.PhoneNumber = U.PhoneNumber;
			customer.City = U.City;
			customer.Address = U.Address;
			customer.Email = U.Email;
			customer.Dob = U.Dob;
			customer.Status = Constants.Status.Active;
			customer.CreatedDate = DateTime.Now;
			customer.Status = Constants.Status.Active;
			_dbContext.Customers.Add(customer);
			_dbContext.SaveChanges();

			ViewBag.Success = "Registered Successfully.";
			return RedirectToAction(nameof(Login));
		}
		#endregion


		#region Seller
		[HttpGet]
        public IActionResult SignupSeller()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignupSeller(SignupSeller U)
        {
			SystemUser systemuser = new SystemUser();
			systemuser.UserName = U.Username;
			systemuser.Password = U.Password;
            systemuser.Status = Constants.Status.Active;
			systemuser.Type = 2;
			_dbContext.SystemUsers.Add(systemuser);
			_dbContext.SaveChanges();

            //to add image and its address
            //if (img != null)
            //{
            //	string FinalFilePathVirtual = "/data/student/pics/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

            //	using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
            //	{
            //		img.CopyTo(FS);
            //	}
            //	student.Images = FinalFilePathVirtual;
            //}
            Seller seller = new Seller();
			seller.SystemUserId = systemuser.Id;
			seller.FirstName = U.FirstName;
			seller.LastName = U.LastName;
			seller.CompanyName = U.CompanyName;
			seller.ShortDescription = U.ShortDescription;
			seller.Gender = U.Gender;
			seller.PhoneNumber = U.PhoneNumber;
			seller.Cnic = U.Cnic;
			seller.City = U.City;
			seller.Address = U.Address;
			seller.Email = U.Email;
			seller.Dob = U.Dob;
            seller.Status = Constants.Status.Active;
            seller.CreatedDate = DateTime.Now;
            seller.Status = Constants.Status.Active;
            _dbContext.Sellers.Add(seller);
            _dbContext.SaveChanges();

            ViewBag.Success = "Registered Successfully.";
			return RedirectToAction(nameof(Login));
        }
		#endregion


		#region Product
		[HttpGet]
		public IActionResult AddProduct()
		{
            if (HttpContext.Session.GetInt32("UserType") != Helper.Constants.Type.Seller)
            {
                return RedirectToAction(nameof(CantAccess));
            }
            ViewBag.Category = _dbContext.Categories.ToList();
			return View();
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Product product, IList<IFormFile>? img)
		{
			//if (ModelState.IsValid)
			{
				//to add image and its address
				if (img != null && img.Count > 0)
				{
					string Final = "";
					foreach (var pics in img)
					{
						string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(pics.FileName);

						using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
						{
							pics.CopyTo(FS);
						}
						Final = Final + ',' + FinalFilePathVirtual;
					}
					if (Final.StartsWith(','))
					{
						Final = Final.Remove(0, 1);
					}
					product.Image = Final;
				}
				var cat = _dbContext.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault();
				product.TopCategoryId = cat.TopCategoryId;
				product.SellerId = HttpContext.Session.GetInt32("Id");
				product.NoOfView = 0;
				product.CreatedDate = DateTime.Now;
				product.CreatedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LastName");
				_dbContext.Products.Add(product);
				_dbContext.SaveChanges();
				TempData["message"] = "Product " + Constants.Message.AddMessage;
				return RedirectToAction(nameof(ProductList));
			}
			return View();
		}
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            var prod = _dbContext.Products.Find(id);
            if (prod != null)
            {
				ViewBag.Category = _dbContext.Categories.ToList();
				return View(prod);
            }
            return RedirectToAction(nameof(Notfound));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(Product product, IList<IFormFile>? img)
        {
            //if (ModelState.IsValid)
            {
                //to update image and its address
                if (img != null && img.Count > 0)
                {
                    string Final = "";
                    foreach (var pics in img)
                    {
                        string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(pics.FileName);

                        using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                        {
                            pics.CopyTo(FS);
                        }
                        Final = Final + ',' + FinalFilePathVirtual;
                    }
                    if (Final.StartsWith(','))
                    {
                        Final = Final.Remove(0, 1);
                    }
                    product.Image = Final;
                }
                var cat = _dbContext.Categories.Where(c => c.Id == product.CategoryId).FirstOrDefault();
                product.TopCategoryId = cat.TopCategoryId;
                product.ModifiedDate = DateTime.Now;
				product.ModifiedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LastName");
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
				TempData["message"] = "Product " + Constants.Message.UpdateMessage;
			    return RedirectToAction(nameof(ProductList));
            }
            return View();
        }
        [HttpGet]
		public IActionResult ProductList(int? id)
		{
			if(id != null)
			{
                var cat = _dbContext.Products.Where(p => p.TopCategoryId == id).ToList();
				var T = _dbContext.TopCategories.Where(c => c.Id == id).FirstOrDefault();
				ViewBag.Title = T.Name;
                return View(cat);
            }
			var list = _dbContext.Products.ToList();
			return View(list);
		}
        public IActionResult ProductDetail(int id)
        {
			var list = _dbContext.Products.Where(u => u.Id == id).FirstOrDefault();
            return View(list);
        }
        public IActionResult DeleteProduct(int id)
        {
			var list = _dbContext.Products.Find(id);
			if(list != null)
            {
				_dbContext.Products.Remove(list);
				_dbContext.SaveChanges();
				return RedirectToAction(nameof(ProductList));
			}
            return RedirectToAction(nameof(Notfound));
        }
        #endregion

        #region TopCategory
        [HttpGet]
		public IActionResult AddTopCategory()
		{
			return View();
		}
        [HttpPost]
		public IActionResult AddTopCategory(TopCategory topCategory, IFormFile? img)
		{
			if (img != null)
			{
				string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

				using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
				{
					img.CopyTo(FS);
				}
				topCategory.Image = FinalFilePathVirtual;
			}
			topCategory.CreatedDate = DateTime.Now;
			topCategory.CreatedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LasttName");
			_dbContext.TopCategories.Add(topCategory);
			_dbContext.SaveChanges();
			return RedirectToAction(nameof(TopCategoryList));
		}
		[HttpGet]
		public IActionResult UpdateTopCategory(int id)
		{
			var cat = _dbContext.TopCategories.Find(id);
			if(cat != null)
			{
				return View(cat);
			}
			
			return RedirectToAction(nameof(Notfound));
		}
		[HttpPost]
		public IActionResult UpdateTopCategory(TopCategory topCategory, IFormFile? img)
		{
			if (img != null)
			{
				string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

				using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
				{
					img.CopyTo(FS);
				}
				topCategory.Image = FinalFilePathVirtual;
			}
			topCategory.ModifiedDate	 = DateTime.Now;
			topCategory.ModifiedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LasttName");
			_dbContext.TopCategories.Update(topCategory);
			_dbContext.SaveChanges();
			return RedirectToAction(nameof(TopCategoryList));
		}
		public IActionResult TopCategoryList()
		{
			return View(_dbContext.TopCategories.ToList());
		}
		public IActionResult DeleteTopCategory(int id)
		{
			var top = _dbContext.TopCategories.Find(id);
			if (top != null)
			{
				_dbContext.TopCategories.Remove(top);
				_dbContext.SaveChanges();
				return View(nameof(ProductList));
			}
			return View(nameof(Notfound));
		}

		#endregion

		#region Category

		[HttpGet]
        public IActionResult AddCategory()
		{
            ViewBag.TopCategory = _dbContext.TopCategories.ToList();
			return View();
        }
		[HttpPost]
		public IActionResult AddCategory(Category category, IFormFile? img)
		{
			//to add image and its address
			if (img != null)
			{
				string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

				using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
				{
					img.CopyTo(FS);
				}
				category.Image = FinalFilePathVirtual;
			}
			category.CreatedDate = DateTime.Now;
			category.CreatedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LastName");
			_dbContext.Categories.Add(category);
			_dbContext.SaveChanges();
			TempData["message"] = "Category " + Constants.Message.AddMessage;
			return RedirectToAction(nameof(CategoryList));
		}
		[HttpGet]
		public IActionResult UpdateCategory(int id)
		{
			var cat = _dbContext.Categories.Find(id);
			if (cat != null)
			{
				ViewBag.TopCategory = _dbContext.TopCategories.ToList();
				return View(cat);
			}
			return RedirectToAction(nameof(Notfound));
		}
		[HttpPost]
        public IActionResult UpdateCategory(Category category, IFormFile? img)
        {
            //to add image and its address
            if (img != null)
            {
                string FinalFilePathVirtual = "/data/" + Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                using (FileStream FS = new FileStream(_he.WebRootPath + FinalFilePathVirtual, FileMode.Create))
                {
                    img.CopyTo(FS);
                }
                category.Image = FinalFilePathVirtual;
            }
            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = HttpContext.Session.GetString("FirstName") + HttpContext.Session.GetString("LastName");
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            TempData["message"] = "Categoty " + Constants.Message.UpdateMessage;
			return RedirectToAction(nameof(CategoryList));
        }
        [HttpGet]
        public IActionResult CategoryList(int? id)
        {
			var list = _dbContext.Categories.Where(c => c.TopCategoryId == id).ToList();
            if(id != null && list != null)
			{
				return View(list);
			}
			return View(_dbContext.Categories.ToList());
        }
		public IActionResult DeleteCategory(int id)
		{
			var cat = _dbContext.Categories.Find(id);
			if (cat != null)
			{
				_dbContext.Categories.Remove(cat);
				_dbContext.SaveChanges();
				return RedirectToAction(nameof(ProductList));
			}
			return RedirectToAction(nameof(Notfound));
        }
        #endregion
    }
}