using UnityEngine;
using System.Collections;
using Antura.Minigames;

namespace Antura.Minigames.SickLetters
{
    public class SickLettersGameManager : MonoBehaviour
    {

        public SickLettersGame game;




        // Use this for initialization
        void Start()
        {
            game = GetComponent<SickLettersGame>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void sucess()
        {
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Win);
            game.slCamera.moveCamera(1);
            game.scale.flyVas(3);
            game.SetCurrentState(game.ResultState);

        }

        public void failure()
        {
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Lose);
            game.slCamera.moveCamera(1);
            StartCoroutine(game.antura.bark(1));
            StartCoroutine(game.scale.dropVase(3, true));
        }

        public void holeON(float speed = 1)
        {
            StartCoroutine(coHoleOn(speed));
        }

        IEnumerator coHoleOn(float speed)
        {
            game.hole.gameObject.SetActive(true);
            game.hole["Take 001"].wrapMode = WrapMode.PingPong;
            game.hole["Take 001"].speed = speed;
            game.hole.Play("Take 001");
            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.TrapdoorOpen);

            yield return new WaitForSeconds((game.hole["Take 001"].length / game.hole["Take 001"].speed) + 0.2f);

            SickLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.TrapdoorClose);

            yield return new WaitForSeconds((game.hole["Take 001"].length / game.hole["Take 001"].speed) - 0.2f);
            //game.hole["Take 001"]
            game.hole.Stop("Take 001");

            game.hole.gameObject.SetActive(false);
        }
    }
}
