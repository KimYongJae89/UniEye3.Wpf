using DynMvp.Device;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UmxService;

namespace UniEye.MachineInterface
{
    public interface IMachineInterface
    {
        void Init(AppProcessor appProcessor, string address);

        bool SetVisionState(string visionState, string value);

        void ImageAcquired();
        void CommandCompleted(string commandResult);
        bool GetVisionStatusDataTable(ref DataTable dataTable);
    }
}
