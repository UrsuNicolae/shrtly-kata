using System.Collections.Generic;
using System.Threading.Tasks;
using ShrtLy.BLL.Dtos;

namespace ShrtLy.BLL.Services.Interfaces
{
    public interface ILinkService
    {
        Task<IEnumerable<LinkDto>> GetShortLinksAsync();
        Task<string> ProcessLinkAsync(string url);
        Task<LinkDto> GetLinkAsync(string url);
    }
}