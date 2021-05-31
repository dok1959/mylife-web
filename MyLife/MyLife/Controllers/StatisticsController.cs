using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLife.Models.StatisticModels;
using MyLife.Models.TargetModels;
using MyLife.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IRepository<Target> _targetRepository;
        public StatisticsController(IRepository<Target> targetRepository)
        {
            _targetRepository = targetRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = HttpContext.User.FindFirst("id")?.Value;
            var userTargets = _targetRepository.Find(t => t.Owner.Equals(userId) || t.Members.Contains(userId)).ToList();
            Statistic statistic = new Statistic();
            int i = userTargets.Count();
            List<Progress> allUserProgress = new List<Progress>();
            userTargets.ForEach(t =>
            {
                allUserProgress.AddRange(t.Progress.FindAll(p => p.Owner.Equals(userId)));
            });

            if(allUserProgress.Count() == 0)
                return Ok(statistic);

            var grouppedTasksByDates = allUserProgress.Where(p => p.Date.HasValue).OrderBy(p => p.Date.Value).GroupBy(p => p.Date.Value).ToList();

            int allCompletedTasks = allUserProgress.Where(p => p.Value.CurrentValue >= p.Value.MaxValue).Count();
            float tasksCompletedFrequency = (allCompletedTasks / (float)allUserProgress.Count()) * 100;
            float AverageCompletedTasksInDay = allCompletedTasks / (float)grouppedTasksByDates.Count();
            int allCompletedDays = 0;
            int currentCompletedDaysInRow = 0;
            int bestCompletedDaysInRow = 0;

            foreach (var item in grouppedTasksByDates)
            {
                if (item.Count() == item.Where(t => t.Value.CurrentValue >= t.Value.MaxValue).Count())
                {
                    allCompletedDays++;
                    bestCompletedDaysInRow++;
                }
                else
                {
                    bestCompletedDaysInRow = 0;
                }
            }

            foreach(var item in grouppedTasksByDates.OrderByDescending(p => p.Key))
            {
                if (item.Count() == item.Where(t => t.Value.CurrentValue >= t.Value.MaxValue).Count())
                {
                    currentCompletedDaysInRow++;
                }
                else
                {
                    break;
                }
            }

            statistic.AllCompletedTasks = allCompletedTasks;
            statistic.TasksCompletedFrequency = (int)MathF.Round(tasksCompletedFrequency);
            statistic.AverageCompletedTasksInDay = AverageCompletedTasksInDay;
            statistic.AllCompletedDays = allCompletedDays;
            statistic.CurrentCompletedDaysInRow = currentCompletedDaysInRow;
            statistic.BestCompletedDaysInRow = bestCompletedDaysInRow;

            return Ok(statistic);
        }
    }
}
