using MongoDB.Driver;
using MyLife.Models.TargetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    public class TargetInMemoryRepository : IRepository<Target>
    {
        private List<Target> _targets;
        private int idCounter = 0;

        public TargetInMemoryRepository()
        {
            _targets = new List<Target>();
        }

        public void Add(Target item)
        {
            idCounter++;
            item.Id = idCounter.ToString();
            _targets.Add(item);
        }

        public IEnumerable<Target> Find(Expression<Func<Target, bool>> predicate)
        {
            return Find(predicate.Compile());
        }

        public IEnumerable<Target> Find(Func<Target, bool> predicate)
        {
            return _targets.Where(predicate);
        }

        public Target FindOne(Expression<Func<Target, bool>> predicate)
        {
            return FindOne(predicate.Compile());
        }

        public Target FindOne(Func<Target, bool> predicate)
        {
            return _targets.Where(predicate).SingleOrDefault();
        }

        public IEnumerable<Target> GetAll()
        {
            return _targets;
        }

        public Target GetById(string id)
        {
            return _targets.Where(u => u.Id.Equals(id)).SingleOrDefault();
        }

        public void Remove(Target item)
        {
            _targets.Remove(item);
        }

        public void Update(Target item)
        {
            var user = _targets.Where(u => u.Id.Equals(item.Id)).SingleOrDefault();
            if (user != null)
            {
                _targets.Remove(user);
                _targets.Add(item);
            }
        }
    }
}
