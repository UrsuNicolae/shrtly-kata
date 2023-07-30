using ShrtLy.DAL.Entities;

namespace ShrtLy.BLL.Services.Interfaces
{
    public interface IShorteningService
    {
        LinkEntity ShortLink(string url);
    }
}
