using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDFSample.Models
{
    public class DottedHelper : DottedLineSeparator
    {
        protected float dash = 5;
        protected float phase = 2.5f;

        public float getDash()
        {
            return dash;
        }

        public float getPhase()
        {
            return phase;
        }

        public void setDash(float dash)
        {
            this.dash = dash;
        }

        public void setPhase(float phase)
        {
            this.phase = phase;
        }

        public void draw(PdfContentByte canvas, float llx, float lly, float urx, float ury, float y)
        {
            canvas.SaveState();
            canvas.SetLineWidth(lineWidth);
            canvas.SetLineDash(dash, gap, phase);
            DrawLine(canvas, llx, urx, y);
            canvas.RestoreState();
        }
    }
}