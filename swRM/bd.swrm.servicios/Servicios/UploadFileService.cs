using bd.swrm.entidades.ObjectTransfer;
using bd.swrm.servicios.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace bd.swrm.servicios.Servicios
{
    public class UploadFileService : IUploadFileService
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadFileService(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public async Task<bool> UploadFile(byte[] file, string folder, string fileName)
        {
            try
            {
                var stream = new MemoryStream(file);
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, folder);
                var targetFile = Path.Combine(targetDirectory, fileName);

                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                using (var fileStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                    await stream.CopyToAsync(fileStream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteFile(string url)
        {
            try
            {
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, url);
                if (File.Exists(targetDirectory))
                {
                    File.Delete(targetDirectory);
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }

        public string FileExtension(string fileName)
        {
            try
            {
                string[] arr = fileName.Split('.');
                return $".{arr[arr.Length - 1]}";
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        private byte[] GetBytesFromFile(string folder, string fileName)
        {
            try
            {
                var targetDirectory = Path.Combine(_hostingEnvironment.WebRootPath, $"{folder}\\{fileName}");
                var file = new FileStream(targetDirectory, FileMode.Open, FileAccess.Read);

                byte[] data;
                using (var br = new BinaryReader(file))
                    data = br.ReadBytes((int)file.Length);

                return data;
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public DocumentoActivoFijoTransfer GetFileDocumentoActivoFijo(string folder, int idDocumentoActivoFijo, string fileName)
        {
            try
            {
                string extensionFile = FileExtension(fileName);
                byte[] data = GetBytesFromFile(folder, $"{idDocumentoActivoFijo}{extensionFile}");
                if (data.Length > 0)
                {
                    return new DocumentoActivoFijoTransfer
                    {
                        Nombre = fileName,
                        Fichero = data
                    };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
