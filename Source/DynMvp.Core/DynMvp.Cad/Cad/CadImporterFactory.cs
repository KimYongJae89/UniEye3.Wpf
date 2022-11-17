using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Cad
{
    public enum CadType
    {
        STL, STEP, IGS, JT
    }

    public class CadImporterFactory
    {
        public static CadImporter Create(CadType cadType)
        {
            return new StlImporter();
        }
    }
}
