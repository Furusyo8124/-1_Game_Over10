using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region//�C���X�y�N�^�[��ɕ\��
    [SerializeField] Hand[] hands;               // �v���C���[�ƓG�̂��ꂼ��̎�D�̃X�N���v�g

    [SerializeField] List<int> decks;            // �J�[�h�̎R�D
    [SerializeField] GameObject[] cardObjcts;    // �v���C���Ɏg����J�[�h�I�u�W�F�N�g
    [SerializeField] GameObject[] orderCard;     // �ŏ��̏��Ԍ��ߗp�̃J�[�h�I�u�W�F�N�g
    [SerializeField] GameObject retryButton;     // ���g���C�{�^��
    [SerializeField] GameObject titleButton;     // �^�C�g���o�b�N�{�^��

    [SerializeField] List<int> playerHands;      // �v���C���[�̎�D�̐���
    [SerializeField] List<int> enemyHands_1;     // �G�P�̎�D�̐���
    [SerializeField] List<int> enemyHands_2;     // �G�Q�̎�D�̐���
    [SerializeField] List<int> enemyHands_3;     // �G�R�̎�D�̐���

    [SerializeField] TMP_Text sumNum;            // ��ɏo���J�[�h�̍��v�l
    [SerializeField] TMP_Text sumNumText_1;      // ���v�l�̑���(��������o�����肷��p)
    [SerializeField] TMP_Text sumNumText_2;      // �@�@�@�V
    [SerializeField] TMP_Text turnText;          // ���N�̃^�[����������e�L�X�g
    [SerializeField] TMP_Text orderText;         // �ŏ��̏��Ԍ��ߗp�̃e�L�X�g
    [SerializeField] TMP_Text[] playerNames;     // �e�v���C���[�̈ʒu�̖��O�e�L�X�g
    [SerializeField] TMP_Text[] turnNum;         // �e�v���C���[�̏��ԃe�L�X�g
    [SerializeField] string[] names;             // �e�v���C���[�̖��O

    [SerializeField] GameObject victoryEffect;   // ���������̃G�t�F�N�g
    [SerializeField] GameObject loseEffect;      // ���������̃G�t�F�N�g
    [SerializeField] Transform l_Pos;            // ���������̃G�t�F�N�g�̃|�W�V����
    [SerializeField] Transform v_Pos;            // ���������̃G�t�F�N�g�̃|�W�V����

    [SerializeField] AudioClip card_1;           // �J�[�h�z�鉹
    [SerializeField] AudioClip card_2;           // �J�[�h�߂��鉹
    [SerializeField] AudioClip victory;          // ���������̉�
    [SerializeField] AudioClip lose;             // ���������̉�
    #endregion

    #region//�v���C�x�[�g�ϐ�
    WaitForSeconds wait = new WaitForSeconds(1);    // �R���[�`�����Ŏg���܂킷�p
    int firldCardPos_z = 0;                         // ��ɏo���J�[�h�̉��s��
    bool isOrder = false;                           // �ŏ��̃J�[�h�̃N���b�N����
    int orderNum;                                   // �I�񂾏��Ԃ̐���
    AudioSource audioSource;
    #endregion


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sumNum.enabled = false;
        sumNumText_1.enabled = false;
        sumNumText_2.enabled = false;

        // ���Ԍ��߂̃J�[�h�̓������n�߂�
        StartCoroutine(CardMotion());
    }

    void Update()
    {
        // ��Ƀ��C���΂��Ă���
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // ���C���������Ă���
        if (hit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �ŏ��̏��ԃJ�[�h���N���b�N������
                if (!isOrder && hit.transform.tag == "Order")
                {
                    // �����ŏ��Ԍ��߂̑��̃J�[�h�ɐG��Ȃ�����
                    isOrder = true;

                    // orderNum�ɃN���b�N�����J�[�h�̐�������
                    orderNum = int.Parse(hit.transform.name);

                    // ���C�����������I�u�W�F�N�g�̃X�N���v�g���Ă�
                    // �J�[�h���߂�����orderNum�Ɠ��������̉摜��\��������
                    hit.collider.gameObject.GetComponent<OrderSprite>().OrderSetSprite(orderNum);
                    hit.collider.gameObject.GetComponent<OrderSprite>().
                        StartCoroutine(hit.collider.gameObject.GetComponent<OrderSprite>().OrderCardRotate());

                    orderText.SetText("���Ȃ���" + orderNum + "�Ԗڂł�");

                    // 2�b�Ԋu�������ăv���C���[�h�Ɉڍs����
                    Invoke(nameof(StartMove), 2f);
                }
            }
        }
    }

    // �Q�[���X�^�[�g���̃J�[�h�̉��o�A���Ԍ��߂̐������J�[�h�ɐU�蕪��
    public IEnumerator CardMotion()
    {
        // �J�[�h�𗠕Ԃ�����
        for (int i = 0; i < 4; i++)
        {
            float time = 0.5f;
            float s = Time.time;
            audioSource.PlayOneShot(card_1);
            while (true)
            {
                // 100�����̌���
                float n = Mathf.Min(Time.time - s, time) / time;
                orderCard[i].transform.eulerAngles = new Vector3(0, 180 * n, 0);
                if (n == 1)
                    break;

                yield return null;
            }
        }

        // �J�[�h���ЂƂ܂Ƃ߂ɂ��铮��
        Vector3[] position = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            position[i] = orderCard[i].transform.position;
        }

        {
            // 100�����̎g����
            float time = 0.2f;
            float s = Time.time;
            while (true)
            {
                // 100�����̌���
                float n = Mathf.Min(Time.time - s, time) / time;

                for (int i = 0; i < 4; i++)
                {
                    // ���[�v�@���b�Ԃ�A�n�_����B�n�_�Ɉړ�������
                    orderCard[i].transform.position = Vector3.Lerp(position[i], Vector3.zero, n);
                }

                if (n == 1)
                    break;

                yield return null;
            }
        }

        // �J�[�h���V���b�t�����铮��
        for (int i = 0; i < 3; i++)
        {
            float time = 0.3f;
            float s = Time.time;
            while (true)
            {
                float n = Mathf.Min(Time.time - s, time) / time;

                Vector3 pos = Vector3.zero;
                pos.y = (Mathf.Cos(Mathf.PI * 2 * n) / 2 - 0.5f) * 3;
                pos.z = n < 0.5f ? 1 : -1;
                orderCard[0].transform.position = pos;

                if (n == 1)
                    break;

                yield return null;
            }
            audioSource.PlayOneShot(card_2);
        }

        // �J�[�h�����̈ʒu�ɖ߂�����
        {
            float time = 0.2f;
            float s = Time.time;
            while (true)
            {
                float n = Mathf.Min(Time.time - s, time) / time;

                for (int i = 0; i < 4; i++)
                {
                    // �J�[�h���܂Ƃ߂铮���Ƃ͋t�Ȃ̂�A�n�_�AB�n�_���t�ɂȂ�
                    orderCard[i].transform.position = Vector3.Lerp(Vector3.zero, position[i], n);
                }

                if (n == 1)
                    break;

                yield return null;
            }
        }

        // ���Ԍ��߂̐������V���b�t�����ăJ�[�h�ɓn��
        List<int> orderNum = new List<int>();
        for (int i = 1; i <= 4; i++)
        {
            orderNum.Add(i);
        }
        for (int i = 0; i < orderNum.Count; i++)
        {
            int j = Random.Range(0, orderNum.Count);
            int t = orderNum[i];
            orderNum[i] = orderNum[j];
            orderNum[j] = t;
        }
        for (int i = 0; i < 4; i++)
        {
            orderCard[i].name = orderNum[i].ToString();
            orderCard[i].tag = "Order";
        }

        orderText.SetText("���Ԃ����߂܂�\n�J�[�h��I��ŉ�����");
    }

    // ��D��z���ăQ�[���X�^�[�g�̏���
    private void StartMove()
    {
        // �I�[�_�[�J�[�h�ƃe�L�X�g������
        for (int i = 0; i < 4; i++)
        {
            orderCard[i].SetActive(false);
        }
        orderText.enabled = false;

        // �f�b�L�����
        decks = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                decks.Add(j);
            }
        }

        // �f�b�L�̐������V���b�t������
        for (int i = 0; i < decks.Count; i++)
        {
            int j = Random.Range(0, decks.Count);
            int t = decks[i];
            decks[i] = decks[j];
            decks[j] = t;
        }

        // �v���C���[�̃n���h��z��
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            playerHands.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[0].Add(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM1�̃n���h��z��
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            enemyHands_1.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[1].AddEnemy(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM2�̃n���h��z��
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            enemyHands_2.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[2].AddEnemy(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM3�̃n���h��z��
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            enemyHands_3.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[3].AddEnemy(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        sumNum.enabled = true;
        sumNumText_1.enabled = true;
        sumNumText_2.enabled = true;

        // �v���C���[�B�����ԖڂȂ̂��ŏ��̏��Ԃɂ���ĕ\�L������
        for (int i = 0; i < 4; i++)
        {
            playerNames[i].SetText(names[i]);
            turnNum[i].SetText(((orderNum + i - 1) % 4 + 1) + "�Ԗ�");
        }

        // ���Ԃɂ���Ďn�߂�v���C���[�̐U�蕪��
        if (orderNum == 1)
        {
            StartCoroutine(PlayerMove());
        }
        else if (orderNum == 2)
        {
            StartCoroutine(EnemyMove_3());
        }
        else if (orderNum == 3)
        {
            StartCoroutine(EnemyMove_2());
        }
        else if (orderNum == 4)
        {
            StartCoroutine(EnemyMove_1());
        }
    }

    // �v���C���[�̃n���h�̎q�I�u�W�F�N�g����ύX������
    private void ChildRename()
    {
        Transform[] _ChildCount = new Transform[hands[0].transform.childCount];
        for (int j = 0; j < _ChildCount.Length; j++)
        {
            _ChildCount[j] = hands[0].transform.GetChild(j);
            _ChildCount[j].name = j.ToString();
        }
    }

    // �v���C���[�̃^�[���̓���
    private IEnumerator PlayerMove()
    {
        turnText.SetText("���Ȃ��̃^�[���ł�");
        while (true)
        {
            // update���̃��C���g�킸�ɂ������ł�����x���C���΂��Ă��闝�R�́A
            // �����̃^�[���ȊO�̓J�[�h�ɐڐG����������Ȃ���
            yield return null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    // �v���C���̃n���h���N���b�N������
                    if (hit.transform.tag == "Player")
                    {
                        audioSource.PlayOneShot(card_2);

                        // �I�񂾃J�[�h�̃C���f�b�N�X�ԍ���i�ɑ��
                        int i = int.Parse(hit.transform.name);
                        sumNum.SetText((int.Parse(sumNum.text) + playerHands[i]).ToString());
                        turnText.SetText("");

                        Instantiate(cardObjcts[playerHands[i]], 
                            new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(-20, 20)));

                        // Hand�X�N���v�g��"���\�b�h��"RemoveAt���Ăяo��
                        hands[0].RemoveAt(i);

                        // ��������"List��"RemoveAt�Ȃ̂�playerHands�̃��X�g���������폜���Ă���
                        playerHands.RemoveAt(i);

                        Destroy(hit.transform.gameObject);

                        Invoke("ChildRename", 0.01f);

                        if (SumNumCheck())
                        {
                            audioSource.PlayOneShot(lose);
                            turnText.SetText("���Ȃ��̕����I");
                            Instantiate(loseEffect, l_Pos);
                        }
                        else
                        {
                            StartCoroutine(EnemyMove_1());
                        }
                    }
                    yield break;
                }
            }
        }
    }

    // COM1�̓���
    private IEnumerator EnemyMove_1()
    {
        yield return wait;

        // COM1�̃��x���F�ア(��������������o���Ă���)
        // COM1�̃n���h�̒������ԏ�����������minNum�ɑ��
        int[] values_1 = new int[enemyHands_1.Count];
        for (int i = 0; i < enemyHands_1.Count; i++)
        {
            values_1[i] = enemyHands_1[i];
        }
        int minNum_Com1 = Mathf.Min(values_1);

        // COM1�̃n���h�̒�����minNum�Ɠ���������T���ď�ɏo��
        for (int i = 0; i < enemyHands_1.Count; i++)
        {
            if (enemyHands_1[i] == minNum_Com1)
            {
                audioSource.PlayOneShot(card_2);
                sumNum.SetText((int.Parse(sumNum.text) + minNum_Com1).ToString());
                Instantiate(cardObjcts[enemyHands_1[i]], 
                    new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(80, 120)));
                hands[1].RemoveAtEnemy(i);
                enemyHands_1.RemoveAt(i);
                yield return null;
                Transform[] e1_ChildCount = new Transform[hands[1].transform.childCount];
                for (int k = 0; k < e1_ChildCount.Length; k++)
                {
                    e1_ChildCount[k] = hands[1].transform.GetChild(k);
                    e1_ChildCount[k].name = k.ToString();
                }
                break;
            }
        }
        
        if(SumNumCheck())
        {
            audioSource.PlayOneShot(victory);
            turnText.SetText("COM1�̕����I");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(EnemyMove_2());
        }
    }

    // COM2�̓���
    private IEnumerator EnemyMove_2()
    {
        yield return wait;

        // COM2�̋����F����(��D�̐������D���Ȃ���(�����_��)����o��)
        int randomNum = Random.Range(0, enemyHands_2.Count);//0�`COM2�̌��݂̎�D�̗v�f������1������I��
        audioSource.PlayOneShot(card_2);
        sumNum.SetText((int.Parse(sumNum.text) + enemyHands_2[randomNum]).ToString());
        Instantiate(cardObjcts[enemyHands_2[randomNum]], 
            new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(160, 210)));
        hands[2].RemoveAtEnemy(randomNum);
        enemyHands_2.RemoveAt(randomNum);
        yield return null;
        Transform[] e2_ChildCount = new Transform[hands[2].transform.childCount];
        for (int k = 0; k < e2_ChildCount.Length; k++)
        {
            e2_ChildCount[k] = hands[2].transform.GetChild(k);
            e2_ChildCount[k].name = k.ToString();
        }

        if (SumNumCheck())
        {
            audioSource.PlayOneShot(victory);
            turnText.SetText("COM2�̕����I");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(EnemyMove_3());
        }
    }

    // COM3�̓���
    private IEnumerator EnemyMove_3()
    {
        yield return wait;

        // COM3�̋����F����(�傫����������o���A�傫��������10�𒴂������������珬�����������o��)
        // COM3�̃n���h�̒������ԑ傫��������maxNum�ɑ��
        int[] values_3 = new int[enemyHands_3.Count];
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            values_3[i] = enemyHands_3[i];
        }
        int maxNum_Com3 = Mathf.Max(values_3);

        // COM3�̃n���h�̒������ԏ�����������minNum�ɑ��
        int[] values_3_2 = new int[enemyHands_3.Count];
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            values_3_2[i] = enemyHands_3[i];
        }
        int minNum_Com3 = Mathf.Min(values_3_2);

        // COM3�̃n���h�̒�����maxNum�Ɠ���������T���ď�ɏo��
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            if (enemyHands_3[i] == maxNum_Com3)
            {
                // ��D�ň�ԑ傫���������o�������A10�𒴂��Ȃ���΂��̂܂܏o��
                if ((int.Parse(sumNum.text) + enemyHands_3[i]) < 10)
                {
                    audioSource.PlayOneShot(card_2);
                    sumNum.SetText((int.Parse(sumNum.text) + maxNum_Com3).ToString());
                    Instantiate(cardObjcts[enemyHands_3[i]],
                        new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(-120, -60)));
                    hands[3].RemoveAtEnemy(i);
                    enemyHands_3.RemoveAt(i);
                    yield return null;
                    Transform[] e3_ChildCount = new Transform[hands[3].transform.childCount];
                    for (int k = 0; k < e3_ChildCount.Length; k++)
                    {
                        e3_ChildCount[k] = hands[3].transform.GetChild(k);
                        e3_ChildCount[k].name = k.ToString();
                    }
                    break;
                }

                // ��D�ň�ԑ傫���������o�������A10�𒴂���Ȃ珬����������T���ďo��
                else if ((int.Parse(sumNum.text) + enemyHands_3[i]) >= 10)
                {
                    for (int j = 0; j < enemyHands_3.Count; j++)
                    {
                        if (enemyHands_3[j] == minNum_Com3)
                        {
                            audioSource.PlayOneShot(card_2);
                            sumNum.SetText((int.Parse(sumNum.text) + minNum_Com3).ToString());
                            Instantiate(cardObjcts[enemyHands_3[j]],
                                new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(-120, -60)));
                            hands[3].RemoveAtEnemy(j);
                            enemyHands_3.RemoveAt(j);
                            yield return null;
                            Transform[] e3_ChildCount = new Transform[hands[3].transform.childCount];
                            for (int k = 0; k < e3_ChildCount.Length; k++)
                            {
                                e3_ChildCount[k] = hands[3].transform.GetChild(k);
                                e3_ChildCount[k].name = k.ToString();
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        if (SumNumCheck())
        {
            audioSource.PlayOneShot(victory);
            turnText.SetText("COM3�̕����I");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(PlayerMove());
        }
    }

    // ���v�l���P�O�𒴂��Ă��Ȃ����`�F�b�N
    public bool SumNumCheck()
    {
        int numCheck = int.Parse(sumNum.text);
        if (numCheck >= 10)
        {
            retryButton.SetActive(true);
            titleButton.SetActive(true);
            return true;
        }

        return false;
    }
}
