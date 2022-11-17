using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    internal class LegendPanel
    {
        private void Draw()
        {
            //            static int m_nLegendOffset = 0;

            //            if (!m_pSurfaceDraw->IsValid())
            //                return;

            //            CRect rc;
            //            int nScrWidth = rcScr.Width();
            //            int nScrHeigth = rcScr.Height();

            //            int nBoxWidth = PERCENT(nScrWidth, SC_BOX_WIDTH);
            //            if (nBoxWidth < SC_BOX_MIN_WIDTH) nBoxWidth = SC_BOX_MIN_WIDTH;
            //            if (nBoxWidth > SC_BOX_MAX_WIDTH) nBoxWidth = SC_BOX_MAX_WIDTH;
            //            int nFontSize = nBoxWidth / 3;
            //            int nGap = nFontSize;

            //            rc.right = rcScr.right - nGap;
            //            rc.left = rc.right - nBoxWidth;
            //            rc.top = rcScr.top + nGap;
            //            rc.bottom = rc.top + PERCENT(nScrHeigth, SC_BOX_HEIGHT);

            //            BOOL m_bView = (m_nWorkSurfaceMode != ST_IMAGE && IsShowColorBar());
            //            int nSize = rcScr.right - rc.left;
            //            int nInc = (int)((m_fTmFrame * nSize) / 500);
            //            if (nInc <= 0) nInc = 1;
            //            if (m_bView)
            //            {
            //                if (nSize > m_nLegendOffset)
            //                {
            //                    m_nLegendOffset += nInc;
            //                    if (nSize < m_nLegendOffset)
            //                        m_nLegendOffset = nSize;
            //                    rc.OffsetRect(nSize - m_nLegendOffset, 0);
            //                }
            //            }
            //            else
            //            {
            //                if (m_nLegendOffset <= 0)
            //                    return;
            //                m_nLegendOffset -= nInc;
            //                rc.OffsetRect(nSize - m_nLegendOffset, 0);
            //            }

            //            m_font.SetAlign(GFA_CENTER);

            //            float fmin, fmax;
            //            float fminSet, fmaxSet;
            //            float fMinRate, fMaxRate;
            //            {
            //                m_pSurfaceDraw->GetMinMax(&fmin, &fmax);
            //                m_pSurfaceDraw->GetRangeMinMaxValue(&fminSet, &fmaxSet);
            //                m_pSurfaceDraw->GetRangeMinMaxRate(&fMinRate, &fMaxRate);
            //            }

            //            float fCen = (float)(rc.left + rc.right) / 2.0f;
            //            float fSize = (float)nFontSize;
            //            int nFrameThick = 4;

            //            CString s;

            //            glColor3f(0, 0, 0);
            //            glRecti(rc.left - nFrameThick, rc.top - nFrameThick, rc.right + nFrameThick, rc.bottom + nFrameThick);

            //            s.Format(_T("%4.0f"), ConvAbsZ(fmax));
            //            DrawText(fCen, (float)rc.top - nFrameThick, fSize, RGB(255, 255, 255), s);
            //            s.Format(_T("%4.0f"), ConvAbsZ(fmin));
            //            DrawText(fCen, (float)rc.bottom + fSize / 2 + nFrameThick, fSize, RGB(255, 255, 255), s);

            //            glColor3f(1, 1, 1);
            //            GL_EXT(glActiveTextureARB(GL_TEXTURE0_ARB));
            //            glEnable(GL_TEXTURE_1D);
            //            glTexEnvi(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_KEEP);
            //            m_TexColor.Bind();

            //            glBegin(GL_POLYGON);
            //            glTexCoord1f(1);
            //            glVertex2i(rc.left, rc.top);
            //            glTexCoord1f(1);
            //            glVertex2i(rc.right, rc.top);
            //            glTexCoord1f(0);
            //            glVertex2i(rc.right, rc.bottom);
            //            glTexCoord1f(0);
            //            glVertex2i(rc.left, rc.bottom);
            //            glEnd();

            //            GL_EXT(glActiveTextureARB(GL_TEXTURE0_ARB));
            //            glDisable(GL_TEXTURE_1D);

            //#define _DrawLine(x1,y1,x2,y2) glBegin(GL_LINES); glVertex2f ((x1),(y1)); glVertex2f ((x2),(y2)); glEnd();

            //            fCen = rc.right + fSize * 2;
            //            float fSt = (float)(rc.left - 20);
            //            float fEd = (float)(rc.right + 10);
            //            float fH = (float)rc.Height();

            //            m_font.SetAlign(GFA_RIGHT);
            //            float fHalfFont = fSize / 2;

            //            float fPosMaxY = rc.top + (fH - (fH * fMaxRate));
            //            glPushMatrix();
            //            glColor3f(1, 1, 1);
            //            glBegin(GL_TRIANGLES);
            //            glVertex3f(fSt + 1, fPosMaxY - fSize / 2, 0);
            //            glVertex3f((float)rc.left, fPosMaxY, 0);
            //            glVertex3f(fSt + 1, fPosMaxY, 0);
            //            glEnd();
            //            glTranslatef(fSt, fPosMaxY, 0);
            //            glRotatef(180, 1, 0, 0);
            //            glScalef(fSize, fSize * 1.5f, 1);
            //            glColor3f(0, 1, 1);
            //            int nMaxTextWid = (int)(m_font.Print(_T("%4.0f"), ConvAbsZ(fmaxSet)) * fSize);
            //            glPopMatrix();

            //            float fPosMinY = rc.top + (fH - (fH * fMinRate));
            //            glPushMatrix();
            //            glColor3f(1, 1, 1);
            //            glBegin(GL_TRIANGLES);
            //            glVertex3f(fSt + 1, fPosMinY, 0);
            //            glVertex3f((float)rc.left, fPosMinY, 0);
            //            glVertex3f(fSt + 1, fPosMinY + fSize / 2, 0);
            //            glEnd();
            //            glTranslatef(fSt, fPosMinY + fSize - 1, 0);
            //            glRotatef(180, 1, 0, 0);
            //            glScalef(fSize, fSize * 1.5f, 1);
            //            glColor3f(1, 0, 1);
            //            int nMinTextWid = (int)(m_font.Print(_T("%4.0f"), ConvAbsZ(fminSet)) * fSize);
            //            glPopMatrix();

            //            m_font.SetAlign(GFA_CENTER);

            //            CSingleLock sync(&m_csPos, TRUE );
            //            m_rcUIPos[UID_COLOR_MAX].SetRect((int)fSt - nMaxTextWid, (int)(fPosMaxY - fSize), rc.left, (int)(fPosMaxY));
            //            m_rcUIPos[UID_COLOR_MIN].SetRect((int)fSt - nMinTextWid, (int)(fPosMinY), rc.left, (int)(fPosMinY + fSize));
            //            m_rcColorBar.CopyRect(rc);
            //            sync.Unlock();
        }
    }
}
