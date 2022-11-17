using Authentication.Core.Datas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Authentication.Core.Sources
{
    public class UserJsonSource : IUserSource
    {
        private string userPath;

        public UserJsonSource(string userPath = null)
        {
            this.userPath = userPath ?? Path.Combine(
                new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName,
                @"Config\User.json");

            if (File.Exists(this.userPath) == false)
            {
                DirectoryInfo directory = new FileInfo(this.userPath).Directory;
                if (directory.Exists == false)
                {
                    directory.Create();
                }
            }
        }

        public List<User> LoadUsers()
        {
            var users = new List<User>();

            if (string.IsNullOrWhiteSpace(userPath))
            {
                return null;
            }
            else if (File.Exists(userPath) == false)
            {
                SaveUsers(new List<User>());
            }

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string readString = File.ReadAllText(userPath);
            users = JsonConvert.DeserializeObject<List<User>>(readString, settings);

            return users;
        }

        public void SaveUsers(List<User> users)
        {
            var file = new FileInfo(userPath);
            if (Directory.Exists(file.Directory.FullName) == false)
            {
                Directory.CreateDirectory(file.Directory.FullName);
            }

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string writeString = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented, settings);
            File.WriteAllText(userPath, writeString);
        }
    }
}
