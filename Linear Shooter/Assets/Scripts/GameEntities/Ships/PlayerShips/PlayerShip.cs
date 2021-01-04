#define UNITY_ANDROID
#define UNITY_IOS
#define UNITY_64

using UnityEngine;
using GameEntities.Bullets;
using Utilities.Constants;
using GameEntities.Ships.Motors.Inputs;
using GameEntities.Ships.Guns.Inputs;
using Utilities;
using GameEntities.Ships.Guns.States;
using GameEntities.Ships.Guns;
using GameEntities.Ships.Motors.States;
using System;
using System.Collections;
using Events;
using GameEntities.PowerUps;

namespace GameEntities.Ships.PlayerShips
{
    public abstract class PlayerShip : Ship
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */

        #region DamageCooldown Effect support
        SpriteRenderer _spriteRenderer;
        Color _originalShipColor;
        Color _shipColorBeforeTakenDamageWithHighAlpha;
        #endregion

        #region Ship Ultimate Support
        int _medalsCollected = 0;
        int _minMedalsToUltimate;
        bool _canShootUltimate = false;
        #endregion

        #region Ship Gun
        [SerializeField] Transform _shipGunDisplayTransform;
        readonly IShipGunInput _shipGunInput = new PlayerShipGunInput();
        StateMachine _shipGunStateMachine;
        protected ShipGunSettings shipGunSettings;
        IShipGun<Bullet> _shipGun;
        #endregion

        public int MinMedalsToUltimate => _minMedalsToUltimate;

        protected IShipGunInput ShipGunInput => _shipGunInput;

        protected Animator Animator { get; private set; }

        public event Action ShipCanShootUltimate;
        public event Action ShipShootingUltimate;
        public event Action ShipUltimateEnded;
        public event Action<PlayerPickedUpMedalEventInfo> ShipCollectedMedal;

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        #region Unity
        protected override void Awake()
        {
            base.Awake();
            InitializePlayerShip();
            Animator = GetComponent<Animator>();
            ExtraLifePowerUp.PickedUp += HandleExtraLifePowerUpPickedUp;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_canShootUltimate && Input.GetAxis(InputAxisConstants.PLAYER_ULTIMATE) != 0)
                ConfigureAndShootUltimate();
        }
        void ConfigureAndShootUltimate()
        {
            ShipShootingUltimate?.Invoke();
            _canShootUltimate = false;
            _medalsCollected = 0;
            ShootUltimate();
        }
        void Update()
        {
            Vector3 mouseWorldPosition = ScreenUtils.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            CustomMethods.LookAt2D(_shipGunDisplayTransform, mouseWorldPosition);
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagsConstants.ENEMY_BULLET))
            {
                Bullet contactBullet = other.gameObject.GetComponent<Bullet>();
                OnShipTookDamage(contactBullet.Damage);
                contactBullet.ReturnToPool();
                return;
            }
            else if (other.CompareTag(TagsConstants.ENEMY))
                OnShipTookDamage(other.gameObject.GetComponent<AliveEntity>().EntityBaseDamage);
            else if (other.CompareTag(TagsConstants.MEDAL))
            {
                other.GetComponent<Medal>().ReturnToPool();
                CollectMedal();
            }
        }
        void CollectMedal()
        {
            ShipCollectedMedal?.Invoke(new PlayerPickedUpMedalEventInfo(++_medalsCollected));
            if (_medalsCollected == _minMedalsToUltimate)
            {
                _canShootUltimate = true;
                ShipCanShootUltimate?.Invoke();
            }
        }
        void OnDestroy() => ExtraLifePowerUp.PickedUp -= HandleExtraLifePowerUpPickedUp;
        #endregion

        #region Configuration
        void InitializePlayerShip()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _originalShipColor = _spriteRenderer.color;
            SetUpColors();

            DamageCooldownTimerFinished += HandleDamageCooldownTimerFinished;

            _shipGun = CreateShipGun();
            _shipGunStateMachine = new StateMachine(new ListenToPlayerGunInputState(_shipGunInput));

            shipGunSettings = CreateShipGunSettings();
            _shipGun.Initialize(_shipGunInput, shipGunSettings, _shipGunStateMachine);

            _minMedalsToUltimate = GetMinMendalsToUltimate();
            EntityDied += HandleEntityDiedEvent;
        }
        protected override StateMachine CreateShipMotorStateMachine() => new StateMachine(new ListenToPlayerMotorInputState(ShipMotorInput));
        protected override IShipMotorInput CreateMotorInput() => new PlayerMotorInput();
        /// <summary>
        /// Get's how many medals are required to activate the ultimate of this ship.
        /// </summary>
        /// <returns>The minimum amount of medals this ship needs to activate it's ultimate.</returns>
        protected abstract int GetMinMendalsToUltimate();
        /// <summary>
        /// Create the ship gun that this ship will use.
        /// </summary>
        /// <returns>The ship gun.</returns>
        protected abstract IShipGun<Bullet> CreateShipGun();
        /// <summary>
        /// Creates the ship gun settings that the ship gun will use.
        /// </summary>
        /// <returns>The ship gun settings.</returns>
        protected abstract ShipGunSettings CreateShipGunSettings();
        #endregion

        #region Event Handling
        void HandleExtraLifePowerUpPickedUp() => OnEntityHeals(1);
        void HandleDamageCooldownTimerFinished()
        {
            gameObject.layer = (int)Layers.Player;
            _spriteRenderer.color = _originalShipColor;
        }
        void HandleEntityDiedEvent() => Destroy(gameObject);
        #endregion

        #region Own Events
        /// <summary>
        /// This method should be called when the ultimate of the ship has ended.
        /// </summary>
        protected void OnUltimateEnded() => ShipUltimateEnded?.Invoke();
        void OnShipTookDamage(float damage)
        {
            if (!CanReceiveDamage || damage <= 0)
                return;

            StartCoroutine(PlayerGhostMode());
            OnEntityTookDamage(damage);
            StartCoroutine(PlayerFlashCoroutine());
        }
        #endregion

        IEnumerator PlayerGhostMode()
        {
            //waits for a frame in order to disable the ship collider,
            //in this way the enemies can handle the collision with this ship.
            yield return null;
            gameObject.layer = (int)Layers.PlayerGhostmode;
        }

        IEnumerator PlayerFlashCoroutine()
        {
            //Used for controlling the Color.Lerp
            float timeElapsed = 0;
            SetUpColors();
            while (!CanReceiveDamage)
            {
                _spriteRenderer.color = Color.Lerp(_originalShipColor, _shipColorBeforeTakenDamageWithHighAlpha, Mathf.PingPong(timeElapsed / 0.5f, 1));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        void SetUpColors()
        {
            _originalShipColor = _spriteRenderer.color;
            _shipColorBeforeTakenDamageWithHighAlpha = _spriteRenderer.color;
            _shipColorBeforeTakenDamageWithHighAlpha.a = 0.05f;
        }
        /// <summary>
        /// Shoots the ultimate ability of this ship.
        /// </summary>
        protected abstract void ShootUltimate();
    }
}
