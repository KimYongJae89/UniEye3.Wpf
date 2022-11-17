using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.GerberCvt
{
    public abstract class GerberCvtLoader
    {
        public abstract bool Load(string fileName);
        public abstract GerberData GetGerberData();
    }
}
