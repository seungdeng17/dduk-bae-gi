using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

// 버프 스킬 초기화, 버프 스킬 텍스트 갱신, 실제 버프 스킬 내용 적용 관련
public class CBuffSkillInfoManager : MonoBehaviour {

    [Header("< 버프 스킬 레벨 >")]
    public ObscuredInt[] _buffSkillLevelArray;

    [Header("< 버프 스킬 업그레이드 필요 루비 >")]
    public ObscuredInt[] _buffSkillUpNeedRubyArray;

    [Header("< 버프 스킬 업그레이드 루비 증가량 >")]
    public ObscuredInt[] _buffSkillUpRaitoRubyArray;

    [Header("< 버프 스킬 능력치 >")]
    public ObscuredFloat[] _buffSkillFunction;

    [Header("< 버프 스킬 레벨업시 능력치 증가량 >")]
    public ObscuredFloat[] _buffSkillIncrementRatio;

    [Header("< 버프 스킬 지속 시간 >")] // 레벨업시 적용되는 버프에만 해당
    public ObscuredFloat[] _buffSkillTime;

    [Header("< 버프 스킬 지속 시간 레벨업시 증가량 >")] // 레벨업시 적용되는 버프에만 해당
    public ObscuredFloat[] _buffSkillIncrementTime;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CBuffSkillManager _buffSkillManager;

    // 버프로 오른 스탯
    [HideInInspector]
    public ObscuredFloat _upValueCON;
    [HideInInspector]
    public ObscuredFloat _upValueDEF;
    [HideInInspector]
    public ObscuredFloat _upValueATS;
    [HideInInspector]
    public ObscuredFloat _upValueCHC;
    [HideInInspector]
    public ObscuredFloat _upValueCHD;
    [HideInInspector]
    public ObscuredFloat _upValueEXP;
    [HideInInspector]
    public ObscuredFloat _upValueCOIN;


    // 버프 스킬 레벨에 해당하는 가격, 기능 정보 갱신
    public void BuffSkillLevelFuntionRefresh()
    {
        BuffSkillLevelApplyRuby();
        BuffSkillLevelApplyFunction();
    }

    // 레벨에 해당하는 필요 루비 갯수로 갱신
    public void BuffSkillLevelApplyRuby()
    {
        for (int i = 0; i < _buffSkillLevelArray.Length; i++)
        {
            _buffSkillUpNeedRubyArray[i] += (_buffSkillLevelArray[i] * _buffSkillUpRaitoRubyArray[i]);
            _buffSkillManager._buffSkillUpNeedRubyText[i].text = _playerInfo._playerState.CommaText(_buffSkillUpNeedRubyArray[i]).ToString();
        } 
    }

    // 레벨에 해당하는 기능과 이름, 레벨 텍스트 갱신
    public void BuffSkillLevelApplyFunction()
    {
        for (int i = 0; i < _buffSkillLevelArray.Length; i++)
        {
            _buffSkillFunction[i] += (_buffSkillLevelArray[i] * _buffSkillIncrementRatio[i]);

            if (_buffSkillTime[i] != 0f)
            {
                _buffSkillTime[i] += (_buffSkillLevelArray[i] * _buffSkillIncrementTime[i]);
            }

            BuffSkillNameLevelTextRefresh(i);
        }
    }


    // 레벨업 버프 스킬
    public void LevelUpBuffOn()
    {
        // 체력 채움
        _playerInfo._playerState._hp = _playerInfo._playerState._originHp;
        _playerInfo._playerState.HpBarRefresh(_playerInfo._playerState._hp);

        // 공격력, 공격속도 업
        BuffSkill00On();
        BuffSkill01On();
    }


    // 버프 스킬 내용
    // 00. 버프 스킬 (레벨업)
    [Header("< 공격력 버프 활성 확인 아이콘>")]
    public Button _buffSkill00CheckIcon; // 버프 활성화를 표시 할 아이콘 (레벨업, 광고 버튼만 해당)
    [HideInInspector]
    public ObscuredBool _isBuffSkill00On = false; // 현재 버프 중?
    public void BuffSkill00On()
    {
        if (!_isBuffSkill00On)
        {
            _isBuffSkill00On = true;
            _buffSkill00CheckIcon.interactable = true;

            // 공격력 업
            _playerInfo._playerState._attackDamage *= _buffSkillFunction[0];
            if (_playerInfo._playerState._attackDamage > 2100000000f) _playerInfo._playerState._attackDamage = 2100000000f;
            _playerInfo._playerState._attackDamageRange = _playerInfo._playerState._attackDamage * 0.5f;
        }
        CancelInvoke("BuffSkill00Off");
        Invoke("BuffSkill00Off", _buffSkillTime[0]);
    }
    private void BuffSkill00Off()
    {
        _isBuffSkill00On = false;
        _buffSkill00CheckIcon.interactable = false;
        _playerInfo.AttackDamageRefresh();
    }
    // 버프중 업그레이드 값 들어왔을 때 초기화 하고 다시 버프
    public void BuffSkill00Refresh()
    {
        _playerInfo._playerState._attackDamage *= _buffSkillFunction[0];
        if (_playerInfo._playerState._attackDamage > 2100000000f) _playerInfo._playerState._attackDamage = 2100000000f;
        _playerInfo._playerState._attackDamageRange = _playerInfo._playerState._attackDamage * 0.5f;
    }


