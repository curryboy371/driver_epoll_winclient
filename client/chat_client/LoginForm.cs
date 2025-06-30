using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using Login;
using Chat;

using System.Net.Sockets;
using Join;
using Admin;
using Leave;

namespace chat_client {
    public partial class LoginForm : Form, IPacketHandler {


        public LoginForm() {
            InitializeComponent();

            bool connected = NetworkManager.Instance.Connect("10.10.16.142", 9000);
            if (!connected)
                NetworkManager.Instance.Disconnect();
                System.Windows.Forms.Application.Exit();

            NetworkManager.Instance.SetHandler(this);
        }


        private void button_login(object sender, EventArgs e) {
            string userId = textBox_ID.Text.Trim();
            string userPw = textBox_PW.Text.Trim();

            LoginRequest login = new LoginRequest {
                Id = userId,
                Password = userPw
            };

            NetworkManager.Instance.SendMessage(PacketCommand.CMD_LOGIN_REQUEST, login);
        }

        private void button_join(object sender, EventArgs e) {

            JoinForm join = new JoinForm(this);
            join.Show();
            this.Hide();
        }

        private void button_connect_Click(object sender, EventArgs e) {

            bool connected = NetworkManager.Instance.Connect("127.0.0.1", 9000);
            if (!connected)
            {
                NetworkManager.Instance.Disconnect();
                System.Windows.Forms.Application.Exit();
            }


            NetworkManager.Instance.SetHandler(this);
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e) {

            NetworkManager.Instance.Disconnect();
            System.Windows.Forms.Application.Exit();
        }

        public void OnLoginResponse(LoginResponse response) {

            // UI thread에서만 실행
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnLoginResponse(response)));
                return; 
            }
             
             if (response.Success)
            {
                // 내 정보 저장
                UserManager.Instance.SetMyUserInfo(response.Sender.Id, response.Sender.Name, response.Sender.Uid);

                // 유저 리스트 전체 클리어
                UserManager.Instance.UsersClear();

                // 서버에서 넘어온 전체 유저 리스트 추가
                foreach (var user in response.Users) {
                    //if (user.Id != response.Sender.Id) {
                        UserManager.Instance.AddUser(user.Id, user.Name, user.Uid);
                    //}
                }

                ChatForm chat = new ChatForm(this);
                chat.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show($"로그인 실패 {response.Message}");
            }
        }
        public void OnJoinResponse(JoinResponse response) {  }
        public void OnChatMessage(ChatMessage message) {  }

        public void OnAdminResponse(AdminMessage response) {
        }

        public void OnLeaveNotice(LeaveNotice message) {
        }

        public void OnJoinNotice(JoinNotice notice) {
        }

        public void OnChangeNameResponse(ChangeNameResponse response) {
        }
        public void OnChangeNameNotice(ChangeNameNotice notice) {
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
