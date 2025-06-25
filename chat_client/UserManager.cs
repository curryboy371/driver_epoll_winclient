using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_client {
    internal class UserManager {

        
        // User 구조체
        internal class User
        {
            public string Id { get; }
            public string Name { get; }

            public User(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        private static UserManager instance = new UserManager();
        public static UserManager Instance => instance;

        private UserManager() { }

        // 유저 딕셔너리
        private Dictionary<string, User> userMap = new Dictionary<string, User>();


        // my info
        public User myUser { get; private set; } = new User("", "");

        public void SetMyUserInfo(string id, string name)
        {
            myUser = new User(id, name);
        }

        public void AddUser(string id, string name)
        {
            if (!userMap.ContainsKey(id))
            {
                userMap[id] = new User(id, name);
            }
        }

        public void RemoveUser(string id)
        {
            userMap.Remove(id);
        }

        public User GetUser(string id)
        {
            userMap.TryGetValue(id, out var user);
            return user;
        }

        public void Clear()
        {
            userMap.Clear();
        }


    }
}
