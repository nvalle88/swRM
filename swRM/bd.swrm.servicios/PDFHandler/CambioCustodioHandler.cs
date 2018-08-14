using bd.swrm.entidades.Negocio;
using bd.swrm.servicios.Interfaces;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.servicios.PDFHandler
{
    public class CambioCustodioHandler : IEventHandler
    {
        private TransferenciaActivoFijoDetalle transferenciaActivoFijoDetalle;
        private DateTime fechaTransferencia;
        private string nombreUsuario;
        private readonly IPdfMethods pdfMethodsService;

        public CambioCustodioHandler(IPdfMethods pdfMethodsService, DateTime fechaTransferencia, TransferenciaActivoFijoDetalle transferenciaActivoFijoDetalle, string nombreUsuario)
        {
            this.transferenciaActivoFijoDetalle = transferenciaActivoFijoDetalle;
            this.fechaTransferencia = fechaTransferencia;
            this.nombreUsuario = nombreUsuario;
            this.pdfMethodsService = pdfMethodsService;
        }

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            PageSize pageSize = PageSize.A4.Rotate();
            PdfCanvas pdfCanvas = new PdfCanvas(page);

            var posXPrimeraColumna = 35;
            var posYPrimeraColumna = 60;
            pdfCanvas
                .SetFontAndSize(PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA), 7)
                .BeginText()
                .MoveText(posXPrimeraColumna, posYPrimeraColumna)
                .ShowText($"FECHA: {fechaTransferencia.ToString("dd/MM/yyyy")}")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna, posYPrimeraColumna - 15)
                .ShowText($"HORA: {fechaTransferencia.ToString("hh:mm:ss tt")}")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 190, posYPrimeraColumna)
                .ShowText("RECIBIDO POR:")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 190, posYPrimeraColumna - 15)
                .ShowText($"{transferenciaActivoFijoDetalle.UbicacionActivoFijoDestino.Empleado.Persona.Nombres} {transferenciaActivoFijoDetalle.UbicacionActivoFijoDestino.Empleado.Persona.Apellidos}")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 400, posYPrimeraColumna)
                .ShowText("ENTREGADO POR:")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 400, posYPrimeraColumna - 15)
                .ShowText($"{transferenciaActivoFijoDetalle.UbicacionActivoFijoOrigen.Empleado.Persona.Nombres} {transferenciaActivoFijoDetalle.UbicacionActivoFijoOrigen.Empleado.Persona.Apellidos}")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 630, posYPrimeraColumna)
                .ShowText($"PÁGINA: {pdfDoc.GetPageNumber(page)} DE {pdfDoc.GetNumberOfPages()}")
                .EndText()
                .BeginText()
                .MoveText(posXPrimeraColumna + 630, posYPrimeraColumna - 15)
                .ShowText($"USUARIO: {nombreUsuario}")
                .EndText()
                .Stroke();
        }
    }
}
