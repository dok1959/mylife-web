using MyLife.Models;

namespace MyLife.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public UserViewModel()
        {

        }
        public UserViewModel(User user)
        {
            Id = user.Id.ToString();
            Username = user.Username;
        }
    }
}
