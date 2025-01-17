using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class TilemapPainter : MonoBehaviour
{
    [System.Serializable]
    public class TileData
    {
        public TileBase tile; // Tile asset
        public int weight;    // Weight for this tile
    }

    public TileData[] tiles;        // Array of tiles with weights
    public Tilemap tilemap;         // Reference to the Tilemap
    public Camera mainCamera;       // Reference to the main camera
    public int range = 10;          // Range of tiles to check and paint around the player
    private Vector3Int lastPlayerCell; // Last painted cell position
    private List<TileBase> weightedTiles; // Weighted list for randomization

    void Start()
    {
        mainCamera = mainCamera ?? Camera.main;
        GenerateWeightedTileList();
    }

    void Update()
    {
        PaintVisibleTiles();
    }

    void GenerateWeightedTileList()
    {
        weightedTiles = new List<TileBase>();

        foreach (TileData tileData in tiles)
        {
            for (int i = 0; i < tileData.weight; i++)
            {
                weightedTiles.Add(tileData.tile); // Add each tile to the weighted list
            }
        }
    }

    void PaintVisibleTiles()
    {
        // Get the camera's world bounds
        Vector3 camPosition = mainCamera.transform.position;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        float minX = camPosition.x - halfWidth;
        float maxX = camPosition.x + halfWidth;
        float minY = camPosition.y - halfHeight;
        float maxY = camPosition.y + halfHeight;

        // Convert world bounds to tilemap bounds
        Vector3Int minCell = tilemap.WorldToCell(new Vector3(minX, minY, 0));
        Vector3Int maxCell = tilemap.WorldToCell(new Vector3(maxX, maxY, 0));

        // Paint all unpainted tiles within the visible bounds
        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                if (!tilemap.HasTile(cellPosition)) // Only paint if there's no tile already
                {
                    TileBase randomTile = GetRandomWeightedTile();
                    tilemap.SetTile(cellPosition, randomTile);
                }
            }
        }
    }

    TileBase GetRandomWeightedTile()
    {
        // Choose a random tile from the weighted list
        int randomIndex = Random.Range(0, weightedTiles.Count);
        return weightedTiles[randomIndex];
    }
}
