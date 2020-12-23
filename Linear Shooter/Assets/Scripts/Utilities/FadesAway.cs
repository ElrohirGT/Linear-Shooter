using System;
using UnityEngine;

namespace Utilities
{
    public class FadesAway : MonoBehaviour
    {
        SpriteRenderer _spriteRenderer;
        Timer _lifeTimer;

        //Cache
        Color _cacheColor;

        public FadesAway Initalize(SpriteRenderer spriteRenderer, Timer timer)
        {
            _spriteRenderer = spriteRenderer;
            _lifeTimer = timer;
            return this;
        }

        public void ResetColor()
        {
            _cacheColor = _spriteRenderer.color;
            _cacheColor.a = 1f;
            _spriteRenderer.color = _cacheColor;
        }

        void Update()
        {
            if (_lifeTimer.IsRunning)
                Fade();
        }

        private void Fade()
        {
            _cacheColor = _spriteRenderer.color;
            _cacheColor.a = _lifeTimer.RemainingSeconds / _lifeTimer.Duration;
            _spriteRenderer.color = _cacheColor;
        }
    }
}