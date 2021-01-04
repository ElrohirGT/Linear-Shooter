using UnityEngine;
using GameEntities.Bullets;
using GameEntities.Ships.Guns.Inputs;
using Utilities;

namespace GameEntities.Ships.Guns
{
    /// <summary>
    /// Represents an entity that can shoot bullets.
    /// </summary>
    public abstract class ShipGun<T> : MonoBehaviour, IShipGun<T> where T : Bullet
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */
        /// <summary>
        /// Manages the states of this gun.
        /// </summary>
        StateMachine _stateMachine;
        /// <summary>
        /// Get's whether it can shoot or not.
        /// </summary>
        bool _canShoot = false;
        /// <summary>
        /// Get's the timer that controls how often the player can shoot.
        /// </summary>
        Timer _shootCooldownTimer;
        /// <summary>
        /// Get's all the settings this gun runs on.
        /// </summary>
        ShipGunSettings _shipGunSettings;
        /// <summary>
        /// Manages the input to know when to shoot.
        /// </summary>
        IShipGunInput _shipGunInput;

        readonly string _bulletTypeName = typeof(T).Name;
        public string BulletTypeName => _bulletTypeName;

        protected ShipGunSettings ShipGunSettings => _shipGunSettings;

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        #region Unity
        void Start()
        {
            _shootCooldownTimer = gameObject.AddComponent<Timer>();
            _shootCooldownTimer.Finished += HandleShootCooldownTimerFinished;
            _canShoot = true;
        }
        /// <summary>
        /// Shoots a bullet with it's predefined motion in the direction the ship is facing.
        /// </summary>
        void FixedUpdate()
        {
            _stateMachine.Tick();

            if (!_canShoot || !_shipGunInput.Shoot)
                return;

            _canShoot = false;
            ConfigureBulletAndStartBullet(GetBulletFromPool());
            _shootCooldownTimer.StartTimer(_shipGunSettings.ShootCooldownDuration);
        }
        /// <summary>
        /// Get's the bullet that this entity shoots from the specific pool.
        /// </summary>
        /// <returns>The bullet that this entity will shoot.</returns>
        protected abstract T GetBulletFromPool();
        #endregion

        /// <summary>
        /// Initializes the ShipGun with the specified <paramref name="shipGunInput"/>, <paramref name="shipGunSettings"/>
        /// and the <paramref name="stateMachine"/>.
        /// </summary>
        /// <param name="shipGunInput">The ship gun input that will be used.</param>
        /// <param name="shipGunSettings">The settings of this ship gun.</param>
        /// <param name="stateMachine">The state machine this ship gun will use.</param>
        public void Initialize(IShipGunInput shipGunInput, ShipGunSettings shipGunSettings, StateMachine stateMachine)
        {
            _shipGunSettings = shipGunSettings;
            _shipGunInput = shipGunInput;
            _stateMachine = stateMachine;
        }

        /// <summary>
        /// Configures the bullet and starts it's move method.
        /// Some guns may want to configure bullets differently, they can override this method.
        /// </summary>
        protected virtual void ConfigureBulletAndStartBullet(T bulletToShoot)
        {
            //Set's the position and rotation
            bulletToShoot.transform.position = transform.position;
            bulletToShoot.transform.rotation = transform.rotation;

            bulletToShoot.Initialize(_shipGunSettings.BulletInitialImpulseMagnitud, false);
        }
        /// <summary>
        /// Enables the entity to shoot again.
        /// </summary>
        void HandleShootCooldownTimerFinished() => _canShoot = true;
    }
}