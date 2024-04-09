using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    
    public enum NeighborType { NorthEast, North, NorthWest, SouthWest, South, SouthEast, Indirect }

    /// <summary>
    /// The size of the graph represented by a Vector2
    /// Defaults to 10x10
    /// </summary>
    public Vector2Int GraphSize { get; private set; }
    public int Width  => (int)GraphSize.x;
    public int Height => (int)GraphSize.y;
    /// <summary>
    /// The list of nodes in the graph
    /// </summary>
    public Dictionary<Vector2Int, Node> Nodes  { get; private set; }
    /// <summary>
    /// The grid used for the <see cref="Graph"/>
    /// Obtained from the <see cref="Board"/> component
    /// </summary>
    public Grid Grid { get; private set; }
    /// <summary>
    /// The <see cref="PathFinder"/> used to traverse the <see cref="Graph"/>
    /// </summary>
    private readonly PathFinder pathfinder;

    /// <summary>
    /// Creates a new <see cref="Graph"/> with default size of 10x10
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="pathfinder"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Graph(Grid grid, PathFinder pathfinder, int width = 10, int height = 10)
    {
        Nodes = new();
        Grid = grid;
        this.pathfinder = pathfinder;
        GraphSize = new Vector2Int(10, 10);
    }
    

    // Generate the grid
    public void GenerateGrid(Vector3 tileSize)
    {
        // Generate nodes
        for (int x = 0; x <= Width; x++)
        {
            for (int y = 0; y <= Height; y++)
            {
                Vector3Int position = new(x,y,0);
                Node node = new(position, this, 10); // Placeholder for Tile
                AddNode(node);
            }
        }

        // Assign neighbors to each node
        foreach (KeyValuePair<Vector2Int, Node> node in Nodes)
        {
            Dictionary<NeighborType, Node> neighbors = FindNeighbors(node);
            node.Value.AddNeighbors(neighbors);
        }
    }

    public Vector3 GridToWorldPosition(int x, int y) => Grid.CellToWorld(new Vector3Int(x, y, 0));
    public Vector3 GridToWorldPosition(Vector2 gridPosition) => Grid.CellToWorld(new Vector3Int((int)gridPosition.x, (int)gridPosition.y, 0));
    public Vector3 GridToWorldPosition(Vector3Int gridPosition) => Grid.CellToWorld(gridPosition);
    public Vector3 WorldToGridPosition(Vector3 worldPosition) => Grid.WorldToCell(worldPosition);
    public Vector2 WorldToGridPosition(Vector2 worldPosition) => new(Grid.WorldToCell(worldPosition).x, Grid.WorldToCell(worldPosition).z);

    
    private Dictionary<NeighborType, Node> FindNeighbors(KeyValuePair<Vector2Int, Node> node)
    {
        
        Dictionary<NeighborType, Node> neighbors = new(8);

        Vector2Int position = node.Key;
        Node    thisNode = node.Value;
        int index = (int)(position.x + position.y * Width);

        Dictionary<NeighborType, Vector2Int> dirToVector2 = new()
        {
            { NeighborType.North, new Vector2Int(0, 1) },
            { NeighborType.NorthEast, new Vector2Int(1, 1) },
            { NeighborType.NorthWest, new Vector2Int(-1, 1) },
            { NeighborType.South, new Vector2Int(0, -1) },
            { NeighborType.SouthEast, new Vector2Int(1, -1) },
            { NeighborType.SouthWest, new Vector2Int(-1, -1) }
        };
        bool[] isOnEdge = new bool[] 
        {
        position.x == 0,         // [0] Is on the left edge
        position.x == Width - 1, // [1] Is on the right edge
        position.y == 0,         // [2] Is on the bottom edge
        position.y == Height - 1 // [3] Is on the top edge
        };

        
        foreach (NeighborType NeighborType in Enum.GetValues(typeof(NeighborType)))
        {
            Vector2Int neighbor = new();
            switch (NeighborType)
            {
                case NeighborType.NorthWest: 
                    if (!isOnEdge[0] && !isOnEdge[3]) neighbor = position + dirToVector2[NeighborType]; 
                    break;
                case NeighborType.North: 
                    if (!isOnEdge[3]) neighbor = position + dirToVector2[NeighborType];
                    break;
                case NeighborType.NorthEast: 
                    if (!isOnEdge[1] && !isOnEdge[3]) neighbor = position + dirToVector2[NeighborType]; 
                    break;
                case NeighborType.SouthWest: 
                    if (!isOnEdge[0] && !isOnEdge[2]) neighbor = position + dirToVector2[NeighborType];
                    break;
                case NeighborType.South:
                    if (!isOnEdge[2]) neighbor = position + dirToVector2[NeighborType];   
                    break;
                case NeighborType.SouthEast: neighbor = position + dirToVector2[NeighborType];  
                    break;
            }

            neighbors[NeighborType] = Nodes.TryGetValue(neighbor, out Node neighborNode) ? neighborNode : null; 
            
        }
        
        return neighbors;
    }

    public void AddNode(Node node)           => Nodes.TryAdd(node.Position2D, node);

    public void RemoveNode(Node node)        => Nodes.Remove(node.Position2D);
    public void RemoveNode(Vector2Int position) => Nodes.Remove(position);

    public List<Node> FindPath(Node startNode, Node endNode)
    {
        // Use the Pathfinder instance for pathfinding
        return pathfinder.FindPath(startNode, endNode, this);
    }

    public List<KeyValuePair<string, Node>> GetNeighborsList       (Node node) => node.Neighbors.ToList();
    public Dictionary<string, Node>         GetNeighborsDictionary (Node node) => node.Neighbors;
}

