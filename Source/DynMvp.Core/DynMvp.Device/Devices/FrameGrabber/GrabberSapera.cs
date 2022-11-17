using DALSA.SaperaLT.SapClassBasic;
using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoSapera : CameraInfo
    {
        public enum EClientType { Master, Slave }
        public enum EScanDirectionType { Forward, Reverse }
        public enum EEncMultiply { E01 = 1, E02 = 2, E04 = 4, E08 = 8, E16 = 16, E32 = 32 }
        public enum EFrameTirggerMode { None, FrameTrigger, RisingSnap, RisingGrabFallingFreeze }
        public enum ETDIMode { TdiMTF, Tdi }

        [Category("CameraInfoSapera"), Description("ClientType")]
        public EClientType ClientType { get; set; }

        [Category("CameraInfoSapera"), Description("ScanDirectionType")]
        public EScanDirectionType ScanDirectionType { get; set; }

        [Category("CameraInfoSapera"), Description("frameTirgger")]
        public EFrameTirggerMode FrameTirggerMode { get; set; }

        [Category("CameraInfoSapera"), Description("TDI Mode")]
        public ETDIMode TdiMode { get; set; }

        [Category("CameraInfoSapera"), Description("Area Mode Width")]
        public int TdiAreaWidth { get; set; }

        [Category("CameraInfoSapera"), Description("Area Mode Heigth")]
        public int TdiAreaHeight { get; set; }

        [Category("CameraInfoSapera"), Description("Encoder Drop")]
        public int EncDrop { get; set; }

        [Category("CameraInfoSapera"), Description("Encoder Multiply")]
        public EEncMultiply EncMultiply { get; set; }

        [Category("CameraInfoSapera"), Description("CCF file")]
        public string CcfFIlePath { get; set; }

        public CameraInfoSapera()
        {
            GrabberType = GrabberType.Sapera;
            useNativeBuffering = true;
            width = 32768;
            height = 65535;
            frameNum = 5;

            FrameTirggerMode = EFrameTirggerMode.None; ;
            TdiAreaWidth = 16384;
            TdiAreaHeight = 128;

            ClientType = EClientType.Master;
            ScanDirectionType = EScanDirectionType.Forward;
            EncDrop = 0;
            EncMultiply = EEncMultiply.E01;
            CcfFIlePath = "";
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            FrameTirggerMode = XmlHelper.GetValue(cameraElement, "FrameTirggerMode", FrameTirggerMode);
            TdiAreaWidth = XmlHelper.GetValue(cameraElement, "TdiAreaWidth", TdiAreaWidth);
            TdiAreaHeight = XmlHelper.GetValue(cameraElement, "TdiAreaHeight", TdiAreaHeight);
            TdiMode = XmlHelper.GetValue(cameraElement, "TdiMode", TdiMode);

            ClientType = XmlHelper.GetValue(cameraElement, "ClientType", ClientType);
            ScanDirectionType = XmlHelper.GetValue(cameraElement, "ScanDirectionType", ScanDirectionType);
            EncDrop = XmlHelper.GetValue(cameraElement, "EncDrop", EncDrop);
            EncMultiply = XmlHelper.GetValue(cameraElement, "EncMultiply", EncMultiply);
            CcfFIlePath = XmlHelper.GetValue(cameraElement, "CcfFIlePath", CcfFIlePath);
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "FrameTirggerMode", FrameTirggerMode);
            XmlHelper.SetValue(cameraElement, "TdiAreaWidth", TdiAreaWidth);
            XmlHelper.SetValue(cameraElement, "tdiAreaHeight", TdiAreaHeight);
            XmlHelper.SetValue(cameraElement, "TdiMode", TdiMode);

            XmlHelper.SetValue(cameraElement, "ClientType", ClientType);
            XmlHelper.SetValue(cameraElement, "ScanDirectionType", ScanDirectionType);
            XmlHelper.SetValue(cameraElement, "EncDrop", EncDrop);
            XmlHelper.SetValue(cameraElement, "EncMultiply", EncMultiply);
            XmlHelper.SetValue(cameraElement, "CcfFIlePath", CcfFIlePath);
        }
    }

    public class GrabberSapera : Grabber
    {
        private static int cntOpenDriver = 0;

        public GrabberSapera(GrabberInfo grabberInfo) : base(GrabberType.Sapera, grabberInfo)
        {

        }

        public override Camera CreateCamera()
        {
            return new CameraSapera();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new SaperaCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

#if !DEBUG
            SapManager.DisplayStatusMode = SapManager.StatusMode.Exception;
#endif

            if (cntOpenDriver == 0 && false)
            {
                // Reset Server
                var isResetDone = new Dictionary<int, bool>();
                var mse = new System.Threading.ManualResetEvent(false);

                SapManager.EndReset += new SapResetHandler((s, e) =>
                {
                    isResetDone[e.ServerIndex] = true;
                    if (isResetDone.Values.All(f => f))
                    {
                        mse.Set();
                    }
                });

                try
                {
                    int totalServerCnt = SapManager.GetServerCount(SapManager.ResourceType.Acq);
                    for (int i = 0; i < totalServerCnt; i++)
                    {
                        string serverName = SapManager.GetServerName(i, SapManager.ResourceType.Acq);
                        int serverIndex = SapManager.GetServerIndex(serverName);
                        isResetDone.Add(serverIndex, false);
                        SapManager.ResetServer(serverIndex, false);
                    }

                    bool waitOne = mse.WaitOne(5000);
                    if (!waitOne)
                    {
                        throw new Exception("GrabberSapera::Initialize - Grabber Reset Timeout.");
                    }

                    SapManager.Open();
                }
                catch (Exception ex)
                {
                    string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                    //System.Windows.Forms.MessageBox.Show(message, "UniEye3Wpf", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    throw new CameraInitializeFailException("Sapera Exception : " + ex.Message);
                }
            }
            cntOpenDriver++;
            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;
            if (cntOpenDriver == 0)
            {
                SapManager.Close();
            }
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
