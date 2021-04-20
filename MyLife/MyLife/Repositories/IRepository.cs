using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    public interface IRepository<T> where T : class
    {
        public T Get(string id);
        public ICollection<T> GetAll();
        public ICollection<T> Find(Expression<Func<T, bool>> predicate);
        public void Add(T item);
        public void Update(T item);
        public void Remove(T item);
    }
}
