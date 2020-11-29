using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ez.Pooly;


// 스탯 분배
public class CStatButtonManager : MonoBehaviour {

    public CPlayerState _playerState;
    public CPlayerInfo _playerInfo;
    public CWeaponInfoManager _weaponInfoManager;
    public CBuffSkillInfoManager _buffSkillInfoManager;

    // 스탯 레벨 텍스트
    [Header("< 스탯 레벨 텍스트 >")]
    public Text _attackDamageText; // 공격력
    public Text _originHpText; // 최대체력
    public Text _depensiveText; // 방어력
    public Text _attackSpeedText; // 공격속도
    public Text _criticalPerText; // 치명타 확률
    public Text _criticalDamageText; // 치명타 데미지
    public Text _addEXPText; // 추가 경험치
    public Text _addCoinText; // 추가 코인


    [Header("< 추가 기타 >")]
    private int _chiledCount;


    // 스탯 텍스트 갱신 메서드
    public void StatTextSetting()
    {
        STRTextRefresh();
        CONTextRefresh();
        DEFTextRefresh();
        ATSTextRefresh();
        CHCTextRefresh();
        CHDTextRefresh();
        EXPTextRefresh();
        COINTextRefresh();
    }


    // 공격력 텍스트 갱신
    public void STRTextRefresh()
    {
        if (_weaponInfoManager._upValueSTR > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("STR + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originSTR));
            CStringBuilder._sb.Append("<size=35><color=red> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_weaponInfoManager._upValueSTR));
            CStringBuilder._sb.Append(")</color></size>");

            _attackDamageText.text = CStringBuilder._sb.ToString();
            //_attackDamageText.text = "STR + " + _playerState.CommaText(_playerInfo._originSTR) + "<size=35><color=red> (+" + _playerState.CommaText(_weaponInfoManager._upValueSTR) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("STR + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._STR));

            _attackDamageText.text = CStringBuilder._sb.ToString();
            //_attackDamageText.text = "STR + " + _playerState.CommaText(_playerInfo._STR).ToString();
        }
    }

    // 최대체력 텍스트 갱신
    public void CONTextRefresh()
    {
        if (_buffSkillInfoManager._upValueCON > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CON + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originCON));
            CStringBuilder._sb.Append("<size=35><color=#6DF842FF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueCON));
            CStringBuilder._sb.Append(")</color></size>");

            _originHpText.text = CStringBuilder._sb.ToString();
            //_originHpText.text = "CON + " + _playerState.CommaText(_playerInfo._originCON) + "<size=35><color=#6DF842FF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueCON) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CON + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._CON));

            _originHpText.text = CStringBuilder._sb.ToString();
            //_originHpText.text = "CON + " + _playerState.CommaText(_playerInfo._CON).ToString();
        }
    }

    public void DEFTextRefresh()
    {
        // 방어력 텍스트 갱신
        if (_buffSkillInfoManager._upValueDEF > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("DEF + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originDEF));
            CStringBuilder._sb.Append("<size=35><color=#3D70FFFF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueDEF));
            CStringBuilder._sb.Append(")</color></size>");

            _depensiveText.text = CStringBuilder._sb.ToString();
            //_depensiveText.text = "DEF + " + _playerState.CommaText(_playerInfo._originDEF) + "<size=35><color=#3D70FFFF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueDEF) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("DEF + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._DEF));

            _depensiveText.text = CStringBuilder._sb.ToString();
            //_depensiveText.text = "DEF + " + _playerState.CommaText(_playerInfo._DEF).ToString();
        }
    }

    // 공격속도 텍스트 갱신
    public void ATSTextRefresh()
    {
        if (_buffSkillInfoManager._upValueATS > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("ATS + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originATS));
            CStringBuilder._sb.Append("<size=35><color=white> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueATS));
            CStringBuilder._sb.Append(")</color></size>");

            _attackSpeedText.text = CStringBuilder._sb.ToString();
            //_attackSpeedText.text = "ATS + " + _playerState.CommaText(_playerInfo._originATS) + "<size=35><color=white> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueATS) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("ATS + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._ATS));

            _attackSpeedText.text = CStringBuilder._sb.ToString();
            //_attackSpeedText.text = "ATS + " + _playerState.CommaText(_playerInfo._ATS).ToString();
        }
    }

    // 치명타 확률 텍스트 갱신
    public void CHCTextRefresh()
    {
        if (_buffSkillInfoManager._upValueCHC > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CHC + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originCHC));
            CStringBuilder._sb.Append("<size=35><color=#9B9894FF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueCHC));
            CStringBuilder._sb.Append(")</color></size>");

            _criticalPerText.text = CStringBuilder._sb.ToString();
            //_criticalPerText.text = "CHC + " + _playerState.CommaText(_playerInfo._originCHC) + "<size=35><color=#9B9894FF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueCHC) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CHC + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._CHC));

            _criticalPerText.text = CStringBuilder._sb.ToString();
            //_criticalPerText.text = "CHC + " + _playerState.CommaText(_playerInfo._CHC).ToString();
        }
    }

    // 치명타 데미지 텍스트 갱신
    public void CHDTextRefresh()
    {
        if (_buffSkillInfoManager._upValueCHD > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CHD + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originCHD));
            CStringBuilder._sb.Append("<size=35><color=#D0D0D0FF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueCHD));
            CStringBuilder._sb.Append(")</color></size>");

            _criticalDamageText.text = CStringBuilder._sb.ToString();
            //_criticalDamageText.text = "CHD + " + _playerState.CommaText(_playerInfo._originCHD) + "<size=35><color=#D0D0D0FF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueCHD) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("CHD + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._CHD));

            _criticalDamageText.text = CStringBuilder._sb.ToString();
            //_criticalDamageText.text = "CHD + " + _playerState.CommaText(_playerInfo._CHD).ToString();
        }
    }


    // 추가 경험치 텍스트 갱신
    public void EXPTextRefresh()
    {
        if (_buffSkillInfoManager._upValueEXP > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("EXP + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originEXP));
            CStringBuilder._sb.Append("<size=35><color=#00FFCAFF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueEXP));
            CStringBuilder._sb.Append(")</color></size>");

            _addEXPText.text = CStringBuilder._sb.ToString();
            //_addEXPText.text = "EXP + " + _playerState.CommaText(_playerInfo._originEXP) + "<size=35><color=#00FFCAFF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueEXP) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("EXP + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._EXP));

            _addEXPText.text = CStringBuilder._sb.ToString();
            //_addEXPText.text = "EXP + " + _playerState.CommaText(_playerInfo._EXP).ToString();
        }
    }


    // 추가 코인 텍스트 갱신
    public void COINTextRefresh()
    {
        if (_buffSkillInfoManager._upValueCOIN > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("COIN + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._originCOIN));
            CStringBuilder._sb.Append("<size=35><color=#FFF93CFF> (+");
            CStringBuilder._sb.Append(_playerState.CommaText(_buffSkillInfoManager._upValueCOIN));
            CStringBuilder._sb.Append(")</color></size>");

            _addCoinText.text = CStringBuilder._sb.ToString();
            //_addCoinText.text = "COIN + " + _playerState.CommaText(_playerInfo._originCOIN) + "<size=35><color=#FFF93CFF> (+" + _playerState.CommaText(_buffSkillInfoManager._upValueCOIN) + ")</color></size>".ToString();
        }
        else
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append("COIN + ");
            CStringBuilder._sb.Append(_playerState.CommaText(_playerInfo._COIN));

            _addCoinText.text = CStringBuilder._sb.ToString();
            //_addCoinText.text = "COIN + " + _playerState.CommaText(_playerInfo._COIN).ToString();
        }
    }


    // 사용 가능 스탯 포인트 갱신 메서드
    public void StatPointRefresh()
    {
        if (_playerInfo._statPoint > 0)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerInfo._statPoint.ToString());
            CStringBuilder._sb.Append("+");

            _playerInfo._statText.text = CStringBuilder._sb.ToString();
            //_playerInfo._statText.text = _playerInfo._statPoint + "+".ToString();
        }
        else
        {
            _playerInfo._statImage.enabled = false;
            _playerInfo._statText.text = "";
        }
    }


    // 스탯업 완료 후
    public void StatUpComplete()
    {
        // 스탯 포인트 차감
        _playerInfo._statPoint -= 1;
        _playerInfo._acStatPoint += 1;

        // 사용 가능 스탯 포인트 갱신
        StatPointRefresh();

        // 스탯 업 이펙트
        _chiledCount = _playerState._effectPositionManager._statUpEffectParent.transform.childCount;

        if (_chiledCount > 2)
        {
            Pooly.Despawn(_playerState._effectPositionManager._statUpEffectParent.transform.GetChild(0).transform);
        }
        Pooly.Spawn("StatUpEffect", _playerState._effectPositionManager._statUpEffectPosition, Quaternion.identity, _playerState._effectPositionManager._statUpEffectParent);

        // 말 풍선
        _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statUp[Random.Range(0, _playerInfo._speechBubbleManager._statUp.Length)]);
    }


    // 공격력 업그레이드 (스탯)
    public void OnAttackDamageUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackDamageUpInvoke();
            InvokeRepeating("OnAttackDamageUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnAttackDamageUpInvoke");
        }
    }
    public void OnAttackDamageUpBtnClick_Cancel()
    {
        CancelInvoke("OnAttackDamageUpInvoke");
    }
    private void OnAttackDamageUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._STR += 1f; // 스탯 레벨 증가
            _playerInfo._originSTR += 1f;

