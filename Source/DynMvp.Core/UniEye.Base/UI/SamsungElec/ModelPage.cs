using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.UI;

namespace UniEye.Base.UI.SamsungElec
{
    public partial class ModelPage : UserControl, IMainTabPage
    {
        public bool ModelSelected { get; } = false;
        public Control ShowHideControl { get; set; }

        public ModelPage()
        {
            InitializeComponent();
        }

        public TabKey TabKey => TabKey.Model;

        public string TabName => "Model";

        public bool IsAdminPage => false;

        public Bitmap TabIcon => global::UniEye.Base.Properties.Resources.Document_model_gray_36;

        public Bitmap TabSelectedIcon => global::UniEye.Base.Properties.Resources.Document_model_black_36;

        public Color TabSelectedColor => Color.YellowGreen;

        public Uri Uri => throw new NotImplementedException();

        public void ChangeCaption()
        {
            labelModelList.Text = StringManager.GetString(labelModelList.Text);
        }

        private void ModelManagePage_Load(object sender, EventArgs e)
        {
        }

        public void Initialize()
        {
            ModelManager.Instance().Refresh();
            RefreshModelList();
        }

        public void OnIdle()
        {

        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            var newModelForm = new ModelForm();
            newModelForm.ModelFormType = ModelFormType.New;
            if (newModelForm.ShowDialog(this) == DialogResult.OK)
            {
                newModelForm.ModelDescription.CreatedDate = DateTime.Now;
                newModelForm.ModelDescription.ModifiedDate = DateTime.Now;

                ModelManager.Instance().AddModel(newModelForm.ModelDescription);
                ModelManager.Instance().CreateModel(newModelForm.ModelDescription);

                RefreshModelList();
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count == 0)
            {
                return;
            }

            string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

            var copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.Edit;

            ModelDescription srcModelDesc = ModelManager.Instance().GetModelDescription(modelName);
            if (srcModelDesc == null)
            {
                Debug.Assert(false, string.Format("Model Description is null : {0}", modelName));
                return;
            }

            copyModelForm.ModelDescription = srcModelDesc.Clone();

            if (copyModelForm.ShowDialog(this) == DialogResult.OK)
            {
                srcModelDesc.CopyTo(copyModelForm.ModelDescription);
                copyModelForm.ModelDescription.ModifiedDate = DateTime.Now;
            }
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            var copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.Copy;

            if (modelList.SelectedRows.Count != 0)
            {
                var srcDescription = ((ModelDescription)modelList.SelectedRows[0].Tag);
                string modelName = srcDescription.Name;

                var modelManager = ModelManager.Instance();

                modelManager.CloseModel();
                //ModelBase srcModel = ModelManager.Instance().OpenModel(modelName, null);
                ModelBase srcModel = modelManager.CreateModel();
                srcModel.ModelPath = modelManager.GetModelPath(modelName);
                if (!srcModel.OpenModel(null))
                {
                    return;
                }

                if (srcModel == null)
                {
                    return;
                }

                if (copyModelForm.ShowDialog(this) == DialogResult.OK)
                {
                    ModelBase dstModel = srcModel.Clone();
                    dstModel.ModelDescription = copyModelForm.ModelDescription;
                    dstModel.ModelDescription.CreatedDate = DateTime.Now;
                    dstModel.ModelDescription.ModifiedDate = DateTime.Now;
                    dstModel.ModelPath = ModelManager.Instance().GetModelPath(dstModel.ModelDescription.Name);
                    dstModel.SaveModel();

                    ModelManager.Instance().AddModel(dstModel.ModelDescription);

                    RefreshModelList();

                    // 새로 복사한 모델을 연다.
                    modelManager.OpenModel(dstModel.Name, null);
                }
            }
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count != 0)
            {
                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;
                ModelManager.Instance().OpenModel(modelName, null);
            }
        }

        //delegate void SelectModelDelegate(string modelName, bool remoteCall);
        //public void SelectModel(string modelName, bool remoteCall)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke(new SelectModelDelegate(SelectModel), modelName, remoteCall);
        //        return;
        //    }

        //    ModelManager.Instance().OpenModel(modelName, null);
        //}

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in modelList.SelectedRows)
            {
                string modelName = ((ModelDescription)dataGridViewRow.Tag).Name;

                DialogResult dialogResult = DialogResult.No;
                dialogResult = DynMvp.UI.MessageForm.Show(ParentForm, string.Format("Do you want to delete the selected model {0}?", modelName), "Delete Model", DynMvp.UI.MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ModelManager.Instance().DeleteModel(modelName);
                    RefreshModelList();

                    break;
                }
            }
        }

        private delegate void DeleteModelDelegate(string modelName);

        public void DeleteModel(string modelName)
        {
            if (InvokeRequired)
            {
                Invoke(new DeleteModelDelegate(DeleteModel), modelName);
                return;
            }

            ModelBase curModel = ModelManager.Instance().CurrentModel;
            if (curModel != null)
            {
                if (curModel.Name == modelName)
                {
                    ModelManager.Instance().CloseModel();
                }
            }
        }

        private void RefreshModelList(string searchText = "")
        {
            modelList.Rows.Clear();

            int index = 0;
            string lastModifiedStr = "";

            foreach (ModelDescription modelDescription in ModelManager.Instance())
            {
                if (searchText == "" || modelDescription.Name.Contains(searchText))
                {
                    var directoryInfo = new DirectoryInfo(ModelManager.Instance().GetModelPath(modelDescription.Name));

                    lastModifiedStr = modelDescription.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");

                    int curIndex = modelList.Rows.Add(index + 1, modelDescription.Name, modelDescription.Owner,
                            modelDescription.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"), lastModifiedStr, modelDescription.Description);
                    modelList.Rows[index].Tag = modelDescription;

                    index++;
                }
            }

            totalModel.Text = string.Format("Total : {0}", index);

            modelList.Sort(modelList.Columns[4], System.ComponentModel.ListSortDirection.Descending);

            for (int i = 0; i < modelList.RowCount; i++)
            {
                modelList.Rows[i].Cells[0].Value = i + 1;
            }

            if (modelList.Rows.Count > 0)
            {
                modelList.Rows[0].Selected = true;
            }
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visible)
        {
            if (visible)
            {
                ModelManager.Instance().Refresh();
                RefreshModelList();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            RefreshModelList(findModel.Text);
        }

        private void FindModel_TextChanged(object sender, EventArgs e)
        {
            modelList.Rows.Clear();
            RefreshModelList(findModel.Text);
        }

        private void ModelList_SelectionChanged(object sender, EventArgs e)
        {

        }

        public void ProcessKeyDown(KeyEventArgs e)
        {

        }

        private void ModelList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string modelName = ((ModelDescription)modelList.Rows[e.RowIndex].Tag).Name;
                ModelManager.Instance().OpenModel(modelName, null);
            }
        }
    }
}
