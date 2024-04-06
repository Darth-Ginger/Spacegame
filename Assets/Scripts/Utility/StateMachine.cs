using System;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A generic state machine class for managing states using an enum.
/// </summary>
/// <typeparam name="T">An enum type representing the states.</typeparam>
public class StateMachine<T> where T : Enum
{
    /// <summary>
    /// The current state of the state machine.
    /// </summary>
    private T _currentState;

    /// <summary>
    /// A dictionary mapping each state to a list of possible transitions.
    /// Each transition consists of a target state and an optional condition.
    /// </summary>
    private Dictionary<T, List<(T nextState, Func<bool> condition)>> _transitions = new();

    /// <summary>
    ///  An Event Group used to dynamically update the state of the state machine.
    /// </summary>
    private bool _eventListener = false;

    /// <summary>
    /// Gets the current state of the state machine.
    /// </summary>
    public T CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            EventManager.SetData("OnStateChanged", _currentState.ToString());
            EventManager.EmitEvent("OnStateChanged");
        }
    }

    /// <summary>
    /// Adds a transition from one state to another with an optional condition.
    /// </summary>
    /// <param name="fromState">The state to transition from.</param>
    /// <param name="toState">The state to transition to.</param>
    /// <param name="condition">An optional condition that must be met for the transition to occur.</param>
    public void AddTransition(T fromState, T toState, Func<bool> condition = null)
    {
        if (!_transitions.ContainsKey(fromState))
        {
            _transitions[fromState] = new List<(T, Func<bool>)>();
        }
    
        // If condition is null, it defaults to a lambda that returns true
        _transitions[fromState].Add((toState, condition ?? (() => true)));
    }

    /// <summary>
    /// Attempts to change the state of the state machine.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    /// <returns>True if the transition was successful, false otherwise.</returns>
    public bool ChangeState(T newState)
    {
        if (EqualityComparer<T>.Default.Equals(_currentState, newState))
        {
            return false; // No transition if the state is unchanged
        }

        if (_transitions.ContainsKey(_currentState))
        {
            var validTransitions = _transitions[_currentState];
            foreach (var (nextState, condition) in validTransitions)
            {
                if (EqualityComparer<T>.Default.Equals(nextState, newState)
                    && (condition == null || condition()))
                {
                    CurrentState = newState;
                    return true;
                }
            }
        }

        return false; // Transition not allowed
    }
    
    /// <summary>
    /// Attempts to change the state of the state machine based on an event.
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns>True if the transition was successful, false otherwise.</returns>
    private void ChangeState()
    {
        var newState = (T)EventManager.GetData("ChangeState");
        Debug.Log(newState);
        if (newState != null) ChangeState(newState);
        // Return false if the data is not of the correct type
        
    }

    /// <summary>
    /// Toggle the state of the state machine event listener.
    /// </summary>
    public bool ToggleListener()
    {
        if (_eventListener){
            EventManager.StartListening("ChangeState", ChangeState);
            _eventListener = true;
        }
        else
        {
            EventManager.StopListening("ChangeState", ChangeState);
            _eventListener = false;
        }
        return _eventListener;
    }

    /// <summary>
    /// Returns true if the state machine is listening for events.
    /// </summary>
    /// <returns>True if the state machine is listening for events</returns>
    public bool Islistening()
    {
        return _eventListener;
    }

}