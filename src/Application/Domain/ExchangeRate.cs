using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application.Domain
{
    public class ExchangeRate
    {
        public string MainCurrency { get; set; }
        public string MoneyCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
