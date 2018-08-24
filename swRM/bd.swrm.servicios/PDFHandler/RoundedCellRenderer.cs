using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.PDFHandler
{
    public class RoundedCellRenderer : CellRenderer
    {
        ColorCmykIsColoredBackground colorCmykIsColoredBackground;
        Cell celda;

        public RoundedCellRenderer(Cell modelElement, ColorCmykIsColoredBackground colorCmykIsColoredBackground)
            : base(modelElement)
        {
            this.colorCmykIsColoredBackground = colorCmykIsColoredBackground;
            this.celda = modelElement;
        }

        public override void DrawBackground(DrawContext drawContext)
        {
            Rectangle rect = GetOccupiedAreaBBox();
            PdfCanvas canvas = drawContext.GetCanvas();
            canvas
                .SaveState()
                .RoundRectangle(rect.GetLeft() + 2.5f, rect.GetBottom() + 2.5f, rect.GetWidth() - 5, rect.GetHeight() - 5, 6)
                .SetStrokeColorCmyk(colorCmykIsColoredBackground.CmykColor[0], colorCmykIsColoredBackground.CmykColor[1], colorCmykIsColoredBackground.CmykColor[2], colorCmykIsColoredBackground.CmykColor[3])
                .SetLineWidth(1.5f);
            if (colorCmykIsColoredBackground.IsColoredBackground)
                canvas.SetFillColor(new DeviceCmyk(colorCmykIsColoredBackground.CmykColor[0], colorCmykIsColoredBackground.CmykColor[1], colorCmykIsColoredBackground.CmykColor[2], colorCmykIsColoredBackground.CmykColor[3])).FillStroke();
            else
                canvas.Stroke();
            canvas.RestoreState();
        }

        public override IRenderer GetNextRenderer()
        {
            return new RoundedCellRenderer(this.celda, colorCmykIsColoredBackground);
        }
    }
}
