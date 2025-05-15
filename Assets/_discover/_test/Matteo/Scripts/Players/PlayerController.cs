using UnityEngine;

namespace PetanqueGame.Players
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Turn Manager Settings")]
        [SerializeField] private GameObject model;
        [SerializeField] private MonoBehaviour scriptToDisableDuringTurn;

        protected System.Action _endTurnCallback;

        public GameObject Model => model;
        public MonoBehaviour ScriptToDisable => scriptToDisableDuringTurn;

        public virtual void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;
        }
    }
}
