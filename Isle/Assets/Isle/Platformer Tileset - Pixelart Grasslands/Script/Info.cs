using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public GameObject Button;  // 按键提示的 UI
    public GameObject infoUI; // 对话框 UI
    public Button okButton;   // OK按钮图片
    
    private void Start()
    {
        // 为OK按钮添加点击事件监听器
        if (okButton != null)
        {
            okButton.onClick.AddListener(CloseDialog);
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
            infoUI.SetActive(false); // 如果对话框还在显示，也关闭它
        }
    }
    
    private void Update()
    {
        // 如果交互提示显示且按下 F 键
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            Button.SetActive(false);
            infoUI.SetActive(true); // 显示对话框
        }
    }
    
    // 关闭对话框的方法
    private void CloseDialog()
    {
        infoUI.SetActive(false); // 关闭对话框
    }
}