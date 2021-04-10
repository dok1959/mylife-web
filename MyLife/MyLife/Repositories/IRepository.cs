using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    interface IRepository<T, I> where T : class
    {
        public T Get(I id);
        public ICollection<T> GetAll();
        public ICollection<T> Find(Expression<Func<T, bool>> predicate);
        public void Add(T item);
        public void Update(T item);
        public void Remove(T item);
    }
}
