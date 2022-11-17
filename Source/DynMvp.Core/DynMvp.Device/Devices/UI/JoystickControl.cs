using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public delegate bool MovableCheckDelegate();
    public interface IJoystickControl
    {
        void InitControl();
        void Initialize(AxisHandler axisHandler);
        void MoveAxis(int axisNo, int direction);
        void StopAxis();

        void SetMovableCheckDelegate(MovableCheckDelegate movableCheckDelegate);
        MovableCheckDelegate GetMovableCheckDelegate();
    }
}
