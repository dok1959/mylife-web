using MyLife.Models;
using System;
using System.Collections.Generic;

namespace MyLife.ViewModels
{
    public class DesireViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public bool IsPrivate { get; set; }

        public List<SubDesireViewModel> SubDesires { get; set; }
    }
}
