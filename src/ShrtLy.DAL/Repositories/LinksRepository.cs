using Microsoft.EntityFrameworkCore;
using ShrtLy.DAL.Entities;
using ShrtLy.DAL.Repositories.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShrtLy.DAL
{
    public sealed class LinksRepository : ILinksRepository
    {
        private readonly ShrtLyContext _context;

        public LinksRepository(ShrtLyContext context)
        {
            _context = context;
        }

        public async Task<int> CreateLinkAsync(LinkEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<LinkEntity>> GetAllLinksAsync()
        {
            return await _context.Links.ToListAsync();
        }

        public async Task<LinkEntity> GetByShortNameAsync(string url)
        {
            return await _context.Links.FirstOrDefaultAsync(l => l.ShortUrl == url);
        }

        public async Task<LinkEntity> GetLinkAsync(string url)
        {
            return await _context.Links.FirstOrDefaultAsync(l => l.Url == url);
        }
    }
}
