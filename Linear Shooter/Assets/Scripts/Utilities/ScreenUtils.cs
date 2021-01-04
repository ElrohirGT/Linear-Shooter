using System;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// Gives information about the screen to the whole application.
    /// </summary>
    public static class ScreenUtils
    {

        #region Fields
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */

        /// <summary>
        /// Get's wether the <c>ScreenUtils</c> has or hasn't been initialized.
        /// </summary>
        private static bool _alreadyInitialized = false;

        /// <summary>
        /// Saves the previous width of the screen.
        /// </summary>
        private static float _previousScreenWidth;

        /// <summary>
        /// Saves the previous height of the screen.
        /// </summary>
        private static float _previousScreenHeight;

        //Used for cache
        static Camera _mainCamera;
        static Vector2 _worldBottomLeftCorner = new Vector2();
        static Vector2 _worldBottomRightCorner = new Vector2();
        static Vector2 _worldUpperLeftCorner = new Vector2();
        static Vector2 _worldUpperRightCorner = new Vector2();

        #endregion

        /*
        .#####...#####....####...#####...######..#####...######..######..######...####..
        .##..##..##..##..##..##..##..##..##......##..##....##......##....##......##.....
        .#####...#####...##..##..#####...####....#####.....##......##....####.....####..
        .##......##..##..##..##..##......##......##..##....##......##....##..........##.
        .##......##..##...####...##......######..##..##....##....######..######...####..
        ................................................................................
        */
        /// <summary>
        /// Get's the main camera.
        /// </summary>
        public static Camera MainCamera => _mainCamera;

        #region World Dimensions
        /// <summary>
        /// Get's the world width. Equal to calling WorldRight-WorldLeft.
        /// </summary>
        public static float WorldWidth => WorldRight - WorldLeft;//WorldLeft is negative, so "-" changes it's sign.
        /// <summary>
        /// Get's the world height. Equal to calling WorldTop-WorldBottom.
        /// </summary>
        public static float WorldHeight => WorldTop - WorldBottom;//WorldBottom is negative, so "-" changes it's sign.
        /// <summary>
        /// Returns the distance a diagonal of that passes through the center of the screen would return.
        /// </summary>
        public static float WorldMaxDistance => WorldTopRightCorner.magnitude * 2;
        #endregion

        #region World Points
        /// <summary>
        /// Get's the left-most point in the world.
        /// </summary>
        public static float WorldLeft => WorldBottomLeftCorner.x;

        /// <summary>
        /// Get's the right-most point in the world.
        /// </summary>
        public static float WorldRight => WorldBottomRightCorner.x;

        /// <summary>
        /// Get's the top-most point in the world.
        /// </summary>
        public static float WorldTop => WorldTopLeftCorner.y;

        /// <summary>
        /// Get's the bottom-most point in the world.
        /// </summary>
        public static float WorldBottom => WorldBottomRightCorner.y;
        #endregion

        #region World Corners
        /// <summary>
        /// Get's the bottom left corner of the world.
        /// </summary>
        public static Vector2 WorldBottomLeftCorner
        {
            get
            {
                CheckIfScreenSizeChanged();
                return _worldBottomLeftCorner;
            }
        }

        /// <summary>
        /// Get's the bottom right corner of the world.
        /// </summary>
        public static Vector2 WorldBottomRightCorner
        {
            get
            {
                CheckIfScreenSizeChanged();
                return _worldBottomRightCorner;
            }
        }

        /// <summary>
        /// Get's the upper left corner of the world.
        /// </summary>
        public static Vector2 WorldTopLeftCorner
        {
            get
            {
                CheckIfScreenSizeChanged();
                return _worldUpperLeftCorner;
            }
        }

        /// <summary>
        /// Get's the upper right corner of the world.
        /// </summary>
        public static Vector2 WorldTopRightCorner
        {
            get
            {
                CheckIfScreenSizeChanged();
                return _worldUpperRightCorner;
            }
        }

        #endregion

        /// <summary>
        /// Fires when the resolution has been changed.
        /// </summary>
        public static event Action ScreenSizeChanged;

        /* 
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */

        /// <summary>
        /// Forces the initialization, possibly replacing previous values and references.
        /// </summary>
        public static void ForceInitialize()
        {
            _alreadyInitialized = true;
            _mainCamera = Camera.main;
            CalculateWorldPoints();
        }

        /// <summary>
        /// Initializes the static class, previous to the call of this function
        /// all properties had the default value.
        /// </summary>
        public static void Initialize()
        {
            if (_alreadyInitialized)
                return;

            ForceInitialize();
        }

        /// <summary>
        /// Checks if the screen has changed size, and if it has recalculates all the properties.
        /// </summary>
        static void CheckIfScreenSizeChanged()
        {
            if (Screen.width != _previousScreenWidth || Screen.height != _previousScreenHeight)
                OnScreenSizeChanged();
        }

        /// <summary>
        /// Fires the screen size changed event after re-calculating the world points.
        /// </summary>
        static void OnScreenSizeChanged()
        {
            CalculateWorldPoints();
            ScreenSizeChanged();
        }

        /// <summary>
        /// This method should only be called if the screen size has changed.
        /// This method set's the WorldPoints to their new values.
        /// </summary>
        static void CalculateWorldPoints()
        {
            _previousScreenWidth = Screen.width;
            _previousScreenHeight = Screen.height;

            float cameraZ = -_mainCamera.transform.position.z;

            Vector3 worldBottomLeftCorner =
                _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, cameraZ));

            Vector3 worldUpperRightCorner =
                _mainCamera.ScreenToWorldPoint(new Vector3(_previousScreenWidth, _previousScreenHeight, cameraZ));

            _worldBottomLeftCorner = worldBottomLeftCorner;
            _worldUpperRightCorner = worldUpperRightCorner;

            _worldUpperLeftCorner = new Vector3(worldBottomLeftCorner.x, worldUpperRightCorner.y, cameraZ);
            _worldBottomRightCorner = new Vector3(worldUpperRightCorner.x, worldBottomLeftCorner.y, cameraZ);

        }
    }
}