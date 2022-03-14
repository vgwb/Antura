using Antura.Tutorial;
using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames.Maze
{
    public class HandTutorial : MonoBehaviour
    {
        public float handSpeed = 2.0f;

        //support mutliple paths:
        public List<GameObject> pathsToFollow;
        public List<GameObject> linesToShow;
        public List<GameObject> visibleArrows;

        private List<Vector3> wayPoints = new List<Vector3>();
        private int currentPath = 0;


        public bool isStopped = false;
        private Vector3 startingPosition;
        public bool isShownOnce = false;

        void Start()
        {

            startingPosition = new Vector3(-1, -1, -1);

            //hide the path
            foreach (GameObject visibleArrow in visibleArrows)
            {
                visibleArrow.SetActive(false);
            }

            foreach (GameObject lineToShow in linesToShow)
            {
                lineToShow.SetActive(false);
            }

            gameObject.SetActive(false);
            isShownOnce = false;
        }

        void Update()
        {
        }

        public void showCurrentTutorial()
        {
            if (startingPosition.x != -1 && startingPosition.y != -1 && startingPosition.z != -1)
            {
                gameObject.transform.position = startingPosition;
            }
            gameObject.SetActive(true);
            setWayPoints();
        }

        public void stopCurrentTutorial()
        {
            TutorialUI.Clear(false);
            wayPoints.Clear();
            gameObject.SetActive(false);

            //set tutorial done:
            //isMovingOnPath = false;
            isStopped = true;
        }

        private void setWayPoints()
        {
            gameObject.SetActive(true);

            wayPoints = new List<Vector3>();
            //construct the path waypoints:
            visibleArrows[currentPath].SetActive(true);
            //numbersToShow [currentPath].SetActive (true);
            linesToShow[currentPath].SetActive(true);

            foreach (Transform child in pathsToFollow[currentPath].transform)
            {
                wayPoints.Add(child.transform.position);
            }

            startingPosition = wayPoints[0];

            //currentWayPoint = 0;
            isShownOnce = true;
            MazeGame.instance.timer.StartTimer();
            if (wayPoints.Count == 1)
            {
                TutorialUI.ClickRepeat(wayPoints[0]);
            }
            else
            {
                TutorialUI.DrawLine(wayPoints.ToArray(), TutorialUI.DrawLineMode.FingerAndArrow, false, true);
            }

        }

        public bool isComplete()
        {
            return currentPath == pathsToFollow.Count - 1;
        }

        public void moveToNextPath()
        {
            if (currentPath < pathsToFollow.Count - 1)
            {
                isStopped = false;

                HideCheckpointsAndLineOfCurrentPath();

                currentPath++;
                setWayPoints();
            }
        }

        public void HideCheckpointsAndLineOfCurrentPath()
        {
            pathsToFollow[currentPath].SetActive(false);
            visibleArrows[currentPath].SetActive(false);
            linesToShow[currentPath].SetActive(false);
        }

        public void HideAllCheckpointsAndLines()
        {
            foreach (var path in pathsToFollow)
            {
                path.SetActive(false);
            }

            foreach (var arrow in visibleArrows)
            {
                arrow.SetActive(false);
            }

            foreach (var line in linesToShow)
            {
                line.SetActive(false);
            }
        }

        public bool isCurrentTutorialDone()
        {
            return true;
        }
    }
}
