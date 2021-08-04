using FxExchange.Application.Commands;
using FxExchange.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application
{
    public class ExchangeService
    {
        private readonly IRateProvider _rateProvider;
        private readonly ValidatorFactory _validatorFactory;
        private const string __SYSTEM_CURRENCY = "DKK";
        public ExchangeService(IRateProvider rateProvider, ValidatorFactory validatorFactory) 
        {
            _rateProvider = rateProvider;
            _validatorFactory = validatorFactory;
        }
        public ConvertResponse Convert(ConvertCommand request)
        {
            var validator = _validatorFactory.CreateValidator<ConvertCommand>();
            if (validator == null)
                return new ConvertResponse { ErrorCode = "NO_VALIDATOR", ErrorMessage = "No validator found" };
            var response = validator.Validate(request);
            if (!response.isValid) return new ConvertResponse { ErrorCode = response.ErrorCode, ErrorMessage = response.ErrorMessage };
            if (!(_rateProvider.IsSupportedCurrency(request.MainCurrency) && _rateProvider.IsSupportedCurrency(request.MoneyCurrency)))
                return new ConvertResponse { ErrorCode = "CURR_NOT_SUPPORTED", ErrorMessage = "Main or money currency is not supported" };
            if (request.MainCurrency == request.MoneyCurrency)
                return new ConvertResponse { Amount = request.Amount };
            if (request.MoneyCurrency == __SYSTEM_CURRENCY)
            {
                var exchangeRate = _rateProvider.GetExchangeRate(request.MainCurrency, request.MoneyCurrency);
                if (exchangeRate != null)
                {
                    return new ConvertResponse { Amount = ConvertByMoneyCurrencyRate(exchangeRate.Rate, request.Amount) };
                }
            }
            else if (request.MainCurrency == __SYSTEM_CURRENCY)
            {
                var exchangeRate = _rateProvider.GetExchangeRate(request.MoneyCurrency, request.MainCurrency);
                if (exchangeRate != null)
                {
                    return new ConvertResponse { Amount = ConvertByMainCurrencyRate(exchangeRate.Rate, request.Amount) };
                }
            }
            else 
            {
                var moneyRate = _rateProvider.GetExchangeRate(request.MainCurrency, __SYSTEM_CURRENCY);
                var mainRate = _rateProvider.GetExchangeRate(request.MoneyCurrency, __SYSTEM_CURRENCY);
                if (moneyRate != null && mainRate != null)
                {
                    var systemAmount = ConvertByMoneyCurrencyRate(moneyRate.Rate, request.Amount);
                    var amount = ConvertByMainCurrencyRate(mainRate.Rate, systemAmount);
                    return new ConvertResponse { Amount = amount };
                }
            }
            return new ConvertResponse { ErrorCode = "NO_CONVERT", ErrorMessage = "Not supported convert" };
        }

        private decimal ConvertByMoneyCurrencyRate(decimal rate, decimal amount) 
        {
            return RoundAmount(RoundAmount(rate / 100) * RoundAmount(amount));
        }

        private decimal ConvertByMainCurrencyRate(decimal rate, decimal amount)
        {
            return RoundAmount(RoundAmount(1 / (rate / 100)) * RoundAmount(amount));
        }

        private decimal RoundAmount(decimal amount) 
        {
            return Math.Round(amount, 8, MidpointRounding.ToEven);
        }
    }
}
