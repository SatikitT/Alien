using UnityEngine;

public class BowHuman : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow prefab
    public float shootInterval = 2f; // Time interval between shots
    public Transform shootPoint; // Point from where the arrow is shot
    public float arrowSpeed = 5f; // Speed of the arrow
    public string shootSoundName; // Name of the arrow shoot sound
    public string hitSoundName; // Name of the arrow hit sound

    private float timeSinceLastShot;
    private Transform target;

    void Start()
    {
        timeSinceLastShot = 0f;
        target = GameObject.FindGameObjectWithTag("Spacecraft").transform;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootInterval)
        {
            ShootArrow();
            timeSinceLastShot = 0f;
        }
    }

    void ShootArrow()
    {
        if (arrowPrefab != null && shootPoint != null && target != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            Vector2 direction = (target.position - shootPoint.position).normalized;
            arrow.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;

            // Rotate the arrow to face the direction it's moving
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Play the shooting sound effect
            AudioManager.Instance.PlaySoundEffect(shootSoundName);

            // Assign target and hit sound to arrow
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.target = target;
                arrowScript.hitSoundName = hitSoundName;
            }
        }
    }
}
