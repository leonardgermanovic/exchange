using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application.Commands
{
    public class ConvertCommand : BaseCommand
    {
        public string MainCurrency { get; set; }
        public string MoneyCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
