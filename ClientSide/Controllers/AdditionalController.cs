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
        public ActionResult Create(Guid guid)
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
    }
}
