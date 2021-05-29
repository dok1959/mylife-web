using MyLife.Models;

namespace MyLife.Services.TargetServices
{
    public interface ITargetService
    {
        void InviteUser(User receiver, string senderId, string targetId);
        void DeleteUser(User receiver, string senderId, string targetId);
    }
}
