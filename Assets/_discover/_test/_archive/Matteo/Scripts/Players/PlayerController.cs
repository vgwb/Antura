using System.Collections.Generic;
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

        public List<Transform> Boules { get; private set; } = new();

        private void Start()
        {
            Boules.Clear();
            foreach (Transform child in _bouldsToPlayHolder)
            {
                Boules.Add(child);
            }
        }

        public virtual void StartTurn(System.Action onEndTurn)
        {
            _endTurnCallback = onEndTurn;
            // Qui può partire la logica del turno per il player specifico
        }
    }
}
