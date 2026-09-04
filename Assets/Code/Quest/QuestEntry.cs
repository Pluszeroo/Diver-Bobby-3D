using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestEntry : MonoBehaviour
{
    public QuestType QuestType;

    [SerializeField] GameObject _crossOut;

    public void CrossOut()
    {
        _crossOut.SetActive(true);
    }
}
