using System;
using System.Linq;
using System.Collections.Generic;

namespace Antura.Helpers
{
    /// <summary>
    /// Static helper class for randomization utilities.
    /// </summary>
    public static class RandomHelper
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        #region Value Random

        /// <summary>
        /// Return random float value around _value parameter + or - _variation.
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_variation"></param>
        /// <returns></returns>
        public static float GetValueWithRandomVariation(float _value, float _variation)
        {
            return UnityEngine.Random.Range(_value - _variation, _value + _variation);
        }

        #endregion


        #region Random List Selection

        /// <summary>
        /// Get a random element from a list.
        /// </summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new Exception("Cannot get a random element from the list as count is zero.");
            }
            return list[_random.Next(0, list.Count)];
        }

        /// <summary>
        /// Get a random element from an array.
        /// </summary>
        public static T GetRandomParams<T>(params T[] ids)
        {
            return ids[_random.Next(0, ids.Length)];
        }

        /// <summary>
        /// Get a random value of an enumerator.
        /// </summary>
        public static T GetRandomEnum<T>()
        {
            var A = Enum.GetValues(typeof(T));
            var V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        #endregion


        #region Safe List Selection

        /// <summary>
        /// Randomly selects one item from a list.
        /// </summary>
        public static T RandomSelectOne<T>(this List<T> all_list)
        {
            if (all_list.Count == 0)
            {
                throw new Exception("The list has zero elements to select from.");
            }

            return RouletteSelectNonRepeating(all_list, 1)[0];
        }

        /// <summary>
        /// Randomly selects a number of items from a list.
        /// </summary>
        public static List<T> RandomSelect<T>(this List<T> all_list, int maxNumberToSelect, bool forceMaxNumber = false)
        {
            if (maxNumberToSelect == 0)
            {
                return new List<T>();
            }

            if (all_list.Count == 0)
            {
                throw new Exception("The list has zero elements to select from.");
            }

            if (!forceMaxNumber && all_list.Count < maxNumberToSelect)
            {
                maxNumberToSelect = all_list.Count;
            }

            return RouletteSelectNonRepeating(all_list, maxNumberToSelect);
        }

        public static List<T> RouletteSelectNonRepeating<T>(List<T> fromList, int numberToSelect)
        {
            if (numberToSelect > fromList.Count)
            {
                throw new Exception("Cannot select more than available with a non-repeating selection");
            }

            var chosenList = new List<T>();

            if (numberToSelect == fromList.Count)
            {
                chosenList.AddRange(fromList);
                return chosenList;
            }

            for (var choice_index = 0; choice_index < numberToSelect; choice_index++)
            {
                var element_index = UnityEngine.Random.Range(0, fromList.Count);
                var chosenItem = fromList[element_index];
                fromList.RemoveAt(element_index);
                chosenList.Add(chosenItem);
            }

            return chosenList;
        }

        public static List<T> RouletteSelectNonRepeating<T>(List<T> fromList, List<float> weightsList, int numberToSelect)
        {
            if (numberToSelect > fromList.Count)
            {
                throw new Exception("Cannot select more than available with a non-repeating selection");
            }

            var chosenList = new List<T>();

            if (numberToSelect == fromList.Count)
            {
                chosenList.AddRange(fromList);
                chosenList.Shuffle();
                return chosenList;
            }

            for (var choice_index = 0; choice_index < numberToSelect; choice_index++)
            {
                var totalWeight = weightsList.Sum();
                var choiceValue = UnityEngine.Random.value * totalWeight;
                float cumulativeWeight = 0;
                for (var element_index = 0; element_index < fromList.Count; element_index++)
                {
                    cumulativeWeight += weightsList[element_index];
                    if (choiceValue <= cumulativeWeight)
                    {
                        var chosenItem = fromList[element_index];
                        fromList.RemoveAt(element_index);
                        weightsList.RemoveAt(element_index);
                        chosenList.Add(chosenItem);
                        break;
                    }
                }
            }

            return chosenList;
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffle a list in place.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Shuffle a list, returning the shuffled copy.
        /// </summary>
        public static IList<T> ShuffleCopy<T>(this IList<T> thisList)
        {
            IList<T> list = new List<T>(thisList.ToArray());

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        #endregion
    }
}
