using FxExchange.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application.Validators
{
    public interface IValidator<T> where T: BaseCommand
    {
        public BaseResponse Validate(T command);
    }
    public class ConvertCommandValidator: IValidator<ConvertCommand>
    {
        public BaseResponse Validate(ConvertCommand command)
        {
            if (command.MainCurrency != null && command.MainCurrency.Length != 3) return new ConvertResponse { ErrorCode = "INVALID_LENGTH", ErrorMessage = "MainCurrency length should be 3" };
            if (command.MoneyCurrency != null && command.MoneyCurrency.Length != 3) return new ConvertResponse { ErrorCode = "INVALID_LENGTH", ErrorMessage = "MoneyCurrency length should be 3" };
            if (command.Amount <= 0) return new ConvertResponse { ErrorCode = "NEGATIVE", ErrorMessage = "Amount should be positive" };
            return new ConvertResponse();
        }
    }
}
