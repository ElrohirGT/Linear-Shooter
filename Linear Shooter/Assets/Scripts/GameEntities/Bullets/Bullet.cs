using UnityEngine;
using Utilities;
using Utilities.Configuration;
using GameEntities.Pools;
using Utilities.Constants;


namespace GameEntities.Bullets
{
    /// <summary>
    /// Represents a bullet entity inside the game.
    /// </summary>
    public abstract class Bullet : MonoBehaviour, IPoolableEntity
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */
        #region Bullet Life
        /// <summary>
        /// Get's the rigidbody of this bullet.
        /// </summary>
        Rigidbody2D _bulletRigidbody;
        /// <summary>
        /// Controls the time the bullet is alive.
        /// </summary>
        Timer _bulletLifeTimer;
        /// <summary>
        /// Get's the duration the bullet will be alive.
        /// </summary>
        float _bulletLifeDuration;
        #endregion

        #region Bullet Movement
        /// <summary>
        /// Get's how many distance the bullet moves per second.
        /// </summary>
        private float _distanceToMove;
        /// <summary>
        /// Get's the target to wich the bullet is moving.
        /// </summary>
        private Vector2 _targetPoint = new Vector2();
        /// <summary>
        /// Get's whether the target should change each frame or not.
        /// </summary>
        private bool _changeTargetPoint = false;
        #endregion

        RaycastHit2D[] raycastHits = new RaycastHit2D[5];

        /// <summary>
        /// Get's the damage that this bullet does to an entity. This can be changed so it's a custom damage.
        /// </summary>
        public float Damage { get; set; }

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        #region Unity
        /// <summary>
        /// Setups the bullet.
        /// </summary>
        protected void Awake()
        {
            ConfigurationUtils.Initialize();

            //Cache rigidbody
            _bulletRigidbody = GetComponent<Rigidbody2D>();

            GetBulletConfigurations();

            //Set-up life timer
            _bulletLifeTimer = gameObject.AddComponent<Timer>();
            _bulletLifeTimer.Finished += HandleBulletLifeTimerFinished;
        }
        /// <summary>
        ///Starts the life timer everytime this entity is gotten from it's pool.
        /// </summary>
        void OnEnable() => _bulletLifeTimer.StartTimer(_bulletLifeDuration);
        /// <summary>
        /// Moves the bullet.
        /// </summary>
        protected virtual void FixedUpdate() => Move();
        /// <summary>
        /// Handles the movement of the bullet, this method is called every fixedUpdate frame.
        /// </summary>
        void Move()
        {
            if (_distanceToMove != 0)
                _bulletRigidbody.MovePosition(Vector2.MoveTowards(transform.position, _targetPoint, _distanceToMove * Time.fixedDeltaTime));

            if (_changeTargetPoint)
                GetTargetPoint();
        }
        #endregion

        #region API
        /// <summary>
        /// Initializes the bullet with the corresponding distance to move and
        /// whether or not it should change it's target point every frame.
        /// </summary>
        /// <param name="distanceToMove">The distance the bullet moves every second.</param>
        /// <param name="changeTargetPoint">Wether the bullet should change it's target point or not. Useful for when a bullet should follow a target.</param>
        public void Initialize(float distanceToMove, bool changeTargetPoint)
        {
            GetTargetPoint();
            _changeTargetPoint = changeTargetPoint;
            _distanceToMove = distanceToMove;
        }
        /// <summary>
        /// Returns the bullet to the correct pool,
        /// each type of bullet has it's own pool, so this method is abstract.
        /// </summary>
        public abstract void ReturnToPool();
        #endregion

        /// <summary>
        /// Get's the configurations for this bullet.
        /// </summary>
        void GetBulletConfigurations() => (_bulletLifeDuration, Damage) = GetBulletLifeAndDamage();
        /// <summary>
        /// Get's this bullet life duration and damage.
        /// </summary>
        /// <returns>The bullet life duration and damage that does to an entity that touches.</returns>
        protected abstract (float bulletLifeDuration, float damage) GetBulletLifeAndDamage();
        /// <summary>
        /// Get's a new target point for this bullet, by default this method just set's the target point to
        /// a point outside the world in the direction the bullet is facing.
        /// Override this method if you want the bullet to behave different (like follow something for example).
        /// </summary>
        protected virtual void GetTargetPoint()
        {
            /*FIXME When the ship is all the way to the top of the screen bullets don't shoot the correct distance,
             * sometimes noe even the correct direction.
             */
            Vector3 transformUp = transform.up;
            //source: https://stackoverflow.com/questions/63034454/unity-get-point-on-edge-of-the-screen-that-object-directed-to
            Ray ray = new Ray(transform.position, transformUp);

            float currentMinDistance = float.MaxValue;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(ScreenUtils.MainCamera);

            //The first 4 points are the left, right, down and up parts of the plane
            for (var i = 0; i < 4; i++)
            {
                // Raycast against the plane
                if (planes[i].Raycast(ray, out float distance))
                {
                    // Since a plane is mathematical infinite
                    // what you would want is the one that hits with the shortest ray distance
                    if (distance < currentMinDistance)
                        _targetPoint = ray.GetPoint(distance);
                }
            }

            //add or substract 1 because the bullet needs to have a point outside the world.
            _targetPoint *= 1.05f;
        }
        /// <summary>
        /// Resets the entity to be used again,
        /// some bullets may need extra things done to reset them, they can expand this method.
        /// </summary>
        public virtual void ResetEntity()
        {
            GetBulletConfigurations();
            _bulletRigidbody.velocity = Vector2.zero;
            _bulletRigidbody.angularVelocity = 0;
            _bulletRigidbody.rotation = 0;
            _bulletLifeTimer.ResetTimer();
        }
        /// <summary>
        /// Handles the Life timer finished event.
        /// </summary>
        void HandleBulletLifeTimerFinished() => ReturnToPool();
    }
}