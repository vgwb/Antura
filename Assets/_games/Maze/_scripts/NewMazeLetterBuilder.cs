using System.Collections.Generic;
using DG.DeExtensions;
using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class NewMazeLetterBuilder : MonoBehaviour
    {
        public int letterDataIndex = 0;
        private System.Action _callback = null;

        public void Build()
        {
            //transform.position = new Vector3(0, 0, -1);
            transform.localScale = new Vector3(15, 15, 15);
            transform.rotation = Quaternion.Euler(90, 0, 0);

            //set up everything correctly:
            string name = gameObject.name;
            int cloneIndex = name.IndexOf("(Clone");

            if (cloneIndex != -1) {
                name = name.Substring(0, cloneIndex);
            }

            gameObject.name = name;

            GameObject character = (GameObject)Instantiate(MazeGame.instance.characterPrefab, transform);
            character.name = "Mazecharacter";
            character.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            MazeCharacter mazeCharacter = character.GetComponent<MazeCharacter>();
            mazeCharacter.SpawnOffscreen();

            MazeLetter letter = null;
            GameObject BorderCollider = null;
            GameObject hd;
            List<GameObject> arrowLines = new List<GameObject>();
            List<GameObject> lines = new List<GameObject>();
            List<GameObject> tutorialWaypoints = new List<GameObject>();

            Vector3 characterStartPosition = new Vector3();

            foreach (Transform child in transform) {
                if (child.name == "Letter")
                {
                    child.name = "MazeLetter";
                    letter = child.gameObject.AddComponent<MazeLetter>();

                    // TODO: handle collisions somehow
                    var box = child.gameObject.AddComponent<BoxCollider>();
                    var boxSize = box.size;
                    boxSize.z = 0.05f;
                    box.size = boxSize;
                    child.gameObject.AddComponent<MeshCollider>();
                }

                // TODO: handle collisions somehow
                if (child.name.IndexOf("Contour") != -1)
                {
                    child.name = "BorderCollider";
                    BorderCollider = child.gameObject;
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;

                    mazeCharacter.myCollider = child.gameObject.AddComponent<MeshCollider>();
                    child.gameObject.AddComponent<TrackBounds>();
                    child.gameObject.layer = 17;
                }

                if (child.name.IndexOf("arrow_stroke") == 0)
                {
                    AddDotAndHideArrow(child);
                    arrowLines.Add(child.gameObject);

                    if (arrowLines.Count == 1) {
                        characterStartPosition = child.GetChild(0).position;
                    }

                    foreach (Transform fruit in child.transform) {
                        var box = fruit.gameObject.AddComponent<BoxCollider>();
                        box.center = new Vector3(0.00789929274f, 0.000789958867f, -3.05139807e-08f);
                        box.size = new Vector3(0.0473955721f, 0.0336077549f, 4.46760318e-09f);
                    }
                }

                if (child.name.IndexOf("tutorialPoint_stroke") == 0)
                {
                    tutorialWaypoints.Add(child.gameObject);
                }

                if (child.name.IndexOf("Lines") == 0)
                {
                    foreach (Transform lineTr in child.transform)
                    {
                        lines.Add(lineTr.gameObject);
                    }
                }
            }

            mazeCharacter.SetMazeLetter(letter);
            mazeCharacter.CreateFruits(arrowLines);

            letter.mazeCharacter = mazeCharacter;

            hd = new GameObject();
            hd.name = "HandTutorial";
            hd.transform.SetParent(transform, false);
            hd.transform.position = characterStartPosition;

            HandTutorial handTut = hd.AddComponent<HandTutorial>();
            handTut.pathsToFollow = tutorialWaypoints;
            handTut.visibleArrows = arrowLines;
            handTut.linesToShow = lines;

            gameObject.AddComponent<MazeShowPrefab>().letterIndex = letterDataIndex;

            if (_callback != null) { _callback(); }
        }

        private void AddDotAndHideArrow(Transform arrowParent)
        {
            GameObject firstArrow = arrowParent.GetChild(0).gameObject;
            GameObject newDot = Instantiate(MazeGame.instance.dotPrefab, firstArrow.transform);
            newDot.name = "Dot";
            newDot.transform.localPosition = Vector3.zero;
            newDot.transform.rotation = firstArrow.transform.rotation;
            newDot.transform.Rotate(Vector3.forward, 180, Space.World);
            newDot.transform.Rotate(Vector3.right, 90, Space.World);
            newDot.transform.localScale = Vector3.one * 0.1f;

            firstArrow.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        public void build(System.Action callback)
        {
            _callback = callback;
        }

    }
}