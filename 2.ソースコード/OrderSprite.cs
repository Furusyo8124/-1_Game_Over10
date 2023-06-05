using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSprite : MonoBehaviour
{
    [SerializeField] Material[] orderMaterials;        // 表示させる数字のマテリアル
    [SerializeField] AudioClip card;                   // カードめくる時の効果音

    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // カードの表側に引数の数字と同じ画像を映す
    public void OrderSetSprite(int num)
    {
        spriteRenderer.material = orderMaterials[num - 1];
    }

    // クリックされたカードが回転する動き
    public IEnumerator OrderCardRotate()
    {
        audioSource.PlayOneShot(card);

        float time = 0.5f;
        float s = Time.time;
        while (true)
        {
            // 100分率の公式
            float n = Mathf.Min(Time.time - s, time) / time;
            this.transform.eulerAngles = new Vector3(0, 180 + 180 * n, 0);
            if (n == 1)
            {
                break;
            }

            yield return null;
        }
    }
}
