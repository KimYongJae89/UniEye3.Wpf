using Aladdin.HASP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class HaspHelper
    {
        private static HaspHelper instance = null;
        public static HaspHelper Instance()
        {
            if (instance == null)
            {
                instance = new HaspHelper();
            }

            return instance;
        }

        public bool CheckLicense(string vendorCode = "")
        {
            if (string.IsNullOrEmpty(vendorCode))
            {
                vendorCode = "6VA968FjuJZ9cWlNImhn0fNYeNLGq7bia6jkjtCUaBxdIzu6W7zUZNeQkvhu1Gd9y0rWaEYNimHuNLcl" +
                            "Tctl0t8D9d/WIJqH9Vr4X+fX9As+ch9qKy204l53j6pgwv/kPsqbuM8tcBlixMyxevpNTAgRk+k2nZYG" +
                            "41Hlds7yBOzAMG+u7jSuesxItBZ3bTKsihbI8TAv3H6CZX1q5EPl/dInETCK1oUgnOj2yFzQYpIx3/jW" +
                            "0o9EyvV6FNXzPvd2tNmmsqckZv2l5s95jyxL9REA4O9PxCkxT2mzO0SSK5NBjO6Gqg0cafVYXMwXjI7l" +
                            "RgVV0xDipgPLdnklujau4U341UzX2CNm/hif13GIsujzpVRAKNAkvqwIqfcoYUhVJepI0Bj9hjkXJmN4" +
                            "cK8Ag+Swr3a7aDWjjUQMDkJ3v8+N+GonXArEiw6pTRnHjAed2Er/kVrRSJ1k18xlwPbmswc1ANsTTFTI" +
                            "rjwHMAlI1nKwGoHFeGJQq1K+pa6bte+yCd6SjnAaK2DihNHbGMTvGHIrNC2v27hxcOc8HMAlRXxRTF11" +
                            "Zk4lvA5PAj6VfWyvE9EaE0Lk6EdIkmO6s7O6fJTBFZO8PxVBSck6mBbYTddRHKDfaRPeKlALz/fk9YOt" +
                            "tmLwfciybMk77LyTFmM0FqCnZF5t//3ucJYumY6qw4esnxgTenSmTBOztXn7RL3rK8acbcnrMUGXgOJz" +
                            "8wSYzmarN0DMECjW6OnefyvjtCjKMMJRCVZ3epQrCu19+ckOSJTBMj88dOSWur+lM/swNrFf6pR9enzu" +
                            "ioR6zGHqn6JO+gzPh5kI3XxBb9UvAj92mZKplz+q13mI2AzppAJ1UyFzlgEa3Yp1iLyObmh9hvHNgm1d" +
                            "5g4BBG7+4jQB019JU/oi6qQVYETYA23CSFQFzNwl5vMB2F2I8uCb9AVfpvDZxjca31ZksjDGoYgtYWqh" +
                            "cuZME1ZDpzrvdnhqhPmWqQ==";
            }

            var hasp = new Hasp();
            HaspStatus status = hasp.Login(vendorCode);

#if DEBUG == true
            return true;
#else
            return true;

            if (HaspStatus.StatusOk == status)
                return true;

            String errorMsg = "";
            if (HaspStatus.HaspDotNetDllBroken == status)
            {
                errorMsg = "HASP DLL or Driver is not installed";
            }

            if (HaspStatus.ContainerNotFound == status)
            {
                errorMsg = "Keylock is not installed";
            }

            LogHelper.Error(errorMsg);
            MessageBox.Show(errorMsg);

            return false;
#endif
        }

        public int GetFeatureCode(string vendorCode)
        {
            if (CheckLicense(vendorCode) == false)
            {
                return -1;
            }

            for (int code = 1; code < 100; code++)
            {
                HaspFeature feature;
                feature = HaspFeature.FromFeature(code);

                var hasp = new Hasp(feature);
                HaspStatus status = hasp.Login(vendorCode);

                if (HaspStatus.StatusOk == status)
                {
                    return code;
                }
            }

            return 0;
        }
    }
}
