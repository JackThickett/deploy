using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace TestAppAuthAndAuthorize.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Roles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Roles");
        }
    }
}