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
		public IActionResult SignupSeller(SignupCustomer U)
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
			customer.PhoneNumber = U.PhoneNo;
			//customer.Cnic = U.Cnic;
			customer.Address = U.Address;
			customer.Email = U.Email;
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
			systemuser.Type = U.Type;
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
			seller.Gender = U.Gender;
			seller.PhoneNumber = U.PhoneNo;
			seller.Cnic = U.Cnic;
			seller.Address = U.Address;
			seller.Email = U.Email;
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

            //if (User.Role == Constants.Role.Customer)
            //{
            //    var supervisor = _dbContext.customer.Where(u => u.SystemuserId == User.Id).FirstOrDefault();
            //    if (supervisor != null && supervisor.Images != null)
            //    {
            //        HttpContext.Session.SetString("Userpic", supervisor.Images);
            //        HttpContext.Session.SetString("Name", supervisor.Name);
            //        HttpContext.Session.SetInt32("SuperId", supervisor.Id);
            //    }
            //}
            //else if (User.Role == Constants.Role.Seller)
            //{
            //    var stu = _dbContext.seller.Where(u => u.SystemuserId == User.Id).FirstOrDefault();
            //    if (stu != null && stu.Images != null)
            //    {
            //        HttpContext.Session.SetString("Userpic", stu.Images);
            //        HttpContext.Session.SetString("Name", stu.Name);
            //        HttpContext.Session.SetInt32("StuId", stu.Id);
            //    }
            //}
            //else
            //{
            //    var admin = _dbContext.admins.Where(u => u.SystemuserId == User.Id).FirstOrDefault();
            //    if (admin != null && admin.Images != null)
            //    {
            //        HttpContext.Session.SetString("Userpic", admin.Images);
            //        HttpContext.Session.SetString("Name", admin.Name);
            //        HttpContext.Session.SetInt32("AdminId", admin.Id);
            //    }
            //}
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