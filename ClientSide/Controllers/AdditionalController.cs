using ClientSide.Contract;
using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Additional;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientSide.Controllers
{
    [Authorize]
    [Controller]
    public class AdditionalController : Controller
    {
        private readonly IAdditionalRepository _additionalRepository;

        public AdditionalController(IAdditionalRepository additionalRepository)
        {
            _additionalRepository = additionalRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid guid)
        {
            var results = await _additionalRepository.GetAdditional(guid);
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            var Additionals = results.Data.Select(entity => new AdditionalVM
            {
                Guid = entity.Guid,
                ProgressGuid = entity.ProgressGuid,
                FileName = entity.FileName,
                FileData = entity.FileData,
            }).ToList();
            ViewBag.Components = components;
            ViewBag.Guid = guid;
            return View("Index", Additionals);
        }

        [HttpGet]
        public IActionResult AddAdditional(Guid guid)
        {
            var additional = new CreateAdditionalVM
            {
                ProgressGuid = guid,
            };

            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            ViewBag.Components = components;
            return View("Create", additional);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdditional([FromForm] CreateAdditionalVM createAdditionalVM)
        {
            var additional = await _additionalRepository.PostAdditional(createAdditionalVM);

            return RedirectToAction("Index", new { guid = createAdditionalVM.ProgressGuid });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await _additionalRepository.DeleteAdditional(guid);
            if (result.Code == 404)
            {
                return Json(new { code = 404, message = "Additional not found" });
            }
            else if (result.Code == 200)
            {
                return Json(new { code = 200, message = "Additional deleted successfully" });
            }
            else
            {
                return Json(new { code = result.Code, message = result.Message });
            }
        }
    }
}
