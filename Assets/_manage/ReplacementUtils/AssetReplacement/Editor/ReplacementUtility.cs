#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Replacement
{

    /// <summary>
    /// Known issues:
    /// - won't work with prefabs
    /// - multiple references to different components inside the same component are treated as the same (gets references wrong)
    /// </summary>
    public class ReplacementUtility
    {
        public static bool VERBOSE = false;
        public static bool findInactiveObjectsToo = true;

        #region Reference Analysis

        /// <summary>
        ///     All objects that depend on the component
        ///     The component needs a reference to these objects.
        ///     We can retrieve assets that need to be SWAPPED IN/OUT here (such as fonts)
        /// </summary>
        public static void CollectObjectsThatDependOnComponent<T>() where T : Component
        {
            string ss;
            ss = "Objects that depend on " + typeof(T).Name + "...";
            foreach (var component in FindAll<Component>())
            {
                var tmpS = "";
                var count = 0;
                foreach (var dep in EditorUtility.CollectDependencies(new Object[] {component}))
                {
                    if (dep.GetType() != typeof(T)) continue;
                    tmpS += "\n --> " + ToS(dep);
                    count++;
                }
                if (count > 0)
                    ss += "\n" + count + ": " + ToS((Object) component) + tmpS;
            }
            if (VERBOSE) Debug.Log(ss);
        }

        public static Dictionary<TC, List<TO>> CollectObjectsOfTypeTheComponentReferencesInFields<TC, TO>() where TC : Component where TO : Object
        {
            var outDict = new Dictionary<TC, List<TO>>();

            var componentType = typeof(TC);
            var fields = componentType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var component in FindAll<TC>())
            {
                outDict[component] = new List<TO>();
                foreach (var fieldInfo in fields)
                {
                    if (FieldMatchesType(fieldInfo, typeof(TO)))
                    {
                        var referenced = fieldInfo.GetValue(component) as TO;
                        if (referenced != null)
                            outDict[component].Add(referenced);
                    }
                }
            }
            return outDict;
        }


        public static Dictionary<TC, List<TO>> CollectObjectsOfTypeTheComponentDependsOn<TC, TO>() where TC : Component where TO : Object
        {
            var outDict = new Dictionary<TC, List<TO>>();
            var allObjectsDict = CollectObjectsTheComponentDependOn<TC>();
            foreach (var pair in allObjectsDict)
            {
                outDict[pair.Key] = pair.Value.OfType<TO>().ToList();
            }
            return outDict;
        }


        /// <summary>
        ///     All objects that the component depends on.
        ///     The component needs a reference to these objects.
        ///     We can retrieve assets that need to be SWAPPED IN/OUT here (such as fonts)
        /// </summary>
        public static Dictionary<T, List<Object>> CollectObjectsTheComponentDependOn<T>() where T : Component
        {
            var dict = new Dictionary<T, List<Object>>();
            var ss = "" + typeof(T).Name + " depends on...";
            foreach (var component in FindAll<T>())
            {
                dict[component] = new List<Object>();
                var tmpS = "";
                var count = 0;
                foreach (var dep in EditorUtility.CollectDependencies(new Object[] {component}))
                {
                    tmpS += "\n <-- " + ToS(dep);
                    dict[component].Add(dep);
                    count++;
                }
                if (count > 0)
                    ss += "\n" + count + ": " + ToS((Object) component) + tmpS;
            }
            if (VERBOSE) Debug.Log(ss);
            return dict;
        }

        public static Dictionary<Object, List<Component>> CollectObjectsWithComponentsOfType<T>() where T : Component
        {
            return CollectObjectsWithComponentsOfType(typeof(T));
        }
        public static Dictionary<Object, List<Component>> CollectObjectsWithComponentsOfType(Type t)
        {
            var dict = new Dictionary<Object, List<Component>>();
            string ss;
            ss = "Components of type " + t.Name;
            foreach (var go in FindAll<GameObject>())
            {
                var list = new List<Component>();
                var tmpS = "";
                foreach (var component in go.GetComponents(t))
                {
                    tmpS += "\n :> " + ToS((Object) component);
                    list.Add(component);
                }
                if (list.Count > 0)
                {
                    dict[go] = list;
                    ss += "\n" + list.Count + ": " + ToS(go) + tmpS;
                }
            }
            //if (VERBOSE)
                Debug.Log(ss);
            return dict;
        }

        /// <summary>
        ///     Collects all objects that have references to the given component type.
        /// </summary>
        public static Dictionary<T, List<Object>> CollectObjectsReferencingComponent<T>() where T : Component
        {
            var dict = new Dictionary<T, List<Object>>();
            string ss;
            ss = "References to " + typeof(T).Name + ":";
            var none_ss = " - NONE - ";
            foreach (var wantedComponent in FindAll<T>())
            {
                dict[wantedComponent] = new List<Object>();
                var tmpS = "";
                var count = 0;
                var referencers = FindObjectsReferencing(wantedComponent);
                foreach (var obj in referencers)
                {
                    tmpS += "\n ^-- " + ToS(obj);
                    dict[wantedComponent].Add(obj);
                    count++;
                }
                if (count > 0)
                {
                    ss += "\n" + count + ": " + ToS((Object) wantedComponent) + tmpS;
                    none_ss = "";
                }
            }
            ss += none_ss;
            if (VERBOSE) Debug.Log(ss);
            return dict;
        }


        /// <summary>
        ///     Finds all objects that have a reference to the given component.
        ///     Useful to find scripts that reference other components.
        /// </summary>
        public static List<Object> FindObjectsReferencing<T>(T componentToReference) where T : Component
        {
            var referencers = new List<Object>();

            var objs = FindAll<Component>();
            if (objs == null) return referencers;
            foreach (var obj in objs)
            {
                var fields =
                    obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
                                            BindingFlags.Static);
                foreach (var fieldInfo in fields)
                    if (FieldReferencesComponent(obj, fieldInfo, componentToReference))
                        referencers.Add(obj);
            }
            return referencers;
        }

        /// <summary>
        ///     Returns whether a field in an object references the given component.
        /// </summary>
        public static bool FieldReferencesComponent<T>(Component objWithField, FieldInfo fieldInfo, T component)
            where T : Component
        {
            //Debug.Log("TYPE " + fieldInfo.FieldType);
            if (fieldInfo.FieldType.IsArray)
            {
                var arr = fieldInfo.GetValue(objWithField) as Array;
                if (arr == null) return false;
                foreach (var elem in arr)
                {
                    if (elem == null) continue;
                    if (FieldMatchesType(fieldInfo, elem))
                    {
                        var o = elem as T;
                        if (o == component)
                            return true;
                    }
                }
            }
            else
            {
                if (FieldMatchesType(fieldInfo, component))
                {
                    var o = fieldInfo.GetValue(objWithField) as T;
                    if (o == component)
                        return true;
                }
            }
            return false;
        }



        #endregion

        #region Replacement

        public static void PlaceAllReferencesInObject<TTo>(Object o, TTo to) where TTo : Object
        {
            var oType = o.GetType();
            var fields = oType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic );
            foreach (var field in fields)
            {
                if (FieldMatchesType(field, to))
                {
                    field.SetValue(o, to);
                    //Debug.Log("Placed object reference to " + ToS(to) + " in " + ToS(o));
                }
            }
        }

        /// <summary>
        /// Replace all references of the component of type T1 to the component of type T2 in an object.
        /// T1 must derive from T2 for this to work.
        /// </summary>
        public static void ReplaceAllReferencesInObject<T1, T2>(Object o, T1 from, T2 to) where T1 : Object, T2
            where T2 : Object
        {
            var oType = o.GetType();
            var fields = oType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (FieldMatchesType(field, @from))
                {
                    field.SetValue(o, to);
                    //if (VERBOSE)
                    Debug.Log("Replaced reference from " + ToS(from) + " to " + ToS(to) + " in " + ToS(o));
                }
            }
        }

        /// <summary>
        /// Replace all components of type TFrom with components of type TTo.
        /// TFrom must derive from TTo
        /// </summary>
        public static Dictionary<TFrom, TTo> ReplaceAllComponentsOfType<TFrom, TTo>() 
            where TTo : Component where TFrom : Component, TTo
        {
            Dictionary<TFrom, TTo> replacementDictionary = new Dictionary<TFrom, TTo>();
            var foundComponents = FindAll<TFrom>();
            if (VERBOSE) Debug.Log("Found " + foundComponents.Length + " of type " + typeof(TFrom).Name);
            foreach (var foundComponent in foundComponents)
            {
                if (VERBOSE) Debug.Log("Replacement of " + ToS(foundComponent) + " with its base " + typeof(TTo).Name);
                var newComponent = ReplaceComponentWithBase<TFrom, TTo>(foundComponent, foundComponent.gameObject);
                if (newComponent != null)
                {
                    replacementDictionary[foundComponent] = newComponent;
                }
            }
            return replacementDictionary;
        }


        ///     Replace a component of type T1 in the object with a new component of type T2.
        ///     All fields are kept, where possible.
        ///     T2 should be a base type of T1
        private static T2 ReplaceComponentWithBase<T1, T2>(T1 originalComponent, GameObject targetGo) where T2 : Component
            where T1 : Component, T2
        {
            // Rect transforms get messed up, so let's save their values
            ObjectState rectTransformState = null;
            var rectTransformComponent = targetGo.GetComponent<RectTransform>();
            if (rectTransformComponent != null) 
                SaveObjectState(rectTransformComponent, out rectTransformState);

            // Prefab instances must be disconnected
            GameObject currentPrefab = null;
            if (PrefabUtility.GetPrefabType(targetGo) == PrefabType.PrefabInstance)
            {
                //currentPrefab = PrefabUtility.GetPrefabParent(targetGo) as GameObject;
                //PrefabUtility.DisconnectPrefabInstance(targetGo);
                //Debug.Log("Disconnecting " + ToS(targetGo) + " from its prefab.");
                Debug.LogWarning("Replacing won't work with prefab instances! First disconnect them. Skipping " + ToS(targetGo));
                return null;
            }
            else 
            // Won't work with prefab types
            if (PrefabUtility.GetPrefabType(targetGo) == PrefabType.Prefab)
            {
                Debug.LogWarning("Replacing won't work with prefabs! Skipping " + ToS(targetGo));
                return null;
            }

            // Create a temporary gameobject that will keep the field values, needed because T1 and T2 may not coexist
            var tmpNewGo = new GameObject("TempReplacingGO");
            //tmpNewGo.transform.SetParent(targetGo.transform.parent);
            //Object.DestroyImmediate(tmpNewGo.GetComponent<T1>());
            var tmpNewComponent = tmpNewGo.AddComponent<T2>();

            // Copy to the temporary component
            var fromType = typeof(T1);
            var fields = fromType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
                field.SetValue(tmpNewComponent, field.GetValue(originalComponent));
            Object.DestroyImmediate(originalComponent);

            // Copy to the final component
            var finalNewComponent = targetGo.AddComponent<T2>();
            foreach (var field in fields)
                field.SetValue(finalNewComponent, field.GetValue(tmpNewComponent));
            Object.DestroyImmediate(tmpNewGo);

            // At last, copy the rect transform values back in
            if (rectTransformComponent != null) { }
                LoadObjectState(rectTransformComponent, rectTransformState);

            // If we have a prefab, reconnect to it
            if (currentPrefab != null)
            {
                PrefabUtility.ConnectGameObjectToPrefab(targetGo, currentPrefab);
            }

            EditorUtility.SetDirty(targetGo);
            EditorUtility.SetDirty(finalNewComponent);

            return finalNewComponent;
        }

        /// <summary>
        /// Replaces all references from one asset of type TFrom to another of type TTo in all components of type TComp
        /// </summary>
        public static void ReplaceAllAssetReferencesSameComponent<TComp, TFrom, TTo, TRL>(List<TRL> replacementList,
            Dictionary<TComp, List<TFrom>> dependencyDict = null)
            where TRL : ReplacementPair<TFrom, TTo>
            where TComp : Component
            where TFrom : Object, TTo
            where TTo : Object
        {
            if (dependencyDict == null) dependencyDict = CollectObjectsOfTypeTheComponentDependsOn<TComp, TFrom>();
            foreach (var dependencyPair in dependencyDict)
            {
                var referenceFrom = dependencyPair.Key;
                foreach (var referencedObject in dependencyPair.Value)
                {
                    var referenceToAsset = replacementList.Find(x => x.from == referencedObject);
                    Debug.Assert(referenceToAsset != null, "Null new asset to swap with old " + ToS(referencedObject));
                    var referenceTo = referenceToAsset.to;
                    //if (VERBOSE)
                        Debug.Log("WE FOUND " + referencedObject.name + " referenced by " + referenceFrom + " and replace it with " + referenceTo);
                    ReplaceAllReferencesInObject(referenceFrom, referencedObject, referenceTo);
                }
            }
        }



        public static void PlaceAllAssetReferencesMirroring<TComp1, TComp2, TFrom, TTo, TRL>(
            Dictionary<TComp1, TComp2> componentReplacementDict,
            List<TRL> replacementList,
            Dictionary<TComp1, List<TFrom>> dependencyDictFrom)
            where TRL : ReplacementPair<TFrom, TTo>
            where TComp1 : Component
            where TComp2 : Component
            where TFrom : Object, TTo
            where TTo : Object
        {
            Dictionary<TComp2, List<TTo>> dependencyDictTo = new Dictionary<TComp2, List<TTo>>();
            foreach (var pair in dependencyDictFrom)
            {
                var comp1 = pair.Key;
                if (!componentReplacementDict.ContainsKey( pair.Key)) continue;
                var comp2 = componentReplacementDict[comp1];

                Debug.Log("COMP " + ToS(comp1) + " was switched with " + ToS(comp2));

                var list1 = pair.Value;
                dependencyDictTo[comp2] = new List<TTo>();
                foreach (var el1 in list1)
                {
                    var el2 = GetReplacementFor<TFrom, TTo, TRL>(replacementList, el1);
                    dependencyDictTo[comp2].Add(el2);
                    Debug.Log("SWITCHING " + ToS(el1) + " TO " + ToS(el2));
                }
            }
            PlaceAllAssetReferences(dependencyDictTo);
        }

        static TTo GetReplacementFor<TFrom, TTo, TRL>(List<TRL> pairs, TFrom from) where TFrom: Object where TTo: Object
            where TRL : ReplacementPair<TFrom, TTo>
        {
            var pair = pairs.Find(x => x.from == from);
            if (pair == null) return null;
            return pair.to;
        }


        /// <summary>
        /// Set all references of the asset of type TTo in all components of type TComp2.
        /// </summary>
        public static void PlaceAllAssetReferences<TComp, TTo>(
            Dictionary<TComp, List<TTo>> dependencyDictTo)
            where TComp : Component
            where TTo : Object
        {
            foreach (var dependencyPair in dependencyDictTo)
            {
                var componentThatShouldReference = dependencyPair.Key;
                foreach (var objectToReference in dependencyPair.Value)
                {
                    Debug.Log("WE PLACE " + ToS(objectToReference) + " to be referenced by " + ToS(componentThatShouldReference));
                    PlaceAllReferencesInObject(componentThatShouldReference, objectToReference);
                }
            }
        }

        /// <summary>
        /// Replaces all references from one component of type TFrom to another of type TTo in all components
        /// </summary>
        public static void ReplaceAllComponentReferences<TFrom, TTo>(Dictionary<TFrom, TTo> componentReplacementDict,
            Dictionary<TFrom, List<Object>> referencesDict = null)
            where TFrom : Component, TTo
            where TTo : Component
        {
            if (referencesDict == null) referencesDict = CollectObjectsReferencingComponent<TFrom>();
            foreach (var referencePair in referencesDict)
            {
                var referenceFrom = referencePair.Key;
                if (!componentReplacementDict.ContainsKey(referenceFrom)) continue;
                var referenceTo = componentReplacementDict[referenceFrom];

                foreach (var referencingObject in referencePair.Value)
                {
                    //if(VERBOSE)
                        Debug.Log("WE FOUND " + ToS(referenceFrom) + " referenced by " + ToS(referencingObject));
                    ReplaceAllReferencesInObject(referencingObject, referenceFrom, referenceTo);
                }
            }
        }

        #endregion


        #region Object State

        public class ObjectState
        {
            public Dictionary<FieldInfo, object> fieldInfoDict = new Dictionary<FieldInfo, object>();
            public Dictionary<PropertyInfo, object> propertyInfoDict = new Dictionary<PropertyInfo, object>();
        }

        private static void SaveObjectState<T>(T obj, out ObjectState state) where T : Object
        {
            state = new ObjectState();
            var objType = typeof(T);
            var objFields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var objProperties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            //Debug.LogError("WE GOT A RECT! " + rectFields.Length + " + " + rectProperties.Length);
            foreach (var field in objFields)
            {
                state.fieldInfoDict[field] = field.GetValue(obj);
            }
            foreach (var property in objProperties)
            {
                state.propertyInfoDict[property] = property.GetValue(obj, null);
            }
        }

        private static void LoadObjectState<T>(T obj, ObjectState state) where T : Object
        {
            var objType = typeof(T);
            var objFields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            var objProperties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var field in objFields)
            {
                field.SetValue(obj, state.fieldInfoDict[field]);
            }
            foreach (var property in objProperties)
            {
                if (property.CanWrite)
                    property.SetValue(obj, state.propertyInfoDict[property], null);
            }
        }

        #endregion


        #region Utilities

        public static bool FieldMatchesType(FieldInfo fieldInfo, object c)
        {
            return c != null && (c.GetType() == fieldInfo.FieldType || c.GetType().IsSubclassOf(fieldInfo.FieldType));
        }

        public static bool FieldMatchesType(FieldInfo fieldInfo, Type t)
        {
            return  (t == fieldInfo.FieldType || t.IsSubclassOf(fieldInfo.FieldType));
        }

        private static T[] FindAll<T>() where T : Object
        {
            if (findInactiveObjectsToo)
            {
                return Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
            }
            else
            {
                return Object.FindObjectsOfType<T>();
            }
        }

        public static string ToS(Object o)
        {
            return o == null ? "NULL" : (o.name + " (" + o.GetType().Name + ")");
        }

        public static string ToS(Component c)
        {
            return c == null ? "NULL" : (c.GetType().Name + " in GameObject " + c.name);
        }

        #endregion

    }
}

#endif