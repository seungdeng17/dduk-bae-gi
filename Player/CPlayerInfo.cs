using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using EnergyBarToolkit;


// 플레이어 정보 관리
public class CPlayerInfo : MonoBehaviour {

    [HideInInspector]
    public CPlayerState _playerState;

    // 이름, 레벨 정보
    [Header("< 이름, 레벨, 나이 >")]
    public ObscuredString _playerName = "김기사";
    public Text _playerNameText;
    public ObscuredInt _playerLevel = 1; // 레벨
    public Text _playerLevelText; // 레벨 텍스트
    public ObscuredFloat _playerAge = 0f; // 나이


    // 재화 정보
    [Header("< 재화 >")]
    public ObscuredLong _my_Coin; // 보유 코인
    public Text _my_CoinText;
    public ObscuredInt _my_Ruby; // 보유 루비
    public Text _my_RubyText;


    // 경험치 정보
    [Header("< 경험치 >")]
    public ObscuredFloat _nowExp = 0f; // 현재 경험치
    public ObscuredFloat _needExp; // 필요 경험치


    // 스탯 정보
    [Header("< 스탯 >")]
    public ObscuredInt _statPoint = 0; // 스탯 포인트
    public ObscuredInt _acStatPoint; // 누적 스탯 포인트

    // 사용 가능 스탯 정보
    public Image _statImage;
    public Text _statText;
    public Animator _statAnimator;

    // 스텟 레벨
    public ObscuredFloat _STR = 0f;
    public ObscuredFloat _CON = 0f;
    public ObscuredFloat _DEF = 0f;
    public ObscuredFloat _ATS = 0f;
    public ObscuredFloat _CHC = 0f;
    public ObscuredFloat _CHD = 0f;
    public ObscuredFloat _EXP = 0f;
    public ObscuredFloat _COIN = 0f;

    [Header("< 순수 스탯 >")]
    public ObscuredFloat _originSTR; // 공격력 스탯
    public ObscuredFloat _originCON; // 최대체력 스탯
    public ObscuredFloat _originDEF; // 방어력 스탯
    public ObscuredFloat _originATS; // 공격속도 스탯
    public ObscuredFloat _originCHC; // 치명타 확률 스탯
    public ObscuredFloat _originCHD; // 치명타 데미지 스탯
    public ObscuredFloat _originEXP; // 추가 경험치 스탯
    public ObscuredFloat _originCOIN; // 추가 코인 스탯


    [Header("< 기본 능력치 >")]
    public ObscuredFloat _basicAttackDamage;
    public ObscuredFloat _basicHp;
    public ObscuredFloat _basicDefensive;
    public ObscuredFloat _basicAttackSpeed;
    public ObscuredFloat _basicCriticalPer;
    public ObscuredFloat _basicCriticalDamage;
    public ObscuredFloat _basicAddExp;
    public ObscuredFloat _basicAddCoin;


    [Header("< 무기 >")]
    // 무기, 무기 이펙트
    public Transform _weaponPoint;
    public Transform _weaponTail;

    // 무기 정보관련 클래스
    public CWeaponSelectionManager _weaponSelectionManager;
    public CWeaponInfoManager _weaponInfoManager;

    public ObscuredInt _selectWeaponNum; // 착용중인 무기 번호
    public ObscuredInt _availableWeaponNum; // 착용 가능한 무기 최대 번호


    [Header("< 버프 스킬 >")]
    // 버프 스킬 관련 클래스
    public CBuffSkillManager _buffSkillManager;
    public CBuffSkillInfoManager _buffSkillInfoManager;

    [Header("< 용병 >")]
    public CCrewManager _crewManager;
    public CCrewInfoManager _crewInfoManager;


    [Header("< 참조 스크립트 >")]
    public CSpeechBubbleManager _speechBubbleManager; // 말풍선
    public CStatButtonManager _statButtonManager;
    public CPlayerInfoManager _playerInfoManager;
    public CBackGroundDataManager _backGroundDataManager;

    [Header("< 능력치 최대값 >")]
    public ObscuredFloat _attackSpeedMaximum; // 공격속도 최대값
    public ObscuredFloat _criticalPerMaximum; // 치명타 확률 최대값
    public ObscuredFloat _criticalDamageMaximum; // 치명타 데미지 최대값
    public ObscuredFloat _addExpMaximum; // 추가 경험치 최대값
    public ObscuredFloat _addCoinMaximum; // 추가 코인 최대값


    private void Awake()
    {
        _playerState = GetComponent<CPlayerState>();
    }


