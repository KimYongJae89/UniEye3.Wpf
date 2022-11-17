using Authentication.Core.Datas;
using System.Collections.Generic;

namespace Authentication.Core.Sources
{
    public interface IUserSource
    {
        List<User> LoadUsers();
        void SaveUsers(List<User> users);
    }
}
