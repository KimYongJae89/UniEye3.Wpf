using OmniDriver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPF.RefractionMeasure.Device
{
    public delegate void ScanDoneDelegate(Dictionary<string, double[]> newRaw);

    public class Spectrometer
    {
        public NETWrapper Wrapper { get; }
        public int NumSpectrometer { get; private set; } = 0;
        public Dictionary<string, Device.SpectrometerInfo> DeviceList { get; }

        private Dictionary<string, double[]> newestSpectrum;
        private Mutex mutex = new Mutex();
        private Task scanTask = null;
        private bool stopScanTask = true;

        public ScanDoneDelegate ScanDone = null;

        private object LockObj = new object();

        public Spectrometer()
        {
            Wrapper = new OmniDriver.NETWrapper();
            DeviceList = new Dictionary<string, SpectrometerInfo>();
            newestSpectrum = new Dictionary<string, double[]>();
        }

        public void Open()
        {
            NumSpectrometer = Wrapper.openAllSpectrometers();
            DeviceList.Clear();
            for (int i = 0; i < NumSpectrometer; i++)
            {
                var info = new SpectrometerInfo();

                info.Index = i;
                info.Name = Wrapper.getName(i);
                info.WaveLengths = Wrapper.getWavelengths(i);
                info.NumberOfPixels = Wrapper.getNumberOfPixels(i);

                info.IntegrationTime = Wrapper.getMinimumIntegrationTime(i);
                info.ScansToAverage = Wrapper.getScansToAverage(i);
                info.BoxcarWidth = Wrapper.getBoxcarWidth(i);

                Wrapper.setIntegrationTime(i, info.IntegrationTime);
                DeviceList.Add(info.Name, info);
                newestSpectrum.Add(info.Name, new double[info.WaveLengths.Count()]);
            }

            stopScanTask = false;
            scanTask = new Task(new Action(ScanProc));
            scanTask.Start();
        }

        public void Close()
        {
            stopScanTask = true;
            if (scanTask != null)
            {
                scanTask.Wait(1000);
            }

            NumSpectrometer = 0;
            DeviceList.Clear();
            newestSpectrum.Clear();
            Wrapper.closeAllSpectrometers();
        }

        public void UpdateSpectrometer(string specName = "")
        {
            if (DeviceList.ContainsKey(specName) == false)
            {
                return;
            }

            SpectrometerInfo info = DeviceList[specName];
            Wrapper.setIntegrationTime(info.Index, info.IntegrationTime);
            Wrapper.setScansToAverage(info.Index, info.ScansToAverage);
            Wrapper.setBoxcarWidth(info.Index, info.BoxcarWidth);
        }

        private void ScanProc()
        {
            while (stopScanTask == false)
            {
                var tasks = new List<Task>();
                foreach (KeyValuePair<string, SpectrometerInfo> pair in DeviceList)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        double[] newRaw = Wrapper.getSpectrum(pair.Value.Index);

                        mutex.WaitOne();
                        Array.Copy(newRaw, newestSpectrum[pair.Key], newestSpectrum[pair.Key].Count());
                        mutex.ReleaseMutex();
                    }));
                }
                Task.WaitAll(tasks.ToArray());

                if (ScanDone != null && stopScanTask != true)
                {
                    ScanDone(newestSpectrum);
                }
            }
        }

        public bool ReadSpectrum(string path, out Dictionary<string, double[]> refData, out Dictionary<string, double[]> bgData)
        {
            double[] spectrum;
            bool isRefOK = false;
            bool isBgOK = false;
            refData = new Dictionary<string, double[]>();
            bgData = new Dictionary<string, double[]>();
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                spectrum = new double[info.WaveLengths.Length];
                isRefOK = ReadSpectrum(string.Format(@"{0}\RefSpect_{1}.TXT", path, info.Name), spectrum);
                if (isRefOK == true)
                {
                    refData.Add(info.Name, spectrum);
                }

                spectrum = new double[info.WaveLengths.Length];
                isBgOK = ReadSpectrum(string.Format(@"{0}\BgSpect_{1}.TXT", path, info.Name), spectrum);
                if (isBgOK == true)
                {
                    bgData.Add(info.Name, spectrum);
                }
            }
            return isRefOK & isBgOK;
        }

        private bool ReadSpectrum(string file, double[] spectrum)
        {
            if (File.Exists(file) == false)
            {
                return false;
            }

            using (var stream = new System.IO.StreamReader(file))
            {
                for (int i = 0; i < spectrum.Count(); i++)
                {
                    spectrum[i] = Convert.ToDouble(stream.ReadLine());
                }
            }

            return true;
        }

        public Dictionary<string, double[]> SaveRefSpectrum(string path, int avg)
        {
            var refData = new Dictionary<string, double[]>();
            string fileName;
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                int originAvg = Wrapper.getScansToAverage(info.Index);
                int originInt = Wrapper.getIntegrationTime(info.Index);
                Wrapper.setScansToAverage(info.Index, avg);
                Thread.Sleep(originAvg * originInt * 2);
                lock (LockObj)
                {
                    refData.Add(info.Name, new double[newestSpectrum[info.Name].Length]);
                    Array.Copy(newestSpectrum[info.Name], refData[info.Name], refData[info.Name].Count());
                }
                fileName = string.Format(@"{0}\RefSpect_{1}.TXT", path, info.Name);
                SaveSpectrum(fileName, refData[info.Name]);

                Wrapper.setScansToAverage(info.Index, originAvg);
            }
            return refData;
        }

        public Dictionary<string, double[]> SaveBgSpectrum(string path, int avg)
        {
            var bgData = new Dictionary<string, double[]>();
            string fileName;
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                int originAvg = Wrapper.getScansToAverage(info.Index);
                int originInt = Wrapper.getIntegrationTime(info.Index);
                Wrapper.setScansToAverage(info.Index, avg);
                Thread.Sleep(originAvg * originInt * 2);
                lock (LockObj)
                {
                    bgData.Add(info.Name, new double[newestSpectrum[info.Name].Length]);
                    Array.Copy(newestSpectrum[info.Name], bgData[info.Name], bgData[info.Name].Count());
                }
                fileName = string.Format(@"{0}\BgSpect_{1}.TXT", path, info.Name);
                SaveSpectrum(fileName, bgData[info.Name]);

                Wrapper.setScansToAverage(info.Index, originAvg);
            }
            return bgData;
        }

        private bool SaveSpectrum(string pathName, double[] spectrum)
        {
            using (var file = new System.IO.StreamWriter(pathName))
            {
                for (int i = 0; i < spectrum.Count(); i++)
                {
                    file.WriteLine("{0:0.00000}", spectrum[i]);
                }

                file.Flush();
            }
            return true;
        }
    }
}
