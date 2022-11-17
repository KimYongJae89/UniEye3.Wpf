using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public class MatroxHelper
    {
        private static MIL_ID applicationId = MIL.M_NULL;

        public static bool InitApplication()
        {
            if (applicationId == MIL.M_NULL)
            {
                LogHelper.Debug(LoggerType.StartUp, "Initialize MIL Applications");
                MIL.MappAlloc(MIL.M_DEFAULT, ref applicationId);
                MIL.MappControl(applicationId, MIL.M_ERROR, MIL.M_PRINT_ENABLE);
#if !DEBUG
                MIL.MappControl(applicationId, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
#endif
            }

            return applicationId != MIL.M_NULL;
        }

        public static void FreeApplication()
        {
            if (applicationId != MIL.M_NULL)
            {
                LogHelper.Debug(LoggerType.StartUp, "Free MIL Applications");

                MIL.MappFree(applicationId);

                applicationId = MIL.M_NULL;
            }
        }

        public static bool LicenseExist(string licenseString)
        {
            if (applicationId == MIL.M_NULL)
            {
                return false;
            }

            bool result = false;

            long licenseValue = 0;
            MIL.MappInquire(MIL.M_LICENSE_MODULES, ref licenseValue);

            string[] licenseTypes = licenseString.Split(';');

            foreach (string licenseType in licenseTypes)
            {
                switch (licenseType)
                {
                    case "PAT": result = (licenseValue & MIL.M_LICENSE_PAT) != 0; break;
                    case "EDGE": result = (licenseValue & MIL.M_LICENSE_EDGE) != 0; break;
                    case "IM": result = (licenseValue & MIL.M_LICENSE_IM) != 0; break;
                    case "MEAS": result = (licenseValue & MIL.M_LICENSE_MEAS) != 0; break;
                    case "BLOB": result = (licenseValue & MIL.M_LICENSE_BLOB) != 0; break;
                    case "CAL": result = (licenseValue & MIL.M_LICENSE_CAL) != 0; break;
                    case "CODE": result = (licenseValue & MIL.M_LICENSE_CODE) != 0; break;
                    case "OCR": result = (licenseValue & MIL.M_LICENSE_OCR) != 0; break;
                    default: break;
                }
            }

            return result;
        }
    }
}
