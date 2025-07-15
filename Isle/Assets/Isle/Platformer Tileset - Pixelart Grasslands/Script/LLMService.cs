using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

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
    
    public System.Action<string> OnResponseReceived;
    public System.Action<string> OnErrorOccurred;
    
    private void Start()
    {
        // Initialize conversation with system prompt
        if (APISettings.Instance != null)
        {
            conversationHistory.Add(new ChatMessage
            {
                role = "system",
                content = APISettings.Instance.systemPrompt
            });
        }
    }
    
    public void SendMessage(string userMessage)
    {
        if (APISettings.Instance == null)
        {
            OnErrorOccurred?.Invoke("API Settings not found!");
            return;
        }
        
        // Add user message to conversation history
        conversationHistory.Add(new ChatMessage
        {
            role = "user",
            content = userMessage
        });
        
        // Start API call
        StartCoroutine(CallOpenAI());
    }
    
    private IEnumerator CallOpenAI()
    {
        // Create request payload
        ChatRequest request = new ChatRequest
        {
            model = APISettings.Instance.model,
            messages = conversationHistory,
            max_tokens = APISettings.Instance.maxTokens,
            temperature = APISettings.Instance.temperature
        };
        
        string jsonRequest = JsonUtility.ToJson(request);
        
        // Create UnityWebRequest
        using (UnityWebRequest webRequest = new UnityWebRequest(APISettings.Instance.baseUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + APISettings.Instance.apiKey);
            
            // Send request
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Parse response
                ChatResponse response = JsonUtility.FromJson<ChatResponse>(webRequest.downloadHandler.text);
                
                if (response.choices != null && response.choices.Count > 0)
                {
                    string assistantMessage = response.choices[0].message.content;
                    
                    // Add assistant response to conversation history
                    conversationHistory.Add(new ChatMessage
                    {
                        role = "assistant",
                        content = assistantMessage
                    });
                    
                    // Trigger callback
                    OnResponseReceived?.Invoke(assistantMessage);
                }
                else
                {
                    OnErrorOccurred?.Invoke("No response from API");
                }
            }
            else
            {
                OnErrorOccurred?.Invoke("API Error: " + webRequest.error);
            }
        }
    }
    
    // Method to clear conversation history
    public void ClearConversation()
    {
        conversationHistory.Clear();
        
        // Re-add system prompt
        if (APISettings.Instance != null)
        {
            conversationHistory.Add(new ChatMessage
            {
                role = "system",
                content = APISettings.Instance.systemPrompt
            });
        }
    }
    
    // Method to get conversation history (for debugging)
    public List<ChatMessage> GetConversationHistory()
    {
        return new List<ChatMessage>(conversationHistory);
    }
} 