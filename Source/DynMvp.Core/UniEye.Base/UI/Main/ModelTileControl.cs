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
    public delegate void ModelSelectedDelegate(ModelDescription modelDescription);
    public delegate void CloseModelDelegate();
    public delegate void ModelDeletedDelegate(string modelName);

    public partial class ModelTileControl : UserControl
    {
        public bool ShowNewModelButton { get; set; } = true;

        private UltraButton selectedModelButton;
        private Infragistics.Win.Appearance appearanceModelButton = new Infragistics.Win.Appearance();

        public ModelSelectedDelegate ModelSelected;
        public CloseModelDelegate CloseModel;
        public ModelDeletedDelegate DeleteModel;

        public ModelTileControl()
        {
            InitializeComponent();

            buttonNewModel.Text = StringManager.GetString(buttonNewModel.Text);
        }

        private void ModelTileControl_Load(object sender, EventArgs e)
        {
            appearanceModelButton.BackColor = System.Drawing.Color.LemonChiffon;
            appearanceModelButton.FontData.Name = "NanumGothic";
            appearanceModelButton.FontData.SizeInPoints = 12F;
            appearanceModelButton.ForeColor = System.Drawing.Color.Black;
            appearanceModelButton.TextHAlignAsString = "Left";
            appearanceModelButton.TextVAlignAsString = "Bottom";

            buttonNewModel.Size = new System.Drawing.Size(150, 150);
        }

        private void RefreshList()
        {
            ModelManager.Instance().Refresh();
        }

        private void AddModelButton(ModelDescription modelDescription)
        {
            var modelButton = new UltraButton();

            modelButton.Appearance = appearanceModelButton;
            modelButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            modelButton.ImageSize = new System.Drawing.Size(116, 116);
            modelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            modelButton.Location = new System.Drawing.Point(3, 3);
            modelButton.Name = "buttonModel";
            modelButton.Size = new System.Drawing.Size(150, 150);
            modelButton.TabIndex = 2;
            modelButton.Tag = modelDescription;
            modelButton.Text = modelDescription.Name;
            modelButton.UseAppStyling = false;
            modelButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            modelButton.Click += modelButton_DoubleClick;
            modelButton.MouseUp += modelButton_MouseUp;
            panelModelList.Controls.Add(modelButton);
        }

        private void modelButton_DoubleClick(object sender, EventArgs e)
        {
            selectedModelButton = (UltraButton)sender;
            ModelManager.Instance().OpenModel((ModelDescription)selectedModelButton.Tag, null);
        }

        private void modelButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Size menuSize = modelMenu.MenuSettings.Size;
                selectedModelButton = (UltraButton)sender;
                Point point = selectedModelButton.PointToScreen(
                        new Point(selectedModelButton.Width - menuSize.Width / 2, selectedModelButton.Height - menuSize.Height / 2));
                modelMenu.Show(ParentForm, point);
            }
        }

        private void buttonNewModel_Click(object sender, EventArgs e)
        {
            CreateNewModel("");
        }

        public void CreateNewModel(string modelName = "")
        {
            //ModelForm newModelForm = new ModelForm();
            ModelForm newModelForm = UiManager.Instance().CreateModelForm();

            newModelForm.InitModelName = modelName;
            newModelForm.ModelFormType = ModelFormType.New;
            if (newModelForm.ShowDialog(this) == DialogResult.OK)
            {
                ModelDescription md = newModelForm.ModelDescription;
                ModelManager.Instance().AddModel(md);
                AddModelButton(md);

            }
        }

        private void modelMenu_ToolClick(object sender, Infragistics.Win.UltraWinRadialMenu.RadialMenuToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "Edit":
                    EditModelDescription();
                    ModelManager.Instance().Refresh();
                    break;
                case "Delete":
                    DeleteModelDescription();
                    ModelManager.Instance().Refresh();
                    break;
                case "Copy":
                    CopyModel();
                    ModelManager.Instance().Refresh();
                    break;
                case "Close":
                    CloseModel?.Invoke();
                    break;
                case "ExportFormat":
                    StepModel curModel = ModelManager.Instance().CurrentStepModel;
                    if (curModel != null && (curModel.Name == selectedModelButton.Text))
                    {
                        var form = new OutputFormatForm();
                        form.Model = curModel;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            ModelManager.Instance().SaveModelDescription(curModel.ModelDescription);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please, select the model first.");
                    }
                    break;
            }

            modelMenu.Hide();
        }

        private void CopyModel()
        {
            var copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.New;
            copyModelForm.ModelDescription = (ModelDescription)selectedModelButton.Tag;

            string srcModelName = copyModelForm.ModelDescription.Name;

            if (copyModelForm.ShowDialog(this) == DialogResult.OK)
            {
                string name = copyModelForm.ModelDescription.Name;
                ModelDescription copyMd = copyModelForm.ModelDescription.Clone();
                copyMd.Name = name;
                ModelManager.Instance().AddModel(copyMd);
                AddModelButton(copyMd);

                ModelManager.Instance().CopyModelData(srcModelName, copyMd.Name);
            }
        }

        private void EditModelDescription()
        {
            //ModelForm editModelForm = new ModelForm();
            ModelForm editModelForm = UiManager.Instance().CreateModelForm();
            editModelForm.ModelFormType = ModelFormType.Edit;
            editModelForm.ModelDescription = (ModelDescription)selectedModelButton.Tag;

            if (editModelForm.ShowDialog(this) == DialogResult.OK)
            {
                ModelManager.Instance().EditModel(editModelForm.ModelDescription);
            }
        }

        private void DeleteModelDescription()
        {

            string selModelName = ((ModelDescription)selectedModelButton.Tag).Name;
            ModelBase curModel = ModelManager.Instance().CurrentModel;

            if (curModel != null && curModel.Name == selModelName)
            {
                MessageBox.Show(this, string.Format("Can't delete current working model."), "Delete Model", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show(this, string.Format("Do you want to delete the model[{0}]?", selModelName), "Delete Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ModelManager.Instance().DeleteModel(selModelName);

                panelModelList.Controls.Remove(selectedModelButton);
                selectedModelButton = null;
                DeleteModel?.Invoke(selModelName);
            }
        }

        private void panelModelList_MouseUp(object sender, MouseEventArgs e)
        {
            modelMenu.Hide();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void RefreshModelList(string category, string searchModelName = "")
        {
            panelModelList.Controls.Clear();
            if (ShowNewModelButton == true)
            {
                panelModelList.Controls.Add(buttonNewModel);
            }

            foreach (ModelDescription modelDescription in ModelManager.Instance())
            {
                if (category == StringManager.GetString("All") || category == modelDescription.Category || modelDescription != null)
                {
                    if (searchModelName == "" || searchModelName == modelDescription.Name)
                    {
                        AddModelButton(modelDescription);
                    }
                }
            }
        }
    }
}
