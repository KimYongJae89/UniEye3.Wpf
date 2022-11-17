using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public enum ModelFormType
    {
        New, Edit, Copy
    }

    public interface IModelFormExtraProperty
    {
        void Initialize(ModelDescription modelDescription);
        bool GetModelData(ModelDescription modelDescription);
    }

    public partial class ModelForm : Form
    {
        public string InitModelName { get; set; }
        public ModelDescription ModelDescription { get; set; } = null;
        public ModelFormType ModelFormType { get; set; }
        public IModelFormExtraProperty ExtraProperty { get; set; } = null;
        public bool ShowProductCode { get; set; }

        public ModelForm()
        {
            InitializeComponent();

            label1.Text = StringManager.GetString(label1.Text);

            // language change
            labelModelName.Text = StringManager.GetString(labelModelName.Text);
            labelDescription.Text = StringManager.GetString(labelDescription.Text);
            labelProductName.Text = StringManager.GetString(labelProductName.Text);
            labelItemCode.Text = StringManager.GetString(labelItemCode.Text);
            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            //SystemType systemType = Settings.Instance().Operation.SystemType;

            //switch(systemType)
            //{
            //    case SystemType.FPCBAlignChecker:
            //    case SystemType.FPCBAlignChecker2:
            //        {
            //            ModelPropertyFpcbAlignCheckPanel modelPropertyFpcbAlignCheckPanel = new ModelPropertyFpcbAlignCheckPanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyFpcbAlignCheckPanel);

            //            modelPropertyFpcbAlignCheckPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyFpcbAlignCheckPanel.Name = "modelPropertyFpcbAlignCheckPanel";
            //            modelPropertyFpcbAlignCheckPanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyFpcbAlignCheckPanel.TabIndex = 147;

            //            extraProperty = modelPropertyFpcbAlignCheckPanel;
            //        }

            //        break;
            //    case SystemType.FPCBAligner:
            //        {
            //            ModelPropertyFpcbAlignerPanel modelPropertyFpcbAlignerPanel = new ModelPropertyFpcbAlignerPanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyFpcbAlignerPanel);

            //            modelPropertyFpcbAlignerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyFpcbAlignerPanel.Name = "modelPropertyFpcbAlignerPanel";
            //            modelPropertyFpcbAlignerPanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyFpcbAlignerPanel.TabIndex = 147;

            //            extraProperty = modelPropertyFpcbAlignerPanel;
            //        }

            //        break;

            //    case SystemType.MaskInspector:
            //        {
            //            ModelPropertyMaskInspector modelPropertyMaskInspector = new ModelPropertyMaskInspector();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyMaskInspector);

            //            modelPropertyMaskInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyMaskInspector.Name = "modelPropertyMaskInspector";
            //            modelPropertyMaskInspector.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyMaskInspector.TabIndex = 147;

            //            extraProperty = modelPropertyMaskInspector;
            //        }

            //        break;
            //    case SystemType.BoxBarcode:
            //        {
            //            ModelPropertyBoxBarcodePanel modelPropertyBoxBarcodePanel = new ModelPropertyBoxBarcodePanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyBoxBarcodePanel);

            //            modelPropertyBoxBarcodePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyBoxBarcodePanel.Name = "modelPropertyBoxBarcodePanel";
            //            modelPropertyBoxBarcodePanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyBoxBarcodePanel.TabIndex = 147;

            //            extraProperty = modelPropertyBoxBarcodePanel;
            //        }
            //        break;
            //    case SystemType.ShampooBarcode:
            //        {
            //            ModelPropertyShampooBarcodePanel modelPropertyShampooBarcodePanel = new ModelPropertyShampooBarcodePanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyShampooBarcodePanel);

            //            modelPropertyShampooBarcodePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyShampooBarcodePanel.Name = "modelPropertyBoxBarcodePanel";
            //            modelPropertyShampooBarcodePanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyShampooBarcodePanel.TabIndex = 147;

            //            extraProperty = modelPropertyShampooBarcodePanel;
            //        }
            //        break;

            //}
        }

        private void ModelForm_Load(object sender, EventArgs e)
        {
            panelProductCode.Visible = ShowProductCode;

            ExtraProperty = UiManager.Instance().CreateModelExtraPropertyPanel();
            if (ExtraProperty != null)
            {
                Debug.Assert(ExtraProperty is UserControl);

                var extraPropertyControl = (UserControl)ExtraProperty;
                extraModelPropertyPanel.Controls.Add(extraPropertyControl);
                extraPropertyControl.Dock = DockStyle.Fill;
            }

            foreach (string category in ModelManager.Instance().CategoryList)
            {
                cmbCategory.Items.Add(category);
            }

            if (ModelFormType != ModelFormType.New && ModelDescription == null)
            {
                if (ModelManager.Instance().CurrentModel != null)
                {
                    ModelDescription = ModelManager.Instance().CurrentModel.ModelDescription;
                }
            }

            if (ModelDescription == null)
            {
                ModelDescription = ModelManager.Instance().CreateModelDescription();
            }

            switch (ModelFormType)
            {
                case ModelFormType.New:
                    Text = StringManager.GetString("New Model");
                    ModelDescription.Name = InitModelName;
                    // int count = modelManager.NewModelExistCount(name);
                    break;
                case ModelFormType.Edit:
                    Text = StringManager.GetString("Edit Model");
                    modelName.Enabled = false;
                    break;
                case ModelFormType.Copy:
                    Text = StringManager.GetString("Copy Model");
                    break;
            }

            SetModelData();
        }

        private void SetModelData()
        {
            modelName.Text = ModelDescription.Name;
            cmbCategory.Text = ModelDescription.Category;
            productName.Text = ModelDescription.ProductName;
            itemCode.Text = ModelDescription.ProductCode;
            description.Text = ModelDescription.Description;

            if (ExtraProperty != null)
            {
                ExtraProperty.Initialize(ModelDescription);
            }
        }

        private bool GetModelData()
        {
            ModelDescription.ProductCode = itemCode.Text;
            ModelDescription.Name = modelName.Text;
            ModelDescription.Category = cmbCategory.Text;
            ModelDescription.ProductName = productName.Text;
            ModelDescription.Description = description.Text;

            if (ExtraProperty != null)
            {
                if (ExtraProperty.GetModelData(ModelDescription) == false)
                {
                    return false;
                }
            }

            ModelManager.Instance().SaveModelDescription(ModelDescription);

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(modelName.Text))
            {
                return;
            }

            if (ModelFormType == ModelFormType.New)
            {
                if (ModelManager.Instance().IsModelExist(modelName.Text))
                {
                    MessageForm.Show(this, StringManager.GetString("The model name is exist. Please, use other model name."), "Error");
                    return;
                }
            }
            if (ModelDescription == null)
            {
                ModelDescription = ModelManager.Instance().CreateModelDescription();
            }

            bool ok = GetModelData();
            if (ok == false)
            {
                MessageForm.Show(this, StringManager.GetString("The model property is invalid."), "Error");
                return;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private int GetModelCount()
        {
            return 0;
        }
    }
}
