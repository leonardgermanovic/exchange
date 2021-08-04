using FxExchange.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application
{
    public interface IRateProvider 
    {
        ExchangeRate GetExchangeRate(string mainCurrency, string moneyCurrency);
        bool IsSupportedCurrency(string currency);
    }
    public class RateProvider : IRateProvider
    {
        public ExchangeRate GetExchangeRate(string mainCurrency, string moneyCurrency) {
            if (moneyCurrency == "DKK")
            {
                switch (mainCurrency) {
                    case "EUR":
                        return new ExchangeRate { MainCurrency = "EUR", MoneyCurrency = "DKK", Rate = 743.94m };
                    case "USD":
                        return new ExchangeRate { MainCurrency = "USD", MoneyCurrency = "DKK", Rate = 663.11m };
                    case "GBP":
                        return new ExchangeRate { MainCurrency = "GBP", MoneyCurrency = "DKK", Rate = 852.85m };
                    case "SEK":
                        return new ExchangeRate { MainCurrency = "SEK", MoneyCurrency = "DKK", Rate = 76.10m };
                    case "NOK":
                        return new ExchangeRate { MainCurrency = "NOK", MoneyCurrency = "DKK", Rate = 78.40m };
                    case "CHF":
                        return new ExchangeRate { MainCurrency = "CHF", MoneyCurrency = "DKK", Rate = 683.58m };
                    case "JPY":
                        return new ExchangeRate { MainCurrency = "JPY", MoneyCurrency = "DKK", Rate = 5.9740m };
                    default:
                        return null;
                }   
            }
            else return null;
        }

        public bool IsSupportedCurrency(string currency) {
            switch (currency) {
                case "EUR":
                case "USD":
                case "GBP":
                case "SEK":
                case "NOK":
                case "CHF":
                case "JPY":
                case "DKK":
                    return true;
                default: return false;
            }
        }
    }
}
