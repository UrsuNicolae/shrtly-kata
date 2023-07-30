using AutoMapper;
using ShrtLy.BLL.Dtos;
using ShrtLy.BLL.Services.Interfaces;
using ShrtLy.DAL.Entities;
using ShrtLy.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShrtLy.BLL.Services
{
    public sealed class ShorteningService : IShorteningService
    {
        private readonly ILinksRepository _repository;
        private readonly IMapper _mapper;

        public ShorteningService(ILinksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> ProcessLinkAsync(string url)
        {
            var entity = await _repository.GetLinkByShortNameAsync(url);
            if (entity == null)
            {
                Thread.Sleep(1);//make everything unique while looping
                long ticks = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;//EPOCH
                char[] baseChars = new char[] { '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x'};

                int i = 32;
                char[] buffer = new char[i];
                int targetBase = baseChars.Length;

                do
                {
                    buffer[--i] = baseChars[ticks % targetBase];
                    ticks = ticks / targetBase;
                }
                while (ticks > 0);

                char[] result = new char[32 - i];
                Array.Copy(buffer, i, result, 0, 32 - i);

                var shortUrl = new string(result);

                var link = new LinkEntity
                {
                    ShortUrl = shortUrl,
                    Url = url
                };

                await _repository.CreateLinkAsync(link);

                return link.ShortUrl;
            }
            else
            {
                return entity.ShortUrl;
            }
        }

        public async Task<IEnumerable<LinkDto>> GetShortLinksAsync()
        {
            var entities = await _repository.GetAllLinksAsync();
            return _mapper.Map<IEnumerable<LinkDto>>(entities);
        }
    }
}
