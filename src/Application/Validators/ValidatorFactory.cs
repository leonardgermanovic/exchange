using FxExchange.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxExchange.Application.Validators
{
    public class ValidatorFactory
    {
        public IValidator<T> CreateValidator<T>() where T : BaseCommand 
        {
            if (typeof(T) == typeof(ConvertCommand)) 
            {
                return (IValidator<T>)new ConvertCommandValidator();
            }
            return null;
        }
    }
}
