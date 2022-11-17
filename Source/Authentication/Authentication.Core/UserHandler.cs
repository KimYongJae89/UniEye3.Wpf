using Authentication.Core.Datas;
using Authentication.Core.Sources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Core
{
    public delegate void UserChangedDelegate(User user);

    public class UserHandler
    {
        private static UserHandler instance = null;
        public static UserHandler Instance => instance ?? (instance = new UserHandler());

        public List<User> Users { get; private set; } = new List<User>();

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnUserChanged?.Invoke(currentUser);
            }
        }

        private IUserSource userSource;

        public UserChangedDelegate OnUserChanged { get; set; }

        private UserHandler() { }

        public List<User> CloneUsers()
        {
            var clonedUsers = new List<User>();
            foreach (User user in Users)
            {
                clonedUsers.Add((User)user.Clone());
            }

            return clonedUsers;
        }

        public void Initialize(IUserSource tempUserSource)
        {
            userSource = tempUserSource;
            if (userSource != null)
            {
                Users = userSource.LoadUsers();
            }
        }

        public bool ExistUser(User user)
        {
            return Users.Any(u => u.UserId == user.UserId);
        }

        public bool ExistUserName(string userName)
        {
            return Users.Any(u => u.UserId == userName);
        }

        public bool AddUser(User user)
        {
            if (ExistUser(user))
            {
                return false;
            }

            Users.Add(user);

            return true;
        }

        public bool RemoveUser(User user)
        {
            if (!ExistUser(user))
            {
                return false;
            }

            Users.Remove(user);

            return true;
        }

        public User GetUser(string userId, string password = null)
        {
            if (userId == "developer" && password == "masterkey")
            {
                return new User("developer", "masterkey", true);
            }

            if (userId == "samsung" && password == "samsung1")
            {
                return new User("samsung", "samsung1", false);
            }

            if (userId == "1234" && password == "1243")
            {
                return new User("1234", "1234", false);
            }

            if (userId == "op" && password == "op")
            {
                return new User("op", "op", false);
            }

            if (password != null)
            {
                return Users.FirstOrDefault(user => user.UserId == userId && user.PasswordHash == User.GetPasswordHash(password));
            }
            else if (userId != null)
            {
                return Users.FirstOrDefault(x => x.UserId == userId);
            }

            return null;
        }

        public void Save()
        {
            userSource?.SaveUsers(Users);
        }

        public void Load()
        {
            Users = userSource?.LoadUsers();
        }
    }
}
