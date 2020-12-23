using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.MenuSystem
{
    /// <summary>
    /// Implements the Pause menu.
    /// </summary>
    public class PauseMenu : Menu
    {

        /// <summary>
        /// Implements the Pause menu.
        /// </summary>
        public PauseMenu(string sceneName) : base(sceneName) { }

        /// <summary>
        /// Defines what to do when the scene is changing to this menu.
        /// </summary>
        public override void EnterMenu()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Time.timeScale = 0;
        }

        /// <summary>
        /// Defines what to do when the scene is changing from this menu to another menu.
        /// </summary>
        public override void LeaveMenu()
        {
            base.LeaveMenu();
            Time.timeScale = 1;
        }

    }
}