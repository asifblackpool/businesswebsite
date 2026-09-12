
using Content.Modelling.Models.Interfaces;

namespace RazorPageBusinessWebsite.Core.Interfaces
{
    public interface IContentService
    {
        Task<List<IPageTemplates>> GetChildPagesAsync(string parentUri);
    }
}
