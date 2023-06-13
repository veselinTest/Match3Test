using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Grid.View;

namespace Assets.Scripts.Grid.Controller
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tilePrefab; // tile to instantiate

        [SerializeField]
        public int _gridSizeX = 8; // rows

        [SerializeField]
        public int _gridSizeY = 8; // columns

        [SerializeField]
        public float _tileSize = 100.0f;

        [SerializeField]
        public float _tileOffset = 7f;

        private List<GameObject> tiles;

        private void Start()
        {
            tiles = new List<GameObject>();

            GenerateGrid();

            FillGridWithGems();
        }

        public void GenerateGrid()
        {
            Vector3 startPosition = transform.position;

            for (int y = 0; y < _gridSizeY; y++)
            {
                for (int x = 0; x < _gridSizeX; x++)
                {
                    Vector3 tilePosition = startPosition + new Vector3(x * _tileSize + _tileOffset, y * -_tileSize - _tileOffset, 0f);

                    GameObject tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
                    tile.name = $"{x}_{y}";
                    tiles.Add(tile);

                    tile.transform.parent = transform;
                }
            }
        }

        public void FillGridWithGems()
        {
            foreach (GameObject tile in tiles)
            {
                if (!HasGem(tile))
                {
                    int gemsInRow = GetGemsInRow(tile);
                    int gemsInColumn = GetGemsInColumn(tile);

                    if (gemsInRow < 2 && gemsInColumn < 2)
                    {
                        string imageName = GetRandomGemImage();
                        tile.GetComponent<GridTile>().icon.sprite = GetImageFromAssets(imageName);
                    }
                }
            }
        }

        private bool HasGem(GameObject tile)
        {
            // Check if the tile has a child sprite renderer
            return tile.GetComponent<GridTile>().icon.sprite != null;
        }

        private int GetGemsInRow(GameObject tile)
        {
            int gemsInRow = 0;

            // Get the x and y position of the tile from its name
            string[] nameParts = tile.name.Split('_');
            int x = int.Parse(nameParts[1]);

            // Count the number of gems in the row
            for (int i = 0; i < _gridSizeX; i++)
            {
                GameObject adjacentTile = GetTileAtPosition(i, x);
                if (HasGem(adjacentTile))
                {
                    gemsInRow++;
                }
            }

            return gemsInRow;
        }

        private int GetGemsInColumn(GameObject tile)
        {
            int gemsInColumn = 0;

            string[] nameParts = tile.name.Split('_');
            int y = int.Parse(nameParts[0]);

            for (int i = 0; i < _gridSizeY; i++)
            {
                GameObject adjacentTile = GetTileAtPosition(y, i);
                if (HasGem(adjacentTile))
                {
                    gemsInColumn++;
                }
            }

            return gemsInColumn;
        }

        private GameObject GetTileAtPosition(int x, int y)
        {
            return tiles.Find(tile => tile.name == $"{x}_{y}");
        }

        private string GetRandomGemImage()
        {
            int randomIndex = Random.Range(0, MainGame.Instance.assetsData.assetsNamesToImagesMap.Count);

            string[] keys = MainGame.Instance.assetsData.assetsNamesToImagesMap.Keys.ToArray();
            string randomKey = keys[randomIndex];

            return randomKey;
        }

        private Sprite GetImageFromAssets(string imageName)
        {
            if (MainGame.Instance.assetsData.assetsNamesToImagesMap.TryGetValue(imageName, out Sprite image))
            {
                return image;
            }
            else
            {
                Debug.LogError($"Image '{imageName}' not found in assetsNamesToImagesMap.");
                return null;
            }
        }

    }
}
