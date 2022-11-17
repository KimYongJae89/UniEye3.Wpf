using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class GerberData // gerbv_image_t
    {
        internal LayerType LayerType { get; set; }
        internal List<Aperture> ApertureList { get; } = new List<Aperture>();
        internal List<Layer> LayerList { get; } = new List<Layer>();
        internal List<NetEntryState> NetEntryStateList { get; } = new List<NetEntryState>();
        internal List<ApertureMacro> ApertureMacroList { get; } = new List<ApertureMacro>();
        internal GerberFormat Format { get; set; }
        internal GerberInfo Info { get; set; }
        internal List<NetEntry> NetEntryList { get; } = new List<NetEntry>();
        internal GerberStat GerberStat { get; set; }
        internal DrillStat DrillStat { get; set; }

        public List<NetEntry> GetRenderableNetEntryList()
        {
            var renderableEntryList = new List<NetEntry>();

            bool onPolygon = false;
            foreach (NetEntry netEntry in NetEntryList)
            {
                if (onPolygon == true)
                {
                    if (netEntry.Interpolation == Interpolation.PolygonEnd)
                    {
                        onPolygon = false;
                    }
                }
                else if (netEntry.Interpolation == Interpolation.PolygonStart)
                {
                    onPolygon = true;
                    renderableEntryList.Add(netEntry);
                }
                else
                {
                    renderableEntryList.Add(netEntry);
                }
            }
            return renderableEntryList;
        }

        public List<NetEntry> GetPolygonNetEntryList(NetEntry startNetEntry)
        {
            var renderableEntryList = new List<NetEntry>();

            bool polygonFound = false;
            foreach (NetEntry netEntry in NetEntryList)
            {
                if (netEntry == startNetEntry)
                {
                    polygonFound = true;
                }

                if (polygonFound == true)
                {
                    renderableEntryList.Add(netEntry);

                    if (netEntry.Interpolation == Interpolation.PolygonEnd)
                    {
                        break;
                    }
                }
            }

            return renderableEntryList;
        }

        public GerberData()
        {
            ApertureList = new List<Aperture>();

            LayerList = new List<Layer>();
            LayerList.Add(new Layer());

            NetEntryStateList = new List<NetEntryState>();
            NetEntryStateList.Add(new NetEntryState());

            ApertureMacroList = new List<ApertureMacro>();
            Format = new GerberFormat();
            Info = new GerberInfo();
            NetEntryList = new List<NetEntry>();
            GerberStat = new GerberStat();
            DrillStat = new DrillStat();
        }

        private void Free()
        {
            foreach (Aperture aperture in ApertureList)
            {
                aperture.Clear();
            }

            ApertureList.Clear();
            ApertureMacroList.Clear();
            Format.Clear();
            Info.Clear();

            NetEntryList.Clear();
            LayerList.Clear();
            NetEntryStateList.Clear();

            GerberStat.Clear();
            DrillStat.Clear();

            return;
        }

        public NetEntryState AddNewEntryState()
        {
            var entryState = new NetEntryState();
            NetEntryStateList.Add(entryState);

            return entryState;
        }

        public void AddAperture(Aperture aperture)
        {
            ApertureList.Add(aperture);
            GerberStat.AddAperture(aperture);
        }

        public ApertureMacro GetApertureMacro(string name)
        {
            return ApertureMacroList.Find(x => x.Name == name);
        }

        public void AddApertureMacro(ApertureMacro apertureMacro)
        {
            ApertureMacroList.Add(apertureMacro);
        }

        public void AddLayer(Layer newLayer)
        {
            LayerList.Add(newLayer);
        }

        public Layer GetLastLayer()
        {
            return LayerList.Last();
        }

        public void AddNetEntryState(NetEntryState netEntryState)
        {
            NetEntryStateList.Add(netEntryState);
        }

        public NetEntryState GetLastNetEntryState()
        {
            return NetEntryStateList.Last();
        }

        public Aperture GetAperture(int apertureNo)
        {
            return ApertureList.Find(x => x.ApertureNo == apertureNo);
        }

        internal void AddNetEntry(NetEntry netEntry)
        {
            NetEntryList.Add(netEntry);
        }
    }
}
