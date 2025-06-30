using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;




namespace chat_client {
    public enum PacketCommand : ushort {
        CMD_CHAT_MESSAGE = 1001,
        CMD_LOGIN_REQUEST = 1002,
        CMD_LOGIN_RESPONSE = 1003,
        CMD_JOIN_REQUEST = 1004,
        CMD_JOIN_RESPONSE = 1005,
        CMD_JOIN_NOTIFY = 1006,
        CMD_LEAVE_NOTIFY = 1007,
        CMD_CHANGE_NAME_REQUEST = 1010,
        CMD_CHANGE_NAME_RESPONSE = 1011,
        CMD_CHANGE_NAME_NOTIFY = 1012,
        CMD_CHAT_COMMAND = 1013,

        CMD_ADMIN_BROADCAST = 2000
    }

    public class SocketHandler {
        private TcpClient client;
        private PacketDispatcher dispatcher;
        public SocketHandler(TcpClient tcpClient, PacketDispatcher dispatcher) {
            client = tcpClient;
            this.dispatcher = dispatcher;
        }

        public void SendMessage(PacketCommand cmd, IMessage message) { 
            try {
                using (var ms = new MemoryStream()) {

                    // 프로토버퍼 직렬화
                    message.WriteTo(ms);
                    byte[] body = ms.ToArray();

                    ushort totalLen = (ushort)(4 + body.Length); // 헤더 4바이트 + 바디 길이
                    ushort command = (ushort)cmd;

                    using (var sendStream = new MemoryStream()) {
                        // 네트워크 바이트 순서 (big-endian)로 변환
                        byte[] lenBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)totalLen));
                        byte[] cmdBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)command));

                        sendStream.Write(lenBytes, 0, 2);
                        sendStream.Write(cmdBytes, 0, 2);
                        sendStream.Write(body, 0, body.Length);

                        NetworkStream stream = client.GetStream();
                        stream.Write(sendStream.ToArray(), 0, (int)sendStream.Length);
                    }
                }
            }
            catch (Exception ex) {
                // 여기서는 UI MessageBox 말고 로그 출력 추천
                Console.WriteLine("메시지 전송 실패: " + ex.Message);
            }
        }
        public void StartReceiveLoop() {
            Task.Run(() =>
            {
                try {
                    NetworkStream stream = client.GetStream();
                    byte[] headerBuf = new byte[4]; // 2바이트 length + 2바이트 cmd

                    while (true) {
                        // 길이 먼저 수신
                        int read = 0;
                        while (read < 4) {
                            int n = stream.Read(headerBuf, read, 4 - read);
                            if (n == 0) throw new Exception("서버 연결 끊김");
                            read += n;
                        }

                        ushort totalLength = BitConverter.ToUInt16(headerBuf, 0);
                        ushort cmd = BitConverter.ToUInt16(headerBuf, 2);

                        totalLength = (ushort)IPAddress.NetworkToHostOrder((short)totalLength);
                        cmd = (ushort)IPAddress.NetworkToHostOrder((short)cmd);

                        if (totalLength < 4 || totalLength > 512) {
                            throw new Exception("비정상 패킷 길이");
                        }

                        int bodyLength = totalLength - 4;
                        byte[] bodyBuf = new byte[bodyLength];

                        int offset = 0;
                        while (offset < bodyLength) {
                            int n = stream.Read(bodyBuf, offset, bodyLength - offset);
                            if (n == 0) throw new Exception("서버 연결 끊김");
                            offset += n;
                        }

                        dispatcher.Dispatch((PacketCommand)cmd, bodyBuf);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("수신 오류: " + ex.Message);
                }
            });
        }

        public void Close() {
            try {
                client?.Close();
            }
            catch (Exception ex) {
                Console.WriteLine("소켓 종료 실패: " + ex.Message);
            }
        }
        public bool IsConnected() {
            if (client == null || !client.Connected)
                return false;

            return true;
        }
    }
}
