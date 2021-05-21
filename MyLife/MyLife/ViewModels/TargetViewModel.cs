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
