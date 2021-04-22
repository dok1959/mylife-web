using MyLife.Models;
using MyLife.ViewModels;

namespace MyLife.Services.AccountServices
{
    public interface IAccountService
    {
        void Register(RegisterViewModel model);
        User Authenticate(LoginViewModel model);
    }
}
