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
    public sealed class LinkService : ILinkService
    {
        private readonly ILinksRepository _repository;
        private readonly IMapper _mapper;
        private readonly IShorteningService _shorteningService;

        public LinkService(ILinksRepository repository, IMapper mapper, IShorteningService shorteningService)
        {
            _repository = repository;
            _mapper = mapper;
            _shorteningService = shorteningService;
        }

        public async Task<string> ProcessLinkAsync(string url)
        {
            var entity = await _repository.GetLinkAsync(url);
            if (entity == null)
            {
                
                var link = _shorteningService.ShortLink(url);
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
        public async Task<LinkDto> GetLinkAsync(string url)
        {
            var entity = await _repository.GetLinkAsync(url);
            return _mapper.Map<LinkDto>(entity);
        }

        public async Task<LinkDto> GetByShortNameAsync(string url)
        {
            var entity = await _repository.GetByShortNameAsync(url);
            return _mapper.Map<LinkDto>(entity);
        }
    }
}
