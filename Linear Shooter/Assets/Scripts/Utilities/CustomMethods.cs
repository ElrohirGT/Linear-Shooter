using System;
using UnityEngine;
using Utilities.Constants;

namespace Utilities
{
    public static class CustomMethods
    {
        /// <summary>
        /// Plays the given animation from the animator, it uses an int so it's slighty more eficcient.
        /// </summary>
        /// <param name="animator">The animator that controls the animation of the gameobject.</param>
        /// <param name="animationName">The state name of the animation controller.</param>
        public static void PlayAnimation(Animator animator, string animationName)
        {
            int hash = Animator.StringToHash(animationName);
            animator.Play(hash);
        }

        /// <summary>
        /// Makes the <paramref name="transform"/> look at the <paramref name="point"/> ignoring z.
        /// </summary>
        /// <param name="transform">The transform to rotate.</param>
        /// <param name="point">The point to look at (in worldpoints).</param>
        public static void LookAt2D(Transform transform, Vector3 point)
        {
            point.z = transform.position.z;
            transform.up = point - transform.position;
        }
        /// <summary>
        /// Calculates the rotation input that an AI ship must give.
        /// Uses a method found on: <see href="https://stackoverflow.com/a/14807604/10812984"/>
        /// </summary>
        /// <param name="entityTransform">The ship transform.</param>
        /// <param name="target">The target position wee need to rotate to.</param>
        /// <param name="delta">The margin of error, the lower the value the more precise the rotation will be.</param>
        /// <returns>0 for no rotation needed, or either a -1 o 1 depending on the rotation needed.</returns>
        public static float CalculateRotationInput(Transform entityTransform, Vector3 target, float delta = 0.3f)
        {
            float rotationInput;

            float v = (entityTransform.position.x - target.x) * entityTransform.up.y;
            float x = (entityTransform.position.y - target.y) * entityTransform.up.x;

            if (NearlyEqual(v, x, delta))
                rotationInput = 0;
            else if (v < x)
                rotationInput = 1;
            else
                rotationInput = -1;

            return rotationInput;
        }

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
