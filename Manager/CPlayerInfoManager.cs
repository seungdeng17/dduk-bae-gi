using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class CPlayerInfoManager : MonoBehaviour {

    [Header("< 스탯 증가시 증가 할 플레이어 능력치 비율 >")]
    public ObscuredFloat _attackDamageIncrement; // 공격력
    public ObscuredFloat _originHpIncrement; // 최대체력
    public ObscuredFloat _defensiveIncrement; // 방어력
    public ObscuredFloat _attackSpeedIncrement; // 공격속도
    public ObscuredFloat _criticalPerIncrement; // 치명타 확률
    public ObscuredFloat _criticalDamageIncrement; // 치명타 데미지
    public ObscuredFloat _addEXPIncrement; // 추가 경험치
    public ObscuredFloat _addCoinIncrement; // 추가 코인


    [Header("< 레벨업시 경험치 증가 비율 >")]
    public ObscuredFloat _needEXPIncrement;

    [Header("< 레벨업시 줄 스탯 갯수 >")]
    public ObscuredInt _statPointNum;
}
