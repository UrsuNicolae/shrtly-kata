using System.Collections.Generic;
using System.Threading.Tasks;
using ShrtLy.DAL.Entities;

namespace ShrtLy.DAL.Repositories.Interfaces
{
    public interface ILinksRepository
    {
        Task<int> CreateLinkAsync(LinkEntity entity);
        Task<IEnumerable<LinkEntity>> GetAllLinksAsync();

        Task<LinkEntity> GetLinkAsync(string url);

        Task<LinkEntity> GetByShortNameAsync(string url);
    }
}