using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public class HomerMetadata
    {
        public string _id;
        public string _uid;
        public string _name;
        public HomerMetadataValue[] _values;
    }


    public class HomerMetadataValue
    {
        public string _id;
        public string _uid;
        public string _metadataId;
        public string _value;
    }
    
    public class HomerMetadataNodeValue
    {
        public string _metadataUid;
        public string _metadataValueId;
    }
    
        
}
