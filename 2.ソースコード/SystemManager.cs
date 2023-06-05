using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemManager : MonoBehaviour
{
    [SerializeField] GameObject rulePanel;        // 操作説明オブジェクト

    void Update()
    {
        // Escが押されたらゲームを終了させる
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    // リトライボタンが押された時
    public void RetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // タイトルに戻るボタンが押された時
    public void TitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    // ゲームスタート時に操作説明パネルを表示させる
    public void RuleBookOn()
    {
        rulePanel.SetActive(true);
    }

}
