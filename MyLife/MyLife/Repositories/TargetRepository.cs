using MyLife.Models.TargetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyLife.Repositories
{
    public class TargetRepository : IRepository<Target>
    {
        public void Add(Target item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Target> Find(Expression<Func<Target, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Target FindOne(Expression<Func<Target, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Target> GetAll()
        {
            throw new NotImplementedException();
        }

        public Target GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Target item)
        {
            throw new NotImplementedException();
        }

        public void Update(Target item)
        {
            throw new NotImplementedException();
        }
    }
}
