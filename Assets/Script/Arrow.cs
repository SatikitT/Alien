using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;
    public string hitSoundName; // Name of the hit sound

    void Update()
    {
        // Rotate the arrow to face its movement direction
        Vector2 direction = GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Spacecraft") && target is PolygonCollider2D)
        {
            Debug.Log("Hti spacecraft");
            SpacecraftHealth health = target.gameObject.GetComponent<SpacecraftHealth>();
            if (health != null)
            {
                health.TakeDamage();
            }

            AudioManager.Instance.PlaySoundEffect(hitSoundName);

            // Destroy the arrow after a short delay to allow sound to play
            Destroy(gameObject, 0.1f);
        }
        else if (target.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

