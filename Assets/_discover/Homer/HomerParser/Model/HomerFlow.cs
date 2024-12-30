using System;
using System.Collections;
using System.Collections.Generic;


namespace Homer
{
    public class HomerFlow
    {
        public string _id;
        public string _projectId;
        public string _name;
        public Int64 _date;
        public string _state;
        public string[] _meta;
        public HomerNode[] _nodes;
        public string _group;
        public string _slug;

        public string[] metaData; 
        
        public HomerFlowRunning Run()
        {
            return HomerFlowRunning.Instantiate(this);
        }

    }
}
