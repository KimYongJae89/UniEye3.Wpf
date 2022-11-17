using DynMvp.Base;
using DynMvp.Vision.Cognex;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public enum ImagingLibrary
    {
        OpenCv, EuresysOpenEVision, CognexVisionPro, MatroxMIL, Halcon, Cuda, Custom
    }

    internal class License
    {
        public ImagingLibrary imagingLibrary;
        public string subLicense;

        public bool exist;

        public License(ImagingLibrary imagingLibrary, string subLicense, bool exist)
        {
            this.imagingLibrary = imagingLibrary;
            this.subLicense = subLicense;
            this.exist = exist;
        }
    }

    public class LicenseManager
    {
        private static List<License> licenseList = new List<License>();
        private static bool cognexInstallationChecked = false;
        private static bool cognexInstalled = false;
        private static bool milInstallationChecked = false;
        private static bool milInstalled = false;

        public static bool VisionProInstalled()
        {
            if (cognexInstallationChecked == false)
            {
                cognexInstalled = RegistryHelper.IsInstalled("Cognex VisionPro");
                cognexInstallationChecked = true;
            }

            return cognexInstalled;
        }

        public static bool MilInstalled()
        {
            if (milInstallationChecked == false)
            {
                milInstalled = RegistryHelper.IsInstalled("Matrox Imaging") || RegistryHelper.IsInstalled("Matrox Imaging (64-bit)");
                milInstallationChecked = true;
            }

            return milInstalled;
        }

        public static void CheckImagingLicense()
        {
            if (MilInstalled() == true)
            {
                LogHelper.Debug(LoggerType.StartUp, "MIL license installed");
            }

            if (VisionProInstalled() == true)
            {
                LogHelper.Debug(LoggerType.StartUp, "Cognex license installed");
            }
        }

        public static bool LicenseExist(ImagingLibrary imagingLibrary, string subLicenseType)
        {
            License foundLicense = null;

            string[] subLicenses = subLicenseType.Split(new char[] { ',', ';' });

            if (string.IsNullOrWhiteSpace(subLicenseType))
            {
                foundLicense = licenseList.Find(x => x.imagingLibrary == imagingLibrary);
            }
            else
            {
                foreach (string subLicense in subLicenses)
                {
                    foundLicense = licenseList.Find(x => x.imagingLibrary == imagingLibrary && x.subLicense == subLicense);

                    if (foundLicense != null)
                    {
                        break;
                    }
                }
            }

            if (foundLicense != null)
            {
                return foundLicense.exist;
            }
            else
            {
                bool exist = false;

                if (string.IsNullOrWhiteSpace(subLicenseType))
                {
                    switch (imagingLibrary)
                    {
                        case ImagingLibrary.EuresysOpenEVision:
                            exist = EuresysHelper.LicenseExist();
                            break;
                        case ImagingLibrary.OpenCv:
                            exist = true;
                            break;
                    }

                    if (exist == true)
                    {
                        licenseList.Add(new License(imagingLibrary, "", exist));
                    }
                }
                else
                {
                    if (MatroxHelper.InitApplication())
                    {
                        foreach (string subLicense in subLicenses)
                        {
                            foundLicense = licenseList.Find(x => x.imagingLibrary == imagingLibrary && x.subLicense == subLicense);

                            if (foundLicense == null)
                            {
                                exist = false;
                                string[] alternativeLicenses = subLicense.Split('|');
                                foreach (string alternativeLicense in alternativeLicenses)
                                {
                                    switch (imagingLibrary)
                                    {
                                        case ImagingLibrary.MatroxMIL:
                                            if (MilInstalled() == true)
                                            {
                                                exist |= MatroxHelper.LicenseExist(alternativeLicense);
                                            }

                                            break;
                                        case ImagingLibrary.CognexVisionPro:
                                            if (VisionProInstalled() == true)
                                            {
                                                exist |= CognexHelper.LicenseExist(alternativeLicense);
                                            }

                                            break;
                                    }
                                }

                                if (exist == false)
                                {
                                    return false;
                                }

                                licenseList.Add(new License(imagingLibrary, subLicense, exist));
                            }
                        }

                        //MatroxHelper.FreeApplication();
                    }
                }
            }

            return true;
        }
    }
}
