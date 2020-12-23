using UnityEngine;
using Utilities;
using Utilities.Configuration;
using GameEntities.Pools;
using System.Collections;

namespace GameEntities
{
    public class Medal : MonoBehaviour, IPoolableEntity
    {
        Timer _aliveTimer;
        float _aliveTime;

        FadesAway _fadesAwayComponent;

        void Awake()
        {

            _aliveTimer = gameObject.AddComponent<Timer>();
            _aliveTimer.Finished += HandleAliveTimerFinished;

            _fadesAwayComponent = gameObject.AddComponent<FadesAway>().Initalize(GetComponent<SpriteRenderer>(), _aliveTimer);
        }

        void OnEnable()
        {
            _aliveTime = Random.Range(ConfigurationUtils.MedalsConfig.MinLifeDuration, ConfigurationUtils.MedalsConfig.MaxLifeDuration);
            _aliveTimer.StartTimer(_aliveTime);
        }

        private void HandleAliveTimerFinished() => ReturnToPool();

        public void ResetEntity()
        {
            _aliveTimer.ResetTimer();
            _fadesAwayComponent.ResetColor();
        }

        public void ReturnToPool() => MedalPool.Instance.ReturnToPool(this);
    }
}
