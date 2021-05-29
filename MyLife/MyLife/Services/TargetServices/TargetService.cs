using MyLife.Models;
using MyLife.Repositories;

namespace MyLife.Services.TargetServices
{
    public class TargetService : ITargetService
    {
        private IRepository<User> _userRepository;
        public TargetService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void InviteUser(User receiver, string senderId, string targetId)
        {
            var targetInvitation = receiver.TargetsInvitations.Find(t => t.SenderId.Equals(senderId) && t.TargetId.Equals(targetId));
            if (targetInvitation == null)
            {
                receiver.TargetsInvitations.Add(new TargetInvitation(senderId, targetId));
                _userRepository.Update(receiver);
            }
        }

        public void DeleteUser(User receiver, string senderId, string targetId)
        {
            var targetInvitation = receiver.TargetsInvitations.Find(t => t.SenderId.Equals(senderId) && t.TargetId.Equals(targetId));
            if (targetInvitation != null)
            {
                receiver.TargetsInvitations.Remove(targetInvitation);
                _userRepository.Update(receiver);
            }
        }
    }
}
