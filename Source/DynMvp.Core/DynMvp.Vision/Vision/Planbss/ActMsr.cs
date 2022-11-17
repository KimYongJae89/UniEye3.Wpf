using DynMvp.Base;
using DynMvp.Vision.OpenCv;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Planbss
{
    public class ActMsr
    {
        protected double PI = 3.141592;
        public HelpDraw gsDraw { get; set; } = null;
        public StringBuilder MessageBuilder { get; set; } = null;

        #region < Constructor >   
        public ActMsr()
        {
        }
        #endregion

        #region < Support Drawing > 
        protected void AddMessage(string strMessage)
        {
            if (MessageBuilder == null)
            {
                return;
            }

            MessageBuilder.Append(strMessage);
        }
        public void ADDLine(Pen Color, Point pt1, Point pt2)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }

            gsDraw.AddLine(Color, pt1, pt2);
        }
        public void ADDLine(Pen Color, PointD pt1, PointD pt2)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }

            gsDraw.AddLine(Color, pt1, pt2);
        }
        public void ADDRect(Pen Color, Rectangle rArea, bool bCircle)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }

            gsDraw.AddRect(Color, rArea, bCircle);
        }
        public void ADDDash(Pen Color, Rectangle rArea, int nType)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }
        }
        public void ADDText(Pen Color, Point pt1, string strText, int nHeight)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }
        }
        public void ADDCross(Pen Color, Point pt1, int nLength)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }

            gsDraw.AddCross(Color, pt1, nLength);
        }
        public void ADDCross(Pen Color, PointD pt, int nLength)
        {
            if ((gsDraw == null) || (IpLibrary.gsUser == EUSER.OPERATOR))
            {
                return;
            }

            gsDraw.AddCross(Color, pt, nLength);
        }
        #endregion

        #region < Support Utility >
        public void Save(AlgoImage algoImage, string strName)
        {
            algoImage.Save(strName, null);
        }

        protected bool MakeImage(Image2D image2d, string strName)
        {
            var Roi = new Rectangle(500, 500, 700, 700);

            int Bpp = 0;
            var bmp = image2d.ToBitmap();
            PixelFormat Imagebpp = bmp.PixelFormat;
            switch (Imagebpp)
            {
                case PixelFormat.Format32bppRgb: Bpp = 32; break;
                case PixelFormat.Format24bppRgb: Bpp = 24; break;
                case PixelFormat.Format8bppIndexed: Bpp = 8; break;
                default:
                    return false;
            }

            var bmpRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(bmpRect, ImageLockMode.ReadWrite, Imagebpp);

            int Step = bmpData.Stride;
            IntPtr pSacn0 = bmpData.Scan0;


            byte[] data = new byte[bmp.Width * bmp.Height * 3];
            switch (Bpp)
            {
                case 8: data = new byte[bmp.Width * bmp.Height * 1]; break;
                case 24: data = new byte[bmp.Width * bmp.Height * 3]; break;
                default:
                    return false;
            }

            // 내부 포인터의 값을 byte 배열에 복사합니다.
            System.Runtime.InteropServices.Marshal.Copy(pSacn0, data, 0, data.Length);

            if (Bpp == 8)
            {
            }
            else if (Bpp == 24)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        byte red, green, blue;

                        red = data[x * bmp.Width + y * 3 + 0];
                        green = data[x * bmp.Width + y * 3 + 1];
                        blue = data[x * bmp.Width + y * 3 + 2];

                        byte val = (byte)(.299 * red + .587 * green + .114 * blue);

                        data[x * bmp.Width + y * 3 + 0] = val;
                        data[x * bmp.Width + y * 3 + 1] = val;
                        data[x * bmp.Width + y * 3 + 2] = val;
                    }
                }
            }

            //흑백으로 변환한 byte 배열을 다시 포인터에 복사 합니다.
            System.Runtime.InteropServices.Marshal.Copy(data, 0, pSacn0, data.Length);

            return true;
        }

        protected bool IsValidRoi(int nSizeX, int nSizeY, Rectangle Roi)
        {
            if ((Roi.Left < 0) || (Roi.Right > nSizeX))
            {
                return false;
            }

            if ((Roi.Top < 0) || (Roi.Bottom > nSizeY))
            {
                return false;
            }

            if ((Roi.Top >= Roi.Bottom) || (Roi.Left >= Roi.Right))
            {
                return false;
            }

            return true;
        }

        protected int Clamping(int nGv)
        {
            if (nGv > 255)
            {
                return 255;
            }

            if (nGv < 0)
            {
                return 0;
            }

            return nGv;
        }

        protected void swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        protected Rectangle InflatePoint(int CenterX, int CenterY, int InflateX, int InflateY)
        {
            int L = CenterX - InflateX;
            int T = CenterY - InflateY;
            int R = CenterX + InflateX;
            int B = CenterY + InflateY;

            return Rectangle.FromLTRB(L, T, R, B);
        }
        #endregion
    }

    public class ActMsrEdge : ActMsr
    {
        #region < Constructor >           
        public ActMsrEdge(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool Find(AlgoImage algoImage, MsrEdge msrEdge)
        {
            if (false == msrEdge.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrEdge::Find] Verify.");
                return false;
            }
            if (false == msrEdge.GsUse)
            {
                return true;
            }

            int GrayValue = msrEdge.GsGv;

            bool bOk = true;

            if (GrayValue > 0)
            {
                bOk = FindGvDifference(algoImage, msrEdge);
            }
            else
            {
                bOk = FindGvThreshold(algoImage, msrEdge, GrayValue * (-1));
            }

            return bOk;
        }

        private int GetAreaGv(AlgoImage algoImage, int CenterX, int CenterY, int ThickW, int ThickH)
        {
            int ImgStep = algoImage.Pitch;
            byte[] ImgByte = algoImage.GetByte();

            double SumGv = 0;
            int roiTop = CenterY - (ThickH / 2);
            int roiLeft = CenterX - (ThickW / 2);
            if ((roiTop < 0) || (roiLeft < 0))
            {
                return 0;
            }

            for (int j = roiTop; j < roiTop + ThickH; j++)
            {
                for (int i = roiLeft; i < roiLeft + ThickW; i++)
                {
                    SumGv += ImgByte[ImgStep * j + i];
                }
            }

            double AvgGv = SumGv / (ThickW * ThickH);

            return (int)(AvgGv + 0.5);
        }

        private bool FindGvDifference(AlgoImage algoImage, MsrEdge msrEdge)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //    @ Remarks : Find Edge of Profile with Fixed Slope
            //
            //        * Direct	1 :   0°, 	2 :  90°, 	3 : 180°, 	4 : 270°
            //		            5 :  45°, 	6 : 135°, 	7 : 225°, 	8 : 315°				
            ///////////////////////////////////////////////////////////////////////////////////////

            // int THICK    = 3;
            int GvThickH = msrEdge.GsThickW;
            int GvThickW = msrEdge.GsThickH;
            int DIST = msrEdge.GsDistance;  // Default : 5       
            int STEP = algoImage.Pitch;
            int DIRECT = msrEdge.GsDirect;
            int WtoB = msrEdge.GsWtoB;
            int GVDIFF = msrEdge.GsGv;
            Rectangle crRoi = msrEdge.GsEdgeRoi;
            Point ptCenter = DrawingHelper.CenterPoint(crRoi); // crRoi.CenterPoint();
            Rectangle ROIError = crRoi; ROIError.Inflate(DIST, DIST);
            if (!IsValidRoi(algoImage.Width, algoImage.Height, ROIError))
            {
                return false;
            }

            var ptEdge = new PointD(ptCenter.X, ptCenter.Y);
            msrEdge.GsEdgeOutPt = ptEdge;

            ///////////////////////////////////////////////////////////////////////////////////////
            // Fast Scan
            ///////////////////////////////////////////////////////////////////////////////////////
            int GVCur, GVNxt;
            bool bOk = false;
            byte[] ImgData = algoImage.GetByte();

            if (DIRECT == 1)    // 0°
            {
                for (int i = crRoi.Left; i < crRoi.Right; i++)
                {
                    /*
                    GVCur = ImgData[STEP * (ptCenter.Y) + (i)];
                    GVNxt = ImgData[STEP * (ptCenter.Y) + (i + DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (ptCenter.Y - 1) + (i)];
                        GVCur += ImgData[STEP * (ptCenter.Y + 1) + (i)];
                        GVNxt += ImgData[STEP * (ptCenter.Y - 1) + (i + DIST)];
                        GVNxt += ImgData[STEP * (ptCenter.Y + 1) + (i + DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, ptCenter.Y, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i + DIST, ptCenter.Y, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0)) { ptEdge.X = i + DIST / 2; bOk = true; break; }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0)) { ptEdge.X = i + DIST / 2; bOk = true; break; }
                        else if ((WtoB == 2) && ((GVCur - GVNxt) < 0)) { ptEdge.X = i + DIST / 2; bOk = true; break; }
                    }
                }
            }
            else if (DIRECT == 2)   // 90°
            {
                for (int i = crRoi.Bottom; i > crRoi.Top; i--)
                {
                    /*
                    GVCur = ImgData[STEP * (i) + ptCenter.X];
                    GVNxt = ImgData[STEP * (i - DIST) + ptCenter.X];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (i) + (ptCenter.X - 1)];
                        GVCur += ImgData[STEP * (i) + (ptCenter.X + 1)];
                        GVNxt += ImgData[STEP * (i - DIST) + (ptCenter.X - 1)];
                        GVNxt += ImgData[STEP * (i - DIST) + (ptCenter.X + 1)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, ptCenter.X, i, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, ptCenter.X, i - DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0)) { ptEdge.Y = i - DIST / 2; bOk = true; break; }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0)) { ptEdge.Y = i - DIST / 2; bOk = true; break; }
                        else if ((WtoB == 2) && ((GVCur - GVNxt) < 0)) { ptEdge.Y = i - DIST / 2; bOk = true; break; }
                    }
                }
            }
            else if (DIRECT == 3)   // 180°
            {
                for (int i = crRoi.Right; i > crRoi.Left; i--)
                {
                    /*
                    GVCur = ImgData[STEP * (ptCenter.Y) + (i)];
                    GVNxt = ImgData[STEP * (ptCenter.Y) + (i - DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (ptCenter.Y - 1) + (i)];
                        GVCur += ImgData[STEP * (ptCenter.Y + 1) + (i)];
                        GVNxt += ImgData[STEP * (ptCenter.Y - 1) + (i - DIST)];
                        GVNxt += ImgData[STEP * (ptCenter.Y + 1) + (i - DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, ptCenter.Y, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i - DIST, ptCenter.Y, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0)) { ptEdge.X = i - DIST / 2; bOk = true; break; }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0)) { ptEdge.X = i - DIST / 2; bOk = true; break; }
                        else if ((WtoB == 2) && ((GVCur - GVNxt) < 0)) { ptEdge.X = i - DIST / 2; bOk = true; break; }
                    }
                }
            }
            else if (DIRECT == 4)   // 270°
            {
                for (int i = crRoi.Top; i < crRoi.Bottom; i++)
                {
                    /*
                    GVCur = ImgData[STEP * (i) + ptCenter.X];
                    GVNxt = ImgData[STEP * (i + DIST) + ptCenter.X];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (i) + (ptCenter.X - 1)];
                        GVCur += ImgData[STEP * (i) + (ptCenter.X + 1)];
                        GVNxt += ImgData[STEP * (i + DIST) + (ptCenter.X - 1)];
                        GVNxt += ImgData[STEP * (i + DIST) + (ptCenter.X + 1)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, ptCenter.X, i, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, ptCenter.X, i + DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0)) { ptEdge.Y = i + DIST / 2; bOk = true; break; }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0)) { ptEdge.Y = i + DIST / 2; bOk = true; break; }
                        else if ((WtoB == 2) && ((GVNxt - GVCur) > 0)) { ptEdge.Y = i + DIST / 2; bOk = true; break; }
                    }
                }
            }
            else if (DIRECT == 5)   // 45°
            {
                for (int i = crRoi.Left, j = crRoi.Bottom; j > crRoi.Top; i++, j--)
                {
                    /*
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j - DIST) + (i + DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (j - 1) + (i - 1)];
                        GVCur += ImgData[STEP * (j + 1) + (i + 1)];
                        GVNxt += ImgData[STEP * ((j - 1) - DIST) + ((i - 1) + DIST)];
                        GVNxt += ImgData[STEP * ((j + 1) - DIST) + ((i + 1) + DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, j, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i + DIST, j - DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 2) && ((GVNxt - GVCur) > 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                    }
                }
            }
            else if (DIRECT == 6)   // 135°
            {
                for (int i = crRoi.Right, j = crRoi.Bottom; j > crRoi.Top; i--, j--)
                {
                    /*
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j - DIST) + (i - DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (j - 1) + (i + 1)];
                        GVCur += ImgData[STEP * (j + 1) + (i - 1)];
                        GVNxt += ImgData[STEP * ((j - 1) - DIST) + ((i + 1) - DIST)];
                        GVNxt += ImgData[STEP * ((j + 1) - DIST) + ((i - 1) - DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, j, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i - DIST, j - DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 2) && ((GVNxt - GVCur) > 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j - DIST / 2; bOk = true; break;
                        }
                    }
                }
            }
            else if (DIRECT == 7)   // 225°
            {
                for (int i = crRoi.Right, j = crRoi.Top; j < crRoi.Bottom; i--, j++)
                {
                    /*
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j + DIST) + (i - DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (j - 1) + (i - 1)];
                        GVCur += ImgData[STEP * (j + 1) + (i + 1)];
                        GVNxt += ImgData[STEP * ((j - 1) + DIST) + ((i - 1) - DIST)];
                        GVNxt += ImgData[STEP * ((j + 1) + DIST) + ((i + 1) - DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, j, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i + DIST, j - DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 2) && ((GVNxt - GVCur) > 0))
                        {
                            ptEdge.X = i - DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                    }
                }
            }
            else if (DIRECT == 8)   // 315°
            {
                for (int i = crRoi.Left, j = crRoi.Top; j < crRoi.Bottom; i++, j++)
                {
                    /*
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j + DIST) + (i + DIST)];
                    if (THICK > 1)
                    {
                        GVCur += ImgData[STEP * (j - 1) + (i + 1)];
                        GVCur += ImgData[STEP * (j + 1) + (i - 1)];
                        GVNxt += ImgData[STEP * ((j - 1) + DIST) + ((i + 1) + DIST)];
                        GVNxt += ImgData[STEP * ((j + 1) + DIST) + ((i - 1) + DIST)];
                    }
                    GVCur = GVCur / THICK;
                    GVNxt = GVNxt / THICK;
                    */

                    GVCur = GetAreaGv(algoImage, i, j, GvThickW, GvThickH);
                    GVNxt = GetAreaGv(algoImage, i + DIST, j + DIST, GvThickW, GvThickH);
                    if (Math.Abs(GVCur - GVNxt) > GVDIFF)
                    {
                        if ((WtoB == 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 1) && ((GVCur - GVNxt) > 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                        else if ((WtoB == 2) && ((GVNxt - GVCur) > 0))
                        {
                            ptEdge.X = i + DIST / 2; ptEdge.Y = j + DIST / 2; bOk = true; break;
                        }
                    }
                }
            }

            if (!bOk)         // Check (?) : !crRoi.PtInRect(ptEdge)
            {
                return false;
            }

            msrEdge.GsEdgeOutPt = ptEdge;

            bool bAppyDerivation = false;
            if (bAppyDerivation)
            {
                ApplySobel(algoImage, ptEdge, msrEdge);
            }

            return true;
        }

        private bool FindGvThreshold(AlgoImage algoImage, MsrEdge msrEdge, int threshold)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //    @ Remarks : Find Edge of Profile with Fixed Slope
            //
            //        * Direct	1 :   0°, 	2 :  90°, 	3 : 180°, 	4 : 270°
            //		            5 :  45°, 	6 : 135°, 	7 : 225°, 	8 : 315°				
            ///////////////////////////////////////////////////////////////////////////////////////

            int DIRECT = msrEdge.GsDirect;
            int WtoB = msrEdge.GsWtoB;
            int GVDIFF = msrEdge.GsGv;
            Rectangle crRoi = msrEdge.GsEdgeRoi;
            Point ptCenter = DrawingHelper.CenterPoint(crRoi);
            Rectangle ROIError = crRoi; ROIError.Inflate(1, 1);
            if (!IsValidRoi(algoImage.Width, algoImage.Height, ROIError))
            {
                return false;
            }

            var ptEdge = new PointD(ptCenter.X, ptCenter.Y);
            msrEdge.GsEdgeOutPt = ptEdge;

            int GVCur, GVNxt;
            double SubPixel = 0;
            bool IsEdge = false;
            bool IsWtoB = false;
            int STEP = algoImage.Pitch;//.Width;
            byte[] ImgData = algoImage.GetByte(); //.GetData();

            if (DIRECT == 1)    // 0°
            {
                for (int i = crRoi.Left; i < crRoi.Right; i++)
                {
                    GVCur = ImgData[STEP * (ptCenter.Y) + (i)];
                    GVNxt = ImgData[STEP * (ptCenter.Y) + (i + 1)];
                    EdgeCheck(GVCur, GVNxt, threshold, WtoB, out IsEdge, out IsWtoB);
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i + SubPixel;
                    break;
                }
            }
            else if (DIRECT == 2)   // 90°
            {
                for (int i = crRoi.Bottom; i > crRoi.Top; i--)
                {
                    GVCur = ImgData[STEP * (i) + ptCenter.X];
                    GVNxt = ImgData[STEP * (i - 1) + ptCenter.X];
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.Y = i - SubPixel;
                    break;
                }
            }
            else if (DIRECT == 3)   // 180°
            {
                for (int i = crRoi.Right; i > crRoi.Left; i--)
                {
                    GVCur = ImgData[STEP * (ptCenter.Y) + (i)];
                    GVNxt = ImgData[STEP * (ptCenter.Y) + (i - 1)];
                    EdgeCheck(GVCur, GVNxt, threshold, WtoB, out IsEdge, out IsWtoB);
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i - SubPixel;
                    break;
                }
            }
            else if (DIRECT == 4)   // 270°
            {
                for (int i = crRoi.Top; i < crRoi.Bottom; i++)
                {
                    GVCur = ImgData[STEP * (i) + ptCenter.X];
                    GVNxt = ImgData[STEP * (i + 1) + ptCenter.X];
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.Y = i + SubPixel;
                    break;
                }
            }
            else if (DIRECT == 5)   // 45°
            {
                for (int i = crRoi.Left, j = crRoi.Bottom; j > crRoi.Top; i++, j--)
                {
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j - 1) + (i + 1)];
                    EdgeCheck(GVCur, GVNxt, threshold, WtoB, out IsEdge, out IsWtoB);
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i + SubPixel;
                    ptEdge.Y = i - SubPixel;
                    break;
                }
            }
            else if (DIRECT == 6)   // 135°
            {
                for (int i = crRoi.Right, j = crRoi.Bottom; j > crRoi.Top; i--, j--)
                {
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j - 1) + (i - 1)];
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i - SubPixel;
                    ptEdge.Y = i - SubPixel;
                    break;
                }
            }
            else if (DIRECT == 7)   // 225°
            {
                for (int i = crRoi.Right, j = crRoi.Top; j < crRoi.Bottom; i--, j++)
                {
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j + 1) + (i - 1)];
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i - SubPixel;
                    ptEdge.Y = i + SubPixel;
                    break;
                }
            }
            else if (DIRECT == 8)   // 315°
            {
                for (int i = crRoi.Left, j = crRoi.Top; j < crRoi.Bottom; i++, j++)
                {
                    GVCur = ImgData[STEP * (j) + (i)];
                    GVNxt = ImgData[STEP * (j + 1) + (i + 1)];
                    if (IsEdge == false)
                    {
                        continue;
                    }

                    SubPixel = GetSubPixel(GVCur, GVNxt, threshold, IsWtoB);
                    ptEdge.X = i + SubPixel;
                    ptEdge.Y = i + SubPixel;
                    break;
                }
            }

            if (IsEdge)
            {
                msrEdge.GsEdgeOutPt = ptEdge;
            }

            return true;
        }

        private void EdgeCheck(int GvCur, int GvNxt, int Threshold, int WtoB, out bool IsEdge, out bool IsWtoB)
        {
            IsEdge = false;
            IsWtoB = false;

            if ((GvCur < Threshold && GvNxt >= Threshold))         // Black -> White
            {
                IsWtoB = false;

                if (WtoB == 1)
                {
                    IsEdge = false;
                }
                else
                {
                    IsEdge = true;
                }
            }
            else if ((GvCur >= Threshold && GvNxt < Threshold))    // White -> Black
            {
                IsWtoB = true;

                if (WtoB == 2)
                {
                    IsEdge = false;
                }
                else
                {
                    IsEdge = true;
                }
            }
        }

        private double GetSubPixel(int GvCur, int GvNxt, int Threshold, bool IsWtoB)
        {
            double SubPixel = 0;

            if (IsWtoB)
            {
                SubPixel = (GvCur - Threshold) / (double)(GvCur - GvNxt);
            }
            else
            {
                SubPixel = (Threshold - GvCur) / (double)(GvNxt - GvCur);
            }

            return SubPixel;
        }

        public bool Find(AlgoImage algoImage, MsrEdge msrEdge, PointD ptCenter, double dRadius, double dRadian, int nRange)
        {
            int GVDIFF = msrEdge.GsGv;
            int WtoB = msrEdge.GsWtoB;
            int Direct = msrEdge.GsDirect;
            bool OutToIn = (Direct == 1) ? true : false;
            int DIST = 5;

            double Start = OutToIn ? (dRadius + nRange) : (dRadius - nRange);

            int ImgSizeX = algoImage.Width;
            int ImgSizeY = algoImage.Height;
            int ImgStep = algoImage.Pitch;
            byte[] ImgData = algoImage.GetByte();

            /*
            if (false)
            {
                byte[] size = BitConverter.GetBytes(ImgData.Length);

                int   nIndex = 0;
                double GvSum = 0;
                for (int sy = 0; sy < ImgSizeY; sy++)
                {
                    for (int sx = 0; sx < ImgSizeX; sx++)
                    {
                        int gv = ImgData[(sx) + (sy) * ImgStep];
                        GvSum += gv;
                    }
                }
            }
            */

            int GVCur = 0;
            int GVNxt = 0;
            var dptSrc = new PointD(0, 0);
            var dptDst = new PointD(0, 0);
            var ptSrc = new Point(0, 0);
            var ptDst = new Point(0, 0);

            bool bOk = false;
            var hMath = new HelpMath();
            for (int i = 0; i < nRange * 2; i++)
            {
                dptSrc = hMath.GetPtWithDA(ptCenter, OutToIn ? (Start - (i)) : (Start + (i)), dRadian);
                dptDst = hMath.GetPtWithDA(ptCenter, OutToIn ? (Start - (i + DIST)) : (Start + (i + DIST)), dRadian);

                ptSrc.X = (int)(dptSrc.X + 0.5f); ptSrc.Y = (int)(dptSrc.Y + 0.5f); ///< ADDCross(VDC_LIGHTGRAY, ptSrc, 1);
                ptDst.X = (int)(dptDst.X + 0.5f); ptDst.Y = (int)(dptDst.Y + 0.5f);

                GVCur = ImgData[ImgStep * ptSrc.Y + ptSrc.X];
                GVNxt = ImgData[ImgStep * ptDst.Y + ptDst.X];
                if (Math.Abs(GVCur - GVNxt) <= GVDIFF)
                {
                    continue;
                }

                if ((WtoB == 0))
                {
                    bOk = true;
                }
                else if ((WtoB == 1) && ((GVCur - GVNxt) > 0))
                {
                    bOk = true;
                }
                else if ((WtoB == 2) && ((GVCur - GVNxt) < 0))
                {
                    bOk = true;
                }

                if (!bOk)
                {
                    continue;
                }

                msrEdge.GsEdgeOutPt = dptDst;
                break;
            }

            return bOk;
        }

        private void ApplySobel(AlgoImage algoImage, PointD ptInput, MsrEdge msrEdge)
        {
            var actFilter = new ActMsrFilter(gsDraw, MessageBuilder);

            var crRoi = new Rectangle();
            int DIRECT = msrEdge.GsDirect;

            if ((DIRECT == 1) || (DIRECT == 3))         // Horizontal Edge	
            {
                crRoi = Rectangle.FromLTRB((int)(ptInput.X - 5), (int)(ptInput.Y - 1), (int)(ptInput.X + 5), (int)(ptInput.Y + 1));
                actFilter.SobelX(algoImage, crRoi, null, msrEdge);
            }
            else if ((DIRECT == 2) || (DIRECT == 4))    // Vertical Edge
            {
                crRoi = Rectangle.FromLTRB((int)(ptInput.X - 1), (int)(ptInput.Y - 5), (int)(ptInput.X + 1), (int)(ptInput.Y + 5));
                actFilter.SobelY(algoImage, crRoi, null, msrEdge);
            }
            else if ((DIRECT == 5) || (DIRECT == 7))    // +45 drgree Edge
            {
                crRoi = Rectangle.FromLTRB((int)(ptInput.X - 5), (int)(ptInput.Y - 5), (int)(ptInput.X + 5), (int)(ptInput.Y + 5));
                actFilter.SobelXY(algoImage, crRoi, null, msrEdge);
            }
            else if ((DIRECT == 6) || (DIRECT == 8))    // -45 drgree Edge
            {
                crRoi = Rectangle.FromLTRB((int)(ptInput.X - 5), (int)(ptInput.Y - 5), (int)(ptInput.X + 5), (int)(ptInput.Y + 5));
                actFilter.SobelYX(algoImage, crRoi, null, msrEdge);
            }
        }
    }

    public class ActMsrLine : ActMsr
    {
        #region < Constructor >
        public ActMsrLine(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        private struct sLine
        {
            public double mx, my;
            public double sx, sy;
        };

        public bool Find(AlgoImage algoImage, MsrLine msrLine)
        {
            if (false == msrLine.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrEdge::Find] Verify.");
                return false;
            }
            if (false == msrLine.GsUse)
            {
                return true;
            }

            int SCAN = msrLine.GsScan;
            int DIRECT = msrLine.GsDirect;
            Rectangle crRoi = msrLine.GsIsRoi; msrLine.GsEdgeRoi = crRoi;

            int Step = 0;
            bool bXValue = true;
            bool bVertical = true;
            if ((DIRECT == 1) || (DIRECT == 3)) { bVertical = true; bXValue = true; Step = crRoi.Height / SCAN; }
            else if ((DIRECT == 2) || (DIRECT == 4)) { bVertical = false; bXValue = false; Step = crRoi.Width / SCAN; }
            else
            {
                return false;
            }

            /////////////////////////////////////////////////////////////////
            //     PDPOINT ptEdges = (PDPOINT)malloc(sizeof(DPOINT) * Step);
            //     memset(ptEdges, 0x00, sizeof(DPOINT) * Step);
            /////////////////////////////////////////////////////////////////
            // Allocation Size !!
            var vEdges = new List<PointD>();
            var mEdge = new ActMsrEdge(gsDraw, MessageBuilder);

            if (bVertical)      ///< Vertical Line
            {
                for (int i = 0; i < Step; i++)
                {
                    Rectangle ROIEdge = msrLine.GsEdgeRoi;
                    ROIEdge.Y = crRoi.Top + i * SCAN;
                    ROIEdge.Height = SCAN;
                    msrLine.GsEdgeRoi = ROIEdge;

                    bool bFind = mEdge.Find(algoImage, msrLine);
                    if (!bFind)
                    {
                        ADDRect(Pens.Red, msrLine.GsEdgeRoi, false);
                    }
                    else
                    {
                        vEdges.Add(msrLine.GsEdgeOutPt);
                        if (IpLibrary.gsUser == EUSER.DEVELOPER)
                        {
                            ADDCross(Pens.LightGreen, msrLine.GsEdgeOutPt, 2);
                            ADDRect(Pens.LightGray, msrLine.GsEdgeRoi, false);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Step; i++)
                {
                    Rectangle ROIEdge = msrLine.GsEdgeRoi;
                    ROIEdge.X = crRoi.Left + i * SCAN;
                    ROIEdge.Width = SCAN;
                    msrLine.GsEdgeRoi = ROIEdge;

                    bool bFind = mEdge.Find(algoImage, msrLine);
                    if (!bFind)
                    {
                        ADDRect(Pens.Red, msrLine.GsEdgeRoi, false);
                    }
                    else
                    {
                        vEdges.Add(msrLine.GsEdgeOutPt);
                        if (IpLibrary.gsUser == EUSER.DEVELOPER)
                        {
                            ADDCross(Pens.LightGreen, msrLine.GsEdgeOutPt, 2);
                            ADDRect(Pens.LightGray, msrLine.GsEdgeRoi, false);
                        }
                    }
                }
            }

            msrLine.SetRateOut((int)((vEdges.Count / (float)Step) * 100));

            ///////////////////////////////////////////////////////////////////////////////////////
            // Fitting
            ///////////////////////////////////////////////////////////////////////////////////////
            bool bOk = true;
            if ((vEdges.Count <= 0) || (msrLine.GetRateOut() <= msrLine.GetRateIn()))
            {
                bOk = false;
            }
            else
            {
                if (bOk)
                {
                    bOk = FilterLine(vEdges, bXValue);
                }

                if (bOk)
                {
                    var OutPt = new PointD();
                    bOk = Fitting(vEdges, bXValue, 70, out OutPt, out double OutAngle);
                    if (bOk)
                    {
                        msrLine.GsOutPt = OutPt;
                        msrLine.SetAngleOut(OutAngle);
                    }
                }
            }

            vEdges.Clear();

            return bOk;
        }

        private bool FilterLine(List<PointD> vEdges, bool bXValue)
        {
            int i = 0;
            int j = 0;
            int nAvg = 0;
            double dSum = 0;
            int size = vEdges.Count;

            if (bXValue)
            {
                for (i = 0; i < size; i++)
                {
                    dSum += vEdges[i].X;
                }

                nAvg = (int)(dSum / size);
            }
            else
            {
                for (i = 0; i < size; i++)
                {
                    dSum += vEdges[i].Y;
                }

                nAvg = (int)(dSum / size);
            }

            var ptEdge = new Point(0, 0);
            var vDistance = new List<double>();

            for (i = 0; i < size; i += 1)
            {
                ptEdge.X = (int)(vEdges[i].X + 0.5f);
                ptEdge.Y = (int)(vEdges[i].Y + 0.5f);

                // Least Square Filter		
                // pDistance[i] = GetOrthoDist(*ptCenter, *pAngle, ptEdge);		

                // Line Filter
                if (bXValue)
                {
                    vDistance.Add(Math.Abs(nAvg - ptEdge.X));
                }
                else
                {
                    vDistance.Add(Math.Abs(nAvg - ptEdge.Y));
                }
            }

            double bufDist = 0;
            var bufEdge = new PointD();
            for (i = 0; i < size - 1; i++)
            {
                for (j = i + 1; j < size; j++)
                {
                    if (vDistance[i] > vDistance[j])    // Swap 
                    {
                        bufEdge = vEdges[i];
                        vEdges[i] = vEdges[j];
                        vEdges[j] = bufEdge;

                        bufDist = vDistance[i];
                        vDistance[i] = vDistance[j];
                        vDistance[j] = bufDist;
                    }
                }
            }

            vDistance.Clear();

            return true;
        }

        private bool Fitting(List<PointD> vEdges, bool bXValue, int useRate, out PointD outCenter, out double outAngle)
        {
            outAngle = 0;

            outCenter = new PointD();
            outCenter.X = 0;
            outCenter.Y = 0;
            int size = vEdges.Count;
            if (size < 2)
            {
                return false;
            }

            var line = new sLine();
            if (false)      // Fitting [ RANSAC ]
            {
                // double cost = RANSAC(ptEdges, size, line, 5);
            }
            else            // Fitting [ Least Square ]
            {
                int sizeValid = (int)(size * (float)(useRate * 0.01));

                LeastSquare(vEdges, size, sizeValid, out line);
            }

            outCenter.X = line.sx;
            outCenter.Y = line.sy;
            outAngle = Math.Atan(line.my / line.mx) * (-1);

            ///////////////////////////////////////////////////////////////////////////////////////
            // Point ptDraw = new Point((int)line.sx, (int)line.sy);
            // ADDCross(Pens.LightBlue, ptDraw, 30); 
            ///////////////////////////////////////////////////////////////////////////////////////

            return true;
        }

        private bool LeastSquare(List<PointD> vEdges, int SizeTatal, int SizeValid, out sLine model)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //   @ Parameters
            //	        : LPIS_PT ptInfo	- < in>	Points Set
            //	        : int size			- < in> Points count
            //	        : LPPOINT ptCenter	- <out> One Point on the Line 
            //	        : double *pAngle	- <out> Angle of Line			
            //    @ Remarks 
            //	        : OpenCV - cvFitLine [ CV_DIST_L2 ]
            ///////////////////////////////////////////////////////////////////////////////////////

            model.mx = 0; // Vx
            model.my = 0; // Vy

            // center of mass(xc, yc)
            model.sx = 0;
            model.sy = 0;

            // PCA 방식으로 직선 모델의 파라메터를 예측한다. ????
            int i;
            float t;//, weights = 0;
            double dx2, dy2, dxy;
            double x = 0, y = 0, x2 = 0, y2 = 0, xy = 0, w = 0;

            // Calculating the average of x and y
            for (i = 0; i < SizeTatal; i += 1)
            {
                if (i < SizeValid)
                {
                    x += vEdges[i].X;
                    y += vEdges[i].Y;
                    x2 += vEdges[i].X * vEdges[i].X;
                    y2 += vEdges[i].Y * vEdges[i].Y;
                    xy += vEdges[i].X * vEdges[i].Y;
                }
                else
                {
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        ADDCross(Pens.Red, vEdges[i], 1);
                    }
                }
            }

            w = (float)SizeValid;

            x /= w;
            y /= w;
            x2 /= w;
            y2 /= w;
            xy /= w;

            dx2 = x2 - x * x;
            dy2 = y2 - y * y;
            dxy = xy - x * y;

            // principal axis
            t = (float)Math.Atan2(2 * dxy, dx2 - dy2) / 2;

            model.mx = Math.Cos(t); // Vx
            model.my = Math.Sin(t); // Vy

            // center of mass(xc, yc)
            model.sx = x;
            model.sy = y;

            return true;
        }
    }

    public class ActMsrCircle : ActMsr
    {
        #region < Constructor >
        public ActMsrCircle(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool Find(AlgoImage algoImage, MsrCircle msrCircle)
        {
            if (false == msrCircle.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrCircle::Find] Verify.");
                return false;
            }
            if (false == msrCircle.GsUse)
            {
                return true;
            }

            bool bSave = false;
            if (bSave)
            {
                algoImage.Save("D:\\STW\\Input.bmp", null);
            }

            Rectangle tcRoi = msrCircle.GsIsRoi;
            int Scan = msrCircle.GsScan;
            int Range = msrCircle.GsRange;
            double InRadius = msrCircle.GsRadius.X;
            var InCenter = new PointD(tcRoi.Left + (tcRoi.Right - tcRoi.Left) / 2,
                                         tcRoi.Top + (tcRoi.Bottom - tcRoi.Top) / 2);

            double RadiusOuter = InRadius + Range;
            double RadiusInner = InRadius - Range;
            int Circumference = (int)(2 * PI * InRadius);
            float StepAngle = 360 / (float)Circumference;
            int MAXPoint = Circumference / Scan; // Maximum Edge Count
            if (MAXPoint < 4)
            {
                return false;
            }

            var hMath = new HelpMath();
            var mEdge = new ActMsrEdge(gsDraw, MessageBuilder);

            int nIndex = 0;
            var vEdges = new List<PointD>();
            for (int i = 0; i < Circumference; i += Scan)
            {
                if (nIndex >= MAXPoint)
                {
                    break;
                }

                float Angle = StepAngle * i;
                double Radian = MathHelper.DegToRad(Angle);
                PointD Contact = hMath.GetPtWithDA(InCenter, InRadius, Radian);

                bool bFind = mEdge.Find(algoImage, msrCircle, InCenter, InRadius, Radian, Range);
                if (bFind)
                {
                    vEdges.Add(msrCircle.GsEdgeOutPt);
                }

                PointD RangeStd = hMath.GetPtWithDA(InCenter, RadiusOuter, Radian);
                PointD RangeEnd = hMath.GetPtWithDA(InCenter, RadiusInner, Radian);
                if (bFind)
                {
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        ADDCross(Pens.LightGreen, msrCircle.GsEdgeOutPt, 1);
                    }
                }
                else
                {
                    var Pt1 = new Point((int)(RangeStd.X + 0.5f), (int)(RangeStd.Y + 0.5f));
                    var Pt2 = new Point((int)(RangeEnd.X + 0.5f), (int)(RangeEnd.Y + 0.5f));
                    ADDLine(Pens.Red, Pt1, Pt2);
                }
            }

            double OutRate = vEdges.Count() / (double)(Circumference / Scan) * 100;
            msrCircle.SetRateOut((int)OutRate);

            Point RateCircle = msrCircle.GsRate;
            if (RateCircle.X > RateCircle.Y)
            {
                return false;
            }

            bool bOk = Fitting(vEdges, msrCircle);

            return bOk;
        }

        public bool IsBumpy(AlgoImage algoImage, MsrCircle msrCircle, List<Rectangle> vBurrs, List<Rectangle> vBrokens)
        {
            int Scan = msrCircle.GsScanBumpy;
            int Range = msrCircle.GsRange;
            PointD OutCenter = msrCircle.GsOutPt;
            double OutRadius = msrCircle.GsRadius.Y;
            SearchSide eBGSide = msrCircle.GsBGSide;
            int ITER = msrCircle.GsIteration;
            int DIST = msrCircle.GsDepth;

            double RadiusOuter = OutRadius + Range;
            double RadiusInner = OutRadius - Range;
            int Circumference = (int)(2 * PI * OutRadius);
            float StepAngle = 360 / (float)Circumference;

            var OverDepth = new PointD(0, 0);
            double OverMaxDist = 0;
            int OverIter = 0;

            var hMath = new HelpMath();
            var mEdge = new ActMsrEdge(gsDraw, MessageBuilder);

            for (int i = 0; i < Circumference; i += Scan)
            {
                float Angle = StepAngle * i;
                double Radian = MathHelper.DegToRad(Angle);
                PointD Contact = hMath.GetPtWithDA(OutCenter, OutRadius, Radian);

                bool bFind = mEdge.Find(algoImage, msrCircle, OutCenter, OutRadius, Radian, Range);
                if (IpLibrary.gsUser > EUSER.OPERATOR /*eUser == DEVELOPER*/ )
                {
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        ADDCross(Pens.Yellow, Contact, 1);
                    }

                    PointD RangeStd = hMath.GetPtWithDA(OutCenter, RadiusOuter, Radian);
                    PointD RangeEnd = hMath.GetPtWithDA(OutCenter, RadiusInner, Radian);
                    if (bFind)
                    {
                        if (IpLibrary.gsUser > EUSER.DEVELOPER)
                        {
                            ADDCross(Pens.LightGreen, msrCircle.GsEdgeOutPt, 1);
                        }
                    }
                    else
                    {
                        ADDLine(Pens.Red, RangeStd, RangeEnd);
                    }
                }

                //////////////////////////////////////////////////////////////////
                // Compare Distance with Specification
                //////////////////////////////////////////////////////////////////
                double DistCur = 0;
                PointD OutEdge = msrCircle.GsEdgeOutPt; //->GetEdgeOutPt();
                bool OverDist = true;
                if (bFind)
                {
                    DistCur = hMath.GetDistance(Contact, OutEdge);
                    if (Math.Abs(DistCur) < DIST)
                    {
                        OverDist = false;
                    }
                }

                //////////////////////////////////////////////////////////////////
                // Compare Iteration with Specification
                //////////////////////////////////////////////////////////////////
                if (OverDist)
                {
                    if (DistCur > OverMaxDist)
                    {
                        OverMaxDist = DistCur;
                        OverDepth = OutEdge;
                    }
                    OverIter++;
                }
                else
                {
                    if (OverIter >= ITER)		///< Case. Defect Find
                    {
                        float StdAngle = StepAngle * (i - (OverIter * Scan));
                        float EndAngle = StepAngle * (i - Scan);
                        Rectangle crBumpy = GetBumpyArea(StdAngle, EndAngle, OutCenter, OutRadius, OverDepth);

                        if (true /*(pBurrs != NULL) && (pBrokens != NULL)*/ )
                        {
                            bool OutBumpy = (OutRadius < hMath.GetDistance(OutCenter, OverDepth)) ? true : false;
                            bool BGInner = (eBGSide == SearchSide.Left) ? true : false;
                            if (BGInner)
                            {
                                if (true == OutBumpy)
                                {
                                    vBrokens.Add(crBumpy);
                                }
                                else
                                {
                                    vBurrs.Add(crBumpy);
                                }
                            }
                            else
                            {
                                if (true == OutBumpy)
                                {
                                    vBurrs.Add(crBumpy);
                                }
                                else
                                {
                                    vBrokens.Add(crBumpy);
                                }
                            }
                        }
                    }
                    OverIter = 0; OverMaxDist = 0;
                }
            }

            return true;
        }

        public bool IsThickness(AlgoImage algoImage, MsrCircle msrCircle, List<Rectangle> vThin, List<Rectangle> vThick)
        {
            int Scan = msrCircle.GsScanWidth;
            PointD OutCenter = msrCircle.GsOutPt;
            int specThin = msrCircle.GsWidthThin;      // <Default> 0 : Not Use
            int specThick = msrCircle.GsWidthThick;     // <Default> 6
            int SpecLength = msrCircle.GsWidthLength;    // <Default> 20
            double OutRadius = msrCircle.GsRadius.Y + msrCircle.GsOffsetRadius;

            double expectedOuterRadius = OutRadius + msrCircle.GsOuterOffset;
            double expectedInnerRadius = OutRadius + msrCircle.GsInnerOffset * (-1);
            double distOuterFar = expectedOuterRadius + msrCircle.GsOuterRange;
            double distOuterNear = expectedOuterRadius - msrCircle.GsOuterRange;
            double distInnerFar = expectedInnerRadius + msrCircle.GsInnerRange;
            double distInnerNear = expectedInnerRadius - msrCircle.GsInnerRange;

            int Circumference = (int)(2 * PI * OutRadius);
            float StepAngle = 360 / (float)Circumference;

            var hMath = new HelpMath();
            var mEdge = new ActMsrEdge(gsDraw, MessageBuilder);
            var msrEdgeOuter = new MsrEdge();
            var msrEdgeInner = new MsrEdge();

            msrEdgeOuter.GsDirect = msrCircle.GsOuterOutToIn ? 1 : 0;
            msrEdgeOuter.GsGv = msrCircle.GsOuterGv;
            msrEdgeOuter.GsWtoB = msrCircle.GsOuterWtoB;

            msrEdgeInner.GsDirect = msrCircle.GsInnerOutToIn ? 1 : 0;
            msrEdgeInner.GsGv = msrCircle.GsInnerGv;
            msrEdgeInner.GsWtoB = msrCircle.GsInnerWtoB;


            int iteration = 0;
            double sumDist = 0;
            var edgeOuter = new PointD(0, 0);
            var edgeInner = new PointD(0, 0);
            for (int i = 0; i < Circumference; i += Scan)
            {
                float Angle = StepAngle * i;
                double Radian = MathHelper.DegToRad(Angle);

                ///////////////////////////////////////////////////////////////////////////////////
                // Find Outer Edge
                ///////////////////////////////////////////////////////////////////////////////////
                // DPOINT ContactOuter = hMath.GetPtWithDA(OutCenter, expectedOuterRadius, Radian);
                // ADDCross(Pens.LightBlue, ContactOuter, 1);
                bool bOkOuter = mEdge.Find(algoImage, msrEdgeOuter, OutCenter,
                                            expectedOuterRadius, Radian, msrCircle.GsOuterRange);
                if (!bOkOuter)
                {
                    PointD edgeStdOuter = hMath.GetPtWithDA(OutCenter, distOuterFar, Radian);
                    PointD edgeEndOuter = hMath.GetPtWithDA(OutCenter, distOuterNear, Radian);
                    ADDLine(Pens.LightBlue, edgeStdOuter, edgeEndOuter);
                }
                else
                {
                    edgeOuter = msrEdgeOuter.GsEdgeOutPt;
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        ADDCross(Pens.LightBlue, edgeOuter, 1);
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////
                // Find Inner Edge
                ///////////////////////////////////////////////////////////////////////////////////
                // DPOINT ContactInner = hMath.GetPtWithDA(OutCenter, expectedInnerRadius, Radian);
                // ADDCross(Pens.Violet, ContactInner, 1);
                bool bOkInner = mEdge.Find(algoImage, msrEdgeInner, OutCenter,
                                            expectedInnerRadius, Radian, msrCircle.GsInnerRange);
                if (!bOkInner)
                {
                    PointD edgeStdInner = hMath.GetPtWithDA(OutCenter, distInnerFar, Radian);
                    PointD edgeEndInner = hMath.GetPtWithDA(OutCenter, distInnerNear, Radian);
                    ADDLine(Pens.Violet, edgeStdInner, edgeEndInner);
                }
                else
                {
                    edgeInner = msrEdgeInner.GsEdgeOutPt;
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        ADDCross(Pens.Violet, edgeInner, 1);
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////
                // Ok Process about Failed Edge Finding
                /////////////////////////////////////////////////////////////////////////////////// 
                if (!bOkOuter || !bOkInner)
                {
                    iteration = 0; sumDist = 0;  // Reset Condition
                    continue;
                }

                ///////////////////////////////////////////////////////////////////////////////////
                // Compare Distance with Specification
                ///////////////////////////////////////////////////////////////////////////////////
                double DistCur = Math.Abs(hMath.GetDistance(edgeInner, edgeOuter));
                if ((DistCur < specThin) || (specThick < DistCur))
                {
                    iteration++;
                    sumDist += DistCur;
                    ADDLine(Pens.Red, edgeInner, edgeOuter);
                }
                else
                {
                    if (iteration >= SpecLength)    // Add Defect
                    {
                        float StdDegree = StepAngle * (i - (iteration * Scan));
                        float EndDegree = StepAngle * (i - Scan);
                        Rectangle ArcCircle = GetCircleArc(OutCenter, OutRadius, StdDegree, EndDegree);

                        bool isThick = ((sumDist / iteration) > specThin) ? true : false;
                        if (!isThick)
                        {
                            vThin.Add(ArcCircle);
                        }
                        else
                        {
                            vThick.Add(ArcCircle);
                        }
                    }

                    iteration = 0; sumDist = 0; // Reset Condition
                }
            }

            return true;
        }

        private Rectangle GetCircleArc(PointD ptCenter, double dRadius, float fStdDegree, float fEndDegree)
        {
            var hMath = new HelpMath();

            double StdRadian = MathHelper.DegToRad(fStdDegree);
            PointD StdOver = hMath.GetPtWithDA(ptCenter, dRadius, StdRadian);

            double EndRadian = MathHelper.DegToRad(fEndDegree);
            PointD EndOver = hMath.GetPtWithDA(ptCenter, dRadius, EndRadian);

            var ptStd = new PointD((long)(StdOver.X + 0.5f), (long)(StdOver.Y + 0.5f));
            var ptEnd = new PointD((long)(EndOver.X + 0.5f), (long)(EndOver.Y + 0.5f));

            // Normalize Rectangle            
            int L = (int)((ptStd.X > ptEnd.X) ? ptEnd.X + 0.5f : ptStd.X + 0.5f);
            int R = (int)((ptStd.X > ptEnd.X) ? ptStd.X + 0.5f : ptEnd.X + 0.5f);
            int T = (int)((ptStd.Y > ptEnd.Y) ? ptEnd.Y + 0.5f : ptStd.Y + 0.5f);
            int B = (int)((ptStd.Y > ptEnd.Y) ? ptStd.Y + 0.5f : ptEnd.Y + 0.5f);

            return Rectangle.FromLTRB(L, T, R, B);
        }

        private Rectangle GetBumpyArea(float fStdAngle, float fEndAngle, PointD ptCenter, double dRadius, PointD ptDepth)
        {
            var hMath = new HelpMath();

            double StdRadian = MathHelper.DegToRad(fStdAngle);
            PointD StdOver = hMath.GetPtWithDA(ptCenter, dRadius, StdRadian);

            double EndRadian = MathHelper.DegToRad(fEndAngle);
            PointD EndOver = hMath.GetPtWithDA(ptCenter, dRadius, EndRadian);

            int L = (int)(StdOver.X + 0.5f);
            int T = (int)(StdOver.Y + 0.5f);
            int R = (int)(EndOver.X + 0.5f);
            int B = (int)(EndOver.Y + 0.5f);

            if (IpLibrary.gsUser == EUSER.DEVELOPER)
            {
                ///< Need AddSquare 
                ///< ADDRect(VDC_LIGHTRED, crBumpy, FALSE);
                //   ADDCross(VDC_LIGHTRED, ptDepth, 1/*(int)(OverMaxDist+0.5f)*/);
            }

            return Rectangle.FromLTRB(L, T, R, B);


            //      RECT crBumpy = new RECT((long)(StdOver.dX + 0.5f), (long)(StdOver.dY + 0.5f),
            //                              (long)(EndOver.dX + 0.5f), (long)(EndOver.dY + 0.5f));



            //    return crBumpy;
        }

        private bool Fitting(List<PointD> vEdges, MsrCircle msrCircle)
        {
            int nSize = vEdges.Count();
            if (nSize < 3)
            {
                return false;
            }

            double sx = 0.0, sy = 0.0;
            double sx2 = 0.0, sy2 = 0.0, sxy = 0.0;
            double sx3 = 0.0, sy3 = 0.0, sx2y = 0.0, sxy2 = 0.0;

            ///< Compute Summations
            for (int i = 0; i < nSize; i++)
            {
                double x = vEdges[i].X;
                double y = vEdges[i].Y;

                double xx = x * x;
                double yy = y * y;

                sx = sx + x;
                sy = sy + y;
                sx2 = sx2 + xx;
                sy2 = sy2 + yy;
                sxy = sxy + x * y;
                sx3 = sx3 + x * xx;
                sy3 = sy3 + y * yy;
                sx2y = sx2y + xx * y;
                sxy2 = sxy2 + x * yy;
            }

            ///< Compute a's, b's, c's	
            double a1 = 2.0 * (sx * sx - sx2 * nSize);
            double a2 = 2.0 * (sx * sy - sxy * nSize);
            double b1 = a2;
            double b2 = 2.0 * (sy * sy - sy2 * nSize);
            double c1 = sx2 * sx - sx3 * nSize + sx * sy2 - sxy2 * nSize;
            double c2 = sx2 * sy - sy3 * nSize + sy * sy2 - sx2y * nSize;

            double det = a1 * b2 - a2 * b1;
            double colinear = 0.0000000001; // 1.e-10 
                                            //  if( fabs(det) < 1.e-10 )	///< Colinear 한 경우임
            if (Math.Abs(det) < colinear)
            {
                ///< Colinear 한 경우임
                return false;
            }

            ///< floating value center
            double cx = (c1 * b2 - c2 * b1) / det;
            double cy = (a1 * c2 - a2 * c1) / det;

            ///< Compute radius squared
            double radsq = (sx2 - 2 * sx * cx + cx * cx * nSize + sy2 - 2 * sy * cy + cy * cy * nSize) / nSize;
            //      msrCircle.gsRadius.dY = Math.Sqrt(radsq); //.SetRadiusOut( sqrt(radsq) );  // pMsrCircle->SetRadiusOut( sqrt(radsq) );
            msrCircle.SetRadiusOut(Math.Sqrt(radsq)); //.gsRadiusOut = Math.Sqrt(radsq);

            ///< integer Value squared
            var OutPt = new PointD(cx, cy);
            msrCircle.GsOutPt = OutPt; // pMsrCircle->SetOutPt( OutPt );

            return true;
        }
    }

    public class ActMsrCorner : ActMsr
    {
        #region < Constructor >
        public ActMsrCorner(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool Find(AlgoImage algoImage, MsrCorner msrCorner)
        {
            if (false == msrCorner.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrCorner::Find] Verify.");
                return false;
            }
            if (false == msrCorner.GsUse)
            {
                return true;
            }

            bool bOkH = true, bOkV = true;
            var OutCorner = new PointD();
            var hMath = new HelpMath();
            var actLine = new ActMsrLine(gsDraw, MessageBuilder);

            var RoiLineH = new Rectangle();
            var RoiLineV = new Rectangle();
            var msrLineH = new MsrLine();
            var msrLineV = new MsrLine();

            int isRange = msrCorner.GsRange;
            int isLength = msrCorner.GsLength;
            Rectangle isRoi = msrCorner.GsIsRoi;
            Point isCenter = DrawingHelper.CenterPoint(isRoi);

            if (msrCorner.GsCardinal == CardinalPoint.NorthEast)
            {
                RoiLineH = Rectangle.FromLTRB(isCenter.X, isCenter.Y - isRange, isCenter.X + isLength, isCenter.Y + isRange);
                CopyParams(msrCorner, RoiLineH, ref msrLineH);
                msrLineH.GsDirect = (msrCorner.GsOtuToIn == true) ? 2 : 4;

                bOkH = actLine.Find(algoImage, msrLineH);     // Horizon

                RoiLineV = Rectangle.FromLTRB(isCenter.X - isRange, isCenter.Y - isLength, isCenter.X + isRange, isCenter.Y);
                CopyParams(msrCorner, RoiLineV, ref msrLineV);
                msrLineV.GsDirect = (msrCorner.GsOtuToIn == true) ? 1 : 3;

                bOkV = actLine.Find(algoImage, msrLineV);     // Vertical
            }
            else if (msrCorner.GsCardinal == CardinalPoint.NorthWest)
            {
                RoiLineH = Rectangle.FromLTRB(isCenter.X - isLength, isCenter.Y - isRange, isCenter.X, isCenter.Y + isRange);
                CopyParams(msrCorner, RoiLineH, ref msrLineH);
                msrLineH.GsDirect = (msrCorner.GsOtuToIn == true) ? 2 : 4;

                bOkH = actLine.Find(algoImage, msrLineH);     // Horizon

                RoiLineV = Rectangle.FromLTRB(isCenter.X - isRange, isCenter.Y - isLength, isCenter.X + isRange, isCenter.Y);
                CopyParams(msrCorner, RoiLineV, ref msrLineV);
                msrLineV.GsDirect = (msrCorner.GsOtuToIn == true) ? 3 : 1;

                bOkV = actLine.Find(algoImage, msrLineV);     // Vertical
            }
            else if (msrCorner.GsCardinal == CardinalPoint.SouthWest)
            {
                RoiLineH = Rectangle.FromLTRB(isCenter.X - isLength, isCenter.Y - isRange, isCenter.X, isCenter.Y + isRange);
                CopyParams(msrCorner, RoiLineH, ref msrLineH);
                msrLineH.GsDirect = (msrCorner.GsOtuToIn == true) ? 4 : 2;

                bOkH = actLine.Find(algoImage, msrLineH);     // Horizon

                RoiLineV = Rectangle.FromLTRB(isCenter.X - isRange, isCenter.Y, isCenter.X + isRange, isCenter.Y + isLength);
                CopyParams(msrCorner, RoiLineV, ref msrLineV);
                msrLineV.GsDirect = (msrCorner.GsOtuToIn == true) ? 3 : 1;

                bOkV = actLine.Find(algoImage, msrLineV);     // Vertical
            }
            else if (msrCorner.GsCardinal == CardinalPoint.SouthEast)
            {
                RoiLineH = Rectangle.FromLTRB(isCenter.X, isCenter.Y - isRange, isCenter.X + isLength, isCenter.Y + isRange);
                CopyParams(msrCorner, RoiLineH, ref msrLineH);
                msrLineH.GsDirect = (msrCorner.GsOtuToIn == true) ? 4 : 2;

                bOkH = actLine.Find(algoImage, msrLineH);     // Horizon

                RoiLineV = Rectangle.FromLTRB(isCenter.X - isRange, isCenter.Y, isCenter.X + isRange, isCenter.Y + isLength);
                CopyParams(msrCorner, RoiLineV, ref msrLineV);
                msrLineV.GsDirect = (msrCorner.GsOtuToIn == true) ? 1 : 3;

                bOkV = actLine.Find(algoImage, msrLineV);     // Vertical
            }
            else
            {
                return false;
            }

            if (bOkH)
            {
                ADDRect(Pens.LightGray, RoiLineH, false);
            }
            else
            {
                ADDRect(Pens.Red, RoiLineH, false);
            }

            if (bOkV)
            {
                ADDRect(Pens.LightGray, RoiLineV, false);
            }
            else
            {
                ADDRect(Pens.Red, RoiLineV, false);
            }

            bool bOk = false;
            if (bOkH && bOkV)
            {
                bOk = hMath.GetCrossPt(msrLineH.GsAngle.Y, msrLineH.GsOutPt,
                                       msrLineV.GsAngle.Y, msrLineV.GsOutPt, out OutCorner);

                msrCorner.GsOutPt = OutCorner;
                msrCorner.GsOutAngleHor = msrLineH.GsAngle.Y;
                msrCorner.GsOutAngleVer = msrLineV.GsAngle.Y;
            }

            return bOk;
        }

        private void CopyParams(MsrCorner msrCorner, Rectangle LineRoi, ref MsrLine msrLine)
        {
            // Msr
            msrLine.GsTcRoi = LineRoi;
            msrLine.GsIsRoi = LineRoi;
            // Edge
            msrLine.GsWtoB = msrCorner.GsWtoB;
            msrLine.GsThickW = msrCorner.GsThickW;
            msrLine.GsThickH = msrCorner.GsThickH;
            msrLine.GsDistance = msrCorner.GsDistance;
            msrLine.GsGv = msrCorner.GsGv;
            // Line
            msrLine.GsScan = msrCorner.GsScan;
            msrLine.GsRate = msrCorner.GsRate;
        }
    }

    public class ActMsrQuadrangle : ActMsr
    {
        #region < Constructor >
        public ActMsrQuadrangle(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool Find(AlgoImage algoImage, MsrQuadrangle msrQuadrangle)
        {
            if (false == msrQuadrangle.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrQuadrangle::Find] Verify.");
                return false;
            }
            if (false == msrQuadrangle.GsUse)
            {
                return true;
            }

            var actCorner = new ActMsrCorner(gsDraw, MessageBuilder);

            int isRange = msrQuadrangle.GsRange;
            int isLength = msrQuadrangle.GsLength;
            Rectangle isRoi = msrQuadrangle.GsIsRoi;

            Rectangle RoiCornerLT = InflatePoint(isRoi.Left, isRoi.Top, isLength, isLength);
            Rectangle RoiCornerRT = InflatePoint(isRoi.Right, isRoi.Top, isLength, isLength);
            Rectangle RoiCornerRB = InflatePoint(isRoi.Right, isRoi.Bottom, isLength, isLength);
            Rectangle RoiCornerLB = InflatePoint(isRoi.Left, isRoi.Bottom, isLength, isLength);

            var msrCornerLT = new MsrCorner();
            var msrCornerRT = new MsrCorner();
            var msrCornerRB = new MsrCorner();
            var msrCornerLB = new MsrCorner();

            if (msrQuadrangle.GsConvex == ConvexShape.None)
            {
                CopyParams(msrQuadrangle, RoiCornerLT, CardinalPoint.SouthEast, ref msrCornerLT);
                CopyParams(msrQuadrangle, RoiCornerRT, CardinalPoint.SouthWest, ref msrCornerRT);
                CopyParams(msrQuadrangle, RoiCornerRB, CardinalPoint.NorthWest, ref msrCornerRB);
                CopyParams(msrQuadrangle, RoiCornerLB, CardinalPoint.NorthEast, ref msrCornerLB);
            }
            else if (msrQuadrangle.GsConvex == ConvexShape.Left)
            {
                CopyParams(msrQuadrangle, RoiCornerLT, CardinalPoint.SouthEast, ref msrCornerLT);
                CopyParams(msrQuadrangle, RoiCornerRT, CardinalPoint.NorthWest, ref msrCornerRT); msrCornerRT.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerRB, CardinalPoint.SouthWest, ref msrCornerRB); msrCornerRB.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerLB, CardinalPoint.NorthEast, ref msrCornerLB);
            }
            else if (msrQuadrangle.GsConvex == ConvexShape.Top)
            {
                CopyParams(msrQuadrangle, RoiCornerLT, CardinalPoint.SouthEast, ref msrCornerLT);
                CopyParams(msrQuadrangle, RoiCornerRT, CardinalPoint.SouthWest, ref msrCornerRT);
                CopyParams(msrQuadrangle, RoiCornerRB, CardinalPoint.NorthEast, ref msrCornerRB); msrCornerRB.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerLB, CardinalPoint.NorthWest, ref msrCornerLB); msrCornerLB.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
            }
            else if (msrQuadrangle.GsConvex == ConvexShape.Right)
            {
                CopyParams(msrQuadrangle, RoiCornerLT, CardinalPoint.NorthEast, ref msrCornerLT); msrCornerLT.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerRT, CardinalPoint.SouthWest, ref msrCornerRT);
                CopyParams(msrQuadrangle, RoiCornerRB, CardinalPoint.NorthWest, ref msrCornerRB);
                CopyParams(msrQuadrangle, RoiCornerLB, CardinalPoint.SouthEast, ref msrCornerLB); msrCornerLB.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
            }
            else if (msrQuadrangle.GsConvex == ConvexShape.Bottom)
            {
                CopyParams(msrQuadrangle, RoiCornerLT, CardinalPoint.SouthWest, ref msrCornerLT); msrCornerLT.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerRT, CardinalPoint.SouthEast, ref msrCornerRT); msrCornerRT.GsOtuToIn = !msrQuadrangle.GsOtuToIn;
                CopyParams(msrQuadrangle, RoiCornerRB, CardinalPoint.NorthWest, ref msrCornerRB);
                CopyParams(msrQuadrangle, RoiCornerLB, CardinalPoint.NorthEast, ref msrCornerLB);
            }

            bool bOkLT = actCorner.Find(algoImage, msrCornerLT); msrQuadrangle.GsVertexLT = msrCornerLT.GsOutPt;
            bool bOkRT = actCorner.Find(algoImage, msrCornerRT); msrQuadrangle.GsVertexRT = msrCornerRT.GsOutPt;
            bool bOkRB = actCorner.Find(algoImage, msrCornerRB); msrQuadrangle.GsVertexRB = msrCornerRB.GsOutPt;
            bool bOkLB = actCorner.Find(algoImage, msrCornerLB); msrQuadrangle.GsVertexLB = msrCornerLB.GsOutPt;

            if (bOkLT) { ADDRect(Pens.LightGray, msrCornerLT.GsIsRoi, false); ADDCross(Pens.Blue, msrQuadrangle.GsVertexLT, 5); }
            else { ADDRect(Pens.Red, msrCornerLT.GsIsRoi, false); }
            if (bOkRT) { ADDRect(Pens.LightGray, msrCornerRT.GsIsRoi, false); ADDCross(Pens.Blue, msrQuadrangle.GsVertexRT, 5); }
            else { ADDRect(Pens.Red, msrCornerRT.GsIsRoi, false); }
            if (bOkRB) { ADDRect(Pens.LightGray, msrCornerRB.GsIsRoi, false); ADDCross(Pens.Blue, msrQuadrangle.GsVertexRB, 5); }
            else { ADDRect(Pens.Red, msrCornerRB.GsIsRoi, false); }
            if (bOkLB) { ADDRect(Pens.LightGray, msrCornerLB.GsIsRoi, false); ADDCross(Pens.Blue, msrQuadrangle.GsVertexLB, 5); }
            else { ADDRect(Pens.Red, msrCornerLB.GsIsRoi, false); }

            bool bOk = AssumeRectangle(bOkLT, bOkRT, bOkRB, bOkLB, msrQuadrangle);
            if (!bOk)
            {
                ADDRect(Pens.Red, msrQuadrangle.GsIsRoi, false);
            }
            else
            {
                if (IpLibrary.gsUser == EUSER.DEVELOPER)
                {
                    ADDLine(Pens.LightGreen, msrQuadrangle.GsVertexLT, msrQuadrangle.GsVertexRT);
                    ADDLine(Pens.LightGreen, msrQuadrangle.GsVertexRT, msrQuadrangle.GsVertexRB);
                    ADDLine(Pens.LightGreen, msrQuadrangle.GsVertexRB, msrQuadrangle.GsVertexLB);
                    ADDLine(Pens.LightGreen, msrQuadrangle.GsVertexLB, msrQuadrangle.GsVertexLT);
                }

                double OutAngle = GetQuadrangleAngle(msrCornerLT.GsOutAngleHor, msrCornerLT.GsOutAngleVer,
                                                      msrCornerRT.GsOutAngleHor, msrCornerRT.GsOutAngleVer,
                                                      msrCornerRB.GsOutAngleHor, msrCornerRB.GsOutAngleVer,
                                                      msrCornerLB.GsOutAngleHor, msrCornerLB.GsOutAngleVer);
                msrQuadrangle.GsOutAngle = OutAngle;

                PointD OutCenter = GetQuadrangleCenter(msrQuadrangle.GsVertexLT, msrQuadrangle.GsVertexRT,
                                                       msrQuadrangle.GsVertexRB, msrQuadrangle.GsVertexLB);
                msrQuadrangle.GsOutPt = OutCenter;
                ADDCross(Pens.LightGreen, OutCenter, 7);
            }

            return bOk;
        }

        private bool AssumeRectangle(bool bLT, bool bRT, bool bRB, bool bLB, MsrQuadrangle msrQuadrangle)
        {
            if (bLT && bRT && bRB && bLB)
            {
                return true;
            }

            int nSum = ((bLT == true) ? 1 : 0)
                     + ((bRT == true) ? 1 : 0)
                     + ((bRB == true) ? 1 : 0)
                     + ((bLB == true) ? 1 : 0);
            if (nSum < 2)
            {
                MessageBuilder.Append(string.Format("!! Information !! Failed Vertex Count : {0}", 4 - nSum));
                return false;
            }


            Rectangle Spec = msrQuadrangle.GsTcRoi;

            if (bLT && bRT)
            {
                msrQuadrangle.GsVertexLB = new PointD(msrQuadrangle.GsVertexLT.X, msrQuadrangle.GsVertexLT.Y + Spec.Height);
                msrQuadrangle.GsVertexRB = new PointD(msrQuadrangle.GsVertexRT.X, msrQuadrangle.GsVertexRT.Y + Spec.Height);
                return true;
            }
            else if (bLB && bRB)
            {
                msrQuadrangle.GsVertexLT = new PointD(msrQuadrangle.GsVertexLB.X, msrQuadrangle.GsVertexLB.Y - Spec.Height);
                msrQuadrangle.GsVertexRT = new PointD(msrQuadrangle.GsVertexRB.X, msrQuadrangle.GsVertexRB.Y - Spec.Height);
                return true;
            }
            else if (bLT && bLB)
            {
                msrQuadrangle.GsVertexRT = new PointD(msrQuadrangle.GsVertexLT.X + Spec.Width, msrQuadrangle.GsVertexLT.Y);
                msrQuadrangle.GsVertexRB = new PointD(msrQuadrangle.GsVertexLB.X + Spec.Width, msrQuadrangle.GsVertexLB.Y);
                return true;
            }
            else if (bRT && bRB)
            {
                msrQuadrangle.GsVertexLT = new PointD(msrQuadrangle.GsVertexRT.X - Spec.Width, msrQuadrangle.GsVertexRT.Y);
                msrQuadrangle.GsVertexLB = new PointD(msrQuadrangle.GsVertexRB.X - Spec.Width, msrQuadrangle.GsVertexRB.Y);
                return true;
            }

            return false;
        }

        private void CopyParams(MsrQuadrangle msrQuadrangle, Rectangle CornerRoi, CardinalPoint eCardianl, ref MsrCorner msrCorner)
        {
            // Msr
            msrCorner.GsTcRoi = CornerRoi;
            msrCorner.GsIsRoi = CornerRoi;

            // Edge
            msrCorner.GsWtoB = msrQuadrangle.GsWtoB;
            msrCorner.GsThickW = msrQuadrangle.GsThickW;
            msrCorner.GsThickH = msrQuadrangle.GsThickH;
            msrCorner.GsDistance = msrQuadrangle.GsDistance;
            msrCorner.GsGv = msrQuadrangle.GsGv;

            // Line and Corner
            msrCorner.GsScan = msrQuadrangle.GsScan;
            msrCorner.GsRate = msrQuadrangle.GsRate;
            msrCorner.GsCardinal = eCardianl;
            msrCorner.GsOtuToIn = msrQuadrangle.GsOtuToIn;
            msrCorner.GsRange = msrQuadrangle.GsRange;
            msrCorner.GsLength = msrQuadrangle.GsLength;
        }

        private PointD GetQuadrangleCenter(PointD ptLT, PointD ptRT, PointD ptRB, PointD ptLB)
        {
            var HorT = new PointD((ptLT.X + ptRT.X) / 2, (ptLT.Y + ptRT.Y) / 2);
            var HorB = new PointD((ptLB.X + ptRB.X) / 2, (ptLB.Y + ptRB.Y) / 2);
            var VerL = new PointD((ptLT.X + ptLB.X) / 2, (ptLT.Y + ptLB.Y) / 2);
            var VerR = new PointD((ptRT.X + ptRB.X) / 2, (ptRT.Y + ptRB.Y) / 2);

            var HorCenter = new PointD((HorT.X + HorB.X) / 2, (HorT.Y + HorB.Y) / 2);
            var VerCenter = new PointD((VerL.X + VerR.X) / 2, (VerL.Y + VerR.Y) / 2);

            var OutCenter = new PointD((HorCenter.X + VerCenter.X) / 2, (HorCenter.Y + VerCenter.Y) / 2);

            return OutCenter;
        }

        private double GetQuadrangleAngle(double LTHor, double LTVer, double RTHor, double RTVer,
                                          double RBHor, double RBVer, double LBHor, double LBVer)
        {
            var hMath = new HelpMath();

            var vAngleHor = new List<double>();
            var vAngleVer = new List<double>();

            double OutAngleLTHor = MathHelper.RadToDeg(LTHor); vAngleHor.Add(OutAngleLTHor);
            double OutAngleLTVer = MathHelper.RadToDeg(LTVer); vAngleVer.Add(GetAngleVertical(OutAngleLTVer));

            double OutAngleRTHor = MathHelper.RadToDeg(RTHor); vAngleHor.Add(OutAngleRTHor);
            double OutAngleRTVer = MathHelper.RadToDeg(RTVer); vAngleVer.Add(GetAngleVertical(OutAngleRTVer));

            double OutAngleRBHor = MathHelper.RadToDeg(RBHor); vAngleHor.Add(OutAngleRBHor);
            double OutAngleRBVer = MathHelper.RadToDeg(RBVer); vAngleVer.Add(GetAngleVertical(OutAngleRBVer));

            double OutAngleLBHor = MathHelper.RadToDeg(LBHor); vAngleHor.Add(OutAngleLBHor);
            double OutAngleLBVer = MathHelper.RadToDeg(LBVer); vAngleVer.Add(GetAngleVertical(OutAngleLBVer));

            vAngleHor.Sort();
            vAngleVer.Sort();

            double AvgAngleHor = (vAngleHor[1] + vAngleHor[2]) / 2;
            double AvgAngleVer = (vAngleVer[1] + vAngleVer[2]) / 2;
            double AvgAngleOut = (AvgAngleHor + AvgAngleVer) / 2;

            return AvgAngleOut;
        }

        private double GetAngleVertical(double dAngle)
        {
            double OutAngle = dAngle;
            if (dAngle < 0)
            {
                OutAngle = (90 + dAngle) * (-1);
            }
            else if (45 < dAngle)
            {
                OutAngle = 90 - dAngle;
            }

            return OutAngle;
        }
    }

    public class ActMsrBlob : ActMsr
    {
        #region < Constructor >
        public ActMsrBlob(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        private byte SEG_MINCELL = 2;
        private byte SEG_DIVIDER = 4;
        private byte SEG_FIRST = 0xC0;
        private byte SEG_LAST = 0x03;
        private byte SEG_EINE = 0xAA;
        private byte SEG_ZWEI = 0x55;
        private byte SEG_LOWER = 0;
        private byte SEG_MIDDLE = 0;
        private byte SEG_HIGHER = 1;

        private unsafe struct BPTR
        {
            public byte* ptr;
            public int cx;
            public int cy;
            public byte bit;
        };

        private struct BRECT
        {
            public Rectangle rect;
            public bool use;
        };

        private List<BRECT> m_upper = new List<BRECT>();
        private List<BRECT> m_lower = new List<BRECT>();

        public bool Find(AlgoImage algoImage, MsrBlob msrBlob, List<Rectangle> vLower, List<Rectangle> vUpper)
        {
            if (false == msrBlob.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrBlob::Find] Verify.");
                return false;
            }
            if (false == msrBlob.GsUse)
            {
                return true;
            }

            int Scan = msrBlob.GsScan;
            Rectangle crBlob = msrBlob.GsIsRoi;
            int GvLow = msrBlob.GsGvLow;
            int GvHigh = msrBlob.GsGvHigh;
            int Linking = msrBlob.GsLinking;
            int SizeLow = msrBlob.GsSizeLow;
            int SizeHigh = msrBlob.GsSizeHigh;
            int blobW = (SizeLow < SizeHigh) ? SizeLow : SizeHigh;
            int blobH = (SizeLow < SizeHigh) ? SizeLow : SizeHigh;
            bool bCircle = (msrBlob.GsRadiusOuter > 0) ? true : false;

            if (GvLow < 0)
            {
                GvLow = 0;
            }

            if (GvHigh > 255)
            {
                GvHigh = 255;
            }

            int nResult = -1;
            if (!bCircle)
            {
                nResult = Blob(algoImage.GetByte(), algoImage.Width, algoImage.Height, algoImage.Pitch,
                               (byte)GvLow, (byte)GvHigh, false, Linking, crBlob,
                               blobW, blobH, false, 40, Scan);
            }
            else
            {
                nResult = Blob(algoImage.GetByte(), algoImage.Width, algoImage.Height, algoImage.Pitch,
                               (byte)GvLow, (byte)GvHigh, false, Linking, crBlob,
                               blobW, blobH, false, 40, Scan,
                               msrBlob.GsRadiusOuter, msrBlob.GsRadiusInner);
            }

            if (nResult == -1) { Clear(); return false; }

            var ptOffset = new Point(0, 0);
            if (vLower != null)
            {
                List<Rectangle> Lowers = Select(SizeLow, SizeLow, out int dwLow, false, SEG_LOWER);
                for (int i = 0; i < dwLow; i++)
                {
                    int L = ptOffset.X + Lowers[i].Left;
                    int T = ptOffset.Y + Lowers[i].Top;
                    int R = ptOffset.X + Lowers[i].Right;
                    int B = ptOffset.Y + Lowers[i].Bottom;

                    vLower.Add(Rectangle.FromLTRB(L, T, R, B));
                }
                Lowers.Clear();
            }
            if (vUpper != null)
            {
                List<Rectangle> Highers = Select(SizeHigh, SizeHigh, out int dwHigh, false, SEG_HIGHER);
                for (int i = 0; i < dwHigh; i++)
                {
                    int L = ptOffset.X + Highers[i].Left;
                    int T = ptOffset.Y + Highers[i].Top;
                    int R = ptOffset.X + Highers[i].Right;
                    int B = ptOffset.Y + Highers[i].Bottom;

                    vUpper.Add(Rectangle.FromLTRB(L, T, R, B));
                }
                Highers.Clear();// Delete(pHigher);
            }

            Clear();

            return true;
        }

        public List<Rectangle> Select(int width, int height, out int count, bool orOperation, int type)
        {
            var pRect = new List<Rectangle>();
            var brect = new List<Rectangle>();

            Blob_select(brect, ((type == SEG_LOWER) || (type == SEG_MIDDLE)) ? m_lower : m_upper,
                width, height, orOperation);

            if (brect.Count() == 0)
            {
                count = 0;
                return pRect;
            }

            //  if(count ) *count = (unsigned int)brect.size();
            count = brect.Count();

            /*
	        LPRECT pRect = new RECT[brect.size()];
	        list<RECT>::iterator i = brect.begin();
	        for(DWORD k = 0; i != brect.end(); k++, i++)
	        {
		        pRect[k] = *i;
	        }
            */

            for (int i = 0; i < brect.Count(); i++)
            {
                pRect.Add(brect[i]);
            }

            return pRect;
        }

        public void Delete(List<Rectangle> blocks)
        {
            blocks.Clear();
        }

        public void Clear()
        {
            m_lower.Clear();
            m_upper.Clear();
        }

        private Rectangle UnionRect(Rectangle rt1, Rectangle rt2)
        {
            int L = Math.Min(rt1.Left, rt2.Left);
            int T = Math.Min(rt1.Top, rt2.Top);
            int R = Math.Max(rt1.Right, rt2.Right);
            int B = Math.Max(rt1.Bottom, rt2.Bottom);

            return Rectangle.FromLTRB(L, T, R, B);
        }

        private bool IsRectCross(Rectangle rt1, Rectangle rt2)
        {
            if (rt1.Left <= rt2.Left && rt1.Right >= rt2.Left &&
                rt1.Top <= rt2.Top && rt1.Bottom >= rt2.Top)
            {
                return true;
            }

            if (rt1.Left <= rt2.Right && rt1.Right >= rt2.Right &&
                rt1.Top <= rt2.Bottom && rt1.Bottom >= rt2.Bottom)
            {
                return true;
            }

            if (rt1.Left <= rt2.Left && rt1.Right >= rt2.Left &&
                rt1.Top <= rt2.Bottom && rt1.Bottom >= rt2.Bottom)
            {
                return true;
            }

            if (rt1.Left <= rt2.Right && rt1.Right >= rt2.Right &&
                rt1.Top <= rt2.Top && rt1.Bottom >= rt2.Top)
            {
                return true;
            }

            return false;
        }

        private int RectDist(Rectangle r1, Rectangle r2)
        {
            if ((r2.Top >= r1.Top && r2.Top <= r1.Bottom) ||
                (r2.Bottom >= r1.Top && r2.Bottom <= r1.Bottom) ||
                 (r2.Top <= r1.Top && r2.Bottom >= r1.Bottom))
            {
                return Math.Min(Math.Abs(r2.Right - r1.Left), Math.Abs(r1.Right - r2.Left));
            }
            else if ((r2.Left >= r1.Left && r2.Left <= r1.Right) ||
                (r2.Right >= r1.Left && r2.Right <= r1.Right) ||
                (r2.Left <= r1.Left && r2.Right >= r1.Right))
            {
                return Math.Min(Math.Abs(r2.Bottom - r1.Top), Math.Abs(r1.Bottom - r2.Top));
            }
            else
            {
                int d1 = Math.Min(Math.Abs(r2.Right - r1.Left), Math.Abs(r1.Right - r2.Left));
                int d2 = Math.Min(Math.Abs(r2.Bottom - r1.Top), Math.Abs(r1.Bottom - r2.Top));

                return (int)Math.Sqrt(d1 * d1 + d2 * d2);
            }
        }

        private void Blob_select(List<Rectangle> blocks, List<BRECT> brect, int width, int height, bool orOperation)
        {
            bool w, h;
            for (int i = 0; i < brect.Count(); i++)
            {
                if (brect[i].use)
                {
                    w = Math.Abs(brect[i].rect.Right - brect[i].rect.Left) >= width;
                    h = Math.Abs(brect[i].rect.Bottom - brect[i].rect.Top) >= height;
                    if (orOperation ? w || h : w && h)
                    {
                        blocks.Add(brect[i].rect);
                    }
                }
            }
        }

        private void Blob_link(List<BRECT> brect, int dist)
        {
            if (dist == 0)
            {
                return;
            }

            BRECT r;
            for (int i = 0; i < brect.Count(); i++)
            {
                if (!brect[i].use)
                {
                    continue;
                }

                for (int j = i; j < brect.Count(); j++)
                {
                    if (!brect[j].use || i == j)
                    {
                        continue;
                    }

                    if (IsRectCross(brect[j].rect, brect[i].rect) || RectDist(brect[j].rect, brect[i].rect) <= dist)
                    {
                        r.rect = UnionRect(brect[j].rect, brect[i].rect);

                        r.use = true;
                        brect.Add(r);

                        BRECT iBuffer = brect[i]; iBuffer.use = false; brect[i] = iBuffer;
                        BRECT jBuffer = brect[j]; jBuffer.use = false; brect[j] = jBuffer;

                        break;
                    }
                }
            }

            /*
            Queue<string> numbers = new Queue<string>();

            numbers.Dequeue(
             * */


        }

        private unsafe void Blob_loop(byte* ptr, byte bit, int cx, int cy, int width, int height, int wln, byte sel, ref Rectangle rect)
        {
            var pstack = new List<BPTR>();

            unsafe
            {
                BPTR bptr, bp;
                bptr.ptr = ptr;
                bptr.bit = bit;
                bptr.cx = cx;
                bptr.cy = cy;

                //     rect.left = width;
                //     rect.top = height;
                //     rect.right = 0;
                //     rect.bottom = 0;
                rect = Rectangle.FromLTRB(width, height, 0, 0);

                pstack.Add(bptr);
                while (pstack.Count() != 0)
                {
                    int last = pstack.Count() - 1;

                    bp = pstack[last];
                    pstack.RemoveAt(last);

                    bool bOk = (((*bp.ptr & bp.bit) & sel) == 0) ? false : true;
                    if (!bOk)
                    {
                        continue;
                    }

                    *bp.ptr &= (byte)~bp.bit;

                    int L = rect.Left;
                    int T = rect.Top;
                    int R = rect.Right;
                    int B = rect.Bottom;
                    if (rect.Left > bp.cx)
                    {
                        L = bp.cx;
                    }

                    if (rect.Top > bp.cy)
                    {
                        T = bp.cy;
                    }

                    if (rect.Right < bp.cx)
                    {
                        R = bp.cx;
                    }

                    if (rect.Bottom < bp.cy)
                    {
                        B = bp.cy;
                    }

                    rect = Rectangle.FromLTRB(L, T, R, B);

                    if (bp.cx + 1 < width)
                    {
                        bptr.bit = (byte)(bp.bit >> SEG_MINCELL);
                        bool bZero = (bptr.bit == 0) ? false : true;
                        if (!bZero) //bptr.bit
                        {
                            bptr.ptr = bp.ptr + 1;
                            bptr.bit = SEG_FIRST;
                        }
                        else
                        {
                            bptr.ptr = bp.ptr;
                        }

                        bptr.cx = bp.cx + 1;
                        bptr.cy = bp.cy;
                        bool bCondition = (((*bptr.ptr & bptr.bit) & sel) == 0) ? false : true;
                        if (bCondition)
                        {
                            pstack.Add(bptr);
                        }
                    }

                    if (bp.cx - 1 >= 0)
                    {
                        bptr.bit = (byte)(bp.bit << SEG_MINCELL);
                        bool bZero = (bptr.bit == 0) ? false : true;
                        if (!bZero)
                        {
                            bptr.ptr = bp.ptr - 1;
                            bptr.bit = SEG_LAST;
                        }
                        else
                        {
                            bptr.ptr = bp.ptr;
                        }

                        bptr.cx = bp.cx - 1;
                        bptr.cy = bp.cy;
                        bool bCondition = (((*bptr.ptr & bptr.bit) & sel) == 0) ? false : true;
                        if (bCondition)
                        {
                            pstack.Add(bptr);
                        }
                    }

                    if (bp.cy + 1 < height)
                    {
                        bptr.ptr = bp.ptr + wln;
                        bptr.bit = bp.bit;
                        bptr.cx = bp.cx;
                        bptr.cy = bp.cy + 1;
                        bool bCondition = (((*bptr.ptr & bptr.bit) & sel) == 0) ? false : true;
                        if (bCondition)
                        {
                            pstack.Add(bptr);
                        }
                    }

                    if (bp.cy - 1 >= 0)
                    {
                        bptr.ptr = bp.ptr - wln;
                        bptr.bit = bp.bit;
                        bptr.cx = bp.cx;
                        bptr.cy = bp.cy - 1;
                        bool bCondition = (((*bptr.ptr & bptr.bit) & sel) == 0) ? false : true;
                        if (bCondition)
                        {
                            pstack.Add(bptr);
                        }
                    }
                }
            }
        }

        private void Blob_block(byte[] buf, int width, int height, int wln, int cellwidth, int cellheight, bool orOperation)
        {
            byte bit, sel;
            List<BRECT> plist;
            var rect = new BRECT();

            unsafe
            {
                byte* uptr;
                byte* rptr;
                fixed (byte* pbuffer = buf)
                {
                    rptr = pbuffer;
                }

                for (int i = 0, k; i < height; i++)
                {
                    uptr = rptr;
                    bit = SEG_FIRST;
                    for (k = 0; k < width; k++)
                    {
                        bool bOk = ((*uptr & bit) == 0) ? false : true;
                        if (bOk)
                        {
                            bool bCondition = (((*uptr & bit) & SEG_EINE) == 0) ? false : true;
                            if (bCondition)
                            {
                                sel = SEG_EINE;
                                plist = m_lower;
                            }
                            else
                            {
                                sel = SEG_ZWEI;
                                plist = m_upper;
                            }

                            Blob_loop(uptr, bit, k, i, width, height, wln, sel, ref rect.rect);

                            rect.use = true;
                            rect.rect.Width++;
                            rect.rect.Height++;

                            if (orOperation ?
                                ((rect.rect.Right - rect.rect.Left) > cellwidth ||
                                (rect.rect.Bottom - rect.rect.Top) > cellheight) :
                                ((rect.rect.Right - rect.rect.Left) > cellwidth &&
                                (rect.rect.Bottom - rect.rect.Top) > cellheight))
                            {
                                plist.Add(rect);  //plist->push_back(rect);
                            }
                        }
                        bit >>= SEG_MINCELL;

                        bool bZoroBit = (bit == 0) ? false : true;
                        if (!bZoroBit/*!bit*/)
                        {
                            uptr++;
                            bit = SEG_FIRST;
                        }
                    }
                    rptr += wln;
                }
            }
        }

        private void ClearInvalid(List<BRECT> brect)
        {
            int nCount = brect.Count();
            for (int i = 0; i < brect.Count(); i++)
            {
                if (brect[i].use == false)
                {
                    brect.RemoveAt(i);
                }
            }
        }

        private int Blob(byte[] buf, int width, int height, int pitch, byte thresmin, byte thresmax,
                         bool isiner, int linkdist, Rectangle rect, int cellwidth, int cellheight,
                         bool orOperation, int validRatio, int scan)
        {
            if (scan == 0)
            {
                return 0;
            }

            if (rect.Left < 0)
            {
                return -1; // rect.Left = 0;
            }

            if (rect.Left > width - 1)
            {
                return -1; // rect.Left = width - 1;
            }

            if (rect.Right < 0)
            {
                return -1; // rect.Right = 0;
            }

            if (rect.Right > width - 1)
            {
                return -1; // rect.Right = width - 1;
            }

            if (rect.Top < 0)
            {
                return -1; // rect.Top = 0;
            }

            if (rect.Top > height - 1)
            {
                return -1; // rect.Top = height - 1;
            }

            if (rect.Bottom < 0)
            {
                return -1; // rect.Bottom = 0;
            }

            if (rect.Bottom > height - 1)
            {
                return -1; // rect.Bottom = height - 1;
            }

            if (thresmin > thresmax)
            {
                swap<byte>(ref thresmin, ref thresmax);
            }

            unsafe
            {
                byte* aptr;
                fixed (byte* pbuffer = buf)
                {
                    aptr = pbuffer + pitch * rect.Top + rect.Right;
                }

                int w = (Math.Abs(rect.Right - rect.Left) + 1) / scan;
                int h = (Math.Abs(rect.Bottom - rect.Top) + 1) / scan;
                if ((w == 0) || (h == 0))
                {
                    return 0;
                }

                int wln = w / SEG_DIVIDER + ((w % SEG_DIVIDER == 0) ? 0 : 1);
                int psz = wln * h;
                byte[] pbuffer2 = new byte[psz];
                for (int i = 0; i < psz; i++)
                {
                    pbuffer2[i] = 0;
                }

                byte bit;
                byte* rptr, uptr;
                byte* ptr, tptr;
                long count = 0;
                fixed (byte* pbuf = pbuffer2)
                {
                    rptr = pbuf;
                }

                ptr = aptr;

                // Point ptErr = new Point(0, 0);
                for (int i = 0, k; i < h; i++)
                {
                    uptr = rptr;
                    tptr = ptr;
                    bit = SEG_FIRST;
                    for (k = 0; k < w; k++)
                    {
                        if (isiner)
                        {
                            if (*tptr >= thresmin && *tptr <= thresmax)
                            {
                                *uptr |= (byte)(bit & SEG_EINE);
                                count++;
                            }
                        }
                        else
                        {
                            if (*tptr < thresmin)
                            {
                                *uptr |= (byte)(bit & SEG_EINE);
                                count++;
                                ///////////////////////////////////////////////////////////////////
                                // ptErr.X = (int)(k + rect.left); ptErr.Y = (int)(i + rect.top);
                                // ADDCross(Pens.LightGreen, ptErr, 1);
                                ///////////////////////////////////////////////////////////////////
                            }
                            else if (*tptr > thresmax)
                            {
                                *uptr |= (byte)(bit & SEG_ZWEI);
                                count++;
                                ///////////////////////////////////////////////////////////////////
                                // ptErr.X = (int)(k + rect.left); ptErr.Y = (int)(i + rect.top);
                                // ADDCross(Pens.Red, ptErr, 1);
                                ///////////////////////////////////////////////////////////////////
                            }
                        }

                        tptr += scan;
                        bit >>= SEG_MINCELL;

                        bool bbit = (bit == 0) ? false : true;
                        if (!bbit)
                        {
                            uptr++;
                            bit = SEG_FIRST;
                        }
                    }
                    rptr += wln;
                    ptr += pitch * scan;
                }

                int errRatio = (int)(((double)count / (w * h)) * 100);
                if ((100 - errRatio) < validRatio)
                {
                    // delete pbuf;
                    return -1;
                }

                Blob_block(pbuffer2/*pbuf*/, w, h, wln, cellwidth / scan, cellheight / scan, orOperation);
                // delete pbuf;

                Blob_link(m_lower, linkdist / scan);
                ClearInvalid(m_lower);
                Blob_link(m_lower, linkdist / scan);

                for (int nIndex = 0; nIndex < m_lower.Count(); nIndex++)
                {
                    if (m_lower[nIndex].use)
                    {
                        BRECT bRect = m_lower[nIndex];

                        int L = rect.Left + bRect.rect.Left * scan;
                        int T = rect.Top + bRect.rect.Top * scan;
                        int R = rect.Left + bRect.rect.Right * scan;
                        int B = rect.Top + bRect.rect.Bottom * scan;

                        bRect.rect = Rectangle.FromLTRB(L, T, R, B);

                        m_lower[nIndex] = bRect;
                    }
                }


                if (!isiner)
                {
                    Blob_link(m_upper, linkdist);
                    ClearInvalid(m_upper);
                    Blob_link(m_upper, linkdist);

                    for (int nIndex = 0; nIndex < m_upper.Count(); nIndex++)
                    {
                        if (m_upper[nIndex].use)
                        {
                            BRECT bRect = m_upper[nIndex];

                            int L = rect.Left + bRect.rect.Left * scan;
                            int T = rect.Top + bRect.rect.Top * scan;
                            int R = rect.Left + bRect.rect.Right * scan;
                            int B = rect.Top + bRect.rect.Bottom * scan;

                            bRect.rect = Rectangle.FromLTRB(L, T, R, B);

                            m_upper[nIndex] = bRect;
                        }
                    }
                }
            }

            // System.GC.Collect(); 

            return 1;
        }

        private int Blob(byte[] buf, int width, int height, int pitch, byte thresmin, byte thresmax,
                         bool isiner, int linkdist, Rectangle rect, int cellwidth, int cellheight,
                         bool orOperation, int validRatio, int scan, int OuterRadius, int InnerRadius)
        {
            if (scan == 0)
            {
                return 0;
            }

            if (rect.Left < 0)
            {
                return -1; // rect.Left = 0;
            }

            if (rect.Left > width - 1)
            {
                return -1; // rect.Left = width - 1;
            }

            if (rect.Right < 0)
            {
                return -1; // rect.Right = 0;
            }

            if (rect.Right > width - 1)
            {
                return -1; // rect.Right = width - 1;
            }

            if (rect.Top < 0)
            {
                return -1; // rect.Top = 0;
            }

            if (rect.Top > height - 1)
            {
                return -1; // rect.Top = height - 1;
            }

            if (rect.Bottom < 0)
            {
                return -1; // rect.Bottom = 0;
            }

            if (rect.Bottom > height - 1)
            {
                return -1; // rect.Bottom = height - 1;
            }

            if (thresmin > thresmax)
            {
                swap<byte>(ref thresmin, ref thresmax);
            }

            var hMath = new HelpMath();
            Point ptCenter = DrawingHelper.CenterPoint(rect);
            ptCenter.X = ptCenter.X - rect.Left;
            ptCenter.Y = ptCenter.Y - rect.Top;
            if ((ptCenter.X < 0) || (ptCenter.Y < 0))
            {
                return -1;
            }

            unsafe
            {
                byte* aptr;
                fixed (byte* pbuffer = buf)
                {
                    aptr = pbuffer + pitch * rect.Top + rect.Left;
                }

                int w = (Math.Abs(rect.Right - rect.Left) + 1) / scan;
                int h = (Math.Abs(rect.Bottom - rect.Top) + 1) / scan;
                if ((w == 0) || (h == 0))
                {
                    return 0;
                }

                int wln = w / SEG_DIVIDER + ((w % SEG_DIVIDER == 0) ? 0 : 1);
                int psz = wln * h;
                byte[] pbuffer2 = new byte[psz];
                for (int i = 0; i < psz; i++)
                {
                    pbuffer2[i] = 0;
                }

                byte bit;
                byte* rptr, uptr;
                byte* ptr, tptr;
                long count = 0;
                fixed (byte* pbuf = pbuffer2)
                {
                    rptr = pbuf;
                }

                ptr = aptr;

                for (int i = 0, k; i < h; i++)
                {
                    uptr = rptr;
                    tptr = ptr;
                    bit = SEG_FIRST;
                    for (k = 0; k < w; k++)
                    {
                        /*
                        ///////////////////////////////////////////////////////////////////////////////
                        double aaa = hMath.GetDistance(new Point(k * scan, i * scan), ptCenter);
                            if ((InnerRadius < aaa) && (aaa < OuterRadius))
                                ADDCross(Pens.LightGreen, new Point((k * scan + rect.Left), 
                                                                    (i * scan + rect.Top) ), 1);
                        ///////////////////////////////////////////////////////////////////////////////
                        continue;
                         * */
                        if (isiner)
                        {
                            if (*tptr >= thresmin && *tptr <= thresmax)
                            {
                                double distance = hMath.GetDistance(new Point(k * scan, i * scan), ptCenter);
                                if ((InnerRadius < distance) && (distance < OuterRadius))
                                {
                                    *uptr |= (byte)(bit & SEG_EINE);
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            if (*tptr < thresmin)
                            {
                                double distance = hMath.GetDistance(new Point(k * scan, i * scan), ptCenter);
                                if ((InnerRadius < distance) && (distance < OuterRadius))
                                {
                                    *uptr |= (byte)(bit & SEG_EINE);
                                    count++;
                                    // ADDCross(Pens.LightGreen,new Point((k * scan + rect.Left), (i * scan + rect.Top)) , 1);
                                }
                            }
                            else if (*tptr > thresmax)
                            {
                                double distance = hMath.GetDistance(new Point(k * scan, i * scan), ptCenter);
                                if ((InnerRadius < distance) && (distance < OuterRadius))
                                {
                                    *uptr |= (byte)(bit & SEG_ZWEI);
                                    count++;
                                    // ADDCross(Pens.LightGreen,new Point((k * scan + rect.Left), (i * scan + rect.Top)) , 1);
                                }
                            }
                        }

                        tptr += scan;
                        bit >>= SEG_MINCELL;

                        bool bbit = (bit == 0) ? false : true;
                        if (!bbit)
                        {
                            uptr++;
                            bit = SEG_FIRST;
                        }
                    }
                    rptr += wln;
                    ptr += pitch * scan;
                }

                int errRatio = (int)(((double)count / (w * h)) * 100);
                if ((100 - errRatio) < validRatio)
                {
                    // delete pbuf;
                    return -1;
                }

                Blob_block(pbuffer2/*pbuf*/, w, h, wln, cellwidth / scan, cellheight / scan, orOperation);
                // delete pbuf;

                Blob_link(m_lower, linkdist / scan);
                ClearInvalid(m_lower);
                Blob_link(m_lower, linkdist / scan);

                for (int nIndex = 0; nIndex < m_lower.Count(); nIndex++)
                {
                    if (m_lower[nIndex].use)
                    {
                        BRECT bRect = m_lower[nIndex];

                        int L = rect.Left + bRect.rect.Left * scan;
                        int T = rect.Top + bRect.rect.Top * scan;
                        int R = rect.Left + bRect.rect.Right * scan;
                        int B = rect.Top + bRect.rect.Bottom * scan;

                        bRect.rect = Rectangle.FromLTRB(L, T, R, B);

                        m_lower[nIndex] = bRect;
                    }
                }


                if (!isiner)
                {
                    Blob_link(m_upper, linkdist);
                    ClearInvalid(m_upper);
                    Blob_link(m_upper, linkdist);

                    for (int nIndex = 0; nIndex < m_upper.Count(); nIndex++)
                    {
                        if (m_upper[nIndex].use)
                        {
                            BRECT bRect = m_upper[nIndex];

                            int L = rect.Left + bRect.rect.Left * scan;
                            int T = rect.Top + bRect.rect.Top * scan;
                            int R = rect.Left + bRect.rect.Right * scan;
                            int B = rect.Top + bRect.rect.Bottom * scan;

                            bRect.rect = Rectangle.FromLTRB(L, T, R, B);

                            m_upper[nIndex] = bRect;
                        }
                    }
                }
            }

            // System.GC.Collect(); 

            return 1;
        }
    }

    public class ActMsrFilter : ActMsr
    {
        #region < Constructor >
        public ActMsrFilter(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool SobelX(AlgoImage ipSrc, Rectangle SrcRoi, AlgoImage ipDst, MsrEdge msrEdge = null)
        {
            return false;
        }
        public bool SobelY(AlgoImage ipSrc, Rectangle SrcRoi, AlgoImage ipDst, MsrEdge msrEdge = null)
        {
            return false;
        }
        public bool SobelXY(AlgoImage ipSrc, Rectangle SrcRoi, AlgoImage ipDst, MsrEdge msrEdge = null)
        {
            return false;
        }
        public bool SobelYX(AlgoImage ipSrc, Rectangle SrcRoi, AlgoImage ipDst, MsrEdge msrEdge = null)
        {
            return false;
        }
    }

    public class ActMsrMatch : ActMsr
    {
        #region < Constructor >
        public ActMsrMatch(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool Find(AlgoImage algoImage, ref MsrMatch msrMatch)
        {
            if (false == msrMatch.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrBlob::Find] Verify.");
                return false;
            }
            if (false == msrMatch.GsUse)
            {
                return true;
            }

            return false;
        }
    }

    public class ActMsrOpenCv : ActMsr
    {
        #region < Constructor >
        public ActMsrOpenCv(HelpDraw hDraw, StringBuilder sBuilder)
        {
            gsDraw = hDraw; MessageBuilder = sBuilder;
        }
        #endregion

        public bool NgImage(AlgoImage algoImage, bool isGray = true)
        {
            if (algoImage.LibraryType != ImagingLibrary.OpenCv)
            {
                return true;
            }

            if (isGray)
            {
                if (algoImage.ImageType != ImageType.Grey)
                {
                    return true;
                }
            }
            else
            {
                if (algoImage.ImageType != ImageType.Color)
                {
                    return true;
                }
            }

            return false;
        }

        public AlgoImage ImageLoad(string filename)
        {
            var cvImage = new OpenCvGreyImage();

            cvImage.Image = new Image<Gray, byte>(filename);
            if (cvImage.Image == null)
            {
                Debug.Assert(false, "ActMsrOpenCv::ImageLoad");
                return null;
            }

            return cvImage;
        }

        public AlgoImage CvtRgbToGray(AlgoImage algoImage)
        {
            if (NgImage(algoImage, false))
            {
                return null;
            }

            var srcImage = (OpenCvColorImage)algoImage;
            Image<Bgr, byte> rgbImage = srcImage.Image;

            var grayImage = new OpenCvGreyImage();
            grayImage.Image = rgbImage.Convert<Gray, byte>();

            // algoImage.Save("D:\\cvRgbToGray_src.bmp", null);
            // grayImage.Save("D:\\cvRgbToGray_dst.bmp", null);

            return grayImage;
        }

        public AlgoImage BinalizeGlobal(AlgoImage algoImage, Rectangle Roi, int Threshold, bool bReverse = false)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            // Example Caller
            // ActMsrOpenCv    actOpenCv = new ActMsrOpenCv();
            // RECT            binRoi    = new RECT(0, 0, algoImage.Width / 2, algoImage.Height / 2);
            // OpenCvGreyImage outImage  = (OpenCvGreyImage)actOpenCv.Binalize(algoImage, binRoi, 125);
            // outImage.Save("D:\\cvBinalize.bmp", null);
            ///////////////////////////////////////////////////////////////////////////////////////
            if (NgImage(algoImage) || (Threshold <= 0))
            {
                return null;
            }

            var srcImage = (OpenCvGreyImage)algoImage;

            var grayMax = new Gray(255);
            var grayThreshold = new Gray(Threshold);

            var outImage = new OpenCvGreyImage();

            if ((algoImage.Width == Roi.Width) || (algoImage.Height == Roi.Height))
            {
                ///////////////////////////////////////////////////////////////////////////////////
                // double rest = CvInvoke.cvThreshold(IntPtr src, IntPtr dst, 
                //                        double threshold, double maxValue, THRESH thresholdType);
                ///////////////////////////////////////////////////////////////////////////////////
                outImage.Image = srcImage.Image.ThresholdBinary(grayThreshold, grayMax);
            }
            else
            {
                Rectangle rect = Roi;
                var roiImage = (OpenCvGreyImage)srcImage.Clip(rect);
                outImage.Image = roiImage.Image.ThresholdBinary(grayThreshold, grayMax);
            }

            bool bSave = false;
            if (bSave && (outImage.Image != null))
            {
                outImage.Save("D:\\cvBinalizeGlobal.bmp", null);
                // outImage.Image.Save("D:\\cvBinalize.bmp");            
            }

            if (bReverse)
            {
                var NotImage = new OpenCvGreyImage();
                NotImage.Image = outImage.Image.Not();
                return NotImage;
            }

            return outImage;
        }

        public AlgoImage BinalizeLocal(AlgoImage algoImage, Rectangle Roi, int Threshold)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            // Example Caller
            // ActMsrOpenCv    actOpenCv = new ActMsrOpenCv();
            // RECT            binRoi    = new RECT(0, 0, algoImage.Width / 2, algoImage.Height / 2);
            // OpenCvGreyImage outImage  = (OpenCvGreyImage)actOpenCv.Binalize(algoImage, binRoi, 125);
            // outImage.Save("D:\\cvBinalize.bmp", null);
            ///////////////////////////////////////////////////////////////////////////////////////
            if (NgImage(algoImage) || (Threshold <= 0))
            {
                return null;
            }

            var srcImage = (OpenCvGreyImage)algoImage;

            var grayMax = new Gray(255);
            var grayThreshold = new Gray(Threshold);

            var outImage = new OpenCvGreyImage();

            ThresholdType eThreshold = ThresholdType.BinaryInv;
            AdaptiveThresholdType eAdaptive = AdaptiveThresholdType.GaussianC;

            if ((algoImage.Width == Roi.Width) || (algoImage.Height == Roi.Height))
            {
                outImage.Image = srcImage.Image.ThresholdAdaptive(new Gray(255), eAdaptive, eThreshold, 11, new Gray(5));

                ////////////////////////////////////////////////////////////////////////////////////
                //     cvAdaptiveThreshold(pIplSrc, pIplDst, 255,
                //             CV_ADAPTIVE_THRESH_MEAN_C, // (WeightType==0) ? CV_ADAPTIVE_THRESH_MEAN_C : CV_ADAPTIVE_THRESH_GAUSSIAN_C, 
                //             0, WeightType, //9, THRESH_BINARY or THRESH_BINARY_INV
                //             5);	
                ////////////////////////////////////////////////////////////////////////////////////
            }
            else
            {
                Rectangle rect = Roi;//.ToRectangle();
                var roiImage = (OpenCvGreyImage)srcImage.Clip(rect);
                outImage.Image = roiImage.Image.ThresholdAdaptive(new Gray(255), eAdaptive, eThreshold, 11, new Gray(5));
            }

            bool bSave = false;
            if (bSave && (outImage.Image != null))
            {
                outImage.Save("D:\\cvBinalizeLocal.bmp", null);
                // outImage.Image.Save("D:\\cvBinalize.bmp");            
            }

            return outImage;
        }

        public AlgoImage BinalizeAdaptive(AlgoImage algoImage, int BlockSize, int ConstantParam)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            // Example Caller
            // ActMsrOpenCv    actOpenCv = new ActMsrOpenCv();
            // RECT            binRoi    = new RECT(0, 0, algoImage.Width / 2, algoImage.Height / 2);
            // OpenCvGreyImage outImage  = (OpenCvGreyImage)actOpenCv.Binalize(algoImage, binRoi, 125);
            // outImage.Save("D:\\cvBinalize.bmp", null);
            ///////////////////////////////////////////////////////////////////////////////////////
            if (NgImage(algoImage) || (BlockSize <= 0))
            {
                return null;
            }

            var srcImage = (OpenCvGreyImage)algoImage;

            var outImage = new OpenCvGreyImage();

            var grayMax = new Gray(255);
            var grayParam = new Gray(ConstantParam);

            // Bigin DEBUG...
            //for (int sz = 11; sz <= 31; sz += 2)
            //{
            //    for (int c =0; c<=7; c++)
            //    {
            //        outImage.Image = srcImage.Image.ThresholdAdaptive(
            //    grayMax,
            //    Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C,
            //    Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV,
            //    sz, new Gray((double)c));

            //        bool bSave = true;
            //        if (bSave && (outImage.Image != null))
            //        {
            //            outImage.Save("D:\\Binalize\\cvBinalizeAdaptive_B"+sz.ToString()+"_C"+c.ToString()+".bmp", null);
            //            // outImage.Image.Save("D:\\cvBinalize.bmp");            
            //        }
            //    }
            //}
            // End DEBUG...

            outImage.Image = srcImage.Image.ThresholdAdaptive(grayMax, AdaptiveThresholdType.GaussianC,
                ThresholdType.BinaryInv, BlockSize, grayParam);

            return outImage;
        }


        public AlgoImage Resize(AlgoImage algoImage, int Width, int Height)
        {
            if (NgImage(algoImage))
            {
                return null;
            }

            var srcImage = (OpenCvGreyImage)algoImage;
            var dstImage = new OpenCvGreyImage();

            dstImage.Image = srcImage.Image.Resize(Width, Height, Inter.Linear);
            return dstImage;
        }

        public bool Integral(AlgoImage algoImage)
        {
            if (NgImage(algoImage))
            {
                return false;
            }

            var srcImage = (OpenCvGreyImage)algoImage;
            Image<Gray, double> sumImage = srcImage.Image.Integral();

            bool bSave = false;
            if (bSave && (sumImage != null))
            {
                sumImage.Save("D:\\cvIntegral.bmp");
            }

            return true;
        }

        public bool GetMaxGvRect(AlgoImage algoImage, Rectangle Roi,
                                    int ObjWidth, int ObjHeight, bool useImageClone, ref Rectangle ObjOut)
        {
            ///////////////////////////////////////////////////////////////////////////////////////
            //	* the Sum of D region value 
            //      : I(4) - I(2) - I(3) + I(1)
            //			---------------------
            //			|   A	|   B	|	
            //			|-------|1------|2	
            //			|   C	|   D   |	
            //			|-------|3------|4	
            //			|				|
            ///////////////////////////////////////////////////////////////////////////////////////
            if (NgImage(algoImage))
            {
                return false;
            }

            var srcImage = (OpenCvGreyImage)algoImage;

            int Scan = 1;
            bool SaveImage = false;
            double sumMax = 0;
            double sumCur = 0;

            var I1 = new Point();
            var I2 = new Point();
            var I3 = new Point();
            var I4 = new Point();
            var Cur = new Rectangle();
            var resObject = new Rectangle(0, 0, 0, 0);

            ///////////////////////////////////////////////////////////////////////////////////////
            // !! User Selection !!
            //      : about Using between Clone and Source Image
            //      : Example > (useImageClone == false) is better about 100 ms 
            //                                                     about 2500x2000 Image.
            ///////////////////////////////////////////////////////////////////////////////////////
            if (useImageClone)
            {
                Rectangle rect = Roi;//.ToRectangle();
                var roiImage = (OpenCvGreyImage)srcImage.Clip(rect);

                Image<Gray, double> sumImage = roiImage.Image.Integral();

                int iterAxisY = ((sumImage.Height - 1) - ObjHeight);
                int iterAxisX = ((sumImage.Width - 1) - ObjWidth);
                if ((iterAxisX <= 0) || (iterAxisY <= 0))
                {
                    return false;
                }

                for (int j = 1; j < (sumImage.Height - 1) - ObjHeight; j += Scan)
                {
                    for (int i = 1; i < (sumImage.Width - 1) - ObjWidth; i += Scan)
                    {
                        // +1 by Integral padding
                        Cur = Rectangle.FromLTRB(i + 1, j + 1, i + ObjWidth + 1, j + ObjHeight + 1);

                        I1.X = Cur.Left; I1.Y = Cur.Top; I2.X = Cur.Right; I2.Y = Cur.Top;
                        I3.X = Cur.Left; I3.Y = Cur.Bottom; I4.X = Cur.Right; I4.Y = Cur.Bottom;

                        sumCur = sumImage.Data[I4.Y, I4.X, 0] - sumImage.Data[I2.Y, I2.X, 0]
                               - sumImage.Data[I3.Y, I3.X, 0] + sumImage.Data[I1.Y, I1.X, 0];

                        sumCur = sumCur / (ObjWidth * ObjHeight);
                        if (sumCur > sumMax)
                        {
                            sumMax = sumCur;
                            resObject = Rectangle.FromLTRB(Roi.Left + i, Roi.Top + j,
                                                            Roi.Left + i + ObjWidth, Roi.Top + j + ObjHeight);
                        }
                    }
                }

                if (SaveImage)
                {
                    sumImage.Save("D:\\cvIntegral.bmp");
                }
            }
            else
            {
                Image<Gray, double> sumImage = srcImage.Image.Integral();

                int iterAxisY = (Roi.Bottom - ObjHeight) - Roi.Top;
                int iterAxisX = (Roi.Right - ObjWidth) - Roi.Left;
                if ((iterAxisX <= 0) || (iterAxisY <= 0))
                {
                    return false;
                }

                for (int j = Roi.Top; j < Roi.Bottom - ObjHeight; j += Scan)
                {
                    for (int i = Roi.Left; i < Roi.Right - ObjWidth; i += Scan)
                    {
                        // +1 by Integral padding
                        Cur = Rectangle.FromLTRB(i + 1, j + 1, i + ObjWidth + 1, j + ObjHeight + 1);

                        I1.X = Cur.Left; I1.Y = Cur.Top; I2.X = Cur.Right; I2.Y = Cur.Top;
                        I3.X = Cur.Left; I3.Y = Cur.Bottom; I4.X = Cur.Right; I4.Y = Cur.Bottom;

                        sumCur = sumImage.Data[I4.Y, I4.X, 0] - sumImage.Data[I2.Y, I2.X, 0]
                               - sumImage.Data[I3.Y, I3.X, 0] + sumImage.Data[I1.Y, I1.X, 0];

                        sumCur = sumCur / (ObjWidth * ObjHeight);
                        if (sumCur > sumMax)
                        {
                            sumMax = sumCur;
                            resObject = Rectangle.FromLTRB(i, j, i + ObjWidth, j + ObjHeight);
                        }
                    }
                }

                if (SaveImage)
                {
                    sumImage.Save("D:\\cvIntegral.bmp");
                }
            }

            ObjOut = resObject;

            return true;
        }

        public bool Blob(AlgoImage algoImage, MsrBlob msrBlob, List<Rectangle> vLower, List<Rectangle> vUpper)
        {
            if (false == msrBlob.Verify(algoImage.Width, algoImage.Height))
            {
                AddMessage("!! Error !! [ActMsrOpenCv::Blob] Verify.");
                return false;
            }
            if (false == msrBlob.GsUse)
            {
                return true;
            }

            int Scan = msrBlob.GsScan;
            Rectangle crBlob = msrBlob.GsIsRoi;
            int GvLow = msrBlob.GsGvLow;
            int GvHigh = msrBlob.GsGvHigh;
            int Linking = msrBlob.GsLinking;
            int SizeLow = msrBlob.GsSizeLow;
            int SizeHigh = msrBlob.GsSizeHigh;
            int blobW = (SizeLow < SizeHigh) ? SizeLow : SizeHigh;
            int blobH = (SizeLow < SizeHigh) ? SizeLow : SizeHigh;
            bool bCircle = (msrBlob.GsRadiusOuter > 0) ? true : false;
            if ((GvLow < 0) || (GvHigh > 255))
            {
                return false;
            }

            if (GvLow > 0)      // Blob Lower
            {
                AlgoImage binImage = BinalizeGlobal(algoImage, crBlob, GvLow, true);
                if (binImage == null)
                {
                    return false;
                }

                int SkipArea = SizeLow * SizeLow;
                bool bOk = Contour(binImage, crBlob, SkipArea, vLower);
                if (!bOk)
                {
                    return false;
                }
                // ContourBiggest(binImage, crBlob);
            }

            if (GvHigh < 255)   // Blob Upper
            {
                AlgoImage binImage = BinalizeGlobal(algoImage, crBlob, GvHigh, false);
                if (binImage == null)
                {
                    return false;
                }

                int SkipArea = SizeHigh * SizeHigh;
                bool bOk = Contour(binImage, crBlob, SkipArea, vUpper);
                if (!bOk)
                {
                    return false;
                }
                // ContourBiggest(binImage, crBlob);
            }

            return true;
        }

        public bool GetCircle(PointF[] vEdge)
        {
            var vecEdgePt = new VectorOfPointF(vEdge);

            if (IpLibrary.gsUser == EUSER.DEVELOPER)    // Bounding Rectangle of Points
            {
                Rectangle boundR = CvInvoke.BoundingRectangle(vecEdgePt);
                ADDRect(Pens.LightBlue, boundR, false);
            }

            bool bCircle = true;
            if (bCircle)
            {
                CircleF circle = CvInvoke.MinEnclosingCircle(vecEdgePt);
                PointF Center = circle.Center;
                float Radius = circle.Radius;

                var rectOne = new Rectangle((int)(Center.X - Radius + 0.5),
                                                  (int)(Center.Y - Radius + 0.5),
                                                  (int)(Radius * 2), (int)(Radius * 2));
                ADDRect(Pens.LightBlue, rectOne, true);
            }

            bool bEllipse = true;
            if (bEllipse)
            {
                RotatedRect rotatedRect = CvInvoke.FitEllipse(vecEdgePt);

                PointF[] Vertics = rotatedRect.GetVertices();

                double Degree = rotatedRect.Angle;
                PointF Center = rotatedRect.Center;
                SizeF Length = rotatedRect.Size;

                var Vt0 = Point.Round(Vertics[0]);
                var Vt1 = Point.Round(Vertics[1]);
                var Vt2 = Point.Round(Vertics[2]);
                var Vt3 = Point.Round(Vertics[3]);

                ADDLine(Pens.LightBlue, Vt0, Vt1);
                ADDLine(Pens.LightBlue, Vt1, Vt2);
                ADDLine(Pens.LightBlue, Vt2, Vt3);
                ADDLine(Pens.LightBlue, Vt3, Vt0);
            }

            return true;
        }

        public bool GetOrientation(AlgoImage algoImage, Point[] vEdge)
        {
            // Constructu a buffer used by the pca analysis

            /*
            // http://www.emgu.com/forum/viewtopic.php?t=3066
            int sX = algoImage.Width;
            int sY = algoImage.Height;
            int i, j;
            Image<Gray, float> ImageFlo = algoImage.Convert<Gray, float>();
            Matrix<float> Window = new Matrix<float>(WindowSize, WindowSize);
            Matrix<float> Avg = new Matrix<float>(1, WindowSize);
            Matrix<float> EigVals = new Matrix<float>(1, WindowSize);
            Matrix<float> EigVects = new Matrix<float>(WindowSize, WindowSize);
            Matrix<float> PCAFeatures = new Matrix<float>(WindowSize, WindowSize);
            Image<Gray, float> TempIm = ImageFlo.CopyBlank();



            for (i = 0; i < (sX - WindowSize); i++)
            {
                for (j = 0; j < (sY - WindowSize); j++)
                {
                    CvInvoke.cvSetImageROI(ImageFlo, new Rectangle(new Point(i, j), new Size(WindowSize, WindowSize)));
                    CvInvoke.cvSetImageROI(TempIm, new Rectangle(new Point(i, j), new Size(WindowSize, WindowSize)));
                    CvInvoke.cvConvert(ImageFlo, Window);
                    CvInvoke.cvCalcPCA(Window, Avg, EigVals, EigVects, Emgu.CV.CvEnum.PCA_TYPE.CV_PCA_DATA_AS_ROW);
                    try
                    {
                        CvInvoke.cvProjectPCA(Window, Avg, EigVects, PCAFeatures);
                    }
                    catch (Exception e)
                    {
                        throw (e);
                    }
                    CvInvoke.cvConvert(PCAFeatures, TempIm);

                    CvInvoke.cvResetImageROI(ImageFlo);
                    CvInvoke.cvResetImageROI(TempIm);


                }
            }

            pictureBox1.Image = TempIm.Bitmap;
            return (TempIm);
            */
            return false;
        }

        public bool Contour(AlgoImage algoImage, Rectangle Roi, double skipArea, List<Rectangle> vRects)
        {
            if (NgImage(algoImage))
            {
                return false;
            }

            var srcImage = (OpenCvGreyImage)algoImage;
            Image<Gray, byte> srcByte = srcImage.Image;

            int nIndex = 0;
            var OffsetLT = new Point(Roi.Left, Roi.Top);

            var hierachy = new Mat();
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(srcImage.Image, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone);

            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);

                if (area > skipArea)
                {
                    double Perimeter = CvInvoke.ArcLength(contours[i], true);
                    Rectangle boundRect = CvInvoke.BoundingRectangle(contours[i]);
                    boundRect.Offset(OffsetLT);
                    vRects.Add(boundRect);
                    if (IpLibrary.gsUser == EUSER.DEVELOPER)
                    {
                        boundRect.Inflate(2, 2);
                        ADDRect(Pens.LightGray, boundRect, false);
                    }

                    ///////////////////////////////////////////////////////////////////////////////
                    // Data Convert Point to PointF
                    ///////////////////////////////////////////////////////////////////////////////                        
                    var vEdge = new PointF[contours[i].Size];
                    foreach (Point p in contours[i].ToArray())
                    {
                        Point ptContour = p; ptContour.Offset(OffsetLT);

                        vEdge[nIndex].X = ptContour.X;
                        vEdge[nIndex].Y = ptContour.Y;
                        nIndex++;
                    }

                    ///////////////////////////////////////////////////////////////////////////////
                    // Option
                    ///////////////////////////////////////////////////////////////////////////////
                    bool bCircle = false;
                    if (bCircle)
                    {
                        GetCircle(vEdge);
                    }
                }
            }

            return true;
        }

        public bool ContourBiggest(AlgoImage algoImage, Rectangle Roi)
        {
            if (NgImage(algoImage))
            {
                return false;
            }

            var srcImage = (OpenCvGreyImage)algoImage;

            Image<Gray, byte> srcByte = srcImage.Image;

            var OffsetLT = new Point(Roi.Left, Roi.Top);

            var hierachy = new Mat();
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(srcImage.Image, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone);

            double maxArea = 0;
            VectorOfPoint biggestContour = null;
            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);

                if (area > maxArea)
                {
                    maxArea = area;
                    biggestContour = contours[i];
                }
            }

            if (biggestContour != null)
            {
                var ptContour = new Point();
                for (int i = 0; i < biggestContour.Size; i++)
                {
                    ptContour.X = biggestContour[i].X + Roi.Left;
                    ptContour.Y = biggestContour[i].Y + Roi.Top;
                    ADDCross(Pens.LightGreen, ptContour, 1);
                }

                bool bFindConvex = false;
                if (bFindConvex)
                {
                    double perimeter = CvInvoke.ArcLength(biggestContour, true);
                    var currentContour = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(biggestContour, currentContour, perimeter * 0.0025, true);

                    biggestContour = currentContour;

                    var hull = new VectorOfPoint();
                    CvInvoke.ConvexHull(currentContour, hull, true);

                    RotatedRect box = CvInvoke.MinAreaRect(biggestContour);
                    PointF[] points = box.GetVertices();

                    var ps = new Point[points.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        ps[i] = new Point((int)points[i].X, (int)points[i].Y);
                    }

                    var filteredHull = new List<Point>();
                    for (int i = 0; i < hull.Size; i++)
                    {
                        if (Math.Sqrt(Math.Pow(hull[i].X - hull[i + 1].X, 2) + Math.Pow(hull[i].Y - hull[i + 1].Y, 2)) > box.Size.Width / 10)
                        {
                            filteredHull.Add(hull[i]);
                        }
                    }

                    var defects = new Mat();
                    CvInvoke.ConvexityDefects(biggestContour, new VectorOfPoint(filteredHull.ToArray()), defects);

                    if (defects.IsEmpty == false)
                    {
                        var m = new Matrix<int>(defects.Rows, defects.Cols, defects.NumberOfChannels);
                        defects.CopyTo(m);

                        for (int i = 0; i < m.Rows; i++)
                        {
                            int startIdx = m.Data[i, 0];
                            int endIdx = m.Data[i, 1];
                            int depthIdx = m.Data[i, 2];

                            Point startPoint = biggestContour[startIdx];
                            Point endPoint = biggestContour[endIdx];
                            Point depthPoint = biggestContour[depthIdx];

                            startPoint.Offset(OffsetLT);
                            endPoint.Offset(OffsetLT);
                            depthPoint.Offset(OffsetLT);

                            ADDLine(Pens.Red, startPoint, endPoint);
                            ADDLine(Pens.Red, startPoint, depthPoint);
                            ADDLine(Pens.Red, endPoint, depthPoint);
                        }
                    }
                }
            }

            return false;
        }

        public bool MatchLearn(AlgoImage algoImage, ref MsrMatch msrMatch)
        {
            if (algoImage == null)  // Load from File
            {
                msrMatch.GsTemplete = (OpenCvGreyImage)ImageLoad(msrMatch.GsTempletePath);
                if (msrMatch.GsTemplete == null)
                {
                    return false;
                }

                msrMatch.GsTcRoi = new Rectangle(0, 0, msrMatch.GsTemplete.Width,
                                                       msrMatch.GsTemplete.Height);
            }
            else                    // Teach from AlgoImage
            {
                AlgoImage tplImage = algoImage.Clip(msrMatch.GsTcRoi);
                if (tplImage == null)
                {
                    return false;
                }

                msrMatch.GsTemplete = (OpenCvGreyImage)tplImage;
            }

            return true;
        }

        public bool MatchNcc(AlgoImage algoImage, ref MsrMatch msrMatch)
        {
            if (NgImage(algoImage))
            {
                return false;
            }
            //if ((algoImage.Width <= Templete.Width) || (algoImage.Height <= Templete.Height))
            //    return false;

            var srcImage = (OpenCvGreyImage)algoImage;
            OpenCvGreyImage tplImage = msrMatch.GsTemplete;

            Image<Gray, byte> srcByte = srcImage.Image;   // ImageFind.Save("D:\\STW\\MatchSrc.bmp");
            Image<Gray, byte> tplByte = tplImage.Image;  // ImageModel.Save("D:\\STW\\MatchDst.bmp");
            Image<Gray, float> ImageNccs = srcByte.MatchTemplate(tplByte, TemplateMatchingType.CcoeffNormed);
            ImageNccs.MinMax(out double[] min, out double[] max, out Point[] pointMin, out Point[] pointMax);
            int maxCount = max.Count();
            if (maxCount != 1)
            {
                return false;
            }

            for (int i = 0; i < maxCount; i++)
            {
                double OutScore = max[i] * 100;

                if (OutScore > msrMatch.GsScore.X)
                {
                    int tcW = msrMatch.GsTcRoi.Width / 2;
                    int tcH = msrMatch.GsTcRoi.Height / 2;
                    msrMatch.GsOutPt = new PointD(pointMax[i].X + tcW, pointMax[i].Y + tcH);
                    msrMatch.SetScoreOut(max[i] * 100);
                }
            }

            bool bSaveImageNcc = false;
            if (bSaveImageNcc)
            {
                float[,,] matches = ImageNccs.Data;
                for (int y = 0; y < matches.GetLength(0); y++)
                {
                    for (int x = 0; x < matches.GetLength(1); x++)
                    {
                        matches[y, x, 0] = matches[y, x, 0] * 255;
                    }
                }

                // ImageNccs.Save("D:\\STW\\MatchNcc.bmp");
            }

            /* ////////////////////////////////////////////////////////////////////////////////////
            float[, ,] matches = ImageNcc.Data;
            for (int y = 0; y < matches.GetLength(0); y++)
            {
                for (int x = 0; x < matches.GetLength(1); x++)
                {
                    double matchScore = matches[y, x, 0];
                    if (matchScore > 0.75)
                    {
                        Rectangle rect = new Rectangle(new Point(x, y), new Size(1, 1));
                    //   imgSource.Draw(rect, new Bgr(Color.Blue), 1);
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////// */

            return true;
        }

        public bool MatchHistogram(AlgoImage algoImage, ref MsrMatch msrMatch)
        {
            if (NgImage(algoImage))
            {
                return false;
            }

            var srcImage = (OpenCvGreyImage)algoImage;
            OpenCvGreyImage tplImage = msrMatch.GsTemplete;
            if ((srcImage.Width < tplImage.Width) || (srcImage.Height < tplImage.Height))
            {
                return false;
            }

            Image<Gray, byte> srcByte = srcImage.Image; // srcByte.Save("D:\\STW\\MatchHistoSrc.bmp");
            Image<Gray, byte> tplByte = tplImage.Image; // tplByte.Save("D:\\STW\\MatchHistoTpl.bmp");

            var srcHisto = new DenseHistogram(255, new RangeF(0, 255));
            srcHisto.Calculate<byte>(new Image<Gray, byte>[] { srcByte }, true, null);
            CvInvoke.Normalize(srcHisto, srcHisto, 0, 1, NormType.L2, DepthType.Default);

            var tplHisto = new DenseHistogram(255, new RangeF(0, 255));
            tplHisto.Calculate<byte>(new Image<Gray, byte>[] { tplByte }, true, null);
            CvInvoke.Normalize(tplHisto, tplHisto, 0, 1, NormType.L2, DepthType.Default);

            double corr = CvInvoke.CompareHist(srcHisto, tplHisto, HistogramCompMethod.Correl);

            return true;
        }
    }
}
