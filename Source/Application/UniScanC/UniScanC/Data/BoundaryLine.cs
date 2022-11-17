using System.Collections.Generic;
using System.Drawing;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Data
{
    public class BoundaryLine : Observable
    {
        #region 필드
        private IEnumerable<PointF> target;
        private IEnumerable<PointF> uWarning;
        private IEnumerable<PointF> lWarning;
        private IEnumerable<PointF> uError;
        private IEnumerable<PointF> lError;
        #endregion

        #region 속성
        public IEnumerable<PointF> Target
        {
            get => target;
            set => Set(ref target, value);
        }

        public IEnumerable<PointF> UWarning
        {
            get => uWarning;
            set => Set(ref uWarning, value);
        }

        public IEnumerable<PointF> LWarning
        {
            get => lWarning;
            set => Set(ref lWarning, value);
        }

        public IEnumerable<PointF> UError
        {
            get => uError;
            set => Set(ref uError, value);
        }

        public IEnumerable<PointF> LError
        {
            get => lError;
            set => Set(ref lError, value);
        }
        #endregion
    }
}
