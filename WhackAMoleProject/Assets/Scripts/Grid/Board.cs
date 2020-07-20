using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Grid
{
    public class Board : MonoBehaviour
    {
        private const int _defaultSize = 5;
        private const float _defaultSpacing = 0.1f;

        [SerializeField] 
        private float _cameraDistance = 10f;
        [SerializeField] 
        private bool _createOnAwake = true;
        // Pragma removes warnings in the Unity editor, as it is not aware of editor injections.
#pragma warning disable 649
        [SerializeField] 
        private GameObject _tilePrefab;
        [SerializeField] 
        private Camera _camera;
#pragma warning restore 649
        [SerializeField] 
        private Vector2Int _boardSize = new Vector2Int(_defaultSize, _defaultSize);
        [SerializeField] 
        private Vector2 _spacing = new Vector2(_defaultSpacing, _defaultSpacing);
        [SerializeField] 
        private Vector3[,] _grid;
        public Vector3[,] Grid { get => _grid; }

        public Vector2Int Size { get => _boardSize; }

        private void Awake()
        {
            if (_createOnAwake)
            {
                Create(_boardSize.x, _boardSize.y);
                Debug.LogWarning("Created on Awake.");
            }
        }

        [ContextMenu("CreateBoard")]
        private void Create()
        {
            Create(_boardSize.x, _boardSize.y);
        }

        public void Create(int width, int height)
        {
            Delete();

            Vector3 tileSize = _tilePrefab.transform.localScale * GetMeshScaleModifier();
            _grid = new Vector3[width, height];

            float halfWidth = width / 2;
            float halfHeight = height / 2;

            float xStart = transform.position.x - (halfWidth * tileSize.x) - (halfWidth * _spacing.x);
            float yStart = transform.position.y - (halfHeight * tileSize.y) - (halfHeight * _spacing.y);

            float horizontalOffset = (width & 1) == 1 ? 0 : 0.5f * tileSize.x;
            float verticalOffset = (height & 1) == 1 ? 0 : 0.5f * tileSize.y;
            Vector2 position = new Vector2(xStart, yStart);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject tile = Instantiate(_tilePrefab, transform);
                    // Tiles are flipped 90 degrees. Therefore their height value is their z value.              
                    tile.transform.position = new Vector3(position.x + x * tileSize.x + x * _spacing.x + horizontalOffset,
                                                          position.y + y * tileSize.z + y * _spacing.y + verticalOffset,
                                                          transform.position.z);
                    _grid[x, y] = tile.transform.position;
                }
            }

            CenterCamera();
        }

        [ContextMenu("Center Camera")]
        private void CenterCamera()
        {
            if (_camera == null)
            {
                Debug.LogError("No camera found. Cannot center camera.");
                return;
            }
            _camera.transform.position = new Vector3(transform.position.x, transform.position.y, -_cameraDistance);
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

        [ContextMenu("DeleteBoard")]
        private void Delete()
        {
            var children = GetComponentsInChildren<Transform>();
            int length = children.Length;
            // Makes sure it doesn't delete itself, as it is the first Transform found in its children.
            if (length <= 1)
                return;
            for (int i = length - 1; i > 0; i--)
            {
#if UNITY_EDITOR
                DestroyImmediate(children[i].gameObject);
#else
            Destroy(children[i].gameObject);
#endif
            }
            _grid = null;
            Debug.LogWarning("Deleted grid.");
        }

        private void Reset()
        {
            _camera = Camera.main;
        }
    }
}
