using MyLife.Models;
using MyLife.Repositories;
using MyLife.Services.PasswordHashers;
using MyLife.ViewModels;
using System.Linq;

namespace MyLife.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private IRepository<User> _repo;
        private IPasswordHasher _passwordHasher;

        public AccountService(IRepository<User> repo, IPasswordHasher passwordHasher)
        {
            _repo = repo;
            _passwordHasher = passwordHasher;
        }

        public User Authenticate(LoginViewModel model)
        {
            var user = _repo.Find(x => x.Login == model.Login)?.FirstOrDefault();
            if (user == null || !_passwordHasher.VerifyPassword(model.Password, user.HashedPassword))
            {
                return null;
            }
            return user;
        }

        public void Register(RegisterViewModel model)
        {
            var user = new User(model);
            user.HashedPassword = _passwordHasher.HashPassword(model.Password);
            _repo.Add(user);
        }
    }
}
