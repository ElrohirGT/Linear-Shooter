using UnityEngine;
using Utilities;
using GameEntities.PowerUps;
using Events;

namespace GameEntities
{
    public class EffectManager : MonoBehaviour
    {
        Timer _freezerEffectTimer;

        public static EffectManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(this);
        }

        void Start()
        {
            FreezerPowerUp.PickedUp += HandleFreezerPowerUpPickedUp;
            _freezerEffectTimer = gameObject.AddComponent<Timer>();
        }

        private void HandleFreezerPowerUpPickedUp(FreezerPowerUpPickedUpEventInfo obj) => _freezerEffectTimer.StartTimer(obj.EffectDuration);

        public bool IsFreezerEffectActivated => _freezerEffectTimer.IsRunning;
    }
}