using Admin;
using Chat;
using Join;
using Leave;
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
        }


        private void button_cancel(object sender, EventArgs e) {
            NetworkManager.Instance.SetHandler(parentForm);
            parentForm.Show();
            this.Close();
        }


        private void JoinForm_Load(object sender, EventArgs e) {

        }

        private void JoinForm_FormClosed(object sender, FormClosedEventArgs e) {
            NetworkManager.Instance.Disconnect();
            System.Windows.Forms.Application.Exit();

        }


        public void OnLoginResponse(LoginResponse response) { }

        public void OnJoinResponse(JoinResponse response) {
            Invoke(new Action(() => {
                if (response.Success == true) {
                    MessageBox.Show("회원가입 성공!");

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

        public void OnLeaveNotice(LeaveNotice message) {
        }

        public void OnJoinNotice(JoinNotice notice) {
        }

        public void OnChangeNameResponse(ChangeNameResponse response) {
        }
        public void OnChangeNameNotice(ChangeNameNotice notice) {
        }
    }
}
