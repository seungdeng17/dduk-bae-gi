using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이펙트 스폰 위치 관리
public class CEffectPositionManager : MonoBehaviour {

    // 상위 오브젝트
    [Header("< 말풍선 이펙트 상위 오브젝트 >")]
    public Transform _speechBubbleParent;

    [Header("< 레벨 업 상위 오브젝트 >")]
    public Transform _LevelUpParent;

    [Header("< 레벨 업 이펙트 상위 오브젝트 >")]
    public Transform _LevelUpEffectParent;

    [Header("< 레벨 업 경험치바 이펙트 상위 오브젝트 >")]
    public Transform _ExpBarFullParent;

    [Header("< 스탯 업 이펙트 상위 오브젝트 >")]
    public Transform _statUpEffectParent;


    // 위치
    [Header("< 말풍선 >")]
    public Vector2 _speechBubblePosition; // 말풍선 위치

    [Header("< 레벨 업 표시 >")]
    public Vector2 _levelUpMarkPosition; // 레벨업 표시 위치 (캐릭터 머리)

    [Header("< 레벨 업 이펙트 표시 >")]
    public Vector2 _levelUpEffectPosition; // 레벨업 이펙트 위치

    [Header("< 레벨 업 경험치바 이펙트 >")]
    public Vector2 _expBarFullEffectPosition; // 경험치바 100%시 이펙트 위치 (100% 지점)

    [Header("< 스탯 업 이펙트 >")]
    public Vector2 _statUpEffectPosition; // 스텟 업 이펙트 위치

    [Header("< 클리어 이펙트 >")]
    public Vector2 _clearEffectPosition; // 클리어 이펙트 표시 위치

    [Header("< 코인 최종 도착 위치 >")]
    public Vector2 _coinDestinationPosition; // 코인 최종 도착 위치
}
