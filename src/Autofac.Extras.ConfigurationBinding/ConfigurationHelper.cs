using System;
using System.Configuration;
using System.Globalization;

namespace Autofac.Extras.ConfigurationBinding
{
    internal class ConfigurationHelper
    {
        public static T ExtractConfig<T>(string key)
        {
            var configValue = ConfigurationManager.AppSettings[key];
            var type = typeof(T);
            object resultValue = null;
            if (type == typeof(int))
            {
                resultValue = int.TryParse(configValue, out var intValue) ? intValue : default(int);
            }

            if (type == typeof(long))
            {
                resultValue = long.TryParse(configValue, out var longValue) ? longValue : default(long);
            }

            if (type == typeof(decimal))
            {
                resultValue = decimal.TryParse(configValue, out var decimalValue) ? decimalValue : default(decimal);
            }

            if (type == typeof(bool))
            {
                resultValue = bool.TryParse(configValue, out var boolValue) ? boolValue : default(bool);
            }

            if (type == typeof(int?))
            {
                if (int.TryParse(configValue, out var intValue))
                {
                    resultValue = intValue;
                }
                else
                {
                    resultValue = null;
                }
            }

            if (type == typeof(long?))
            {
                if (long.TryParse(configValue, out var longValue))
                {
                    resultValue = longValue;
                }
                else
                {
                    resultValue = null;
                }
            }

            if (type == typeof(decimal?))
            {
                if (decimal.TryParse(configValue, out var decimalValue))
                {
                    resultValue = decimalValue;
                }
                else
                {
                    resultValue = null;
                }
            }

            if (type == typeof(bool?))
            {
                if (bool.TryParse(configValue, out var boolValue))
                {
                    resultValue = boolValue;
                }
                else
                {
                    resultValue = null;
                }
            }

            if (type == typeof(string))
            {
                resultValue = configValue;
            }

            return (T) resultValue;
        }
    }
}