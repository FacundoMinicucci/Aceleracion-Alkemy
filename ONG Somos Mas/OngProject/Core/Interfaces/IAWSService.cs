using Microsoft.AspNetCore.Http;
using OngProject.Core.Helper;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IAWSService
    {
        Task<AWSManagerResponse> UploadImage(IFormFile file);

        Task<AWSManagerResponse> DeleteImage(string urlKey);
    }
}
