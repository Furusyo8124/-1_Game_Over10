using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSprite : MonoBehaviour
{
    [SerializeField] Material[] orderMaterials;        // �\�������鐔���̃}�e���A��
    [SerializeField] AudioClip card;                   // �J�[�h�߂��鎞�̌��ʉ�

    SpriteRenderer spriteRenderer;
    AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // �J�[�h�̕\���Ɉ����̐����Ɠ����摜���f��
    public void OrderSetSprite(int num)
    {
        spriteRenderer.material = orderMaterials[num - 1];
    }

    // �N���b�N���ꂽ�J�[�h����]���铮��
    public IEnumerator OrderCardRotate()
    {
        audioSource.PlayOneShot(card);

        float time = 0.5f;
        float s = Time.time;
        while (true)
        {
            // 100�����̌���
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
