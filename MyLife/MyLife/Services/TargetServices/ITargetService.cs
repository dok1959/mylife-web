using MyLife.Models;
using MyLife.Models.TargetModels;

namespace MyLife.Services.TargetServices
{
    public interface ITargetService
    {
        void InviteUser(User receiver, string senderId, string targetId);
        void AcceptInvitation(User receiver, TargetInvitation targetInvitation, Target target);
        void DeclineInvitation(User receiver, TargetInvitation targetInvitation);
        void DeleteUser(User receiver, string senderId, Target target);
    }
}
