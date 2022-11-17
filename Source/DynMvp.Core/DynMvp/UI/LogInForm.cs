using Authentication.Core;
using Authentication.Core.Datas;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public partial class LogInForm : Form
    {
        public User LogInUser { get; set; }

        public LogInForm()
        {
            InitializeComponent();
            // 언어 변환 코드 작성부분 - 폼마다 다 만들어 줘야 함. 
            btnOk.Text = StringManager.GetString(btnOk.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);
            labelUserId.Text = StringManager.GetString(labelUserId.Text);
            labelPassword.Text = StringManager.GetString(labelPassword.Text);
        }

        private void LogInForm_Load(object sender, EventArgs e)
        {
            if (LogInUser != null)
            {
                userId.Text = LogInUser.UserId;
                password.Text = "";
            }
            else
            {
                userId.Text = "op";
                password.Text = "op";
            }
#if DEBUG
            userId.Text = "developer";
            password.Text = "masterkey";
#endif
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            LogInUser = UserHandler.Instance.GetUser(userId.Text, password.Text);
            if (LogInUser == null)
            {
                MessageBox.Show(StringManager.GetString("Invalid user id or password."), StringManager.GetString("Log In"));
                return;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
