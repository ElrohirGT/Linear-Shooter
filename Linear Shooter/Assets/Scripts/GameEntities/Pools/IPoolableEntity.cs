using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEntities.Pools
{
    /// <summary>
    /// Provides the requirements for an Entity to be pooled.
    /// </summary>
    public interface IPoolableEntity
    {
        /// <summary>
        /// Returns this entity to the corresponding pool.
        /// </summary>
        void ReturnToPool();

        /// <summary>
        /// Reset's the entity state in order to be reused.
        /// This should only be called by the pool this entity belongs.
        /// </summary>
        void ResetEntity();

    }
}