
# ComponentSO Implementation

## Explanation

By integrating the power management callbacks in the activation and deactivation methods of `ComponentSO`, the `Ship` class can dynamically manage power distribution based on the current operational state of its components. The handling of component efficiency related to health will also be prepared within the `Ship` class to ensure that damage effects are realistically portrayed. Extending `HullSO` from `ComponentSO` will allow the hull to share common component attributes and behaviors, simplifying the management of base properties like size, mass, and functional effects.

## UML

```text
+--------------------------------+
|        ComponentSO             |
+--------------------------------+
| - componentName: string        |
| - size: int                    |
| - mass: int                    |
| - baseHullPoints: int          |
| - cards: Card[]                |
| - componentType: ComponentType |
+--------------------------------+
| + Initialize(): void           |
| + ApplyEffect(): void          |
| + Activate(): void             |
| + Deactivate(): void           |
| + HandleDamage(): void         |
+--------------------------------+
           ^
           | Inherits
           |
+----------------------------+    +----------------------------+    +--------------------------+
| EngineSO                   |    | ShieldSO                   |    |       HullSO             |
+----------------------------+    +----------------------------+    +--------------------------+
| Specific engine attributes |    | Specific shield attributes |    | Specific hull attributes |
+----------------------------+    +----------------------------+    +--------------------------+
```

## Code

```csharp
using UnityEngine;

/// <summary>
/// Base class for all ship component ScriptableObjects.
/// </summary>
public abstract class ComponentSO : ScriptableObject
{
    [Tooltip("Name of the component.")]
    public string componentName;

    [Tooltip("Size of the component, affects placement and compatibility.")]
    public int size;

    [Tooltip("Mass of the component, affects ship handling and speed.")]
    public int mass;

    [Tooltip("Hull points provided by this component, additive to the ship's total hull points.")]
    public int baseHullPoints;

    [Tooltip("Cards provided by this component for the ship's card deck.")]
    public Card[] cards;

    [Tooltip("Type of component, defines its general category.")]
    public ComponentType componentType;

    /// <summary>
    /// Initialize the component, setting up its initial state or configuration.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Apply the component's effects to the ship, such as adjusting hull points or power consumption.
    /// </summary>
    public abstract void ApplyEffect();

    /// <summary>
    /// Activate the component, making it operational. Adjust power allocation.
    /// </summary>
    public virtual void Activate()
    {
        // Logic to activate the component and adjust power
        Ship.ManagePower(); // Callback to Ship class
    }

    /// <summary>
    /// Deactivate the component, making it non-operational. Adjust power allocation.
    /// </summary>
    public virtual void Deactivate()
    {
        // Logic to deactivate the component and adjust power
        Ship.ManagePower(); // Callback to Ship class
    }

    /// <summary>
    /// Handle damage specific to the component, which may affect its functionality.
    /// </summary>
    public virtual void HandleDamage()
    {
        // Logic to handle damage effects on the component
    }
}

/// <summary>
/// ScriptableObject for defining hull properties of ships, extends ComponentSO.
/// </summary>
[CreateAssetMenu(fileName = "New HullSO", menuName = "Ship Components/Hull")]
public class HullSO : ComponentSO
{
    public override void Initialize()
    {
        // Specific initialization for hull
    }

    public override void ApplyEffect()
    {
        // Adjust ship's base hull points
    }
}
```
