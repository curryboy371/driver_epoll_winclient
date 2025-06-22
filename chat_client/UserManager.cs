using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat_client {
    internal class UserManager {

        private static UserManager instance = new UserManager();
        public static UserManager Instance => instance;

        private UserManager() { }

        public string UserId { get; private set; }
        public string UserName { get; private set; }

        public void SetUserInfo(string id, string name) {
            UserId = id;
            UserName = name;
        }

        public void Clear() {
            UserId = null;
            UserName = null;
        }


    }
}
