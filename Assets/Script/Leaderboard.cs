using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    string leaderBoardID = "23029";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;
    public TMP_InputField playerNameInputfield;

    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(SetUpRoutine());
    }

    public void SetPlayerName()
    {
        if (playerNameInputfield.text != "")
        {
            LootLockerSDKManager.SetPlayerName(playerNameInputfield.text, (response) =>
            {
                if (response.success)
                {
                    Debug.Log("Success set name");
                }
                else
                {
                    Debug.Log("Can't set name");
                }
            });
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    IEnumerator SetUpRoutine()
    {
        yield return LoginRoutine();
        yield return FetchTopHighScoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    IEnumerator FetchTopHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderBoardID, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Name\n";
                string tempPlayerScores = "Score\n";

                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Could not fetch scores");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
