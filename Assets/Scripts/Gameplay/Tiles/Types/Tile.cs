using System;
using UnityEngine;
using UnityEngine.Events;

namespace Spacegame.Gameplay.Tiles
{
    [Serializable]
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileSO _tileSpecs;
        [SerializeField] public string Name { get; private set; }
        [SerializeField] private TileType Type;
        [SerializeField] private double CostModifier;
        public bool Occupancy  { get; private set; } = false;
        private bool isPathable;
        private bool IsWalkable { get => CanTraverse(); }
        public bool Visibility { get; private set; }
        public GameObject GameObject { get; protected set; }

        //@todo Figure out how we are implementing the effects 
        private UnityEvent TileEffects; 
        //@todo Include a place to reference the Unit that is occupying the tile

        public Tile(TileType type, GameObject gameObject)
        {

        }


        public void OnValidate()
        {
            if (_tileSpecs == null) return;
            this.Name           = _tileSpecs.Name;
            this.Type           = _tileSpecs.TileType;
            this.CostModifier   = _tileSpecs.CostModifier;
            this.isPathable     = _tileSpecs.Pathable;
            this.Visibility     = _tileSpecs.Visibility;
            this.GameObject     = _tileSpecs.GameObject;
            this.TileEffects    = _tileSpecs.TileEffects;
        }

        public bool SetTileSpecs(TileSO tileSpecs)
        {
            try{ _tileSpecs = tileSpecs; return true; }
            catch { return false; }
        }

        public bool CanTraverse() => !Occupancy && isPathable;

        public double GetCostModifier() => CostModifier;

        // Occupancy and visibility methods 
        public void SetOccupancy(bool status) => Occupancy = status;

        public void ToggleOccupancy() => Occupancy = !Occupancy;

        public void SetVisibility(bool status) => Visibility = status;

        public void ToggleVisibility() => Visibility = !Visibility;

        // Additional methods and properties specific to tile types can be implemented in derived classes
    }
}
