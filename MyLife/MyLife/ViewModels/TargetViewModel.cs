using MyLife.Models.TargetModels;
using System.Collections.Generic;

namespace MyLife.ViewModels
{
    public class TargetViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<ProgressViewModel> Progress { get; set; }

        public TargetViewModel()
        {

        }
        public TargetViewModel(Target target)
        {
            Id = target.Id;
            Title = target.Title;
            Progress = new List<ProgressViewModel>();
            foreach (var progress in target.Progress)
            {
                var p = new ProgressViewModel
                {
                    Value = progress.Value,
                    CheckBoxes = progress.CheckBoxes
                };
                if (progress.Date.HasValue)
                    p.Date = progress.Date.Value.ToString();
                else
                    p.Date = null;
                Progress.Add(p);
            }
        }
    }
}
