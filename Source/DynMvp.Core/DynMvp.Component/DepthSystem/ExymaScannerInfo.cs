using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Component.DepthSystem
{
    public enum MeasureMode
    {
        SIngleWavePmp, DoubleWavePmp, TripleWavePmp, SIngleWavePmpGrayCode, DoubleWavePmpGrayCode, TripleWavePmpGrayCode
    }

    public enum MeasureTriggerMode
    {
        None, CameraIo, UartStepByStep, UartSequential
    }

    //enum CameraType
    //{
    //    None, BaslerAca, Marline
    //}

    public enum ControlBoardType
    {
        None, Legacy, Ver2013
    }

    public enum EEPROM
    {
        S1EN,   //0 sequence1 enable
        S1BK,   //sequence1 Bucket
        S1T1,   //sequence1 T1
        S1T2,   //sequence1 T2
        S1T3,   //sequence1 T3

        S2EN,
        S2BK,
        S2T1,
        S2T2,
        S2T3,

        S3EN,
        S3BK,
        S3T1,
        S3T2,
        S3T3,

        S4EN,
        S4BK,
        S4T1,
        S4T2,
        S4T3, //19

        EPMD, //20expose Mode ( time, SyncCount)
        EPMS, //21expose time ms    
        EPDL, //22expose delay ms
        EPCN,  //23expose count Sync tick
        MAON, //24Motor always On
        MVEL,   //25Motor Velocity  
        USR0,  //26  user reserved
        USR1,  //27  user reserved
        USR2,  //28  user reserved
        USR3,  //29   user reserved
        END
    };

    public class PmpData
    {
        public bool enable; //"S1EN",   //0 sequence1 enable
        public bool isPMP; // pmp or gray
        public uint bucket; //	"S1BK",   //sequence1 Bucket
        public uint T1; //"S1T1",   //sequence1 T1
        public uint T2; //"S1T2",   //sequence1 T2
        public uint T3; //"S1T3",   //sequence1 T3

        public Image3D referenceData;
        public List<Image2D> imageBufferList = new List<Image2D>();
        public Image3D zCalibrationData;
    }

    public class ExymaScannerInfo : DepthScannerInfo
    {
        private const int MAX_PROTOCAL_STR = 30;
        private const int MAX_TRANSFORM_DATA_SIZE = 3;
        public bool UseSerialControl { get; set; }
        public SerialPortInfo ControlBoardSerialInfo { get; private set; }
        public SerialPortInfo ControlBoardSerialInfo2 { get; private set; }
        public int DepthScannerIndex { get; set; }
        public int CenterCameraIndex { get; set; }
        public int CameraIndex { get; set; }
        public int Camera2Index { get; set; }
        public ControlBoardType ControlBoardType { get; set; }
        public MeasureTriggerMode MeasureTriggerMode { get; set; }
        public MeasureMode MeasureMode { get; set; }
        public uint[] BoardSettingData { get; set; } = new uint[(int)EEPROM.END];
        public string ConfigPath { get; set; }
        public TransformDataList TransformDataList { get; private set; } = new TransformDataList();

        //노이즈
        private float noiseLevel;
        private byte gain;
        private byte offset;

        public ExymaScannerInfo(DepthScannerType type = DepthScannerType.Exyma)
        {
            Type = type;

            ControlBoardSerialInfo = new SerialPortInfo("COM2");
            ControlBoardSerialInfo2 = new SerialPortInfo("COM4");

            BoardSettingData[(int)EEPROM.S1EN] = 1;
            BoardSettingData[(int)EEPROM.S1BK] = 7;
            BoardSettingData[(int)EEPROM.S1T1] = 100;
            BoardSettingData[(int)EEPROM.S1T2] = 56;
            BoardSettingData[(int)EEPROM.S1T3] = 12000;

            BoardSettingData[(int)EEPROM.S2EN] = 1;
            BoardSettingData[(int)EEPROM.S2BK] = 8;
            BoardSettingData[(int)EEPROM.S2T1] = 100;
            BoardSettingData[(int)EEPROM.S2T2] = 64;
            BoardSettingData[(int)EEPROM.S2T3] = 12000;

            BoardSettingData[(int)EEPROM.S3EN] = 1;
            BoardSettingData[(int)EEPROM.S3BK] = 9;
            BoardSettingData[(int)EEPROM.S3T1] = 100;
            BoardSettingData[(int)EEPROM.S3T2] = 72;
            BoardSettingData[(int)EEPROM.S3T3] = 12000;

            BoardSettingData[(int)EEPROM.S4EN] = 0;
            BoardSettingData[(int)EEPROM.S4BK] = 5;
            BoardSettingData[(int)EEPROM.S4T1] = 100;
            BoardSettingData[(int)EEPROM.S4T2] = 56;
            BoardSettingData[(int)EEPROM.S4T3] = 12000;

            BoardSettingData[(int)EEPROM.EPMD] = 1;
            BoardSettingData[(int)EEPROM.EPMS] = 30;
            BoardSettingData[(int)EEPROM.EPDL] = 18;
            BoardSettingData[(int)EEPROM.EPCN] = 14;
            BoardSettingData[(int)EEPROM.MAON] = 0;
            BoardSettingData[(int)EEPROM.MVEL] = 10000;
            BoardSettingData[(int)EEPROM.USR0] = 12340;
            BoardSettingData[(int)EEPROM.USR1] = 12341;
            BoardSettingData[(int)EEPROM.USR2] = 12342;
            BoardSettingData[(int)EEPROM.USR3] = 12343;
        }

        public override DepthScannerInfo Clone()
        {
            var exymaScannerInfo = new ExymaScannerInfo();
            exymaScannerInfo.Copy(this);

            return exymaScannerInfo;
        }

        public override void Copy(DepthScannerInfo srcDepthScannerInfo)
        {
            base.Copy(srcDepthScannerInfo);

            var exymaScannerInfo = (ExymaScannerInfo)srcDepthScannerInfo;

            ControlBoardSerialInfo = exymaScannerInfo.ControlBoardSerialInfo.Clone();
            ControlBoardSerialInfo2 = exymaScannerInfo.ControlBoardSerialInfo2.Clone();

            CameraIndex = exymaScannerInfo.CameraIndex;
            Camera2Index = exymaScannerInfo.Camera2Index;

            ControlBoardType = exymaScannerInfo.ControlBoardType;
            MeasureTriggerMode = exymaScannerInfo.MeasureTriggerMode;
            MeasureMode = exymaScannerInfo.MeasureMode;
            noiseLevel = exymaScannerInfo.noiseLevel;
            gain = exymaScannerInfo.gain;
            offset = exymaScannerInfo.offset;
            noiseLevel = exymaScannerInfo.noiseLevel;
            noiseLevel = exymaScannerInfo.noiseLevel;

            TransformDataList = exymaScannerInfo.TransformDataList.Clone();

            BoardSettingData = (uint[])exymaScannerInfo.BoardSettingData.Clone();
        }

        internal Point3d[] GetOrigin(int index)
        {
            var pointList = new List<Point3d>();
            pointList.Add(new Point3d(1, 1, 1));
            pointList.Add(new Point3d(2, 2, 2));
            pointList.Add(new Point3d(3, 3, 3));
            pointList.Add(new Point3d(4, 4, 4));

            return pointList.ToArray();
        }

        public string GetReferencePath(int scannerIndex)
        {
            return string.Format("{0}\\Reference_{1}.dat", ConfigPath, scannerIndex);
        }

        public string GetZCalibrationPath(int scannerIndex)
        {
            return string.Format("{0}\\ZCalibration_{1}.dat", ConfigPath, scannerIndex);
        }

        public string GetLensCalibrationPath(int scannerIndex)
        {
            return string.Format("{0}\\CalibrationLens_{1}.dat", ConfigPath, scannerIndex);
        }

        public bool Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                string bakConfigFileName = fileName + ".bak";
                if (File.Exists(bakConfigFileName) == true)
                {
                    File.Move(bakConfigFileName, fileName);
                }
                else
                {
                    MessageBox.Show(StringManager.GetString("There is no scanner configuration file."));
                    return false;
                }
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlElement configElement = xmlDocument.DocumentElement;

            Load(configElement);

            return true;
        }

        public override void Load(XmlElement nodeElement)
        {
            base.Load(nodeElement);

            ControlBoardSerialInfo.Load(nodeElement, "ControlBoardSerialInfo");
            ControlBoardSerialInfo2.Load(nodeElement, "ControlBoardSerialInfo2");

            CenterCameraIndex = Convert.ToInt32(XmlHelper.GetValue(nodeElement, "CenterCameraIndex", "0"));
            CameraIndex = Convert.ToInt32(XmlHelper.GetValue(nodeElement, "CameraIndex", "0"));
            Camera2Index = Convert.ToInt32(XmlHelper.GetValue(nodeElement, "Camera2Index", "0"));
            ControlBoardType = (ControlBoardType)Enum.Parse(typeof(ControlBoardType), XmlHelper.GetValue(nodeElement, "ControlBoardType", "None"));
            MeasureTriggerMode = (MeasureTriggerMode)Enum.Parse(typeof(MeasureTriggerMode), XmlHelper.GetValue(nodeElement, "MeasureTriggerMode", "None"));
            MeasureMode = (MeasureMode)Enum.Parse(typeof(MeasureMode), XmlHelper.GetValue(nodeElement, "MeasureMode", "None"));

            noiseLevel = Convert.ToSingle(XmlHelper.GetValue(nodeElement, "NoiseLevel", "0"));
            gain = Convert.ToByte(XmlHelper.GetValue(nodeElement, "Gain", "0"));
            offset = Convert.ToByte(XmlHelper.GetValue(nodeElement, "Offset", "0"));

            TransformDataList.Reset();

            XmlElement transformDataListElement = nodeElement["TransformDataList"];
            if (transformDataListElement != null)
            {
                foreach (XmlElement transformDataElement in transformDataListElement)
                {
                    if (transformDataElement.Name == "TransformData")
                    {
                        var transformData = new TransformData();
                        transformData.LoadData(transformDataElement);

                        TransformDataList.Add(transformData);
                    }
                }
            }

            XmlElement boardSettingElement = nodeElement["BoardSetting"];
            if (boardSettingElement != null)
            {
                BoardSettingData[(int)EEPROM.S1EN] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S1EN", "1"));
                BoardSettingData[(int)EEPROM.S1BK] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S1BK", "7"));
                BoardSettingData[(int)EEPROM.S1T1] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S1T1", "100"));
                BoardSettingData[(int)EEPROM.S1T2] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S1T2", "56"));
                BoardSettingData[(int)EEPROM.S1T3] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S1T3", "12000"));

                BoardSettingData[(int)EEPROM.S2EN] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S2EN", "1"));
                BoardSettingData[(int)EEPROM.S2BK] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S2BK", "8"));
                BoardSettingData[(int)EEPROM.S2T1] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S2T1", "100"));
                BoardSettingData[(int)EEPROM.S2T2] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S2T2", "64"));
                BoardSettingData[(int)EEPROM.S2T3] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S2T3", "12000"));

                BoardSettingData[(int)EEPROM.S3EN] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S3EN", "1"));
                BoardSettingData[(int)EEPROM.S3BK] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S3BK", "9"));
                BoardSettingData[(int)EEPROM.S3T1] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S3T1", "100"));
                BoardSettingData[(int)EEPROM.S3T2] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S3T2", "72"));
                BoardSettingData[(int)EEPROM.S3T3] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S3T3", "12000"));

                BoardSettingData[(int)EEPROM.S4EN] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S4EN", "0"));
                BoardSettingData[(int)EEPROM.S4BK] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S4BK", "5"));
                BoardSettingData[(int)EEPROM.S4T1] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S4T1", "100"));
                BoardSettingData[(int)EEPROM.S4T2] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S4T2", "56"));
                BoardSettingData[(int)EEPROM.S4T3] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "S4T3", "12000"));

                BoardSettingData[(int)EEPROM.EPMD] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "EPMD", "1"));
                BoardSettingData[(int)EEPROM.EPMS] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "EPMS", "30"));
                BoardSettingData[(int)EEPROM.EPDL] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "EPDL", "18"));
                BoardSettingData[(int)EEPROM.EPCN] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "EPCN", "14"));
                BoardSettingData[(int)EEPROM.MAON] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "MAON", "0"));
                BoardSettingData[(int)EEPROM.MVEL] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "MVEL", "10000"));
                BoardSettingData[(int)EEPROM.USR0] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "USR0", "12340"));
                BoardSettingData[(int)EEPROM.USR1] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "USR1", "12341"));
                BoardSettingData[(int)EEPROM.USR2] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "USR2", "12342"));
                BoardSettingData[(int)EEPROM.USR3] = Convert.ToUInt32(XmlHelper.GetValue(boardSettingElement, "USR3", "12343"));
            }
        }

        public void Save(string fileName)
        {
            string tempConfigFileName = fileName + "~";
            string bakFileName = fileName + ".bak";

            var xmlDocument = new XmlDocument();

            XmlElement configElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configElement);

            Save(configElement);

            xmlDocument.Save(tempConfigFileName);

            FileHelper.SafeSave(tempConfigFileName, bakFileName, fileName);
        }

        public override void Save(XmlElement nodeElement)
        {
            base.Save(nodeElement);

            ControlBoardSerialInfo.Save(nodeElement, "ControlBoardSerialInfo");
            ControlBoardSerialInfo2.Save(nodeElement, "ControlBoardSerialInfo2");

            XmlHelper.SetValue(nodeElement, "CenterCameraIndex", CenterCameraIndex.ToString());
            XmlHelper.SetValue(nodeElement, "CameraIndex", CameraIndex.ToString());
            XmlHelper.SetValue(nodeElement, "Camera2Index", Camera2Index.ToString());
            XmlHelper.SetValue(nodeElement, "ControlBoardType", ControlBoardType.ToString());
            XmlHelper.SetValue(nodeElement, "MeasureTriggerMode", MeasureTriggerMode.ToString());
            XmlHelper.SetValue(nodeElement, "MeasureMode", MeasureMode.ToString());

            XmlHelper.SetValue(nodeElement, "NoiseLevel", noiseLevel.ToString());
            XmlHelper.SetValue(nodeElement, "Gain", gain.ToString());
            XmlHelper.SetValue(nodeElement, "Offset", offset.ToString());

            XmlElement transformDataListElement = nodeElement.OwnerDocument.CreateElement("", "TransformDataList", "");
            nodeElement.AppendChild(transformDataListElement);

            foreach (TransformData transformData in TransformDataList)
            {
                XmlElement transformDataElement = nodeElement.OwnerDocument.CreateElement("", "TransformData", "");
                transformDataListElement.AppendChild(transformDataElement);

                transformData.SaveData(transformDataElement);
            }

            XmlElement boardSettingElement = nodeElement.OwnerDocument.CreateElement("", "BoardSetting", "");
            nodeElement.AppendChild(boardSettingElement);

            XmlHelper.SetValue(boardSettingElement, "S1EN", BoardSettingData[(int)EEPROM.S1EN].ToString());
            XmlHelper.SetValue(boardSettingElement, "S1BK", BoardSettingData[(int)EEPROM.S1BK].ToString());
            XmlHelper.SetValue(boardSettingElement, "S1T1", BoardSettingData[(int)EEPROM.S1T1].ToString());
            XmlHelper.SetValue(boardSettingElement, "S1T2", BoardSettingData[(int)EEPROM.S1T2].ToString());
            XmlHelper.SetValue(boardSettingElement, "S1T3", BoardSettingData[(int)EEPROM.S1T3].ToString());

            XmlHelper.SetValue(boardSettingElement, "S2EN", BoardSettingData[(int)EEPROM.S2EN].ToString());
            XmlHelper.SetValue(boardSettingElement, "S2BK", BoardSettingData[(int)EEPROM.S2BK].ToString());
            XmlHelper.SetValue(boardSettingElement, "S2T1", BoardSettingData[(int)EEPROM.S2T1].ToString());
            XmlHelper.SetValue(boardSettingElement, "S2T2", BoardSettingData[(int)EEPROM.S2T2].ToString());
            XmlHelper.SetValue(boardSettingElement, "S2T3", BoardSettingData[(int)EEPROM.S2T3].ToString());

            XmlHelper.SetValue(boardSettingElement, "S3EN", BoardSettingData[(int)EEPROM.S3EN].ToString());
            XmlHelper.SetValue(boardSettingElement, "S3BK", BoardSettingData[(int)EEPROM.S3BK].ToString());
            XmlHelper.SetValue(boardSettingElement, "S3T1", BoardSettingData[(int)EEPROM.S3T1].ToString());
            XmlHelper.SetValue(boardSettingElement, "S3T2", BoardSettingData[(int)EEPROM.S3T2].ToString());
            XmlHelper.SetValue(boardSettingElement, "S3T3", BoardSettingData[(int)EEPROM.S3T3].ToString());

            XmlHelper.SetValue(boardSettingElement, "S4EN", BoardSettingData[(int)EEPROM.S4EN].ToString());
            XmlHelper.SetValue(boardSettingElement, "S4BK", BoardSettingData[(int)EEPROM.S4BK].ToString());
            XmlHelper.SetValue(boardSettingElement, "S4T1", BoardSettingData[(int)EEPROM.S4T1].ToString());
            XmlHelper.SetValue(boardSettingElement, "S4T2", BoardSettingData[(int)EEPROM.S4T2].ToString());
            XmlHelper.SetValue(boardSettingElement, "S4T3", BoardSettingData[(int)EEPROM.S4T3].ToString());

            XmlHelper.SetValue(boardSettingElement, "EPMD", BoardSettingData[(int)EEPROM.EPMD].ToString());
            XmlHelper.SetValue(boardSettingElement, "EPMS", BoardSettingData[(int)EEPROM.EPMS].ToString());
            XmlHelper.SetValue(boardSettingElement, "EPDL", BoardSettingData[(int)EEPROM.EPDL].ToString());
            XmlHelper.SetValue(boardSettingElement, "EPCN", BoardSettingData[(int)EEPROM.EPCN].ToString());
            XmlHelper.SetValue(boardSettingElement, "MAON", BoardSettingData[(int)EEPROM.MAON].ToString());
            XmlHelper.SetValue(boardSettingElement, "MVEL", BoardSettingData[(int)EEPROM.MVEL].ToString());
            XmlHelper.SetValue(boardSettingElement, "USR0", BoardSettingData[(int)EEPROM.USR0].ToString());
            XmlHelper.SetValue(boardSettingElement, "USR1", BoardSettingData[(int)EEPROM.USR1].ToString());
            XmlHelper.SetValue(boardSettingElement, "USR2", BoardSettingData[(int)EEPROM.USR2].ToString());
            XmlHelper.SetValue(boardSettingElement, "USR3", BoardSettingData[(int)EEPROM.USR3].ToString());
        }

        public void LoadEEPROM(string fileName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlElement eepromElement = xmlDocument.DocumentElement;

            for (EEPROM eepromIndex = EEPROM.S1EN; eepromIndex < EEPROM.END; eepromIndex++)
            {
                BoardSettingData[(int)eepromIndex] = Convert.ToUInt32(XmlHelper.GetValue(eepromElement, eepromIndex.ToString(), "0"));
            }
        }

        public void SaveEEPROM(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement eepromElement = xmlDocument.CreateElement("", "EEPROM", "");
            xmlDocument.AppendChild(eepromElement);

            for (EEPROM eepromIndex = EEPROM.S1EN; eepromIndex < EEPROM.END; eepromIndex++)
            {
                XmlHelper.SetValue(eepromElement, eepromIndex.ToString(), BoardSettingData[(int)eepromIndex].ToString());
            }
        }

        public int GetExploseTimeUs()
        {
            /*	EPMD, //20expose Mode (  0 time, 1SyncCount)
            EPMS, //21expose time ms    
            EPDL, //22expose delay ms
            EPCN,  //23expose count Sync tick
            MAON, //24Motor always On
            MVEL,   //25Motor Velocity  */

            if (BoardSettingData[(int)EEPROM.EPMD] != 0) //synccount
            {
                // count * us/rev / 14면경
                return (int)(BoardSettingData[(int)EEPROM.EPCN] * 6000.0f / 14.0f);
            }
            else // 0 time
            {
                return (int)(BoardSettingData[(int)EEPROM.EPMS] * 1000);
            }
        }

        public int GetMaxImageCount()
        {
            uint cnt = 0;
            if (BoardSettingData[(int)EEPROM.S1EN] != 0)
            {
                cnt += BoardSettingData[(int)EEPROM.S1BK];
            }

            if (BoardSettingData[(int)EEPROM.S2EN] != 0)
            {
                cnt += BoardSettingData[(int)EEPROM.S2BK];
            }

            if (BoardSettingData[(int)EEPROM.S3EN] != 0)
            {
                cnt += BoardSettingData[(int)EEPROM.S3BK];
            }

            if (BoardSettingData[(int)EEPROM.S4EN] != 0)
            {
                cnt += BoardSettingData[(int)EEPROM.S4BK];
            }

            return (int)cnt;
        }

        public int GetPmpCount()
        {
            int cnt = 0;
            if (BoardSettingData[(int)EEPROM.S1EN] != 0)
            {
                cnt++;
            }

            if (BoardSettingData[(int)EEPROM.S2EN] != 0)
            {
                cnt++;
            }

            if (BoardSettingData[(int)EEPROM.S3EN] != 0)
            {
                cnt++;
            }

            return cnt;
        }

        public PmpData GetPmpData(int index)
        {
            var pmpData = new PmpData();

            int arrayPos = (int)(EEPROM.S1EN + (index * 5));

            pmpData.isPMP = (index == 3) ? false : true;

            pmpData.enable = BoardSettingData[arrayPos + 0] != 0;
            pmpData.bucket = BoardSettingData[arrayPos + 1];
            pmpData.T1 = BoardSettingData[arrayPos + 2];
            pmpData.T2 = BoardSettingData[arrayPos + 3];
            pmpData.T3 = BoardSettingData[arrayPos + 4];

            return pmpData;
        }
    }
}
