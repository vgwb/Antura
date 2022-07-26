using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeArrow : MonoBehaviour
    {
        public bool tweenToColor = false;
        public bool pingPong = false;

        private Color normalColor;
        private Color highlightedColor;
        private Color unreachedColor;
        private Color launchPositionColor;

        public Renderer arrowOrDotMesh;

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

        public void HighlightAsLaunchPosition()
        {
            if (highlightState != HighlightState.LaunchPosition)
            {
                particleSystemMainModule.loop = true;
                particleSystemMainModule.startColor = yellowParticleSystemColor;
                arrowOrDotMesh.material.color = launchPositionColor;
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
                arrowOrDotMesh.material.color = highlightedColor;
                MazeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.OK);

                highlightState = HighlightState.Reached;
            }
        }

        public void Unhighlight()
        {
            highlightFX.SetActive(false);
            arrowOrDotMesh.material.color = normalColor;

            highlightState = HighlightState.None;
        }

        public void MarkAsUnreached(bool isFirstUnreachedArrow)
        {
            if (highlightState != HighlightState.Unreached)
            {
                arrowOrDotMesh.material.color = unreachedColor;

                if (isFirstUnreachedArrow)
                {
                    particleSystemMainModule.startColor = redParticleSystemColor;
                    highlightFX.SetActive(true);
                }

                highlightState = HighlightState.Unreached;
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

