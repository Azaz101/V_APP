using V_APP.Data;
using V_APP.DTOs;
using V_APP.Helper;
using V_APP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Pipes;

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
            HttpContext.Session.SetInt32("Id", User.Id);
            HttpContext.Session.SetString("UserName", User.UserName);
            HttpContext.Session.SetInt32("Type", User.Type ?? 0);

            if (User.Type == Constants.Type.Customer)
            {
                var customer = _dbContext.Customers.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
                if (customer != null && customer.Image != null)
                {
                    HttpContext.Session.SetString("Userpic", customer.Image);
                    HttpContext.Session.SetString("FirstName", customer.FirstName);
                    HttpContext.Session.SetString("LastName", customer.LastName);
                    HttpContext.Session.SetInt32("CustomerId", customer.Id);
                }
            }
            else if (User.Type == Constants.Type.Seller)
            {
                var seller = _dbContext.Sellers.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
                if (seller != null && seller.Image != null)
                {
                    HttpContext.Session.SetString("Userpic", seller.Image);
                    HttpContext.Session.SetString("FirstName", seller.FirstName);
                    HttpContext.Session.SetString("LastName", seller.LastName);
                    HttpContext.Session.SetInt32("SellerId", seller.Id);
                }
            }
            else
            {
                var staff = _dbContext.staff.Where(u => u.SystemUserId == User.Id).FirstOrDefault();
                if (staff != null && staff.Image != null)
                {
                    HttpContext.Session.SetString("Userpic", staff.Image);
                    HttpContext.Session.SetString("FirstName", staff.FirstName);
                    HttpContext.Session.SetString("LastName", staff.LastName);
                    HttpContext.Session.SetInt32("StaffId", staff.Id);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Index()
        
        {
            return View();
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
    }
}