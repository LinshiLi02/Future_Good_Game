using UnityEngine;
using TMPro;

public class ResultViewer : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        // 从 PlayerPrefs 中读取多个得分
        int careerScore = PlayerPrefs.GetInt("Career Identity Scale", 0);
        int healthScore = PlayerPrefs.GetInt("The Future Work Self Scale", 0);
        int savingScore = PlayerPrefs.GetInt("SavingScore", 0);

        // 显示结果
        resultText.text =
            "Career Identity Scale: " + careerScore + "\n" +
            "The Future Work Self Scale: " + healthScore + "\n" +
            "💰 Saving Intention Score: " + savingScore;
    }
}
