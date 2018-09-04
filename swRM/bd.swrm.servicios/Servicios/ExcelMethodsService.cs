using bd.swrm.servicios.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace bd.swrm.servicios.Servicios
{
    public class ExcelMethodsService : IExcelMethods
    {
        public ExcelRange Ajustar(ExcelWorksheet ws, string texto, int filaInicial, int columnaInicial, int? filaFinal = null, int? columnaFinal = null, Font font = null, bool isBold = false, ExcelHorizontalAlignment excelHorizontalAlignment = ExcelHorizontalAlignment.Left, bool isMerge = false, ExcelVerticalAlignment excelVerticalAlignment = ExcelVerticalAlignment.Center, bool isWrapText = false, bool isUnderLine = false, bool isItalic = false)
        {
            var excelRange = filaFinal != null && columnaFinal != null ? ws.Cells[filaInicial, columnaInicial, (int)filaFinal, (int)columnaFinal] : ws.Cells[filaInicial, columnaInicial];

            double cellSize = ws.Cells[filaInicial, columnaInicial].Worksheet.Column(columnaInicial).Width;
            double proposedCellSize = texto.Trim().Length * 1.3;

            if (cellSize <= proposedCellSize)
                excelRange.Worksheet.Column(columnaInicial).Width = proposedCellSize;

            if (font != null)
                excelRange.Style.Font.SetFromFont(font);
            
            excelRange.Style.HorizontalAlignment = excelHorizontalAlignment;
            excelRange.Style.VerticalAlignment = excelVerticalAlignment;
            excelRange.Merge = isMerge;
            excelRange.Style.Font.Bold = isBold;
            excelRange.Style.WrapText = isWrapText;
            excelRange.Value = texto;
            excelRange.Style.Font.UnderLine = isUnderLine;
            excelRange.Style.Font.Italic = isItalic;
            return excelRange;
        }

        public Font ArialFont(float size)
        {
            return new Font("Arial", size);
        }

        public Font CalibriFont(float size)
        {
            return new Font("Calibri", size);
        }

        public void ChangeBackground(ExcelRange excelRange, Color color)
        {
            excelRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelRange.Style.Fill.BackgroundColor.SetColor(color);
        }
    }
}
