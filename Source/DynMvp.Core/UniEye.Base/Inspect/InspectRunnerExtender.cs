using Authentication.Core;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.BarcodeReader;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.Inspect
{
    public delegate void UpdateGrabImageDelegate();

    public class InspectRunnerExtender
    {
        protected InspectEventHandler inspectEventHandler;
        protected InspectRunner inspectRunner;
        public InspectRunner InspectRunner
        {
            set
            {
                inspectRunner = value;
                inspectEventHandler = inspectRunner.InspectEventHandler;
            }
            get => inspectRunner;
        }

        public virtual int GetInspectStepNo() { return 0; }

        public virtual ProductResult CreateProductResult()
        {
            return new ProductResult();
        }

        public virtual string GetInspectNo(ProductResult productResult)
        {
            return productResult.InspectStartTime.ToString("yyyyMMddHHmmssfff");
        }

        public virtual ProductResult BuildProductResult()
        {
            ProductResult productResult = CreateProductResult();

            productResult.ModelName = ModelManager.Instance().CurrentModel.Name;

            productResult.InspectStartTime = DateTime.Now;
            productResult.InspectionNo = GetInspectNo(productResult);
            productResult.JobOperator = UserHandler.Instance.CurrentUser.UserId;
            productResult.ModelName = ModelManager.Instance().CurrentModel.Name;

            string shortTime = productResult.InspectStartTime.ToString("yyyyMMdd");
            string hourStr = productResult.InspectStartTime.ToString("HH");

            productResult.InputBarcode = "";

            productResult.ResultPath = PathManager.GetResultPath(ModelManager.Instance().CurrentModel.Name, productResult.InspectStartTime, productResult.InspectionNo);

            LogHelper.Debug(LoggerType.Inspection, string.Format("Create Inspection Result {0}", productResult.InspectionNo));

            return productResult;
        }

        public virtual ProductResult BuildProductResult(string data)
        {
            return CreateProductResult();
        }

        public virtual void DoInspect(int triggerIndex)
        {

        }

        public virtual void DoScan(int triggerIndex, string scanImagePath)
        {

        }

        public virtual StepResult DoStepInspect(int stepNo)
        {
            return new StepResult();
        }

        public virtual StepResult DoStepScan(int stepNo, string scanImagePath)
        {
            return new StepResult();
        }
    }
}
