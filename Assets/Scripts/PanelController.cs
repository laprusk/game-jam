using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // パネルを格納する配列
    private int currentPanelIndex = 0;

    void Start()
    {
        // シーン開始時にすべてのパネルを非表示にする
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // 最初のパネルを表示する
    public void ShowFirstPanel()
    {
        if (panels.Length > 0)
        {
            panels[0].SetActive(true);
            currentPanelIndex = 0; // 最初のパネルのインデックスを設定
        }
    }

    // 次のパネルを表示する
    public void ShowNextPanel()
    {
        if (currentPanelIndex < panels.Length - 1)
        {
            panels[currentPanelIndex].SetActive(false);
            currentPanelIndex++;
            panels[currentPanelIndex].SetActive(true);
        }
    }

    // すべてのパネルを閉じる
    public void ClosePanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