    private void Start()
    {
        // 이름 표시
        _playerNameText.text = _playerName;

        // 레벨 표시
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("Lv. ");
        CStringBuilder._sb.Append(_playerState.CommaText(_playerLevel).ToString());
        _playerLevelText.text = CStringBuilder._sb.ToString();

        // 재화 표시
        _my_CoinText.text = _playerState.CommaText(_my_Coin).ToString();
        _my_RubyText.text = _playerState.CommaText(_my_Ruby).ToString();

        // 사용 가능 스탯이 있다면 표시
        if (_statPoint > 0)
        {
            _statImage.enabled = true;
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_statPoint.ToString());
            CStringBuilder._sb.Append("+");
            _statText.text = CStringBuilder._sb.ToString();
        }
    }


    // 원래 스텟 저장 메서드
    public void OriginStatSetting()
    {
        _STR = _originSTR + _weaponInfoManager._upValueSTR;
        _CON = _originCON + _buffSkillInfoManager._upValueCON;
        _DEF = _originDEF + _buffSkillInfoManager._upValueDEF;
        _ATS = _originATS + _buffSkillInfoManager._upValueATS;
        _CHC = _originCHC + _buffSkillInfoManager._upValueCHC;
        _CHD = _originCHD + _buffSkillInfoManager._upValueCHD;
        _EXP = _originEXP + _buffSkillInfoManager._upValueEXP;
        _COIN = _originCOIN + _buffSkillInfoManager._upValueCOIN;
    }


    // 스탯 정보 갱신 메서드
    // 공격력
    public void AttackDamageRefresh()
    {
        _playerState._attackDamage = _basicAttackDamage + (_STR * _playerInfoManager._attackDamageIncrement); // (기본값) + (증가량)
        if (_playerState._attackDamage > 2100000000f) _playerState._attackDamage = 2100000000f;
        _playerState._attackDamageRange = _playerState._attackDamage * 0.5f;

        if (_buffSkillInfoManager._isBuffSkill00On) _buffSkillInfoManager.BuffSkill00Refresh();
    }

    // 최대체력
    public void OriginHpRefresh()
    {
        _playerState._originHp = _basicHp + (_CON * _playerInfoManager._originHpIncrement);
        if (_playerState._originHp > 2100000000f) _playerState._originHp = 2100000000f;
        _playerState._hp += _playerInfoManager._originHpIncrement;
        if (_playerState._hp > _playerState._originHp) _playerState._hp = _playerState._originHp;
        _playerState.HpRefresh(_playerState._originHp);
    }

    // 방어력
    public void DefensiveRefresh()
    {
        _playerState._defensive = _basicDefensive + (_DEF * _playerInfoManager._defensiveIncrement);
        if (_playerState._defensive > 1000000000f) _playerState._defensive = 1000000000f;
    }

    // 공격속도 갱신
    public void AttackSpeedRefresh()
    {
        _playerState._attackSpeed = _basicAttackSpeed + (_ATS * _playerInfoManager._attackSpeedIncrement);
        if (_playerState._attackSpeed > _attackSpeedMaximum) _playerState._attackSpeed = _attackSpeedMaximum; // 공격속도 제한
        _playerState._animator.SetFloat("AttackSpeed", _playerState._attackSpeed); // 공격속도 적용

        if (_buffSkillInfoManager._isBuffSkill01On) _buffSkillInfoManager.BuffSkill01Refresh();
    }

    // 치명타 확률
    public void CriticalPerRefresh()
    {
        _playerState._criticalPer = _basicCriticalPer + (_CHC * _playerInfoManager._criticalPerIncrement);
        if (_playerState._criticalPer > _criticalPerMaximum) _playerState._criticalPer = _criticalPerMaximum; // 치명타 확률 제한
    }

    // 치명타 데미지
    public void CriticalDamageRefresh()
    {
        _playerState._criticalDamage = _basicCriticalDamage + (_CHD * _playerInfoManager._criticalDamageIncrement);
        if (_playerState._criticalDamage > _criticalDamageMaximum) _playerState._criticalDamage = _criticalDamageMaximum; // 치명타 데미지 제한 (배수)
    }

    // 추가 경험치
    public void AddEXPRefresh()
    {
        _playerState._addEXP = _basicAddExp + (_EXP * _playerInfoManager._addEXPIncrement);
        if (_playerState._addEXP > _addExpMaximum) _playerState._addEXP = _addExpMaximum; // 추가 경험치 제한 (배수)
    }

    // 추가 코인
    public void AddCoinRefresh()
    {
        _playerState._addCoin = _basicAddCoin + (_COIN * _playerInfoManager._addCoinIncrement);
        if (_playerState._addCoin > _addCoinMaximum) _playerState._addCoin = _addCoinMaximum; // 추가 코인 제한 (배수)
    }
}
