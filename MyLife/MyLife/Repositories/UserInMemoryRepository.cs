using MyLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    public class UserInMemoryRepository : IRepository<User>
    {
        private List<User> _users;
        private int idCounter = 0;

        public UserInMemoryRepository()
        {
            _users = new List<User>();
        }

        public void Add(User item)
        {
            idCounter++;
            item.Id = idCounter.ToString();
            item.Username = item.Login;
            _users.Add(item);
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return Find(predicate.Compile());
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return _users.Where(predicate);
        }

        public User FindOne(Expression<Func<User, bool>> predicate)
        {
            return FindOne(predicate.Compile());
        }

        public User FindOne(Func<User, bool> predicate)
        {
            return _users.Where(predicate).SingleOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(string id)
        {
            return _users.Where(u => u.Id.Equals(id)).SingleOrDefault();
        }

        public void Remove(User item)
        {
            _users.Remove(item);
        }

        public void Update(User item)
        {
            var user = _users.Where(u => u.Id.Equals(item.Id)).SingleOrDefault();
            if (user != null)
            {
                _users.Remove(user);
                _users.Add(item);
            }
        }
    }
}
