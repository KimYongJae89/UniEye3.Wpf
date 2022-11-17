using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Enums
{
    public enum ERoleType
    {
        ModelPage = 0x0001,
        InspectPage = 0x0002,
        TeachPage = 0x0004,
        ReportPage = 0x0008,

        SettingPage = 0x0010,
        ModelManage = 0x0020,
        DetailTeaching = 0x0040,
        GeneralSetting = 0x0080,

        DetailSetting = 0x0100,
        DeviceSetting = 0x0200,
        UserSetting = 0x0400,
        TaskSetting = 0x0800,
    };
}
