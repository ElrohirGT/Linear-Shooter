using System;
using UnityEngine;
using Utilities.Constants;

namespace Utilities
{
    public static class CustomMethods
    {
        /// <summary>
        /// Checks if the two numbers are nearly equals. Found on:
        /// <see href="https://stackoverflow.com/a/3875619/10812984"/>
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <param name="epsilon">The error margin.</param>
        /// <returns>True if they are nearle equal, false otherwise.</returns>
        public static bool NearlyEqual(double a, double b, double epsilon)
        {
            const double MinNormal = 2.2250738585072014E-308d;
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (a.Equals(b))
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || absA + absB < MinNormal)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * MinNormal);
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }
        /// <summary>
        /// Checks if there are any colliders that collide with the player in
        /// a circle that has a center in <paramref name="position"/> and a radius equal to <paramref name="radius"/>.
        /// </summary>
        /// <param name="position">The position the center of the circle will be.</param>
        /// <param name="radius">The radius of the circle to check.</param>
        /// <param name="maxCollidersToCheck">The quantity of colliders that will be returned max.</param>
        /// <returns>True if a collider that collides with the player was found, false otherwise.</returns>
        public static bool CollidesWithPlayer(Vector2 position, float radius, int maxCollidersToCheck = 10)
        {
            Collider2D[] colliders = new Collider2D[maxCollidersToCheck];

            if (Physics2D.OverlapCircleNonAlloc(position, radius, colliders) == 0)
                return false;

            foreach (var collider in colliders)
                if (collider != null && collider.gameObject.CompareTag(TagsConstants.PLAYER))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true or false randomly every time is called.
        /// </summary>
        public static bool RandomBoolean() => UnityEngine.Random.Range(0, 2) == 0;
        /// <summary>
        /// Checks if the given <paramref name="generic"/> is a parent of the type <paramref name="toCheck"/>.
        /// Found on: <see href="https://stackoverflow.com/a/457708/10812984"/>.
        /// <example> For example:
        ///     <code>
        ///         CustomMethods.IsSubclassOfRawGeneric(typeof(BasePool<>), type)
        ///     </code>
        /// The generic must not have anything supplied in the <c><></c>.
        /// </example>
        /// </summary>
        /// <param name="generic">The generic type that is supposedly inherited.</param>
        /// <param name="toCheck">The type that inherits the <paramref name="generic"/>.</param>
        /// <returns>True if <paramref name="toCheck"/> is indeed a subclass of <paramref name="generic"/>, false otherwise.</returns>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
