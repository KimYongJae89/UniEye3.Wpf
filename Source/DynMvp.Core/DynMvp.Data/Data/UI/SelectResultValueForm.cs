using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class SelectResultValueForm : Form
    {
        public string ObjectName { get; private set; } = "";
        public string ValueName { get; private set; } = "";

        private StepModel model;
        public StepModel Model
        {
            set => model = value;
        }

        private ObjectTree objectTree;

        public SelectResultValueForm()
        {
            InitializeComponent();

            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            objectTree = new ObjectTree();

            // 
            // objectTree
            // 
            objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            objectTree.Location = new System.Drawing.Point(0, 0);
            objectTree.Margin = new System.Windows.Forms.Padding(5);
            objectTree.Name = "objectTree";
            objectTree.Size = new System.Drawing.Size(294, 468);
            objectTree.TabIndex = 0;
            objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(objectTree_AfterSelect);

            // 
            // splitContainer.Panel1
            // 

            panelTree.Controls.Add(objectTree);

            buttonOk.Enabled = false;
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ObjectName = "";

            object obj = objectTree.SelectedNode.Tag;
            if (obj != null)
            {
                if (obj is string)
                {
                    TreeNode parentNode = objectTree.SelectedNode.Parent;
                    if (parentNode.Tag != null)
                    {
                        ObjectName = ((Probe)parentNode.Tag).FullId;
                        ValueName = obj.ToString();
                    }
                }
                else if (obj is Probe)
                {
                    ObjectName = ((Probe)obj).FullId;
                    ValueName = "Result";
                }
                else if (obj is Target)
                {
                    ObjectName = ((Target)obj).Name;
                    ValueName = "Result";
                }
            }

            if (ObjectName != "")
            {
                buttonOk.Enabled = true;
            }
            else
            {
                buttonOk.Enabled = false;
            }
        }

        private void SelectResultValueForm_Load(object sender, EventArgs e)
        {
            objectTree.Initialize(model);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
