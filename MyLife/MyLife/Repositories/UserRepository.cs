using MyLife.Data;
using MyLife.Models;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace MyLife.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ApplicationContext _context;
        public UserRepository(ApplicationContext context) => _context = context;
        private IMongoCollection<User> Users => _context.Database.GetCollection<User>(nameof(User));
        public User Get(string id) => Users.Find(user => user.Id.Equals(new ObjectId(id))).FirstOrDefault();
        public ICollection<User> GetAll() => Users.Find(user => true).ToList();
        public ICollection<User> Find(Expression<Func<User, bool>> predicate) => Users.Find(predicate).ToList();
        public void Add(User item) => Users.InsertOne(item);
        public void Update(User item) => Users.ReplaceOne(user => user.Id == item.Id, item);
        public void Remove(User item) => Users.DeleteOne(user => user.Id == item.Id);
    }
}
