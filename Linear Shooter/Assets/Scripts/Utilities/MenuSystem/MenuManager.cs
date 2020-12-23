using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace Utilities.MenuSystem
{
    /// <summary>
    /// Manages the transition between menus in the game.
    /// </summary>
    public static class MenuManager
    {
        /// <summary>
        /// Get's wether the <c>MenuManager</c> has or hasn't been initialized.
        /// </summary>
        static bool _alreadyInitialized = false;

        /// <summary>
        /// A dictionary containing every Menu instance in the game keyed by their name.
        /// </summary>
        static Dictionary<Menus, Menu> _menus = new Dictionary<Menus, Menu>();

        /// <summary>
        /// Get's the current menu. By default is -1 because none of the values in the enum has a -1,
        /// it's like setting it to null.
        /// </summary>
        static Menus _currentMenu = (Menus)(-1);

        /// <summary>
        /// Fills the dictionary of menus using reflection
        /// and changes the alreadyInitialized property to true.
        /// </summary>
        public static void Initialize()
        {
            if (_alreadyInitialized)
                return;

            ForceInitialize();
        }

        /// <summary>
        /// Forces the initialization, possibly replacing previous values and references.
        /// </summary>
        public static void ForceInitialize()
        {
            _alreadyInitialized = true;

            Type menusType = typeof(Menus);
            IEnumerable<Type> possibleTypes = menusType.Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Menu)));
            _menus = new Dictionary<Menus, Menu>();

            foreach (string menuName in Enum.GetNames(menusType))
            {
                bool foundOneMenu = false;
                Menus menuEnumValue = (Menus)Enum.Parse(menusType, menuName);
                foreach (var type in possibleTypes)
                {
                    if (type.Name.Equals(menuName))
                    {
                        foundOneMenu = true;
                        ConstructorInfo[] ctors = type.GetConstructors();

                        _menus.Add(menuEnumValue, (Menu)ctors[0].Invoke(new object[] { menuName }));
                        break;
                    }
                }
                if (!foundOneMenu)
                    _menus.Add(menuEnumValue, new Menu(menuName));
            }
        }

        /// <summary>
        /// Goes to the menu supplied in the parameter.
        /// </summary>
        public static void GoToMenu(Menus menuToGo)
        {
            if (_menus.TryGetValue(_currentMenu, out Menu currentMenuInstance))
                currentMenuInstance.LeaveMenu();

            _currentMenu = menuToGo;
            _menus[_currentMenu].EnterMenu();
        }

        /// <summary>
        /// Exits the current menu and loads the scene with the supplied name.
        /// </summary>
        /// <param name="sceneName">The scene to load</param>
        public static void ExitMenuAndLoadScene(string sceneName)
        {
            if (_menus.TryGetValue(_currentMenu, out Menu currentMenuInstance))
                currentMenuInstance.LeaveMenu();

            _currentMenu = (Menus)(-1);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}