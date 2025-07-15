using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

[System.Serializable]
public class ChatMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatRequest
{
    public string model;
    public List<ChatMessage> messages;
    public int max_tokens;
    public float temperature;
}

[System.Serializable]
public class ChatResponse
{
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string content;
}

public class LLMService : MonoBehaviour
{
    private List<ChatMessage> conversationHistory = new List<ChatMessage>();
    
    public delegate void OnResponseReceived(string response);
    public event OnResponseReceived ResponseReceived;
    
    public delegate void OnErrorOccurred(string error);
    public event OnErrorOccurred ErrorOccurred;
    
    private void Start()
    {
        // Initialize conversation with system prompt
        if (APISettings.Instance != null)
        {
            AddSystemMessage(APISettings.Instance.GetSystemPrompt());
        }
    }
    
    public void AddSystemMessage(string content)
    {
        conversationHistory.Add(new ChatMessage { role = "system", content = content });
    }
    
    public void AddUserMessage(string content)
    {
        conversationHistory.Add(new ChatMessage { role = "user", content = content });
    }
    
    public void AddAssistantMessage(string content)
    {
        conversationHistory.Add(new ChatMessage { role = "assistant", content = content });
    }
    
    public void SendMessage(string userMessage)
    {
        if (APISettings.Instance == null)
        {
            ErrorOccurred?.Invoke("API Settings not found!");
            return;
        }
        
        AddUserMessage(userMessage);
        StartCoroutine(SendChatRequest());
    }
    
    private IEnumerator SendChatRequest()
    {
        var request = new ChatRequest
        {
            model = APISettings.Instance.model,
            messages = conversationHistory,
            max_tokens = APISettings.Instance.maxTokens,
            temperature = APISettings.Instance.temperature
        };
        
        string jsonRequest = JsonUtility.ToJson(request);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        
        using (UnityWebRequest webRequest = new UnityWebRequest(APISettings.Instance.apiEndpoint, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + APISettings.Instance.apiKey);
            
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    ChatResponse response = JsonUtility.FromJson<ChatResponse>(webRequest.downloadHandler.text);
                    if (response.choices != null && response.choices.Count > 0)
                    {
                        string assistantResponse = response.choices[0].message.content;
                        AddAssistantMessage(assistantResponse);
                        ResponseReceived?.Invoke(assistantResponse);
                    }
                    else
                    {
                        ErrorOccurred?.Invoke("No response from API");
                    }
                }
                catch (Exception e)
                {
                    ErrorOccurred?.Invoke("Failed to parse response: " + e.Message);
                }
            }
            else
            {
                ErrorOccurred?.Invoke("API request failed: " + webRequest.error);
            }
        }
    }
    
    public void ClearConversation()
    {
        conversationHistory.Clear();
        if (APISettings.Instance != null)
        {
            AddSystemMessage(APISettings.Instance.GetSystemPrompt());
        }
    }
} 