using UnityEngine;

public class HumanWalker : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float changeDirectionTime = 2f; // Time between changing directions
    private SpacecraftController spacecraft;

    private float timeSinceLastChange;
    private int direction; // -1 for left, 1 for right
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spacecraft = FindObjectOfType<SpacecraftController>();
        timeSinceLastChange = 0f;
        direction = Random.Range(0, 2) * 2 - 1; // Randomly set initial direction to -1 (left) or 1 (right)
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSpriteDirection();
    }

    void Update()
    {
        // Move the human
        transform.Translate(Vector2.right * direction * walkSpeed * Time.deltaTime);

        // Update the timer and change direction if necessary
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= changeDirectionTime)
        {
            ChangeDirection();
            timeSinceLastChange = 0f;
        }
    }

    void ChangeDirection()
    {
        // Change direction randomly
        direction = Random.Range(0, 2) * 2 - 1; // Randomly set direction to -1 (left) or 1 (right)
        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        // Flip the sprite based on the direction
        if (direction == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the human hits a wall or obstacle, change direction
        if (other.CompareTag("Wall"))
        {
            ChangeDirection();
        }
        if (other.CompareTag("Spacecraft"))
        {
            spacecraft.score++;
            Destroy(gameObject);
        }
    }
}
