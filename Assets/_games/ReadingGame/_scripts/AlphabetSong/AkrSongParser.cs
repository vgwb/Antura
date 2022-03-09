using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Antura.Minigames.ReadingGame
{
    // Antura KaRaoke Format
    public class AkrSongParser : ISongParser
    {
        enum TokenType
        {
            Invalid,
            Time,
            Text,
            BreakLine
        }

        private const string PAUSE_TOLEN = "PAUSE";
        const string BREAK_TOKEN = "BREAK";
        const string DELIMITATOR1_TOKEN = "[";
        const string DELIMITATOR2_TOKEN = "]";

        public List<KaraokeSegment> Parse(Stream stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from stream");

            var words = new List<KaraokeSegment>();

            KaraokeSegment currentItem = null;
            float lastParsedTime = -1;
            bool mustBreakLine = false;

            using (var reader = new StreamReader(stream, true))
            {
                string nextLine;
                while ((nextLine = reader.ReadLine()) != null)
                {
                    int currPosition = 0;

                    while (currPosition < nextLine.Length)
                    {
                        TokenType type;
                        object token;

                        nextLine = nextLine.Substring(currPosition);
                        currPosition = NextToken(nextLine, out type, out token);

                        if (type == TokenType.Text)
                        {
                            if (currentItem != null) // multi-line
                            {
                                if (string.IsNullOrEmpty(currentItem.text.Trim()))
                                    currentItem.text = (string)token;
                                else
                                    currentItem.text = currentItem.text + " " + (string)token;
                            }
                            else
                            {
                                if (lastParsedTime < 0)
                                    throw new FormatException("Unable to parse: start time not specified");

                                currentItem = new KaraokeSegment();
                                currentItem.start = lastParsedTime;
                                currentItem.text = (string)token;

                                if (mustBreakLine)
                                {
                                    currentItem.starsWithLineBreak = true;
                                    mustBreakLine = false;
                                }
                            }
                        }
                        else if (type == TokenType.BreakLine)
                        {
                            mustBreakLine = true;
                        }
                        else if (type == TokenType.Time)
                        {
                            lastParsedTime = (float)token;

                            if (currentItem != null) // completed
                            {
                                if (string.IsNullOrEmpty(currentItem.text.Trim()))
                                {
                                    // skip
                                    if (currentItem.starsWithLineBreak)
                                        mustBreakLine = true;
                                }
                                else
                                {
                                    currentItem.end = lastParsedTime;
                                    words.Add(currentItem);
                                }
                                currentItem = null;
                            }
                        }
                    }
                }
            }

            return words;
        }

        int NextToken(string line, out TokenType type, out object token)
        {
            int firstDelimitator = line.IndexOf(DELIMITATOR1_TOKEN);

            if (firstDelimitator > 0)
            {
                type = TokenType.Text;
                token = line.Substring(0, firstDelimitator);
                return firstDelimitator;
            }
            else if (firstDelimitator < 0)
            {
                type = TokenType.Text;
                token = line;
                return line.Length;
            }
            else
            {
                // skip first delimitator
                line = line.Substring(1);

                // break or time
                int secondDelimitator = line.IndexOf(DELIMITATOR2_TOKEN);

                if (secondDelimitator <= 0)
                {
                    throw new FormatException("Unable to parse AKR format.");
                }
                else
                {
                    var controlToken = line.Substring(0, secondDelimitator).Trim().ToUpperInvariant();

                    if (controlToken == BREAK_TOKEN)
                    {
                        type = TokenType.BreakLine;
                        token = null;
                    }
                    else
                    {
                        float time;
                        if (ParseAkrTimeCode(controlToken, out time))
                        {
                            type = TokenType.Time;
                            token = time;
                        }
                        else
                            throw new FormatException("Unable to parse AKR format: " + controlToken);
                    }
                }
                return secondDelimitator + 2;
            }
        }

        private bool ParseAkrTimeCode(string s, out float time)
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
                    time = (float)result.TotalSeconds;
                    return true;
                }
            }

            time = 0;
            return false;
        }
    }
}
