using System;
using System.Windows.Forms;

namespace YYX.MailSendException
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            var host = textBoxServer.Text;
            if (string.IsNullOrEmpty(host))
            {
                return;
            }

            var userName = textBoxUserName.Text;
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            var password = textBoxPassword.Text;
            if (string.IsNullOrEmpty(password))
            {
                return;
            }

            try
            {
                const string message = "软件嗝屁了！！！";
                throw new Exception(message);
            }
            catch (Exception exception)
            {
                var body = exception.ToString();
                CrashHandler.SendEmail(host,userName, password, userName, body);
            }
        }
    }
}
 