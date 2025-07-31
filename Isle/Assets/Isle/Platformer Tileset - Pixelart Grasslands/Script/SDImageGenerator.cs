using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SDImageGenerator : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("输入生成图片的 3 条文字提示")]
    public string[] prompts = new string[3]; // 在 Inspector 中输入 3 条 Prompt

    [Header("UI")]
    public RawImage[] outputImages; // 用于显示三张图片

    [Header("Stability API")]
    public string apiKey = "sk-motVbYqoqFwugTxRAKKkMVFN3L2WbIVJdKx2rGj0nWeohI7d";  // ⚠️ 替换为你的 API Key
    public string apiUrl = "https://api.stability.ai/v1alpha/generation/stable-diffusion-xl-1024-v1-0/text-to-image";  // ⚠️ 替换为你的 API URL

    /// <summary>
    void Start()
    {
        // 检查每个 RawImage 并设置为透明
        foreach (RawImage image in outputImages)
        {
            if (image != null)
            {
                image.color = new Color(0, 0, 0, 0); // 设置为完全透明
            }
        }
    }
    /// 点击按钮触发图片生成
    /// </summary>
    public void OnGenerateClick()
    {
        // 检查 Prompt 数量和内容是否正确
        if (prompts.Length != 3 || string.IsNullOrEmpty(prompts[0]) || string.IsNullOrEmpty(prompts[1]) || string.IsNullOrEmpty(prompts[2]))
        {
            Debug.LogWarning("⚠️ 请确保输入 3 条非空的 Prompt！");
            return;
        }

        Debug.Log("🚀 Prompts entered: " + string.Join(", ", prompts));
        StartCoroutine(GenerateImages(prompts));
    }

    /// <summary>
    /// 协程：按照每个 Prompt 单独发送请求并生成图片
    /// </summary>
    IEnumerator GenerateImages(string[] prompts)
    {
        for (int i = 0; i < prompts.Length; i++)
        {
            // 构造单独的请求体
            RequestBody body = new RequestBody
            {
                text_prompts = new TextPrompt[] { new TextPrompt { text = prompts[i] } },
                cfg_scale = 7,
                height = 512,
                width = 512,
                samples = 1, // 每次生成 1 张图片
                steps = 30
            };

            string jsonData = JsonUtility.ToJson(body);
            Debug.Log($"📄 Sending request for Prompt {i + 1}:\n{jsonData}");

            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string resultJson = request.downloadHandler.text;
                Debug.Log($"✅ Prompt {i + 1} Response:\n{resultJson}");

                Artifact[] artifacts = ExtractArtifactsFromResponse(resultJson);
                if (artifacts != null && artifacts.Length > 0)
                {
                    string base64Image = artifacts[0].base64; // 获取第一张图片
                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        byte[] imageBytes = System.Convert.FromBase64String(base64Image);
                        Texture2D tex = new Texture2D(2, 2);
                        if (tex.LoadImage(imageBytes))
                        {
                            Debug.Log($"✅ Image {i + 1} successfully loaded.");
                            outputImages[i].texture = tex; // 将图片显示到对应的 RawImage
                            outputImages[i].color = Color.white; // 恢复不透明
                        }
                        else
                        {
                            Debug.LogError($"❌ Failed to decode texture for Prompt {i + 1}.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ No image found in response for Prompt {i + 1}.");
                }
            }
            else
            {
                Debug.LogError($"❌ Request failed for Prompt {i + 1}: {request.error}");
            }
        }
    }

    /// <summary>
    /// 从 JSON 响应中提取图片信息
    /// </summary>
    Artifact[] ExtractArtifactsFromResponse(string json)
    {
        try
        {
            SDResponse response = JsonUtility.FromJson<SDResponse>(json);
            return response.artifacts;
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ JSON parsing failed: " + e.Message);
        }
        return null;
    }

    // JSON 结构体类定义
    [System.Serializable]
    public class TextPrompt
    {
        public string text;
    }

    [System.Serializable]
    public class RequestBody
    {
        public TextPrompt[] text_prompts;
        public int cfg_scale;
        public int height;
        public int width;
        public int samples;
        public int steps;
    }

    [System.Serializable]
    public class Artifact
    {
        public string base64;
    }

    [System.Serializable]
    public class SDResponse
    {
        public Artifact[] artifacts;
    }
}