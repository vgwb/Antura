using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public class HomerLocalVariable
    {
        public string Name;

        public bool? BoolValue;
        public string StringValue;
        public int? IntValue;
        public float? FloatValue;

        public void SetValue(object o)
        {
            if(o is bool)
            {
                BoolValue = (bool)o;

            } else if(o is int)
            {
                IntValue = (int)o;

            } else if(o is float)
            {
                FloatValue = (float)o;

            } else if(o is double)
            {
                FloatValue = (float)((double)o);
                
            } else
            {
                StringValue = o.ToString();
            }
        }

        public object GetValue()
        {
            if(BoolValue != null)
                return BoolValue;

            if(IntValue != null)
                return IntValue;

            if(FloatValue != null)
                return FloatValue;

            return StringValue;
        }
    }
}
