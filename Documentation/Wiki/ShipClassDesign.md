# Ship Class System

## Explanation

The `Ship` class now inherits from a basic `Unit` class, which will also be the base for `Structures` and `Crew`. This allows for shared functionality like taking damage or basic movement (if applicable) across different types of units. The `Ship` class will manage its components, power distribution, and status updates based on game events, which will be crucial for dynamic game mechanics like resource management during battles. Power distribution will affect component efficiency, with potential over-allocation providing boosts at a cost, and under-allocation reducing effectiveness.

## UML

```text
+--------------------------+
|           Unit           |
+--------------------------+
| - unitName: string       |
| - unitID: int            |
+--------------------------+
| + TakeDamage(amount: int)|
+--------------------------+
           ^
           | Inherits
           |
+--------------------------------------+
|           Ship                       |
+--------------------------------------+
| - hull: Hull                         |
| - hardpoints: Hardpoint[]            |
| - armor: Armor                       |
| - shields: Shield                    |
| - powerPlant: PowerPlant             |
| - engines: Engine                    |
| - commsArray: CommsArray             |
| - specializedComponents: Component[] |
| - crew: Crew                         |
+--------------------------------------+
| + InitializeComponents()             |
| + ManagePower()                      |
| + UpdateStatus()                     |
| + ModifyComponent()                  |
+--------------------------------------+
```

## Code

```csharp
using UnityEngine;

/// <summary>
/// Base class for all units in the game, including ships, structures, and crew.
/// </summary>
public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitID;

    /// <summary>
    /// Handle damage to the unit.
    /// </summary>
    /// <param name="amount">Amount of damage to take.</param>
    public virtual void TakeDamage(int amount)
    {
        // Base damage handling logic here.
    }
}

/// <summary>
/// Represents ships which are a type of unit in the game.
/// </summary>
public class Ship : Unit
{
    [SerializeField] private Hull hull;
    [SerializeField] private Hardpoint[] hardpoints;
    [SerializeField] private Armor armor;
    [SerializeField] private Shield shields;
    [SerializeField] private PowerPlant powerPlant;
    [SerializeField] private Engine engines;
    [SerializeField] private CommsArray commsArray;
    [SerializeField] private Component[] specializedComponents;
    [SerializeField] private Crew crew;  // Assumes Crew is derived from Unit

    void Start()
    {
        InitializeComponents();
    }

    /// <summary>
    /// Initialize ship components and set up initial configurations.
    /// </summary>
    private void InitializeComponents()
    {
        // Setup component based on ScriptableObject definitions
        // and possibly initial power allocation here.
    }

    /// <summary>
    /// Manage power distribution among ship components, affecting their efficiency.
    /// </summary>
    private void ManagePower()
    {
        // Calculate power needs vs availability and adjust component efficiency.
        // Over-allocation and under-allocation effects implemented here.
    }

    /// <summary>
    /// Update ship status per turn or event, e.g., regenerating shields.
    /// </summary>
    public void UpdateStatus()
    {
        // Listen for event triggers to handle status updates like shield regen or damage over time.
    }

    /// <summary>
    /// Modify components during gameplay (e.g., swapping out weapons).
    /// </summary>
    /// <param name="component">The component to modify.</param>
    public void ModifyComponent(Component component)
    {
        // Implement logic for adding, removing, or changing components.
    }

    public override void TakeDamage(int amount)
    {
        // Custom damage logic for ships, considering shields and armor.
    }
}
```

## Questions/Refinements

- Do we need to consider limitations on how often components can be modified (e.g., only in specific scenarios or at certain times)?
- How should the `Crew` influence specific ship functions? Should there be direct modifiers or more of a passive benefit system?

Your input will be essential to continue refining these systems further.
