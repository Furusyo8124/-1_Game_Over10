using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    List<Transform> cards = new List<Transform>();        // ��D�̈ʒu���X�g

    [SerializeField] float space = 1.5f;                  // ��D���ϓ��ɕ��Ԏ��̑���


    // �v���C���[�ƓG�ŕ��������R�́A
    // �v���C���[���Ƀ��\�b�h�Ă΂ꂽ�^�C�~���O�Ń^�O�t����������������

    // �z��ꂽ�J�[�h��Transform��ǉ�����(�v���C���[)
    public void Add(Transform t)
    {
        t.SetParent(transform);
        t.SetPositionAndRotation(transform.position, transform.rotation);
        cards.Add(t);
        t.gameObject.tag = "Player";
    }

    // �z��ꂽ�J�[�h��Transform��ǉ�����(�G)
    public void AddEnemy(Transform t)
    {
        t.SetParent(transform);
        t.SetPositionAndRotation(transform.position, transform.rotation);
        cards.Add(t);
    }

    // ��ɏo���J�[�h�����X�g�������(�v���C���[)
    public void RemoveAt(int index)
    {
        cards.RemoveAt(index);
    }

    // ��ɏo���J�[�h�����X�g�������(�G)
    public void RemoveAtEnemy(int index)
    {
        cards.RemoveAt(index);
        Destroy(transform.GetChild(index).gameObject);
    }

    // ��Ɏ�D�̃J�[�h���m�̋��������ɕۂ��A�Ȃ߂炩�ɓ�����
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
