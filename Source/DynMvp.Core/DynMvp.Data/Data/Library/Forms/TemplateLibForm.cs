using DynMvp.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Data.Library.Forms
{
    public partial class TemplateLibForm : Form
    {
        #region Internal member variables

        private DrawBox drawBox;
        private ImageList treeImageList;

        #endregion

        #region Properties


        #endregion


        public TemplateLibForm()
        {
            InitializeComponent();

            treeImageList = new ImageList();

            treeImageList.Images.Add(Properties.Resources.Folder);
            treeImageList.Images.Add(Properties.Resources.FolderOpen);
            treeImageList.Images.Add(Properties.Resources.LibraryPart);

            tvwLibCategory.ImageList = treeImageList;

            SuspendLayout();

            drawBox = new DrawBox();
            drawBox.BorderStyle = BorderStyle.None;
            drawBox.Dock = DockStyle.Fill;
            drawBox.Name = "drawBox";
            drawBox.TabStop = false;
            drawBox.BackColor = SystemColors.Window;

            pnlLibraryFigureView.Controls.Add(drawBox);

            ResumeLayout(false);
        }

        private void LoadLibrary()
        {
            tvwLibCategory.Nodes.Clear();
            dgLibList.DataSource = null;
            rtxTemplateSummary.Text = "";

            PopulateTreeView(0, null);

            return;
        }

        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            List<TreeViewItem> treeItemList = LibraryManager.Instance().GetTreeItemList();

            if (treeItemList.Count < 1)
            {
                return;
            }

            IEnumerable<TreeViewItem> filteredItems = treeItemList.Where(item => item.ParentId == parentId);

            TreeNode childNode;

            foreach (TreeViewItem i in filteredItems.ToList())
            {
                if (parentNode == null)
                {
                    childNode = tvwLibCategory.Nodes.Add(i.Name, i.Name, i.ImageIndex, i.SelectedIndex);
                }
                else
                {
                    childNode = parentNode.Nodes.Add(i.Name, i.Name, i.ImageIndex, i.SelectedIndex);
                }

                childNode.Tag = i.TemplateLibGroup;
                PopulateTreeView(i.Id, childNode);
            }
        }

        private TreeNode PopulateTemplateGroup(DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Exists == false)
            {
                return null;
            }

            var libraryMgr = LibraryManager.Instance();
            var node = new TreeNode(directoryInfo.Name, 0, 0);
            node.Tag = directoryInfo;
            libraryMgr.AddTemplateGroup(directoryInfo);

            foreach (DirectoryInfo sub in directoryInfo.GetDirectories())
            {
                node.Nodes.Add(PopulateTemplateGroup(sub));
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Exists)
                {
                    var childNode = new TreeNode(file.Name, 2, 2);
                    node.Nodes.Add(childNode);

                    var template = new Template();
                    libraryMgr.AddTemplate(directoryInfo, template);
                }
            }

            return node;
        }

        private void BindLibraryList(TreeNode node)
        {
            if (node.Tag == null || node.Tag is TemplateGroup == false)
            {
                return;
            }

            if (!(node.Tag is TemplateGroup templateGroup))
            {
                return;
            }

            dgLibList.DataSource = templateGroup.TemplateList;
            dgLibList.ClearSelection();
        }

        private void AddNewCategory()
        {
            if (tvwLibCategory.Focused && tvwLibCategory.SelectedNode != null)
            {
                TreeNode selectedNode = tvwLibCategory.SelectedNode;

                if (selectedNode != null && selectedNode.Tag != null)
                {
                    var form = new AddLibraryCategoryForm();

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        string categoryName = form.txtCategoryName.Text.Trim();

                        var templateGroup = selectedNode.Tag as TemplateGroup;
                        string path = Path.Combine(templateGroup.DirInfo.FullName, categoryName);

                        var newGroup = new TemplateGroup(categoryName);
                        newGroup.DirInfo = Directory.CreateDirectory(path);

                        LibraryManager.Instance().AddTemplateGroup(newGroup.DirInfo);

                        var newNode = new TreeNode(categoryName, 0, 0);
                        newNode.Tag = newGroup;
                        selectedNode.Nodes.Add(newNode);
                        selectedNode.Expand();
                    }
                }
            }
        }

        private void DeleteTemplateGroupNode()
        {
            TreeNode selectedNode = tvwLibCategory.SelectedNode;

            if (selectedNode != null && selectedNode.Tag != null)
            {
                var libManager = LibraryManager.Instance();
                var templateGroup = selectedNode.Tag as TemplateGroup;

                if (templateGroup.DirInfo.FullName == libManager.RootPath)
                {
                    MessageBox.Show("Can't delete root category.");
                    return;
                }

                if (templateGroup.HasTemplateLibrary)
                {
                    MessageBox.Show("Can't delete category because it has child nodes.");
                    return;
                }

                libManager.RemoveTemplateGroup(templateGroup);

                templateGroup.DirInfo.Delete();
                tvwLibCategory.Nodes.Remove(selectedNode);
            }
        }

        private void DeleteTemplateLibraryNode()
        {
            if (dgLibList.CurrentRow != null)
            {
                var template = dgLibList.CurrentRow.DataBoundItem as Template;

                template.FileInfo.Delete();

                LibraryManager.Instance().RemoveTemplate(template, template.FileInfo.Directory);
                TemplateGroup group = LibraryManager.Instance().GetTemplateGroup(template.FileInfo.Directory);

                dgLibList.DataSource = null;
                dgLibList.DataSource = group.TemplateList;

                drawBox.FigureGroup.Clear();
                drawBox.Invalidate();
            }
        }

        private void ultraToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "tbbtnNewCategory":
                    {
                        AddNewCategory();
                    }
                    break;
                case "tbbtnDeleteItem":
                    {
                        if (tvwLibCategory.Focused)
                        {
                            DeleteTemplateGroupNode();
                        }
                        else if (dgLibList.Focused)
                        {
                            DeleteTemplateLibraryNode();
                        }
                    }
                    break;
                case "tbbtnRenameItem":
                    break;
                case "tbbtnCopyItem":
                    break;
                case "tbbtnPasteItem":
                    break;
                case "tbbtnCutItem":
                    break;
                case "tbbtnCopyPath":
                    CopyLibraryPath();
                    break;
                case "tbbtnExploreWnd":
                    break;
                case "tbbtnPreviewWnd":
                    break;
                case "tbbtnDetailWnd":
                    break;
                case "tbbtnLayoutBig":
                    break;
                case "tbbtnLayoutDetail":
                    break;
                case "tbbtnRefresh":
                    {
                        LibraryManager.Instance().Load();
                        LoadLibrary();
                    }
                    break;
                default:
                    break;
            }
        }

        private void CopyLibraryPath()
        {
            if (tvwLibCategory.Focused && tvwLibCategory.SelectedNode != null)
            {
                var templateGroup = tvwLibCategory.SelectedNode.Tag as TemplateGroup;
                Clipboard.SetText(templateGroup.DirInfo.FullName);
            }
            else if (dgLibList.Focused && dgLibList.CurrentRow != null)
            {
                var template = dgLibList.CurrentRow.DataBoundItem as Template;
                Clipboard.SetText(template.FileInfo.DirectoryName);
            }
        }

        private void tvwLibCategory_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        private void tvwLibCategory_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        private void tvwLibCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BindLibraryList(e.Node);
        }

        private void tvwLibCategory_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode selectedNode = tvwLibCategory.GetNodeAt(e.X, e.Y);

                if (selectedNode != null)
                {
                    if (selectedNode.Level == 0)
                    {
                        menuRename.Enabled = false;
                        menuDeleteItem.Enabled = false;
                    }

                    ctxLibraryTreeMenu.Show(tvwLibCategory, new Point(e.X, e.Y));
                }
            }
        }

        private void menuNewCategory_Click(object sender, EventArgs e)
        {
            AddNewCategory();
        }

        private void menuDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteTemplateGroupNode();
        }

        private void menuRename_Click(object sender, EventArgs e)
        {

        }

        private void menuDeleteTemplate_Click(object sender, EventArgs e)
        {
            DeleteTemplateLibraryNode();
        }

        private void menuRenameTemplate_Click(object sender, EventArgs e)
        {

        }

        private void DispLibrarySummary(Template template)
        {
            rtxTemplateSummary.Text = "";

            var sb = new StringBuilder();
            sb.AppendFormat("Name\t: {0}", template.Name);
            var targetRect = template.Target.BaseRegion.ToRectangle();
            sb.AppendFormat("\nWidth\t: {0}\nHeight\t: {1}", targetRect.Width, targetRect.Height);

            rtxTemplateSummary.Text = sb.ToString();
        }

        private void DrawLibraryFigure(Template template)
        {
            drawBox.FigureGroup.Clear();
            template.Target.UpdateRegion();

            foreach (Probe probe in template.Target.ProbeList)
            {
                var figure = new DynMvp.UI.RectangleFigure(probe.BaseRegion, "");
                drawBox.FigureGroup.AddFigure(figure);
            }

            var rect = new RectangleF();
            rect = drawBox.FigureGroup.GetRectangle().ToRectangleF();
            rect.Inflate(5, 5);

            drawBox.DisplayRect = rect;
            drawBox.ZoomFit();
            drawBox.Invalidate();
        }

        private void pnlDrawingTemplate_Resize(object sender, EventArgs e)
        {
            drawBox.ZoomFit();
        }

        private void TemplateLibForm_Load(object sender, EventArgs e)
        {
            dgLibList.AutoGenerateColumns = false;
        }

        private void tlpnlLibraryInfo_Resize(object sender, EventArgs e)
        {
            drawBox.ZoomFit();
        }

        private void dgLibList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgLibList.CurrentRow.Selected = false;

                int clickedRowIndex = dgLibList.HitTest(e.X, e.Y).RowIndex;
                dgLibList.Rows[clickedRowIndex].Selected = true;

                var template = dgLibList.Rows[clickedRowIndex].DataBoundItem as Template;

                DrawLibraryFigure(template);
                DispLibrarySummary(template);

                ctxTemplateGridViewMenu.Show(dgLibList, new Point(e.X, e.Y));
            }
            else
            {
                if (dgLibList.CurrentRow != null)
                {
                    var template = dgLibList.CurrentRow.DataBoundItem as Template;

                    DrawLibraryFigure(template);
                    DispLibrarySummary(template);
                }
            }
        }
    }
}
