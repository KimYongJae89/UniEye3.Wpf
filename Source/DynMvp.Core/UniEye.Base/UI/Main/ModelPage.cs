using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using Infragistics.Win.Misc;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class ModelPage : UserControl, IMainTabPage
    {
        private ModelTileControl modelTileControl;
        private Infragistics.Win.Appearance appearanceModelButton = new Infragistics.Win.Appearance();

        public ModelSelectedDelegate ModelSelected
        {
            get => modelTileControl.ModelSelected;
            set => modelTileControl.ModelSelected = value;
        }
        public CloseModelDelegate CloseModel { get; set; }

        public string TabName => "Model";

        public TabKey TabKey => TabKey.Model;

        public Bitmap TabIcon => Properties.Resources.product_gray;

        public Bitmap TabSelectedIcon => Properties.Resources.product_white;

        public Color TabSelectedColor => Color.FromArgb(206, 26, 55);

        public bool IsAdminPage => false;

        public Uri Uri => throw new NotImplementedException();

        public ModelPage()
        {
            InitializeComponent();

            modelTileControl = new ModelTileControl();

            modelTileControlPanel.Controls.Add(modelTileControl);

            modelTileControl.BackColor = System.Drawing.SystemColors.ButtonFace;
            modelTileControl.Dock = System.Windows.Forms.DockStyle.Fill;
            modelTileControl.Location = new System.Drawing.Point(0, 313);
            modelTileControl.Name = "ModelTileControl";
            modelTileControl.Size = new System.Drawing.Size(466, 359);
            modelTileControl.TabIndex = 0;
        }

        public void ChangeCaption()
        {
            labelCategory.Text = StringManager.GetString(labelCategory.Text);
            buttonRefresh.Text = StringManager.GetString(buttonRefresh.Text);
            labelFind.Text = StringManager.GetString(labelFind.Text);
        }

        private void HomePanel_Load(object sender, EventArgs e)
        {

        }

        public void Initialize()
        {
            RefreshList();
        }

        public void OnIdle()
        {

        }

        private void RefreshList()
        {
            ModelManager.Instance().Refresh();

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(StringManager.GetString("All"));

            foreach (string category in ModelManager.Instance().CategoryList)
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchModelName.Text = "";
            modelTileControl.RefreshModelList(cmbCategory.Text);
        }

        private void buttonCloseMode_Click(object sender, EventArgs e)
        {
            CloseModel?.Invoke();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            modelTileControl.RefreshModelList(cmbCategory.Text, searchModelName.Text);
        }

        private void searchModelName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                modelTileControl.RefreshModelList(cmbCategory.Text, searchModelName.Text);
            }
        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {

        }

        private void ModelTileControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                searchModelName.Text = "";
            }
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {
            if (searchModelName.Focused == false)
            {
                searchModelName.Focus();
                if ((e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z) || (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9))
                {
                    searchModelName.Text = e.KeyCode.ToString();
                }

                searchModelName.Select(searchModelName.Text.Length, 0);
            }
        }
    }
}
