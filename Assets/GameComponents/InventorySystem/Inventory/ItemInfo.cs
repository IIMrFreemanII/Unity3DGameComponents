using System;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText = null;

    public string Text
    {
        get => tmpText.text;
        set
        {
            if (value != null)
            {
                if (value != tmpText.text)
                {
                    tmpText.text = value;
                }
            }
        }
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
