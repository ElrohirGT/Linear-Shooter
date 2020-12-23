using UnityEngine;

namespace Utilities
{
    public class MoveInARandomDirection : MonoBehaviour
    {
        Rigidbody2D _rigidbody;
        Vector2 _force;

        private void Awake() => _rigidbody = GetComponent<Rigidbody2D>();

        private void OnEnable() => _rigidbody.AddForce(GetRandomForce(), ForceMode2D.Impulse);

        private Vector2 GetRandomForce()
        {
            _force = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return _force *= Random.Range(-1.5f, 1.5f);
        }

        private void OnDisable() => _rigidbody.AddForce(_force * -1, ForceMode2D.Impulse);
    }
}
