using MyLife.Models.TargetModels;
using System.Collections.Generic;

namespace MyLife.ViewModels
{
    public class ProgressViewModel
    {
        public string Date { get; set; }
        public ProgressValue Value { get; set; }
        public List<SubProgress> CheckBoxes { get; set; }
    }
}
