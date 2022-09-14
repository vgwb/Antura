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
            GameObject BorderCollider = null;
            GameObject hd;
            List<GameObject> arrowLines = new List<GameObject>();
            List<GameObject> lines = new List<GameObject>();
            List<GameObject> tutorialWaypoints = new List<GameObject>();

            Vector3 characterStartPosition = new Vector3();

            foreach (Transform child in transform)
            {
                if (child.name == "Letter")
                {
                    child.name = "MazeLetter";
                    letter = child.gameObject.AddComponent<MazeLetter>();

                    var box = child.gameObject.AddComponent<BoxCollider>();
                    var boxSize = box.size;
                    boxSize.z = 0.05f;
                    box.size = boxSize;
                    child.gameObject.AddComponent<MeshCollider>();
                }

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
                    AddDotAndHideArrow(child.GetChild(0));

                    arrowLines.Add(child.gameObject);

                    if (arrowLines.Count == 1)
                    {
                        characterStartPosition = child.GetChild(0).position;
                    }

                    foreach (Transform fruit in child.transform)
                    {
                        var box = fruit.gameObject.AddComponent<BoxCollider>();
                        box.center = new Vector3(0.00789929274f, 0.000789958867f, 0.02f);
                        box.size = new Vector3(0.0473955721f, 0.0336077549f, 0.04f);
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

            if (_callback != null)
            { _callback(); }
        }

        //public static GameObject prevDot;

        public void HideDotAndShowArrow(Transform arrowTr)
        {
            var mazeArrow = arrowTr.gameObject.GetComponent<MazeArrow>();
            if (mazeArrow.arrowMesh != null) mazeArrow.arrowMesh.enabled = true;
            if (mazeArrow.dotMesh != null) mazeArrow.dotMesh.enabled = false;
        }

        public void AddDotAndHideArrow(Transform arrowTr)
        {
            //if (prevDot != null)
            //{
            //    prevDot.GetComponent<Renderer>().enabled = false;
            //}

            var mazeArrow = arrowTr.gameObject.GetComponent<MazeArrow>();
            if (mazeArrow.dotMesh == null)
            {
                var currentDot = Instantiate(MazeGame.instance.dotPrefab, arrowTr.transform);
                currentDot.name = "Dot";
                currentDot.transform.localPosition = new Vector3(0, 0.05f, 0f);
                currentDot.transform.localEulerAngles = new Vector3(90, 0, 0);
                currentDot.transform.localScale = Vector3.one * 0.1f;
                mazeArrow.dotMesh = currentDot.GetComponent<Renderer>();
                mazeArrow.dotMesh.GetComponent<Renderer>().enabled = true;

                mazeArrow.GetComponentInChildren<MeshRenderer>().material.color = mazeArrow.highlightedColor;
                mazeArrow.dotMesh.material.color = mazeArrow.highlightedColor;

                //prevDot = currentDot;
            }
            arrowTr.GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        public void build(System.Action callback)
        {
            _callback = callback;
        }

    }
}
