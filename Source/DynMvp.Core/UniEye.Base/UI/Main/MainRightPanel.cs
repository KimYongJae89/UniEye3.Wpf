using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class MainRightPanel : UserControl
    {
        public IRightPanel RightPanelIf { get; }

        public MainRightPanel()
        {
            InitializeComponent();

            RightPanelIf = UiManager.Instance().CreateRightPanel();
        }

        public void Initialize()
        {
            if (RightPanelIf == null)
            {
                return;
            }

            controlPanel.Controls.Add((UserControl)RightPanelIf);
            ((UserControl)RightPanelIf).Dock = DockStyle.Fill;

            RightPanelIf.Initialize();
        }
    }
}
