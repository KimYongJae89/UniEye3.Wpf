using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

//Rectangle targetRegion = new Rectangle();



//CadImporter cadImporter = CadImporterFactory.Create(CadType.STL);
//Cad3dModel model = cadImporter.Import("test.stl");

//CadConverter cadConverter = new CadConverter();
//Image3D image3d = cadConverter.Convert(model, targetRegion);

namespace DynMvp.Cad
{
    public abstract class CadImporter
    {
        public abstract Cad3dModel Import(string fileName);
    }
}
