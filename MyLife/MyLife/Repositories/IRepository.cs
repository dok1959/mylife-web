using System;
using System.Collections.Generic;

namespace MyLife.Repositories
{
    interface IRepository<T> where T : class
    {
        public T Get(int id);
        public ICollection<T> GetAll();
        public ICollection<T> Find(Func<T, bool> predicate);
        public void Add(T item);
        public void Update(T item);
        public void Remove(T item);
    }
}
