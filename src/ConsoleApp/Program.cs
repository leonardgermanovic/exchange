using FxExchange.Application;
using FxExchange.Application.Commands;
using FxExchange.Application.Validators;
using System;

namespace FxExchange.ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>");
            var exchangeService = new ExchangeService(new RateProvider(), new ValidatorFactory());
            string mainCurrency = null;
            string moneyCurrency = null;
            decimal amount = 0;
            try
            {
                mainCurrency = args[0].Split("/")[0];
                moneyCurrency = args[0].Split("/")[1];
                amount = decimal.Parse(args[1]);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to parse arguments, provide arguments correctly!");
                Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>");
                Console.WriteLine("Example: Exchange EUR/DKK 1");
            }
            var result = exchangeService.Convert(new ConvertCommand {
                MainCurrency = mainCurrency,
                MoneyCurrency = moneyCurrency,
                Amount = amount
            });
            if (result.isValid) Console.WriteLine($"{result.Amount}");
            else Console.WriteLine($"{result.ErrorMessage}");
            Console.ReadLine();
        }
    }
}
