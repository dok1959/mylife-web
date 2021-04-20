using MyLife.ViewModels;

namespace MyLife.Services
{
    public interface IAccountService
    {
        void Register(RegisterViewModel model);
        bool Authenticate(LoginViewModel model);
    }
}
