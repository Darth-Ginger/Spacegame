# HullSO Implementation

## Explanation

We've refined the `HullSO` by including properties for cargo capacity, which allows the ship to carry a set number of items and a maximum mass for these items. This addition is important for logistics and resource management in gameplay. Also, we've planned for an integration with a card-based game mechanic by anticipating a property for cards, which ship components will provide to the ship's capabilities. This will enrich the strategic depth and variability in gameplay.

## UML

```text
+-------------------------------------+
|       HullSO                        |
+-------------------------------------+
| - size: int                         |
| - mass: int                         |
| - baseHullPoints: int               |
| - componentLimits: ComponentLimit[] |
| - maxCargoCapacity: int             |
| - maxMassCapacity: int              |
| - cards: Card[]                     |
+-------------------------------------+
```

## Code

```csharp
using UnityEngine;

/// <summary>
/// ScriptableObject for defining hull properties of ships, including capacities and component limits.
/// </summary>
[CreateAssetMenu(fileName = "New HullSO", menuName = "Ship Components/Hull")]
public class HullSO : ScriptableObject
{
    [Tooltip("Relative size of the ship, impacts grid placement and maneuverability.")]
    public int size;

    [Tooltip("Mass of the ship, affects speed and inertia.")]
    public int mass;

    [Tooltip("Base hull points, represents the total health of the ship.")]
    public int baseHullPoints;

    [Tooltip("Limits on the types and sizes of components that can be equipped.")]
    public ComponentLimit[] componentLimits;

    [Tooltip("Maximum number of cargo items the ship can carry.")]
    public int maxCargoCapacity;

    [Tooltip("Maximum mass of cargo the ship can carry.")]
    public int maxMassCapacity;

    [Tooltip("Cards provided by ship components for game mechanics.")]
    public Card[] cards; // Placeholder for future card game mechanics

    // Consider adding later: Specific models or brands compatibility.
}

/// <summary>
/// Defines limits for a specific type of component that can be equipped on a ship.
/// </summary>
[System.Serializable]
public struct ComponentLimit
{
    public ComponentType componentType;
    public int maxSize;
    public int minSize;
    public int maxCount;
}

/// <summary>
/// Enum to define different types of components.
/// </summary>
public enum ComponentType
{
    Engine,
    Weapon,
    Shield,
    Armor,
    PowerPlant,
    CommsArray,
    Specialized
}

/// <summary>
/// Represents a card in the game's card deck mechanic.
/// </summary>
[System.Serializable]
public class Card
{
    // Attributes and methods for Card would be defined here.
}
```
