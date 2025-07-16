using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public GameObject Button;  // 按键提示的 UI
    public GameObject talkUI; // 对话框 UI

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
        }
    }

    private void Update()
    {
        // 如果交互提示显示且按下 R 键
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            Button.SetActive(false); 
            talkUI.SetActive(true); // 显示对话框
        }
    }
}