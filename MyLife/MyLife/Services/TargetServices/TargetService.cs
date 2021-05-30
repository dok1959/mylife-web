using MyLife.Models;
using MyLife.Models.TargetModels;
using MyLife.Repositories;

namespace MyLife.Services.TargetServices
{
    public class TargetService : ITargetService
    {
        private IRepository<User> _userRepository;
        private IRepository<Target> _targetRepository;
        public TargetService(IRepository<User> userRepository, IRepository<Target> targetRepository)
        {
            _userRepository = userRepository;
            _targetRepository = targetRepository;
        }

        public void InviteUser(User receiver, string senderId, string targetId)
        {
            AddInvitationToUser(receiver, senderId, targetId);
        }

        public void DeleteUser(User receiver, string senderId, Target target)
        {
            RemoveUserFromTarget(receiver.Id, target);
        }

        public void AcceptInvitation(User receiver, TargetInvitation targetInvitation, Target target)
        {
            RemoveInvitationFromUser(receiver, targetInvitation);
            AddUserToTarget(receiver.Id, target);
        }

        public void DeclineInvitation(User receiver, TargetInvitation targetInvitation)
        {
            RemoveInvitationFromUser(receiver, targetInvitation);
        }

        #region Invitations
        private void AddInvitationToUser(User receiver, string senderId, string targetId)
        {
            receiver.TargetsInvitations.Add(new TargetInvitation(senderId, targetId));
            _userRepository.Update(receiver);
        }

        private void RemoveInvitationFromUser(User receiver, TargetInvitation targetInvitation)
        {
            receiver.TargetsInvitations.Remove(targetInvitation);
            _userRepository.Update(receiver);
        }
        #endregion

        #region Targets
        private void AddUserToTarget(string receiverId, Target target)
        {
            target.Members.Add(receiverId);
            _targetRepository.Update(target);
        }

        private void RemoveUserFromTarget(string receiverId, Target target)
        {
            target.Members.Remove(receiverId);
            _targetRepository.Update(target);
        }
        #endregion
    }
}
