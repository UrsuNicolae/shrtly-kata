using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShrtLy.Api.ViewModels;
using ShrtLy.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShrtLy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly IShorteningService _service;
        private readonly IMapper _mapper;

        public LinksController(IShorteningService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public Task<string> GetShortLinkAsync(string url)
        {
            return _service.ProcessLinkAsync(url);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<LinkViewModel>> GetShortLinksAsync()
        {
            var dtos = await _service.GetShortLinksAsync();
            return _mapper.Map<IEnumerable<LinkViewModel>>(dtos);
        }
    }
}
