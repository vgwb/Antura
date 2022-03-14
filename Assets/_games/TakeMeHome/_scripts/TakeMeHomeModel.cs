using System.Collections.Generic;


namespace Antura.Minigames.TakeMeHome
{

    public class TakeMeHomeModel
    {

        public int[][] tubesToLetters = new int[4][];
        List<int> usedLetters = new List<int>();
        /////////////////
        // Singleton Pattern
        static TakeMeHomeModel instance;
        public static TakeMeHomeModel Instance
        {
            get
            {
                if (instance == null)
                    instance = new TakeMeHomeModel();
                return instance;
            }
        }
        /////////////////

        private TakeMeHomeModel()
        {
            tubesToLetters[0] = new int[] { 19, 1, 26, 23, 13, 11, 10, 8, 3, 16 };
            tubesToLetters[1] = new int[] { 2, 7, 15, 24, 22, 9 };
            tubesToLetters[2] = new int[] { 14, 12, 4, 27 };
            tubesToLetters[3] = new int[] { 20, 21, 18, 6, 17, 5, 0, 25 };
        }


        public int getRandomLetterOnTube(int tubeId)
        {
            int randLetter = tubesToLetters[tubeId][UnityEngine.Random.Range(0, tubesToLetters[tubeId].Length)];
            while (usedLetters.IndexOf(randLetter) != -1)
                randLetter = tubesToLetters[tubeId][UnityEngine.Random.Range(0, tubesToLetters[tubeId].Length)];

            usedLetters.Add(randLetter);
            return randLetter;
        }

    }
}
