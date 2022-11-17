using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    /* keep track of apertures used in stats reporting */
    public class ApertureStatItem
    {
        public ApertureStatItem() { }
        public ApertureStatItem(Aperture aperture)
        {
            LayerIndex = -1;
            ApertureNo = aperture.ApertureNo;
            Type = aperture.Type;
            for (int i = 0; i < 5; i++)
            {
                Parameter[i] = aperture.Parameter[i];
            }
        }

        public int ApertureNo { get; set; }
        public int LayerIndex { get; set; }
        public int Count { get; set; }
        public ApertureType Type { get; set; }
        public double[] Parameter { get; set; } = new double[5];
    }

    /*! Contains statistics on the various codes used in a RS274X file */
    public class GerberStat
    {
        public List<ErrorItem> ErrorList { get; } = new List<ErrorItem>();
        public List<ApertureStatItem> ApertureList { get; } = new List<ApertureStatItem>();
        public List<ApertureStatItem> DCodeList { get; } = new List<ApertureStatItem>();

        public int LayerCount { get; set; }
        public int G0 { get; set; }
        public int G1 { get; set; }
        public int G2 { get; set; }
        public int G3 { get; set; }
        public int G4 { get; set; }
        public int G10 { get; set; }
        public int G11 { get; set; }
        public int G12 { get; set; }
        public int G36 { get; set; }
        public int G37 { get; set; }
        public int G54 { get; set; }
        public int G55 { get; set; }
        public int G70 { get; set; }
        public int G71 { get; set; }
        public int G74 { get; set; }
        public int G75 { get; set; }
        public int G90 { get; set; }
        public int G91 { get; set; }
        public int GUnknown;

        public int D1 { get; set; }
        public int D2 { get; set; }
        public int D3 { get; set; }
        /*    GHashTable *D_user_defined; */
        public int DUnknown { get; set; }
        public int DError { get; set; }

        public int M0 { get; set; }
        public int M1 { get; set; }
        public int M2 { get; set; }
        public int MUnknown { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int I { get; set; }
        public int J { get; set; }

        /* Must include % RS-274 codes */
        public int Star { get; set; }
        public int Unknown { get; set; }

        public void AddGerberStat(GerberStat gerberStat)
        {
            LayerCount++;

            G0 += gerberStat.G0;
            G1 += gerberStat.G1;
            G2 += gerberStat.G2;
            G3 += gerberStat.G3;
            G4 += gerberStat.G4;
            G10 += gerberStat.G10;
            G11 += gerberStat.G11;
            G12 += gerberStat.G12;
            G36 += gerberStat.G36;
            G37 += gerberStat.G37;
            G54 += gerberStat.G54;
            G55 += gerberStat.G55;
            G70 += gerberStat.G70;
            G71 += gerberStat.G71;
            G74 += gerberStat.G74;
            G75 += gerberStat.G75;
            G90 += gerberStat.G90;
            G91 += gerberStat.G91;
            GUnknown += gerberStat.GUnknown;

            D1 += gerberStat.D1;
            D2 += gerberStat.D2;
            D3 += gerberStat.D3;

            DUnknown += gerberStat.DUnknown;
            DError += gerberStat.DError;

            M0 += gerberStat.M0;
            M1 += gerberStat.M1;
            M2 += gerberStat.M2;
            MUnknown += gerberStat.MUnknown;

            X += gerberStat.X;
            Y += gerberStat.Y;
            I += gerberStat.I;
            J += gerberStat.J;

            Star += gerberStat.Star;
            Unknown += gerberStat.Unknown;

            ErrorList.AddRange(gerberStat.ErrorList);
            ApertureList.AddRange(gerberStat.ApertureList);

            foreach (ApertureStatItem dCodeItem in gerberStat.DCodeList)
            {
                AddDCodeItem(dCodeItem);
            }
        }

        public void AddAperture(Aperture aperture)
        {
            ApertureList.Add(new ApertureStatItem(aperture));
        }

        public void AddDCodeItem(ApertureStatItem apertureStatItem)
        {
            AddDCodeItem(apertureStatItem.ApertureNo, apertureStatItem.Count);
        }

        public void AddDCodeItem(int apertureNo, int count)
        {
            ApertureStatItem apertureStatItem = DCodeList.Find(x => x.ApertureNo == apertureNo);
            if (apertureStatItem == null)
            {
                apertureStatItem = new ApertureStatItem() { ApertureNo = apertureNo, Count = count };
                DCodeList.Add(apertureStatItem);
            }
            else
            {
                apertureStatItem.Count += count;
            }
        }

        public bool IncrementDCodeItem(int apertureNo)
        {
            ApertureStatItem dCodeItem = DCodeList.Find(x => x.ApertureNo == apertureNo);
            if (dCodeItem == null)
            {
                return false;
            }

            dCodeItem.Count++;
            return true;
        }

        public void AddError(string errorMsg, int layerIndex = -1, MessageType messageType = MessageType.Error)
        {
            ErrorList.Add(new ErrorItem() { LayerIndex = layerIndex, Message = errorMsg, Type = messageType });
        }

        internal void Clear()
        {
            ErrorList.Clear();
            ApertureList.Clear();
            DCodeList.Clear();
        }
    }
}
