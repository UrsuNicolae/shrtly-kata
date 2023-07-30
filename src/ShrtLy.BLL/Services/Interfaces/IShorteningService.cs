using System.Collections.Generic;
using System.Threading.Tasks;
using ShrtLy.BLL.Dtos;

namespace ShrtLy.BLL.Services.Interfaces
{
    public interface IShorteningService
    {
        Task<IEnumerable<LinkDto>> GetShortLinksAsync();
        Task<string> ProcessLinkAsync(string url);
    }
}