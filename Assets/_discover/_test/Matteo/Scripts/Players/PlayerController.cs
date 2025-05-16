using UnityEngine;

namespace PetanqueGame.Players
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Turn Manager Settings")]
        [SerializeField] private MonoBehaviour scriptToDisableDuringTurn;
        [SerializeField] private Transform _bouldsToPlayHolder;

        protected System.Action _endTurnCallback;

        public MonoBehaviour ScriptToDisable => scriptToDisableDuringTurn;
        public Transform BouldsToPlayHolder => _bouldsToPlayHolder;

        public virtual void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;
        }
    }
}
