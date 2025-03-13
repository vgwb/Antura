// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2023/12/09

using System;
using UnityEngine;

namespace Demigiant.DemiTools.DeUnityExtended
{
    /// <summary>
    /// When extending Components that have a custom Inspector, like Button, mark your extra custom serialized properties with this attribute,
    /// so that you can use DeSerializedObjectUtils in a custom Inspector to quickly find them and draw them 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomPropertyAttribute : PropertyAttribute {}
}