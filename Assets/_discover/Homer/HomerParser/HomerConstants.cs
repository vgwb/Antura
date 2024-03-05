using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public static class Regexes
    {
        public static string VARIATIONS = "\\[\\[(.*?)\\]\\]";
        public static string CONDITIONALS = "\\[IF(.*?)\\]";
        public static string CONDITIONALS_INTERNAL = "([\"'])(?:(?=(\\\\?))\\2.)*?\\1";
        public static string VARIABLES = "\\{(.*?)\\}";
        public static string LOCAL_VARIABLES = "\\%([a-zA-Z0-9\\]\\w.?()[a-zA-Z]+)";
        public static string GLOBAL_VARIABLES = "\\$([a-zA-Z0-9\\]\\w.?()[a-zA-Z]+)";
        public static string EXTENDED_TRIM = "^(\\s+<br(\\/)?>)*|(<br(\\/)?>\\s)*$";
        public static string EXTENDED_REPLACE = "^(\\?<br(\\/)?>\\?)+|(\\?<br(\\/)?>\\?)+$";
        public static string SQUARE_BRACKETS = "\\[\\+\\]";
        public static string TODO = "\\[TODO(.*?)\\]";
        public static string ONCE = "\\[-\\]";

	}

	public static class NodeType
	{
		public const string start = "Start";
		public const string text = "Text";
		public const string note = "Note";
		public const string choice = "Choice";
		public const string condition = "Condition";
		public const string variables = "Variables";
		public const string random = "Random";
		public const string sequence = "Sequence";
		public const string jumpToNode = "JumpToNode";
		public const string label = "Label";
		public const string layout = "Layout";
		public const string subFlow = "SubFlow";

		public const string failCondition = "FailCondition";
	}

	public static class CycleType
	{
		public const string list = "List";
		public const string loop = "Loop";
		public const string random = "Random";
		public const string smartRandom = "Smart Random";
		public const string none = null;
	}

	public static class VariationType
	{
		public const string list = "LIST";
		public const string loop = "LOOP";
		public const string rnd = "RND";
		public const string srnd = "SRND";
	}


}
