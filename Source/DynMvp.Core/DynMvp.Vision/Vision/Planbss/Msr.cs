using DynMvp.Base;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Vision.Planbss
{
    public class Msr
    {
        #region < Constructor >
        public Msr()
        {
            GsFrame.Set(0, EIMAGECOLOR.COLOR_RGB, EIMAGEMONO.MONO_GRAY);
        }

        #endregion
        #region < Variable and Default >
        private Rectangle m_TcRoi = new Rectangle(0, 0, 0, 0);
        #endregion

        #region < Get/Set Function >    
        public int GsErr { get; set; } = 0;
        public int GsMsrCount { get; set; } = 4;
        public bool GsUse { get; set; } = true;
        public Rectangle GsTcRoi
        {
            get => m_TcRoi;
            set => m_TcRoi = value;
        }
        public Rectangle GsIsRoi { get; set; } = new Rectangle(0, 0, 0, 0);
        public IF_FRAME GsFrame { get; set; } = new IF_FRAME();
        public PointD GsOutPt { get; set; } = new PointD(0, 0);
        #endregion

        #region < Member Function >
        public virtual bool Verify(int SizeX, int SizeY)				///< Verify Parameter
        {
            if ((m_TcRoi.Left < 0) || (m_TcRoi.Right >= SizeX))
            {
                return false;
            }

            if ((m_TcRoi.Top < 0) || (m_TcRoi.Bottom >= SizeY))
            {
                return false;
            }

            if ((m_TcRoi.Width <= 0) || (m_TcRoi.Height <= 0))
            {
                return false;
            }

            if ((GsIsRoi.Left < 0) || (GsIsRoi.Right >= SizeX))
            {
                return false;
            }

            if ((GsIsRoi.Top < 0) || (GsIsRoi.Bottom >= SizeY))
            {
                return false;
            }

            if ((GsIsRoi.Width <= 0) || (GsIsRoi.Height <= 0))
            {
                return false;
            }

            if (GsFrame.Source.eMono == EIMAGEMONO.MONO_NONE)
            {
                return false;
            }

            if (GsFrame.Source.nIndex < 0)
            {
                return false;
            }

            if (GsFrame.eUnion != EIMAGEUNION.UNION_NONE)
            {
                if (GsFrame.Target.eMono == EIMAGEMONO.MONO_NONE)
                {
                    return false;
                }

                if (GsFrame.Target.nIndex < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private string GetMsrName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Use";
                case 1: return "Area";
                case 2: return "Source";
                case 3: return "Target";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetMsr(string strTitle, string strData)
        {
            var helper = new HelpMath();

            for (int i = 0; i < GsMsrCount; i++)
            {
                if (false == strTitle.Equals(GetMsrName(i)))
                {
                    continue;
                }

                var rData = Rectangle.FromLTRB(0, 0, 0, 0);

                switch (i)
                {
                    case 0: GsUse = helper.ToSplit(strData); break;
                    case 1: helper.ToSplit(strData, ref m_TcRoi); break;
                    case 2:
                        helper.ToSplit(strData, ref rData);
                        GsFrame.Source.nIndex = rData.Left;
                        GsFrame.Source.eColor = (EIMAGECOLOR)rData.Top;
                        GsFrame.Source.eMono = (EIMAGEMONO)rData.Right;
                        GsFrame.eUnion = (EIMAGEUNION)rData.Bottom;
                        break;
                    case 3:
                        helper.ToSplit(strData, ref rData);
                        GsFrame.Target.nIndex = rData.Left;
                        GsFrame.Target.eColor = (EIMAGECOLOR)rData.Top;
                        GsFrame.Target.eMono = (EIMAGEMONO)rData.Right;
                        break;
                    default:
                        return false;
                }

                return true;
            }

            return false;	// !! NOT Error : Only SetMsr isn't have member.
        }

        public bool GetMsr(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsMsrCount))
            {
                return false;
            }

            strName = GetMsrName(nIndex);

            switch (nIndex)
            {
                case 0:
                    strData = string.Format("{0}", (GsUse == true) ? 1 : 0);
                    break;
                case 1:
                    strData = string.Format("{0}, {1}, {2}, {3}", GsTcRoi.Left, GsTcRoi.Top,
                                                                  GsTcRoi.Right, GsTcRoi.Bottom);
                    break;
                case 2:
                    strData = "Not Support";
                    break;
                case 3:
                    strData = "Not Support";
                    break;
                default:
                    return false;
            }

            return true;
        }

        #endregion        
    }

    public class MsrEdge : Msr
    {
        #region < Constructor >
        public MsrEdge()
        {
        }

        #endregion

        #region < Get/Set Function >
        public int GsEdgeCount { get; set; } = 6;
        public int GsDirect { get; set; } = 1;
        public int GsWtoB { get; set; } = 0;
        public int GsGv { get; set; } = 20;
        public int GsThickW { get; set; } = 1;
        public int GsThickH { get; set; } = 5;
        public int GsDistance { get; set; } = 5;
        public Rectangle GsEdgeRoi { get; set; } = new Rectangle(0, 0, 0, 0);
        public PointD GsEdgeOutPt { get; set; } = new PointD(0, 0);
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if ((GsDirect < 1) || (GsDirect > 8))
            {
                return false;
            }

            if ((GsWtoB < 0) || (GsWtoB > 2))
            {
                return false;
            }

            if ((GsThickW <= 0) || (GsThickH <= 0))
            {
                return false;
            }

            if (GsDistance <= 0)
            {
                return false;
            }

            if ((GsGv == 0) || (GsGv == 255))
            {
                return false;
            }

            return true;
        }

        private string GetEdgeName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Direct";
                case 1: return "WtoB";
                case 2: return "ThickW";
                case 3: return "ThickH";
                case 4: return "Distance";
                case 5: return "GrayValue";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetEdge(string strTitle, string strData)
        {
            if (SetMsr(strTitle, strData))
            {
                return true;
            }

            bool bEntry = false;
            for (int i = 0; i < GsEdgeCount; i++)
            {
                if (false == strTitle.Equals(GetEdgeName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0: GsDirect = Convert.ToInt32(strData); break;
                    case 1: GsWtoB = Convert.ToInt32(strData); break;
                    case 2: GsThickW = Convert.ToInt32(strData); break;
                    case 3: GsThickH = Convert.ToInt32(strData); break;
                    case 4: GsDistance = Convert.ToInt32(strData); break;
                    case 5: GsGv = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }

        public bool GetEdge(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsEdgeCount))
            {
                return false;
            }

            strName = GetEdgeName(nIndex);

            switch (nIndex)
            {
                case 0: strData = string.Format("{0}", GsDirect); break;
                case 1: strData = string.Format("{0}", GsWtoB); break;
                case 2: strData = string.Format("{0}", GsThickW); break;
                case 3: strData = string.Format("{0}", GsThickH); break;
                case 4: strData = string.Format("{0}", GsDistance); break;
                case 5: strData = string.Format("{0}", GsGv); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrLine : MsrEdge
    {
        #region < Constructor >
        public MsrLine()
        {
        }

        #endregion
        #region < Variable and Default >
        #endregion

        #region < Get/Set Function >
        public int GsLineCount { get; set; } = 10;
        public int GsBumpyCount { get; set; } = 5;
        public int GsScan { get; set; } = 3;
        public Point GsRate { get; set; } = new Point(60, 0);
        public int GetRateIn()
        {
            return GsRate.X;
        }
        public int GetRateOut()
        {
            return GsRate.Y;
        }
        public void SetRateOut(int nRate)
        {
            GsRate = new Point(GsRate.X, nRate);
        }
        public PointD GsAngle { get; set; } = new PointD(0, 0);
        public double GetAngleOut()
        {
            return GsAngle.Y;
        }
        public void SetAngleOut(double dAngle)
        {
            GsAngle = new PointD(GsAngle.X, dAngle);
        }
        public double GsAngleTolP { get; set; } = 5.0f;
        public double GsAngleTolM { get; set; } = -5.0f;

        public bool GsUseBumpy { get; set; } = false;
        public int GsScanBumpy { get; set; } = 2;
        public SearchSide GsBGSide { get; set; } = SearchSide.Left;
        public int GsIteration { get; set; } = 3;
        public int GsDepth { get; set; } = 5;
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if (GsScan <= 0)
            {
                return false;
            }

            if (GsUseBumpy)
            {
                if (GsScanBumpy < 1)
                {
                    return false;
                }

                if ((GsBGSide < SearchSide.Left) || (GsBGSide > SearchSide.Bottom))
                {
                    return false;
                }

                if ((GsIteration < 2) || (GsDepth < 3))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetLineName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                // Line
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Angle";
                case 3: return "AngleTolP";
                case 4: return "AngleTolM";
                // Bumpy
                case 5: return "UseBumpy";
                case 6: return "ScanBumpy";
                case 7: return "BGSide";
                case 8: return "Iteration";
                case 9: return "Depth";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetLine(string strTitle, string strData)
        {
            if (SetEdge(strTitle, strData))
            {
                return true;
            }

            var helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < GsLineCount; i++)
            {
                if (false == strTitle.Equals(GetLineName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    // Line
                    case 0: GsScan = Convert.ToInt32(strData); break;
                    case 1: GsRate = new Point(Convert.ToInt32(strData), GsRate.Y); break;
                    case 2: GsAngle = new PointD(Convert.ToDouble(strData), GsAngle.Y); break;
                    case 4: GsAngleTolP = Convert.ToDouble(strData); break;
                    case 5: GsAngleTolM = Convert.ToDouble(strData); break;
                    // Bumpy
                    case 6: GsUseBumpy = helper.ToSplit(strData); break;
                    case 7: GsScanBumpy = Convert.ToInt32(strData); break;
                    case 8: GsBGSide = (SearchSide)(Convert.ToInt32(strData)); break;
                    case 9: GsIteration = Convert.ToInt32(strData); break;
                    case 10: GsDepth = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }
        public bool GetLine(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsLineCount))
            {
                return false;
            }

            strName = GetLineName(nIndex);
            switch (nIndex)
            {
                // Line
                case 0: strData = string.Format("{0}", GsScan); break;
                case 1: strData = string.Format("{0}", GsRate.X); break;
                case 2: strData = string.Format("{0}", GsAngle.X); break;
                case 3: strData = string.Format("{0}", GsAngleTolP); break;
                case 4: strData = string.Format("{0}", GsAngleTolM); break;
                // Bumpy
                case 5: strData = string.Format("{0}", (GsUseBumpy == true) ? 1 : 0); break;
                case 6: strData = string.Format("{0}", GsScanBumpy); break;
                case 7: strData = string.Format("{0}", GsBGSide); break;
                case 8: strData = string.Format("{0}", GsIteration); break;
                case 9: strData = string.Format("{0}", GsDepth); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrCorner : MsrEdge
    {
        #region < Constructor >
        public MsrCorner()
        {
        }

        #endregion

        #region < Get/Set Function >
        public int GsCornerCount { get; set; } = 6;
        public int GsScan { get; set; } = 3;
        public Point GsRate { get; set; } = new Point(60, 0);
        public int GetRateIn()
        {
            return GsRate.X;
        }
        public void SetRateIn(int nRate)
        {
            GsRate = new Point(GsRate.X, nRate);
        }
        public int GetRateOut()
        {
            return GsRate.Y;
        }
        public void SetRateOut(int nRate)
        {
            GsRate = new Point(GsRate.X, nRate);
        }
        public CardinalPoint GsCardinal { get; set; } = CardinalPoint.NorthEast;
        public bool GsOtuToIn { get; set; } = true;
        public int GsRange { get; set; } = 30;
        public int GsLength { get; set; } = 80;
        public double GsOutAngleHor { get; set; } = 0;
        public double GsOutAngleVer { get; set; } = 0;
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if (GsScan <= 0)
            {
                return false;
            }

            return true;
        }

        private string GetCornerName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Cardinal";
                case 3: return "OutToIn";
                case 4: return "Range";
                case 5: return "Length";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetCorner(string strTitle, string strData)
        {
            if (SetEdge(strTitle, strData))
            {
                return true;
            }

            var helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < GsCornerCount; i++)
            {
                if (false == strTitle.Equals(GetCornerName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0: GsScan = Convert.ToInt32(strData); break;
                    case 1: GsRate = new Point(Convert.ToInt32(strData), GsRate.Y); break;
                    case 2: GsCardinal = (CardinalPoint)(Convert.ToInt32(strData)); break;
                    case 3: GsOtuToIn = helper.ToSplit(strData); break;
                    case 4: GsRange = Convert.ToInt32(strData); break;
                    case 5: GsLength = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }
        public bool GetCorner(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsCornerCount))
            {
                return false;
            }

            strName = GetCornerName(nIndex);
            switch (nIndex)
            {
                case 0: strData = string.Format("{0}", GsScan); break;
                case 1: strData = string.Format("{0}", GsRate.X); break;
                case 2: strData = string.Format("{0}", GsCardinal); break;
                case 3: strData = string.Format("{0}", GsOtuToIn); break;
                case 4: strData = string.Format("{0}", GsRange); break;
                case 5: strData = string.Format("{0}", GsLength); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrQuadrangle : MsrCorner
    {
        #region < Constructor >
        public MsrQuadrangle()
        {
        }

        #endregion

        #region < Get/Set Function >
        public int GsQuadrangleCount { get; set; } = 1;
        public ConvexShape GsConvex { get; set; } = ConvexShape.None;
        public double GsOutAngle { get; set; } = 0;
        public PointD GsVertexLT { get; set; } = new PointD(0, 0);
        public PointD GsVertexRT { get; set; } = new PointD(0, 0);
        public PointD GsVertexRB { get; set; } = new PointD(0, 0);
        public PointD GsVertexLB { get; set; } = new PointD(0, 0);
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            return true;
        }

        private string GetQuadrangleName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Convex";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetQuadrangle(string strTitle, string strData)
        {
            if (SetCorner(strTitle, strData))
            {
                return true;
            }

            bool bEntry = false;
            for (int i = 0; i < GsCornerCount; i++)
            {
                if (false == strTitle.Equals(GetQuadrangleName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0:
                        GsConvex = (ConvexShape)(Convert.ToInt32(strData));
                        break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }
        public bool GetQuadrangle(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsQuadrangleCount))
            {
                return false;
            }

            strName = GetQuadrangleName(nIndex);
            switch (nIndex)
            {
                case 0:
                    strData = string.Format("{0}", GsConvex);
                    break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrCircle : MsrEdge
    {
        #region < Constructor >
        public MsrCircle()
        {
        }

        #endregion

        #region < Get/Set Function >
        public int CircleCount { get; set; } = 6    // Circle
                                      + 5    // Check Bumpy
                                      + 16    // Check Width
                                      + 0;
        public int GsScan { get; set; } = 8;
        public Point GsRate { get; set; } = new Point(60, 0);
        public void SetRateIn(int InRate)
        {
            GsRate = new Point(InRate, GsRate.Y);
        }
        public void SetRateOut(int OutRate)
        {
            GsRate = new Point(GsRate.X, OutRate);
        }
        public PointD GsRadius { get; set; } = new PointD(650, 0);
        public double GetRadiusOut()
        {
            return GsRadius.Y;
        }
        public void SetRadiusIn(double dRadius)
        {
            GsRadius = new PointD(dRadius, GsRadius.Y);
        }
        public void SetRadiusOut(double dRadius)
        {
            GsRadius = new PointD(GsRadius.X, dRadius);
        }
        public int GsRange { get; set; } = 24;
        public int GsRadiusTolP { get; set; } = 3;
        public int GsRadiusTolM { get; set; } = 3;
        // Bumpy
        public bool GsUseBumpy { get; set; } = false;
        public int GsScanBumpy { get; set; } = 2;
        public SearchSide GsBGSide { get; set; } = SearchSide.Left;
        public int GsIteration { get; set; } = 5;
        public int GsDepth { get; set; } = 5;
        // Width
        public bool GsUseWidth { get; set; } = false;
        public int GsScanWidth { get; set; } = 1;
        public int GsOffsetRadius { get; set; } = 0;
        // Width : Outer
        public int GsOuterOffset { get; set; } = 2;
        public bool GsOuterOutToIn { get; set; } = true;
        public int GsOuterRange { get; set; } = 24;
        public int GsOuterWtoB { get; set; } = 2;
        public int GsOuterGv { get; set; } = 15;
        // Width : Inner
        public int GsInnerOffset { get; set; } = 2;
        public bool GsInnerOutToIn { get; set; } = false;
        public int GsInnerRange { get; set; } = 24;
        public int GsInnerWtoB { get; set; } = 2;
        public int GsInnerGv { get; set; } = 15;
        // Width : Spec
        public int GsWidthThin { get; set; } = 0;
        public int GsWidthThick { get; set; } = 6;
        public int GsWidthLength { get; set; } = 20;
        // Uniformity
        public bool GsUseUniform { get; set; } = false;
        public int GsFanSize { get; set; } = 8;
        public int GsGvUniform { get; set; } = 20;
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if (GsScan <= 0)
            {
                return false;
            }

            if (GsRadius.X <= 0)
            {
                return false;
            }

            if ((GsRadiusTolM < 0) || (GsRadiusTolP < 0))
            {
                return false;
            }

            if (GsUseBumpy)
            {
                if (GsScanBumpy < 1)
                {
                    return false;
                }

                if ((GsBGSide < SearchSide.Left) || (GsBGSide > SearchSide.Bottom))
                {
                    return false;
                }

                if ((GsIteration < 2) || (GsDepth < 3))
                {
                    return false;
                }
            }

            if (GsUseWidth)
            {
                if (GsScanWidth < 1)
                {
                    return false;
                }
            }

            if (GsUseUniform)
            {
                if (GsFanSize < 2)
                {
                    return false;
                }

                if (GsGvUniform < 2)
                {
                    return false;
                }
            }

            return true;
        }

        public Rectangle GetOutCircle()
        {
            PointD OutCenter = GsOutPt;
            double OutRadius = GetRadiusOut();

            int L = (int)((OutCenter.X - OutRadius) + 0.5f);
            int T = (int)((OutCenter.Y - OutRadius) + 0.5f);
            int R = (int)((OutCenter.X + OutRadius) + 0.5f);
            int B = (int)((OutCenter.Y + OutRadius) + 0.5f);

            return Rectangle.FromLTRB(L, T, R, B);
        }

        private string GetCircleName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                // Check Circle
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Radius";
                case 3: return "Range";
                case 4: return "ToleranceP";
                case 5: return "ToleranceM";
                // Check Bumpy
                case 6: return "UseBumpy";
                case 7: return "ScanBumpy";
                case 8: return "BGSide";
                case 9: return "Iteration";
                case 10: return "Depth";
                // Check Width
                case 11: return "UseWidth";
                case 12: return "ScanWidth";
                case 13: return "OffsetRadius";
                case 14: return "OuterOffset";
                case 15: return "OuterOutToIn";
                case 16: return "OuterRange";
                case 17: return "OuterWtoB";
                case 18: return "OuterGv";
                case 19: return "InnerOffset";
                case 20: return "InnerOutToIn";
                case 21: return "InnerRange";
                case 22: return "InnerWtoB";
                case 23: return "InnerGv";
                case 24: return "WidthThin";
                case 25: return "WidthThick";
                case 26: return "WidthLength";
                // Check Uniform
                case 27: return "UseUniform";
                case 28: return "FanSize";
                case 29: return "GvUniform";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetCircle(string strTitle, string strData)
        {
            if (SetEdge(strTitle, strData))
            {
                return true;
            }

            var helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < CircleCount; i++)
            {
                if (false == strTitle.Equals(GetCircleName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0: GsScan = Convert.ToInt32(strData); break;
                    case 1: GsRate = new Point(Convert.ToInt32(strData), GsRate.Y); break;
                    case 2: GsRadius = new PointD(Convert.ToDouble(strData), GsRadius.Y); break;
                    case 3: GsRange = Convert.ToInt32(strData); break;
                    case 4: GsRadiusTolP = Convert.ToInt32(strData); break;
                    case 5: GsRadiusTolM = Convert.ToInt32(strData); break;
                    // Check Bumpy
                    case 6: GsUseBumpy = helper.ToSplit(strData); break;
                    case 7: GsScanBumpy = Convert.ToInt32(strData); break;
                    case 8: GsBGSide = (SearchSide)(Convert.ToInt32(strData)); break;
                    case 9: GsIteration = Convert.ToInt32(strData); break;
                    case 10: GsDepth = Convert.ToInt32(strData); break;
                    // Check Width
                    case 11: GsUseWidth = helper.ToSplit(strData); break;
                    case 12: GsScanWidth = Convert.ToInt32(strData); break;
                    case 13: GsOffsetRadius = Convert.ToInt32(strData); break;
                    case 14: GsOuterOffset = Convert.ToInt32(strData); break;
                    case 15: GsOuterOutToIn = helper.ToSplit(strData); break;
                    case 16: GsOuterRange = Convert.ToInt32(strData); break;
                    case 17: GsOuterWtoB = Convert.ToInt32(strData); break;
                    case 18: GsOuterGv = Convert.ToInt32(strData); break;
                    case 19: GsInnerOffset = Convert.ToInt32(strData); break;
                    case 20: GsInnerOutToIn = helper.ToSplit(strData); break;
                    case 21: GsInnerRange = Convert.ToInt32(strData); break;
                    case 22: GsInnerWtoB = Convert.ToInt32(strData); break;
                    case 23: GsInnerGv = Convert.ToInt32(strData); break;
                    case 24: GsWidthThin = Convert.ToInt32(strData); break;
                    case 25: GsWidthThick = Convert.ToInt32(strData); break;
                    case 26: GsWidthLength = Convert.ToInt32(strData); break;
                    // Check Uniform
                    case 27: GsUseUniform = helper.ToSplit(strData); break;
                    case 28: GsFanSize = Convert.ToInt32(strData); break;
                    case 29: GsGvUniform = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }

        public bool GetCircle(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= CircleCount))
            {
                return false;
            }

            strName = GetCircleName(nIndex);
            switch (nIndex)
            {
                case 0: strData = string.Format("{0}", GsScan); break;
                case 1: strData = string.Format("{0}", GsRate.X); break;
                case 2: strData = string.Format("{0}", GsRadius.X); break;
                case 3: strData = string.Format("{0}", GsRange); break;
                case 4: strData = string.Format("{0}", GsRadiusTolP); break;
                case 5: strData = string.Format("{0}", GsRadiusTolM); break;
                // Check Bumpy
                case 6: strData = string.Format("{0}", (GsUseBumpy == true) ? 1 : 0); break;
                case 7: strData = string.Format("{0}", GsScanBumpy); break;
                case 8: strData = string.Format("{0}", GsBGSide); break;
                case 9: strData = string.Format("{0}", GsIteration); break;
                case 10: strData = string.Format("{0}", GsDepth); break;
                // Check Width
                case 11: strData = string.Format("{0}", (GsUseWidth == true) ? 1 : 0); break;
                case 12: strData = string.Format("{0}", GsScanWidth); break;
                case 13: strData = string.Format("{0}", GsOffsetRadius); break;
                case 14: strData = string.Format("{0}", GsOuterOffset); break;
                case 15: strData = string.Format("{0}", GsOuterOutToIn); break;
                case 16: strData = string.Format("{0}", GsOuterRange); break;
                case 17: strData = string.Format("{0}", GsOuterWtoB); break;
                case 18: strData = string.Format("{0}", GsOuterGv); break;
                case 19: strData = string.Format("{0}", GsInnerOffset); break;
                case 20: strData = string.Format("{0}", GsInnerOutToIn); break;
                case 21: strData = string.Format("{0}", GsInnerRange); break;
                case 22: strData = string.Format("{0}", GsInnerWtoB); break;
                case 23: strData = string.Format("{0}", GsInnerGv); break;
                case 24: strData = string.Format("{0}", GsWidthThin); break;
                case 25: strData = string.Format("{0}", GsWidthThick); break;
                case 26: strData = string.Format("{0}", GsWidthLength); break;
                // Check Uniform
                case 27: strData = string.Format("{0}", (GsUseUniform == true) ? 1 : 0); break;
                case 28: strData = string.Format("{0}", GsFanSize); break;
                case 29: strData = string.Format("{0}", GsGvUniform); break;
                default:
                    return false;
            }

            return true;
        }

        public void LoadParam(XmlElement paramElement)
        {
            GsScan = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Scan", GsScan.ToString()));
            GsRate = new Point(Convert.ToInt32(XmlHelper.GetValue(paramElement, "Rate", GsRate.X.ToString())), GsRate.Y);
            GsRadius = new PointD(Convert.ToInt32(XmlHelper.GetValue(paramElement, "Radius", GsRadius.X.ToString())), GsRadius.Y);
            GsRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Range", GsRange.ToString()));
            GsRadiusTolP = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ToleranceP", GsRadiusTolP.ToString()));
            GsRadiusTolM = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ToleranceM", GsRadiusTolM.ToString()));
            GsUseBumpy = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseBumpy", GsUseBumpy.ToString()));
            GsScanBumpy = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ScanBumpy", GsScanBumpy.ToString()));
            GsBGSide = (SearchSide)Enum.Parse(typeof(SearchSide), XmlHelper.GetValue(paramElement, "BGSide", GsBGSide.ToString()));
            GsIteration = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Iteration", GsIteration.ToString()));
            GsDepth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Depth", GsDepth.ToString()));
            GsUseWidth = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseWidth", GsUseWidth.ToString()));
            GsScanWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ScanWidth", GsScanWidth.ToString()));
            GsOffsetRadius = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRadius", GsOffsetRadius.ToString()));
            GsOuterOffset = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterOffset", GsOuterOffset.ToString()));
            GsOuterOutToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "OuterOutToIn", GsOuterOutToIn.ToString()));
            GsOuterRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterRange", GsOuterRange.ToString()));
            GsOuterWtoB = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterWtoB", GsOuterWtoB.ToString()));
            GsOuterGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterGv", GsOuterGv.ToString()));
            GsInnerOffset = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerOffset", GsInnerOffset.ToString()));
            GsInnerOutToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "InnerOutToIn", GsInnerOutToIn.ToString()));
            GsInnerRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerRange", GsInnerRange.ToString()));
            GsInnerWtoB = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerWtoB", GsInnerWtoB.ToString()));
            GsInnerGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerGv", GsInnerGv.ToString()));
            GsWidthThin = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthThin", GsWidthThin.ToString()));
            GsWidthThick = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthThick", GsWidthThick.ToString()));
            GsWidthLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthLength", GsWidthLength.ToString()));
            GsUseUniform = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseUniform", GsUseUniform.ToString()));
            GsFanSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FanSize", GsFanSize.ToString()));
            GsGvUniform = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvUniform", GsGvUniform.ToString()));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Scan", GsScan.ToString());
            XmlHelper.SetValue(paramElement, "Rate", GsRate.X.ToString());
            XmlHelper.SetValue(paramElement, "Radius", GsRadius.X.ToString());
            XmlHelper.SetValue(paramElement, "Range", GsRange.ToString());
            XmlHelper.SetValue(paramElement, "ToleranceP", GsRadiusTolP.ToString());
            XmlHelper.SetValue(paramElement, "ToleranceM", GsRadiusTolM.ToString());
            XmlHelper.SetValue(paramElement, "UseBumpy", GsUseBumpy.ToString());
            XmlHelper.SetValue(paramElement, "ScanBumpy", GsScanBumpy.ToString());
            XmlHelper.SetValue(paramElement, "BGSide", GsBGSide.ToString());
            XmlHelper.SetValue(paramElement, "Iteration", GsIteration.ToString());
            XmlHelper.SetValue(paramElement, "Depth", GsDepth.ToString());
            XmlHelper.SetValue(paramElement, "UseWidth", GsUseWidth.ToString());
            XmlHelper.SetValue(paramElement, "ScanWidth", GsScanWidth.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRadius", GsOffsetRadius.ToString());
            XmlHelper.SetValue(paramElement, "OuterOffset", GsOuterOffset.ToString());
            XmlHelper.SetValue(paramElement, "OuterOutToIn", GsOuterOutToIn.ToString());
            XmlHelper.SetValue(paramElement, "OuterRange", GsOuterRange.ToString());
            XmlHelper.SetValue(paramElement, "OuterWtoB", GsOuterWtoB.ToString());
            XmlHelper.SetValue(paramElement, "OuterGv", GsOuterGv.ToString());
            XmlHelper.SetValue(paramElement, "InnerOffset", GsInnerOffset.ToString());
            XmlHelper.SetValue(paramElement, "InnerOutToIn", GsInnerOutToIn.ToString());
            XmlHelper.SetValue(paramElement, "InnerRange", GsInnerRange.ToString());
            XmlHelper.SetValue(paramElement, "InnerWtoB", GsInnerWtoB.ToString());
            XmlHelper.SetValue(paramElement, "InnerGv", GsInnerGv.ToString());
            XmlHelper.SetValue(paramElement, "WidthThin", GsWidthThin.ToString());
            XmlHelper.SetValue(paramElement, "WidthThick", GsWidthThick.ToString());
            XmlHelper.SetValue(paramElement, "WidthLength", GsWidthLength.ToString());
            XmlHelper.SetValue(paramElement, "UseUniform", GsUseUniform.ToString());
            XmlHelper.SetValue(paramElement, "FanSize", GsFanSize.ToString());
            XmlHelper.SetValue(paramElement, "GvUniform", GsGvUniform.ToString());
        }

        public MsrCircle Clone()
        {
            var cloneCircle = new MsrCircle();
            cloneCircle.GsScan = GsScan;
            cloneCircle.GsRate = new Point(GsRate.X, cloneCircle.GsRate.Y);
            cloneCircle.GsRadius = new PointD(GsRadius.X, cloneCircle.GsRadius.Y);
            cloneCircle.GsRange = GsRange;
            cloneCircle.GsRadiusTolP = GsRadiusTolP;
            cloneCircle.GsRadiusTolM = GsRadiusTolM;
            cloneCircle.GsUseBumpy = GsUseBumpy;
            cloneCircle.GsScanBumpy = GsScanBumpy;
            cloneCircle.GsBGSide = GsBGSide;
            cloneCircle.GsIteration = GsIteration;
            cloneCircle.GsDepth = GsDepth;
            cloneCircle.GsUseWidth = GsUseWidth;
            cloneCircle.GsScanWidth = GsScanWidth;
            cloneCircle.GsOffsetRadius = GsOffsetRadius;
            cloneCircle.GsOuterOffset = GsOuterOffset;
            cloneCircle.GsOuterOutToIn = GsOuterOutToIn;
            cloneCircle.GsOuterRange = GsOuterRange;
            cloneCircle.GsOuterWtoB = GsOuterWtoB;
            cloneCircle.GsOuterGv = GsOuterGv;
            cloneCircle.GsInnerOffset = GsInnerOffset;
            cloneCircle.GsInnerOutToIn = GsInnerOutToIn;
            cloneCircle.GsInnerRange = GsInnerRange;
            cloneCircle.GsInnerWtoB = GsInnerWtoB;
            cloneCircle.GsInnerGv = GsInnerGv;
            cloneCircle.GsWidthThin = GsWidthThin;
            cloneCircle.GsWidthThick = GsWidthThick;
            cloneCircle.GsWidthLength = GsWidthLength;
            cloneCircle.GsUseUniform = GsUseUniform;
            cloneCircle.GsFanSize = GsFanSize;
            cloneCircle.GsGvUniform = GsGvUniform;

            return cloneCircle;
        }
        #endregion
    }

    public class MsrBlob : Msr
    {
        #region < Constructor >
        public MsrBlob()
        {
        }

        #endregion

        #region < Get/Set Function >
        public int GsBlobCount { get; set; } = 9;
        public int GsScan { get; set; } = 1;
        public int GsBgAvgGv { get; set; } = 128;
        public int GsGvLow { get; set; } = 0;
        public int GsGvHigh { get; set; } = 200;
        public int GsLinking { get; set; } = 3;
        public int GsSizeLow { get; set; } = 5;
        public int GsSizeHigh { get; set; } = 5;
        public int GsRadiusInner { get; set; } = 0;
        public int GsRadiusOuter { get; set; } = 0;
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if (GsUse)
            {
                if (GsScan <= 0)
                {
                    return false;
                }

                if ((GsGvLow < 0) || (GsGvHigh > 255))
                {
                    return false;
                }

                if (GsGvLow > GsGvHigh)
                {
                    return false;
                }

                if (GsLinking <= 0)
                {
                    return false;
                }

                if ((GsSizeLow < 1))
                {
                    return false;
                }

                if ((GsSizeHigh < 1))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetBlobName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Scan";
                case 1: return "BgAvgGv";
                case 2: return "GvLow";
                case 3: return "GvHigh";
                case 4: return "Linking";
                case 5: return "SizeLow";
                case 6: return "SizeHigh";
                case 7: return "RadiusInner";
                case 8: return "RadiusOuter";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetBlob(string strTitle, string strData)
        {
            if (SetMsr(strTitle, strData))
            {
                return true;
            }

            bool bEntry = false;
            for (int i = 0; i < GsBlobCount; i++)
            {
                if (false == strTitle.Equals(GetBlobName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0: GsScan = Convert.ToInt32(strData); break;
                    case 1: GsBgAvgGv = Convert.ToInt32(strData); break;
                    case 2: GsGvLow = Convert.ToInt32(strData); break;
                    case 3: GsGvHigh = Convert.ToInt32(strData); break;
                    case 4: GsLinking = Convert.ToInt32(strData); break;
                    case 5: GsSizeLow = Convert.ToInt32(strData); break;
                    case 6: GsSizeHigh = Convert.ToInt32(strData); break;
                    case 7: GsRadiusInner = Convert.ToInt32(strData); break;
                    case 8: GsRadiusOuter = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }

        public bool GetBlob(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsBlobCount))
            {
                return false;
            }

            strName = GetBlobName(nIndex);
            switch (nIndex)
            {
                case 0: strData = string.Format("{0}", GsScan); break;
                case 1: strData = string.Format("{0}", GsBgAvgGv); break;
                case 2: strData = string.Format("{0}", GsGvLow); break;
                case 3: strData = string.Format("{0}", GsGvHigh); break;
                case 4: strData = string.Format("{0}", GsLinking); break;
                case 5: strData = string.Format("{0}", GsSizeLow); break;
                case 6: strData = string.Format("{0}", GsSizeHigh); break;
                case 7: strData = string.Format("{0}", GsRadiusInner); break;
                case 8: strData = string.Format("{0}", GsRadiusOuter); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion

        public void LoadParam(XmlElement paramElement)
        {
            GsScan = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Scan", GsScan.ToString()));
            GsBgAvgGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "BgAvgGv", GsBgAvgGv.ToString()));
            GsGvLow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvLow", GsGvLow.ToString()));
            GsGvHigh = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvHigh", GsGvHigh.ToString()));
            GsLinking = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Linking", GsLinking.ToString()));
            GsSizeLow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SizeLow", GsSizeLow.ToString()));
            GsSizeHigh = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SizeHigh", GsSizeHigh.ToString()));
            GsRadiusInner = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RadiusInner", GsRadiusInner.ToString()));
            GsRadiusOuter = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RadiusOuter", GsRadiusOuter.ToString()));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Scan", GsScan.ToString());
            XmlHelper.SetValue(paramElement, "BgAvgGv", GsBgAvgGv.ToString());
            XmlHelper.SetValue(paramElement, "GvLow", GsGvLow.ToString());
            XmlHelper.SetValue(paramElement, "GvHigh", GsGvHigh.ToString());
            XmlHelper.SetValue(paramElement, "Linking", GsLinking.ToString());
            XmlHelper.SetValue(paramElement, "SizeLow", GsSizeLow.ToString());
            XmlHelper.SetValue(paramElement, "SizeHigh", GsSizeHigh.ToString());
            XmlHelper.SetValue(paramElement, "RadiusInner", GsRadiusInner.ToString());
            XmlHelper.SetValue(paramElement, "RadiusOuter", GsRadiusOuter.ToString());
        }

        public MsrBlob Clone()
        {
            var cloneBlob = new MsrBlob();
            cloneBlob.GsScan = GsScan;
            cloneBlob.GsBgAvgGv = GsBgAvgGv;
            cloneBlob.GsGvLow = GsGvLow;
            cloneBlob.GsGvHigh = GsGvHigh;
            cloneBlob.GsLinking = GsLinking;
            cloneBlob.GsSizeLow = GsSizeLow;
            cloneBlob.GsSizeHigh = GsSizeHigh;
            cloneBlob.GsRadiusInner = GsRadiusInner;
            cloneBlob.GsRadiusOuter = GsRadiusOuter;

            return cloneBlob;
        }
    }

    public class MsrMatch : Msr
    {
        #region < Constructor >
        public MsrMatch()
        {
        }

        #endregion
        #region < Variable and Default >
        private Point m_MatchEx = new Point(20, 20);
        #endregion

        #region < Get/Set Function >
        public int GsMatchCount { get; set; } = 3;
        public PointD GsScore { get; set; } = new PointD(60, 0);
        public double GetScoreIn()
        {
            return GsScore.X;
        }
        public void SetScoreIn(double dScore)
        {
            GsScore = new PointD(dScore, GsScore.Y);
        }
        public void SetScoreOut(double dScore)
        {
            GsScore = new PointD(GsScore.X, dScore);
        }
        public Point GsMatchEx
        {
            get => m_MatchEx;
            set => m_MatchEx = value;
        }
        public PointD GsAngle { get; set; } = new PointD(0, 0);
        public double GetAngleIn()
        {
            return GsAngle.X;
        }
        public void SetAngleIn(double dAngle)
        {
            GsAngle = new PointD(dAngle, GsScore.Y);
        }
        public OpenCvGreyImage GsTemplete { get; set; } = new OpenCvGreyImage();
        public string GsTempletePath { get; set; }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
            {
                return false;
            }

            if (GsUse)
            {
                if ((GsScore.X < 30) || (GsScore.X >= 100))
                {
                    return false;
                }

                if ((GsMatchEx.X < 1) || (GsMatchEx.Y < 1))
                {
                    return false;
                }

                if ((GsMatchEx.X > SizeX) || (GsMatchEx.Y > SizeY))
                {
                    return false;
                }
            }

            return true;
        }

        private string GetMatchName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Score";
                case 1: return "MatchEx";
                case 2: return "Angle";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetMatch(string strTitle, string strData)
        {
            if (SetMsr(strTitle, strData))
            {
                return true;
            }

            var helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < GsMatchCount; i++)
            {
                if (false == strTitle.Equals(GetMatchName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    case 0: SetScoreIn(Convert.ToDouble(strData)); break;
                    case 1: helper.ToSplit(strData, ref m_MatchEx); break;
                    case 2: SetAngleIn(Convert.ToDouble(strData)); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
            {
                return false;
            }

            return true;
        }

        public bool GetMatch(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= GsMatchCount))
            {
                return false;
            }

            strName = GetMatchName(nIndex);
            switch (nIndex)
            {
                case 0: strData = string.Format("{0}", GsScore.X); break;
                case 1: strData = string.Format("{0}, {1}", GsMatchEx.X, GsMatchEx.Y); break;
                case 2: strData = string.Format("{0}", GsAngle.X); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }
}
