using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelManager
    {
        private List<DaqChannel> daqChannelList = new List<DaqChannel>();

        private static DaqChannelManager _instance = null;

        private DaqChannelManager()
        {
        }

        public static DaqChannelManager Instance()
        {
            if (_instance == null)
            {
                _instance = new DaqChannelManager();
            }

            return _instance;
        }

        public DaqChannel CreateDaqChannel(DaqChannelType daqChannelType, string name, bool isVirtual)
        {
            DaqChannel daqChannel;

            switch (daqChannelType)
            {
                case DaqChannelType.Daqmx:
                    daqChannel = new DaqChannelNiDaqmx();
                    break;
                case DaqChannelType.MeDAQ:
                    daqChannel = new DaqChannelMedaq();
                    break;
                default:
                    daqChannel = new DaqChannelVirtual();
                    break;
            }

            if (isVirtual)
            {
                daqChannel = new DaqChannelVirtual();
            }

            daqChannel.Name = name;
            daqChannelList.Add(daqChannel);

            return daqChannel;
        }

        public DaqChannel GetDaqChannel(string name)
        {
            foreach (DaqChannel daqChannel in daqChannelList)
            {
                if (daqChannel.Name == name)
                {
                    return daqChannel;
                }
            }

            return null;
        }

        public void FillComboDaqChannel(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            comboBox.Items.Add("None");

            foreach (DaqChannel daqChannel in daqChannelList)
            {
                comboBox.Items.Add(daqChannel.Name);
            }
        }
    }
}
