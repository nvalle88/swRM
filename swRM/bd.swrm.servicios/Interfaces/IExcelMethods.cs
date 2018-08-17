using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace bd.swrm.servicios.Interfaces
{
    public interface IExcelMethods
    {
        ExcelRange Ajustar(ExcelWorksheet ws, string texto, int filaInicial, int columnaInicial, int? filaFinal = null, int? columnaFinal = null, System.Drawing.Font font = null, bool isBold = false, ExcelHorizontalAlignment excelHorizontalAlignment = ExcelHorizontalAlignment.Left, bool isMerge = false, ExcelVerticalAlignment excelVerticalAlignment = ExcelVerticalAlignment.Center, bool isWrapText = false, bool isUnderLine = false, bool isItalic = false);
        Font ArialFont(float size);
    }
}
