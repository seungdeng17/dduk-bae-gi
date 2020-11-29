using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Ez.Pooly;
using UnityEngine.UI;

// 스테이지 관련 관리 (현재 스테이지 정보, 스테이지에 맞는 벨런싱, 보스킬시 이벤트 등)
public class CHardStageManager : MonoBehaviour {

    [Header("< 현재 스테이지 >")]
    public ObscuredFloat _hardStage = 1; // 현재 스테이지
    public Text _hardStageText;
    [HideInInspector]
    public int _clearHardStageNum = 0;


    // 스테이지 증가시 몬스터, 스탯 변화량
    [Header("< 스테이지 증가시 몬스터 스탯 변화량 >")]
    public ObscuredFloat _hpIncrement; // 몬스터 hp 증가량
    public ObscuredFloat _defensiveIncrement; // 몬스터 방어력 증가량
    public ObscuredFloat _attackDamageIncrement; // 몬스터 공격력 증가량
    public ObscuredFloat _expIncrement; // 몬스터 경험치 증가량


    [Header("< 최초 코인 정보 >")]
    public ObscuredFloat _coinValueMin; // 코인 최소 값
    public ObscuredFloat _coinValueMaxRatio; // 코인 최대 값 (해당 배수 만큼)

    [Header("< 코인 증가량 >")]
    public ObscuredFloat _coinIncremen; // 코인 증가량

    private Transform _clearEffect; // 클리어 표시 이펙트
    //private Text _clearText; // 클리어 표시 내용 텍스트


    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo; // 말풍선
    public CPlayerState _playerState; // 콤마 텍스트, 카메라 애니메이션
    public CEffectPositionManager _effectPositionManager; // 클리어 이펙트 표시 위치
    public CHardBossMonsterManager _hardBossMonsterManager; // 하드 보스 몬스터 리스폰
    public CBackGroundDataManager _backGroundDataManager;


    private void OnEnable()
    {
        HardStageTextRefresh();

        _clearHardStageNum = 0;
    }


    // 몬스터 스탯 밸런스 세팅
    public void HardStageBalanceMonster(CMonsterState monsterState, ObscuredFloat stage)
    {
        monsterState._hp *= (1f + (stage * _hpIncrement));
        monsterState._defensive *= (1f + (stage * _defensiveIncrement));
        monsterState._attackDamage *= (1f + (stage * _attackDamageIncrement));
        monsterState._exp *= (1f + (stage * _expIncrement));

        if (monsterState._hp > 2100000000f) monsterState._hp = 2100000000f;
        if (monsterState._defensive > 2100000000f) monsterState._defensive = 2100000000f;
        if (monsterState._attackDamage > 2100000000f) monsterState._attackDamage = 2100000000f;
        if (monsterState._exp > 2100000000f) monsterState._exp = 2100000000f;
    }

    // 코인 밸런스 세팅
    public void HardStageBalanceMoney(CCoin coin, ObscuredFloat stage)
    {
        coin._coinValueMin *= (1f + (stage * _coinIncremen));

        if (coin._coinValueMin > 2100000000f) coin._coinValueMin = 2100000000f;
    }


    // 스테이지 클리어 (보스킬시 이벤트)
    public void HardStageClear()
    {
        // 클리어 이펙트 표시, 카메라 애니메이션
        _clearEffect = Pooly.Spawn("Clear", _effectPositionManager._clearEffectPosition, Quaternion.identity);
        //_clearText = _clearEffect.GetComponentInChildren<Text>();
        //_clearText.text = _playerState.CommaText(_stage) + " <size=2><color=#FDCE62>단계 클리어 !</color></size>".ToString();
        _playerState._cameraDoAnim.DORestartById("Clear");

        // 말풍선
        _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._stageClear[Random.Range(0, _playerInfo._speechBubbleManager._stageClear.Length)]);

        // 스테이지 벨류 올림
        _hardStage += 1f;

        // 백그라운드 체인지
        _clearHardStageNum += 1;
        if (_clearHardStageNum == 5)
        {
            _backGroundDataManager.BackGroundDataChange(Random.Range(0, _backGroundDataManager._backGround_AnimCtrlArray.Length), false);
            _clearHardStageNum = 0;
        }

        // 플레이어가 죽지 않았다면 다음 보스를 스폰
        _hardBossMonsterManager._hardBossMonsterNum -= 1;
        if (_hardBossMonsterManager._hardBossMonsterNum == (_hardBossMonsterManager._hardBossMonsterMaxNum - 1) && _hardBossMonsterManager.gameObject.activeSelf) // 보스 몬스터 매니저가 비활성화가 아니라면 (플레이어가 살아있다면)
        {
            _hardBossMonsterManager.HardBossMonsterSpawn(); // 보스 스폰
        }
    }

    public void HardStageTextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("HARD STAGE <color=#FFFFFFC8>");
        CStringBuilder._sb.Append(_playerState.CommaText(_hardStage).ToString());
        CStringBuilder._sb.Append("</color>");
        _hardStageText.text = CStringBuilder._sb.ToString();
    }
}
