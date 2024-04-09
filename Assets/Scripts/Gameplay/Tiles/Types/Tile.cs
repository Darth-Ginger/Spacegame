using System;
using Injector;
using NaughtyAttributes;
using UnityEngine;
using static Board;

[Serializable]
public abstract class Tile : MonoBehaviour
{
    
    [ShowNativeProperty] public    string    Name            { get => transform.name; set => Name = transform.name; }    
    [SerializeField]     protected TileType  Type;
    [SerializeField]     protected double    CostModifier;
                         public    bool      Occupancy       { get; protected set; }
                         public    bool      Visibility      { get; protected set; }
    [GetFromThis]
                         public GameObject   GameObject      { get; protected set; }

    protected Tile(TileType type, GameObject gameObject)
    {
        this.Type       = type;
        Occupancy       = false;
        Visibility      = true;
        CostModifier    = 1; // Default value, should be adjusted in derived classes
    }

    // Abstract method to enforce implementation in derived classes for updating modifiers
    
    
    // Default logic that can be overridden in derived classes
    public virtual bool CanWalk() => !Occupancy && Visibility;

    public virtual double GetCostModifier() => CostModifier;
        
    // Occupancy and visibility methods 
    public void SetOccupancy(bool status) => Occupancy = status;

    public void ToggleOccupancy() => Occupancy = !Occupancy;

    public void SetVisibility(bool status) => Visibility = status;

    public void ToggleVisibility() => Visibility = !Visibility;

    // Additional methods and properties specific to tile types can be implemented in derived classes
}
