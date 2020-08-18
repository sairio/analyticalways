using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public partial class StockTable
    {
        public int Id { get; set; }
        public string PointOfSale { get; set; }
        public string Product { get; set; }
        public DateTime? Date { get; set; }
        public int Stock { get; set; }
    }
}
