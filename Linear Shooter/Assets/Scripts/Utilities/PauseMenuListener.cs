using System.Collections;
using UnityEngine;
using Utilities.Constants;
using Utilities.MenuSystem;

namespace Utilities
{
    public class PauseMenuListener : MonoBehaviour
    {
        bool _canReceiveInput = true;
        bool _isInPause = false;

        void Awake() => MenuManager.Initialize();

        void Update()
        {
            if (_canReceiveInput && Input.GetAxisRaw(InputAxisConstants.PAUSE) != 0)
                PauseUnPause();
        }

        void PauseUnPause()
        {
            _canReceiveInput = false;

            if (_isInPause)
                MenuManager.ExitCurrentMenu();
            else
                MenuManager.GoToMenu(Menus.PauseMenu);

            _isInPause = !_isInPause;

            StartCoroutine(BlockInput());
        }

        IEnumerator BlockInput()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            _canReceiveInput = true;
        }
    }
}
