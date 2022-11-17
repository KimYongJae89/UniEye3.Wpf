using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public enum MilSystemType
    {
        Solios, Rapixo
    }

    public class CameraInfoMil : CameraInfo
    {
        public MilSystemType SystemType { get; set; }

        public uint SystemNum { get; set; }

        // InitializeDevice 후, boardType, boardId로 할당된 SystemId를 저장하는 변수
        public MilSystem MilSystem { get; set; }

        public uint DigitizerNum { get; set; }

        public CameraType CameraType { get; set; }

        public string DcfFilePath { get; set; }

        public CameraInfoMil()
        {
            GrabberType = GrabberType.MIL;
        }

        public CameraInfoMil(MilSystemType systemType, uint systemNum, uint digitizerNum, CameraType cameraType)
        {
            GrabberType = GrabberType.MIL;

            SystemType = systemType;
            SystemNum = systemNum;
            DigitizerNum = digitizerNum;
            CameraType = cameraType;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            SystemType = XmlHelper.GetValue(cameraElement, "SystemType", MilSystemType.Solios);
            SystemNum = (uint)XmlHelper.GetValue(cameraElement, "SystemNum", 0);
            DigitizerNum = (uint)XmlHelper.GetValue(cameraElement, "DigitizerNum", 0);
            CameraType = XmlHelper.GetValue(cameraElement, "CameraType", CameraType.PrimeTech_PXCB120VTH);
            DcfFilePath = XmlHelper.GetValue(cameraElement, "DcfFilePath", "");
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "SystemType", SystemType);
            XmlHelper.SetValue(cameraElement, "SystemNum", SystemNum);
            XmlHelper.SetValue(cameraElement, "DigitizerNum", DigitizerNum);
            XmlHelper.SetValue(cameraElement, "CameraType", CameraType);
            XmlHelper.SetValue(cameraElement, "DcfFilePath", DcfFilePath);
        }
    }

    public class MilSystem
    {
        public string SystemDescriptor { get; set; }
        public uint SystemNum { get; set; }
        public MIL_ID SystemId { get; set; }
    }

    public class GrabberMil : Grabber
    {
        private static List<MilSystem> milSystemList = new List<MilSystem>();

        public GrabberMil(GrabberInfo grabberInfo) : base(GrabberType.MIL, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "MIL Grabber is Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraMil();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new MatroxBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        private static string GetSystemDescriptor(MilSystemType systemType)
        {
            if (systemType == MilSystemType.Solios)
            {
                return MIL.M_SYSTEM_SOLIOS;
            }
            else if (systemType == MilSystemType.Rapixo)
            {
                return MIL.M_SYSTEM_RAPIXOCXP;
            }

            return MIL.M_SYSTEM_SOLIOS;
        }

        public static MilSystem GetMilSystem(MilSystemType systemType, uint systemNum)
        {
            string systemDescriptor = GetSystemDescriptor(systemType);

            return GetMilSystem(systemDescriptor, systemNum);

        }

        private static MilSystem GetMilSystem(string systemDescriptor, uint systemNum)
        {
            MilSystem milSystem = milSystemList.Find(x => x.SystemDescriptor == systemDescriptor && x.SystemNum == systemNum);
            if (milSystem == null)
            {
                milSystem = CreateMilSystem(systemDescriptor, systemNum);
            }

            return milSystem;
        }

        private static MilSystem CreateMilSystem(string systemDescriptor, uint systemNum)
        {
            MilSystem milSystem = null;

            MIL_ID systemId = MIL.M_NULL;
            //MIL 10.20 버전 함수
            //if (MIL.MsysAlloc(systemDescriptor, systemNum, MIL.M_DEFAULT, ref systemId) == MIL.M_NULL)
            //MIL 10.40 버전 함수
            if (MIL.MsysAlloc(MIL.M_DEFAULT, systemDescriptor, systemNum, MIL.M_DEFAULT, ref systemId) == MIL.M_NULL)
            {
                LogHelper.Error(string.Format("Can't Allocate MIL System. {0}, {1}", systemDescriptor, systemNum));
            }
            else
            {
                milSystem = new MilSystem();
                milSystem.SystemDescriptor = systemDescriptor;
                milSystem.SystemNum = systemNum;
                milSystem.SystemId = systemId;
            }

            return milSystem;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            // MatroxHelper.InitApplication();

            return true;
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Shutdown, "Release MilSystem");

            foreach (MilSystem milSystem in milSystemList)
            {
                MIL.MsysFree(milSystem.SystemId);
            }
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
