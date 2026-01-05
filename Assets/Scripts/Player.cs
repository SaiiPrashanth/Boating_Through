using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Config")]
    public float moveSpeed = 9.0f;
    public float turnSpeed = 10.0f;
    public float sinkingSpeed = 2.0f;

    [Header("Components")]
    public Transform modelTransform;

    private Rigidbody _rb;
    private bool _isSinking = false;
    private Quaternion _targetRot;
    private Vector3 _currentDir;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        // Set initial rotation target
        if (modelTransform != null)
        {
            _targetRot = modelTransform.rotation;
        }
        else
        {
            _targetRot = transform.rotation;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.isGameOver)
        {
            ProcessInput();
            Move();
            RotateModel();
        }
        
        if (_isSinking)
        {
            // Move boat down
            transform.position += Vector3.down * sinkingSpeed * Time.deltaTime;
        }

        // Freeze physics if game is paused/frozen
        if (GameManager.Instance.areTilesFrozen && _rb != null)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Switch direction
            if (_currentDir == Vector3.forward)
            {
                _currentDir = Vector3.left;
            }
            else
            {
                _currentDir = Vector3.forward;
            }

            // Update rotation target
            if (_currentDir != Vector3.zero)
            {
                _targetRot = Quaternion.LookRotation(_currentDir);
            }
        }
    }

    private void Move()
    {
        transform.Translate(_currentDir * moveSpeed * Time.deltaTime);
    }

    private void RotateModel()
    {
        Transform target = modelTransform != null ? modelTransform : transform;
        
        // Smooth rotation
        target.rotation = Quaternion.Slerp(target.rotation, _targetRot, turnSpeed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if we left the path
        if (other.CompareTag("TopTile") || other.CompareTag("LeftTile") || other.CompareTag("StartTile"))
        {
            CheckGroundStatus();
        }
    }

    private void CheckGroundStatus()
    {
        RaycastHit hit;
        Ray rayDown = new Ray(transform.position, Vector3.down);
        
        // If nothing is below us
        if (!Physics.Raycast(rayDown, out hit, 10f))
        {
            if (GameManager.Instance != null && GameManager.Instance.hasGameEnded)
            {
                // Let gravity take over
                if (_rb != null)
                {
                    _rb.isKinematic = false;
                    _rb.useGravity = true;
                    _rb.constraints = RigidbodyConstraints.None;
                }
            }
            else
            {
                // We fell off during gameplay
                if (_rb != null)
                {
                    // Stop sliding
                    _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
                }

                GameManager.Instance.TriggerGameOver();
                StartCoroutine(SinkRoutine());
            }
        }
    }

    private IEnumerator SinkRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        _isSinking = true;
        
        // Disable physics for manual sinking
        if (_rb != null)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }
    }
}