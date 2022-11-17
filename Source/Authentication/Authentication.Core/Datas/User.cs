using Authentication.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.Core.Datas
{
    public class User : INotifyPropertyChanged, ICloneable
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string userId = string.Empty;
        public string UserId
        {
            get => userId;
            set => Set(ref userId, value);
        }

        private string passwordHash;
        public string PasswordHash
        {
            get => passwordHash;
            set => Set(ref passwordHash, value);
        }

        private DateTime createdDate;
        public DateTime CreatedDate
        {
            get => createdDate;
            set => Set(ref createdDate, value);
        }

        private Dictionary<string, bool> authorizeDic = new Dictionary<string, bool>();
        public Dictionary<string, bool> AuthorizeDic
        {
            get => authorizeDic;
            set => Set(ref authorizeDic, value);
        }

        public int RoleType => GetRoleType();

        public User()
        {
            foreach (string type in Enum.GetNames(typeof(ERoleType)))
            {
                AuthorizeDic.Add(type, false);
            }
        }

        public User(User user)
        {
            UserId = user.UserId;
            CreatedDate = user.CreatedDate;

            AuthorizeDic.Clear();
            foreach (string type in Enum.GetNames(typeof(ERoleType)))
            {
                AuthorizeDic.Add(type, false);
                if (user.AuthorizeDic.ContainsKey(type))
                {
                    AuthorizeDic[type] = user.AuthorizeDic[type];
                }
            }
        }

        public User(string userId, string password, bool superAccount = false)
        {
            UserId = userId;
            PasswordHash = GetPasswordHash(password);
            CreatedDate = DateTime.Now;

            foreach (string type in Enum.GetNames(typeof(ERoleType)))
            {
                if (superAccount == true)
                {
                    AuthorizeDic.Add(type, true);
                }
                else
                {
                    if (userId == "samsung" || userId == "1234")
                    {
                        var tempRoleType = (ERoleType)Enum.Parse(typeof(ERoleType), type);
                        if (((int)tempRoleType & 0x07BF) != 0x0000)
                        {
                            AuthorizeDic.Add(type, true);
                        }
                        else
                        {
                            AuthorizeDic.Add(type, false);
                        }
                    }
                    else
                    {
                        var tempRoleType = (ERoleType)Enum.Parse(typeof(ERoleType), type);
                        if (((int)tempRoleType & 0x009B) != 0x0000)
                        {
                            AuthorizeDic.Add(type, true);
                        }
                        else
                        {
                            AuthorizeDic.Add(type, false);
                        }
                    }
                }
            }
        }

        public User(string userId, string password, Dictionary<string, bool> authorizeDic)
        {
            UserId = userId;
            PasswordHash = GetPasswordHash(password);
            CreatedDate = DateTime.Now;
            AuthorizeDic = authorizeDic;
        }

        private int GetRoleType()
        {
            int roleType = 0x0000;
            foreach (KeyValuePair<string, bool> pair in AuthorizeDic)
            {
                if (pair.Value == true)
                {
                    if (Enum.TryParse<ERoleType>(pair.Key, out ERoleType tmpeRoleType))
                    {
                        roleType |= (int)tmpeRoleType;
                    }
                }
            }

            return roleType;
        }

        public static string GetPasswordHash(string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            byte[] result = sha.ComputeHash(passwordByte);

            return Convert.ToBase64String(result);
        }

        public object Clone()
        {
            var clone = new User();
            System.Reflection.PropertyInfo[] properties = typeof(User).GetProperties();
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                prop.SetValue(clone, prop.GetValue(this));
            }

            return clone;
        }

        public bool IsAuth(ERoleType roleType)
        {
            return ((int)roleType & RoleType) == (int)roleType;
        }
    }
}
