using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region//インスペクター上に表示
    [SerializeField] Hand[] hands;               // プレイヤーと敵のそれぞれの手札のスクリプト

    [SerializeField] List<int> decks;            // カードの山札
    [SerializeField] GameObject[] cardObjcts;    // プレイ中に使われるカードオブジェクト
    [SerializeField] GameObject[] orderCard;     // 最初の順番決め用のカードオブジェクト
    [SerializeField] GameObject retryButton;     // リトライボタン
    [SerializeField] GameObject titleButton;     // タイトルバックボタン

    [SerializeField] List<int> playerHands;      // プレイヤーの手札の数字
    [SerializeField] List<int> enemyHands_1;     // 敵１の手札の数字
    [SerializeField] List<int> enemyHands_2;     // 敵２の手札の数字
    [SerializeField] List<int> enemyHands_3;     // 敵３の手札の数字

    [SerializeField] TMP_Text sumNum;            // 場に出たカードの合計値
    [SerializeField] TMP_Text sumNumText_1;      // 合計値の装飾(消したり出したりする用)
    [SerializeField] TMP_Text sumNumText_2;      // 　　　〃
    [SerializeField] TMP_Text turnText;          // 今誰のターンか教えるテキスト
    [SerializeField] TMP_Text orderText;         // 最初の順番決め用のテキスト
    [SerializeField] TMP_Text[] playerNames;     // 各プレイヤーの位置の名前テキスト
    [SerializeField] TMP_Text[] turnNum;         // 各プレイヤーの順番テキスト
    [SerializeField] string[] names;             // 各プレイヤーの名前

    [SerializeField] GameObject victoryEffect;   // 勝った時のエフェクト
    [SerializeField] GameObject loseEffect;      // 負けた時のエフェクト
    [SerializeField] Transform l_Pos;            // 負けた時のエフェクトのポジション
    [SerializeField] Transform v_Pos;            // 勝った時のエフェクトのポジション

    [SerializeField] AudioClip card_1;           // カード配る音
    [SerializeField] AudioClip card_2;           // カードめくる音
    [SerializeField] AudioClip victory;          // 勝った時の音
    [SerializeField] AudioClip lose;             // 負けた時の音
    #endregion

    #region//プライベート変数
    WaitForSeconds wait = new WaitForSeconds(1);    // コルーチン内で使いまわす用
    int firldCardPos_z = 0;                         // 場に出すカードの奥行き
    bool isOrder = false;                           // 最初のカードのクリック判定
    int orderNum;                                   // 選んだ順番の数字
    AudioSource audioSource;
    #endregion


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sumNum.enabled = false;
        sumNumText_1.enabled = false;
        sumNumText_2.enabled = false;

        // 順番決めのカードの動きを始める
        StartCoroutine(CardMotion());
    }

    void Update()
    {
        // 常にレイを飛ばしている
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // レイが当たってかつ
        if (hit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 最初の順番カードをクリックした時
                if (!isOrder && hit.transform.tag == "Order")
                {
                    // ここで順番決めの他のカードに触れなくする
                    isOrder = true;

                    // orderNumにクリックしたカードの数字を代入
                    orderNum = int.Parse(hit.transform.name);

                    // レイが当たったオブジェクトのスクリプトを呼ぶ
                    // カードをめくってorderNumと同じ数字の画像を表示させる
                    hit.collider.gameObject.GetComponent<OrderSprite>().OrderSetSprite(orderNum);
                    hit.collider.gameObject.GetComponent<OrderSprite>().
                        StartCoroutine(hit.collider.gameObject.GetComponent<OrderSprite>().OrderCardRotate());

                    orderText.SetText("あなたは" + orderNum + "番目です");

                    // 2秒間隔をあけてプレイモードに移行する
                    Invoke(nameof(StartMove), 2f);
                }
            }
        }
    }

    // ゲームスタート時のカードの演出、順番決めの数字をカードに振り分け
    public IEnumerator CardMotion()
    {
        // カードを裏返す動き
        for (int i = 0; i < 4; i++)
        {
            float time = 0.5f;
            float s = Time.time;
            audioSource.PlayOneShot(card_1);
            while (true)
            {
                // 100分率の公式
                float n = Mathf.Min(Time.time - s, time) / time;
                orderCard[i].transform.eulerAngles = new Vector3(0, 180 * n, 0);
                if (n == 1)
                    break;

                yield return null;
            }
        }

        // カードをひとまとめにする動き
        Vector3[] position = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            position[i] = orderCard[i].transform.position;
        }

        {
            // 100分率の使い方
            float time = 0.2f;
            float s = Time.time;
            while (true)
            {
                // 100分率の公式
                float n = Mathf.Min(Time.time - s, time) / time;

                for (int i = 0; i < 4; i++)
                {
                    // ラープ　何秒間でA地点からB地点に移動させる
                    orderCard[i].transform.position = Vector3.Lerp(position[i], Vector3.zero, n);
                }

                if (n == 1)
                    break;

                yield return null;
            }
        }

        // カードをシャッフルする動き
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

        // カードを元の位置に戻す動き
        {
            float time = 0.2f;
            float s = Time.time;
            while (true)
            {
                float n = Mathf.Min(Time.time - s, time) / time;

                for (int i = 0; i < 4; i++)
                {
                    // カードをまとめる動きとは逆なのでA地点、B地点が逆になる
                    orderCard[i].transform.position = Vector3.Lerp(Vector3.zero, position[i], n);
                }

                if (n == 1)
                    break;

                yield return null;
            }
        }

        // 順番決めの数字をシャッフルしてカードに渡す
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

        orderText.SetText("順番を決めます\nカードを選んで下さい");
    }

    // 手札を配ってゲームスタートの準備
    private void StartMove()
    {
        // オーダーカードとテキストを消す
        for (int i = 0; i < 4; i++)
        {
            orderCard[i].SetActive(false);
        }
        orderText.enabled = false;

        // デッキを作る
        decks = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                decks.Add(j);
            }
        }

        // デッキの数字をシャッフルする
        for (int i = 0; i < decks.Count; i++)
        {
            int j = Random.Range(0, decks.Count);
            int t = decks[i];
            decks[i] = decks[j];
            decks[j] = t;
        }

        // プレイヤーのハンドを配る
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            playerHands.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[0].Add(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM1のハンドを配る
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            enemyHands_1.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[1].AddEnemy(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM2のハンドを配る
        for (int i = 0; i < 4; i++)
        {
            int num = decks[0];
            enemyHands_2.Add(num);
            Transform t = Instantiate(cardObjcts[num]).transform;
            hands[2].AddEnemy(t);
            t.name = i.ToString();
            decks.Remove(decks[0]);
        }

        // COM3のハンドを配る
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

        // プレイヤー達が何番目なのか最初の順番によって表記させる
        for (int i = 0; i < 4; i++)
        {
            playerNames[i].SetText(names[i]);
            turnNum[i].SetText(((orderNum + i - 1) % 4 + 1) + "番目");
        }

        // 順番によって始めるプレイヤーの振り分け
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

    // プレイヤーのハンドの子オブジェクト名を変更させる
    private void ChildRename()
    {
        Transform[] _ChildCount = new Transform[hands[0].transform.childCount];
        for (int j = 0; j < _ChildCount.Length; j++)
        {
            _ChildCount[j] = hands[0].transform.GetChild(j);
            _ChildCount[j].name = j.ToString();
        }
    }

    // プレイヤーのターンの動き
    private IEnumerator PlayerMove()
    {
        turnText.SetText("あなたのターンです");
        while (true)
        {
            // update内のレイを使わずにこっちでもう一度レイを飛ばしている理由は、
            // 自分のターン以外はカードに接触判定をさせない為
            yield return null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    // プレイ中のハンドをクリックした時
                    if (hit.transform.tag == "Player")
                    {
                        audioSource.PlayOneShot(card_2);

                        // 選んだカードのインデックス番号をiに代入
                        int i = int.Parse(hit.transform.name);
                        sumNum.SetText((int.Parse(sumNum.text) + playerHands[i]).ToString());
                        turnText.SetText("");

                        Instantiate(cardObjcts[playerHands[i]], 
                            new Vector3(Random.Range(-0.3f, 0.4f), Random.Range(-0.3f, 0.4f), firldCardPos_z--), Quaternion.Euler(0, 0, Random.Range(-20, 20)));

                        // Handスクリプトの"メソッドの"RemoveAtを呼び出す
                        hands[0].RemoveAt(i);

                        // こっちは"Listの"RemoveAtなのでplayerHandsのリスト内数字を削除している
                        playerHands.RemoveAt(i);

                        Destroy(hit.transform.gameObject);

                        Invoke("ChildRename", 0.01f);

                        if (SumNumCheck())
                        {
                            audioSource.PlayOneShot(lose);
                            turnText.SetText("あなたの負け！");
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

    // COM1の動き
    private IEnumerator EnemyMove_1()
    {
        yield return wait;

        // COM1のレベル：弱い(小さい数字から出していく)
        // COM1のハンドの中から一番小さい数字をminNumに代入
        int[] values_1 = new int[enemyHands_1.Count];
        for (int i = 0; i < enemyHands_1.Count; i++)
        {
            values_1[i] = enemyHands_1[i];
        }
        int minNum_Com1 = Mathf.Min(values_1);

        // COM1のハンドの中からminNumと同じ数字を探して場に出す
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
            turnText.SetText("COM1の負け！");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(EnemyMove_2());
        }
    }

    // COM2の動き
    private IEnumerator EnemyMove_2()
    {
        yield return wait;

        // COM2の強さ：普通(手札の数字を好きなもの(ランダム)から出す)
        int randomNum = Random.Range(0, enemyHands_2.Count);//0～COM2の現在の手札の要素数から1つ数字を選ぶ
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
            turnText.SetText("COM2の負け！");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(EnemyMove_3());
        }
    }

    // COM3の動き
    private IEnumerator EnemyMove_3()
    {
        yield return wait;

        // COM3の強さ：強い(大きい数字から出す、大きい数字が10を超えそうだったら小さい数字を出す)
        // COM3のハンドの中から一番大きい数字をmaxNumに代入
        int[] values_3 = new int[enemyHands_3.Count];
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            values_3[i] = enemyHands_3[i];
        }
        int maxNum_Com3 = Mathf.Max(values_3);

        // COM3のハンドの中から一番小さい数字をminNumに代入
        int[] values_3_2 = new int[enemyHands_3.Count];
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            values_3_2[i] = enemyHands_3[i];
        }
        int minNum_Com3 = Mathf.Min(values_3_2);

        // COM3のハンドの中からmaxNumと同じ数字を探して場に出す
        for (int i = 0; i < enemyHands_3.Count; i++)
        {
            if (enemyHands_3[i] == maxNum_Com3)
            {
                // 手札で一番大きい数字を出した時、10を超えなければそのまま出す
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

                // 手札で一番大きい数字を出した時、10を超えるなら小さい数字を探して出す
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
            turnText.SetText("COM3の負け！");
            Instantiate(victoryEffect, v_Pos);
            yield break;
        }
        else
        {
            StartCoroutine(PlayerMove());
        }
    }

    // 合計値が１０を超えていないかチェック
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
