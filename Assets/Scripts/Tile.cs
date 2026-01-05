using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Tile : MonoBehaviour
{
    [Header("Animation Settings")]
    public float delayBeforeSink = 0.1f;
    public float sinkDuration = 3f;

    private Rigidbody _tileRb;
    private bool _isAlreadySinking;

    private void Awake()
    {
        _tileRb = GetComponent<Rigidbody>();
        if (_tileRb != null) 
        { 
            _tileRb.isKinematic = true; 
        }
    }

    private void Update()
    {
        // Stop moving if everything is frozen
        if (GameManager.Instance != null && 
            GameManager.Instance.isGameOver && 
            GameManager.Instance.areTilesFrozen && 
            GameManager.Instance.hasGameEnded)
        {
            if (_tileRb != null) 
            {
                _tileRb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When player leaves, spawn new tile and remove this one
        if (other.CompareTag("Player"))
        {
            if (TileManager.Instance != null) 
            {
                TileManager.Instance.SpawnNextTile();
            }

            if (!_isAlreadySinking) 
            {
                StartCoroutine(SinkRoutine());
            }
        }
    }

    private IEnumerator SinkRoutine()
    {
        _isAlreadySinking = true;
        
        yield return new WaitForSeconds(delayBeforeSink);
        
        if (_tileRb != null) 
        { 
            _tileRb.isKinematic = false; 
        }
        
        yield return new WaitForSeconds(sinkDuration);
        
        // Recycle logic
        if (GameManager.Instance == null || !GameManager.Instance.areTilesFrozen)
        {
            ReturnToPool();
        }
        else
        {
            // Just freeze if game ended
            if (_tileRb != null) 
            { 
                _tileRb.isKinematic = true; 
                _tileRb.Sleep(); 
            }
        }
        
        _isAlreadySinking = false;
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        
        if (_tileRb != null) 
        { 
            _tileRb.isKinematic = true; 
            _tileRb.constraints = RigidbodyConstraints.None; 
        }

        if (TileManager.Instance != null)
        {
            if (CompareTag("TopTile")) 
            { 
                TileManager.Instance.ReturnTopTile(gameObject); 
            }
            else if (CompareTag("LeftTile")) 
            { 
                TileManager.Instance.ReturnLeftTile(gameObject); 
            }
        }
    }
}