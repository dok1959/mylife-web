using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FindOne(Expression<Func<T, bool>> predicate);
        T GetById(string id);
        void Add(T item);
        void Update(T item);
        void Remove(T item);
    }
}
