using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private GameObject _tilePrefab;
#pragma warning restore 649
    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    private Vector2 _spacing = Vector2.zero;
    private bool[,] _grid;

    private void Start() => Create(_boardSize.x, _boardSize.y);

    public void Create(int width, int height)
    {       
        Vector3 tileSize = _tilePrefab.transform.localScale * GetMeshScaleModifier();
        _grid = new bool[width, height];

        float halfWidth = width / 2;
        float halfHeight = height / 2;
        
        float xStart = transform.position.x - (halfWidth * tileSize.x) - (halfWidth * _spacing.x);
        float yStart = transform.position.y - (halfHeight * tileSize.y) - (halfWidth * _spacing.y);
        Vector2 position = new Vector2(xStart, yStart);

        for (float x = 0; x < width; x++)
        {
            for (float y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(_tilePrefab, transform);
                // Tiles are flipped 90 degrees. Therefore their height value is their z value.              
                tile.transform.position = new Vector3(position.x + x * tileSize.x + x * _spacing.x, position.y + y * tileSize.z + y * _spacing.y, transform.position.z);               
            }
        }
    }

    // Planes have a larger scale. This method allows to apply different scalers to different sharedMeshes.
    private int GetMeshScaleModifier()
    {
        string name = _tilePrefab.GetComponent<MeshFilter>().sharedMesh.name;
        switch (name)
        {
            case "Plane":
                return 10;
            default:
                return 1;
        }
    }

    // Checks if the tile is currently locked/occupied
    public bool IsTileLocked(int x, int y) => _grid[x, y];
}
