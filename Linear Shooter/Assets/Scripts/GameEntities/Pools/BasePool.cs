using System.Collections.Generic;
using UnityEngine;

namespace GameEntities.Pools
{
    /// <summary>
    /// Serves as the base class for all the pools of the game.
    /// </summary>
    public abstract class BasePool<T> : MonoBehaviour, IBasePool<T> where T : Component, IPoolableEntity
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */
        #region Fields
        [SerializeField]
        /// <summary>
        /// The initial amount of objects this pool will start with
        /// </summary>
        protected int initialObjectCount = 5;
        /// <summary>
        /// The prefab to pool, it can be a component attached to the prefab.
        /// </summary>
        [SerializeField]
        T prefab;
        /// <summary>
        /// A list of all the gameObjects that will be pooled.
        /// </summary>
        readonly Queue<T> _objects = new Queue<T>();
        /// <summary>
        /// The type name of the entity that is being pooled.
        /// </summary>
        string _pooledEntityTypeName;
        #endregion

        public static BasePool<T> Instance { get; private set; }
        public string PooledEntityTypeName => _pooledEntityTypeName;

        /*
         .##...##..######..######..##..##...####...#####....####..
         .###.###..##........##....##..##..##..##..##..##..##.....
         .##.#.##..####......##....######..##..##..##..##...####..
         .##...##..##........##....##..##..##..##..##..##......##.
         .##...##..######....##....##..##...####...#####....####..
         .........................................................
        */

        #region Unity
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
                return;
            }
            Destroy(gameObject);
        }
        #endregion


        #region API
        /// <summary>
        /// Get's an object form the pool.
        /// </summary>
        public T Get()
        {
            if (_objects.Count == 0)
                ExtendPool(1);

            T component = _objects.Dequeue();
            component.gameObject.SetActive(true);
            return component;
        }

        /// <summary>
        /// Returns the given object to the pull.
        /// </summary>
        /// <param name="component">The object to be returned.</param>
        public void ReturnToPool(T component)
        {
            component.ResetEntity();
            component.gameObject.SetActive(false);
            _objects.Enqueue(component);
        }
        #endregion

        #region Privates
        /// <summary>
        /// Initializes all lists and keys in the GameObjectPool.
        /// </summary>
        void Initialize()
        {
            _pooledEntityTypeName = prefab.GetType().Name;
            ExtendPool(initialObjectCount);
        }

        /// <summary>
        /// Extends the pool by the given amount. <paramref name="extendQuantity"/> must be positive.
        /// </summary>
        /// <param name="extendQuantity">The amount to extend the pool, must be positive.</param>
        void ExtendPool(int extendQuantity)
        {
            if (extendQuantity <= 0)
                return;

            for (int i = 0; i < extendQuantity; i++)
            {
                T gameOb = Instantiate(prefab);
                gameOb.transform.SetParent(gameObject.transform, false);
                ReturnToPool(gameOb);
            }
        }

        public override string ToString() => $"{{Pool for: {_pooledEntityTypeName}. C:{_objects.Count}}}";
        #endregion
    }
}