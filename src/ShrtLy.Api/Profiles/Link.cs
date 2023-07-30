using AutoMapper;
using ShrtLy.Api.ViewModels;
using ShrtLy.BLL.Dtos;

namespace ShrtLy.Api.Profiles
{
    public sealed class Link : Profile
    {
        public Link()
        {
            CreateMap<LinkDto, LinkViewModel>();   
        }
    }
}
