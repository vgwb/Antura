using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public class HomerElement
	{
        public string _id;
        public string _type;
        public string _nodeId;
        public HomerLocalizedContent[] _localizedContents;
        public bool _onlyOnce;
        public bool _ifNoMore;
        public bool _visited;
        public bool _toBeTranslated;

        public bool _justOnce;
        
        public string _permalink;
    }
}
