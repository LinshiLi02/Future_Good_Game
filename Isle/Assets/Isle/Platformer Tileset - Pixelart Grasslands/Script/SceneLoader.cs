using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTargetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