    // 01. 버프 스킬 (레벨업)
    [Header("< 공격속도 버프 활성 확인 아이콘 >")]
    public Button _buffSkill01CheckIcon;
    [HideInInspector]
    public ObscuredBool _isBuffSkill01On = false;
    public void BuffSkill01On()
    {
        if (!_isBuffSkill01On)
        {
            _isBuffSkill01On = true;
            _buffSkill01CheckIcon.interactable = true;

            // 공격속도 업
            _playerInfo._playerState._attackSpeed *= _buffSkillFunction[1];
            if (_playerInfo._playerState._attackSpeed > _playerInfo._attackSpeedMaximum) _playerInfo._playerState._attackSpeed = _playerInfo._attackSpeedMaximum; // 공격속도 제한
            _playerInfo._playerState._animator.SetFloat("AttackSpeed", _playerInfo._playerState._attackSpeed);
        }
        CancelInvoke("BuffSkill01Off");
        Invoke("BuffSkill01Off", _buffSkillTime[1]);
    }
    private void BuffSkill01Off()
    {
        _isBuffSkill01On = false;
        _buffSkill01CheckIcon.interactable = false;
        _playerInfo.AttackSpeedRefresh();
    }
    // 버프중 업그레이드 값 들어왔을 때 초기화 하고 다시 버프
    public void BuffSkill01Refresh()
    {
        _playerInfo._playerState._attackSpeed *= _buffSkillFunction[1];
        if (_playerInfo._playerState._attackSpeed > _playerInfo._attackSpeedMaximum) _playerInfo._playerState._attackSpeed = _playerInfo._attackSpeedMaximum; // 공격속도 제한
        _playerInfo._playerState._animator.SetFloat("AttackSpeed", _playerInfo._playerState._attackSpeed);
    }

    // 02. 버프 스킬 (패시브)
    public void BuffSkill02On()
    {
        // 세팅
        _playerInfo._CON = _playerInfo._originCON;
        _upValueCON = (int)(_playerInfo._CON * (_buffSkillFunction[2] * 0.01));
        _playerInfo._CON += (int)(_playerInfo._CON * (_buffSkillFunction[2] * 0.01));
        
        // 적용
        _playerInfo.OriginHpRefresh();
        _playerInfo._statButtonManager.CONTextRefresh();
    }

    // 03. 버프 스킬 (패시브)
    public void BuffSkill03On()
    {
        // 세팅
        _playerInfo._DEF = _playerInfo._originDEF;
        _upValueDEF = (int)(_playerInfo._DEF * (_buffSkillFunction[3] * 0.01));
        _playerInfo._DEF += (int)(_playerInfo._DEF * (_buffSkillFunction[3] * 0.01));

        // 적용
        _playerInfo.DefensiveRefresh();
        _playerInfo._statButtonManager.DEFTextRefresh();
    }

    // 04. 버프 스킬 (패시브)
    public void BuffSkill04On()
    {
        // 세팅
        _playerInfo._CHC = _playerInfo._originCHC;
        _upValueCHC = (int)(_playerInfo._CHC * (_buffSkillFunction[4] * 0.01));
        _playerInfo._CHC += (int)(_playerInfo._CHC * (_buffSkillFunction[4] * 0.01));

        // 적용
        _playerInfo.CriticalPerRefresh();
        _playerInfo._statButtonManager.CHCTextRefresh();
    }

    // 05. 버프 스킬 (패시브)
    public void BuffSkill05On()
    {
        // 세팅
        _playerInfo._CHD = _playerInfo._originCHD;
        _upValueCHD = (int)(_playerInfo._CHD * (_buffSkillFunction[5] * 0.01));
        _playerInfo._CHD += (int)(_playerInfo._CHD * (_buffSkillFunction[5] * 0.01));

        // 적용
        _playerInfo.CriticalDamageRefresh();
        _playerInfo._statButtonManager.CHDTextRefresh();
    }

    // 06. 버프 스킬 (패시브)
    public void BuffSkill06On()
    {
        // 세팅
        _playerInfo._EXP = _playerInfo._originEXP;
        _upValueEXP = (int)(_playerInfo._EXP * (_buffSkillFunction[6] * 0.01));
        _playerInfo._EXP += (int)(_playerInfo._EXP * (_buffSkillFunction[6] * 0.01));

        // 적용
        _playerInfo.AddEXPRefresh();
        _playerInfo._statButtonManager.EXPTextRefresh();
    }

