using System;
using UnityEngine;

namespace GameEntities.Pools
{
    public interface IBasePool<out T> where T : Component, IPoolableEntity
    {
        string PooledEntityTypeName { get; }

        T Get();
    }
}