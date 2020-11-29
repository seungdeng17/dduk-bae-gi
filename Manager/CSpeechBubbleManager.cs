using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using UnityEngine.UI;


// 말풍선 관리
public class CSpeechBubbleManager : MonoBehaviour {

    private Transform _speechBubble;
    private Text _speechBubbleText;
    private int _chiledCount;

    // 게임
    [Header("< 플레이 중 >")]
    public string[] _playing;
    [Header("< 스테이지 클리어 >")]
    public string[] _stageClear;
    [Header("< 사망 >")]
    public string[] _die;

    // 스탯 관련
    [Header("< 스탯 올릴 때 >")]
    public string[] _statUp;
    [Header("< 스탯 부족할 때 >")]
    public string _statNotEnough;

    // 무기 관련
    [Header("< 강화 성공 >")]
    public string[] _weaponUpSuccess;
    [Header("< 강화 실패 >")]
    public string[] _weaponUpFaile;
    [Header("< 미착용시 >")]
    public string _weaponNotWear;
    [Header("< 코인부족 >")]
    public string _weaponNotEnoughCoin;
    [Header("< 무기 조건 미달 >")]
    public string _weaponNotEnoughCondition;

    // 용병 관련
    [Header("< 용병 조건 미달 >")]
    public string _crewNotEnoughCondition;


    [Header("< 참조 스크립트 >")]
    public CEffectPositionManager _effectPositionManager; // 말풍선 위치 참조


    // 말풍선 스폰 메서드
    public void SpawnSpeechBubble(string text)
    {
        _chiledCount = _effectPositionManager._speechBubbleParent.transform.childCount;

        if (_chiledCount > 0)
        {
            Pooly.Despawn(_effectPositionManager._speechBubbleParent.transform.GetChild(0).transform);
        }
        _speechBubble = Pooly.Spawn("SpeechBubble", _effectPositionManager._speechBubblePosition, Quaternion.identity, _effectPositionManager._speechBubbleParent);
        _speechBubbleText = _speechBubble.GetComponentInChildren<Text>();
        _speechBubbleText.text = text;
    }
}
