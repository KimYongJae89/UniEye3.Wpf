using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Spectrometer.Data
{
    public class Layer
    {
        public string Name { get; set; } = "Layer";
        public LayerParam Param { get; set; }
        public SpecRawData SpecRawData { get; set; }
        public SpecDividedData SpecDividedData { get; set; }
        public SpecThicknessData SpecThicknessData { get; set; }
        public double Thickness { get; set; } = 0;
        public double Refraction { get; set; } = 1;
        public double Angle { get; set; } = 0;

        private Layer() { }

        public Layer(string name, LayerParam layerParam)
        {
            Name = name;
            Param = new LayerParam();
            Param.CopyFrom(layerParam);

            SpecRawData = new SpecRawData();
            SpecDividedData = new SpecDividedData();
            SpecThicknessData = new SpecThicknessData();
            SpecThicknessData.Initialize(layerParam);
        }

        public Layer Clone()
        {
            var layer = new Layer();
            layer.CopyFrom(this);
            return layer;
        }

        public void CopyFrom(Layer layer)
        {
            Name = layer.Name;
            Param = layer.Param.Clone();
            SpecRawData = layer.SpecRawData.Clone();
            SpecDividedData = layer.SpecDividedData.Clone();
            SpecThicknessData = layer.SpecThicknessData.Clone();
            Thickness = layer.Thickness;
            Refraction = layer.Refraction;
            Angle = layer.Angle;
        }
    }
}
