using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Antura.Helpers
{
    /// <summary>
    /// Static helper class for generic utility functions.
    /// </summary>
    public static class GenericHelper
    {
        #region Enumerators

        /// <summary>
        /// sort an Enum by its names.. returns List;
        /// to be used like: var orderedEnumList = Sort<Sfx>();
        /// </summary>
        public static IOrderedEnumerable<TEnum> SortEnums<TEnum>()
        {
            return ((TEnum[])Enum.GetValues(typeof(TEnum))).OrderBy(v => v.ToString());
        }

        #endregion

        #region Text

        public static string ReverseText(string _source)
        {
            if (_source.Contains('\n'))
            {
                return ReverseMultiParagraphText(_source);
            }
            else
            {
                return ReverseSingleParagraphText(_source);
            }
        }

        /// <summary>
        /// Reverses the text of a single paragraph
        /// </summary>
        /// <returns>The text reversed.</returns>
        /// <param name="_source">Source.</param>
        private static string ReverseSingleParagraphText(string _source)
        {
            var cArray = _source.ToCharArray();
            var reverse = String.Empty;
            for (var i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }

        /// <summary>
        /// Reverses the text but keeps the paragraphs order.
        /// </summary>
        /// <returns>The text with all paragraphs reversed.</returns>
        /// <param name="_source">Source.</param>
        private static string ReverseMultiParagraphText(string _source)
        {
            char[] split = { '\n' };
            string[] paragraphs = _source.Split(split);
            string result = "";
            foreach (string paragraph in paragraphs)
            {
                result += ReverseSingleParagraphText(paragraph);
                result += "\n";
            }
            return result;
        }

        public static string ToUnicodeString(this string unicodeNumbers)
        {
            var res = string.Empty;
            if (unicodeNumbers.Contains(@"\u"))
            {
                for (int i = 0; i < unicodeNumbers.Length; i += 6)
                {
                    int code = int.Parse(unicodeNumbers.Substring(i + 2, 4), NumberStyles.HexNumber);
                    var ch = char.ConvertFromUtf32(code);
                    res += ch;
                }
            }
            else
            {
                for (int i = 0; i < unicodeNumbers.Length; i += 4)
                {
                    int code = int.Parse(unicodeNumbers.Substring(i, 4), NumberStyles.HexNumber);
                    var ch = char.ConvertFromUtf32(code);
                    res += ch;
                }
            }
            return res;
        }

        #endregion

        #region DateTime

        private static DateTime TIME_START = new DateTime(1970, 1, 1, 0, 0, 0);

        public static int GetRelativeTimestampFromNow(int deltaDays)
        {
            var timeSpan = new TimeSpan(deltaDays, 0, 0, 0, 0);
            return GetTimestampForNow() + (int)timeSpan.TotalSeconds;
        }

        public static int GetTimestampForNow()
        {
            var timeSpan = (DateTime.UtcNow - TIME_START);
            return (int)timeSpan.TotalSeconds;
        }

        public static DateTime FromTimestamp(int timestamp)
        {
            var span = TimeSpan.FromSeconds(timestamp);
            return TIME_START + span;
        }

        public static TimeSpan GetTimeSpanBetween(int timestamp_from, int timestamp_to)
        {
            return FromTimestamp(timestamp_to) - FromTimestamp(timestamp_from);
        }

        /// <summary>
        /// Convert a date time object to Unix time representation.
        /// </summary>
        /// <param name="datetime">The datetime object to convert to Unix time stamp.</param>
        /// <returns>Returns a numerical representation (Unix time) of the DateTime object.</returns>
        public static int GetUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (int)(datetime - sTime).TotalSeconds;
        }

        #endregion

        #region Layers

        public static int LayerMaskToIndex(LayerMask _mask)
        {
            int layerIndex = 0;
            int layer = _mask.value;
            while (layer > 1)
            {
                layer = layer >> 1;
                layerIndex++;
            }
            return layerIndex;
        }

        #endregion

        #region Colors

        // Taken from here: http://answers.unity3d.com/questions/812240/convert-hex-int-to-colorcolor32.html
        public static Color HexToColor(string _hex)
        {
            _hex = _hex.Replace("0x", "");
            _hex = _hex.Replace("#", "");
            byte a = 255;
            byte r = Byte.Parse(_hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = Byte.Parse(_hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = Byte.Parse(_hex.Substring(4, 2), NumberStyles.HexNumber);
            // Only use alpha if the string has enough characters
            if (_hex.Length == 8)
                a = Byte.Parse(_hex.Substring(4, 2), NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        // Taken from here: http://wiki.unity3d.com/index.php?title=HexConverter
        public static string ColorToHex(Color32 _color, bool _addHashPrefix = false)
        {
            string hex = _color.r.ToString("X2") + _color.g.ToString("X2") + _color.b.ToString("X2");
            return _addHashPrefix ? "#" + hex : hex;
        }

        public static Color GetColorFromString(string color)
        {
            Color drawingColor;
            switch (color)
            {
                case "blue":
                    drawingColor = new Color(17f / 255f, 85f / 255f, 204f / 255f);
                    break;
                case "brown":
                    drawingColor = new Color(100f / 255f, 62f / 255f, 49f / 255f);
                    break;
                case "gold":
                    drawingColor = new Color(255f / 255f, 215f / 255f, 0);
                    break;
                case "green":
                    drawingColor = new Color(30f / 255f, 177f / 255f, 0f / 203f);
                    break;
                case "grey":
                    drawingColor = Color.grey;
                    break;
                case "orange":
                    drawingColor = new Color(255f / 255f, 148f / 255f, 0);
                    break;
                case "pink":
                    drawingColor = new Color(255f / 255f, 192f / 255f, 128f / 203f);
                    break;
                case "purple":
                    drawingColor = new Color(103f / 255f, 78f / 255f, 167f / 255f);
                    break;
                case "red":
                    drawingColor = new Color(237f / 255f, 35f / 255f, 13f / 255f);
                    break;
                case "silver":
                    drawingColor = new Color(128f / 255f, 128f / 255f, 128f / 255f);
                    break;
                case "white":
                    drawingColor = Color.white;
                    break;
                case "yellow":
                    drawingColor = new Color(249f / 255f, 226f / 255f, 49f / 255f);
                    break;
                default:
                    drawingColor = Color.black;
                    break;
            }
            return drawingColor;
        }

        #endregion


        #region Debug Extensions

        public static string ToJoinedString<T>(this IEnumerable<T> list)
        {
            return string.Join(",", list.ToList().ConvertAll(x => x.ToString()).ToArray());
        }

        public static string ToDebugString<T>(this IEnumerable<T> list)
        {
            return "{" + string.Join(",", list.ToList().ConvertAll(x => x == null ? "NONE" : x.ToString()).ToArray()) +
                   "}";
        }

        public static string ToDebugStringNewline<T>(this IEnumerable<T> list)
        {
            return "{" + string.Join(",", list.ToList().ConvertAll(x => (x == null ? "NONE" : x.ToString()) + "\n").ToArray()) +
                   "}";
        }

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        #endregion
    }
}
