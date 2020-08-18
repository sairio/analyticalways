using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IStockService
    {
        Task ReadAndSaveFile(string path);
        StockTable FormatData(string line);
    }

    public class StockService : IStockService
    {
        protected ApplicationContext _context { get; set; }

        public StockService(ApplicationContext context)
        {
            _context = context;
        }
        public StockTable FormatData(string line)
        {
            string[] chunks;
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            chunks = line.Split(';');
            if (chunks.Length == 1)
            {
                chunks = line.Split(',');
                if (chunks.Length == 1)
                {
                    return null;
                }
            }
            if (chunks.Length == 4)
            {
                string pointOfSale = chunks[0];
                string product = chunks[1];
                DateTime.TryParse(chunks[2], out DateTime date);
                int.TryParse(chunks[3], out int stock);

                return new StockTable
                {
                    PointOfSale = pointOfSale,
                    Product = product,
                    Date = date,
                    Stock = stock
                };
            }
            return null;
        }

        public async Task ReadAndSaveFile(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    _context.ChangeTracker.AutoDetectChangesEnabled = false;
                    Console.WriteLine("Eliminando Registros Previos");
                    await _context.Database.ExecuteSqlRawAsync("DELETE FROM StockTable");
                    string line;
                    int count = -1;
                    int savedLines = 0;
                    Console.WriteLine("Iniciando Lectura!!");
                    while ((line = sr.ReadLine()) != null)
                    {
                        count++;
                        if (count == 0)
                            continue;

                        StockTable stock = FormatData(line);
                        if (stock != null)
                        {
                            await _context.StockTable.AddAsync(stock);
                            await _context.SaveChangesAsync();
                            savedLines++;
                            Console.Write(".");
                        }
                    }

                    Console.WriteLine("Numero de registros leidos: {0}", count);
                    Console.WriteLine("Numero de registros ingresados en la base de datos: {0}", savedLines);
                }
            }
            else
            {
                Console.WriteLine("Ha ocurrido un error, no se puede encontrar el archivo descargado");
            }
        }
    }
}
