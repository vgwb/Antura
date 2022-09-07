using UnityEngine;
using UnityEngine.Serialization;

namespace Antura.Minigames.Maze
{
    public class MazeArrow : MonoBehaviour
    {
        public bool tweenToColor = false;
        public bool pingPong = false;

        public Color normalColor;
        public Color highlightedColor;
        public Color unreachedColor;
        public Color launchPositionColor;

        public Renderer arrowMesh;
        public Renderer dotMesh;

        private GameObject highlightFX;
        private ParticleSystem.MainModule particleSystemMainModule;

        private Color greenParticleSystemColor = new Color(0.2549f, 1f, 0f, 0.3765f);
        private Color redParticleSystemColor = new Color(1f, 0f, 0.102f, 0.3765f);
        private Color yellowParticleSystemColor = new Color(1f, 0.714f, 0f, 0.3765f);

        public enum HighlightState
        {
            None, LaunchPosition, Reached, Unreached
        }
        public HighlightState highlightState;
        public float splineValue;

        public void HighlightAsLaunchPosition()
        {
            if (highlightState != HighlightState.LaunchPosition)
            {
                particleSystemMainModule.loop = true;
                particleSystemMainModule.startColor = yellowParticleSystemColor;
                arrowMesh.material.color = launchPositionColor;
                if (dotMesh != null) dotMesh.material.color = launchPositionColor;
                highlightFX.SetActive(true);

                highlightState = HighlightState.LaunchPosition;
            }
        }

        public void HighlightAsReached()
        {
            if (highlightState != HighlightState.Reached)
            {
                particleSystemMainModule.loop = false;
                particleSystemMainModule.startColor = greenParticleSystemColor;
                highlightFX.SetActive(true);
                arrowMesh.material.color = highlightedColor;
                if (dotMesh != null) dotMesh.material.color = highlightedColor;
                MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.OK);

                highlightState = HighlightState.Reached;
                //Debug.LogError("SET REACHED " + this.name);
            }
        }

        public void Unhighlight()
        {
            highlightFX.SetActive(false);
            arrowMesh.material.color = normalColor;
            if (dotMesh != null) dotMesh.material.color = normalColor;

            highlightState = HighlightState.None;
            //Debug.LogError("SET UNHIGHLIGHTED " + this.name);
        }

        public void MarkAsUnreached(bool isFirstUnreachedArrow)
        {
            if (highlightState != HighlightState.Unreached)
            {
                arrowMesh.material.color = unreachedColor;
                if (dotMesh != null) dotMesh.material.color = unreachedColor;

                if (isFirstUnreachedArrow)
                {
                    particleSystemMainModule.startColor = redParticleSystemColor;
                    highlightFX.SetActive(true);
                }

                highlightState = HighlightState.Unreached;
                //Debug.LogError("SET UNREACHED " + this.name);
            }
        }

        void Awake()
        {
            normalColor = new Color(0.964f, 0.875f, 0f, 1f);
            highlightedColor = new Color(0.4275f, 1f, 0.4471f, 1f);
            unreachedColor = Color.red;
            launchPositionColor = Color.Lerp(yellowParticleSystemColor, Color.white, 0.2f);
            launchPositionColor.a = 1f;

            highlightFX = Instantiate(MazeGame.instance.arrowTargetPrefab);
            highlightFX.transform.position = transform.position;
            highlightFX.transform.localScale = Vector3.one * 2f;
            highlightFX.transform.parent = gameObject.transform;

            particleSystemMainModule = highlightFX.GetComponent<ParticleSystem>().main;

            highlightFX.SetActive(false);
        }

        public void Reset()
        {
            tweenToColor = false;
            pingPong = false;
            particleSystemMainModule.startColor = greenParticleSystemColor;
            particleSystemMainModule.loop = true;
            Unhighlight();
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}

