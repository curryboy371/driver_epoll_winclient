using System;
using System.Collections.Generic;
using System.Linq;

namespace chat_client {
    internal class UserManager {

        internal class User {
            public int Uid { get; }
            public string Id { get; }
            public string Name { get; }
            public User(string id, string name, int uid) {
                Uid = uid;
                Id = id;
                Name = name;
            }
        }

        private static UserManager instance = new UserManager();
        public static UserManager Instance => instance;

        private UserManager() { }

        private Dictionary<int, User> userMap = new Dictionary<int, User>();

        public User MyUser { get; private set; } = new User("", "", 0);

        public bool IsDebugUser {  get;  set; }
        public int debug_timer { get; set; }
        public int debug_user_count { get; set; }


        public void SetMyUserInfo(string id, string name, int uid) {
            MyUser = new User(id, name, uid);
        }

        public void AddUser(string id, string name, int uid) {
            if (!userMap.ContainsKey(uid)) {
                userMap[uid] = new User(id, name, uid);
            }
        }

        public void EditUserName(int uid, string newName) {
            if (userMap.TryGetValue(uid, out var oldUser)) {
                var updatedUser = new User(oldUser.Id, newName, oldUser.Uid);
                userMap[uid] = updatedUser;
            }
        }

        public void RemoveUser(int uid) {
            userMap.Remove(uid);
        }

        public User GetUser(int uid) {
            userMap.TryGetValue(uid, out var user);
            return user;
        }

        public void UsersClear() {
            userMap.Clear();
        }

        public List<User> GetAllUsers() {
            return userMap.Values.ToList();
        }
    }
}
