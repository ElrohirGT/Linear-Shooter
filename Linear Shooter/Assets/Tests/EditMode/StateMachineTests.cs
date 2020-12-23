using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;
using NSubstitute;

namespace Tests
{
    public class StateMachineTests
    {
        protected class TestObj
        {
            public string Name { get; set; }
        }

        protected class MuckEnterState : IState
        {
            readonly TestObj _obj;

            public MuckEnterState(TestObj obj) => _obj = obj;

            public event Action Entered;
            public event Action Exited;

            public void OnEnter() => _obj.Name = GetType().Name;

            public void OnExit() { }

            public void Tick() { }
        }
        protected class MuckExitState : IState
        {
            readonly TestObj _obj;

            public MuckExitState(TestObj obj) => _obj = obj;

            public event Action Entered;
            public event Action Exited;

            public void OnEnter() { }

            public void OnExit() => _obj.Name = GetType().Name;

            public void Tick() { }
        }

        #region Null Testing
        [Test]
        public void Cant_Have_An_Initial_Null_State()
        {
            Assert.That(
                () => new StateMachine(null),
                Throws.TypeOf<ArgumentNullException>()
            );
        }
        [Test]
        public void Cant_Transition_From_A_Null_State()
        {
            var initialState = Substitute.For<IState>();

            StateMachine stateMachine = new StateMachine(initialState);
            Assert.That(
                () => stateMachine.AddTransition(null, initialState, () => false),
                Throws.TypeOf<ArgumentNullException>()
            );
        }
        [Test]
        public void Cant_Transition_To_A_Null_State()
        {
            var initialState = Substitute.For<IState>();

            StateMachine stateMachine = new StateMachine(initialState);
            Assert.That(
                () => stateMachine.AddTransition(initialState, null, () => false),
                Throws.TypeOf<ArgumentNullException>()
            );
        }
        #endregion

        #region State Changes Tests
        [Test]
        public void State_OnEnter_Is_Called()
        {
            var obj = new TestObj();
            var initialState = new MuckEnterState(obj);

            new StateMachine(initialState);
            Assert.AreEqual(initialState.GetType().Name, obj.Name);
        }
        [Test]
        public void State_OnExit_Is_Called()
        {
            var obj = new TestObj();
            var initialState = new MuckExitState(obj);

            StateMachine stateMachine = new StateMachine(initialState);
            stateMachine.AddTransition(initialState, Substitute.For<IState>(), () => true);
            stateMachine.Tick();

            Assert.AreEqual(initialState.GetType().Name, obj.Name);
        }

        [Test]
        public void Change_State()
        {
            var obj = new TestObj();

            var initialState = new MuckEnterState(obj);
            var nextState = new MuckExitState(obj);

            StateMachine stateMachine = new StateMachine(initialState);
            stateMachine.AddTransition(initialState, nextState, () => true);

            stateMachine.Tick();

            Assert.AreEqual(nextState.GetType(), stateMachine.CurrentState);
        }
        #endregion

        #region AddTransition Tests
        [Test]
        public void Adds_Transitions_To_Current_State_Transitions()
        {
            //Arrange
            TestObj obj = new TestObj();
            MuckEnterState initialState = new MuckEnterState(obj);
            List<IState> transitionStates = new List<IState>();

            for (int i = 0; i < 10; i++)
                transitionStates.Add(Substitute.For<IState>());
            StateMachine stateMachine = new StateMachine(initialState);

            //Act
            for (int i = 0; i < transitionStates.Count; i++)
                stateMachine.AddTransition(initialState, transitionStates[i], () => true);

            Assert.AreEqual(transitionStates.Count, stateMachine.CurrentStateTransitions.Count);
        }
        [Test]
        public void Adds_Transitions_To_Inactive_State()
        {
            //Arrange
            TestObj obj = new TestObj();
            IState initialState = Substitute.For<IState>();
            MuckEnterState stateToTestTransitions = new MuckEnterState(obj);

            List<IState> transitionStates = new List<IState>();
            for (int i = 0; i < 10; i++)
                transitionStates.Add(Substitute.For<IState>());

            StateMachine stateMachine = new StateMachine(initialState);

            //Act
            stateMachine.AddTransition(initialState, stateToTestTransitions, () => true);
            for (int i = 0; i < transitionStates.Count; i++)
                stateMachine.AddTransition(stateToTestTransitions, transitionStates[i], () => true);

            Assert.AreEqual(1, stateMachine.CurrentStateTransitions.Count);
            stateMachine.Tick();
            Assert.AreEqual(transitionStates.Count, stateMachine.CurrentStateTransitions.Count);
        }
        #endregion

        #region AddAnyTransition Tests
        [Test]
        public void Adds_An_Any_State_Transition()
        {
            //Arrange
            TestObj obj = new TestObj();
            MuckEnterState initialState = new MuckEnterState(obj);
            List<IState> transitionStates = new List<IState>();

            for (int i = 0; i < 10; i++)
                transitionStates.Add(Substitute.For<IState>());
            StateMachine stateMachine = new StateMachine(initialState);

            //Act
            for (int i = 0; i < transitionStates.Count; i++)
                stateMachine.AddAnyTransition(transitionStates[0], () => true);

            Assert.AreEqual(transitionStates.Count, stateMachine.AnyStateTransitions.Count);
        }

        [Test]
        public void Enters_A_From_Any_State()
        {
            var obj = new TestObj();
            var initialState = Substitute.For<IState>();
            var testState = new MuckEnterState(obj);

            StateMachine stateMachine = new StateMachine(initialState);
            stateMachine.AddAnyTransition(testState, () => true);

            stateMachine.Tick();

            Assert.AreEqual(testState.GetType().Name, obj.Name);
        }
        #endregion
    }
}
