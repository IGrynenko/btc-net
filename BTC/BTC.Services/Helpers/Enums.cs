using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.Services.Helpers
{
    public enum Currency
    {
        BTC,
        USD,
        UAH
    }

    public static class Dictionaries
    {
        public static Dictionary<Currency, string> Currencies = new Dictionary<Currency, string>
        {
            { Currency.BTC, "BTC" },
            { Currency.USD, "USD" },
            { Currency.UAH, "UAH" }
        };
    }
}
