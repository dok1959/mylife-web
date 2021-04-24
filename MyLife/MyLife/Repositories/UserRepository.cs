using MyLife.Data;
using MyLife.Models;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Linq;

namespace MyLife.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IMongoCollection<User> Users;
        public UserRepository(ApplicationContext context) => Users = context.Users;
        public IEnumerable<User> GetAll() => Users.Find(user => true).ToList();
        public User GetById(string id) => Users.Find(user => user.Id.Equals(id)).FirstOrDefault();
        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate) => Users.Find(predicate).ToList();
        public User FindOne(Expression<Func<User, bool>> predicate) => Users.Find(predicate).ToList().FirstOrDefault();
        public void Add(User item) => Users.InsertOne(item);
        public void Update(User item) => Users.ReplaceOne(user => user.Id == item.Id, item);
        public void Remove(User item) => Users.DeleteOne(user => user.Id == item.Id);
    }
}
