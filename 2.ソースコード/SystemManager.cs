using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SystemManager : MonoBehaviour
{
    [SerializeField] GameObject rulePanel;        // ��������I�u�W�F�N�g

    void Update()
    {
        // Esc�������ꂽ��Q�[�����I��������
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    // ���g���C�{�^���������ꂽ��
    public void RetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    // �^�C�g���ɖ߂�{�^���������ꂽ��
    public void TitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    // �Q�[���X�^�[�g���ɑ�������p�l����\��������
    public void RuleBookOn()
    {
        rulePanel.SetActive(true);
    }

}
