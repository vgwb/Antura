using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Antura.Minigames.ReadingGame
{
    public class VttSongParser : ISongParser
    {
        private readonly string[] arrows = new string[] { "-->", "->", "- >" };

        public List<KaraokeSegment> Parse(Stream stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from stream");

            var words = new List<KaraokeSegment>();

            using (var reader = new StreamReader(stream, true))
            {
                var parts = GetParts(reader);
                if (parts.Count > 0)
                {
                    foreach (var p in parts)
                    {
                        var lines = p.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        var item = new KaraokeSegment();
                        bool parsedTime = false;
                        for (int i = 0; i < lines.Length; ++i)
                        {
                            var line = lines[i];
                            line = line.Trim();
                            if (string.IsNullOrEmpty(line))
                                continue;

                            if (!parsedTime)
                            {
                                float startTime;
                                float endTime;

                                if (TryParseTime(line, out startTime, out endTime))
                                {
                                    parsedTime = true;
                                    item.start = startTime;
                                    item.end = endTime;
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(item.text))
                                    item.text = line;
                                else
                                    item.text = item.text + " " + line;
                            }
                        }

                        if (parsedTime && item.text != null)
                            words.Add(item);
                    }

                    if (words.Count > 0)
                    {
                        return words;
                    }
                    else
                    {
                        throw new ArgumentException("The song is empty");
                    }
                }
                else
                {
                    throw new FormatException("Unable to parse VTT format.");
                }
            }
        }

        List<string> GetParts(TextReader reader)
        {
            List<string> parts = new List<string>();

            var stringBuilder = new StringBuilder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line.Trim()))
                {
                    var trimmedLine = stringBuilder.ToString().TrimEnd();
                    if (!string.IsNullOrEmpty(trimmedLine))
                    {
                        parts.Add(trimmedLine);
                    }
                    stringBuilder = new StringBuilder();
                }
                else
                {
                    stringBuilder.AppendLine(line);
                }
            }

            if (stringBuilder.Length > 0)
            {
                parts.Add(stringBuilder.ToString());
            }

            return parts;
        }

        private bool TryParseTime(string line, out float startTime, out float endTime)
        {
            var parts = line.Split(arrows, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                // can't parse
                startTime = -1;
                endTime = -1;
                return false;
            }
            else
            {
                startTime = ParseVttTimeCode(parts[0]);
                endTime = ParseVttTimeCode(parts[1]);
                return true;
            }
        }

        private float ParseVttTimeCode(string s)
        {
            string timeString = string.Empty;
            var match = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+[,\\.][0-9]+");
            if (match.Success)
            {
                timeString = match.Value;
            }
            else
            {
                match = Regex.Match(s, "[0-9]+:[0-9]+[,\\.][0-9]+");
                if (match.Success)
                {
                    timeString = "00:" + match.Value;
                }
            }

            if (!string.IsNullOrEmpty(timeString))
            {
                timeString = timeString.Replace(',', '.');
                TimeSpan result;
                if (TimeSpan.TryParse(timeString, out result))
                {
                    return (float)result.TotalSeconds;
                }
            }

            return -1;
        }
    }
}
