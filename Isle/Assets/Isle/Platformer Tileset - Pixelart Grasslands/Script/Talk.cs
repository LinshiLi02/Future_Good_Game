using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    public GameObject Button;  // 按键提示的 UI
    public GameObject talkUI; // 对话框 UI
    public Button okButton;   // OK按钮 (reused as send button)
    public InputField inputField; // 输入框
    public Text npcResponseText; // NPC回复文本
    public Text playerInputText; // 玩家输入文本
    
    private LLMService llmService;
    private bool isConversationActive = false;
    private bool isWaitingForResponse = false;
    
    private void Start()
    {
        // 获取LLM服务
        llmService = FindObjectOfType<LLMService>();
        if (llmService == null)
        {
            // 如果没有找到LLM服务，创建一个
            GameObject llmServiceObj = new GameObject("LLMService");
            llmService = llmServiceObj.AddComponent<LLMService>();
        }
        
        // 订阅LLM事件
        llmService.ResponseReceived += OnLLMResponse;
        llmService.ErrorOccurred += OnLLMError;
        
        // 为OK按钮添加点击事件监听器 (reused as send button)
        if (okButton != null)
        {
            okButton.onClick.AddListener(SendMessage);
        }
        
        // 为输入框添加回车键监听
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 确保只有主角触发
        if (other.CompareTag("Player"))
        {
            Button.SetActive(true); // 显示交互提示
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 确保只有主角离开时触发
        if (other.CompareTag("Player"))
        {
            Button.SetActive(false); // 关闭交互提示
            if (!isConversationActive)
            {
                talkUI.SetActive(false); // 关闭对话框
            }
        }
    }

    private void Update()
    {
        // 如果交互提示显示且按下 R 键
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Button.SetActive(false); 
            StartConversation();
        }
        
        // 如果对话正在进行，按ESC键关闭对话
        if (isConversationActive && Input.GetKeyDown(KeyCode.Escape))
        {
            EndConversation();
        }
    }
    
    private void StartConversation()
    {
        talkUI.SetActive(true);
        isConversationActive = true;
        
        // 清空之前的对话
        if (npcResponseText != null)
        {
            npcResponseText.text = "";
        }
        if (playerInputText != null)
        {
            playerInputText.text = "";
        }
        if (inputField != null)
        {
            inputField.text = "";
            inputField.interactable = true;
        }
        
        // 设置OK按钮为发送模式
        if (okButton != null)
        {
            var buttonText = okButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Send";
            }
        }
        
        // 发送初始消息给LLM
        llmService.SendMessage("Hello");
    }
    
    private void SendMessage()
    {
        if (!isConversationActive || isWaitingForResponse)
            return;
            
        string message = inputField != null ? inputField.text.Trim() : "";
        if (string.IsNullOrEmpty(message))
            return;
            
        // 显示玩家消息
        if (playerInputText != null)
        {
            playerInputText.text = "You: " + message;
        }
        
        // 清空输入框
        if (inputField != null)
        {
            inputField.text = "";
        }
        
        // 显示等待状态
        if (npcResponseText != null)
        {
            npcResponseText.text = "Elvin is thinking...";
        }
        
        isWaitingForResponse = true;
        
        // 发送消息给LLM
        llmService.SendMessage(message);
    }
    
    private void OnInputFieldEndEdit(string text)
    {
        // 当玩家按回车键时发送消息
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMessage();
        }
    }
    
    private void OnLLMResponse(string response)
    {
        isWaitingForResponse = false;
        
        // 显示NPC回复
        if (npcResponseText != null)
        {
            npcResponseText.text = "Elvin: " + response;
        }
        
        // 重新启用输入框
        if (inputField != null)
        {
            inputField.interactable = true;
        }
    }
    
    private void OnLLMError(string error)
    {
        isWaitingForResponse = false;
        
        // 显示错误信息
        if (npcResponseText != null)
        {
            npcResponseText.text = "Error: " + error;
        }
        
        // 重新启用输入框
        if (inputField != null)
        {
            inputField.interactable = true;
        }
    }
    
    private void EndConversation()
    {
        isConversationActive = false;
        talkUI.SetActive(false);
        
        // 重置按钮文本
        if (okButton != null)
        {
            var buttonText = okButton.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "OK";
            }
        }
    }
    
    private void OnDestroy()
    {
        // 取消订阅事件
        if (llmService != null)
        {
            llmService.ResponseReceived -= OnLLMResponse;
            llmService.ErrorOccurred -= OnLLMError;
        }
    }
}
