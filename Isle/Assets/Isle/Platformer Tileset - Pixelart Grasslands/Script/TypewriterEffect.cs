using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI textComponent;
    public CanvasGroup uiCanvasGroup;       // 控制 UI 透明度
    public CanvasGroup blackOverlay;        // 黑幕
    public AudioSource typeSound;

    [Header("Typing Settings")]
    public float typeSpeed = 0.05f;
    public float paragraphDelay = 0.1f;

    [TextArea(3, 10)]
    public string[] paragraphs;

    [Header("Scene Transition")]
    public string nextSceneName;

    private int currentIndex = 0;
    private bool waitingForClick = false;

    private void Start()
    {
        // 黑幕一开始就是透明的，不再渐入
        if (blackOverlay != null)
            blackOverlay.alpha = 0f;

        // UI 一开始也设为完全可见
        if (uiCanvasGroup != null)
            uiCanvasGroup.alpha = 1f;

        // 播放第一个段落
        StartCoroutine(PlayParagraph(currentIndex));
    }

    private void Update()
    {
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            waitingForClick = false;
            currentIndex++;

            if (currentIndex < paragraphs.Length)
            {
                StartCoroutine(PlayParagraph(currentIndex));
            }
            else
            {
                Debug.Log("文字播放完毕，准备跳转场景");
                StartCoroutine(FadeAndLoadScene());
            }
        }
    }

    IEnumerator PlayParagraph(int index)
    {
        textComponent.text = "";

        foreach (char c in paragraphs[index])
        {
            textComponent.text += c;

            if (!char.IsWhiteSpace(c) && typeSound != null)
            {
                typeSound.pitch = Random.Range(0.95f, 1.05f);
                typeSound.Play();
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        yield return new WaitForSeconds(paragraphDelay);
        waitingForClick = true;
    }

    IEnumerator FadeAndLoadScene()
    {
        LeanTween.alphaCanvas(blackOverlay, 1f, 1f);  // 黑幕渐显
        yield return new WaitForSeconds(1.2f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("未设置 nextSceneName，无法跳转场景！");
        }
    }
}
