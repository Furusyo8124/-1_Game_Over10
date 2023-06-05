using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    List<Transform> cards = new List<Transform>();        // 手札の位置リスト

    [SerializeField] float space = 1.5f;                  // 手札が均等に並ぶ時の速さ


    // プレイヤーと敵で分けた理由は、
    // プレイヤー側にメソッド呼ばれたタイミングでタグ付けしたかったから

    // 配られたカードのTransformを追加する(プレイヤー)
    public void Add(Transform t)
    {
        t.SetParent(transform);
        t.SetPositionAndRotation(transform.position, transform.rotation);
        cards.Add(t);
        t.gameObject.tag = "Player";
    }

    // 配られたカードのTransformを追加する(敵)
    public void AddEnemy(Transform t)
    {
        t.SetParent(transform);
        t.SetPositionAndRotation(transform.position, transform.rotation);
        cards.Add(t);
    }

    // 場に出すカードをリストから消す(プレイヤー)
    public void RemoveAt(int index)
    {
        cards.RemoveAt(index);
    }

    // 場に出すカードをリストから消す(敵)
    public void RemoveAtEnemy(int index)
    {
        cards.RemoveAt(index);
        Destroy(transform.GetChild(index).gameObject);
    }

    // 常に手札のカード同士の距離を一定に保ちつつ、なめらかに動かす
    void Update()
    {
        Vector3 p = Vector3.left * (space * (cards.Count - 1) / 2);

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 pos = p + Vector3.right * (space * i);
            cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, pos, 0.1f);
        }
    }
}
