using System;

namespace Utilities
{
    public interface IState
    {
        /// <summary>
        /// An event that fires when this state is entered.
        /// </summary>
        event Action Entered;
        /// <summary>
        /// An event that fires when this state is exited.
        /// </summary>
        event Action Exited;

        /// <summary>
        /// Defines what this state does in one frame.
        /// </summary>
        void Tick();

        /// <summary>
        /// Defines what to do when this state is entered.
        /// Used for transitions.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Defines what this state does when exiting itself.
        /// Used for transitions.
        /// </summary>
        void OnExit();
    }
}
