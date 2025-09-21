using UnityEngine;
using TMPro;

public class GameManager3D : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI cutsText;
    public TextMeshProUGUI resultText;

    [Header("Rules")]
    public int requiredCuts = 5;

    int cuts = 0;
    bool gameOver = false;

    void Start()
    {
        // 游戏开始时隐藏结果文本
        if (resultText != null)
            resultText.gameObject.SetActive(false);
        UpdateCutsUI();
    }

    public void OnSuccessfulCut()
    {
        if (gameOver) return;
        cuts++;
        UpdateCutsUI();
        if (cuts >= requiredCuts)
        {
            gameOver = true;
            ShowResult("You win!", new Color(0.1f, 1f, 0.1f));
        }
    }

    public void OnMiss()
    {
        if (gameOver) return;
        gameOver = true;
        ShowResult("You failed", Color.red);
    }

    void UpdateCutsUI()
    {
        if (cutsText != null)
            cutsText.text = $"Cuts: {cuts}/{requiredCuts}";
    }

    void ShowResult(string msg, Color col)
    {
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true); // 显示结果文本
            resultText.text = msg;
            resultText.color = col;
        }
    }
}