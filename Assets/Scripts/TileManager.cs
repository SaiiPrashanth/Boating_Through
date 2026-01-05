using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    public GameObject topTilePrefab;
    public GameObject leftTilePrefab;
    public GameObject currentActiveTile;

    // Object pooling
    private Stack<GameObject> _topTilePool = new Stack<GameObject>();
    private Stack<GameObject> _leftTilePool = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Fill up the pools
        GeneratePoolItems(100);

        // Spawn initial path
        for (int i = 0; i < 50; i++)
        {
            SpawnNextTile();
        }
    }

    public void GeneratePoolItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject top = Instantiate(topTilePrefab);
            GameObject left = Instantiate(leftTilePrefab);
            
            top.SetActive(false);
            left.SetActive(false);
            
            _topTilePool.Push(top);
            _leftTilePool.Push(left);
        }
    }

    public void SpawnNextTile()
    {
        // Refill if empty
        if (_leftTilePool.Count == 0 || _topTilePool.Count == 0)
        {
            GeneratePoolItems(100);
        }

        // 0 = Top, 1 = Left
        int direction = Random.Range(0, 2);
        GameObject newTile = null;

        if (direction == 0 && _topTilePool.Count > 0)
        {
            newTile = _topTilePool.Pop();
        }
        else if (direction == 1 && _leftTilePool.Count > 0)
        {
            newTile = _leftTilePool.Pop();
        }

        if (newTile != null && currentActiveTile != null)
        {
            newTile.SetActive(true);

            // Place at the attach point of current tile
            newTile.transform.position = currentActiveTile.transform.GetChild(0).GetChild(direction).position;
            
            currentActiveTile = newTile;

            TrySpawnPickup(currentActiveTile);
        }
    }

    private void TrySpawnPickup(GameObject tile)
    {
        // 1 in 10 chance
        int roll = Random.Range(0, 10);
        if (roll == 0 && tile.transform.childCount > 1)
        {
            tile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ReturnTopTile(GameObject tile)
    {
        if (tile != null)
        {
            _topTilePool.Push(tile);
        }
    }

    public void ReturnLeftTile(GameObject tile)
    {
        if (tile != null)
        {
            _leftTilePool.Push(tile);
        }
    }
}