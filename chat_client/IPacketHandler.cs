using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Login;
using Chat;
using Join;
using Admin;
using Leave;

namespace chat_client {
    public interface IPacketHandler {
        void OnLoginResponse(LoginResponse response);
        void OnJoinResponse(JoinResponse response);
        void OnChatMessage(ChatMessage message);

        void OnLeaveNotice(LeaveNotice message);

        void OnJoinNotice(JoinNotice notice);                         // 누군가 입장함
        void OnChangeNameResponse(ChangeNameResponse response);       // 닉네임 변경 응답
        void OnChangeNameNotice(ChangeNameNotice notice);               // 닉네임 변경 알림

        void OnAdminResponse(AdminMessage response);
    }
}