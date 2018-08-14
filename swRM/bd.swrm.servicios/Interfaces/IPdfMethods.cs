using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.Interfaces
{
    public interface IPdfMethods
    {
        void AdicionarCeldaHeader(string titulo, Table table, float fontSize = 7, TextAlignment textAlignment = TextAlignment.CENTER, bool isHeaderCell = false, bool isFooterCell = false);
        void AdicionarValorCelda(string valorCelda, Table table, TextAlignment textAlignment = TextAlignment.CENTER, int colspan = 1, bool pintarBordeIzquierdo = true, bool pintarBordeDerecho = true, bool pintarBordeArriba = true, bool pintarBordeAbajo = true, bool isBold = false, float fontSize = 7, DeviceRgb backgroundColor = null, float? paddingLeft = 0, Image imagen = null, bool isHeaderCell = false, bool isFooterCell = false);
    }
}
