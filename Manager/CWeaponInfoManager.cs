using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


// 무기 정보 관리, 무기 기능 반영
public class CWeaponInfoManager : MonoBehaviour {

    // 무기 정보
    [Header("< 무기 레벨 >")]
    public ObscuredInt[] _weaponLevelArray; // 무기 레벨

    [Header("< 무기 기본 공격력 >")]
    public ObscuredFloat[] _weaponBasicAttackDamage; // 무기 기본 공격력

    [Header("< 무기 기능 증가량 (공격력) >")]
    public ObscuredFloat[] _weaponFunctionSTRArray; // 무기 기능 증가량 (공격력)

    // 업그레이드 관련
    [Header("< 무기 업그레이드 필요 코인 >")]
    public ObscuredDouble[] _weaponUpNeedCoinArray; // 무기 업그레이드 필요 코인

    [Header("< 무기 업그레이드 코인 증가량 >")]
    public ObscuredFloat[] _weaponUpRatioArray; // 무기 업그레이드 코인 증가량

    [Header("< 무기 업그레이드 확률 >")]
    public ObscuredFloat[] _weaponUpPercent; // 무기 업그레이드 확률

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CWeaponSelectionManager _weaponSelectionManager;
    public CStatButtonManager _statButtonManager; // 스탯 텍스트 갱신을 위해 참조

    [HideInInspector]
    public ObscuredFloat _weaponUpChanceNum;

    // 기능으로 오른 스탯 (무기는 공격력만 올려줌)
    [HideInInspector]
    public ObscuredFloat _upValueSTR;


    // 시작시 무기 레벨에 따른 기능 적용 메서드
    public void OnStartWeaponSetting(ObscuredInt availableWeaponNum)
    {
        for (int i = 0; i <= availableWeaponNum; i++)
        {
            OnStartWeaponRefresh(i, _weaponLevelArray[i]); // 필요 골드, 무기 기능, 강화 확률 갱신
            OnWeaponFunction(false, i); // 단순 Text 갱신을 위해
        }
    }

    public void OnStartWeaponRefresh(ObscuredInt weaponNum, ObscuredInt weaponLevel)
    {
        for (int i = 0; i < weaponLevel; i++)
        {
            _weaponUpNeedCoinArray[weaponNum] =
                _weaponSelectionManager.PlayerCoinDown(false, true, _weaponSelectionManager._weaponNeedCoinText[weaponNum], _weaponUpNeedCoinArray[weaponNum],
                _weaponUpRatioArray[weaponNum]); // 필요 골드 갱신
        }

        _weaponFunctionSTRArray[weaponNum] += _weaponFunctionSTRArray[weaponNum] * weaponLevel; // 무기 기능 갱신
        _weaponUpPercent[weaponNum] -= weaponLevel * 0.25f; // 강화 확률 갱신
        _weaponUpPercent[weaponNum] = Mathf.Clamp(_weaponUpPercent[weaponNum], 20f, 100f); // 최대 20까지 감소
    }


    // 공격력 증가치 적용 메서드
    public void STRUpFunction(ObscuredInt weaponNum, ObscuredFloat functionSTR)
    {
        // 세팅
        _playerInfo._STR = _playerInfo._originSTR;
        //_playerInfo._originSTR = _playerInfo._STR;
        _upValueSTR = (int)(_playerInfo._STR * (functionSTR * 0.01));
        _playerInfo._STR += (int)(_playerInfo._STR * (functionSTR * 0.01));
        _playerInfo._basicAttackDamage = _weaponBasicAttackDamage[weaponNum];

        // 적용
        _playerInfo.AttackDamageRefresh();
        _statButtonManager.STRTextRefresh();
    }

    // (다른 무기로 변경시) 공격력 증가치 초기화 메서드
    public void STRRebackFunction()
    {
        _playerInfo._STR = _playerInfo._originSTR;
        _playerInfo.AttackDamageRefresh();
        //_statButtonManager._attackDamageText.text = "STR + " + _statButtonManager._playerState.CommaText(_playerInfo._STR).ToString();
    }

