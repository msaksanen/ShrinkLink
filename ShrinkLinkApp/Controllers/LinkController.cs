using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShrinkLinkApp.Helpers;
using ShrinkLinkApp.Models;

namespace ShrinkLinkApp.Controllers
{
    public class LinkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly URLChecker _urlChecker;

        public LinkController(IMapper mapper, URLChecker urlChecker)
        {
            _mapper = mapper;
            _urlChecker = urlChecker;
        }

        [HttpPost]
        public async Task<IActionResult> Shorten (LinkInputModel model)
        {
            ShortenModel respone = new();
            if (model==null || String.IsNullOrEmpty(model?.URL)) 
            {
                respone.SystemFlag = 0;
                respone.SystemInfo = "Input any weblink";
                return View(respone);
            }
            if (!_urlChecker.IsValidURL(model.URL))
            {
                respone.SystemFlag = 0;
                respone.SystemInfo = "Invalid URL";
                return View(respone);
            }
            return View();
        }
    }
}
