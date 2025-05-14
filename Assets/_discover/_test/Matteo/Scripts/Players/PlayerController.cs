
using UnityEngine;
using System;

namespace PetanqueGame.Players
{
    public abstract class PlayerController : MonoBehaviour
    {
        protected Action _endTurnCallback;
        public abstract void StartTurn(Action onEndTurn);
    }
}
