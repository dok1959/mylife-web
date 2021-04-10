using MyLife.Data;
using MyLife.Models;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace MyLife.Repositories
{
    public class UserRepository : IRepository<User, ObjectId>
    {
        private ApplicationContext _context;
        public UserRepository(ApplicationContext context) => _context = context;

        private IMongoCollection<User> Users => _context.Database.GetCollection<User>(nameof(User));
        public User Get(ObjectId id) => Users.Find(user => user.Id == id).FirstOrDefault();
        public ICollection<User> GetAll() => Users.Find(user => true).ToList();
        public ICollection<User> Find(Expression<Func<User, bool>> predicate) => Users.Find(predicate).ToList();
        public void Add(User item) => Users.InsertOne(item);
        public void Update(User item) => Users.ReplaceOne(user => user.Id == item.Id, item);
        public void Remove(User item) => Users.DeleteOne(user => user.Id == item.Id);
    }
}