            _weaponInfoManager.OnWeaponFunction(true, _playerInfo._selectWeaponNum); // 무기 기능 갱신
            STRTextRefresh();

            _playerInfo.AttackDamageRefresh(); // 공격력 갱신

            StatUpComplete(); // 스탯업 완료
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 최대체력 업그레이드
    public void OnOriginHpUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnOriginHpUpInvoke();
            InvokeRepeating("OnOriginHpUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnOriginHpUpInvoke");
        }
    }
    public void OnOriginHpUpBtnClick_Cancel()
    {
        CancelInvoke("OnOriginHpUpInvoke");
    }
    private void OnOriginHpUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._CON += 1f;
            _playerInfo._originCON += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[2] > 0) _buffSkillInfoManager.BuffSkill02On(); // 버프 스킬 적용 유무
            else _playerInfo.OriginHpRefresh();
            CONTextRefresh();

            StatUpComplete();
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 방어력 업그레이드
    public void OnDefensiveUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDefensiveUpInvoke();
            InvokeRepeating("OnDefensiveUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnDefensiveUpInvoke");
        }
    }
    public void OnDefensiveUpBtnClick_Cancel()
    {
        CancelInvoke("OnDefensiveUpInvoke");
    }
    private void OnDefensiveUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._DEF += 1f;
            _playerInfo._originDEF += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[3] > 0) _buffSkillInfoManager.BuffSkill03On(); // 버프 스킬 적용 유무
            else _playerInfo.DefensiveRefresh();
            DEFTextRefresh();

            StatUpComplete();
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 공격속도 업그레이드
    public void OnAttackSpeedUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackSpeedUpInvoke();
            InvokeRepeating("OnAttackSpeedUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnAttackSpeedUpInvoke");
        }
    }
    public void OnAttackSpeedUpBtnClick_Cancel()
    {
        CancelInvoke("OnAttackSpeedUpInvoke");
    }
    private void OnAttackSpeedUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._ATS += 1f;
            _playerInfo._originATS += 1f;
            ATSTextRefresh();

            _playerInfo.AttackSpeedRefresh();

            StatUpComplete();
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 치명타 확률 업그레이드
    public void OnCriticalPerUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnCriticalPerUpInvoke();
            InvokeRepeating("OnCriticalPerUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnCriticalPerUpInvoke");
        }
    }
    public void OnCriticalPerUpBtnClick_Cancel()
    {
        CancelInvoke("OnCriticalPerUpInvoke");
    }
    private void OnCriticalPerUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._CHC += 1f;
            _playerInfo._originCHC += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[4] > 0) _buffSkillInfoManager.BuffSkill04On(); // 버프 스킬 적용 유무
            else _playerInfo.CriticalPerRefresh();
            CHCTextRefresh();

            StatUpComplete();
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 치명타 데미지 업그레이드
    public void OnCriticalDamageUpBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnCriticalDamageUpInvoke();
            InvokeRepeating("OnCriticalDamageUpInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnCriticalDamageUpInvoke");
        }
    }
    public void OnCriticalDamageUpBtnClick_Cancel()
    {
        CancelInvoke("OnCriticalDamageUpInvoke");
    }
    private void OnCriticalDamageUpInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._CHD += 1f;
            _playerInfo._originCHD += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[5] > 0) _buffSkillInfoManager.BuffSkill05On(); // 버프 스킬 적용 유무
            else _playerInfo.CriticalDamageRefresh();
            CHDTextRefresh();

            StatUpComplete();
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 추가 경험치 업그레이드 (스탯)
    public void OnAddEXPBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAddEXPBtnClickInvoke();
            InvokeRepeating("OnAddEXPBtnClickInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnAddEXPBtnClickInvoke");
        }
    }
    public void OnAddEXPBtnClick_Cancel()
    {
        CancelInvoke("OnAddEXPBtnClickInvoke");
    }
    private void OnAddEXPBtnClickInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._EXP += 1f; // 스탯 레벨 증가
            _playerInfo._originEXP += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[6] > 0) _buffSkillInfoManager.BuffSkill06On(); // 버프 스킬 적용 유무
            else _playerInfo.AddEXPRefresh(); // 능력치 갱신
            EXPTextRefresh();

            StatUpComplete(); // 스탯업 완료
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }


    // 추가 코인 업그레이드 (스탯)
    public void OnAddCoinBtnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAddCoinBtnClickInvoke();
            InvokeRepeating("OnAddCoinBtnClickInvoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnAddCoinBtnClickInvoke");
        }
    }
    public void OnAddCoinBtnClick_Cancel()
    {
        CancelInvoke("OnAddCoinBtnClickInvoke");
    }
    private void OnAddCoinBtnClickInvoke()
    {
        if (!_playerState._isDie && _playerInfo._statPoint > 0)
        {
            // 레벨업
            _playerInfo._COIN += 1f; // 스탯 레벨 증가
            _playerInfo._originCOIN += 1f;

            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[7] > 0) _buffSkillInfoManager.BuffSkill07On(); // 버프 스킬 적용 유무
            else _playerInfo.AddCoinRefresh(); // 능력치 갱신
            COINTextRefresh();

            StatUpComplete(); // 스탯업 완료
        }
        else if (!_playerState._isDie && _playerInfo._statPoint == 0)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._statNotEnough);
        }
    }
}
