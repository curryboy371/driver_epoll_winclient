namespace chat_client {
    partial class ChatForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.listBox_Chat = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.richTextBox_ChatInput = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Chat
            // 
            this.listBox_Chat.FormattingEnabled = true;
            this.listBox_Chat.ItemHeight = 12;
            this.listBox_Chat.Location = new System.Drawing.Point(136, 84);
            this.listBox_Chat.Name = "listBox_Chat";
            this.listBox_Chat.Size = new System.Drawing.Size(218, 160);
            this.listBox_Chat.TabIndex = 0;
            this.listBox_Chat.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(610, 84);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 88);
            this.listBox2.TabIndex = 1;
            // 
            // richTextBox_ChatInput
            // 
            this.richTextBox_ChatInput.Location = new System.Drawing.Point(189, 283);
            this.richTextBox_ChatInput.Name = "richTextBox_ChatInput";
            this.richTextBox_ChatInput.Size = new System.Drawing.Size(327, 96);
            this.richTextBox_ChatInput.TabIndex = 2;
            this.richTextBox_ChatInput.Text = "";
            this.richTextBox_ChatInput.TextChanged += new System.EventHandler(this.richTextBox_ChatInput_TextChanged);
            this.richTextBox_ChatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox_ChatInput_KeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 76);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox_ChatInput);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox_Chat);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Chat;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.RichTextBox richTextBox_ChatInput;
        private System.Windows.Forms.Button button1;
    }
}