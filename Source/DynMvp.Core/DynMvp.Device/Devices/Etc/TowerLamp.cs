using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices
{
    public enum TowerLampStateType
    {
        Unknown = -1, Idle, Wait, Working, Defect, Alarm
    }

    public enum TowerLampValue
    {
        Off, On, Blink
    }

    public class Lamp
    {
        public TowerLampValue Value { get; set; }

        public Lamp(TowerLampValue value = TowerLampValue.Off)
        {
            Value = value;
        }
    }

    public class TowerLampState : IEnumerable
    {
        public TowerLampStateType Type { get; private set; }
        public Lamp RedLamp { get; }
        public Lamp YellowLamp { get; }
        public Lamp GreenLamp { get; }
        public Lamp Buzzer { get; }

        public TowerLampState()
        {
            Type = TowerLampStateType.Unknown;
            RedLamp = new Lamp(TowerLampValue.Off);
            YellowLamp = new Lamp(TowerLampValue.Off);
            GreenLamp = new Lamp(TowerLampValue.Off);
            Buzzer = new Lamp(TowerLampValue.Off);
        }

        public TowerLampState(TowerLampStateType type, Lamp redLamp, Lamp yellowLamp, Lamp greenLamp, Lamp buzzer)
        {
            Type = type;
            RedLamp = redLamp;
            YellowLamp = yellowLamp;
            GreenLamp = greenLamp;
            Buzzer = buzzer;
        }

        public void LoadXml(XmlElement xmlElement)
        {
            string[] towerLampValues = Enum.GetNames(typeof(TowerLampValue));

            Type = (TowerLampStateType)Enum.Parse(typeof(TowerLampStateType), XmlHelper.GetValue(xmlElement, "Type", "Idle"));

            RedLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "RedLampValue", "Off"));
            YellowLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "YellowLampValue", "Off"));
            GreenLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "GreenLampValue", "Off"));
            Buzzer.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "BuzzerValue", "Off"));
        }

        public void SaveXml(XmlElement xmlElement)
        {
            //ResetValue();

            XmlHelper.SetValue(xmlElement, "Type", Type.ToString());
            XmlHelper.SetValue(xmlElement, "RedLampValue", RedLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "YellowLampValue", YellowLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "GreenLampValue", GreenLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "BuzzerValue", Buzzer.Value.ToString());
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public delegate TowerLampState GetDynamicStateDelegate();

    public class TowerLamp
    {
        public GetDynamicStateDelegate GetDynamicState;
        private DigitalIoHandler digitalIoHandler;
        private IoPort towerLampRed;
        private IoPort towerLampYellow;
        private IoPort towerLampGreen;
        private IoPort towerBuzzer;
        private Task workingTask;
        public bool UseBuzzerPlayer { get; set; }

        private bool buzzerPlayerOn = false;
        private SoundPlayer buzzerPlayer = new SoundPlayer(DynMvp.Properties.Resources.BUZZER_1);
        public List<TowerLampState> TowerLampStateList { get; } = new List<TowerLampState>();

        public void Stop()
        {
            stopThreadFlag = true;

            if (workingTask != null)
            {
                workingTask.Wait();
            }

            TurnOffTowerLamp();
        }

        private bool stopThreadFlag = false;
        private TowerLampStateType towerLampStateType;
        public TowerLampStateType TowerLampStateType
        {
            set => towerLampStateType = value;
        }

        private int updateIntervalMs;

        public void Setup(DigitalIoHandler digitalIoHandler, int updateIntervalMs)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.updateIntervalMs = updateIntervalMs;

            InitTowerLampSateList();
        }

        public void SetupPort(IoPort towerLampRed, IoPort towerLampYellow, IoPort towerLampGreen, IoPort towerBuzzer)
        {
            this.towerLampRed = towerLampRed;
            this.towerLampYellow = towerLampYellow;
            this.towerLampGreen = towerLampGreen;
            this.towerBuzzer = towerBuzzer;
        }

        private void InitTowerLampSateList()
        {
            TowerLampStateList.Add(new TowerLampState(TowerLampStateType.Idle, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off)));
            TowerLampStateList.Add(new TowerLampState(TowerLampStateType.Wait, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off)));
            TowerLampStateList.Add(new TowerLampState(TowerLampStateType.Working, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off)));
            TowerLampStateList.Add(new TowerLampState(TowerLampStateType.Defect, new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On)));
            TowerLampStateList.Add(new TowerLampState(TowerLampStateType.Alarm, new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On)));
        }

        public void Save(string configPath)
        {
            string filePath = string.Format(@"{0}\TowerLamp.xml", configPath);

            var xmlDocument = new XmlDocument();

            XmlElement element = xmlDocument.CreateElement("TowerLamp");
            xmlDocument.AppendChild(element);
            foreach (TowerLampState state in TowerLampStateList)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(state.Type.ToString());
                element.AppendChild(subElement);
                state.SaveXml(subElement);
            }

            XmlHelper.Save(xmlDocument, filePath);
        }

        public void Load(string configPath)
        {
            string filePath = string.Format(@"{0}\TowerLamp.xml", configPath);
            XmlDocument xmlDocument = XmlHelper.Load(filePath);
            if (xmlDocument == null)
            {
                return;
            }

            XmlElement element = xmlDocument.DocumentElement;
            string[] types = Enum.GetNames(typeof(TowerLampStateType));
            foreach (string type in types)
            {
                XmlElement subElement = element[type];
                if (subElement == null)
                {
                    continue;
                }

                // 중복된 상태 제거
                List<TowerLampState> findList = TowerLampStateList.FindAll(f => f.Type.ToString() == type);
                foreach (TowerLampState find in findList)
                {
                    TowerLampStateList.Remove(find);
                }

                var state = new TowerLampState();
                state.LoadXml(subElement);
                TowerLampStateList.Add(state);
            }
        }

        private TowerLampState GetState()
        {
            return TowerLampStateList.Find(x => x.Type == towerLampStateType);
        }

        private TowerLampState GetState(TowerLampStateType type)
        {
            return TowerLampStateList.Find(x => x.Type == type);
        }

        public void SetState(TowerLampStateType type)
        {
            towerLampStateType = type;
        }

        private void TurnOnTowerLamp(TowerLampState towerLampState, bool isblinkOn = false)
        {
            TurnOnTowerLamp(towerLampRed, towerLampState.RedLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerLampYellow, towerLampState.YellowLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerLampGreen, towerLampState.GreenLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerBuzzer, towerLampState.Buzzer.Value, isblinkOn);
        }

        public void TurnOnTowerLamp(IoPort towerLampPort, TowerLampValue value, bool isblinkOn)
        {
            bool isOnLamp = value == TowerLampValue.On || (isblinkOn && value == TowerLampValue.Blink);
            digitalIoHandler.WriteOutput(towerLampPort, isOnLamp);
        }

        public void TurnOffTowerLamp()
        {
            digitalIoHandler.WriteOutput(towerLampRed, false);
            digitalIoHandler.WriteOutput(towerLampYellow, false);
            digitalIoHandler.WriteOutput(towerLampGreen, false);
            digitalIoHandler.WriteOutput(towerBuzzer, false);
        }

        public void Start()
        {
            towerLampStateType = TowerLampStateType.Idle;
            workingTask = new Task(new Action(WorkingProc));
            workingTask.Start();
        }

        public void WorkingProc()
        {
            bool isblinkOn = false;

            while (stopThreadFlag == false)
            {
                try
                {
                    // Blink 시키기 위해 On/Off 플래그 생성
                    TowerLampState state;

                    if (ErrorManager.Instance().IsAlarmed())
                    {
                        state = GetState(TowerLampStateType.Alarm);

                        if (ErrorManager.Instance().BuzzerOn)
                        {
                            state.Buzzer.Value = TowerLampValue.On;
                            if (buzzerPlayerOn == false)
                            {
                                buzzerPlayerOn = true;
                                buzzerPlayer.Play();
                            }
                        }
                        else
                        {
                            state.Buzzer.Value = TowerLampValue.Off;
                            if (buzzerPlayerOn == true)
                            {
                                buzzerPlayerOn = false;
                                buzzerPlayer.Stop();
                            }
                        }
                    }
                    else
                    {
                        if (buzzerPlayerOn == true)
                        {
                            buzzerPlayerOn = false;
                            buzzerPlayer.Stop();
                        }

                        if (GetDynamicState != null)
                        {
                            state = GetDynamicState();
                        }
                        else
                        {
                            state = GetState();
                        }
                    }

                    if (state != null)
                    {
                        TurnOnTowerLamp(state, isblinkOn);
                        Thread.Sleep(500);
                    }

                    //Thread.Sleep(updateIntervalMs);
                    //TurnOffTowerLamp();
                    Thread.Sleep(updateIntervalMs);

                    isblinkOn = !isblinkOn;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
