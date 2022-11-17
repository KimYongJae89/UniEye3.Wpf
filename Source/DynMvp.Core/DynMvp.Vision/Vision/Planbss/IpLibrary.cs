using DynMvp.Base;
//using DynMvp.Base;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision.Planbss
{
    public class IpParam
    {
    }

    public class IpLibrary
    {
        public static EUSER gsUser { get; set; } = EUSER.OPERATOR;
        public long gsTime { get; set; } = 0;
        public StringBuilder gsError { get; set; } = new StringBuilder(256);

        protected virtual void Clear()
        {
            gsError.Clear();
        }

        protected bool IsValid(AlgoImage algoImage)
        {
            if (algoImage == null)
            {
                return false;
            }

            if (algoImage.ImageType == ImageType.Grey)
            {
                return true;
            }

            return true;
        }

        public bool MsrEdge(AlgoImage algoImage, MsrEdge msrEdge, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrEdge == null))
            {
                return false;
            }

            msrEdge.GsIsRoi = msrEdge.GsTcRoi;
            msrEdge.GsEdgeRoi = msrEdge.GsTcRoi;

            var actEdge = new ActMsrEdge(hDraw, gsError);
            bool bOk = actEdge.Find(algoImage, msrEdge);
            if (!bOk)
            {
                actEdge.ADDRect(Pens.Red, msrEdge.GsIsRoi, false);
            }
            else
            {
                PointD OutPt = msrEdge.GsEdgeOutPt;
                actEdge.ADDRect(Pens.LightGray, msrEdge.GsIsRoi, false);
                actEdge.ADDCross(Pens.LightGreen, OutPt, 3);
            }

            return bOk;
        }

        public bool MsrLine(AlgoImage algoImage, MsrLine msrLine, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrLine == null))
            {
                return false;
            }

            msrLine.GsIsRoi = msrLine.GsTcRoi;

            var actLine = new ActMsrLine(hDraw, gsError);
            bool bOk = actLine.Find(algoImage, msrLine);
            if (!bOk)
            {
                actLine.ADDRect(Pens.Red, msrLine.GsIsRoi, false);
            }
            else
            {
                actLine.ADDRect(Pens.LightGray, msrLine.GsIsRoi, false);

                var hMath = new HelpMath();

                var ptCross1 = new PointD(msrLine.GsIsRoi.Left, msrLine.GsIsRoi.Top);
                var ptCross2 = new PointD(msrLine.GsIsRoi.Right, msrLine.GsIsRoi.Bottom);
                PointD OutLT = hMath.GetLinePt(msrLine.GsOutPt, msrLine.GetAngleOut(), ptCross1);
                PointD OutRB = hMath.GetLinePt(msrLine.GsOutPt, msrLine.GetAngleOut(), ptCross2);
                if (IpLibrary.gsUser >= EUSER.ADMIN)
                {
                    actLine.ADDLine(Pens.Yellow, OutLT, OutRB);
                    actLine.ADDCross(Pens.Yellow, msrLine.GsOutPt, 5);
                }
            }

            //////////////////////////////////////////////////////////////////////////
            // Find Bumpy
            //////////////////////////////////////////////////////////////////////////
            /*
            if (msrLine.gsUseBumpy)
            {
                List<RECT> vBurrs = new List<RECT>();
                List<RECT> vBrokens = new List<RECT>();

                bool IsBummpy = actLine.IsThickness(ipImage, msrCircle, vBurrs, vBrokens);//, ref hDraw);

                if (vBurrs.Count > 0)
                {
                    for (int i = 0; i < vBurrs.Count; i++)
                        actCircle.ADDRect(Pens.Orange, vBurrs[i], false);
                }

                if (vBrokens.Count > 0)
                {
                    for (int i = 0; i < vBrokens.Count; i++)
                        actCircle.ADDRect(Pens.Red, vBrokens[i], false);
                }

                vBurrs.Clear();
                vBrokens.Clear();              
            }
            */

            return bOk;
        }

        public bool MsrCorner(AlgoImage algoImage, MsrCorner msrCorner, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrCorner == null))
            {
                return false;
            }

            //////////////////////////////////////////////////////////////////////////
            if (msrCorner.GsCardinal == CardinalPoint.NorthEast)
            {
                msrCorner.GsTcRoi = Rectangle.FromLTRB(146 - 60, 426 - 60,
                                                       146 + 60, 426 + 60);
            }
            //////////////////////////////////////////////////////////////////////////

            msrCorner.GsIsRoi = msrCorner.GsTcRoi;

            var actCorner = new ActMsrCorner(hDraw, gsError);
            bool bOk = actCorner.Find(algoImage, msrCorner);
            if (!bOk)
            {
                actCorner.ADDRect(Pens.Red, msrCorner.GsIsRoi, false);
            }
            else
            {
                actCorner.ADDCross(Pens.Blue, msrCorner.GsOutPt, 20);
                actCorner.ADDRect(Pens.LightGray, msrCorner.GsIsRoi, false);
            }

            return bOk;
        }

        public bool MsrQuadrangle(AlgoImage algoImage, MsrQuadrangle msrQuadrangle, DebugContext debugContext, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrQuadrangle == null))
            {
                return false;
            }

            //     algoImage.Save("Input.bmp", debugContext);         

            var Sw = new Stopwatch(); Sw.Start();

            /*
            //////////////////////////////////////////////////////////////////////////
            if (msrCorner.gsCardinal == eCARDINAL.NE)
            {
                msrCorner.gsTcRoi = new RECT(146 - 60, 426 - 60,
                                             146 + 60, 426 + 60);
            }
            //////////////////////////////////////////////////////////////////////////
            */
            msrQuadrangle.GsIsRoi = msrQuadrangle.GsTcRoi;

            var actQuadrangle = new ActMsrQuadrangle(hDraw, gsError);
            bool bOk = actQuadrangle.Find(algoImage, msrQuadrangle);
            if (!bOk)
            {
                actQuadrangle.ADDRect(Pens.Red, msrQuadrangle.GsIsRoi, false);
            }
            else
            {
                actQuadrangle.ADDCross(Pens.Blue, msrQuadrangle.GsOutPt, 20);
                actQuadrangle.ADDRect(Pens.LightGray, msrQuadrangle.GsIsRoi, false);
            }

            Sw.Stop(); gsTime = Sw.ElapsedMilliseconds;

            return bOk;
        }

        public bool MsrQuadrangle(AlgoImage algoImage, MsrQuadrangle msrQuadrangle, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrQuadrangle == null))
            {
                return false;
            }

            var Sw = new Stopwatch(); Sw.Start();

            /*
            //////////////////////////////////////////////////////////////////////////
            if (msrCorner.gsCardinal == eCARDINAL.NE)
            {
                msrCorner.gsTcRoi = new RECT(146 - 60, 426 - 60,
                                             146 + 60, 426 + 60);
            }
            //////////////////////////////////////////////////////////////////////////
            */
            msrQuadrangle.GsIsRoi = msrQuadrangle.GsTcRoi;

            var actQuadrangle = new ActMsrQuadrangle(hDraw, gsError);
            bool bOk = actQuadrangle.Find(algoImage, msrQuadrangle);
            if (!bOk)
            {
                actQuadrangle.ADDRect(Pens.Red, msrQuadrangle.GsIsRoi, false);
            }
            else
            {
                actQuadrangle.ADDCross(Pens.Blue, msrQuadrangle.GsOutPt, 20);
                actQuadrangle.ADDRect(Pens.LightGray, msrQuadrangle.GsIsRoi, false);
            }

            Sw.Stop(); gsTime = Sw.ElapsedMilliseconds;

            return bOk;
        }

        public bool MsrCircle(AlgoImage algoImage, MsrCircle msrCircle,
                                List<Rectangle> vWidthThin, List<Rectangle> vWidthThick,
                                List<Rectangle> vBumpyBurr, List<Rectangle> vBumpyBroken, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrCircle == null))
            {
                return false;
            }

            msrCircle.GsIsRoi = msrCircle.GsTcRoi;

            var actCircle = new ActMsrCircle(hDraw, gsError);
            bool bOk = actCircle.Find(algoImage, msrCircle);
            if (!bOk)
            {
                gsError.Append("!! Infomation !! [MsrCircle] Circle Finding is failed.");
                hDraw.AddRect(Pens.Red, msrCircle.GsTcRoi, false);
            }
            else
            {
                actCircle.ADDRect(Pens.LightCyan, msrCircle.GsTcRoi, false);
                actCircle.ADDRect(Pens.LightCyan, msrCircle.GetOutCircle(), true);

                if (gsUser == EUSER.DEVELOPER)
                {
                    PointD OutCenter = msrCircle.GsOutPt;
                    double OutRadius = msrCircle.GsRadius.Y;
                    double RadiusOuter = OutRadius + msrCircle.GsRange;
                    double RadiusInner = OutRadius - msrCircle.GsRange;
                    actCircle.ADDCross(Pens.LightCyan, OutCenter, (int)(msrCircle.GsRadius.Y + 0.5f));

                    var OneOuter = Rectangle.FromLTRB((int)((OutCenter.X - RadiusOuter) + 0.5f), (int)((OutCenter.Y - RadiusOuter) + 0.5f),
                                             (int)((OutCenter.X + RadiusOuter) + 0.5f), (int)((OutCenter.Y + RadiusOuter) + 0.5f));
                    actCircle.ADDRect(Pens.LightGray, OneOuter, true);
                    var OneInner = Rectangle.FromLTRB((int)((OutCenter.X - RadiusInner) + 0.5f), (int)((OutCenter.Y - RadiusInner) + 0.5f),
                                             (int)((OutCenter.X + RadiusInner) + 0.5f), (int)((OutCenter.Y + RadiusInner) + 0.5f));
                    actCircle.ADDRect(Pens.LightGray, OneInner, true);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Check Width Inspection
            ///////////////////////////////////////////////////////////////////////////////////////
            if (bOk && msrCircle.GsUseWidth)
            {
                bool bOkWidth = actCircle.IsThickness(algoImage, msrCircle, vWidthThin, vWidthThick);
                if (bOkWidth)
                {
                    for (int i = 0; i < vWidthThin.Count; i++)
                    {
                        actCircle.ADDRect(Pens.Red, vWidthThin[i], false);
                    }

                    for (int i = 0; i < vWidthThick.Count; i++)
                    {
                        actCircle.ADDRect(Pens.Red, vWidthThick[i], false);
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // Check Bumpy Inspection
            ///////////////////////////////////////////////////////////////////////////////////////
            if (bOk && msrCircle.GsUseBumpy)
            {
                bool bOkBumpy = actCircle.IsBumpy(algoImage, msrCircle, vBumpyBurr, vBumpyBroken);
                if (bOkBumpy)
                {
                    for (int i = 0; i < vBumpyBurr.Count; i++)
                    {
                        actCircle.ADDRect(Pens.Orange, vBumpyBurr[i], false);
                    }

                    for (int i = 0; i < vBumpyBroken.Count; i++)
                    {
                        actCircle.ADDRect(Pens.Red, vBumpyBroken[i], false);
                    }
                }
            }

            return bOk;
        }

        public bool MsrBlob(AlgoImage algoImage, MsrBlob msrBlob,
                                List<Rectangle> vLower, List<Rectangle> vUpper, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrBlob == null))
            {
                return false;
            }

            msrBlob.GsIsRoi = msrBlob.GsTcRoi;

            bool bOk = true;
            bool useCv = false;
            if (!useCv)
            {
                var actBlob = new ActMsrBlob(hDraw, gsError);
                bOk = actBlob.Find(algoImage, msrBlob, vLower, vUpper);
            }
            else
            {
                /*
                msrBlob.gsGvLow = 100;  // 0
                msrBlob.gsGvHigh = 255; // 100
                msrBlob.gsTcRoi = new Rectangle(20, 20, algoImage.Width - 40, algoImage.Height - 40);

                msrBlob.gsIsRoi = msrBlob.gsTcRoi;
                 * */

                var actOpenCv = new ActMsrOpenCv(hDraw, gsError);
                bOk = actOpenCv.Blob(algoImage, msrBlob, vLower, vUpper);
            }

            if (!bOk)
            {
                if (hDraw != null)
                {
                    hDraw.AddRect(Pens.Red, msrBlob.GsTcRoi, false);
                }
            }
            else
            {
                if (hDraw != null)
                {
                    hDraw.AddRect(Pens.LightCyan, msrBlob.GsTcRoi, false);

                    for (int i = 0; i < vLower.Count; i++)
                    {
                        hDraw.AddRect(Pens.Red, vLower[i], false);
                    }

                    for (int i = 0; i < vUpper.Count; i++)
                    {
                        hDraw.AddRect(Pens.Red, vUpper[i], false);
                    }
                }
            }

            return bOk;
        }

        public bool MsrMatch(AlgoImage algoImage, MsrMatch msrMatch, HelpDraw hDraw = null)
        {
            if (!IsValid(algoImage) || (msrMatch == null))
            {
                return false;
            }

            if (!msrMatch.GsUse)
            {
                return true;
            }

            if (msrMatch.GsTemplete.Image == null)
            {
                gsError.Append("!! Error !! [MsrMatch] Templete Image is empty.");
                return false;
            }

            int WTemplete = msrMatch.GsTemplete.Width;
            int HTemplete = msrMatch.GsTemplete.Height;
            if ((msrMatch.GsIsRoi.Width < WTemplete) || (msrMatch.GsIsRoi.Height < HTemplete))
            {
                //    msrMatch.gsIsRoi = msrMatch.gsTcRoi.Inflate(msrMatch.gsMatchEx.X,
                //                                                msrMatch.gsMatchEx.Y);

                msrMatch.GsIsRoi = Rectangle.Inflate(msrMatch.GsTcRoi, msrMatch.GsMatchEx.X, msrMatch.GsMatchEx.Y);
            }

            bool bOk = true;
            bool useCv = true;
            if (!useCv)
            {
                var actMatch = new ActMsrMatch(hDraw, gsError);
                bOk = actMatch.Find(algoImage, ref msrMatch);
                gsError.Append("!! Error !! [MsrMatch] Not Support.");
            }
            else
            {
                var actOpenCv = new ActMsrOpenCv(hDraw, gsError);
                bOk = actOpenCv.MatchNcc(algoImage, ref msrMatch);
            }
            if (!bOk)
            {
                return false;
            }

            PointD Score = msrMatch.GsScore;
            if (Score.X > Score.Y)
            {
                bOk = false;
                gsError.Append("!! Information !! [MsrMatch] Score is not enough.");
                if (hDraw != null)
                {
                    hDraw.AddRect(Pens.Red, msrMatch.GsIsRoi, false);
                }
            }
            else
            {
                Rectangle tcTemplete = msrMatch.GsTcRoi;
                var isTemplete = Rectangle.FromLTRB((int)(msrMatch.GsOutPt.X - tcTemplete.Width / 2),
                                                          (int)(msrMatch.GsOutPt.Y - tcTemplete.Height / 2),
                                                          (int)(msrMatch.GsOutPt.X + tcTemplete.Width / 2),
                                                          (int)(msrMatch.GsOutPt.Y + tcTemplete.Height / 2));

                ///////////////////////////////////////////////////////////////////////////////////
                // Check Bound
                ///////////////////////////////////////////////////////////////////////////////////
                Rectangle resultRoi = isTemplete;
                Rectangle searchRoi = msrMatch.GsIsRoi;
                if ((resultRoi.Left <= searchRoi.Left) || (resultRoi.Right >= searchRoi.Right) ||
                    (resultRoi.Top <= searchRoi.Top) || (resultRoi.Bottom >= searchRoi.Bottom))
                {
                    bOk = false;
                    gsError.Append("!! Information !! [MsrMatch] Inspection Templete is bound.");
                    if (hDraw != null)
                    {
                        hDraw.AddRect(Pens.LightGreen, searchRoi, false);
                        hDraw.AddRect(Pens.Red, resultRoi, false);
                    }
                }
                else
                {
                    if (hDraw != null)
                    {
                        hDraw.AddRect(Pens.LightGreen, isTemplete, false);
                        hDraw.AddCross(Pens.LightGreen, msrMatch.GsOutPt, 5);
                        hDraw.AddRect(Pens.LightGray, msrMatch.GsIsRoi, false);
                    }
                }
            }

            return bOk;
        }

        public bool MsrOpenCv(AlgoImage algoImage, HelpDraw hDraw = null)
        {
            var actOpenCv = new ActMsrOpenCv(hDraw, gsError);

            string strSave = "D:\\STW\\OutImage.bmp";
            if (true)
            {
                AlgoImage resImage = actOpenCv.Resize(algoImage,
                                                      algoImage.Width / 2,
                                                      algoImage.Height / 2);
                if (resImage != null)
                {
                    resImage.Save(strSave, null);
                }
            }

            return true;
        }
    }

    public class VCFpcbParam : IpParam
    {
        #region < Constructor >
        public VCFpcbParam()
        {
        }

        #endregion
        #region < Variable and Default >
        public MsrMatch m_Match = new MsrMatch();
        public MsrQuadrangle m_Quadrangle = new MsrQuadrangle();
        #endregion

        #region < Get/Set Function >
        public int gsFpcbCount { get; set; } = 15;
        public MsrMatch gsMatch
        {
            get => m_Match;
            set => m_Match = value;
        }
        public MsrQuadrangle gsQuadrangle
        {
            get => m_Quadrangle;
            set => m_Quadrangle = value;
        }
        #endregion

        private string GetVcFpcbName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                // Match
                case 0: return "TcRoi";
                case 1: return "IsRoi";
                case 2: return "Score";
                case 3: return "MatchEx";
                // Quadrangle
                case 4: return "WtoB";
                case 5: return "ThickW";
                case 6: return "ThickH";
                case 7: return "Distance";
                case 8: return "GrayValue";
                case 9: return "Scan";
                case 10: return "Rate";
                case 11: return "Convex";
                case 12: return "OutToIn";
                case 13: return "Range";
                case 14: return "Length";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetVcFpcb(string strTitle, string strData)
        {
            var helper = new HelpMath();
            var point = new Point(0, 0);
            var rectangle = new Rectangle(0, 0, 0, 0);

            bool bEntry = false;
            for (int i = 0; i < gsFpcbCount; i++)
            {
                if (false == strTitle.Equals(GetVcFpcbName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    // Match
                    case 0: helper.ToSplit(strData, ref rectangle); gsMatch.GsTcRoi = rectangle; break;
                    case 1: helper.ToSplit(strData, ref rectangle); gsMatch.GsIsRoi = rectangle; break;
                    case 2: gsMatch.SetScoreIn(Convert.ToDouble(strData)); break;
                    case 3: helper.ToSplit(strData, ref point); gsMatch.GsMatchEx = point; break;
                    // Quadrangle
                    case 4: gsQuadrangle.GsWtoB = Convert.ToInt32(strData); break;
                    case 5: gsQuadrangle.GsThickW = Convert.ToInt32(strData); break;
                    case 6: gsQuadrangle.GsThickH = Convert.ToInt32(strData); break;
                    case 7: gsQuadrangle.GsDistance = Convert.ToInt32(strData); break;
                    case 8: gsQuadrangle.GsGv = Convert.ToInt32(strData); break;
                    case 9: gsQuadrangle.GsScan = Convert.ToInt32(strData); break;
                    case 10: gsQuadrangle.SetRateIn(Convert.ToInt32(strData)); break;
                    case 11: gsQuadrangle.GsConvex = (ConvexShape)(Convert.ToInt32(strData)); break;
                    case 12: gsQuadrangle.GsOtuToIn = helper.ToSplit(strData); break;
                    case 13: gsQuadrangle.GsRange = Convert.ToInt32(strData); break;
                    case 14: gsQuadrangle.GsLength = Convert.ToInt32(strData); break;
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

        public bool GetVcFpcb(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= gsFpcbCount))
            {
                return false;
            }

            strName = GetVcFpcbName(nIndex);
            switch (nIndex)
            {
                // Match
                case 0:
                    strData = string.Format("{0}, {1}, {2}, {3}", gsMatch.GsTcRoi.Left, gsMatch.GsTcRoi.Top,
                                                                 gsMatch.GsTcRoi.Right, gsMatch.GsTcRoi.Bottom);
                    break;
                case 1:
                    strData = string.Format("{0}, {1}, {2}, {3}", gsMatch.GsIsRoi.Left, gsMatch.GsIsRoi.Top,
                                                                 gsMatch.GsIsRoi.Right, gsMatch.GsIsRoi.Bottom);
                    break;
                case 2: strData = string.Format("{0}", gsMatch.GsScore.X); break;
                case 3: strData = string.Format("{0}, {1}", gsMatch.GsMatchEx.X, gsMatch.GsMatchEx.Y); break;
                // Qurdrangle
                case 4: strData = string.Format("{0}", gsQuadrangle.GsWtoB); break;
                case 5: strData = string.Format("{0}", gsQuadrangle.GsThickW); break;
                case 6: strData = string.Format("{0}", gsQuadrangle.GsThickH); break;
                case 7: strData = string.Format("{0}", gsQuadrangle.GsDistance); break;
                case 8: strData = string.Format("{0}", gsQuadrangle.GsGv); break;
                case 9: strData = string.Format("{0}", gsQuadrangle.GsScan); break;
                case 10: strData = string.Format("{0}", gsQuadrangle.GsRate.X); break;
                case 11: strData = string.Format("{0}", gsQuadrangle.GsConvex); break;
                case 12: strData = string.Format("{0}", gsQuadrangle.GsOtuToIn); break;
                case 13: strData = string.Format("{0}", gsQuadrangle.GsRange); break;
                case 14: strData = string.Format("{0}", gsQuadrangle.GsLength); break;
                default:
                    return false;
            }

            return true;
        }
    }

    public class VCFpcb : IpLibrary
    {
        #region < Constructor >
        public VCFpcb()
        {
        }
        #endregion

        #region < Get/Set Function >
        public Rectangle gsOutArea { get; set; } = new Rectangle(0, 0, 0, 0);
        #endregion

        protected override void Clear()
        {
            base.Clear();
            gsOutArea = Rectangle.FromLTRB(0, 0, 0, 0);
        }

        public void Caller()
        {
            /*
            AlgoImage probeClipImage = inspectParam.ProbeClipImage;
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RectangleF imageRegionInFov = inspectParam.ImageRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RotatedRect probeRegionInImage = new RotatedRect(probeRegionInFov);
            probeRegionInImage.Offset(-imageRegionInFov.X, -imageRegionInFov.Y);

            AlgorithmResult algorithmResult = new AlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov.GetBoundRect();

            ///////////////////////////////////////////////////////////////////////////////////////
            // Vision Initialize
            ///////////////////////////////////////////////////////////////////////////////////////  
            bool bAuto = false;                // Graphic use or ignore
            eUSER eUser = eUSER.DEVELOPER;      // Set User Mode

            VCFpcb vcFpcb = new VCFpcb();
            VCFpcbParam vcFpcbParam = new VCFpcbParam();
            HelpDraw helpDraw = new HelpDraw();

            VCLens.gsUser = eUser;
            vcFpcbParam.gsMatch.gsUse = false;
            vcFpcbParam.gsQuadrangle.gsTcRoi = Rectangle.FromLTRB((int)probeRegionInImage.Left, (int)probeRegionInImage.Top,
                                                                  (int)probeRegionInImage.Right, (int)probeRegionInImage.Bottom);

            ///////////////////////////////////////////////////////////////////////////////////////
            // Connect Parameter
            ///////////////////////////////////////////////////////////////////////////////////////  
            switch (Param.EdgeType)
            {
                case EdgeType.DarkToLight: vcFpcbParam.gsQuadrangle.gsWtoB = 2; break;
                case EdgeType.LightToDark: vcFpcbParam.gsQuadrangle.gsWtoB = 1; break;
                default:
                    vcFpcbParam.gsQuadrangle.gsWtoB = 0;
                    break;
            }

            vcFpcbParam.gsQuadrangle.gsGv = Param.GrayValue;          // 20
            vcFpcbParam.gsQuadrangle.gsScan = Param.ProjectionHeight;   // 3
            vcFpcbParam.gsQuadrangle.SetRateIn(Param.PassRate);             // 30
            vcFpcbParam.gsQuadrangle.gsCardinal = Param.CardinalPoint;      // NE
            vcFpcbParam.gsQuadrangle.gsOtuToIn = Param.OutToIn;            // true
            vcFpcbParam.gsQuadrangle.gsRange = Param.SearchRange;        // 20
            vcFpcbParam.gsQuadrangle.gsLength = Param.SearchLength;       // 70
            vcFpcbParam.gsQuadrangle.gsConvex = Param.ConvexShape;        // None

            ///////////////////////////////////////////////////////////////////////////////////////
            // Vision Inspection and Result
            ///////////////////////////////////////////////////////////////////////////////////////            
            int vResult = vcFpcb.Inspection(probeClipImage, vcFpcbParam, debugContext, bAuto ? null : helpDraw);

            int offsetX = (int)imageRegionInFov.X;
            int offsetY = (int)imageRegionInFov.Y;
            double outAngle = vcFpcbParam.gsQuadrangle.gsOutAngle;
            PointF outCenter = new PointF((float)vcFpcbParam.gsQuadrangle.gsOutPt.dX + offsetX,
                                              (float)vcFpcbParam.gsQuadrangle.gsOutPt.dY + offsetY);

            algorithmResult.Good = (vResult == 1) ? true : false;
            algorithmResult.AddResultValue(new AlgorithmResultValue("Result", algorithmResult.Good));
            if (algorithmResult.Good == false)
                algorithmResult.AddResultValue(new AlgorithmResultValue("Message", vcFpcb.gsError));
            algorithmResult.AddResultValue(new AlgorithmResultValue("CenterPos", outCenter));
            algorithmResult.AddResultValue(new AlgorithmResultValue("Angle", outAngle));

            ///////////////////////////////////////////////////////////////////////////////////////
            // Display Graphics
            /////////////////////////////////////////////////////////////////////////////////////// 
            long countRect = helpDraw.SizeRect();
            for (int i = 0; i < countRect; i++)
            {
                DrawRect drRect = helpDraw.GetRect(i);

                Rectangle Area = drRect.mrArea;
                Rectangle rect = new Rectangle(offsetX + Area.Left, offsetY + Area.Top, Area.Width, Area.Height);
                if (drRect.mbCircle) algorithmResult.ResultFigureGroup.AddFigure(new EllipseFigure(rect, drRect.gsColor));
                else algorithmResult.ResultFigureGroup.AddFigure(new RectangleFigure(rect, drRect.gsColor));
            }

            long countCross = helpDraw.SizeCross();
            for (int i = 0; i < countCross; i++)
            {
                DrawCross drCross = helpDraw.GetCross(i);

                Point pt = new Point(drCross.mpt.X + offsetX, drCross.mpt.Y + offsetY);
                CrossFigure crossFigure = new CrossFigure(pt, drCross.mnLength, drCross.gsColor);
                algorithmResult.ResultFigureGroup.AddFigure(crossFigure);
            }

            long countLine = helpDraw.SizeLine();
            for (int i = 0; i < countLine; i++)
            {
                DrawLine drLine = helpDraw.GetLine(i);

                Point pt1 = new Point(drLine.mpt1.X + offsetX, drLine.mpt1.Y + offsetY);
                Point pt2 = new Point(drLine.mpt2.X + offsetX, drLine.mpt2.Y + offsetY);
                LineFigure lineFigure = new LineFigure(pt1, pt2, drLine.gsColor);
                algorithmResult.ResultFigureGroup.AddFigure(lineFigure);
            }

            helpDraw.Clear();

            return algorithmResult;
            */
        }

        public int Inspection(AlgoImage algoImage, VCFpcbParam param, DebugContext dContext, HelpDraw hDraw = null)
        {
            Clear();
            if (!IsValid(algoImage) || (param == null))
            {
                return -1;
            }

            int res = 1;    // -1 : Error, 0 : Ng, 1 : Ok

            ///////////////////////////////////////////////////////////////////////////////////////
            // 1. Align by Matching
            ///////////////////////////////////////////////////////////////////////////////////////
            int resAlign = Align(algoImage, param, hDraw);
            if (resAlign < 1)
            {
                return resAlign;        // Failed.! Align Blob 
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // 2. Find Quadrangle
            ///////////////////////////////////////////////////////////////////////////////////////
            int resQuadrangle = CheckQuadrangle(algoImage, param, hDraw);
            if (resQuadrangle < 0)
            {
                return -1;
            }

            return res;
        }

        private int Align(AlgoImage algoImage, VCFpcbParam param, HelpDraw hDraw = null)
        {
            if (param.gsMatch == null)
            {
                return -1;
            }

            bool bOk = MsrMatch(algoImage, param.gsMatch, hDraw);
            if (!bOk)
            {
                return 0;
            }

            return 1;
        }

        private int CheckQuadrangle(AlgoImage algoImage, VCFpcbParam param, HelpDraw hDraw = null)
        {
            if (param.gsQuadrangle == null)
            {
                return -1;
            }

            param.gsQuadrangle.GsIsRoi = param.gsQuadrangle.GsTcRoi;

            var actQuadrangle = new ActMsrQuadrangle(hDraw, gsError);

            bool bOk = actQuadrangle.Find(algoImage, param.gsQuadrangle);
            if (!bOk)
            {
                return -1;
            }

            // Compare Result or Spec.

            return 1;
        }
    }

    public class VCLensParam : IpParam
    {
        #region < Constructor >   
        public VCLensParam()
        {
            Initialize();
        }

        #endregion
        #region < Variable and Default >
        private Rectangle m_AlignArea = new Rectangle(0, 0, 0, 0);    // Align Area by User Tracker

        public MsrBlob m_BlobAlign = new MsrBlob();
        public MsrCircle m_CircleWidth = new MsrCircle();
        public MsrBlob m_BlobFmOuter = new MsrBlob();
        public MsrBlob m_BlobFmMiddle = new MsrBlob();
        public MsrBlob m_BlobFmInner = new MsrBlob();
        #endregion

        #region < Get/Set Function >
        public int gsLensCount { get; set; } = 49;
        public Rectangle gsAlignArea
        {
            get => m_AlignArea;
            set => m_AlignArea = value;
        }
        public int gsBgThreshold { get; set; } = 50;
        public MsrBlob gsBlobAlign
        {
            get => m_BlobAlign;
            set => m_BlobAlign = value;
        }
        public MsrCircle gsCircleWidth
        {
            get => m_CircleWidth;
            set => m_CircleWidth = value;
        }
        public MsrBlob gsBlobFmOuter
        {
            get => m_BlobFmOuter;
            set => m_BlobFmOuter = value;
        }
        public MsrBlob gsBlobFmMiddle
        {
            get => m_BlobFmMiddle;
            set => m_BlobFmMiddle = value;
        }
        public MsrBlob gsBlobFmInner
        {
            get => m_BlobFmInner;
            set => m_BlobFmInner = value;
        }
        #endregion

        private void Initialize()
        {
            gsLensCount = 50;
            m_AlignArea = new Rectangle(0, 0, 0, 0);    // Align Area by User Tracker
            gsBgThreshold = 50;                         // Background Threshold

            //m_BlobAlign = new MsrBlob();
            //m_BlobAlign.gsUse = true;
            //m_BlobAlign.gsScan = 2;
            //m_BlobAlign.gsBgAvgGv = 128;
            //m_BlobAlign.gsLinking = 100;
            //m_BlobAlign.gsGvLow = 0;
            //m_BlobAlign.gsGvHigh = 45;
            //m_BlobAlign.gsSizeLow = 15;
            //m_BlobAlign.gsSizeHigh = 15;

            m_CircleWidth = new MsrCircle();
            m_CircleWidth.GsUse = true;
            m_CircleWidth.GsScan = 8;
            m_CircleWidth.SetRateIn(60);
            m_CircleWidth.SetRadiusIn(650);
            m_CircleWidth.GsRange = 24;

            m_CircleWidth.GsUseWidth = true;
            m_CircleWidth.GsOffsetRadius = 0;
            m_CircleWidth.GsOuterOffset = 1;
            m_CircleWidth.GsOuterOutToIn = true;
            m_CircleWidth.GsOuterRange = 15;
            m_CircleWidth.GsOuterWtoB = 2;
            m_CircleWidth.GsOuterGv = 15;
            m_CircleWidth.GsInnerOffset = 2;
            m_CircleWidth.GsInnerOutToIn = false;
            m_CircleWidth.GsInnerRange = 15;
            m_CircleWidth.GsInnerWtoB = 1;
            m_CircleWidth.GsInnerGv = 15;
            m_CircleWidth.GsWidthThin = 0;
            m_CircleWidth.GsWidthThick = 10;
            m_CircleWidth.GsWidthLength = 20;

            m_BlobFmOuter = new MsrBlob();
            m_BlobFmOuter.GsUse = true;
            m_BlobFmOuter.GsScan = 1;
            m_BlobFmOuter.GsGvLow = 0;
            m_BlobFmOuter.GsGvHigh = 160;
            m_BlobFmOuter.GsLinking = 1;
            m_BlobFmOuter.GsSizeLow = 1;
            m_BlobFmOuter.GsSizeHigh = 1;
            m_BlobFmOuter.GsRadiusInner = 600;
            m_BlobFmOuter.GsRadiusOuter = 635;

            m_BlobFmMiddle = new MsrBlob();
            m_BlobFmMiddle.GsUse = true;
            m_BlobFmMiddle.GsScan = 1;
            m_BlobFmMiddle.GsGvLow = 0;
            m_BlobFmMiddle.GsGvHigh = 140;
            m_BlobFmMiddle.GsLinking = 3;
            m_BlobFmMiddle.GsSizeLow = 1;
            m_BlobFmMiddle.GsSizeHigh = 1;
            m_BlobFmMiddle.GsRadiusInner = 150;
            m_BlobFmMiddle.GsRadiusOuter = 600;

            m_BlobFmInner = new MsrBlob();
            m_BlobFmInner.GsUse = true;
            m_BlobFmInner.GsScan = 1;
            m_BlobFmInner.GsGvLow = 0;
            m_BlobFmInner.GsGvHigh = 180;
            m_BlobFmInner.GsLinking = 3;
            m_BlobFmInner.GsSizeLow = 1;
            m_BlobFmInner.GsSizeHigh = 1;
            m_BlobFmInner.GsRadiusInner = 10;
            m_BlobFmInner.GsRadiusOuter = 140;
        }

        private string GetVcLensName(int nIndex)
        {
            string strTitle = "Unknown";

            switch (nIndex)
            {
                // Align
                case 0: return "Align Area";
                case 1: return "Background Gv";
                // Circle
                case 2: return "Circle Use";
                case 3: return "Circle Scan";
                case 4: return "Circle Rate";
                case 5: return "Circle Radius";
                case 6: return "Circle Range";
                // Width
                case 7: return "Width Use";
                case 8: return "Width Scan";
                case 9: return "Width OffsetRadius";
                case 10: return "Width OuterOffset";
                case 11: return "Width OuterOutToIn";
                case 12: return "Width OuterRange";
                case 13: return "Width OuterWtoB";
                case 14: return "Width OuterGv";
                case 15: return "Width InnerOffset";
                case 16: return "Width InnerrOutToIn";
                case 17: return "Width InnerRange";
                case 18: return "Width InnerWtoB";
                case 19: return "Width InnerGv";
                case 20: return "Width Thin";
                case 21: return "Width Thick";
                case 22: return "Width Length";
                // Fm-Outer
                case 23: return "FmOuter Use";
                case 24: return "FmOuter Scan";
                case 25: return "FmOuter GvLow";
                case 26: return "FmOuter GvHigh";
                case 27: return "FmOuter Linking";
                case 28: return "FmOuter SizeLow";
                case 29: return "FmOuter SizeHigh";
                case 30: return "FmOuter RadiusInner";
                case 31: return "FmOuter RadiusOuter";
                // Fm-Middle
                case 32: return "FmMiddle Use";
                case 33: return "FmMiddle Scan";
                case 34: return "FmMiddle GvLow";
                case 35: return "FmMiddle GvHigh";
                case 36: return "FmMiddle Linking";
                case 37: return "FmMiddle SizeLow";
                case 38: return "FmMiddle SizeHigh";
                case 39: return "FmMiddle RadiusInner";
                case 40: return "FmMiddle RadiusOuter";
                // Fm-Inner
                case 41: return "FmInner Use";
                case 42: return "FmInner Scan";
                case 43: return "FmInner GvLow";
                case 44: return "FmInner GvHigh";
                case 45: return "FmInner Linking";
                case 46: return "FmInner SizeLow";
                case 47: return "FmInner SizeHigh";
                case 48: return "FmInner RadiusInner";
                case 49: return "FmInner RadiusOuter";

                default:
                    break;
            }

            return strTitle;
        }

        public bool SetVcLens(string strTitle, string strData)
        {
            var helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < gsLensCount; i++)
            {
                if (false == strTitle.Equals(GetVcLensName(i)))
                {
                    continue;
                }

                switch (i)
                {
                    // Align
                    case 0: helper.ToSplit(strData, ref m_AlignArea); break;
                    case 1: gsBgThreshold = Convert.ToInt32(strData); break;
                    // Circle
                    case 2: gsCircleWidth.GsUse = helper.ToSplit(strData); break;
                    case 3: gsCircleWidth.GsScan = Convert.ToInt32(strData); break;
                    case 4: gsCircleWidth.SetRateIn(Convert.ToInt32(strData)); break;
                    case 5: gsCircleWidth.SetRadiusIn(Convert.ToDouble(strData)); break;
                    case 6: gsCircleWidth.GsRange = Convert.ToInt32(strData); break;
                    // Width
                    case 7: gsCircleWidth.GsUseWidth = helper.ToSplit(strData); break;
                    case 8: gsCircleWidth.GsScanWidth = Convert.ToInt32(strData); break;
                    case 9: gsCircleWidth.GsOffsetRadius = Convert.ToInt32(strData); break;
                    case 10: gsCircleWidth.GsOuterOffset = Convert.ToInt32(strData); break;
                    case 11: gsCircleWidth.GsOuterOutToIn = helper.ToSplit(strData); break;
                    case 12: gsCircleWidth.GsOuterRange = Convert.ToInt32(strData); break;
                    case 13: gsCircleWidth.GsOuterWtoB = Convert.ToInt32(strData); break;
                    case 14: gsCircleWidth.GsOuterGv = Convert.ToInt32(strData); break;
                    case 15: gsCircleWidth.GsInnerOffset = Convert.ToInt32(strData); break;
                    case 16: gsCircleWidth.GsInnerOutToIn = helper.ToSplit(strData); break;
                    case 17: gsCircleWidth.GsInnerRange = Convert.ToInt32(strData); break;
                    case 18: gsCircleWidth.GsInnerWtoB = Convert.ToInt32(strData); break;
                    case 19: gsCircleWidth.GsInnerGv = Convert.ToInt32(strData); break;
                    case 20: gsCircleWidth.GsWidthThin = Convert.ToInt32(strData); break;
                    case 21: gsCircleWidth.GsWidthThick = Convert.ToInt32(strData); break;
                    case 22: gsCircleWidth.GsWidthLength = Convert.ToInt32(strData); break;
                    // Fm-Outer
                    case 23: gsBlobFmOuter.GsUse = helper.ToSplit(strData); break;
                    case 24: gsBlobFmOuter.GsScan = Convert.ToInt32(strData); break;
                    case 25: gsBlobFmOuter.GsGvLow = Convert.ToInt32(strData); break;
                    case 26: gsBlobFmOuter.GsGvHigh = Convert.ToInt32(strData); break;
                    case 27: gsBlobFmOuter.GsLinking = Convert.ToInt32(strData); break;
                    case 28: gsBlobFmOuter.GsSizeLow = Convert.ToInt32(strData); break;
                    case 29: gsBlobFmOuter.GsSizeHigh = Convert.ToInt32(strData); break;
                    case 30: gsBlobFmOuter.GsRadiusInner = Convert.ToInt32(strData); break;
                    case 31: gsBlobFmOuter.GsRadiusOuter = Convert.ToInt32(strData); break;
                    // Fm-Middle
                    case 32: gsBlobFmMiddle.GsUse = helper.ToSplit(strData); break;
                    case 33: gsBlobFmMiddle.GsScan = Convert.ToInt32(strData); break;
                    case 34: gsBlobFmMiddle.GsGvLow = Convert.ToInt32(strData); break;
                    case 35: gsBlobFmMiddle.GsGvHigh = Convert.ToInt32(strData); break;
                    case 36: gsBlobFmMiddle.GsLinking = Convert.ToInt32(strData); break;
                    case 37: gsBlobFmMiddle.GsSizeLow = Convert.ToInt32(strData); break;
                    case 38: gsBlobFmMiddle.GsSizeHigh = Convert.ToInt32(strData); break;
                    case 39: gsBlobFmMiddle.GsRadiusInner = Convert.ToInt32(strData); break;
                    case 40: gsBlobFmMiddle.GsRadiusOuter = Convert.ToInt32(strData); break;
                    // Fm-Inner
                    case 41: gsBlobFmInner.GsUse = helper.ToSplit(strData); break;
                    case 42: gsBlobFmInner.GsScan = Convert.ToInt32(strData); break;
                    case 43: gsBlobFmInner.GsGvLow = Convert.ToInt32(strData); break;
                    case 44: gsBlobFmInner.GsGvHigh = Convert.ToInt32(strData); break;
                    case 45: gsBlobFmInner.GsLinking = Convert.ToInt32(strData); break;
                    case 46: gsBlobFmInner.GsSizeLow = Convert.ToInt32(strData); break;
                    case 47: gsBlobFmInner.GsSizeHigh = Convert.ToInt32(strData); break;
                    case 48: gsBlobFmInner.GsRadiusInner = Convert.ToInt32(strData); break;
                    case 49: gsBlobFmInner.GsRadiusOuter = Convert.ToInt32(strData); break;
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

        public bool GetVcLens(int nIndex, ref string strName, ref string strData)
        {
            if ((nIndex < 0) || (nIndex >= gsLensCount))
            {
                return false;
            }

            strName = GetVcLensName(nIndex);
            switch (nIndex)
            {
                // Align
                case 0:
                    strData = string.Format("{0}, {1}, {2}, {3}", gsAlignArea.Left, gsAlignArea.Top,
                                                                  gsAlignArea.Right, gsAlignArea.Bottom);
                    break;
                case 1: strData = string.Format("{0}", gsBgThreshold); break;
                // Circle
                case 2: strData = string.Format("{0}", (gsCircleWidth.GsUse == true) ? 1 : 0); break;
                case 3: strData = string.Format("{0}", gsCircleWidth.GsScan); break;
                case 4: strData = string.Format("{0}", gsCircleWidth.GsRate.X); break;
                case 5: strData = string.Format("{0}", gsCircleWidth.GsRadius.X); break;
                case 6: strData = string.Format("{0}", gsCircleWidth.GsRange); break;
                // Width
                case 7: strData = string.Format("{0}", (gsCircleWidth.GsUseWidth == true) ? 1 : 0); break;
                case 8: strData = string.Format("{0}", gsCircleWidth.GsScanWidth); break;
                case 9: strData = string.Format("{0}", gsCircleWidth.GsOffsetRadius); break;
                case 10: strData = string.Format("{0}", gsCircleWidth.GsOuterOffset); break;
                case 11: strData = string.Format("{0}", gsCircleWidth.GsOuterOutToIn); break;
                case 12: strData = string.Format("{0}", gsCircleWidth.GsOuterRange); break;
                case 13: strData = string.Format("{0}", gsCircleWidth.GsOuterWtoB); break;
                case 14: strData = string.Format("{0}", gsCircleWidth.GsOuterGv); break;
                case 15: strData = string.Format("{0}", gsCircleWidth.GsInnerOffset); break;
                case 16: strData = string.Format("{0}", gsCircleWidth.GsInnerOutToIn); break;
                case 17: strData = string.Format("{0}", gsCircleWidth.GsInnerRange); break;
                case 18: strData = string.Format("{0}", gsCircleWidth.GsInnerWtoB); break;
                case 19: strData = string.Format("{0}", gsCircleWidth.GsInnerGv); break;
                case 20: strData = string.Format("{0}", gsCircleWidth.GsWidthThin); break;
                case 21: strData = string.Format("{0}", gsCircleWidth.GsWidthThick); break;
                case 22: strData = string.Format("{0}", gsCircleWidth.GsWidthLength); break;
                // Fm-Outer
                case 23: strData = string.Format("{0}", (gsBlobFmOuter.GsUse == true) ? 1 : 0); break;
                case 24: strData = string.Format("{0}", gsBlobFmOuter.GsScan); break;
                case 25: strData = string.Format("{0}", gsBlobFmOuter.GsGvLow); break;
                case 26: strData = string.Format("{0}", gsBlobFmOuter.GsGvHigh); break;
                case 27: strData = string.Format("{0}", gsBlobFmOuter.GsLinking); break;
                case 28: strData = string.Format("{0}", gsBlobFmOuter.GsSizeLow); break;
                case 29: strData = string.Format("{0}", gsBlobFmOuter.GsSizeHigh); break;
                case 30: strData = string.Format("{0}", gsBlobFmOuter.GsRadiusInner); break;
                case 31: strData = string.Format("{0}", gsBlobFmOuter.GsRadiusOuter); break;
                // Fm-Middle
                case 32: strData = string.Format("{0}", (gsBlobFmMiddle.GsUse == true) ? 1 : 0); break;
                case 33: strData = string.Format("{0}", gsBlobFmMiddle.GsScan); break;
                case 34: strData = string.Format("{0}", gsBlobFmMiddle.GsGvLow); break;
                case 35: strData = string.Format("{0}", gsBlobFmMiddle.GsGvHigh); break;
                case 36: strData = string.Format("{0}", gsBlobFmMiddle.GsLinking); break;
                case 37: strData = string.Format("{0}", gsBlobFmMiddle.GsSizeLow); break;
                case 38: strData = string.Format("{0}", gsBlobFmMiddle.GsSizeHigh); break;
                case 39: strData = string.Format("{0}", gsBlobFmMiddle.GsRadiusInner); break;
                case 40: strData = string.Format("{0}", gsBlobFmMiddle.GsRadiusOuter); break;
                // Fm-Inner
                case 41: strData = string.Format("{0}", (gsBlobFmInner.GsUse == true) ? 1 : 0); break;
                case 42: strData = string.Format("{0}", gsBlobFmInner.GsScan); break;
                case 43: strData = string.Format("{0}", gsBlobFmInner.GsGvLow); break;
                case 44: strData = string.Format("{0}", gsBlobFmInner.GsGvHigh); break;
                case 45: strData = string.Format("{0}", gsBlobFmInner.GsLinking); break;
                case 46: strData = string.Format("{0}", gsBlobFmInner.GsSizeLow); break;
                case 47: strData = string.Format("{0}", gsBlobFmInner.GsSizeHigh); break;
                case 48: strData = string.Format("{0}", gsBlobFmInner.GsRadiusInner); break;
                case 49: strData = string.Format("{0}", gsBlobFmInner.GsRadiusOuter); break;
                default:
                    return false;
            }

            return true;
        }

        public void LoadParam(XmlElement paramElement)
        {
            m_BlobFmMiddle.LoadParam(paramElement["FmMiddle"]);
            m_BlobFmInner.LoadParam(paramElement["FmInner"]);
            m_BlobFmOuter.LoadParam(paramElement["FmOuter"]);
            m_CircleWidth.LoadParam(paramElement["Circle"]);
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlElement fmMiddleElement = paramElement.OwnerDocument.CreateElement("FmMiddle");
            paramElement.AppendChild(fmMiddleElement);
            m_BlobFmMiddle.SaveParam(fmMiddleElement);

            XmlElement fmInnerElement = paramElement.OwnerDocument.CreateElement("FmInner");
            paramElement.AppendChild(fmInnerElement);
            m_BlobFmInner.SaveParam(fmInnerElement);

            XmlElement fmOutterElement = paramElement.OwnerDocument.CreateElement("FmOuter");
            paramElement.AppendChild(fmOutterElement);
            m_BlobFmOuter.SaveParam(fmOutterElement);

            XmlElement circleElement = paramElement.OwnerDocument.CreateElement("Circle");
            paramElement.AppendChild(circleElement);
            m_CircleWidth.SaveParam(circleElement);
        }

        public VCLensParam Clone()
        {
            var cloneParam = new VCLensParam();
            cloneParam.m_BlobFmMiddle = m_BlobFmMiddle.Clone();
            cloneParam.m_BlobFmOuter = m_BlobFmOuter.Clone();
            cloneParam.m_BlobFmInner = m_BlobFmInner.Clone();
            cloneParam.m_CircleWidth = m_CircleWidth.Clone();

            return cloneParam;
        }
    }

    public class VCLens : IpLibrary
    {
        #region < Constructor >   
        public VCLens()
        {
        }

        #endregion
        #region < Variable >        

        private List<Rectangle> m_vWidthThin = new List<Rectangle>();  // Width Thin Area
        private List<Rectangle> m_vWidthThick = new List<Rectangle>();  // Width Thick Area
        private List<Rectangle> m_vFmLowerOuter = new List<Rectangle>();    // FM Lower Outer
        private List<Rectangle> m_vFmUpperOuter = new List<Rectangle>();    // FM Upper Outer
        private List<Rectangle> m_vFmLowerMiddle = new List<Rectangle>();    // FM Upper Middle
        private List<Rectangle> m_vFmUpperMiddle = new List<Rectangle>();    // FM Upper Middle
        private List<Rectangle> m_vFmLowerInner = new List<Rectangle>();    // FM Upper Inner
        private List<Rectangle> m_vFmUpperInner = new List<Rectangle>();    // FM Upper Inner
        #endregion

        #region < Get/Set Function >
        public Rectangle gsLenArea { get; set; } = new Rectangle(0, 0, 0, 0);
        public List<Rectangle> GetFmLoweOuterr()
        {
            return m_vFmLowerOuter;
        }
        public List<Rectangle> GetFmUpperOuter()
        {
            return m_vFmUpperOuter;
        }
        public List<Rectangle> GetFmLowerMiddle()
        {
            return m_vFmLowerMiddle;
        }
        public List<Rectangle> GetFmUpperMiddle()
        {
            return m_vFmUpperMiddle;
        }
        public List<Rectangle> GetFmLowerInner()
        {
            return m_vFmLowerInner;
        }
        public List<Rectangle> GetFmUpperInner()
        {
            return m_vFmUpperInner;
        }
        #endregion

        protected override void Clear()
        {
            base.Clear();
            gsLenArea = Rectangle.FromLTRB(0, 0, 0, 0);

            m_vWidthThin.Clear();
            m_vWidthThick.Clear();
            m_vFmLowerOuter.Clear();
            m_vFmUpperOuter.Clear();
            m_vFmLowerMiddle.Clear();
            m_vFmUpperMiddle.Clear();
            m_vFmLowerInner.Clear();
            m_vFmUpperInner.Clear();
        }

        public int Inspection(AlgoImage algoImage, VCLensParam param, DebugContext dContext, HelpDraw hDraw = null)
        {
            Debug.WriteLineIf(true, " * VCLens::Inspection Start.");
            Debug.Indent();

            Clear();  // MsrOpenCv(algoImage);
            if (!IsValid(algoImage) || (param == null))
            {
                return -1;
            }

            int res = 1;    // -1 : Error, 0 : Ng, 1 : Ok

            ///////////////////////////////////////////////////////////////////////////////////////
            // 1. Align            
            ///////////////////////////////////////////////////////////////////////////////////////
            param.gsAlignArea = Rectangle.FromLTRB(100, 100, algoImage.Width - 100, algoImage.Height - 100);

            int resAlign = Align(algoImage, param, hDraw);
            if (resAlign < 1)
            {
                return resAlign;        // Failed.! Align Blob 
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // 2. Align by Circle Fitting and Check Width
            ///////////////////////////////////////////////////////////////////////////////////////            
            // param.gsCircleWidth.gsUseWidth = true;
            // param.gsCircleWidth.gsRange = 24;
            param.gsCircleWidth.GsTcRoi = gsLenArea;
            int resCircle = CheckCircle(algoImage, param.gsCircleWidth, hDraw);
            if (resCircle < 0)
            {
                return resCircle;       // Failed.! Align Circle
            }

            if ((m_vWidthThin.Count() > 0) || (m_vWidthThick.Count() > 0))
            {
                res = 0;
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            // 3. Check FM
            ///////////////////////////////////////////////////////////////////////////////////////           
            int resFmOuter = 1;
            int resFmMiddle = 1;
            int resFmInner = 1;

            for (int i = 0; i < 3; i++)
            {
                if (i == 0)         // Outer
                {
                    if (param.gsBlobFmOuter.GsUse == false)
                    {
                        continue;
                    }

                    param.gsBlobFmOuter.GsTcRoi = gsLenArea;
                    resFmOuter = CheckFm(algoImage, param.gsBlobFmOuter, ref m_vFmLowerOuter, ref m_vFmUpperOuter, hDraw);
                    if (resFmOuter < 0)
                    {
                        return resFmOuter;      // Only Error
                    }
                }
                else if (i == 1)    // Middle
                {
                    if (param.gsBlobFmMiddle.GsUse == false)
                    {
                        continue;
                    }

                    param.gsBlobFmMiddle.GsTcRoi = gsLenArea;
                    resFmMiddle = CheckFm(algoImage, param.gsBlobFmMiddle, ref m_vFmLowerMiddle, ref m_vFmUpperMiddle, hDraw);
                    if (resFmMiddle < 0)
                    {
                        return resFmMiddle;     // Only Error
                    }
                }
                else if (i == 2)    // Inner
                {
                    if (param.gsBlobFmInner.GsUse == false)
                    {
                        continue;
                    }

                    param.gsBlobFmInner.GsTcRoi = gsLenArea;
                    resFmInner = CheckFm(algoImage, param.gsBlobFmInner, ref m_vFmLowerInner, ref m_vFmUpperInner, hDraw);
                    if (resFmInner < 0)
                    {
                        return resFmInner;      // Only Error
                    }
                }
                else
                {
                    return -1;
                }
            }

            if (res == 1)
            {
                if ((resFmOuter == 0) || (resFmMiddle == 0) || (resFmInner == 0))
                {
                    res = 0;
                }
            }

            Debug.Unindent();
            Debug.WriteLineIf(true, " * VCLens::Inspection Finish.");
            return res;
        }

        private int Align(AlgoImage algoImage, VCLensParam param, HelpDraw hDraw = null)
        {
            int res = 0;
            bool useBlob = false;

            if (useBlob)    // Case. Align by Blob
            {
                // Blob Align Fixed Parameter
                ////////////////////////////////////////////////////////////
                //param.gsBlobAlign.gsScan = 2;
                //param.gsBlobAlign.gsBgAvgGv = 128;
                //param.gsBlobAlign.gsLinking = 100;
                //param.gsBlobAlign.gsGvLow = 0;
                //param.gsBlobAlign.gsGvHigh = 45;
                //param.gsBlobAlign.gsSizeLow = 15;
                //param.gsBlobAlign.gsSizeHigh = 15;
                param.gsBlobAlign.GsTcRoi = param.gsAlignArea;
                ////////////////////////////////////////////////////////////

                res = AlignbyBlob(algoImage, param, hDraw);
            }
            else            // Case. Align by Brightness
            {
                res = AlignbyMaxGv(algoImage, param, hDraw);

                //  ActMsrOpenCv actOpenCv = new ActMsrOpenCv(hDraw, gsError);

                /*
                ////////////////////////////////////////////////////////////
                MsrMatch msrMatch = new MsrMatch();

                msrMatch.gsTcRoi = gsLenArea;
                msrMatch.gsIsRoi = param.gsAlignArea;
                msrMatch.gsTempletePath = "D:\\STW\\Templete.bmp";

                bool bFromFile = (msrMatch.gsTemplete.Image == null) ? true : false;
                bool bLearn = actOpenCv.MatchLearn(!bFromFile ? null : algoImage, ref msrMatch); 

                #if DEBUG
                if (msrMatch.gsTemplete.Image != null)
                    msrMatch.gsTemplete.Save(msrMatch.gsTempletePath, null);
                #endif

                bool bOkMatchNcc  = actOpenCv.MatchNcc(algoImage, ref msrMatch);
                bool bOkMatchHist = actOpenCv.MatchHistogram(algoImage, ref msrMatch);
                ////////////////////////////////////////////////////////////       
                 * */
            }

            if (res > 0)
            {
                Rectangle outLens = gsLenArea;
                if (hDraw != null)
                {
                    var outCenter = new Point(outLens.Left + outLens.Width / 2,
                                                outLens.Top + outLens.Height / 2);
                    hDraw.AddRect(Pens.LightGray, param.gsAlignArea, false);
                    hDraw.AddRect(Pens.Yellow, outLens, true);
                    hDraw.AddCross(Pens.Yellow, outCenter, 15);
                }
            }
            else
            {
                gsError.Append("!! Infomation !! [Align] Align is failed.");
                if (hDraw != null)
                {
                    hDraw.AddRect(Pens.Red, param.gsAlignArea, false);
                }
            }

            return res;
        }

        private int AlignbyMaxGv(AlgoImage algoImage, VCLensParam param, HelpDraw hDraw = null)
        {
            int SizeX = algoImage.Width;
            int SizeY = algoImage.Height;
            int Step = algoImage.Pitch;
            byte[] pData = algoImage.GetByte();
            int LensW = (int)param.gsCircleWidth.GsRadius.X * 2;
            int LensH = (int)param.gsCircleWidth.GsRadius.X * 2;
            var outLens = Rectangle.FromLTRB(0, 0, 0, 0);

            var actOpenCv = new ActMsrOpenCv(hDraw, gsError);

            bool bOk = false;
            Rectangle isRoi = param.gsAlignArea;
            int bgThreshold = param.gsBgThreshold;

            // Threshholding
            if ((bgThreshold > 0))
            {
                var binRoi = Rectangle.FromLTRB(0, 0, algoImage.Width, algoImage.Height);
                //AlgoImage binImage = actOpenCv.BinalizeGlobal(algoImage, binRoi, bgThreshold);
                //actOpenCv.BinalizeGlobal(algoImage, binRoi, bgThreshold);
                AlgoImage binImage = actOpenCv.BinalizeAdaptive(algoImage, 25, 4);
                if (binImage == null)
                {
                    return -1;
                }

                //binImage.Save("D:\\temp.bmp",null);
                bOk = actOpenCv.GetMaxGvRect(binImage, isRoi, LensW, LensH, false, ref outLens);
            }
            else
            {
                bOk = actOpenCv.GetMaxGvRect(algoImage, isRoi, LensW, LensH, false, ref outLens);
            }

            if (!bOk && false)      // LongTime : Not-Use
            {
                int iterAxisY = (SizeY - LensH) - isRoi.Top;
                int iterAxisX = (SizeX - LensW) - isRoi.Left;
                if ((iterAxisX < 0) || (iterAxisY < 0))
                {
                    return -1;
                }

                double GvAvg = 0;
                double GvCur = 0;
                int Scan1st = 12;
                for (int j = isRoi.Top; j < isRoi.Bottom - LensH; j += Scan1st)
                {
                    for (int i = isRoi.Left; i < isRoi.Right - LensW; i += Scan1st)
                    {
                        GvCur = 0;
                        for (long n = 0; n < LensH; n += Scan1st)
                        {
                            for (long m = 0; m < LensW; m += Scan1st)
                            {
                                GvCur += pData[Step * (j + n) + (i + m)];
                            }
                        }

                        GvCur = GvCur / (LensW * LensH);
                        if (GvCur > GvAvg)
                        {
                            GvAvg = GvCur; outLens = Rectangle.FromLTRB(i, j, i + LensW, j + LensH);
                        }
                    }
                }

                GvAvg = 0;
                GvCur = 0;
                int Scan2nd = 6;
                var outCenter = new Point(outLens.Left + outLens.Width / 2,
                                             outLens.Top + outLens.Height / 2);
                for (int j = outCenter.Y - (LensH / 2) - Scan1st; j < outCenter.Y - (LensH / 2) + Scan1st; j += Scan2nd)
                {
                    for (int i = outCenter.X - (LensW / 2) - Scan1st; i < outCenter.X - (LensW / 2) + Scan1st; i += Scan2nd)
                    {
                        GvCur = 0;
                        for (long n = 0; n < LensH; n += Scan2nd)
                        {
                            for (long m = 0; m < LensW; m += Scan2nd)
                            {
                                GvCur += pData[Step * (j + n) + (i + m)];
                            }
                        }

                        GvCur = GvCur / (LensW * LensH);
                        if (GvCur > GvAvg)
                        {
                            GvAvg = GvCur; outLens = Rectangle.FromLTRB(i, j, i + LensW, j + LensH);
                        }
                    }
                }
            }

            gsLenArea = outLens;

            return 1;
        }

        private int AlignbyBlob(AlgoImage algoImage, VCLensParam param, HelpDraw hDraw = null)
        {
            var vLower = new List<Rectangle>();
            var vUpper = new List<Rectangle>();

            int res = 0;
            bool bOk = MsrBlob(algoImage, param.gsBlobAlign, vLower, vUpper, hDraw);
            if (bOk)
            {
                int CountLower = vLower.Count();
                int CountUpper = vUpper.Count();
                if ((CountLower == 0) && (CountUpper == 1))
                {
                    Rectangle OutBlob = vUpper[0];
                    int Width = OutBlob.Width;
                    int Height = OutBlob.Height;
                    int Diameter = (int)(param.gsCircleWidth.GsRadius.X * 2);
                    int CirRange = param.gsCircleWidth.GsRange;

                    if ((Math.Abs(Width - Diameter) < CirRange) && (Math.Abs(Height - Diameter) < CirRange))
                    {
                        gsLenArea = OutBlob;
                        res = 1;
                    }
                }
            }

            vLower.Clear();
            vUpper.Clear();

            return res;
        }

        private int CheckCircle(AlgoImage algoImage, MsrCircle CircleWidth, HelpDraw hDraw = null)
        {
            int res = 1;

            bool bOk = MsrCircle(algoImage, CircleWidth, m_vWidthThin, m_vWidthThick, null, null, hDraw);
            if (!bOk)
            {
                res = -1;
            }
            else
            {
                int CountWidth = m_vWidthThin.Count() + m_vWidthThick.Count();
                if (CountWidth > 0)
                {
                    res = 0;
                }
                /*
if (hDraw != null)
{
   POINT ptCenter = m_rLens.CenterPoint();
   RECT RadiusInner = new RECT(ptCenter.x - blobFm.gsRadiusInner, ptCenter.y - blobFm.gsRadiusInner,
                               ptCenter.x + blobFm.gsRadiusInner, ptCenter.y + blobFm.gsRadiusInner);
   RECT RadiusOuter = new RECT(ptCenter.x - blobFm.gsRadiusOuter, ptCenter.y - blobFm.gsRadiusOuter,
                               ptCenter.x + blobFm.gsRadiusOuter, ptCenter.y + blobFm.gsRadiusOuter);
   hDraw.AddRect(Pens.LightGreen, RadiusInner, true);
   hDraw.AddRect(Pens.LightGreen, RadiusOuter, true);
}
*/
            }

            return res;
        }

        private int CheckFm(AlgoImage algoImage, MsrBlob blobFm, ref List<Rectangle> vLower,
                                                                 ref List<Rectangle> vUpper, HelpDraw hDraw = null)
        {
            int res = 1;

            bool bOk = MsrBlob(algoImage, blobFm, vLower, vUpper, hDraw);
            if (!bOk)
            {
                res = -1;
            }
            else
            {
                int CountFm = vLower.Count() + vUpper.Count();
                if (CountFm > 0)
                {
                    res = 0;
                }

                if (hDraw != null)
                {
                    var ptCenter = new Point(gsLenArea.Left + (gsLenArea.Right - gsLenArea.Left) / 2,
                                               gsLenArea.Top + (gsLenArea.Bottom - gsLenArea.Top) / 2);
                    //      gsLenArea.CenterPoint();
                    var RadiusInner = Rectangle.FromLTRB(ptCenter.X - blobFm.GsRadiusInner, ptCenter.Y - blobFm.GsRadiusInner,
                                                               ptCenter.X + blobFm.GsRadiusInner, ptCenter.Y + blobFm.GsRadiusInner);
                    var RadiusOuter = Rectangle.FromLTRB(ptCenter.X - blobFm.GsRadiusOuter, ptCenter.Y - blobFm.GsRadiusOuter,
                                                               ptCenter.X + blobFm.GsRadiusOuter, ptCenter.Y + blobFm.GsRadiusOuter);
                    hDraw.AddRect(Pens.LightGreen, RadiusInner, true);
                    hDraw.AddRect(Pens.LightGreen, RadiusOuter, true);
                }
            }

            return res;
        }
    }
}
