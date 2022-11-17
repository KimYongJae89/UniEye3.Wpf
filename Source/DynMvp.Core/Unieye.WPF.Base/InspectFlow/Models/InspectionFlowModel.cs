using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public class InspectionFlowModel
    {
        public string Name { get; set; } = "New Inspector";

        public ObservableCollection<FlowAlgorithmModel> FlowAlgorithms { get; set; } = new ObservableCollection<FlowAlgorithmModel>();

        //private static InspectionFlowModel _instance;
        //public static InspectionFlowModel Instance => _instance ?? (_instance = new InspectionFlowModel());

        public InspectionFlowModel()
        {
        }

        public AlgoImage Inspect(AlgoImage AlgoImage)
        {
            if (AlgoImage == null)
            {
                return null;
            }

            AlgoImage srcImage = AlgoImage;
            foreach (FlowAlgorithmModel algorithm in FlowAlgorithms)
            {
                algorithm.BufferImage?.Dispose();
                algorithm.BufferImage = null;
                algorithm.UpdateParameter();
                srcImage = algorithm.Inspect(srcImage);
            }

            return srcImage;
        }

        public void UpdateParameter()
        {
            foreach (FlowAlgorithmModel algorithm in FlowAlgorithms)
            {
                algorithm.UpdateParameter();
            }
        }

        public bool UpdateBufferImage(AlgoImage[] bufferImages)
        {
            if (bufferImages.Length != FlowAlgorithms.Count)
            {
                return false;
            }

            for (int i = 0; i < FlowAlgorithms.Count; i++)
            {
                FlowAlgorithms[i].BufferImage = bufferImages[i];
            }

            return true;
        }
    }
}
