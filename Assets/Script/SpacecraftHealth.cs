using UnityEngine;
using System.Collections;
using LootLocker.Requests;
using UnityEngine.SceneManagement;

public class SpacecraftHealth : MonoBehaviour
{
    public GameObject healthIconPrefab; // The health icon prefab
    public Transform healthIconsParent; // Parent object to hold health icons
    public int maxHealth = 5; // Maximum health
    public Vector3 healthIconScale = new Vector3(1f, 1f, 1f); // Scale for the health icons

    private int currentHealth;
    private GameObject[] healthIcons;
    private SpacecraftController spacecraft;

    void Start()
    {
        currentHealth = maxHealth;
        healthIcons = new GameObject[maxHealth];
        spacecraft = GetComponent<SpacecraftController>();

        // Instantiate health icons
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject icon = Instantiate(healthIconPrefab, healthIconsParent);
            icon.transform.localPosition = new Vector3(i * 30, 0, 0); // Adjust position as needed
            icon.transform.localScale = healthIconScale; // Set the scale of the health icon
            healthIcons[i] = icon;
        }
    }

    public void TakeDamage()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            Destroy(healthIcons[currentHealth]);

            if (currentHealth == 0)
            {
                Lose();
            }
        }
    }

    private void Lose()
    {
        Debug.Log("Spacecraft lost!");
        StartCoroutine(SubmitScoreRoutine(spacecraft.score));
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, "23029", (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
