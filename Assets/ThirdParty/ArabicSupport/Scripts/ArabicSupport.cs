#region File Description
//-----------------------------------------------------------------------------
/// <summary>
/// This is an Open Source File Created by: Abdullah Konash (http://abdullahkonash.com/) Twitter: @konash
/// This File allow the users to use arabic text in XNA and Unity platform.
/// It flips the characters and replace them with the appropriate ones to connect the letters in the correct way.
/// </summary>
//-----------------------------------------------------------------------------

// NDMichele: 20/07/2022: edited this to use our tables, instead of hardcoded values. Also fixed various bugs.
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using DG.DeExtensions;
using UnityEngine;
using static Antura.Language.ArabicTable;

#endregion

namespace Antura.Language
{

	public class ArabicFixer
    {
        public static bool ConvertFarsiYehToAlefMaqsura;

        public static void AddToMappingTable(List<ArabicMapping> mappings)
        {
            var mapper = ArabicMapper;
            mapper.AddToTable(mappings);
        }

		/// <summary>
		/// Fix the specified string.
		/// </summary>
		/// <param name='str'>
		/// String to be fixed.
		/// </param>
		public static string Fix(string str)
		{
			return Fix(str, false, true);
		}

		public static string Fix(string str, bool rtl)
		{
			if(rtl)
			{
				return Fix(str);
			}
			else
			{
				string[] words = str.Split(' ');
				string result = "";
				string arabicToIgnore = "";
				foreach(string word in words)
				{
					if(char.IsLower(word.ToLower()[word.Length/2]))
					{
						result += Fix(arabicToIgnore) + word + " ";
						arabicToIgnore = "";
					}
					else
					{
						arabicToIgnore += word + " ";

					}
				}
				if(arabicToIgnore != "")
					result += Fix(arabicToIgnore);

				return result;
			}
		}

		/// <summary>
		/// Fix the specified string with customization options.
		/// </summary>
		/// <param name='str'>
		/// String to be fixed.
		/// </param>
		/// <param name='showTashkeel'>
		/// Show tashkeel.
		/// </param>
		/// <param name='useHinduNumbers'>
		/// Use hindu numbers.
		/// </param>
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			ArabicFixerTool.showTashkeel = showTashkeel;
			ArabicFixerTool.useHinduNumbers =useHinduNumbers;

            if (str.IsNullOrEmpty()) return "";

			if(str.Contains("\n"))
				str = str.Replace("\n", System.Environment.NewLine);

