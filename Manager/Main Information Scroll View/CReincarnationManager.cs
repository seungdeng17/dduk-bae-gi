using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class CReincarnationManager : MonoBehaviour {

    [Header("< 환생 버튼, 조건 텍스트 >")]
    public Button _reincarnationButton;
    public GameObject _reincanationConditionText;

    [Header("< 획득 루비와 나이, 표시 텍스트 >")]
    public ObscuredInt _gainRuby;
    public Text _gainRubyText;
    public ObscuredInt _gainAge;
    public Text _gainAgeText;

    [Header("< 획득 루비 레벨, 스테이지 보너스 >")]
    public ObscuredInt _gainRuby_Level;
    public ObscuredInt _gainRuby_Stage;

    [Header("< 환생 확인 팝업창 관련 >")]
    public GameObject _touchCutter;
    public GameObject _reincarnationpopup;

    //[Header("< 환생 완료 팝업창 관련 >")]
    //public GameObject _reincarnationCompletepopup;

    [Header("< 환생 중 필요한 오브젝트 >")]
    public GameObject _noneButtonTouchCutter;
    public GameObject _reincarnationFadeInOut;
    public Animator _bossMonsterHpBarAnimator;
    public Text _bossMonsterHpText;
    public CBackGroundDataManager _backGroundDataManager;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CPlayerState _playerState;
    public CPlayerAttack _playerAttack;
    public CStageManager _stageManager;
    public CStatButtonManager _statButtonManager;

    // 몬스터 디스폰 델리게이트
    public delegate void MonsterDespawnHandler();
    public static event MonsterDespawnHandler OnMonsterDespawn;


    private void OnEnable()
    {
        // 획득 루비와 증가할 나이를 세팅하고 Text 갱신
        if (_playerInfo._playerLevel >= 100 && _stageManager._stage >= 100)
        {
            GainRubyAgeSetting();
            _reincarnationButton.interactable = true;
            _reincanationConditionText.SetActive(false);
        }
        else
        {
            _gainRubyText.text = "";
            _gainAgeText.text = "";
            _reincarnationButton.interactable = false;
            _reincanationConditionText.SetActive(true);
        }
    }


    // 획득 루비와 증가할 나이를 세팅하고 Text 갱신
    private void GainRubyAgeSetting()
    {
        // 루비 획득
        if (_playerInfo._playerLevel <= 150) GainRubyLevel(2.5f);
        else if (_playerInfo._playerLevel <= 300) GainRubyLevel(2f);
        else if (_playerInfo._playerLevel <= 450) GainRubyLevel(1.5f);
        else GainRubyLevel(1.25f);

        if (_stageManager._stage <= 200f) GainRubyStage(4f);
        else if (_stageManager._stage <= 400f) GainRubyStage(3.5f);
        else if (_stageManager._stage <= 600f) GainRubyStage(3f);
        else if (_stageManager._stage <= 800f) GainRubyStage(2.5f);
        else GainRubyStage(2f);

        _gainRuby = _gainRuby_Level + _gainRuby_Stage;

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 획득 루비 : <color=#F0046D>");
        CStringBuilder._sb.Append(_playerState.CommaText(_gainRuby).ToString());
        CStringBuilder._sb.Append("</color>");
        _gainRubyText.text = CStringBuilder._sb.ToString();

        // 나이 증가
        if (_playerInfo._playerLevel >= 100) _gainAge = _playerInfo._playerLevel / 100;
        else _gainAge = 0;

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 나이 증가 : <color=#F0046D>");
        CStringBuilder._sb.Append(_playerState.CommaText(_gainAge).ToString());
        CStringBuilder._sb.Append("</color>");
        _gainAgeText.text = CStringBuilder._sb.ToString();
    }

    // 레벨이 높을수록 보너스 루비가 많아지는 메서드
    private void GainRubyLevel(ObscuredFloat bonusRatio)
    {
        _gainRuby_Level = (int)(_playerInfo._playerLevel / bonusRatio);
    }

    // 스테이지가 높을수록 보너스 루비가 많아지는 메서드
    private void GainRubyStage(ObscuredFloat bonusRatio)
    {
        _gainRuby_Stage = (int)(_stageManager._stage / bonusRatio);
    }


    // 환생 팝업창 활성
    public void OnReincarnationBtnClick()
    {
        if (!_playerState._isHardDungeon)
        {
            _touchCutter.SetActive(true);
            _reincarnationpopup.SetActive(true);
        }
        // else문 매운맛 던전에서는 환생 안된다고 말풍선 뜨게하기
    }


    // 환생 팝업창 취소 버튼 클릭
    public void OnReincarnationPopupCancelBtnClick()
    {
        _touchCutter.SetActive(false);
        _reincarnationpopup.SetActive(false);
    }


    // 환생 팝업창 확인 버튼 클릭
    public void OnReincarnationPopupOkBtnClick()
    {
        if (!_playerState._isDie && _playerInfo._playerLevel >= 100 && _stageManager._stage >= 100 && !_playerState._isHardDungeon)
        {
            // 스폰 된 모든 몬스터 디스폰, 몬스터 매니저 비활성
            _playerState._dungeonManager._nomal_BossMonsterManager.gameObject.SetActive(false);
            _playerState._dungeonManager._nomal_MonsterManager.gameObject.SetActive(false);
            if (_playerState._dungeonManager._nomal_BossMonsterManager._bossMonsterNum > 0 || _playerState._dungeonManager._nomal_MonsterManager._monsterNum > 1)
            {
                OnMonsterDespawn();
            }

            // 보스 체력바, 체력 텍스트 초기화
            _bossMonsterHpBarAnimator.Play("BossMonsterHpBarZero");
            _bossMonsterHpText.text = "";

            // 백그라운드, UI 정보 변경
            Invoke("BackGroundUIChange", 0.975f);

            // 환생 팝업창 닫기
            _touchCutter.SetActive(false);
            _reincarnationpopup.SetActive(false);

            // 환생 시 레벨, 필요 경험치, 나이, 스탯, 스테이지, 루비 변화
            Reincarnation();

            // 환생 효과
            StartCoroutine("ReincarnationCoroutine");
        }
    }


    // 환생 효과
    private IEnumerator ReincarnationCoroutine()
    {
        _noneButtonTouchCutter.SetActive(true);
        Time.timeScale = 0.6f;
        _reincarnationFadeInOut.SetActive(true);
        _reincarnationFadeInOut.GetComponent<Animator>().speed = 0.6f;
        _playerState._animator.SetTrigger("Idle");
        _playerState._state = CPlayerState.State.IDLE;
        _playerAttack.PlayAndStopBackGroundAnim();

        yield return new WaitForSeconds(1.95f);

        _noneButtonTouchCutter.SetActive(false);
        Time.timeScale = 1f;
        _reincarnationFadeInOut.SetActive(false);
        _reincarnationFadeInOut.GetComponent<Animator>().speed = 1f;

        _playerState._dungeonManager._nomal_BossMonsterManager.gameObject.SetActive(true);
        _playerState._dungeonManager._nomal_MonsterManager.gameObject.SetActive(true);
    }


    // 백그라운드, UI 정보 변경
    private void BackGroundUIChange()
    {
        _stageManager._clearStageNum = 0;
        _backGroundDataManager.BackGroundDataChange(Random.Range(0, _backGroundDataManager._backGround_AnimCtrlArray.Length), true);

        _gainRubyText.text = "";
        _gainAgeText.text = "";
        _reincarnationButton.interactable = false;
        _reincanationConditionText.SetActive(true);

        _playerInfo._playerLevelText.text = "Lv. " + _playerInfo._playerLevel.ToString();
        _playerInfo._my_RubyText.text = _playerState.CommaText(_playerInfo._my_Ruby).ToString();

        _playerState._expBar.Value = 0f;
    }


    // 환생 시 레벨, 필요 경험치, 나이, 스탯, 스테이지, 루비 변화 메서드
    public void Reincarnation()
    {
        _playerInfo._playerLevel = 1; // 레벨 초기화
        _playerInfo._nowExp = 0f; // 현재 경험치 초기화
        _playerInfo._needExp = 2f; // 필요 경험치 초기화
        _stageManager._stage = 1; // 스테이지 초기화
        _stageManager.StageTextRefresh();
        _playerInfo._playerAge += _gainAge; // 나이 증가
        _playerInfo._my_Ruby += _gainRuby; // 루비 증가
        _playerInfo._statPoint = 0; // 잔여 스탯 포인트 제거
        _playerState.AvailableStatPoint(false);

        // 스탯 변화
        ReincarnationStatChange();
    }


    // 스탯 변화 메서드
    public void ReincarnationStatChange()
    {
        // STR
        if (_playerInfo._originSTR >= 2f)
        {
            _playerInfo._originSTR *= 0.5f;
            _statButtonManager._weaponInfoManager.OnWeaponFunction(true, _playerInfo._selectWeaponNum);
            _statButtonManager.STRTextRefresh();
            _playerInfo.AttackDamageRefresh();
        }

        // CON
        if (_playerInfo._originCON >= 2f)
        {
            _playerInfo._originCON *= 0.5f;
            _playerInfo._buffSkillInfoManager.BuffSkill02On();
        }

        // DEF
        if (_playerInfo._originDEF >= 2f)
        {
            _playerInfo._originDEF *= 0.5f;
            _playerInfo._buffSkillInfoManager.BuffSkill03On();
        }

        // ATS
        if (_playerInfo._originATS >= 2f)
        {
            _playerInfo._originATS *= 0.5f;
            _playerInfo._ATS = _playerInfo._originATS;
            _statButtonManager.ATSTextRefresh();
            _playerInfo.AttackSpeedRefresh();
        }

        // CHC
        if (_playerInfo._originCHC >= 2f)
        {
            _playerInfo._originCHC *= 0.5f;
            _playerInfo._buffSkillInfoManager.BuffSkill04On();
        }

        // CHD
        if (_playerInfo._originCHD >= 2f)
        {
            _playerInfo._originCHD *= 0.5f;
            _playerInfo._buffSkillInfoManager.BuffSkill05On();
        }
    }
}
