﻿using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{

    public TextMeshProUGUI currentScoreCounter;
    public TextMeshProUGUI highestScoreCounter;
    public GameObject leaderboardsButton;

    private void Awake()
    {
        if (Game.IsPlayServicesEnabled)
            leaderboardsButton.SetActive(true);
    }

    private void Start()
    {
        if (Player.Score > Game.Settings.HighestScore)
        {
            if (Game.IsPlayServicesEnabled)
            {
                var leaderboard = Social.CreateLeaderboard();
                leaderboard.id = GPGSIds.leaderboard_overall_high_scores;
                leaderboard.LoadScores(success =>
                {
                    if (!success)
                        return;
                    var highestScoreOverall = -1;
                    foreach (var score in leaderboard.scores)
                        if (score.rank <= 1)
                            highestScoreOverall = score.rank;
                    if (highestScoreOverall <= 0)
                        return;
                    if (Player.Score > highestScoreOverall)
                        Social.ReportProgress(GPGSIds.achievement_the_winner_of_the_blue_power_core, 100, success2 =>
                        {
                            if (success2)
                                Debug.Log("[GPGS] Achieved the winner of the Blue Power Core!");
                            else
                                Debug.LogError("[GPGS] Failed to unlock achievement!");
                        });
                });
                Social.ReportScore(Player.Score, GPGSIds.leaderboard_overall_high_scores, success =>
                {
                    if (success)
                        Debug.Log("[GPGS] Posted to leaderboards!");
                    else
                        Debug.LogError("[GPGS] Unable to post to leaderboards!");
                });
            }
            Game.Settings.HighestScore = Player.Score;
            Game.Settings.Save();
        }
        currentScoreCounter.text = "CURRENT SCORE: " + Player.Score;
        highestScoreCounter.text = "HIGHEST SCORE: " + Game.Settings.HighestScore;
        if (Player.Deaths >= 5)
        {
            if (Advertisement.isInitialized && Advertisement.IsReady())
                Advertisement.Show();
            Player.Deaths = 0;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void ShowLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }
    
}