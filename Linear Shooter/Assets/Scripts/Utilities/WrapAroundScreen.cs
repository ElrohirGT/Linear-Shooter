using UnityEngine;
using Utilities;
using Utilities.Constants;

namespace Utilities
{
    public class WrapAroundScreen : MonoBehaviour
    {
        void Awake() => ScreenUtils.Initialize();

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(TagsConstants.MAIN_CAMERA))
            {
                Vector3 position = transform.position;

                //The clamp makes it more difficult for the player to go out of bounds when we teleport it to the other side.
                if (position.x >= ScreenUtils.WorldRight || position.x <= ScreenUtils.WorldLeft)
                    position.x = Mathf.Clamp(-1 * position.x, ScreenUtils.WorldLeft, ScreenUtils.WorldRight);

                if (position.y >= ScreenUtils.WorldTop || position.y <= ScreenUtils.WorldBottom)
                    position.y = Mathf.Clamp(-1 * position.y, ScreenUtils.WorldBottom, ScreenUtils.WorldTop);

                transform.position = position;
            }
        }
    }
}
