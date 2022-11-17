using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Planbss
{
    public class DrawOjbect
    {
        public Pen gsColor { get; set; }
    }

    public class DrawLine : DrawOjbect
    {
        public Point mpt1;
        public Point mpt2;

        public void CreateObject(Pen mColor, Point pt1, Point pt2)
        {
            base.gsColor = mColor; mpt1 = pt1; mpt2 = pt2;
        }
        public void CreateObject(Pen mColor, PointD pt1, PointD pt2)
        {
            base.gsColor = mColor; mpt1.X = (int)(pt1.X + 0.5f); mpt1.Y = (int)(pt1.Y + 0.5f);
            mpt2.X = (int)(pt2.X + 0.5f); mpt2.Y = (int)(pt2.Y + 0.5f);
        }
    }

    public class DrawRect : DrawOjbect
    {
        public Rectangle mrArea;
        public bool mbCircle;

        public void CreateObject(Pen mColor, Rectangle rArea, bool bCircle)
        {
            base.gsColor = mColor; mrArea = rArea; mbCircle = bCircle;
        }
    }

    public class DrawDash : DrawOjbect
    {
        private int mnType;
        private Rectangle mrArea;

        public void CreateObject(Pen mColor, Rectangle rArea, int nType)
        {
            base.gsColor = mColor; mrArea = rArea; mnType = nType;
        }
    }

    public class DrawText : DrawOjbect
    {
        private Point mpt;
        private string mText;
        private int mnHeight;

        public void CreateObject(Pen mColor, Point ptStd, string strText, int nHeight)
        {
            base.gsColor = mColor; mpt = ptStd; mText = strText; mnHeight = nHeight;
        }
    }

    public class DrawCross : DrawOjbect
    {
        public Point mpt;
        public int mnLength;
        public int mnColor;

        public void CreateObject(Pen mColor, Point pt, int nLength)
        {
            base.gsColor = mColor; mpt = pt; mnLength = nLength;
        }
    }

    public class HelpDraw
    {
        public HelpDraw()
        {
            mlDrawLine = new List<DrawLine>();
            mlDrawRect = new List<DrawRect>();
            mlDrawDash = new List<DrawDash>();
            mlDrawText = new List<DrawText>();
            mlDrawCross = new List<DrawCross>();
        }

        private List<DrawLine> mlDrawLine;
        private List<DrawRect> mlDrawRect;
        private List<DrawDash> mlDrawDash;
        private List<DrawText> mlDrawText;
        private List<DrawCross> mlDrawCross;

        public void Clear()
        {
            mlDrawLine.Clear();
            mlDrawRect.Clear();
            mlDrawDash.Clear();
            mlDrawText.Clear();
            mlDrawCross.Clear();
        }

        public void Copy(HelpDraw pDrawObject)
        {
        }

        public long SizeLine() { return mlDrawLine.Count; }
        public long SizeRect() { return mlDrawRect.Count; }
        public long SizeDash() { return mlDrawDash.Count; }
        public long SizeText() { return mlDrawText.Count; }
        public long SizeCross() { return mlDrawCross.Count; }

        public DrawLine GetLine(int nIndex) { return mlDrawLine[nIndex]; }
        public DrawRect GetRect(int nIndex) { return mlDrawRect[nIndex]; }
        public DrawDash GetDash(int nIndex) { return mlDrawDash[nIndex]; }
        public DrawText GetText(int nIndex) { return mlDrawText[nIndex]; }
        public DrawCross GetCross(int nIndex) { return mlDrawCross[nIndex]; }

        public long AddLine(Pen Color, Point pt1, Point pt2)
        {
            //    _ERRColor(nColor);
            var Line = new DrawLine();
            Line.CreateObject(Color, pt1, pt2);
            mlDrawLine.Add(Line); return (mlDrawLine.Count - 1);
        }

        public long AddLine(Pen Color, PointD pt1, PointD pt2)
        {
            //    _ERRColor(nColor);
            var Line = new DrawLine();
            Line.CreateObject(Color, pt1, pt2);
            mlDrawLine.Add(Line); return (mlDrawLine.Count - 1);
        }

        public long AddRect(Pen Color, Rectangle rArea, bool bCircle)
        {
            //     _ERRColor(nColor);
            var Rect = new DrawRect();
            Rect.CreateObject(Color, rArea, bCircle);
            mlDrawRect.Add(Rect); return (mlDrawRect.Count - 1);
        }

        public long AddDash(Pen Color, Rectangle rArea, int nType)
        {
            //    _ERRColor(nColor);
            var Dash = new DrawDash();
            Dash.CreateObject(Color, rArea, nType);
            mlDrawDash.Add(Dash); return (mlDrawDash.Count - 1);
        }

        public long AddText(Pen Color, Point pt1, string strText, int nHeight)
        {
            //    _ERRColor(nColor);
            var Text = new DrawText();
            Text.CreateObject(Color, pt1, strText, nHeight);
            mlDrawText.Add(Text); return (mlDrawText.Count - 1);
        }

        public long AddCross(Pen Color, Point pt1, int nLength)
        {
            //    _ERRColor(nColor);	
            var Cross = new DrawCross();
            Cross.CreateObject(Color, pt1, nLength);
            mlDrawCross.Add(Cross); return mlDrawCross.Count - 1;
        }

        public long AddCross(Pen Color, PointD pt, int nLength)
        {
            //   _ERRColor(nColor);		
            var Cross = new DrawCross();

            var ptCross = new Point(); //((long)(pt.dX + 0.5f), (long)(pt.dY + 0.5f));

            ptCross.X = (int)(pt.X + 0.5f);
            ptCross.Y = (int)(pt.Y + 0.5f);

            Cross.CreateObject(Color, ptCross, nLength);
            mlDrawCross.Add(Cross); return mlDrawCross.Count - 1;
        }
        /*
	    long AddSquare(int nColor, QUADRANGLE QRangle)
	    { 
		    AddLine(nColor, CPoint((int)(QRangle.Vertex[0].dX+0.5f), (int)(QRangle.Vertex[0].dY+0.5f)),
						    CPoint((int)(QRangle.Vertex[1].dX+0.5f), (int)(QRangle.Vertex[1].dY+0.5f)));
		    AddLine(nColor, CPoint((int)(QRangle.Vertex[1].dX+0.5f), (int)(QRangle.Vertex[1].dY+0.5f)),
						    CPoint((int)(QRangle.Vertex[2].dX+0.5f), (int)(QRangle.Vertex[2].dY+0.5f)));
		    AddLine(nColor, CPoint((int)(QRangle.Vertex[2].dX+0.5f), (int)(QRangle.Vertex[2].dY+0.5f)),
						    CPoint((int)(QRangle.Vertex[3].dX+0.5f), (int)(QRangle.Vertex[3].dY+0.5f)));
		    AddLine(nColor, CPoint((int)(QRangle.Vertex[3].dX+0.5f), (int)(QRangle.Vertex[3].dY+0.5f)),
						    CPoint((int)(QRangle.Vertex[0].dX+0.5f), (int)(QRangle.Vertex[0].dY+0.5f)));
		    return (long)(m_vDrawLine.size()-1);
	    }
        */
        /*  
          long AddSquare(int nColor, DPOINT dPt1, DPOINT dPt2, DPOINT dPt3, DPOINT dPt4)
          { 		
              AddLine(nColor, CPoint((int)(dPt1.dX+0.5f), (int)(dPt1.dY+0.5f)), CPoint((int)(dPt2.dX+0.5f), (int)(dPt2.dY+0.5f)));
              AddLine(nColor, CPoint((int)(dPt2.dX+0.5f), (int)(dPt2.dY+0.5f)), CPoint((int)(dPt3.dX+0.5f), (int)(dPt3.dY+0.5f)));
              AddLine(nColor, CPoint((int)(dPt3.dX+0.5f), (int)(dPt3.dY+0.5f)), CPoint((int)(dPt4.dX+0.5f), (int)(dPt4.dY+0.5f)));
              AddLine(nColor, CPoint((int)(dPt4.dX+0.5f), (int)(dPt4.dY+0.5f)), CPoint((int)(dPt1.dX+0.5f), (int)(dPt1.dY+0.5f)));
              return (long)(m_vDrawLine.size()-1);
          }
         */

        //    void _ERRColor(int nColor)
        //    {
        //   if( (nColor < 0) || (nColor > 19) )
        //      ASSERT(FALSE);
        //    } 
    }
}
