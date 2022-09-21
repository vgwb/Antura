using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames.Maze
{
    public class MazeLetterBuilder : MonoBehaviour
    {
        public int letterDataIndex = 0;
        private System.Action _callback = null;

        void Start()
        {
            transform.position = new Vector3(0, 0, -1);
            transform.localScale = new Vector3(15, 15, 15);
            transform.rotation = Quaternion.Euler(0, 180, 0);

            //set up everything correctly:
            string name = gameObject.name;
            int cloneIndex = name.IndexOf("(Clone");

            if (cloneIndex != -1)
            {
                name = name.Substring(0, cloneIndex);
            }

            gameObject.name = name;

            GameObject character = (GameObject)Instantiate(MazeGame.instance.characterPrefab, transform);
            character.name = "Mazecharacter";
            character.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            MazeCharacter mazeCharacter = character.GetComponent<MazeCharacter>();
            mazeCharacter.SpawnOffscreen();

            MazeLetter letter = null;
            GameObject BorderColldider = null;
            GameObject hd;
            List<GameObject> arrows = new List<GameObject>();
            List<GameObject> lines = new List<GameObject>();
            List<GameObject> tutorialWaypoints = new List<GameObject>();

            Vector3 characterPosition = new Vector3();

            foreach (Transform child in transform)
            {
                if (child.name == name)
                {
                    child.name = "MazeLetter";
                    letter = child.gameObject.AddComponent<MazeLetter>();

                    child.gameObject.AddComponent<BoxCollider>();
                    child.gameObject.AddComponent<MeshCollider>();
                }

                if (child.name.IndexOf("_coll") != -1)
                {
                    child.name = "BorderCollider";
                    BorderColldider = child.gameObject;
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;

                    mazeCharacter.myCollider = child.gameObject.AddComponent<MeshCollider>();
                    child.gameObject.AddComponent<TrackBounds>();
                    child.gameObject.layer = 17;
                }
                if (child.name.IndexOf("arrow") == 0)
                {
                    AddDotAndHideArrow(child);
                    arrows.Add(child.gameObject);

                    if (arrows.Count == 1)
                    {
                        //find the first child in the transform:
                        characterPosition = child.GetChild(0).position;
                    }

                    foreach (Transform fruit in child.transform)
                    {
                        fruit.gameObject.AddComponent<BoxCollider>();
                    }
                    Transform tutorialWaypointsForPath = transform.Find("TutorialWaypoints" + child.name.Substring(5));
                    tutorialWaypoints.Add(tutorialWaypointsForPath == null ? child.gameObject : tutorialWaypointsForPath.gameObject);

                }
                if (child.name.IndexOf("line") == 0)
                {
                    lines.Add(child.gameObject);
                }
            }
            //character.transform.position = characterPosition + new Vector3(0,1,0);

            //fix mazecharacter:
            mazeCharacter.SetMazeLetter(letter);
            mazeCharacter.CreateFruits(arrows);

            letter.mazeCharacter = mazeCharacter;

            hd = new GameObject();
            hd.name = "HandTutorial";
            hd.transform.SetParent(transform, false);
            hd.transform.position = characterPosition;

            HandTutorial handTut = hd.AddComponent<HandTutorial>();
            handTut.pathsToFollow = tutorialWaypoints;
            handTut.visibleArrows = arrows;
            handTut.linesToShow = lines;

            gameObject.AddComponent<MazeShowPrefab>().letterIndex = letterDataIndex;
            //gameObject.GetComponent<MazeShowPrefab>().letterId = letterData;

            if (_callback != null)
            { _callback(); }
        }

        private void AddDotAndHideArrow(Transform arrowParent)
        {
            GameObject firstArrow = arrowParent.GetChild(0).gameObject;
            GameObject newDot = Instantiate(MazeGame.instance.dotPrefab, firstArrow.transform);
            newDot.name = "Dot";
            newDot.transform.localPosition = Vector3.zero;
            newDot.transform.rotation = firstArrow.transform.rotation;
            newDot.transform.Rotate(Vector3.forward, 180, Space.World);
            newDot.transform.localScale = Vector3.one * 0.1f;
            newDot.GetComponent<Renderer>().enabled = true;

            firstArrow.GetComponent<MeshRenderer>().enabled = false;
        }

    }
}
