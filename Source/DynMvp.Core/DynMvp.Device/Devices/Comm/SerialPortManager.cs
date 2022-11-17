using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.Comm
{
    public class SerialPortManager
    {
        private List<SerialPortEx> serialPortList = new List<SerialPortEx>();

        private static SerialPortManager _instance = null;

        private SerialPortManager()
        {
        }

        public static SerialPortManager Instance()
        {
            if (_instance == null)
            {
                _instance = new SerialPortManager();
            }

            return _instance;
        }

        public void AddSerialPort(SerialPortEx serialPort)
        {
            serialPortList.Add(serialPort);
        }

        public void ReleaseSerialPort(SerialPortEx serialPort)
        {
            serialPortList.Remove(serialPort);
        }

        public bool IsPortAvailable(string portName)
        {
            if (IsPortExist(portName) == true)
            {
                return IsPortAssigned(portName) == false;
            }

            return false;
        }

        private bool IsPortExist(string portName)
        {
            foreach (string installedPortName in SerialPort.GetPortNames())
            {
                if (installedPortName == portName)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPortAssigned(string portName)
        {
            foreach (SerialPortEx namedSerialPort in serialPortList)
            {
                if (namedSerialPort.PortName == portName)
                {
                    return true;
                }
            }

            return false;
        }

        public static void FillComboAllPort(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            comboBox.Items.Add("None");

            foreach (string portName in SerialPort.GetPortNames())
            {
                comboBox.Items.Add(portName);
            }

        }

        public void FillComboUsedPort(ComboBox comboBox)
        {
            comboBox.Items.Clear();

            comboBox.Items.Add("None");

            foreach (SerialPortEx namedSerialPort in serialPortList)
            {
                comboBox.Items.Add(namedSerialPort.Name);
            }
        }

        public SerialPortEx GetSerialPort(string nameOrReadableName)
        {
            foreach (SerialPortEx namedSerialPort in serialPortList)
            {
                if (namedSerialPort.PortName == nameOrReadableName || namedSerialPort.Name == nameOrReadableName)
                {
                    return namedSerialPort;
                }
            }

            return null;
        }
    }
}
