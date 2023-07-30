using AutoMapper;
using ShrtLy.BLL.Dtos;
using ShrtLy.DAL.Entities;

namespace ShrtLy.BLL.Profiles
{
    public sealed class Link : Profile
    {
        public Link()
        {
            CreateMap<LinkEntity, LinkDto>();
        }
    }
}
