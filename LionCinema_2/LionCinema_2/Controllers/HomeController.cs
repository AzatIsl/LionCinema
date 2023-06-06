using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LionCinema_2.Models;
using System.Diagnostics;
using System.Security.Claims;


namespace LionCinema.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Authorization()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authorization(string login, string password, string role)
        {
            if (role == "Модератор")
            {
                using (LionCinemaContext db = new LionCinemaContext())
                {
                    Employee? employee = (from o in db.Employees where o.EmployeeLog == login && o.EmployeePas == password select o).FirstOrDefault();
                    if (employee is not null)
                    {
                        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()), new Claim(ClaimsIdentity.DefaultRoleClaimType, "Employee") };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("PrivateAccountEmp");
                    }
                    else
                    {
                        ViewBag.Enter = "Неверные данные для входа";
                        return View();
                    }
                }
            }
            else if (role == "Пользователь")
            {
                using (LionCinemaContext db = new LionCinemaContext())
                {
                    User client = (from c in db.Users where (c.UserLog == login) && c.UserPas == password select c).FirstOrDefault();
                    //HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (client is not null)
                    {

                        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, client.UserId.ToString()),
                        new Claim(ClaimTypes.Email, "testc@mail.ru"), new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")};
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Main");
                    }
                    else
                    {
                        ViewBag.Enter = "Неверные данные для входа";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Enter = "Выберите роль";
                return View();
            }
        }
        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Registration(string login, string password)
        {
            using (LionCinemaContext db = new LionCinemaContext())
            {
                int countClients = (from c in db.Users where c.UserLog == login && c.UserPas == password select c).Count();
                if (countClients > 0)
                {
                    ViewBag.Enter = "Пользователь с данным логином уже зарегистрирован";
                    return View();
                }
                else
                {
                    User user = new User();
                    user.UserLog = login;
                    user.UserPas = password;
                    user.UserName = "name";
                    db.Add(user);
                    db.SaveChanges();
                    return View("Authorization");
                }
            }
        }
        [Authorize(Roles = "User")]
        public IActionResult Main()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult Catalog()
        {
            return View();
        }
        //public IActionResult ShowMovie(int id)
        //{
        //    using (LionCinemaContext db = new LionCinemaContext())
        //    {
        //        var movie = db.Movies.FirstOrDefault(m => m.MovieId == id);
        //        if (movie != null)
        //        {
        //            byte[] imageBytes = movie.MovieImg.ToArray();
        //            return File(imageBytes, "image/jpeg");
        //        }
        //    }
        //    return NotFound();
        //}
        public RedirectToActionResult LeaveAccount()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authorization");
        }
        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult PrivateAccount()
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(user.Value);
            using (LionCinemaContext db = new LionCinemaContext())
            {
                User currentUser = db.Users.Where(e => e.UserId == id).FirstOrDefault();
                return View("PrivateAccount", currentUser);
            }

        }
        public RedirectToActionResult SaveChangesUser(string login, string password, string name)
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(claim.Value);
            using (LionCinemaContext db = new LionCinemaContext())
            {
                User currentUser = db.Users.Where(e => e.UserId == id).FirstOrDefault();
                currentUser.UserLog = login;
                currentUser.UserPas = password;
                currentUser.UserName = name;
                db.SaveChanges();
            }

            ViewBag.Message = string.Format("Данные были сохранены");
            return RedirectToAction("PrivateAccount");
        }


        [Authorize(Roles = "Employee")]
        public IActionResult PrivateAccountEmp()
        {
            var moder = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(moder.Value);
            using (LionCinemaContext db = new LionCinemaContext())
            {
                Employee currentModerator = db.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
                return View("PrivateAccountEmp", currentModerator);
            }

        }
        public RedirectToActionResult SaveChangesEmp(string login, string password)
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int id = Convert.ToInt32(claim.Value);
            using (LionCinemaContext db = new LionCinemaContext())
            {
                Employee currentEmp = db.Employees.Where(e => e.EmployeeId == id).FirstOrDefault();
                currentEmp.EmployeeLog = login;
                currentEmp.EmployeePas = password;
                db.SaveChanges();
            }

            ViewBag.Message = string.Format("Данные были сохранены");
            return RedirectToAction("PrivateAccountModer");
        }

        [Authorize(Roles = "Employee")]
        public IActionResult AddingMovie()
        {
            return View();
        }
        public RedirectToActionResult AddMovie( string moviename, string moviepath, IFormFile movieimg)
        {
			MemoryStream ms = new MemoryStream();
            movieimg.CopyTo(ms);
            using (LionCinemaContext db = new LionCinemaContext())
            {
                Movie movie = new Movie();
                movie.MovieName = moviename;
                movie.MoviePath = moviepath;
                movie.MovieImg = ms.ToArray();
                db.Add(movie);
                db.SaveChanges();
            }
            return RedirectToAction("AddingMovie");
        }
        [Authorize(Roles = "Employee")]
        public IActionResult CatalogEmp()
        {
            return View();
        }
        public IActionResult ChangeMovie(int id)
        {
            using (LionCinemaContext db = new LionCinemaContext())
            {
            var movieDetail = db.Movies.Find(id);
            return View(movieDetail);
            }
        }
		public RedirectToActionResult SaveChangeMovie(int id, string moviename, string moviepath, IFormFile movieimg)
		{
			MemoryStream ms = new MemoryStream();
			movieimg.CopyTo(ms);
			using (LionCinemaContext db = new LionCinemaContext())
			{
				var movie = db.Movies.FirstOrDefault(m => m.MovieId == id);
				if (movie != null)
                {
					movie.MovieName = moviename;
					movie.MoviePath = moviepath;
					movie.MovieImg = ms.ToArray();
					db.SaveChanges();
					return RedirectToAction("CatalogEmp");
				}
			}
			return RedirectToAction("CatalogEmp");
		}
		public RedirectToActionResult DeleteMovie(int id)
		{
			using (LionCinemaContext db = new LionCinemaContext())
			{
				var movie = db.Movies.FirstOrDefault(m => m.MovieId == id);
				if (movie != null)
				{
					db.Movies.Remove(movie);
					db.SaveChanges();
					return RedirectToAction("CatalogEmp");
				}
			}
			return RedirectToAction("CatalogEmp");
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