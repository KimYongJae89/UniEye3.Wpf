using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public interface IAlgorithmParamControl
    {
        AlgorithmValueChangedDelegate ValueChanged { get; set; }
        CommandManager CommandManager { get; set; }

        void Show();
        void Hide();

        string GetTypeName();

        void SelectProbe(ProbeList selectedProbeList);
        void ClearSelectedProbe();
        void UpdateProbeImage();
        void PointSelected(Point clickPos, ref bool processingCancelled);
    }

    public enum ValueChangedType
    {
        None, Position, ImageProcessing, Light
    }

    public delegate void AlgorithmValueChangedDelegate(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam, bool modified);
    public delegate void FiducialChangedDelegate(bool useFiducialProbe);
}
