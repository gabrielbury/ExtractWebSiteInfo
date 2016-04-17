using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Models
{
    public class ItemModel
    {
        public Guid Id { get; set; }
        public String Lote { get; set; }
        public String Descricao { get; set; }
        public String UrlFoto { get; set; }
        public Decimal LanceInicial { get; set; }
        public Decimal MaiorLance { get; set; }
        public int QuantidadeLances { get; set; }
    }
}
