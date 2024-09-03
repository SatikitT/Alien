using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SpacecraftController : MonoBehaviour
{
    public float speed = 5f;
    public float lineWidth = 0.1f;
    public Color lineColor = Color.green;
    public float suckForce = 1f; // Speed at which humans are sucked towards the spacecraft
    public float boxWidth = 1f; // Width of the sucking box
    public float rotationSpeed = 2f; // Speed of smooth rotation
    public float rotationTolerance = 5f; // Cap for rotation in degrees
    public string suckSoundName; // Name of the sucking sound
    public string backgroundMusicName; // Name of the background music
    public TextMeshProUGUI scoreText;

    public int score = 0;

    private LineRenderer lineRenderer;
    private Quaternion initialRotation;
    private GameObject suckingBox;
    private BoxCollider2D suckingBoxCollider;
    private List<Transform> humansToSuck;

    void Start()
    {
        // Save the initial rotation of the spacecraft
        initialRotation = transform.rotation;

        // Add a LineRenderer component to the spacecraft
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // Create the sucking box as a child of the spacecraft
        suckingBox = new GameObject("SuckingBox");
        suckingBox.transform.SetParent(transform);
        suckingBoxCollider = suckingBox.AddComponent<BoxCollider2D>();
        suckingBoxCollider.isTrigger = true;
        suckingBoxCollider.size = new Vector2(boxWidth, 0); // Initially, the height is set to 0

        humansToSuck = new List<Transform>();

        // Start playing background music
        AudioManager.Instance.PlayMusic(backgroundMusicName);
        scoreText.text = $"Score: {score}";
    }

    void Update()
    {
        scoreText.text = $"Score: {score}";
        // Move the spacecraft
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);

        // Handle mouse click for line activation
        if (Input.GetMouseButton(0))
        {
            ActivateLine();
            // Play the sucking sound effect if not already playing
            if (!AudioManager.Instance.sfxSource.isPlaying)
            {
                AudioManager.Instance.PlaySoundEffect(suckSoundName);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DeactivateLine();
            // Stop the sucking sound effect
            AudioManager.Instance.sfxSource.Stop();
        }

        // Move the humans towards the spacecraft
        MoveHumansTowardsSpacecraft();

        // Check if the spacecraft needs to rotate back to its initial position
        SmoothRotateBackToInitial();
    }

    void ActivateLine()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Update the LineRenderer to draw a ray from the spacecraft to the mouse position
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, mousePos);

        // Calculate the direction and distance to the mouse position
        Vector2 direction = mousePos - transform.position;
        float distance = direction.magnitude;

        // Adjust the sucking box size and position
        suckingBox.transform.localPosition = new Vector3(direction.x / 2, direction.y / 2, 0);
        suckingBox.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
        suckingBoxCollider.size = new Vector2(boxWidth, distance);
    }

    void DeactivateLine()
    {
        // Clear the LineRenderer
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        // Reset the sucking box size
        suckingBoxCollider.size = new Vector2(boxWidth, 0);
        humansToSuck.Clear(); // Clear the list when the line is deactivated
    }

    void MoveHumansTowardsSpacecraft()
    {
        foreach (Transform human in humansToSuck)
        {
            if (human != null)
            {
                Vector2 direction = (Vector2)transform.position - (Vector2)human.position;
                human.position = Vector2.MoveTowards(human.position, transform.position, suckForce * Time.deltaTime);
            }
        }
    }

    void SmoothRotateBackToInitial()
    {
        float angleDifference = Quaternion.Angle(transform.rotation, initialRotation);
        if (angleDifference > rotationTolerance)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Human"))
        {
            humansToSuck.Add(other.transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Human"))
        {
            humansToSuck.Remove(other.transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (suckingBoxCollider != null)
        {
            Gizmos.matrix = suckingBox.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, suckingBoxCollider.size);
        }
    }
}
