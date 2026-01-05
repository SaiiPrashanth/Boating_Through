using UnityEngine;

public class Pickups : MonoBehaviour
{
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add score
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(2);
            }

            // Spawn particle effect
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Hide the pickup
            gameObject.SetActive(false);
        }
    }
}