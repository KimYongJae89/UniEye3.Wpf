using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Views
{
    public interface IShellWindow
    {
        void Initialize();
        void ShowTab(UniEye.Base.UI.TabKey tabKey);
        void EnableTab(UniEye.Base.UI.TabKey tabKey, bool enable);
    }
}