			if(str.Contains(System.Environment.NewLine))
			{
				string[] stringSeparators = new string[] {System.Environment.NewLine};
				string[] strSplit = str.Split(stringSeparators, StringSplitOptions.None);

				if(strSplit.Length == 0)
					return ArabicFixerTool.FixLine(str);
				else if(strSplit.Length == 1)
					return ArabicFixerTool.FixLine(str);
				else
				{
					string outputString = ArabicFixerTool.FixLine(strSplit[0]);
					int iteration = 1;
					if(strSplit.Length > 1)
					{
						while(iteration < strSplit.Length)
						{
							outputString += System.Environment.NewLine + ArabicFixerTool.FixLine(strSplit[iteration]);
							iteration++;
						}
					}
					return outputString;
				}
			}
			else
			{
				return ArabicFixerTool.FixLine(str);
			}

		}

	}
    /// <summary>
    /// Arabic Contextual forms - Isolated
    /// </summary>
    internal enum CharUnicode
    {
        // Lam & Alef variations, used to generated Lam Alef variations
        Alef = 0x0627,
        AlefHamzaHi = 0x0623,
        AlefHamzaLow = 0x0625,
        Lam = 0x0644,
        AlefMadda = 0x0622,

        // Persian Yeh, which is the same character as Arabic Alef Maqsura
        AlefMaqsura = 0x0649,
        PersianYeh = 0x06CC,

        // Diacritics
        TanwinFathah = 0x064B,
        TanwinDammah = 0x064C,
        TanwinKasrah = 0x064D,
        Fathah = 0x064E,
        Dammah = 0x064F,
        Kasrah = 0x0650,
        Shaddah = 0x0651,
        Sukun = 0x0652,
        Maddah = 0x0653,
    }

    /// <summary>
    /// Data Structure for conversion
    /// </summary>
    public class ArabicMapping
    {
	    public int from;

        public int isolated;
        public int initial;
        public int medial;
        public int final;

        public bool canConnectBefore;
        public bool canConnectAfter;

        public ArabicMapping(int from, int isolated, int initial, int medial, int final, bool canConnectBefore, bool canConnectAfter)
        {
            this.from = from;
            this.isolated = isolated;
            this.final = final > 0 ? final : this.isolated;
            this.initial = initial > 0 ? initial : this.isolated;
            this.medial = medial > 0 ? medial : this.final;
            this.canConnectBefore = canConnectBefore;
            this.canConnectAfter = canConnectAfter;
        }

        public bool InitialDefined => initial != isolated;
        public bool MedialDefined => medial != isolated && medial != final;
        public bool FinalDefined => final != isolated;

        public string IsolatedHex => Convert.ToString(isolated, 16);
        public string MedialHex => Convert.ToString(medial, 16);
        public string FinalHex => Convert.ToString(final, 16);
        public string InitialHex => Convert.ToString(initial, 16);

    }

    /// <summary>
    /// Sets up and creates the conversion table
    /// </summary>
    internal class ArabicTable
    {

	    private static List<ArabicMapping> mapList;
	    private static ArabicTable arabicMapper;

        public void AddToTable(List<ArabicMapping> mappings)
        {
            if (mapList == null) mapList = new List<ArabicMapping>();
            mapList.AddRange(mappings);
        }

	    /// <summary>
	    /// Singleton design pattern, Get the mapper. If it was not created before, create it.
	    /// </summary>
	    internal static ArabicTable ArabicMapper
	    {
		    get
		    {
			    if (arabicMapper == null)
				    arabicMapper = new ArabicTable();
			    return arabicMapper;
		    }
	    }

	    internal int ConvertToIsolated(int generic)
        {
            var mapping = MappingFor(generic);
            if (mapping != null) return mapping.isolated;
            return generic;
        }


        internal ArabicMapping MappingFor(int generic)
        {
            for (var iMapping = 0; iMapping < mapList.Count; iMapping++)
            {
                ArabicMapping arabicMap = mapList[iMapping];
                if (arabicMap.@from == generic)
                {
                    return arabicMap;
                }
            }
            return null;
        }
    }


    internal class TashkeelLocation
    {
	    public char tashkeel;
	    public int position;
	    public TashkeelLocation(char tashkeel, int position)
	    {
		    this.tashkeel = tashkeel;
		    this.position = position;
	    }
    }


    internal class ArabicFixerTool
    {
	    internal static bool showTashkeel = true;
	    internal static bool useHinduNumbers = false;

	    internal static string RemoveTashkeel(string str, out List<TashkeelLocation> tashkeelLocation)
	    {
		    tashkeelLocation = new List<TashkeelLocation>();
		    char[] letters = str.ToCharArray();
		    for (int i = 0; i < letters.Length; i++)
		    {
			    if(letters[i] == (char)CharUnicode.TanwinFathah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.TanwinDammah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.TanwinKasrah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Fathah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Dammah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Kasrah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Shaddah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Sukun)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
			    else if(letters[i] == (char)CharUnicode.Maddah)
				    tashkeelLocation.Add(new TashkeelLocation(letters[i], i));
		    }

		    string[] split = str.Split(new char[]{(char)CharUnicode.TanwinFathah,(char)CharUnicode.TanwinDammah,(char)CharUnicode.TanwinKasrah,
			    (char)CharUnicode.Fathah,(char)CharUnicode.Dammah,(char)CharUnicode.Kasrah,
			    (char)CharUnicode.Shaddah,(char)CharUnicode.Sukun,(char)CharUnicode.Maddah,});

		    str = "";

		    foreach(string s in split)
		    {
			    str += s;
		    }

		    return str;
	    }

	    internal static char[] ReturnTashkeel(char[] letters, List<TashkeelLocation> tashkeelLocation)
	    {
		    char[] lettersWithTashkeel = new char[letters.Length + tashkeelLocation.Count];

		    int letterWithTashkeelTracker = 0;
		    for(int i = 0; i<letters.Length; i++)
		    {
			    lettersWithTashkeel[letterWithTashkeelTracker] = letters[i];
			    letterWithTashkeelTracker++;
			    foreach(TashkeelLocation hLocation in tashkeelLocation)
			    {
				    if(hLocation.position == letterWithTashkeelTracker)
				    {
					    lettersWithTashkeel[letterWithTashkeelTracker] = hLocation.tashkeel;
					    letterWithTashkeelTracker++;
				    }
			    }
		    }

		    return lettersWithTashkeel;
	    }

	    /// <summary>
	    /// Converts a string to a form in which the string will be displayed correctly for arabic text.
	    /// </summary>
	    /// <param name="str">String to be converted. Example: "Aaa"</param>
	    /// <returns>Converted string. Example: "aa aaa A" without the spaces.</returns>
	    internal static string FixLine(string str)
	    {
		    List<TashkeelLocation> tashkeelLocations;

		    string originString = RemoveTashkeel(str, out tashkeelLocations);

		    char[] lettersInput = originString.ToCharArray();
		    char[] lettersOutput = originString.ToCharArray();

		    for (int i = 0; i < lettersInput.Length; i++)
		    {
			    lettersInput[i] = (char)ArabicMapper.ConvertToIsolated(lettersInput[i]);

                if (ArabicFixer.ConvertFarsiYehToAlefMaqsura && lettersInput[i] == (char)CharUnicode.PersianYeh)
                {
                    Debug.LogError($"We found a FARSI YEH at index {i}, we transform it into an ALEF MAKSURA");
                    lettersInput[i] = (char)CharUnicode.AlefMaqsura;
                }
                else if (!ArabicFixer.ConvertFarsiYehToAlefMaqsura && lettersInput[i] == (char)CharUnicode.AlefMaqsura)
                {
                    Debug.LogError($"We found an ALEF MAKSURA at index {i}, we transform it into a FARSI YEH");
                    lettersInput[i] = (char)CharUnicode.PersianYeh;
                }
		    }

		    for (int i = 0; i < lettersInput.Length; i++)
		    {
			    bool skip = false;

			    // For special Lam Letter connections.
			    if (lettersInput[i] == (char)CharUnicode.Lam)
			    {
				    if (i < lettersInput.Length - 1)
				    {
                        if ((lettersInput[i + 1] == (char)CharUnicode.Alef))
                        {
                            lettersInput[i] = (char)0xFEFB;         // Lam Alef
                            lettersOutput[i + 1] = (char)0xFFFF;    // No character
                            skip = true;
                        }
					    else if ((lettersInput[i + 1] == (char)CharUnicode.AlefHamzaHi))
					    {
						    lettersInput[i] = (char)0xFEF7;         // Lam Alef Hamza Hi
						    lettersOutput[i + 1] = (char)0xFFFF;    // No character
						    skip = true;
					    }
                        else if ((lettersInput[i + 1] == (char)CharUnicode.AlefHamzaLow))
                        {
                            lettersInput[i] = (char)0xFEF9;         // Lam Alef Hamza Low
                            lettersOutput[i + 1] = (char)0xFFFF;    // No character
                            skip = true;
                        }
					    else if ((lettersInput[i + 1] == (char)CharUnicode.AlefMadda))
					    {
						    lettersInput[i] = (char)0xFEF5;         // Lam Alef Kashrah / Lam Alef Maddah
						    lettersOutput[i + 1] = (char)0xFFFF;    // No character
						    skip = true;
					    }
				    }

			    }

			    if (ArabicMapper.MappingFor((int)lettersInput[i]) != null)
			    {
                    var mapping = ArabicMapper.MappingFor(lettersInput[i]);

                    ArabicMapping prevMapping = null;
                    if (i > 0) prevMapping = ArabicMapper.MappingFor(lettersInput[i - 1]);
                    bool prevConnecting = prevMapping != null && mapping.canConnectBefore && prevMapping.canConnectAfter;

                    ArabicMapping nextMapping = null;
                    if (i < lettersInput.Length-1) nextMapping = ArabicMapper.MappingFor(lettersInput[i + 1]);
                    bool nextConnecting = nextMapping != null && mapping.canConnectAfter && nextMapping.canConnectBefore;

                    if (prevConnecting && nextConnecting)
                    {
                        lettersOutput[i] = (char)mapping.medial;
                    }
                    else if (!prevConnecting && nextConnecting)
                    {
                        lettersOutput[i] = (char)mapping.initial;
                    }
                    else if (prevConnecting && !nextConnecting)
                    {
                        lettersOutput[i] = (char)mapping.final;
                    }
                    else // if (!prevConnecting && !nextConnecting)
                    {
                        lettersOutput[i] = (char)mapping.isolated;
                    }
			    }
			    if (skip)
				    i++;

			    //chaning numbers to hindu
			    if(useHinduNumbers){
				    if(lettersInput[i] == (char)0x0030)
					    lettersOutput[i] = (char)0x0660;
				    else if(lettersInput[i] == (char)0x0031)
					    lettersOutput[i] = (char)0x0661;
				    else if(lettersInput[i] == (char)0x0032)
					    lettersOutput[i] = (char)0x0662;
				    else if(lettersInput[i] == (char)0x0033)
					    lettersOutput[i] = (char)0x0663;
				    else if(lettersInput[i] == (char)0x0034)
					    lettersOutput[i] = (char)0x0664;
				    else if(lettersInput[i] == (char)0x0035)
					    lettersOutput[i] = (char)0x0665;
				    else if(lettersInput[i] == (char)0x0036)
					    lettersOutput[i] = (char)0x0666;
				    else if(lettersInput[i] == (char)0x0037)
					    lettersOutput[i] = (char)0x0667;
				    else if(lettersInput[i] == (char)0x0038)
					    lettersOutput[i] = (char)0x0668;
				    else if(lettersInput[i] == (char)0x0039)
					    lettersOutput[i] = (char)0x0669;
			    }

		    }



		    //Return the Tashkeel to their places.
		    if(showTashkeel)
			    lettersOutput = ReturnTashkeel(lettersOutput, tashkeelLocations);


		    List<char> list = new List<char>();

		    List<char> numberList = new List<char>();

		    for (int i = lettersOutput.Length - 1; i >= 0; i--)
		    {
			    if (char.IsPunctuation(lettersOutput[i]) && i>0 && i < lettersOutput.Length-1 &&
			        (char.IsPunctuation(lettersOutput[i-1]) || char.IsPunctuation(lettersOutput[i+1])))
			    {
				    if (lettersOutput[i] == '(')
					    list.Add(')');
				    else if (lettersOutput[i] == ')')
					    list.Add('(');
				    else if (lettersOutput[i] == '<')
					    list.Add('>');
				    else if (lettersOutput[i] == '>')
					    list.Add('<');
				    else if (lettersOutput[i] == '[')
					    list.Add(']');
				    else if (lettersOutput[i] == ']')
					    list.Add('[');
				    else if (lettersOutput[i] != 0xFFFF)
					    list.Add(lettersOutput[i]);
			    }
			    // For cases where english words and arabic are mixed. This allows for using arabic, english and numbers in one sentence.
			    else if(lettersOutput[i] == ' ' && i > 0 && i < lettersOutput.Length-1 &&
			            (char.IsLower(lettersOutput[i-1]) || char.IsUpper(lettersOutput[i-1]) || char.IsNumber(lettersOutput[i-1])) &&
			            (char.IsLower(lettersOutput[i+1]) || char.IsUpper(lettersOutput[i+1]) ||char.IsNumber(lettersOutput[i+1])))

			    {
				    numberList.Add(lettersOutput[i]);
			    }

			    else if (char.IsNumber(lettersOutput[i]) || char.IsLower(lettersOutput[i]) ||
			             char.IsUpper(lettersOutput[i]) || char.IsSymbol(lettersOutput[i]) ||
			             char.IsPunctuation(lettersOutput[i]))// || lettersFinal[i] == '^') //)
			    {

				    if (lettersOutput[i] == '(')
					    numberList.Add(')');
				    else if (lettersOutput[i] == ')')
					    numberList.Add('(');
				    else if (lettersOutput[i] == '<')
					    numberList.Add('>');
				    else if (lettersOutput[i] == '>')
					    numberList.Add('<');
				    else if (lettersOutput[i] == '[')
					    list.Add(']');
				    else if (lettersOutput[i] == ']')
					    list.Add('[');
				    else
					    numberList.Add(lettersOutput[i]);
			    }
			    else if( (lettersOutput[i] >= (char)0xD800 && lettersOutput[i] <= (char)0xDBFF) ||
			            (lettersOutput[i] >= (char)0xDC00 && lettersOutput[i] <= (char)0xDFFF))
			    {
				    numberList.Add(lettersOutput[i]);
			    }
			    else
			    {
				    if (numberList.Count > 0)
				    {
					    for (int j = 0; j < numberList.Count; j++)
						    list.Add(numberList[numberList.Count - 1 - j]);
					    numberList.Clear();
				    }
				    if (lettersOutput[i] != 0xFFFF)
					    list.Add(lettersOutput[i]);

			    }
		    }
		    if (numberList.Count > 0)
		    {
			    for (int j = 0; j < numberList.Count; j++)
				    list.Add(numberList[numberList.Count - 1 - j]);
			    numberList.Clear();
		    }

		    // Moving letters from a list to an array.
		    lettersOutput = new char[list.Count];
		    for (int i = 0; i < lettersOutput.Length; i++)
			    lettersOutput[i] = list[i];

		    str = new string(lettersOutput);
            return str;
	    }

    }

}
