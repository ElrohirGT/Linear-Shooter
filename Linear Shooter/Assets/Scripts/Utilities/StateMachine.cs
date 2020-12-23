using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    /// Used to manage states and their transitions.
    /// </summary>
    public class StateMachine
    {
        /*
        .######..######..######..##......#####....####..
        .##........##....##......##......##..##..##.....
        .####......##....####....##......##..##...####..
        .##........##....##......##......##..##......##.
        .##......######..######..######..#####....####..
        ................................................
        */
        /// <summary>
        /// Gets the current state of the machine.
        /// </summary>
        IState _currentState;

        /// <summary>
        /// A dictionary that contains all the registered transitions until this moment.
        /// </summary>
        Dictionary<Type, List<StateTransition>> _transitions = new Dictionary<Type, List<StateTransition>>();

        /// <summary>
        /// A list of all the possible transitions that the current state has.
        /// </summary>
        List<StateTransition> _currentStateTransitions = new List<StateTransition>();

        /// <summary>
        /// A list of all transitions that don't require any previous state.
        /// </summary>
        List<StateTransition> _anyStateTransitions = new List<StateTransition>();

        /// <summary>
        /// Used to cache the list so we don't need to call <c>List.Clear()</c>, 
        /// <a href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.clear?view=net-5.0#remarks">an O(n) operation.</a>
        /// </summary>
        readonly List<StateTransition> EMPTY_TRANSITIONS = new List<StateTransition>();

        public IState CurrentState => _currentState;
        public IReadOnlyCollection<StateTransition> CurrentStateTransitions => _currentStateTransitions.AsReadOnly();
        public IReadOnlyCollection<StateTransition> AnyStateTransitions => _anyStateTransitions.AsReadOnly();

        /*
        .##...##..######..######..##..##...####...#####....####..
        .###.###..##........##....##..##..##..##..##..##..##.....
        .##.#.##..####......##....######..##..##..##..##...####..
        .##...##..##........##....##..##..##..##..##..##......##.
        .##...##..######....##....##..##...####...#####....####..
        .........................................................
        */
        /// <summary>
        /// Initializes the state machine with the initial state of this machine.
        /// </summary>
        /// <param name="initialState">The initial state of this machine.</param>
        public StateMachine(IState initialState) => ChangeCurrentState(initialState);

        /// <summary>
        /// Checks if there is a valid transition, and ticks the current state.
        /// </summary>
        public void Tick()
        {
            if (TryGettingTransition(out StateTransition activeTransition))
                ChangeCurrentState(activeTransition.ToState);

            _currentState.Tick();
        }

        /// <summary>
        /// Changes the current state to the one provided.
        /// </summary>
        /// <param name="state">The state to change to.</param>
        void ChangeCurrentState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state), "You can't change to a null state!");

            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            if (!_transitions.TryGetValue(_currentState.GetType(), out _currentStateTransitions))
                _currentStateTransitions = EMPTY_TRANSITIONS;

            _currentState.OnEnter();

        }

        /// <summary>
        /// Adds a transition between two states.
        /// </summary>
        /// <param name="fromState">The initial state.</param>
        /// <param name="toState">The state to go to.</param>
        /// <param name="condition">The condition to change state.</param>
        public void AddTransition(IState fromState, IState toState, Func<bool> condition)
        {
            if (fromState == null)
                throw new ArgumentNullException(nameof(fromState), "Can't move from a null state to another!");
            if (toState == null)
                throw new ArgumentNullException(nameof(toState), "Can't move to a null state!");

            Type fromType = fromState.GetType();

            if (!_transitions.TryGetValue(fromType, out _))
                _transitions[fromType] = new List<StateTransition>();

            if (fromState.Equals(_currentState))
                _currentStateTransitions.Add(new StateTransition(toState, condition));
            else
                _transitions[fromType].Add(new StateTransition(toState, condition));
        }

        /// <summary>
        /// Adds a transition that doesn't needs to have a previous state.
        /// </summary>
        /// <param name="state">The state to go to.</param>
        /// <param name="condition">The condition to change this state.</param>
        public void AddAnyTransition(IState state, Func<bool> condition) =>
            _anyStateTransitions.Add(new StateTransition(state, condition));

        /// <summary>
        /// Tries getting a transition that is valid.
        /// </summary>
        /// <param name="validTransition">The transition whose condition returned true. Null otherwise.</param>
        /// <returns>True if a transition was found, false otherwise.</returns>
        bool TryGettingTransition(out StateTransition validTransition)
        {
            validTransition = null;

            foreach (var anyTransition in _anyStateTransitions)
            {
                if (anyTransition.Condition())
                {
                    validTransition = anyTransition;
                    return true;
                }
            }

            foreach (var currentTransition in _currentStateTransitions)
            {
                if (currentTransition.Condition())
                {
                    validTransition = currentTransition;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Represents the transition to a certain state.
        /// </summary>
        public class StateTransition
        {
            /// <summary>
            /// Get's the state to transition to.
            /// </summary>
            public IState ToState { get; }
            /// <summary>
            /// Get's the condition that needs to be true to transition to the state.
            /// </summary>
            public Func<bool> Condition { get; }

            /// <summary>
            /// Creates a transition to a certain state.
            /// </summary>
            /// <param name="toState">The state to transition to.</param>
            /// <param name="condition">The condition that determines whether or not to transition.</param>
            public StateTransition(IState toState, Func<bool> condition)
            {
                Condition = condition;
                ToState = toState;
            }
        }
    }
}
