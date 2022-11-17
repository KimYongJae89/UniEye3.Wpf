using DynMvp.InspectData;
using UniEye.Base.Config;
using UniScanC.Data;

namespace UniScanC.Inspect
{
    public class InspectRunnerExtender : UniEye.Base.Inspect.InspectRunnerExtender
    {
        public string LotNo { get; private set; }

        public override ProductResult CreateProductResult()
        {
            var inspectResult = new InspectResult();
            inspectResult.LotNo = LotNo;
            inspectResult.Judgment = DynMvp.Vision.Judgment.OK;
            inspectResult.ResultPath = PathConfig.Instance().Result;

            return inspectResult;
        }

        public override ProductResult BuildProductResult(string data)
        {
            LotNo = data;
            return base.BuildProductResult(data);
        }
    }
}
