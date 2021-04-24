using MyLife.Data;
using MyLife.Models;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Linq;

namespace MyLife.Repositories
{
    public class DesireRepository : IRepository<Desire>
    {
        private readonly IMongoCollection<Desire> Desires;
        public DesireRepository(ApplicationContext context) => Desires = context.Desires;
        public IEnumerable<Desire> GetAll() => Desires.Find(user => true).ToList();
        public Desire GetById(string id) => Desires.Find(user => user.Id.Equals(id)).ToList().FirstOrDefault();
        public IEnumerable<Desire> Find(Expression<Func<Desire, bool>> predicate) => Desires.Find(predicate).ToList();
        public Desire FindOne(Expression<Func<Desire, bool>> predicate) => Desires.Find(predicate).ToList().FirstOrDefault();
        public void Add(Desire item) => Desires.InsertOne(item);
        public void Update(Desire item) => Desires.ReplaceOne(desire => desire.Id == item.Id, item);
        public void Remove(Desire item) => Desires.DeleteOne(desire => desire.Id == item.Id);
    }
}
