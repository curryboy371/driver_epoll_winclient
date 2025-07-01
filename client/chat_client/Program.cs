using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chat_client {
    internal static class Program {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        /// 
        public static LoginForm login_form;


        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            login_form = new LoginForm(args);

            if(NetworkManager.Instance.isConnect()) {
                Application.Run(login_form);
            }
        }
    }
}
