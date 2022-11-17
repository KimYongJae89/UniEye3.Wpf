using DynMvp.Devices.Comm;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using static DynMvp.Devices.Comm.SerialEncoderV105;

namespace Unieye.WPF.Base.ViewModels
{
    public class EncoderCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objectList = new List<object>();
            objectList.Add(value);
            objectList.Add(parameter);

            return objectList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EnInJudgementSeletor : DataTemplateSelector
    {
        public DataTemplate EnInDataTemplate { get; set; }
        public DataTemplate DefaultDataTemplate { get; set; }

        public EnInJudgementSeletor() { }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }

            var tupleImte = item as WTuple<string, object>;

            if (tupleImte.Item1.ToString() == "EN" || tupleImte.Item1.ToString() == "IN")
            {
                return EnInDataTemplate;
            }
            else
            {
                return DefaultDataTemplate;
            }
        }
    }

    public class EncoderSettingViewModel : Observable
    {
        public EncoderSettingViewModel(SerialEncoder serialEncoder)
        {
            UseAutoUpdateMode = false;
            Initialize(serialEncoder);
            UpdateDataSoruce();
            BuildSendCommandList();
        }
        public SerialEncoder SerialEncoder { get; set; }

        private bool useAutoUpdateMode = false;
        public bool UseAutoUpdateMode
        {
            get => useAutoUpdateMode;
            set => Set(ref useAutoUpdateMode, value);
        }

        public void Initialize(SerialEncoder serialEncoder)
        {
            SerialEncoder = serialEncoder;
        }

        private List<string> ecommandList = new List<string>();
        public List<string> EcommandList
        {
            get => ecommandList;
            set => Set(ref ecommandList, value);
        }

        private List<string> sendCommandList = new List<string>();
        public List<string> SendCommandList
        {
            get => sendCommandList;
            set => Set(ref sendCommandList, value);
        }

        private void BuildSendCommandList()
        {
            foreach (string data in SerialEncoder.GetSendCommandString())
            {
                SendCommandList.Add(data);
            }
            //string version = serialEncoder.Version;

            ////serialEncoder.GetSendCommandString();
            //Array ecommandArray1, ecommandArray2;
            //int index = 0;
            //switch (version)
            //{
            //    case "1.05":
            //        ecommandArray1 = Enum.GetValues(typeof(SerialEncoderV105.ECommand));
            //        foreach (object data in ecommandArray1)
            //            ecommandList.Add(data.ToString());
            //        SendCommandList = ecommandList.GetRange(0, 3);
            //        break;

            //    case "1.07":
            //        ecommandArray1 = Enum.GetValues(typeof(SerialEncoderV107.ECommand));
            //        ecommandArray2 = Enum.GetValues(typeof(SerialEncoderV107.ECommandV2));

            //        foreach (object data in ecommandArray1)
            //            ecommandList.Add(data.ToString());
            //        SendCommandList = ecommandList.GetRange(0, 3);

            //        foreach (object data in ecommandArray2)
            //        {
            //            index = ecommandList.Count;
            //            ecommandList.Add(data.ToString());
            //        }
            //        SendCommandList = ecommandList.GetRange(index + 1, 3);

            //        break;
            //}

            //return ecommandList;
        }
        private List<string> resultList = new List<string>();
        public List<string> ResultList
        {
            get => resultList;
            set => Set(ref resultList, value);
        }

        private List<string> newResultList = new List<string>();

        private List<WTuple<string, object>> encoderList = new List<WTuple<string, object>>();
        public List<WTuple<string, object>> EncoderList
        {
            get => encoderList;
            set => Set(ref encoderList, value);
        }

        private ICommand encoderCommand;
        public ICommand EncoderCommand => encoderCommand ?? (encoderCommand = new RelayCommand<object>(ClickedButton));

        private void ClickedButton(object commandString)
        {
            switch (SerialEncoder.Version)
            {
                case "1.05":
                    ESendCommand command = ESendCommand.AP;
                    if (Enum.TryParse<ESendCommand>((string)commandString, out command))
                    {
                        ExcuteCommand(command);
                    }

                    break;
                case "1.07":
                    SerialEncoderV107.ESendCommandV2 command2 = SerialEncoderV107.ESendCommandV2.CY;
                    if (Enum.TryParse<SerialEncoderV107.ESendCommandV2>((string)commandString, out command2))
                    {
                        ExcuteCommand(command2);
                    }

                    break;

            }
        }

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand<ChildWindow>(Close));

        private void Close(ChildWindow wnd)
        {
            wnd.Close(false);
        }

        //private ICommand multiEncoderCommand;
        //public ICommand MultiEncoderCommand { get => multiEncoderCommand ?? (multiEncoderCommand = new RelayCommand<List<object>>(MultiEncoderCommander)); }

        //private void MultiEncoderCommander(List<object> obj)
        //{
        //    bool check = (bool)obj[0];
        //    ESendCommand command = ESendCommand.AP;
        //    if (Enum.TryParse<ESendCommand>((string)obj[1], out command))
        //        ExcuteCommand(command, check ? "1" : "0");
        //}

        private ICommand clickToggleSwitch;
        public ICommand ClickToggleSwitch => clickToggleSwitch ?? (clickToggleSwitch = new RelayCommand(ClickedToggleButton));
        private void ClickedToggleButton()
        {
            UseAutoUpdateMode = !UseAutoUpdateMode;
            if (UseAutoUpdateMode)
            {
                ExcuteCommand(SerialEncoderV105.ESendCommand.AP);
            }

            if (UseAutoUpdateMode)
            {
                ExcuteCommand(SerialEncoderV107.ESendCommandV2.PC);
            }
        }

        private string manualTextBox;
        public string ManuualTextBox
        {
            get => manualTextBox;
            set => Set(ref manualTextBox, value);
        }

        private ICommand manualSendCommand;
        public ICommand ManualSendCommand => manualSendCommand ?? (manualSendCommand = new RelayCommand<string>(SendManualCommand));
        private void SendManualCommand(string command)
        {
            ExcuteCommand(command);
            ManuualTextBox = "";
        }

        private void UpdateDataSoruce()
        {
            EncoderList.Clear();

            if (SerialEncoder.IsCompatible(SerialEncoderV105.ESendCommand.GR))
            {
                ExcuteCommand(SerialEncoderV105.ESendCommand.GR);
            }

            if (SerialEncoder.IsCompatible(SerialEncoderV107.ESendCommandV2.PC))
            {
                ExcuteCommand(SerialEncoderV107.ESendCommandV2.PC);
            }
        }

        private void ExcuteCommand(Enum en, params string[] args)
        {
            if (SerialEncoder.IsCompatible(en))
            {
                //serialEncoder.ExcuteCommand(en, args);
                string packetString = SerialEncoder.MakePacket(en.ToString(), args);
                ExcuteCommand(packetString);
            }
        }

        private bool isWorkingCommand = false;
        private void ExcuteCommand(string packetString)
        {
            if (!isWorkingCommand)
            {
                isWorkingCommand = true;

                string[] token = SerialEncoder.ExcuteCommand(packetString);

                UpdateValue(token);

                if (token != null)
                {
                    // send ok
                    newResultList.Insert(0, "TX >>    [ OK ] : " + packetString);

                    if (token.Length > 0)
                    {
                        // recive ok
                        var sb = new StringBuilder();
                        sb.Append(token[0]);
                        for (int i = 1; i < token.Length; i++)
                        {
                            sb.AppendFormat(",{0}", token[i]);
                        }

                        string tokenString = sb.ToString().Trim();
                        newResultList.Insert(0, "   << RX [ OK ] : " + tokenString);
                    }
                    else
                    {
                        // recive fail
                        newResultList.Insert(0, "   << RX [FAIL] : ");
                    }
                }
                else
                {
                    // send Fail
                    newResultList.Insert(0, "TX >>    [FAIL] : " + packetString);
                }
                //ResultList = resultList.Append(newResultList.ToArray()).ToList();
                //ResultList.InsertRange(0, newResultList);
                ResultList = new List<string>();
                ResultList = newResultList;
                isWorkingCommand = false;
            }
        }

        private delegate void UpdateValueDelegate(string[] token);
        private void UpdateValue(string[] token)
        {
            var newEncoderList = new List<WTuple<string, object>>();

            if (token == null)
            {
                return;
            }

            for (int i = 0; i < token.Count(); i++)
            {
                string s = string.Join("", token[i].Where(char.IsLetter));

                if (SerialEncoder.IsCompatible(s))
                {
                    WTuple<string, object> tuple;
                    Enum e = SerialEncoder.GetCommand(s);

                    lock (newEncoderList)
                    {
                        switch (e)
                        {
                            case SerialEncoderV105.EResponseCommand.AR:
                                tuple = new WTuple<string, object>(token[i].ToString(), string.Format("{0},{1}", token[i + 1], token[i + 2]));
                                break;
                            default:
                                tuple = new WTuple<string, object>(token[i].ToString(), token[i + 1]);
                                break;
                        }
                    }

                    tuple.TuplePropertyChanged += TuplePropertyChanged;
                    newEncoderList.Add(tuple);
                }
            }

            var wTupleList = new List<WTuple<string, object>>(EncoderList);
            newEncoderList.ForEach(f =>
            {
                string name = f.Item1;
                WTuple<string, object> found = wTupleList.Find(g => g.Item1.Equals(name));
                if (found != null)
                {
                    found.Item2 = f.Item2;
                }
                else
                {
                    wTupleList.Add(f);
                }
            });

            EncoderList = wTupleList;
        }

        private void TuplePropertyChanged(string item1, object item2)
        {
            switch (item1)
            {
                case "AR":
                    string[] item2Array = item2.ToString().Split(',');
                    string commandString = string.Format("{0},{1},{2}", item1, item2Array[0], item2Array[1]);
                    ExcuteCommand(commandString);
                    break;
                default:
                    ExcuteCommand(item1 + "," + item2);
                    break;
            }
        }
    }
}
