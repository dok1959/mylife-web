using MyLife.Models;
using MyLife.ViewModels;

namespace MyLife.Services
{
    public interface IAccountService
    {
        void Register(RegisterViewModel model);
        User Authenticate(LoginViewModel model);
    }
}
