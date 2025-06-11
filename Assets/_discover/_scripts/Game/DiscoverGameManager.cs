using System.Collections;
using Antura.Utilities;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public enum GameplayState
    {
        None, // no state defined
        Changing, // here if the state is currently changing
        Setup, // setup here we set things useful for gameplay
        Intro, // intro state
        Play3D, // here we play in the world
        Play2D, // here we play a 2D game
        Map, // here if we're in the world map
        Dialogue, // all dialogues magic dialogs are here
        End, // end the game, show results
    }

    public class DiscoverGameManager : SingletonMonoBehaviour<DiscoverGameManager>
    {
        public GameplayState State { get; private set; }
        /// <summary>Last play state (used to know to which play state to return after a dialogue/etc. state)</summary>
        public GameplayState LastPlayState { get; private set; }
        // when we pause the game we use this global var
        public bool isPaused;

        Coroutine coChangeState;

        IEnumerator Start()
        {
            State = GameplayState.None;

            yield return new WaitForSeconds(0.5f);
            
            // TODO > Set to Play3D at the right time, after eventual Intro etc.
            DiscoverGameManager.I.ChangeState(GameplayState.Play3D, true);
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        /// <summary>Changing state takes one frame unless forced to be immediate</summary>
        public void ChangeState(GameplayState newState, bool immediate = false)
        {
            if (newState == State) return;

            this.RestartCoroutine(ref coChangeState, CO_ChangeState(newState, immediate));
        }
        IEnumerator CO_ChangeState(GameplayState newState, bool immediate)
        {
            if (!immediate)
            {
                State = GameplayState.Changing;
                yield return null;
            }
            
            // Store last play state
            switch (newState)
            {
                case GameplayState.Play3D:
                case GameplayState.Play2D:
                    LastPlayState = newState;
                    break;
                default:
                    switch (State)
                    {
                        case GameplayState.Play3D:
                        case GameplayState.Play2D:
                            LastPlayState = State;
                            break;
                    }
                    break;
            }
            
            State = newState;
        }
    }
}
