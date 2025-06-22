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

namespace chat_client {
    public partial class LoginForm : Form, IPacketHandler {


        public LoginForm() {
            InitializeComponent();

            bool connected = NetworkManager.Instance.Connect("127.0.0.1", 9000);
            if (!connected)
                Application.Exit();

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
                Application.Exit();

            NetworkManager.Instance.SetHandler(this);
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }

        public void OnLoginResponse(LoginResponse response) {  }
        public void OnJoinResponse(JoinResponse response) {  }
        public void OnChatMessage(ChatMessage message) {  }

        public void OnAdminResponse(AdminMessage response) {
        }
    }
}
