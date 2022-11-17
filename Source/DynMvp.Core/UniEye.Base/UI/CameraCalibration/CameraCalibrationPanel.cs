using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI.CameraCalibration
{
    internal delegate void SetPictureBoxImageDelegate(PictureBox pictureBox, Image image);
    internal delegate void SetLabelTextDelegate(Label label, string message);
    internal delegate void UpdateDataDelegate(Calibration calibration);

    internal interface CameraCalibrationPanel
    {
        void Initialize();
        void UpdateData(Calibration calibration);
        void UpdateResult(CalibrationResult result, int subResultId = -1);
        CalibrationResult Calibrate(Calibration calibration, ImageD imageD);
    }
}
