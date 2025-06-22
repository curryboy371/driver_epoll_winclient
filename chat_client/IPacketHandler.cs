using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Login;
using Chat;
using Join;
using Admin;

namespace chat_client {
    public interface IPacketHandler {
        void OnLoginResponse(LoginResponse response);
        void OnJoinResponse(JoinResponse response);
        void OnChatMessage(ChatMessage message);

        void OnAdminResponse(AdminMessage response);
    }
}