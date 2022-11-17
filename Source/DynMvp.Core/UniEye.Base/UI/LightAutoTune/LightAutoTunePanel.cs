using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI.LightAutoTune
{
    public partial class LightAutoTunePanel : UserControl
    {
        public int DeviceIndex { get; }

        public LightAutoTunePanel(int deviceIndex)
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            DeviceIndex = deviceIndex;
            labelTitle.Text = string.Format("Cam {0}", deviceIndex);
        }

        public void UpdateData(int lightValue, float std)
        {
            valueList.Rows.Add(lightValue, std);
            valueList.Sort(valueList.Columns[1], System.ComponentModel.ListSortDirection.Descending);
        }

        public void Clear()
        {
            valueList.Rows.Clear();
        }
    }
}
