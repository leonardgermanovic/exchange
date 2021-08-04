using System;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using FxExchange.Application;
using NSubstitute;
using FxExchange.Application.Domain;
using FxExchange.Application.Validators;
using FxExchange.Application.Commands;

namespace FxExchange.ConsoleApp.UnitTests
{
    public class ExchangeServiceTests
    {
        public ExchangeService BuildExchangeService()
        {
            return new ExchangeService(new RateProvider(), new ValidatorFactory());
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "EUR", 7.4394m },
            new object[] { "USD", 6.6311m },
            new object[] { "GBP", 8.5285m },
            new object[] { "SEK", 0.7610m },
            new object[] { "NOK", 0.7840m },
            new object[] { "CHF", 6.8358m },
            new object[] { "JPY", 0.059740m }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Convert_DKKMoneyCurrencyAndOneAmount_ReturnsExpectedResult(string mainCurrency, decimal expectedResult)
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = mainCurrency, MoneyCurrency = "DKK", Amount = 1m });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> SameData =>
        new List<object[]>
        {
            new object[] { "EUR", 1 },
            new object[] { "USD", 2 },
            new object[] { "GBP", 3 },
            new object[] { "SEK", 4 },
            new object[] { "NOK", 5 },
            new object[] { "CHF", 6 },
            new object[] { "JPY", 7 },
            new object[] { "DKK", 8 }
        };

        [Theory]
        [MemberData(nameof(SameData))]
        public void Convert_SameMainAndMoneyCurrency_ReturnsSameAmount(string currency, decimal amount)
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = currency, MoneyCurrency = currency, Amount = amount });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(amount);
        }

        public static IEnumerable<object[]> Data100 =>
        new List<object[]>
        {
            new object[] { "EUR", 743.94m },
            new object[] { "USD", 663.11m },
            new object[] { "GBP", 852.85m },
            new object[] { "SEK", 76.10m },
            new object[] { "NOK", 78.40m },
            new object[] { "CHF", 683.58m },
            new object[] { "JPY", 5.9740m }
        };

        [Theory]
        [MemberData(nameof(Data100))]
        public void Convert_DKKMoneyCurrencyAnd100Amount_ReturnsExpectedResult(string mainCurrency, decimal expectedResult)
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = mainCurrency, MoneyCurrency = "DKK", Amount = 100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(expectedResult);
        }

        [Fact]
        public void Convert_NotSupportedMoneyCurrency_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "DKK", MoneyCurrency = "LTU", Amount = 100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("CURR_NOT_SUPPORTED");
        }

        [Fact]
        public void Convert_NotSupportedMainCurrency_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "LTU", MoneyCurrency = "DKK", Amount = 100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("CURR_NOT_SUPPORTED");
        }

        [Fact]
        public void Convert_InvalidLengthMainCurrency_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "LT", MoneyCurrency = "DKK", Amount = 100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("INVALID_LENGTH");
        }

        [Fact]
        public void Convert_InvalidLengthMoneyCurrency_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "LTU", MoneyCurrency = "DK", Amount = 100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("INVALID_LENGTH");
        }

        [Fact]
        public void Convert_ZeroAmount_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "EUR", MoneyCurrency = "DKK", Amount = 0m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("NEGATIVE");
        }

        [Fact]
        public void Convert_NegativeAmount_ReturnsError()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "LTU", MoneyCurrency = "DKK", Amount = -100m });

            // ASSSERT
            result.Should().NotBeNull();
            result.ErrorCode.Should().Be("NEGATIVE");
        }

        [Fact]
        public void Convert_DKKEUR_ReturnsCorrectAmount()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "DKK", MoneyCurrency = "EUR", Amount = 1m });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(0.13441944m);
        }

        [Fact]
        public void Convert_DKKEUR01Amount_ReturnsCorrectAmount()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "DKK", MoneyCurrency = "EUR", Amount = 0.1m });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(0.01344194m);
        }

        [Fact]
        public void Convert_EURUSDAmount_ReturnsCorrectAmount()
        {
            // ARRANGE
            var service = BuildExchangeService();

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "EUR", MoneyCurrency = "USD", Amount = 1m });

            // ASSSERT
            result.Should().NotBeNull();
            result.Amount.Should().Be(1.12189529m);
        }

        [Fact]
        public void Convert_SupportedCurrency_CallsIsSupportedWithCorrectArguments()
        {
            // ARRANGE
            var rateProvider = Substitute.For<IRateProvider>();
            rateProvider.IsSupportedCurrency(Arg.Any<string>()).Returns(true);
            var service = new ExchangeService(rateProvider, new ValidatorFactory());

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "EUR", MoneyCurrency = "DKK", Amount = 1m });

            // ASSSERT
            result.Should().NotBeNull();
            rateProvider.Received(1).IsSupportedCurrency(Arg.Is<string>(x => x == "EUR"));
            rateProvider.Received(1).IsSupportedCurrency(Arg.Is<string>(x => x == "DKK"));
        }

        [Fact]
        public void Convert_SupportedCurrency_CallsGetExchangeRateWithCorrectArguments()
        {
            // ARRANGE
            var rateProvider = Substitute.For<IRateProvider>();
            rateProvider.IsSupportedCurrency(Arg.Any<string>()).Returns(true);
            rateProvider.GetExchangeRate(Arg.Any<string>(), Arg.Any<string>()).Returns(new ExchangeRate 
            { MainCurrency = "EUR", MoneyCurrency = "DKK", Rate = 743.94m });
            var service = new ExchangeService(rateProvider, new ValidatorFactory());

            // ACT
            var result = service.Convert(new ConvertCommand { MainCurrency = "EUR", MoneyCurrency = "DKK", Amount = 1m });

            // ASSSERT
            result.Should().NotBeNull();
            rateProvider.Received(1).GetExchangeRate(Arg.Is<string>(x => x == "EUR"), Arg.Is<string>(x => x == "DKK"));
        }
    }
}
