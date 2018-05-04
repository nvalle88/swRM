using bd.swrm.entidades.ObjectTransfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bd.swrm.servicios.Interfaces
{
    public interface IUploadFileService
    {
        Task<bool> UploadFile(byte[] file, string folder, string fileName);
        bool DeleteFile(string folder, string fileName);
        string FileExtension(string fileName);
        ActivoFijoDocumentoTransfer GetFileActivoFijoDocumento(string folder, string fileName);
    }
}
