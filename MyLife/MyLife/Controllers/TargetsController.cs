using Microsoft.AspNetCore.Mvc;
using MyLife.Models.TargetModels;
using MyLife.Repositories;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetsController : ControllerBase
    {
        private IRepository<Target> _targetRepository;
        public TargetsController(IRepository<Target> targetRepository)
        {
            _targetRepository = targetRepository;
        }

        public IActionResult Get()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var userTargets = _targetRepository.Find(t => t.Owner.Equals(userId) || (t.Members != null && t.Members.Contains(userId)));
            foreach(var target in userTargets)
            {
                target.Progress.RemoveAll(p => p.Owner != userId);
            }
            return Ok(userTargets);
        }

    }
}
