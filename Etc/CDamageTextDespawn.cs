using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
using UnityEngine.UI;

// 데미지 텍스트 디스폰 될 때 설정 할 것
public class CDamageTextDespawn : MonoBehaviour {

    private Text _text;
    private Color _textAlpha = new Color();

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _textAlpha = _text.color;
        _textAlpha.a = 0f;
    }

    public void OnDespawned()
    {
        //TMP_Text _text = GetComponentInChildren<TMP_Text>();
        _text.color = _textAlpha;
    }
}
