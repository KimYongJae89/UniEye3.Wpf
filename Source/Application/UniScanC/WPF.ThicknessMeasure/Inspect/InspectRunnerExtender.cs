using Authentication.Core;
using DynMvp.Data;
using DynMvp.InspectData;
using System;
using UniEye.Base.Config;
using UniScanC.Data;
using WPF.ThicknessMeasure.Data;

namespace WPF.ThicknessMeasure.Inspect
{
    public class InspectRunnerExtender : UniEye.Base.Inspect.InspectRunnerExtender
    {
        #region 메서드
        public override ProductResult CreateProductResult()
        {
            var measureResult = new ThicknessResult();
            measureResult.ResultPath = PathConfig.Instance().Result;

            return measureResult;
        }

        public override ProductResult BuildProductResult(string data)
        {
            return BuildProductResult();
        }

        public override ProductResult BuildProductResult()
        {
            ProductResult productResult = CreateProductResult();

            productResult.ModelName = ModelManager.Instance().CurrentModel.Name;
            productResult.InspectStartTime = DateTime.Now;
            productResult.JobOperator = UserHandler.Instance.CurrentUser?.UserId;

            return productResult;
        }
        #endregion
    }
}
