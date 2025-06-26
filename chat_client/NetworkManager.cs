using System;
using System.Net.Sockets;

namespace chat_client {
    public class NetworkManager {
        private static NetworkManager instance = null;
        public static NetworkManager Instance {
            get {
                if (instance == null)
                    instance = new NetworkManager();
                return instance;
            }
        }

        private SocketHandler socketHandler;
        private PacketDispatcher dispatcher;

        private NetworkManager() { }

        public bool Connect(string serverIP, int port) {
            try {
                TcpClient client = new TcpClient();
                client.Connect(serverIP, port);

                dispatcher = new PacketDispatcher(null);
                socketHandler = new SocketHandler(client, dispatcher);
                socketHandler.StartReceiveLoop();

                return true;
            }
            catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show("서버 연결 실패: " + ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                socketHandler?.Close();

                dispatcher = null;
                socketHandler = null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("연결 해제 중 오류: " + ex.Message);
            }
        }

        public void SetHandler(IPacketHandler handler) {
            dispatcher?.SetHandler(handler);
        }

        public void SendMessage(PacketCommand cmd, Google.Protobuf.IMessage message) {

            Console.WriteLine($"Send packet: {cmd}");

            socketHandler?.SendMessage(cmd, message);
        }
    }
}
