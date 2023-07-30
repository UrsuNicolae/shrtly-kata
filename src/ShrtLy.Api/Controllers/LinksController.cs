using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShrtLy.Api.ViewModels;
using ShrtLy.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShrtLy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly ILinkService _service;
        private readonly IMapper _mapper;

        public LinksController(ILinkService service, IMapper mapper)
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

        [HttpGet("{url}")]
        public async Task<IActionResult> RedirectAsync(string url)
        {
            var link = await _service.GetByShortNameAsync(url);
            if(link == null)
            {
                return NotFound();
            }

            return Redirect(link.Url);
        }
    }
}
