using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.Dio.UI
{
    public partial class PortSettingPanel : UserControl
    {
        public PortSettingPanel()
        {
            InitializeComponent();
        }

        internal void UpdateData(DigitalIoInfo digitalIoInfo)
        {
            numInPortGroup.Value = digitalIoInfo.NumInPortGroup;
            inPortStartGroupIndex.Value = digitalIoInfo.InPortStartGroupIndex;
            numInPort.Value = digitalIoInfo.NumInPort;
            numOutPortGroup.Value = digitalIoInfo.NumOutPortGroup;
            outPortStartGroupIndex.Value = digitalIoInfo.OutPortStartGroupIndex;
            numOutPort.Value = digitalIoInfo.NumOutPort;
        }

        public void ApplyData(DigitalIoInfo digitalIoInfo)
        {
            digitalIoInfo.NumInPortGroup = (int)numInPortGroup.Value;
            digitalIoInfo.InPortStartGroupIndex = (int)inPortStartGroupIndex.Value;
            digitalIoInfo.NumInPort = (int)numInPort.Value;
            digitalIoInfo.NumOutPortGroup = (int)numOutPortGroup.Value;
            digitalIoInfo.OutPortStartGroupIndex = (int)outPortStartGroupIndex.Value;
            digitalIoInfo.NumOutPort = (int)numOutPort.Value;
        }
    }
}
