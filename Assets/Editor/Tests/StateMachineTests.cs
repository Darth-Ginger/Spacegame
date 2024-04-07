// Here's an example of how you could write unit tests for the different scenarios you mentioned using NUnit:

using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;
using TigerForge;


[TestFixture]
public class StateMachineTests
{
    private enum YourEnumType{
        State1,
        State2,
        State3
    }
    [Test]
    public void TransitionAddition_TransitionsAddedCorrectly()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();

        // Act
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2);

        // Assert
        Assert.IsTrue(stateMachine.ChangeState(YourEnumType.State2));
    }

    [Test]
    public void StateChangeSuccess_ValidTransition_ChangesState()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2);

        // Act
        bool result = stateMachine.ChangeState(YourEnumType.State2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(YourEnumType.State2, stateMachine.CurrentState);
    }

    [Test]
    public void StateChangeFailure_InvalidTransition_DoesNotChangeState()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2);

        // Act
        bool result = stateMachine.ChangeState(YourEnumType.State3);

        // Assert
        Assert.IsFalse(result);
        Assert.AreNotEqual(YourEnumType.State3, stateMachine.CurrentState);
    }

    [Test]
    public void TransitionCondition_ConditionMet_StateChanges()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2, () => true);

        // Act
        bool result = stateMachine.ChangeState(YourEnumType.State2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(YourEnumType.State2, stateMachine.CurrentState);
    }

    [Test]
    public void TransitionCondition_ConditionNotMet_StateDoesNotChange()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2, () => false);

        // Act
        bool result = stateMachine.ChangeState(YourEnumType.State2);

        // Assert
        Assert.IsFalse(result);
        Assert.AreNotEqual(YourEnumType.State2, stateMachine.CurrentState);
    }

    [Test]
    public void EventHandling_EventChangesState()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2);

        // Act
        stateMachine.ToggleListener();

        EventManager.SetData("ChangeState", YourEnumType.State2.ToString());
        EventManager.EmitEvent("ChangeState");        

        // Assert
        Assert.AreEqual(YourEnumType.State2, stateMachine.CurrentState);
    }

    [Test]
    public void CurrentStateRetrieval_ReturnsCurrentState()
    {
        // Arrange
        StateMachine<YourEnumType> stateMachine = new StateMachine<YourEnumType>();
        stateMachine.AddTransition(YourEnumType.State1, YourEnumType.State2);

        // Act
        stateMachine.ChangeState(YourEnumType.State2);

        // Assert
        Assert.AreEqual(YourEnumType.State2, stateMachine.CurrentState);
    }

}