
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Web;

namespace Extensions.Common.STExtension
{
    /// <summary>
    /// System Extenstion
    /// </summary>
    public static class STExtension
    {


        #region Convert to int 
        /// <summary>
        /// <br>Ex. Convert string to int</br>
        /// <br>string stringNumber = "1";</br>
        /// <br>int number = stringNumber.toInt();</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Return int only. If convert value error return 0.</returns>
        public static int toInt(this string instance)
        {
            int value = instance.toIntOrNull() ?? 0;
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to int or null</br>
        /// <br>string stringNumber = "1";</br>
        /// <br>int? numberOrNull = stringNumber.toIntOrNull();</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Return int or null.</returns>
        public static int? toIntOrNull(this string instance)
        {
            int? value = null;
            if (!string.IsNullOrEmpty(instance))
            {
                instance = replaceExponential(instance);
                int temp;
                bool isInt = int.TryParse(instance, out temp);
                if (isInt)
                {
                    value = temp;
                }
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert int? to int</br>
        /// <br>int? numberOrNull = stringNumber.toIntOrNull();</br>
        /// <br> int numberToNumber = numberOrNull.toInt();</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Return int or 0.</returns>
        public static int toInt(this int? instance)
        {
            int value = instance.HasValue ? instance.Value : 0;
            return value;
        }
        #endregion

        #region Convert to DateTime
        /// <summary>
        /// String From Date
        /// </summary>
        public static string ToStringFromDate(this DateTime? instance, string sFormat = "dd/MM/yyyy", string sCulture = "en-US")
        {
            return instance != null ? instance.Value.ToStringFromDate(sFormat, sCulture) : "";
        }
        /// <summary>
        /// String From Date
        /// </summary>
        public static string ToStringFromDate(this DateTime instance, string sFormat = "dd/MM/yyyy", string sCulture = "en-US")
        {
            sCulture = string.IsNullOrEmpty(sCulture) ? "en-US" : sCulture;
            return instance.ToString(sFormat, new CultureInfo(sCulture));
        }
        /// <summary>
        /// Date From String
        /// </summary>
        public static DateTime? ToDateFromString(this string instance, string sFormat = "dd/MM/yyyy", string sCulture = "en-US")
        {
            try
            {
                string strTime = "";
                DateTime? dResult = null;
                DateTime dTemp;

                bool checkDate = DateTime.TryParseExact(instance + " " + ((strTime) != "" ? strTime : "00.00"), "yyyy-MM-dd HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
                if (!checkDate)
                {
                    if (strTime.Length < 5)
                    {
                        dResult = DateTime.TryParseExact(instance + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "yyyy-MM-dd HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                    }
                }
                else
                {
                    dResult = DateTime.TryParseExact(instance + " " + ((strTime) != "" ? strTime : "00.00"), "yyyy-MM-dd HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                }

                if (!dResult.HasValue && !string.IsNullOrEmpty(instance))
                {
                    string[] sDateTemp = instance.Split('/');
                    string sDate_Index = sDateTemp[0];
                    if (sDate_Index.Count() == 1)
                    {
                        sDate_Index = "0" + sDate_Index;
                    }
                    string sMonth_Index = sDateTemp[1];
                    if (sMonth_Index.Count() == 1)
                    {
                        sMonth_Index = "0" + sMonth_Index;
                    }
                    instance = sDateTemp[2].Substring(0, 4).Trim() + "-" + sMonth_Index + "-" + sDate_Index;
                    checkDate = DateTime.TryParseExact(instance, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
                    if (!checkDate)
                    {
                        if (strTime.Length < 5)
                        {
                            dResult = DateTime.TryParseExact(instance, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                        }
                    }
                    else
                    {
                        dResult = DateTime.TryParseExact(instance, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                    }
                }

                return dResult;
            }
            catch { return null; }

        }
        /// <summary>
        /// Change Time
        /// </summary>
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }
        /// <summary>
        /// DateTime From String
        /// </summary>
        public static DateTime? ToDateTimeFromString(this string instance, string sFormat = "dd/MM/yyyy", string sCulture = "en-US")
        {
            try
            {
                DateTime? dResult = null;
                DateTime dTemp;

                bool checkDate = DateTime.TryParseExact(instance, sFormat, new CultureInfo(sCulture), DateTimeStyles.None, out dTemp);
                if (!dResult.HasValue && !string.IsNullOrEmpty(instance))
                {
                    dResult = dTemp;
                }
                return dResult;
            }
            catch { return null; }

        }
        #endregion

        #region Convert to Datetime By UnixTime
        /// <summary>
        /// <br>Ex. Convert double to datetime</br>
        /// <br>double numberDate = "1661851724";</br>
        /// <br>DateTime? date = numberDate.toDateTime();</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Return datetime or null.</returns> 
        public static DateTime toDateTime(this double instance)
        {
            DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            result = result.AddMilliseconds(instance).ToLocalTime();
            return result;
        }
        /// <summary>
        /// <br>Ex. Convert datetime to double</br>
        /// <br>DateTime datetime = DateTime.Now;</br>
        /// <br>double number = datetime.ToUnixTime();</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>Return datetime or null.</returns> 
        public static double toUnixTime(this DateTime instance)
        {
            double result;
            DateTimeOffset dateValue = new DateTimeOffset(instance.ToUniversalTime());
            result = dateValue.ToUnixTimeMilliseconds();
            return result;
        }
        #endregion

        #region Convert to double 
        /// <summary>
        /// <br>Ex. Convert string to double</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double number = stringNumber.toDouble();</br>
        /// <br>double number = stringNumber.toDouble(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double only normal case less more than 5 round down and more than or equal 5 round up.
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double toDouble(this string instance, int? digit = null)
        {
            double value = instance.toDoubleOrNull(digit) ?? 0;
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to double round up</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double number = stringNumber.toDoubleRoundUp();</br>
        /// <br>double number = stringNumber.toDoubleRoundUp(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double only and round up.
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.26</br>
        /// </returns> 
        public static double toDoubleRoundUp(this string instance, int? digit = null)
        {
            double value = instance.toDoubleRoundUpOrNull(digit) ?? 0;
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to double round down</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double number = stringNumber.toDoubleRoundDown();</br>
        /// <br>double number = stringNumber.toDoubleRoundDown(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double only and round down.
        /// <br>Ex. value = 1.255 return 1.25</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double toDoubleRoundDown(this string instance, int? digit = null)
        {
            double value = instance.toDoubleRoundDownOrNull(digit) ?? 0;
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to double or null</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double? number = stringNumber.toDoubleOrNull();</br>
        /// <br>double? number = stringNumber.toDoubleOrNull(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double or null.
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double? toDoubleOrNull(this string instance, int? digit = null)
        {
            double? value = null;
            if (!string.IsNullOrEmpty(instance))
            {
                double temp;
                string stringValue = replaceExponential(instance);
                bool isDouble = double.TryParse(stringValue, out temp);
                if (isDouble)
                {
                    if (digit.HasValue)
                    {
                        value = Math.Round(temp, digit.Value);
                    }
                    else
                    {
                        value = temp;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to double round up or null</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double? number = stringNumber.toDoubleRoundUpOrNull();</br>
        /// <br>double? number = stringNumber.toDoubleRoundUpOrNull(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double or null.
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.26</br>
        /// </returns> 
        public static double? toDoubleRoundUpOrNull(this string instance, int? digit = null)
        {
            double? value = null;
            if (!string.IsNullOrEmpty(instance))
            {
                double temp;
                string stringValue = replaceExponential(instance);
                bool isDouble = double.TryParse(stringValue, out temp);
                if (isDouble)
                {
                    if (digit.HasValue)
                    {
                        double mathPow = (double)Math.Pow(10, digit.Value);
                        value = (Math.Ceiling(temp * mathPow) / mathPow);
                    }
                    else
                    {
                        value = temp;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert string to double round down or null</br>
        /// <br>string stringNumber = "1.256";</br>
        /// <br>double? number = stringNumber.toDoubleRoundDownOrNull();</br>
        /// <br>double? number = stringNumber.toDoubleRoundDownOrNull(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double or null.
        /// <br>Ex. value = 1.255 return 1.25</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double? toDoubleRoundDownOrNull(this string instance, int? digit = null)
        {
            double? value = null;
            if (!string.IsNullOrEmpty(instance))
            {
                double temp;
                string stringValue = replaceExponential(instance);
                bool isDouble = double.TryParse(stringValue, out temp);
                if (isDouble)
                {
                    if (digit.HasValue)
                    {
                        value = Math.Round(temp, digit.Value, MidpointRounding.ToZero);
                    }
                    else
                    {
                        value = temp;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert double to double</br>
        /// <br>double numberOrNull = 10.255;</br>
        /// <br>double numberToNumber = numberOrNull.ToDouble();</br>
        /// <br>double numberToNumber = numberOrNull.ToDouble(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double only normal case less more than 5 round down and more than or equal 5 round up.
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double toDouble(this double? instance, int? digit = null)
        {
            double value;
            if (digit.HasValue)
            {
                value = instance.HasValue ? Math.Round(instance.Value, digit.Value) : 0;
            }
            else
            {
                value = instance.HasValue ? instance.Value : 0;
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert double to double round up</br>
        /// <br>double numberOrNull = 10.255;</br>
        /// <br>double numberToNumber = numberOrNull.toDoubleRoundUp();</br>
        /// <br>double numberToNumber = numberOrNull.toDoubleRoundUp(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double round up only .
        /// <br>Ex. value = 1.255 return 1.26</br>
        /// <br>Ex. value = 1.254 return 1.26</br>
        /// </returns> 
        public static double toDoubleRoundUp(this double? instance, int? digit = null)
        {
            double value;
            if (digit.HasValue)
            {
                double mathPow = (double)Math.Pow(10, digit.Value);
                value = instance.HasValue ? (Math.Ceiling(instance.Value * mathPow) / mathPow) : 0;
            }
            else
            {
                value = instance.HasValue ? instance.Value : 0;
            }
            return value;
        }
        /// <summary>
        /// <br>Ex. Convert double to double round down</br>
        /// <br>double numberOrNull = 10.255;</br>
        /// <br>double numberToNumber = numberOrNull.toDoubleRoundDown();</br>
        /// <br>double numberToNumber = numberOrNull.toDoubleRoundDown(2);</br>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="digit"></param>
        /// <returns>Return double round down only.
        /// <br>Ex. value = 1.255 return 1.25</br>
        /// <br>Ex. value = 1.254 return 1.25</br>
        /// </returns> 
        public static double toDoubleRoundDown(this double? instance, int? digit = null)
        {
            double value;
            if (digit.HasValue)
            {
                value = instance.HasValue ? Math.Round(instance.Value, digit.Value, MidpointRounding.ToZero) : 0;
            }
            else
            {
                value = instance.HasValue ? instance.Value : 0;
            }
            return value;
        }
        #endregion

        #region Numberic
        public static string ToStringFromNumber(this int? instance)
        {
            return instance != null ? instance.Value.ToString("n0") : "";
        }

        public static string ToStringFromNumber(this decimal? instance, int nDigit)
        {
            return instance != null ? instance.Value.ToString("n" + nDigit) : "";
        }

        public static string ToStringFromNumber(this decimal? instance, int nDigit, bool IsPercent = false)
        {
            return instance != null ? instance.Value.ToString(("n" + nDigit) + (IsPercent ? "%" : "")) : "";
        }

        public static string ToStringFromNumber(this double? instance, int nDigit, bool IsPercent = false)
        {
            return instance != null ? instance.Value.ToString(("n" + nDigit) + (IsPercent ? "%" : "")) : "";
        }
        #endregion

        #region String
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetNameFromEmail(this object instance)
        {
            return instance != null ? (instance + "").Split('@')[0] + "" : "";
        }

        /// <summary>
        /// Extension
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="nStartIndex"></param>
        /// <param name="nLength"></param>
        /// <returns></returns>
        public static string SubStr(this string instance, int nStartIndex, int nLength)
        {
            if (!string.IsNullOrEmpty(instance))
            {
                if (instance.Length < (nStartIndex + nLength))
                {
                    return instance.Substring(nStartIndex, instance.Length);
                }
                else
                {
                    return instance.Substring(nStartIndex, nLength);
                }
            }
            else
                return "";
        }
        /// <summary>
        /// 
        /// </summary>
        public static string SubStrMax(this string instance, int nMaxLength)
        {
            return instance.SubStr(0, nMaxLength);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string TrimAll(this string instance)
        {
            return (instance + "").Trim();
        }
        #endregion

        #region Sorting
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");

            return matchedProperty;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        #endregion

        #region Sum
        /// <summary>
        /// Sum Decimal
        /// </summary>
        public static decimal? SumOrDefault(this List<decimal?> instance)
        {
            if (instance.Any(x => x.HasValue))
                return instance.Sum();
            else
                return null;
        }
        /// <summary>
        /// Sum Double
        /// </summary>
        public static double? SumOrDefault(this List<double?> instance)
        {
            if (instance.Any(x => x.HasValue))
                return instance.Sum();
            else
                return null;
        }
        /// <summary>
        /// Sum int
        /// </summary>
        public static int? SumOrDefault(this List<int?> instance)
        {
            if (instance.Any(x => x.HasValue))
                return instance.Sum();
            else
                return null;
        }
        #endregion

        /// <summary>
        /// Has Item
        /// </summary>
        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return (source?.Any() ?? false);
        }

        /// <summary>
        /// Replace Exponential
        /// </summary>
        public static string replaceExponential(this string sVal)
        {
            string sRsult = "";
            try
            {
                decimal nTemp = 0;
                bool check = decimal.TryParse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float, null, out nTemp);
                if (check)
                {
                    decimal d = decimal.Parse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float);
                    sRsult = (d + "").Replace(",", "");
                }
                else
                {
                    sRsult = sVal;
                }
            }
            catch
            {
                sRsult = sVal;
            }

            return sRsult;
        }
        /// <summary>
        /// Is Number
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNumber(this string instance)
        {
            foreach (char ch in instance)
            {
                if (!char.IsNumber(ch)) return false;
            }
            return true;
        }
        /// <summary>
        /// Is Digit
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsDigit(this string instance)
        {
            foreach (char ch in instance)
            {
                var checkT = ch;
                if (!instance.Any(char.IsDigit)) return false;
            }
            return true;
        }

    }
}