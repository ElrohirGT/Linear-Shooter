using UnityEngine.SceneManagement;

namespace Utilities.MenuSystem
{
    /// <summary>
    /// Defines how the menu enters and leaves the scene.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// Get's the name of the unity scene that contains this menu.
        /// </summary>
        protected string sceneName;

        /// <summary>
        /// Defines how the menu enters and leaves the scene.
        /// </summary>
        public Menu(string sceneName)
        {
            this.sceneName = sceneName;
        }

        /// <summary>
        /// Defines what to do when the scene is changing to this menu.
        /// </summary>
        public virtual void EnterMenu()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        /// <summary>
        /// Defines what to do when the scene is changing from this menu to another menu.
        /// </summary>
        public virtual void LeaveMenu()
        {
            if (SceneManager.GetSceneByName(sceneName) != null)
                SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}