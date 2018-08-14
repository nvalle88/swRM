using bd.swrm.servicios.Interfaces;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.Servicios
{
    public class PdfMethodsService : IPdfMethods
    {
        public void AdicionarCeldaHeader(string titulo, Table table, float fontSize = 7, TextAlignment textAlignment = TextAlignment.CENTER, bool isHeaderCell = false, bool isFooterCell = false)
        {
            Cell cell = new Cell().Add(new Paragraph(titulo));
            cell.SetTextAlignment(textAlignment);
            cell.SetPadding(5);
            cell.SetFontSize(fontSize);
            cell.SetBackgroundColor(new DeviceRgb(45, 69, 134));
            cell.SetFontColor(new DeviceRgb(255, 255, 255));

            if (isHeaderCell)
                table.AddHeaderCell(cell);
            else if (isFooterCell)
                table.AddFooterCell(cell);
            else
                table.AddCell(cell);
        }

        public void AdicionarValorCelda(string valorCelda, Table table, TextAlignment textAlignment = TextAlignment.CENTER, int colspan = 1, bool pintarBordeIzquierdo = true, bool pintarBordeDerecho = true, bool pintarBordeArriba = true, bool pintarBordeAbajo = true, bool isBold = false, float fontSize = 7, DeviceRgb backgroundColor = null, float? paddingLeft = 0, Image imagen = null, bool isHeaderCell = false, bool isFooterCell = false)
        {
            Paragraph parrafoCelda = new Paragraph();
            if (imagen != null)
                parrafoCelda.Add(imagen).Add(" ");

            Cell nuevaCelda = new Cell(1, colspan).Add(parrafoCelda.Add(valorCelda));
            nuevaCelda.SetFontColor(new DeviceRgb(0, 0, 0));

            if (!pintarBordeIzquierdo)
                nuevaCelda.SetBorderLeft(Border.NO_BORDER);

            if (!pintarBordeDerecho)
                nuevaCelda.SetBorderRight(Border.NO_BORDER);

            if (!pintarBordeArriba)
                nuevaCelda.SetBorderTop(Border.NO_BORDER);

            if (!pintarBordeAbajo)
                nuevaCelda.SetBorderBottom(Border.NO_BORDER);

            if (isBold)
                nuevaCelda.SetBold();

            nuevaCelda.SetVerticalAlignment(VerticalAlignment.MIDDLE);
            nuevaCelda.SetTextAlignment(textAlignment);
            nuevaCelda.SetFontSize(fontSize);
            nuevaCelda.SetBackgroundColor(backgroundColor ?? new DeviceRgb(255, 255, 255));

            if (paddingLeft != null)
                nuevaCelda.SetPaddingLeft((float)paddingLeft);

            if (isHeaderCell)
                table.AddHeaderCell(nuevaCelda);
            else if (isFooterCell)
                table.AddFooterCell(nuevaCelda);
            else
                table.AddCell(nuevaCelda);
        }
    }
}