    // 실제 무기 착용 및 기능 갱신 메서드
    public void WeaponRefresh(ObscuredInt weaponNum, ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        if (isOn)
        {
            _weaponSelectionManager._weaponGameObjects[weaponNum].SetActive(true);
            _weaponSelectionManager._weaponToggles[weaponNum].interactable = false;
            STRUpFunction(weaponNum, functionSTR);
        }
        else
        {
            _weaponSelectionManager._weaponGameObjects[weaponNum].SetActive(false);
            _weaponSelectionManager._weaponToggles[weaponNum].interactable = true;
            STRRebackFunction();
        }
    }


    // 어떤 무기의 기능을 수행할지 
    public void OnWeaponFunction(ObscuredBool isOn, ObscuredInt weaponNum)
    {
        if (isOn) // 무기 토글이 활성화 됐고 
        {
            switch (weaponNum)
            {
                // 0번 무기를 선택했다면, 0번 무기의 기능을 갱신
                case 0: Weapon00Function(true, _weaponFunctionSTRArray[0]); break;
                case 1: Weapon01Function(true, _weaponFunctionSTRArray[1]); break;
                case 2: Weapon02Function(true, _weaponFunctionSTRArray[2]); break;
                case 3: Weapon03Function(true, _weaponFunctionSTRArray[3]); break;
                case 4: Weapon04Function(true, _weaponFunctionSTRArray[4]); break;
                case 5: Weapon05Function(true, _weaponFunctionSTRArray[5]); break;
            }
        }
        else
        {
            switch (weaponNum) // 무기 토글이 비활성화
            {
                case 0: Weapon00Function(false, _weaponFunctionSTRArray[0]); break;
                case 1: Weapon01Function(false, _weaponFunctionSTRArray[1]); break;
                case 2: Weapon02Function(false, _weaponFunctionSTRArray[2]); break;
                case 3: Weapon03Function(false, _weaponFunctionSTRArray[3]); break;
                case 4: Weapon04Function(false, _weaponFunctionSTRArray[4]); break;
                case 5: Weapon05Function(false, _weaponFunctionSTRArray[5]); break;
            }
        }
    }


    // 00. 나무 몽둥이
    public void Weapon00Function(ObscuredBool isOn, ObscuredFloat functionSTR) // 무기 기능 적용 메서드
    {
        WeaponRefresh(0, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("나무 몽둥이 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[0].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[0].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[0].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[0].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[0].text = CStringBuilder._sb.ToString();
    }

    // 01. 식칼
    public void Weapon01Function(ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        WeaponRefresh(1, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("식칼 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[1].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[1].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[1].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[1].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[1].text = CStringBuilder._sb.ToString();
    }

    // 02. 작지만 영롱해
    public void Weapon02Function(ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        WeaponRefresh(2, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("작지만 영롱해 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[2].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[2].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[2].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[2].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[2].text = CStringBuilder._sb.ToString();
    }

    // 03. 만들다 만
    public void Weapon03Function(ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        WeaponRefresh(3, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("만들다 만 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[3].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[3].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[3].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[3].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[3].text = CStringBuilder._sb.ToString();
    }

    // 04. 좀 멋진
    public void Weapon04Function(ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        WeaponRefresh(4, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("좀 멋진 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[4].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[4].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[4].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[4].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[4].text = CStringBuilder._sb.ToString();
    }

    // 05. 멋지고 영롱해
    public void Weapon05Function(ObscuredBool isOn, ObscuredFloat functionSTR)
    {
        WeaponRefresh(5, isOn, functionSTR);

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("멋지고 영롱해 + ");
        CStringBuilder._sb.Append(_weaponLevelArray[5].ToString());
        _weaponSelectionManager._weaponToggleNameTexts[5].text = CStringBuilder._sb.ToString();

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("ATK + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponBasicAttackDamage[5].ToString());
        CStringBuilder._sb.Append("</color>, STR + <color=#FF2828FF>");
        CStringBuilder._sb.Append(_weaponFunctionSTRArray[5].ToString("N1"));
        CStringBuilder._sb.Append("</color>%");
        _weaponSelectionManager._weaponToggleFunctionTexts[5].text = CStringBuilder._sb.ToString();
    }
}