    // 07. 버프 스킬 (패시브)
    public void BuffSkill07On()
    {
        // 세팅
        _playerInfo._COIN = _playerInfo._originCOIN;
        _upValueCOIN = (int)(_playerInfo._COIN * (_buffSkillFunction[7] * 0.01));
        _playerInfo._COIN += (int)(_playerInfo._COIN * (_buffSkillFunction[7] * 0.01));

        // 적용
        _playerInfo.AddCoinRefresh();
        _playerInfo._statButtonManager.COINTextRefresh();
    }

    // 08. 버프 스킬 (패시브)
    public void BuffSkill08On()
    {
        // 08. 버프 스킬은 PlayerAttack 클래스에서 해당 스킬 레벨이 1 이상일 때 적용되는 것으로 세팅돼있음
        // 세팅
        // 적용
    }

    // 09. 버프 스킬 (패시브)
    public void BuffSkill09On()
    {
        // 세팅
        _playerInfo._playerState._attackCount = 1;

        // 적용
        _playerInfo._playerState._attackCount += (int)_buffSkillFunction[9];
    }


    // 버프 스킬 레벨 갱신
    // 00. 버프 스킬
    public void BuffSkill00TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("화가 난다 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[0].ToString());
        _buffSkillManager._buffSkillNameLevelText[0].text = CStringBuilder._sb.ToString();
    }

    // 01. 버프 스킬
    public void BuffSkill01TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("나만 빨라 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[1].ToString());
        _buffSkillManager._buffSkillNameLevelText[1].text = CStringBuilder._sb.ToString();
    }

    // 02. 버프 스킬
    public void BuffSkill02TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("혈액 순환 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[2].ToString());
        _buffSkillManager._buffSkillNameLevelText[2].text = CStringBuilder._sb.ToString();
    }

    // 03. 버프 스킬
    public void BuffSkill03TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("안 아파 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[3].ToString());
        _buffSkillManager._buffSkillNameLevelText[3].text = CStringBuilder._sb.ToString();
    }

    // 04. 버프 스킬
    public void BuffSkill04TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("야! + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[4].ToString());
        _buffSkillManager._buffSkillNameLevelText[4].text = CStringBuilder._sb.ToString();
    }

    // 05. 버프 스킬
    public void BuffSkill05TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("뼈 맞았어 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[5].ToString());
        _buffSkillManager._buffSkillNameLevelText[5].text = CStringBuilder._sb.ToString();
    }

    // 06. 버프 스킬
    public void BuffSkill06TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("위이잉 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[6].ToString());
        _buffSkillManager._buffSkillNameLevelText[6].text = CStringBuilder._sb.ToString();
    }

    // 07. 버프 스킬
    public void BuffSkill07TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("두둑한 주머니 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[7].ToString());
        _buffSkillManager._buffSkillNameLevelText[7].text = CStringBuilder._sb.ToString();
    }

    // 08. 버프 스킬
    public void BuffSkill08TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("모기 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[8].ToString());
        _buffSkillManager._buffSkillNameLevelText[8].text = CStringBuilder._sb.ToString();
    }

    // 09. 버프 스킬
    public void BuffSkill09TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("다중 공격 + ");
        CStringBuilder._sb.Append(_buffSkillLevelArray[9].ToString());
        _buffSkillManager._buffSkillNameLevelText[9].text = CStringBuilder._sb.ToString();
    }


    // 버프 이름, 레벨 텍스트 갱신
    public void BuffSkillNameLevelTextRefresh(int buffSkillNum)
    {
        switch (buffSkillNum)
        {
            case 0: BuffSkill00TextRefresh(); break;
            case 1: BuffSkill01TextRefresh(); break;
            case 2: BuffSkill02TextRefresh(); break;
            case 3: BuffSkill03TextRefresh(); break;
            case 4: BuffSkill04TextRefresh(); break;
            case 5: BuffSkill05TextRefresh(); break;
            case 6: BuffSkill06TextRefresh(); break;
            case 7: BuffSkill07TextRefresh(); break;
            case 8: BuffSkill08TextRefresh(); break;
            case 9: BuffSkill09TextRefresh(); break;
        }
    }

    // 패시브성 버프 스킬
    public void PassiveBuffSkill()
    {
        // BuffSkill00On() 은 레벨업 스킬
        // BuffSkill01On() 은 레벨업 스킬
        BuffSkill02On();
        BuffSkill03On();
        BuffSkill04On();
        BuffSkill05On();
        BuffSkill06On();
        BuffSkill07On();
        BuffSkill08On();
        BuffSkill09On();
    }
}
