using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Powerup
{
    None,        // 0
    SweetPotato, // 1
    ChiliPepper, // 2
    Carrot,      // 3
    Ice          // 4
}

// Manage player status and UI on playing stage screen
public class StageManager : MonoBehaviour
{
    [SerializeField] bool obtainedRecipe = false;
    [SerializeField] int stageCoins = 0;
    [SerializeField] int stageScore = 0;
    [SerializeField] Powerup powerup = Powerup.None;
    [SerializeField] float stamina = 10.0f;
    [SerializeField] int life = 6;
    [SerializeField] bool isGameOver = false;
    [SerializeField] bool isGameClear = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public Image recipeImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        stageScore += score;
        scoreText.text = stageScore.ToString("D5");
    }

    public void AddCoins(int coins)
    {
        stageCoins += coins;
        UpdateScore(coins * 100);
        coinText.text = "X " + stageCoins.ToString("D2");
    }

    public void ObtainRecipe()
    {
        obtainedRecipe = true;
        UpdateScore(5000);

        // Make UI image color opaque
        Color opaqueColor = recipeImage.color;
        opaqueColor.a = 1.0f;
        recipeImage.color = opaqueColor;
    }

    public void GameClear()
    {
        isGameClear = true;
        GameManager.Instance.UpdateStageClear(stageScore, obtainedRecipe);
    }
}
