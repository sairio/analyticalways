using Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Analyticalways
{
    public class StockApplication
    {
        private readonly IRemoteDownloadServices _business;
        private readonly IConfiguration _config;
        private readonly IStockService _stockService;

        public StockApplication(
            IRemoteDownloadServices business, 
            IConfiguration config,
            IStockService stockService
            )
        {
            _business = business;
            _config = config;
            _stockService = stockService;
        }

        internal async Task Run()
        {
            Console.WriteLine("Bienvenido(a) al lector de archivos csv analyticalways");

            string url = _config.GetSection("UrlToDownload").Value;

            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("No se puede iniciar la descarga del archivo, no se ha indicado la url");
            }
            else
            {
                Console.WriteLine("Iniciaremos la descarga, Este proceso puede tardar un poco, por favor tenga un poco de paciencia!!");

                string pathFile = _business.GetPathFile("stock.csv");
                _business.DownloadRemoteFile(url, pathFile);

                Console.WriteLine("Iniciando el registro en base de datos, por favor tenga un poco de paciencia!!");
                await _stockService.ReadAndSaveFile(pathFile);
                Console.WriteLine("Culminado el registro en base de datos, gracias por esperar!!");
            }

            Console.WriteLine("PRESS <ENTER> TO EXIT");
        }
    }
}
