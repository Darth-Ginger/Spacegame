using System;
using UnityEngine;
using UnityEngine.Events;

namespace Spacegame.Gameplay
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
        public GameObject GameObject { get; private set; }

        //@todo Figure out how we are implementing the effects 
        private UnityEvent TileEffects; 
        //@todo Include a place to reference the Unit that is occupying the tile

        /// <summary>
        /// Sets the <see cref="Tile"/>'s specs
        /// </summary>
        public void OnValidate()
        {
            if (_tileSpecs == null) return;
            this.Name           = _tileSpecs.Name;
            this.Type           = _tileSpecs.TileType;
            this.CostModifier   = _tileSpecs.CostModifier;
            this.isPathable     = _tileSpecs.Pathable;
            this.Visibility     = _tileSpecs.Visibility;
            this.TileEffects    = _tileSpecs.TileEffects;
            this.GameObject     = transform.gameObject;
        }

        /// <summary>
        /// Sets the <see cref="Tile"/>'s specs
        /// </summary>
        /// <param name="tileSpecs"><see cref="TileOS"/> with the new <see cref="Tile"/>'s specs </param>
        /// <returns>True if the <see cref="Tile"/>'s specs were set successfully</returns>
        public bool SetTileSpecs(TileSO tileSpecs)
        {
            try{ _tileSpecs = tileSpecs; return true; }
            catch { return false; }
        }

        /// <summary>
        /// Returns if the <see cref="Tile"/> can be traversed
        /// </summary>
        /// <returns>Returns true if the <see cref="Tile"/> is unoccupied and is pathable</returns>
        public bool CanTraverse() => !Occupancy && isPathable;

        /// <summary>
        /// Returns the cost modifier of the <see cref="Tile"/>
        /// </summary>
        /// <returns>A double representing the cost modifier</returns>
        public double GetCostModifier() => CostModifier;

        // Occupancy and visibility methods 
        /// <summary>
        /// Sets the occupancy of the <see cref="Tile"\>
        /// </summary>
        /// <param name="status"></param>
        public void SetOccupancy(bool status) => Occupancy = status;

        /// <summary>
        /// Toggles the occupancy of the <see cref="Tile"\>
        /// </summary>
        public void ToggleOccupancy() => Occupancy = !Occupancy;

        /// <summary>
        /// Sets the visibility of the <see cref="Tile"\>
        /// </summary>
        /// <param name="status"></param>
        public void SetVisibility(bool status) => Visibility = status;

        /// <summary>
        /// Toggles the visibility of the <see cref="Tile"\>
        /// </summary>
        public void ToggleVisibility() => Visibility = !Visibility;


    }
}
