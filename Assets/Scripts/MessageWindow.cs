using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageWindow : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI messageText;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (panel.activeSelf && (
            Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space)
        ))
        {
            HideMessage();
            // GameManager.Instance.SetState(GameState.EndTurn);
        }
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
        // messageText.text = message;
        // panel.SetActive(true);
    }

    IEnumerator ShowMessageCoroutine(string message)
    {
        yield return new WaitForSeconds(0.5f);
        messageText.text = message;
        panel.SetActive(true);
    }

    public void HideMessage()
    {
        // panel.SetActive(false);
        StartCoroutine(HideMessageCoroutine());
    }

    IEnumerator HideMessageCoroutine()
    {
        panel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.SetState(GameState.EndTurn);
        Debug.Log("MessageWindow Closed");
    }
}
