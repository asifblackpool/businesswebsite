using Microsoft.AspNetCore.Mvc;
using RazorPageBusinessWebsite.Constants;
using RazorPageBusinessWebsite.Controllers.Base;
using RazorPageBusinessWebsite.Services.Interfaces;

namespace RazorPageBusinessWebsite.Controllers
{
    public class BusinessController : DynamicCmsController
    {
        public BusinessController(IZengentiClient cmsClient, ICmsViewModelFactory viewModelFactory, ILogger<BusinessController> logger)
          : base(cmsClient, viewModelFactory, logger) { }

        public async Task<IActionResult> Dynamic(string slug)
        {
            slug ??= "";
            if (string.IsNullOrEmpty(slug))
            {
                var node = await _cmsClient.GetNodeByPathAsync("business");
                _logger.LogWarning(
                    "Root diagnostic — Path: {Path}, EntryId: {EntryId}, Title: {Title}, ContentType: {ContentType}, HasContent: {HasContent}",
                    node?.Path,
                    node?.EntryId,
                    node?.Title,
                    node?.ContentType,
                    !string.IsNullOrEmpty(node?.EntryJson));
            }
            return await RenderDynamicPageAsync(WebsiteConstants.SITE_PATH, slug);
        }
    }
}
