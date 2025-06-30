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
using static System.Net.Mime.MediaTypeNames;

namespace chat_client {
    public partial class ChatForm : Form, IPacketHandler {

        private LoginForm parentForm;


        public ChatForm(LoginForm form) {
            InitializeComponent();
            parentForm = form;
            NetworkManager.Instance.SetHandler(this);

            RefreshUserListUI();
            string welcomMsg = $"{UserManager.Instance.MyUser.Name} 님 어서오세요";
            AppendSystemMessage(welcomMsg);

            textbox_name.Text = UserManager.Instance.MyUser.Name;
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

                bool bCommand = false;
                // 슬래시로 시작하면 명령어로 처리
                if (inputText.StartsWith("/"))
                {

                    if (inputText.StartsWith("/bmp180"))
                    {
                        bCommand = true;
                    }
                    else if(inputText.StartsWith("/lcd1602"))
                    {
                        bCommand = true;
                    }
                }

                if(bCommand)
                {
                    ChatCommand msg = new ChatCommand
                    {
                        Message = inputText
                    };
                    NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_COMMAND, msg);
                }
                else
                {
                    ChatMessage msg = new ChatMessage
                    {
                        Name = UserManager.Instance.MyUser.Name,
                        Message = inputText
                    };
                    NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_MESSAGE, msg);
                }

                // 입력창 비우기
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

                if (!string.IsNullOrEmpty(inputText))
                {

                    bool bCommand = false;
                    // 슬래시로 시작하면 명령어로 처리
                    if (inputText.StartsWith("/"))
                    {

                        if (inputText.StartsWith("/bmp180"))
                        {
                            bCommand = true;
                        }
                        else if (inputText.StartsWith("/lcd1602"))
                        {
                            bCommand = true;
                        }
                    }

                    if (bCommand)
                    {
                        ChatCommand msg = new ChatCommand
                        {
                            Message = inputText
                        };
                        NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_COMMAND, msg);
                    }
                    else
                    {
                        ChatMessage msg = new ChatMessage
                        {
                            Name = UserManager.Instance.MyUser.Name,
                            Message = inputText
                        };
                        NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_MESSAGE, msg);
                    }

                    // 입력창 비우기
                    richTextBox_ChatInput.Clear();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;  // 엔터 소리/다음 줄 방지
            }

        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e) {
            NetworkManager.Instance.Disconnect();
            System.Windows.Forms.Application.Exit();
        }
        private void AppendSystemMessage(string message) {
            listBox_Chat.Items.Add(message);
        }


        public void OnAdminResponse(AdminMessage response) {
            // UI 스레드에서 안전하게 업데이트 (Invoke 필요)
            Invoke(new Action(() => {
                string text = response.Message;
                AppendSystemMessage(text);

                // 스크롤 내림
                listBox_Chat.TopIndex = listBox_Chat.Items.Count - 1;
            }));
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {

        }
        public void RefreshUserListUI() {
            // ListBox 초기화
            listBoxUsers.Items.Clear();

            // UserManager에서 유저 리스트 가져옴
            var userList = UserManager.Instance.GetAllUsers();

            foreach (var user in userList) {
                if(UserManager.Instance.MyUser.Uid == user.Uid) {
                    listBoxUsers.Items.Add($"{user.Name}({user.Id})  - 나");
                }else {
                    listBoxUsers.Items.Add($"{user.Name}({user.Id})");
                }

            }
            label5.Text = userList.Count.ToString();
        }  
   
        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e) {

        }

        public void OnLeaveNotice(LeaveNotice message) {

            Invoke(new Action(() => {

                UserManager.Instance.RemoveUser(message.Sender.Uid);
                RefreshUserListUI();

                // msg
                string leave = $"{message.Sender.Name} 퇴장했습니니다";
                AppendSystemMessage(leave);
            }));
        }

        public void OnJoinNotice(JoinNotice notice) {


            if (this.InvokeRequired) {
                this.Invoke(new Action(() => OnJoinNotice(notice)));
                return;
            }

            UserManager.Instance.AddUser(notice.Sender.Id, notice.Sender.Name, notice.Sender.Uid);

            RefreshUserListUI();

            string joinMsg = $"{notice.Sender.Name} 님이 입장하였습니다.";
            // msg
            AppendSystemMessage(joinMsg);
        }

        public void OnChangeNameResponse(ChangeNameResponse response) {

            if (this.InvokeRequired) {
                this.Invoke(new Action(() => OnChangeNameResponse(response)));
                return;
            }

            if (response.Success) {
                // 내 닉네임도 업데이트
                UserManager.Instance.SetMyUserInfo(UserManager.Instance.MyUser.Id, response.NewName, UserManager.Instance.MyUser.Uid);
                UserManager.Instance.EditUserName(UserManager.Instance.MyUser.Uid, response.NewName);

                textbox_name.Text = UserManager.Instance.MyUser.Name;

                RefreshUserListUI();
                MessageBox.Show("닉네임이 성공적으로 변경");
            } else {
                AppendSystemMessage($"닉네임 변경 실패: {response.Message}");
            }
        }
        public void OnChangeNameNotice(ChangeNameNotice notice) {

            if (this.InvokeRequired) {
                this.Invoke(new Action(() => OnChangeNameNotice(notice)));
                return;
            }

            // 다른 유저 닉네임 업데이트
            UserManager.Instance.EditUserName(notice.Sender.Uid, notice.Sender.Name);
            RefreshUserListUI();

            string text = $"{notice.OldName} 님이 닉네임을 '{notice.Sender.Name}'(으)로 변경했습니다.";
            AppendSystemMessage(text);

        }

        private void ChatForm_Load(object sender, EventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {

            string currentName = UserManager.Instance.MyUser.Name;
            string newName = textbox_name.Text.Trim();

            if (string.IsNullOrEmpty(newName)) {
                MessageBox.Show("닉네임을 입력해주세요.");
                return;
            }

            if (currentName == newName) {
                MessageBox.Show("현재 닉네임과 동일합니다.");
                return;
            }

            // 변경 요청 프로토콜 생성
            var req = new ChangeNameRequest {
                NewName = newName
            };

            NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHANGE_NAME_REQUEST, req);
        }

        private async void button3_Click(object sender, EventArgs e) {

            int count = 5000;

            for (int i = 0; i < count; i++) {
                var msg = new ChatMessage {
                    Name = UserManager.Instance.MyUser.Name,
                    Message = $"[Spam@  !!!!!] Chat {i}"
                };

                NetworkManager.Instance.SendMessage(PacketCommand.CMD_CHAT_MESSAGE, msg);
                await Task.Delay(10);  // 10ms
            }

            MessageBox.Show("스팸 테스트 완료");

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
