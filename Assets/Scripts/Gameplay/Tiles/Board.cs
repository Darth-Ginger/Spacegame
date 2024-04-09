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
    [GetFromThis]
    public Grid Grid;
    public Graph Graph;
    public PathFinder PathFinder;

    [NaughtyAttributes.Button("Generate Tiles")]
    public void Generate() 
    {
        if (transform.childCount > 0 && generated) 
        {
            Clear();
        }
        GenerateTiles();
    }

    [NaughtyAttributes.Button("Clear Tiles")]
    public void Clear()
    {
        while (transform.Find("Tiles").childCount > 0) DestroyImmediate(transform.Find("Tiles").GetChild(0).gameObject);
        generated = false;
        Graph = null;
    }
    private bool generated = false;
    private GameObject tileContainer;
    

    public Board() 
    { 
        Graph = new Graph(Grid, PathFinder);
    }

    public void Awake()
    {
        Graph = new Graph(Grid, PathFinder);
    }

    public void Start()
    {
        GenerateTiles();
    }


    public void GenerateTiles()
    {
        if (generated) return;
        Graph ??= new Graph(Grid, PathFinder);
        if ( tileContainer == null )
        {
            tileContainer = new GameObject("Tiles");
            tileContainer.transform.parent = transform;
            tileContainer.transform.localPosition = Vector3.zero;
            tileContainer.name = "Tiles";
        }
        
        // Generate the grid
        Graph.GenerateGrid( Grid.cellSize);
        //@todo - create method for procedural tile selection
        // Map tiles to nodes
        foreach (ScriptableTileDict tileDict in AllTiles.Values)
        {
            foreach (GameObject tile in tileDict.GetPrefabs())
            {
                foreach (KeyValuePair<Vector2Int, Node> node in Graph.Nodes)
                {
                    Vector3 worldPosition = Graph.GridToWorldPosition(node.Key);
                    GameObject tileObject = Instantiate(original: tile, position: worldPosition, rotation: Quaternion.Euler(90, 0, 0), parent: tileContainer.transform);
                    // tileObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                    tileObject.name = node.Value.Position2D.ToString();
                    Tile tileComponent = tileObject.GetComponent<Tile>();
                    node.Value.AddTileReference(tileComponent);  
                }
            }
        }

        generated = true;
    }


}