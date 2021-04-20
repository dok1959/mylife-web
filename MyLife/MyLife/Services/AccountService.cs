using MyLife.Data;
using MyLife.Models;
using MyLife.Repositories;
using MyLife.ViewModels;
using System.Linq;

namespace MyLife.Services
{
    public class AccountService : IAccountService
    {
        private IRepository<User> _repo;

        public AccountService(IRepository<User> repo)
        {
            _repo = repo;
        }

        public bool Authenticate(LoginViewModel model)
        {
            var user = _repo.Find(x => x.Login == model.Login).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.HashedPassword))
            {
                return false;
            }
            return true;
        }

        public void Register(RegisterViewModel model)
        {
            var user = new User(model);
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            _repo.Add(user);
        }
    }
}
