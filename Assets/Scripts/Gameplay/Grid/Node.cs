using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Spacegame.Gameplay.Tiles;

namespace Spacegame.Gameplay
{
    public class Node
    {
        public Vector3Int Position;
        public Vector2Int Position2D { get => (Vector2Int)Position; }
        public string Name { get => Position.ToString(); } // Getter for Position
        public float X { get => Position.x; } // Getter for Position.x
        public float Y { get => Position.y; } // Getter for Position.y
        public int BaseCost;
        private int AdjustedCost { get; set; }
        public int Cost { get => AdjustedCost; } // Getter for AdjustedCost;
        public Dictionary<string, Node> Neighbors { get; private set; }
        private Graph Graph { get; }
        private Tile TileReference;
        private bool Traversible;


        public Node(Vector3Int position, Graph graph, int baseCost = 10, Tile tileReference = null)
        {
            this.Position = position;
            this.BaseCost = baseCost;
            this.TileReference = tileReference;
            this.Graph = graph;
            AdjustedCost = baseCost; // Default to base cost, updated by Tile attribute
            Neighbors = new Dictionary<string, Node>();
            Traversible = true; // Default to true, updated by Tile attribute
        }

        #region     Position Conversions
        /// <summary>
        /// Converts the node's position to a world position
        /// </summary>
        /// <returns></returns>
        public Vector3 GridPositionToWorld() => Position + Graph.GridToWorldPosition(Mathf.FloorToInt(Position.x), Mathf.FloorToInt(Position.y));
        /// <summary>
        /// Converts the node's position to a grid position
        /// </summary>
        /// <returns></returns>
        public Vector3 WorldPositionToGrid() => Graph.WorldToGridPosition(Position);

        #endregion  Position Conversions

        #region     Utility Methods
        #region     Add/Remove Neighbors

        public void AddNeighbor(Node node) => Neighbors[node.Name] = node;
        public void RemoveNeighbor(string name) => Neighbors.Remove(name);
        public void RemoveNeighbor(Node node) => Neighbors.Remove(node.Name);
        public void AddNeighbors(Node[] nodes)
        {
            foreach (Node node in nodes)
            {
                AddNeighbor(node);
            }
        }
        public void AddNeighbors(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                AddNeighbor(node);
            }
        }
        public void AddNeighbors(Dictionary<string, Node> neighbors) => Neighbors = neighbors;
        public void AddNeighbors(Dictionary<NeighborType, Node> neighbors) => Neighbors = neighbors.ToDictionary(x => x.Key.ToString(), x => x.Value);

        #endregion Add/Remove Neighbors

        public void AddTileReference(Tile tile) => TileReference = tile;
        public Tile GetTileReference() => TileReference;

        public void SetTraversible(bool status) => Traversible = status;
        public void ToggleTraversible() => Traversible = !Traversible;
        public bool IsTraversible() => Traversible;

        // Method to update node's properties based on the tile's current state
        public void UpdateNodeState()
        {
            AdjustedCost = (int)(BaseCost * TileReference.GetCostModifier());
            SetTraversible(TileReference.CanTraverse());
        }
        #endregion  Utility Methods
    }
}