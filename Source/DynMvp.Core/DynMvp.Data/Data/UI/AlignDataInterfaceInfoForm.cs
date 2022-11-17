using DynMvp.Base;
using DynMvp.Data;
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
    public partial class AlignDataInterfaceInfoForm : Form
    {
        public AlignDataInterfaceInfo AlignDataInterfaceInfo { get; set; }

        public AlignDataInterfaceInfoForm()
        {
            InitializeComponent();

            groupBoxAlignDataInterfaceInfo.Text = StringManager.GetString(groupBoxAlignDataInterfaceInfo.Text);
            labelOffsetXAddress1.Text = StringManager.GetString(labelOffsetXAddress1.Text);
            labelOffsetYAddress1.Text = StringManager.GetString(labelOffsetYAddress1.Text);
            labelAngleAddress.Text = StringManager.GetString(labelAngleAddress.Text);
            labelOffsetXAddress2.Text = StringManager.GetString(labelOffsetXAddress2.Text);
            labelOffsetYAddress2.Text = StringManager.GetString(labelOffsetYAddress2.Text);
            label1.Text = StringManager.GetString(label1.Text);
            label2.Text = StringManager.GetString(label2.Text);
            label3.Text = StringManager.GetString(label3.Text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            AlignDataInterfaceInfo.OffsetXAddress1 = Convert.ToInt32(offsetXAddress1.Text);
            AlignDataInterfaceInfo.OffsetYAddress1 = Convert.ToInt32(offsetYAddress1.Text);
            AlignDataInterfaceInfo.AngleAddress = Convert.ToInt32(angleAddress.Text);
            AlignDataInterfaceInfo.OffsetXAddress2 = Convert.ToInt32(offsetXAddress2.Text);
            AlignDataInterfaceInfo.OffsetYAddress2 = Convert.ToInt32(offsetYAddress2.Text);
            AlignDataInterfaceInfo.XAxisCalibration = Convert.ToSingle(xAxisCalibration.Text);
            AlignDataInterfaceInfo.YAxisCalibration = Convert.ToSingle(yAxisCalibration.Text);
            AlignDataInterfaceInfo.RAxisCalibration = Convert.ToSingle(rAxisCalibration.Text);
        }

        private void AlignDataInterfaceInfoForm_Load(object sender, EventArgs e)
        {
            offsetXAddress1.Text = AlignDataInterfaceInfo.OffsetXAddress1.ToString();
            offsetYAddress1.Text = AlignDataInterfaceInfo.OffsetYAddress1.ToString();
            angleAddress.Text = AlignDataInterfaceInfo.AngleAddress.ToString();
            offsetXAddress2.Text = AlignDataInterfaceInfo.OffsetXAddress2.ToString();
            offsetYAddress2.Text = AlignDataInterfaceInfo.OffsetYAddress2.ToString();
            xAxisCalibration.Text = AlignDataInterfaceInfo.XAxisCalibration.ToString();
            yAxisCalibration.Text = AlignDataInterfaceInfo.YAxisCalibration.ToString();
            rAxisCalibration.Text = AlignDataInterfaceInfo.RAxisCalibration.ToString();
        }
    }
}
