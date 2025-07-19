using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionSimple : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName;

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
