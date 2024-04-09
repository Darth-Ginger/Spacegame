using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AYellowpaper;
using CustomInspector;
using NaughtyAttributes;
using Injector;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(Grid))]
public class Board : MonoBehaviour
{
    public enum TileType { OpenSpace, Nebula, AsteroidField, Base }

    [Tooltip("Each Key is a TileType, the Value contains all possible tiles for that type.")]
    [SerializedDictionary("TileType", "Possible Tiles")]
    public SerializableSortedDictionary<TileType, ScriptableTileDict> AllTiles;
    public List<Tile> Tiles { get; private set; }
    public Grid Grid;
    public Graph Graph;
    public PathFinder PathFinder;

    private bool generated = false;
    

    public Board() 
    { 
        Graph = new Graph(Grid, PathFinder);
    }

    public void Start()
    {
        Grid = GetComponent<Grid>();
        Grid.cellLayout = GridLayout.CellLayout.Hexagon;
        Grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
    }


    public void GenerateTiles()
    {
        if (generated) return;
        // Generate the grid
        Graph.GenerateGrid( Grid.cellSize);

        //@todo - create method for procedural tile selection
        // Map tiles to nodes
        foreach (ScriptableTileDict tileDict in AllTiles.Values)
        {
            foreach (GameObject tile in tileDict.GetPrefabs())
            {
                foreach (KeyValuePair<Vector2, Node> node in Graph.Nodes)
                {
                    Vector3 worldPosition = Graph.GridToWorldPosition(node.Key);
                    GameObject tileObject = Instantiate(tile, worldPosition, Quaternion.identity);
                    tileObject.transform.parent = transform;
                    Tile tileComponent = tileObject.GetComponent<Tile>();
                    node.Value.AddTileReference(tileComponent);  
                }
            }
        }

        generated = true;
    }


}