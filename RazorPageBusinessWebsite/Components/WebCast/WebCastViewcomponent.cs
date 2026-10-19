using Microsoft.AspNetCore.Mvc;
using RazorPageBusinessWebsite.Components.Extensions;

namespace RazorPageBusinessWebsite.Components.Webcast
{
    public class WebCastViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string? introOverride)
        {
            return View(ViewComponentExtensions.GetViewPath("WebCast"), introOverride);
        }
    }
}