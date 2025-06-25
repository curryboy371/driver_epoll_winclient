using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Login;
using Chat;
using Join;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.IO;
using Admin;
using Leave;

namespace chat_client {

    public class PacketDispatcher {
        private IPacketHandler handler;

        public PacketDispatcher(IPacketHandler packetHandler) {
            handler = packetHandler;
        }
        public void SetHandler(IPacketHandler packetHandler) {
            handler = packetHandler;
        }

        public void Dispatch(PacketCommand cmd, byte[] body) {

            Console.WriteLine($"Receive packet: {cmd}");

            try {

                switch (cmd) {
                    case PacketCommand.CMD_LOGIN_RESPONSE:
                        LoginResponse loginRes = LoginResponse.Parser.ParseFrom(body);
                        handler?.OnLoginResponse(loginRes);
                        break;

                    case PacketCommand.CMD_JOIN_RESPONSE:
                        JoinResponse joinRes = JoinResponse.Parser.ParseFrom(body);
                        handler?.OnJoinResponse(joinRes);
                        break;

                    case PacketCommand.CMD_CHAT_MESSAGE:
                        ChatMessage chatMsg = ChatMessage.Parser.ParseFrom(body);
                        handler?.OnChatMessage(chatMsg);
                        break;

                    case PacketCommand.CMD_ADMIN_BROADCAST:
                        AdminMessage admin = AdminMessage.Parser.ParseFrom(body);
                        handler?.OnAdminResponse(admin);
                        break;

                    case PacketCommand.CMD_LEAVE_NOTIFY:
                        LeaveNotice leave = LeaveNotice.Parser.ParseFrom(body);
                        handler?.OnLeaveNotice(leave);
                        break;

                    case PacketCommand.CMD_JOIN_NOTIFY:
                        JoinNotice join = JoinNotice.Parser.ParseFrom(body);
                        handler?.OnJoinNotice(join);
                        break;

                    case PacketCommand.CMD_CHANGE_NAME_NOTIFY:
                        ChangeNameNotice changeName = ChangeNameNotice.Parser.ParseFrom(body);
                        handler?.OnChangeNameNotice(changeName);
                        break;

                    case PacketCommand.CMD_CHANGE_NAME_RESPONSE:
                        ChangeNameResponse changeNameResponse = ChangeNameResponse.Parser.ParseFrom(body);
                        handler?.OnChangeNameResponse(changeNameResponse);
                        break;

                    default:
                        Console.WriteLine($"알 수 없는 커맨드 수신: {cmd}");
                        break;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("패킷 파싱 실패: " + ex.Message);
            }

        }
    }


}
