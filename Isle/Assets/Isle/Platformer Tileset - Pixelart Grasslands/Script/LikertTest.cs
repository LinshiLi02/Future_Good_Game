using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LikertTest : MonoBehaviour
{
    [Header("optionButtons")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;

    [Header("Scoring Config")]
    public string scoreKey = "DefaultScoreKey";  // ⚠️ 自定义保存的键（用于不同测试项）

    private int selectedScore = 0;

    void Start()
    {
        // 添加监听器：使用闭包捕获 score 值
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int score = i + 1; // Likert 分数从 1 开始
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(score));
        }
    }

    void OnOptionSelected(int score)
    {
        selectedScore = score;

        Debug.Log($"[{scoreKey}] Selected Score: {selectedScore}");

        // 保存至 PlayerPrefs，供后续场景使用
        PlayerPrefs.SetInt(scoreKey, selectedScore);
    }
}
