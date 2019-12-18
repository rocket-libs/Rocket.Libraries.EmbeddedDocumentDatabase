using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase.ValueParsing
{
    internal class ValueParser
    {
        public static TTargetType Parse<TTargetType>(object value)
        {
            var returnValue = Parse(value, typeof(TTargetType));
            if (returnValue == default)
            {
                return default;
            }
            else
            {
                return (TTargetType)returnValue;
            }
        }

        public static object Parse(object value, Type targetType)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return default;
                }

                var parsers = new Dictionary<Type, Func<string, object>>
                {
                    { typeof(long), (v) => long.Parse(v, CultureInfo.InvariantCulture) },
                    { typeof(int), (v) => int.Parse(v, CultureInfo.InvariantCulture) },
                    { typeof(string), (v) => v },
                    { typeof(DateTime), (v) =>  DateParser(v) },
                    { typeof(DateTime?), (v) =>  NullableDateParser(v) },
                    { typeof(bool), (v) => BoolParser(v) },
                    { typeof(short), (v) => short.Parse(v,CultureInfo.InvariantCulture) },
                    { typeof(byte), (v) => byte.Parse(v,CultureInfo.InvariantCulture) },
                    { typeof(double), (v) => double.Parse(v,CultureInfo.InvariantCulture) },
                    { typeof(float), (v) => float.Parse(v,CultureInfo.InvariantCulture) },
                    { typeof(long?), (v) => NullableLongParser(v) },
                };

                Func<string, object> targetParser;
                parsers.TryGetValue(targetType, out targetParser);

                if (targetParser == default)
                {
                    targetParser = parsers[typeof(string)];
                }

                return targetParser(value.ToString());
            }
            catch (Exception e)
            {
                throw new ValueParsingFailedException($"Could not parse value to its actual data type.", e);
            }
        }

        private static DateTime? NullableDateParser(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                return DateParser(value);
            }
        }

        private static DateTime DateParser(string value)
        {
            try
            {
                return DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        private static bool BoolParser(string value)
        {
            var trueStrings = new List<string> { "yes", "true", "1" };
            var trueState = trueStrings.Any(v => v.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            if (trueState)
            {
                return true;
            }
            else
            {
                _ = bool.TryParse(value, out trueState);
                return trueState;
            }
        }

        private static long? NullableLongParser(string value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return long.Parse(value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            }
        }
    }
}