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
    public partial class ChatForm : Form, IPacketHandler {

        private LoginForm parentForm;

        public ChatForm(LoginForm form) {
            InitializeComponent();
            parentForm = form;
            NetworkManager.Instance.SetHandler(this);
        }

        public void OnChatMessage(ChatMessage message) {

            // UI 스레드에서 안전하게 업데이트 (Invoke 필요)
            Invoke(new Action(() =>
            {
                string text = $"{message.Name}: {message.Message}";
                listBox_Chat.Items.Add(text);


                // 스크롤 내림
                listBox_Chat.TopIndex = listBox_Chat.Items.Count - 1;
            }));
        }

        public void OnJoinResponse(JoinResponse response) {
            throw new NotImplementedException();
        }

        public void OnLoginResponse(LoginResponse response) {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e) {

            string inputText = richTextBox_ChatInput.Text.Trim();

            if (!string.IsNullOrEmpty(inputText)) {
                // 1. 서버에 채팅 전송
                ChatMessage msg = new ChatMessage {
                    Name = UserManager.Instance.UserName,
                    Message = inputText
                };

                NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_MESSAGE, msg);

                // 2. 입력창 비우기
                richTextBox_ChatInput.Clear();
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {


        }

        private void richTextBox_ChatInput_TextChanged(object sender, EventArgs e) {

        }

        private void richTextBox_ChatInput_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyCode == Keys.Enter) {
                string inputText = richTextBox_ChatInput.Text.Trim();

                if (!string.IsNullOrEmpty(inputText)) {
                    // 1. 서버에 채팅 전송
                    ChatMessage msg = new ChatMessage {
                        Name = UserManager.Instance.UserName,
                        Message = inputText
                    };

                    NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_MESSAGE, msg);

                    // 2. 입력창 비우기
                    richTextBox_ChatInput.Clear();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;  // 엔터 소리/다음 줄 방지
            }

        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e) {
            Application.Exit();
        }

        public void OnAdminResponse(AdminMessage response) {
            // UI 스레드에서 안전하게 업데이트 (Invoke 필요)
            Invoke(new Action(() => {
                string text = response.Message;
                listBox_Chat.Items.Add(text);

                // 스크롤 내림
                listBox_Chat.TopIndex = listBox_Chat.Items.Count - 1;
            }));
        }
    }
}
