using Admin;
using Chat;
using Join;
using Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chat_client {
    public partial class JoinForm : Form, IPacketHandler {

        private LoginForm parentForm;
        public JoinForm(LoginForm form) {
            InitializeComponent();
            parentForm = form;
            NetworkManager.Instance.SetHandler(this);
        }

        private void button_join(object sender, EventArgs e) {

            string id = textBox_ID.Text.Trim();
            string password = textBox_PW.Text.Trim();
            string nickname = textBox_Name.Text.Trim();

            if (string.IsNullOrEmpty(id)) {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }
            if (string.IsNullOrEmpty(password)) {
                MessageBox.Show("비밀번호를 입력하세요.");
                return;
            }
            if (string.IsNullOrEmpty(nickname)) {
                MessageBox.Show("닉네임을 입력하세요.");
                return;
            }

            // 서버로 회원가입 요청 전송
            JoinRequest request = new JoinRequest {
                Id = id,
                Password = password,
                Name = nickname
            };

            NetworkManager.Instance.SendMessage(PacketCommand.CMD_JOIN_REQUEST, request);

            UserManager.Instance.SetMyUserInfo(id, nickname);
        }


        private void button_cancel(object sender, EventArgs e) {
            NetworkManager.Instance.SetHandler(parentForm);
            parentForm.Show();
            this.Close();
        }


        private void JoinForm_Load(object sender, EventArgs e) {

        }

        private void JoinForm_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }


        public void OnLoginResponse(LoginResponse response) { }

        public void OnJoinResponse(JoinResponse response) {
            Invoke(new Action(() => {
                if (response.Success == true) {
                    MessageBox.Show("회원가입 성공!");
                    ChatForm chat = new ChatForm(parentForm);
                    chat.Show();
                    this.Hide();

                } else {
                    MessageBox.Show("회원가입 실패: " + response.Message);
                }
            }));
        }
        public void OnChatMessage(ChatMessage message) { }


        public void OnAdminResponse(AdminMessage response) {
        }
    }
}
