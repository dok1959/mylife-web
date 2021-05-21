using MyLife.Models.TargetModels;
using System.Collections.Generic;

namespace MyLife.ViewModels
{
    public class TargetViewModel
    {
        public string Title { get; set; }
        public List<ProgressViewModel> Progress { get; set; }

        public TargetViewModel()
        {

        }
        public TargetViewModel(Target target)
        {
            Title = target.Title;
            Progress = new List<ProgressViewModel>();
            foreach (var progress in target.Progress)
            {
                Progress.Add(new ProgressViewModel
                {
                    Date = progress.Date.ToString(),
                    Value = progress.Value,
                    CheckBoxes = progress.CheckBoxes
                });
            }
        }
    }
}
