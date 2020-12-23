using System;
using UnityEngine;
using UnityEngine.UI;


namespace GameEntities
{
    [Serializable]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        Slider _slider;
        [SerializeField]
        Image _fill;

        [SerializeField]
        Gradient _gradient;

        float _maxHealth;
        float _currentHealth;

        float HealthPercentage => _currentHealth / _maxHealth;

        public void Initialize(float maxHealth, float currentHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            _slider.minValue = 0;
            UpdateSlider();
        }

        public void HealthChanged(float newCurrentHealth)
        {
            _currentHealth = newCurrentHealth;
            UpdateSlider();
        }

        void UpdateSlider()
        {
            _slider.maxValue = _maxHealth;
            _slider.value = _currentHealth;

            _fill.color = _gradient.Evaluate(HealthPercentage);
        }
    }
}
