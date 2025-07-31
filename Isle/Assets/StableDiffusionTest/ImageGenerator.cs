using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ImageGenerator : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField promptInput;
    public RawImage outputImage;

    [Header("Stability API")]
    public string apiKey = "sk-motVbYqoqFwugTxRAKKkMVFN3L2WbIVJdKx2rGj0nWeohI7d";  // ⚠️ 替换为你的 API Key
    private string apiUrl = "https://api.stability.ai/v1alpha/generation/stable-diffusion-xl-1024-v1-0/text-to-image";



    public void OnGenerateClick()
    {
        string prompt = promptInput.text;
        if (!string.IsNullOrEmpty(prompt))
        {
            Debug.Log("🚀 Prompt entered: " + prompt);
            StartCoroutine(GenerateImage(prompt));
        }
        else
        {
            Debug.LogWarning("⚠️ Prompt is empty.");
        }
    }

    IEnumerator GenerateImage(string prompt)
    {
        // 构造结构化 JSON 请求体
        RequestBody body = new RequestBody
        {
            text_prompts = new TextPrompt[] { new TextPrompt { text = prompt } },
            cfg_scale = 7,
            height = 512,
            width = 512,
            samples = 1,
            steps = 30
        };

        string jsonData = JsonUtility.ToJson(body);
        Debug.Log("🧾 Final JSON payload:\n" + jsonData);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ API Request Failed: " + request.error);
            yield break;
        }

        string resultJson = request.downloadHandler.text;
        Debug.Log("✅ API Response received.");
        Debug.Log("📄 Raw JSON:\n" + resultJson);

        string base64Image = ExtractBase64FromResponse(resultJson);

        if (!string.IsNullOrEmpty(base64Image))
        {
            Debug.Log("🖼️ Base64 image extracted. Length: " + base64Image.Length);
            byte[] imageBytes = System.Convert.FromBase64String(base64Image);
            Texture2D tex = new Texture2D(2, 2);
            bool loaded = tex.LoadImage(imageBytes);
            if (loaded)
            {
                Debug.Log("✅ Texture decoded. Size: " + tex.width + "x" + tex.height);
                outputImage.texture = tex;
            }
            else
            {
                Debug.LogError("❌ Failed to decode texture.");
            }
        }
        else
        {
            Debug.LogError("❌ No image found in response.");
        }
    }

    // 提取 base64 图像字符串
    string ExtractBase64FromResponse(string json)
    {
        try
        {
            SDResponse response = JsonUtility.FromJson<SDResponse>(json);
            if (response.artifacts != null && response.artifacts.Length > 0)
            {
                return response.artifacts[0].base64;
            }
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
