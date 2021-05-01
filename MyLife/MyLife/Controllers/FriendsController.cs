using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.ViewModels;
using System.Collections.Generic;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private IRepository<User> _usersRepository;
        private IMapper _mapper;

        public FriendsController(IRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var availableFriends = _usersRepository.GetById(userId)?.Friends.Available;
            
            if(availableFriends.Count == 0)
            {
                return BadRequest(new { errorMessage = "User doesn't have friends"});
            }

            var friends = new List<ProfileViewModel>();
            foreach(var friend in availableFriends)
            {
                var user = _usersRepository.GetById(friend);
                if (user == null)
                    continue;
                friends.Add(_mapper.Map<ProfileViewModel>(user));
            }

            return Ok(friends);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var availableFriends = _usersRepository.GetById(userId)?.Friends?.Available;

            if (availableFriends == null)
            {
                return BadRequest(new { errorMessage = "User doesn't have friends" });
            }

            var availableFriend = availableFriends.Find(friend => friend.Equals(id));

            var user = _usersRepository.GetById(availableFriend);

            return Ok(_mapper.Map<ProfileViewModel>(user));
        }

        [Authorize]
        [HttpGet("sent")]
        public IActionResult GetSentInvitations()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var sentInvitations = _usersRepository.GetById(userId)?.Friends.Sent;

            if (sentInvitations.Count == 0)
            {
                return BadRequest(new { errorMessage = "User doesn't have invitations" });
            }

            var sentInvitationsViewModels = new List<ProfileViewModel>();
            foreach (var friend in sentInvitations)
            {
                var repoSentFriend = _usersRepository.GetById(friend);
                if (repoSentFriend == null)
                    continue;
                sentInvitationsViewModels.Add(_mapper.Map<ProfileViewModel>(repoSentFriend));
            }

            return Ok(sentInvitationsViewModels);
        }

        [Authorize]
        [HttpPost("sent")]
        public IActionResult SendInvitation([FromForm] string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;

            if (userId.Equals(id))
            {
                return BadRequest(new { errorMessage = "You can't invite yourself" });
            }

            var user = _usersRepository.GetById(userId);
            var friend = _usersRepository.GetById(id);

            if(friend == null)
            {
                return BadRequest(new { errorMessage = "User doesn't exist" });
            }

            if(user.Friends.Available.Contains(id))
            {
                return BadRequest(new { errorMessage = "User already your friend" });
            }

            if(user.Friends.Sent.Contains(id))
            {
                return BadRequest(new { errorMessage = "You already sent invitation this user" });
            }

            if(user.Friends.Received.Contains(id))
            {
                return BadRequest(new { errorMessage = "You have already received an invitation from this user" });
            }

            user.Friends.Sent.Add(id);
            friend.Friends.Received.Add(userId);

            _usersRepository.Update(user);
            _usersRepository.Update(friend);

            return Ok();
        }

        [Authorize]
        [HttpGet("received")]
        public IActionResult GetReceivedInvitations()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var receivedInvitations = _usersRepository.GetById(userId)?.Friends.Received;

            if (receivedInvitations.Count == 0)
            {
                return BadRequest(new { errorMessage = "User doesn't have invitations" });
            }

            var receivedInvitationsViewModels = new List<ProfileViewModel>();
            foreach (var friend in receivedInvitations)
            {
                var repoReceivedFriend = _usersRepository.GetById(friend);
                if (repoReceivedFriend == null)
                    continue;
                receivedInvitationsViewModels.Add(_mapper.Map<ProfileViewModel>(repoReceivedFriend));
            }

            return Ok(receivedInvitationsViewModels);
        }

        [Authorize]
        [HttpPost("received")]
        public IActionResult AcceptInvitation([FromForm] string id)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var user = _usersRepository.GetById(userId);
            var friend = _usersRepository.GetById(id);

            if (userId.Equals(id))
            {
                return BadRequest(new { errorMessage = "You can't invite yourself" });
            }

            if (user.Friends.Available.Contains(id))
            {
                return BadRequest(new { errorMessage = "User already your friend" });
            }

            if(!user.Friends.Received.Contains(id))
            {
                return BadRequest(new { errorMessage = "User doesn't sent invite" });
            }

            user.Friends.Received.Remove(id);
            friend.Friends.Sent.Remove(userId);

            user.Friends.Available.Add(id);
            friend.Friends.Available.Add(userId);

            _usersRepository.Update(user);
            _usersRepository.Update(friend);

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromBody] string id)
        {
            return Ok();
        }

        [Authorize]
        [HttpGet("find")]
        public IActionResult FindAll()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var users = _usersRepository.Find(u => !u.Id.Equals(userId));
            return Ok(_mapper.Map<IEnumerable<ProfileViewModel>>(users));
        }

        [Authorize]
        [HttpGet("find/{username}")]
        public IActionResult FindByUsername(string username)
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var users = _usersRepository.Find(u => u.Username == username && !u.Id.Equals(userId));
            return Ok(_mapper.Map<IEnumerable<ProfileViewModel>>(users));
        }
    }
}
