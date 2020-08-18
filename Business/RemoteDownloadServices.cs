using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

namespace Business
{
    public interface IRemoteDownloadServices
    {
        void DownloadRemoteFile(string url, string fileName);
        void EnsureFolder();
        string GetFolderToDownload();
        string GetPathFile(string filename);
    }

    public class RemoteDownloadServices : IRemoteDownloadServices
    {
        private readonly IConfiguration _conf;

        public RemoteDownloadServices(IConfiguration conf)
        {
            _conf = conf;
        }

        public void DownloadRemoteFile(string url, string fileName)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("Url invalida");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Nombre de archivo invalido");

            EnsureFolder();

            Uri uri = new Uri(url);
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileComplete);

            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);

            webClient.DownloadFile(uri, fileName);
            Console.WriteLine("Se ha descargado satisfactoriamente el archivo");
        }

        public void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("{0}    Descargado {1} de {2} bytes. {3} % completado...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }

        public void DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("{0}    Se ha cancelado la descarga del archivo...",
                (string)e.UserState);
            }

            if (e.Error != null)
            {
                Console.WriteLine("{0}    Ha ocurrido un error en la descarga del archivo...",
                (string)e.UserState);
                throw e.Error;
            }
        }

        public void EnsureFolder()
        {
            string folder = _conf.GetSection("FolderName").Value;
            string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public string GetFolderToDownload()
        {
            string folder = _conf.GetSection("FolderName").Value;
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folder);
        }

        public string GetPathFile(string filename)
        {
            string folder = _conf.GetSection("FolderName").Value;
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folder, filename);
        }
    }
}
