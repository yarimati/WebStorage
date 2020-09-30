using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStorage.Models;
using WebStorage.Models.DTOs;

namespace WebStorage.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public CabinetController(UserManager<AppUser> userManager, AppIdentityDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        /// <summary>
        /// Shows all links which belongs user
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var listOfLinks = _context.Links.Where(x => x.AppUser.Id == user.Id)
                                                .Select(x => new UsersEntryDto {Date = x.Date, Link = x.Link}).ToList();
            return View(listOfLinks);
        }

    }
}
