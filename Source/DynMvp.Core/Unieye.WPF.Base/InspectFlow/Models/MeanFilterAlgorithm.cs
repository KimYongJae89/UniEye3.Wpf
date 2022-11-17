using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public class MeanFilterAlgorithm : FlowAlgorithmModel
    {
        [FlowAlgorithmParameter]
        public int FilterSize { get; set; } = 3;

        public override AlgoImage Inspect(AlgoImage srcImage)
        {
            if (BufferImage == null)
            {
                BufferImage = CloneImage(srcImage);
            }

            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(ImagingLibrary);
            imageProcessing.Median(srcImage, BufferImage, FilterSize);

            return BufferImage;
        }
    }
}
