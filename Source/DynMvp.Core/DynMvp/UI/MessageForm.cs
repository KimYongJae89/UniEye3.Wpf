using DynMvp.Base;
using DynMvp.UI;
using Infragistics.Win;
using Infragistics.Win.UltraMessageBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.UI
{
    public enum MessageFormType
    {
        Close, YesNo, RetryCancel, Buzzer
    }

    public partial class MessageForm : Form
    {
        private static UltraMessageBoxManager ultraMessageBoxManager = null;
        internal MessageFormType Type { get; set; }

        private string titleText;
        public string TitleText
        {
            set => titleText = value;
        }

        private static string defaultTitleText;
        public static string DefaultTitleText
        {
            set => defaultTitleText = value;
        }

        private string messageText;
        public string MessageText
        {
            set => messageText = value;
        }

        private SoundPlayer buzzerPlayer = new SoundPlayer(DynMvp.Properties.Resources.BUZZER_1);

        private MessageForm()
        {
            InitializeComponent();

            TopMost = true;

            //languge change
            btnYes.Text = StringManager.GetString(btnYes.Text);
            btnNo.Text = StringManager.GetString(btnNo.Text);
            btnClose.Text = StringManager.GetString(btnClose.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "btnClose_Click");
            LogHelper.Debug(LoggerType.Operation, string.Format("DialogResult = {0}", btnClose.DialogResult.ToString()));

            buzzerStart = false;

            Close();
        }

        public static DialogResult Show(Form parentForm, string msg, MessageFormType type = MessageFormType.Close)
        {
            //MessageForm messageForm = new MessageForm();
            //messageForm.Type = type;
            //messageForm.TitleText = defaultTitleText;
            //messageForm.MessageText = msg;

            //if (parentForm == null)
            //    return messageForm.ShowDialog();
            //else
            //    return messageForm.ShowDialog(parentForm);

            var _messageinfo = new UltraMessageBoxInfo
            {
                Style = MessageBoxStyle.Default,
                Icon = MessageBoxIcon.Error,
                DefaultButton = MessageBoxDefaultButton.Button1
            };

            if (type == MessageFormType.Close)
            {
                _messageinfo.Buttons = MessageBoxButtons.OK;
                _messageinfo.Icon = MessageBoxIcon.Information;
            }
            else if (type == MessageFormType.YesNo)
            {
                _messageinfo.Buttons = MessageBoxButtons.YesNo;
                _messageinfo.Icon = MessageBoxIcon.Question;
            }
            else if (type == MessageFormType.RetryCancel)
            {
                _messageinfo.Buttons = MessageBoxButtons.RetryCancel;
                _messageinfo.Icon = MessageBoxIcon.Warning;
            }

            // Set the Button to be selected by default
            //_messageinfo.ButtonAppearance.FontData.Bold = DefaultableBoolean.True;

            _messageinfo.Caption = "UniEye";
            //_messageinfo.Appearance.FontData.Bold = DefaultableBoolean.True;

            _messageinfo.Text = msg;
            //_messageinfo.Header = "Message";

            if (ultraMessageBoxManager == null)
            {
                ultraMessageBoxManager = new UltraMessageBoxManager();
            }

            // Aligns the buttons to the right
            ultraMessageBoxManager.ButtonAlignment = HAlign.Center;
            ultraMessageBoxManager.Appearance.FontData.SizeInPoints = 12;
            ultraMessageBoxManager.Appearance.FontData.Name = "맑은 고딕";
            return ultraMessageBoxManager.ShowMessageBox(_messageinfo);
        }

        public static DialogResult Show(Form parentForm, string msg, string title, MessageFormType type = MessageFormType.Close)
        {
            var _messageinfo = new UltraMessageBoxInfo
            {
                Style = MessageBoxStyle.Default,
                Icon = MessageBoxIcon.Error,
                DefaultButton = MessageBoxDefaultButton.Button1
            };

            if (type == MessageFormType.Close)
            {
                _messageinfo.Buttons = MessageBoxButtons.OK;
                _messageinfo.Icon = MessageBoxIcon.Information;
            }
            else if (type == MessageFormType.YesNo)
            {
                _messageinfo.Buttons = MessageBoxButtons.YesNo;
                _messageinfo.Icon = MessageBoxIcon.Question;
            }
            else if (type == MessageFormType.RetryCancel)
            {
                _messageinfo.Buttons = MessageBoxButtons.RetryCancel;
                _messageinfo.Icon = MessageBoxIcon.Warning;
            }

            // Set the Button to be selected by default
            //_messageinfo.ButtonAppearance.FontData.Bold = DefaultableBoolean.True;

            _messageinfo.Caption = title;
            //_messageinfo.Appearance.FontData.Bold = DefaultableBoolean.True;

            _messageinfo.Text = msg;
            //_messageinfo.Header = "Message";

            if (ultraMessageBoxManager == null)
            {
                ultraMessageBoxManager = new UltraMessageBoxManager();
            }

            // Aligns the buttons to the right
            ultraMessageBoxManager.ButtonAlignment = HAlign.Center;
            return ultraMessageBoxManager.ShowMessageBox(_messageinfo);
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            message.Text = messageText;
            labelTitle.Text = titleText;
            //StartBuzzerThread();
            switch (Type)
            {
                case MessageFormType.Close:
                    btnYes.Hide();
                    btnNo.Hide();
                    break;
                case MessageFormType.YesNo:
                    btnYes.DialogResult = DialogResult.Yes;
                    btnNo.DialogResult = DialogResult.No;
                    btnClose.Hide();
                    break;
                case MessageFormType.RetryCancel:
                    btnYes.DialogResult = DialogResult.Retry;
                    btnYes.Text = StringManager.GetString("Retry");

                    btnNo.DialogResult = DialogResult.Cancel;
                    btnNo.Text = StringManager.GetString("Cancel");
                    btnClose.Hide();

                    break;
                case MessageFormType.Buzzer:
                    btnYes.Hide();
                    btnNo.Hide();
                    StartBuzzerThread();
                    break;
                default:
                    throw new InvalidTypeException();
            }

            alarmCheckTimer.Start();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            FormMoveHelper.MouseDown(this);
        }

        private Thread buzzerThread;
        private void StartBuzzerThread()
        {
            if (buzzerThread == null || buzzerThread.IsAlive == false)
            {
                buzzerThread = new Thread(new ThreadStart(BuzzerThreadProc));
                buzzerThread.IsBackground = true;
                buzzerThread.Start();
            }
        }

        private bool buzzerStart = true;

        private void BuzzerThreadProc()
        {
            while (buzzerStart == true)
            {
                buzzerPlayer.Play();
                Thread.Sleep(1000);
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                LogHelper.Debug(LoggerType.Operation, "btnYes_Click");
                LogHelper.Debug(LoggerType.Operation, string.Format("DialogResult = {0}", btnYes.DialogResult.ToString()));

                buzzerStart = false;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "btnNo_Click");
            LogHelper.Debug(LoggerType.Operation, string.Format("DialogResult = {0}", btnNo.DialogResult.ToString()));

            buzzerStart = false;

            Close();
        }

        private void MessageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MessageForm_FormClosed");
        }

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MessageForm_FormClosing");
        }

        private void alarmCheckTimer_Tick(object sender, EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                Close();
            }
        }
    }
}
