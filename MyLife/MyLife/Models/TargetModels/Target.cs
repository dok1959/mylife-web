using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyLife.ViewModels;
using MyLife.ViewModels.TargetViewModels;
using System;
using System.Collections.Generic;

namespace MyLife.Models.TargetModels
{
    public class Target
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Members { get; set; }

        public List<Progress> Progress { get; set; }

        public Target()
        {
            Members = new List<string>();
        }

        public Target(TargetViewModel model, string id) : this()
        {
            Owner = id;
            Title = model.Title;
            Progress = new List<Progress>();
            foreach (var progress in model.Progress)
            {
                var p = new Progress
                {
                    Owner = id,
                    Value = progress.Value,
                    CheckBoxes = progress.CheckBoxes
                };
                if (progress.Date != null)
                {
                    var dateTime = DateTime.Parse(progress.Date);
                    if (dateTime != null)
                        p.Date = dateTime.Date;
                }
                Progress.Add(p);
            }
        }
        public Target(TargetCreationViewModel model, string id) : this()
        {
            Owner = id;
            Title = model.Title;
            Progress = new List<Progress>();
            foreach (var progress in model.Progress)
            {
                var p = new Progress
                {
                    Owner = id,
                    Value = progress.Value,
                    CheckBoxes = progress.CheckBoxes
                };
                if (progress.Date != null)
                {
                    var dateTime = DateTime.Parse(progress.Date);
                    if (dateTime != null)
                        p.Date = dateTime.Date;
                }    
                Progress.Add(p);
            }
        }
    }
}
