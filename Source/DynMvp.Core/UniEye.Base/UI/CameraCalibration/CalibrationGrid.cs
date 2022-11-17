using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.UI.CameraCalibration
{
    public partial class CalibrationGrid : UserControl, CameraCalibrationPanel, IMultiLanguageSupport
    {
        public CalibrationGrid()
        {
            InitializeComponent();
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public CalibrationResult Calibrate(Calibration calibration, ImageD imageD)
        {
            CalibrationResult result = null;
            if (calibrationTypeGrid.Checked)
            {
                result = calibration.Calibrate(imageD, CalibrationGridType.Dots, (int)numRow.Value, (int)numCol.Value, Convert.ToSingle(rowSpace.Text), Convert.ToSingle(colSpace.Text));
            }
            else
            {
                result = calibration.Calibrate(imageD, CalibrationGridType.Chessboard, (int)numRow.Value, (int)numCol.Value, Convert.ToSingle(rowSpace.Text), Convert.ToSingle(colSpace.Text));
            }

            return result;
        }

        //public void ChangeLanguage()
        //{
        //    calibrationTypeGrid.Text = StringManager.GetString(calibrationTypeGrid.Text);
        //    calibrationTypeChessboard.Text = StringManager.GetString(calibrationTypeChessboard.Text);
        //    labelNumRow.Text = StringManager.GetString(labelNumRow.Text);
        //    labelRowSpace.Text = StringManager.GetString(labelRowSpace.Text);
        //    labelNumCol.Text = StringManager.GetString(labelNumCol.Text);
        //    labelColSpace.Text = StringManager.GetString(labelColSpace.Text);
        //}

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void UpdateData(Calibration calibration)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData), calibration);
                return;
            }
            //numRow.Value = calibration.PatternCount.Width;
            //numCol.Value = calibration.PatternCount.Height;
            //rowSpace.Text = calibration.PatternSize.Width.ToString();
            //colSpace.Text = calibration.PatternSize.Height.ToString();
        }

        public void UpdateResult(CalibrationResult result, int subResultId = -1)
        {
            return;
            throw new NotImplementedException();
        }
    }
}
