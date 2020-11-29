using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Ez.Pooly;
using DG.Tweening;
using UnityEngine.UI;


public class CPlayerState : CCharacterState
{
    [Header("< 현재 플레이어 고유 능력치 >")]
    public ObscuredFloat _criticalPer; // 치명타 확률
    public ObscuredFloat _criticalDamage; // 치명타 데미지 증가량
    public ObscuredInt _attackCount = 1; // 복수 공격
    public ObscuredFloat _addEXP; // 추가 경험치
    public ObscuredFloat _addCoin; // 추가 코인
    public ObscuredBool _isHardDungeon = false; // 매운맛 던전 플레이 중인가

    [Header("< 플레이어 사망, 재시작 시 필요한 오브젝트 >")]
    public GameObject _noneButtonTouchCutter;
    public CTouchCutter _touchCutter;
    public GameObject _playerDieFadeInOut;
    public Animator _bossMonsterHpBarAnimator;
    public Text _bossMonsterHpText;
    public Animator _playerHpBarAnimator;

    [Header("< 참조 스크립트 >")]
    public CCharacterStateManager _characterStateManager; // 캐릭터 정보 매니저
    public CEffectPositionManager _effectPositionManager; // 이펙트 스폰 위치 매니저
    public CDungeonManager _dungeonManager;
    protected CPlayerInfo _playerInfo; // 플레이어 정보 관리

    [Header("< 추가 기타 >")]
    public ProgressBarPro _expBar; // 경험치바
    public DOTweenAnimation _cameraDoAnim; // 카메라 애니메이션 (클리어, 크리티컬시 카메라 애니메이션 동작)
    private ObscuredFloat _expValue; // 스탯 정보가 계산 된 경험치


    // 용병 애니메이션 델리게이트 이벤트
    public delegate void CrewAnimDieHandler();
    public static event CrewAnimDieHandler OnCrewAnimDie;

    // 몬스터 디스폰 델리게이트
    public delegate void MonsterDespawnHandler_PlayerDie();
    public static event MonsterDespawnHandler_PlayerDie OnMonsterDespawn_PlayerDie;


    protected override void Awake()
    {
        base.Awake();

        _playerInfo = GetComponent<CPlayerInfo>();
    }


    protected override void Start()
    {
        // 원래 스탯 정보 저장
        OriginSetting();

        // 원래 스탯 저장
        _playerInfo.OriginStatSetting();

        // 무기 기본 데미지 받아옴
        _playerInfo._basicAttackDamage = _playerInfo._weaponInfoManager._weaponBasicAttackDamage[_playerInfo._selectWeaponNum];

        // 공격력 범위, 공격 속도 세팅
        _attackDamageRange = _attackDamage * 0.5f;
        _animator.SetFloat("AttackSpeed", _attackSpeed);

        PlayerInfoSetting(); // 현재 스탯에 대한 능력치 정보 갱신
        PlayerWeaponFunctionSetting(); // 플레이어 무기 기능 정보 반영
        PlayerAvailableWeaponSetting(); // 플레이어 사용 가능 무기 정보 반영
        _playerInfo._buffSkillInfoManager.BuffSkillLevelFuntionRefresh(); // 버프 스킬 레벨에 해당하는 가격, 기능 정보 갱신
        _playerInfo._buffSkillInfoManager.PassiveBuffSkill(); // 패시브성 버프 스킬 적용
        _playerInfo._statButtonManager.StatTextSetting(); // 스탯 텍스트 갱신
        PlayerHpSetting(); // 플레이어 현재 Hp정보 갱신
        _playerInfo._crewInfoManager.CrewLevelFunctionRefresh(); // 용병 레벨에 해당하는 가격, 기능 정보 갱신 후 표시

        // 체력바 세팅
        HpBarSetting();

        // 경험치바 세팅
        _expBar.Value = _playerInfo._nowExp / _playerInfo._needExp; 

        InvokeRepeating("PlayerSpeech", 6f, 12f); // 말풍선
    }


    // 현재 Hp값 세팅
    protected void PlayerHpSetting()
    {
        // 추후 플레이어 프리팹스에 원래 체력값과 현재 체력값을 저장할 때는 각각 설정해주기
        _hp = _originHp;
    }


    // 플레이어 체력 세팅 메서드
    protected override void HpBarSetting()
    {
        if (_hp == _originHp)
        {
            // 확장 : 시작시 hp값도 같이 올라가도록
            _hp = _originHp;
            HpBarRefresh(_hp);
        }
        else
        {
            HpBarRefresh(_hp);
        }
    }


    // 플레이어 스탯 세팅 메서드
    protected void PlayerInfoSetting()
    {
        // 갱신 내용
        _playerInfo.AttackDamageRefresh(); // 공격력
        _playerInfo.OriginHpRefresh(); // 최대체력
        _playerInfo.DefensiveRefresh(); // 방어력
        _playerInfo.AttackSpeedRefresh(); // 공격속도
        _playerInfo.CriticalPerRefresh(); // 치명타 확률
        _playerInfo.CriticalDamageRefresh(); // 치명타 데미지
        _playerInfo.AddEXPRefresh(); // 추가 경험치
        _playerInfo.AddCoinRefresh(); // 추가 코인
    }

    // 플레이어 무기 기능 정보 반영
    protected void PlayerWeaponFunctionSetting()
    {
        _playerInfo._weaponInfoManager.OnStartWeaponSetting(_playerInfo._availableWeaponNum); // 시작시 무기 레벨에 따른 기능 적용
        _playerInfo._weaponSelectionManager._weaponToggles[_playerInfo._selectWeaponNum].isOn = true; // 토글의 isOn값이 바뀌면서 무기 기능을 수행하는 함수가 호출 됨
        _playerInfo._weaponSelectionManager._weaponToggles[_playerInfo._selectWeaponNum].interactable = false;
    }

    // 플레이어 사용 가능 무기 정보 반영
    protected void PlayerAvailableWeaponSetting()
    {
        for (int i = 0; i <= _playerInfo._availableWeaponNum; i++)
        {
            // 막고있는 버튼을 지우고 무기 이미지를 활성화
            Destroy(_playerInfo._weaponSelectionManager._weaponConditionCheckButton[i]);
            _playerInfo._weaponSelectionManager._weaponToggleImages[i].color = _playerInfo._weaponSelectionManager._weaponIconColor;
            if (_playerInfo._weaponSelectionManager._weaponConditionText[i + 1] != null)
            {
                _playerInfo._weaponSelectionManager._weaponConditionText[i + 1].text = _playerInfo._weaponSelectionManager._weaponCondition[i + 1];
            }
        }
    }

    // 플레이어 게임중 말풍선
    protected void PlayerSpeech()
    {
        _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._playing[Random.Range(0, _playerInfo._speechBubbleManager._playing.Length)]);
    }


    // 체력 갱신 메서드
    public void HpRefresh(ObscuredFloat originHp)
    {
        _hpBar.valueMax = (int)originHp;
        HpBarRefresh(_hp); // 현재 체력 정보로 갱신
    }


    // 체력 감소 처리
    public override ObscuredFloat HpDown(ObscuredFloat damage)
    {
        // 체력 감소
        _hp -= damage;

        // 체력 바 갱신
        HpBarRefresh(_hp);

        if (_hpBar.ValueF <= 0f)
        {
            PlayerDie();
        }

        return _hp;
    }


    // 체력 흡수 처리
    private Transform hpAbsorptionText;
    private Text hpAb_text;
    private ObscuredFloat _hpAbsorptionValue;
    public void HpAbscorption(ObscuredFloat c_damage)
    {
        // 체력 흡수 (더하기)
        _hpAbsorptionValue = c_damage * (_playerInfo._buffSkillInfoManager._buffSkillFunction[8] * 0.01f);
        _hp += _hpAbsorptionValue;
        if (_hp > _originHp) _hp = _originHp;

        // 체력 바 갱신
        HpBarRefresh(_hp);

        // 흡수량 텍스트 스폰
        hpAbsorptionText = Pooly.Spawn("HpAbsorptionTextUICanvas", transform.position, Quaternion.identity, gameObject.transform);
        hpAb_text = hpAbsorptionText.GetComponentInChildren<Text>();
        hpAb_text.text = "+" + CommaText(_hpAbsorptionValue).ToString();

        hpAbsorptionText.GetComponentInChildren<Animator>().Play("HpAbsorptionText");
    }


    public void PlayerDie()
    {
        _isDie = true;
        _animator.Play("Die");
        _state = State.DIE;

        // 패널티
        if (_dungeonManager._nomal_BossMonsterManager.gameObject.activeSelf)
        {
            _dungeonManager._nomal_StageManager._stage -= 10f;
            if (_dungeonManager._nomal_StageManager._stage <= 1f) _dungeonManager._nomal_StageManager._stage = 1f;

            // 몬스터 스포너 비활성
            _dungeonManager._nomal_BossMonsterManager.gameObject.SetActive(false);
            _dungeonManager._nomal_MonsterManager.gameObject.SetActive(false);
            _dungeonManager._nomal_StageManager.gameObject.SetActive(false);
        }
        else if (_dungeonManager._hard_HardBossMonsterManager.gameObject.activeSelf)
        {
            _isHardDungeon = false;

            _dungeonManager._hardDungeonCheckText.enabled = false;
            _dungeonManager._hard_HardBossMonsterManager.gameObject.SetActive(false);
            _dungeonManager._hard_StageManager.gameObject.SetActive(false);
        }

        // 무기, 무기 이펙트 비활성
        _playerInfo._weaponPoint.gameObject.SetActive(false);
        _playerInfo._weaponTail.gameObject.SetActive(false);

        // 플레이중 말풍선 중지
        CancelInvoke("PlayerSpeech");

        // 말풍선
        _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._die[Random.Range(0, _playerInfo._speechBubbleManager._die.Length)]);

        if (_playerInfo._crewInfoManager._crewPosition[0].childCount > 0 || _playerInfo._crewInfoManager._crewPosition[1].childCount > 0)
        {
            OnCrewAnimDie();
        }
        
        // 체력 채움
        _hp = _originHp;

        StartCoroutine(PlayerDieRestart());
    }

    private IEnumerator PlayerDieRestart()
    {
        _noneButtonTouchCutter.SetActive(true);
        _playerDieFadeInOut.GetComponent<Animator>().speed = 0.2f;
        _playerDieFadeInOut.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        _animator.SetBool("IsTargetOn", false);

        // 스폰 된 모든 몬스터 디스폰
        if (_dungeonManager._nomal_BossMonsterManager._bossMonsterNum > 0 || _dungeonManager._nomal_MonsterManager._monsterNum > 1 || _dungeonManager._hard_HardBossMonsterManager._hardBossMonsterNum > 0)
        {
            OnMonsterDespawn_PlayerDie();
        }

        // 보스 체력바, 체력 텍스트 초기화
        _bossMonsterHpBarAnimator.Play("BossMonsterHpBarZero");
        _bossMonsterHpText.text = "";

        _isDie = false;
        _touchCutter.OnTouchCutterClick();
        _playerInfo._backGroundDataManager.BackGroundDataChange(Random.Range(0, _playerInfo._backGroundDataManager._backGround_AnimCtrlArray.Length), true);

        // 스테이지 텍스트 갱신
        _dungeonManager._nomalDungeonCheckText.enabled = true;
        _dungeonManager._nomal_StageManager.gameObject.SetActive(true);
        _dungeonManager._nomal_StageManager.StageTextRefresh();
        yield return new WaitForSeconds(3.5f);

        _noneButtonTouchCutter.SetActive(false);
        _playerDieFadeInOut.GetComponent<Animator>().speed = 1f;
        _playerDieFadeInOut.SetActive(false);

        // 몬스터 스포너 활성
        _dungeonManager._nomal_BossMonsterManager.gameObject.SetActive(true);
        _dungeonManager._nomal_MonsterManager.gameObject.SetActive(true);

        // 무기, 무기 이펙트 활성
        _playerInfo._weaponPoint.gameObject.SetActive(true);
        _playerInfo._weaponTail.gameObject.SetActive(true);

        // 체력 UI 갱신
        _playerHpBarAnimator.Play("BossMonsterHpBar");
    }


    // 플레이어 경험치업 메서드
    public void PlayerExpUp(ObscuredFloat exp)
    {
        if (_state != State.DIE)
        {
            _expValue = exp * _addEXP;
            if (_expValue > 2000000000f) _expValue = 2000000000f;

            _playerInfo._nowExp += _expValue; // 현재 경험치 업
            _expBar.Value = _playerInfo._nowExp / _playerInfo._needExp; // 경험치바 세팅

            if (_playerInfo._nowExp >= _playerInfo._needExp) // 현재 경험치가 필요 경험치 보다 많다면
            {
                while (_playerInfo._nowExp >= _playerInfo._needExp)
                {
                    _playerInfo._nowExp -= _playerInfo._needExp; // 현재 경험치 세팅
                    _playerInfo._needExp *= _playerInfo._playerInfoManager._needEXPIncrement; // 필요 경험치 증가
                    if (_playerInfo._needExp > 2100000000f) _playerInfo._needExp = 2100000000f;

                    _playerInfo._playerLevel += 1; // 레벨업
                    _characterStateManager._levelTextContent.text = _playerInfo._playerLevel.ToString(); // 캐릭터 정보 매니저 레벨 표시 갱신
                    CStringBuilder.StringBuilderRefresh();
                    CStringBuilder._sb.Append("Lv. ");
                    CStringBuilder._sb.Append(CommaText(_playerInfo._playerLevel).ToString());
                    _playerInfo._playerLevelText.text = CStringBuilder._sb.ToString(); // 플레이어 머리 위 UI 레벨 텍스트 갱신

                    // 스탯 포인트 줌
                    if (_playerInfo._playerLevel <= 200) _playerInfo._statPoint += _playerInfo._playerInfoManager._statPointNum;
                    else if (_playerInfo._playerLevel <= 400) _playerInfo._statPoint += (_playerInfo._playerInfoManager._statPointNum * 2);
                    else if (_playerInfo._playerLevel <= 600) _playerInfo._statPoint += (_playerInfo._playerInfoManager._statPointNum * 3);
                    else _playerInfo._statPoint += (_playerInfo._playerInfoManager._statPointNum * 4);

                    // 사용 가능 스탯 표시
                    AvailableStatPoint(true);

                    // 레벨 업 관련 이펙트
                    //if (_effectPositionManager._LevelUpParent.transform.childCount > 1) Pooly.Despawn(_effectPositionManager._LevelUpParent.transform.GetChild(0).transform);
                    if (_effectPositionManager._LevelUpEffectParent.transform.childCount > 1) Pooly.Despawn(_effectPositionManager._LevelUpEffectParent.transform.GetChild(0).transform);
                    if (_effectPositionManager._ExpBarFullParent.transform.childCount > 1) Pooly.Despawn(_effectPositionManager._ExpBarFullParent.transform.GetChild(0).transform);

                    //Pooly.Spawn("LevelUp", _effectPositionManager._levelUpMarkPosition, Quaternion.identity, _effectPositionManager._LevelUpParent);
                    Pooly.Spawn("LevelUpEffect", _effectPositionManager._levelUpEffectPosition, Quaternion.Euler(-22.5f, 0f, 0f), _effectPositionManager._LevelUpEffectParent);
                    Pooly.Spawn("ExpBarFull", _effectPositionManager._expBarFullEffectPosition, Quaternion.identity, _effectPositionManager._ExpBarFullParent);

                    // 레벨업 버프
                    _playerInfo._buffSkillInfoManager.LevelUpBuffOn();
                }

                _expBar.Value = _playerInfo._nowExp / _playerInfo._needExp; // 경험치바 세팅
            }
        }
    }

    // 사용 가능 스탯 표시 메서드
    public void AvailableStatPoint(bool isEnabled)
    {
        _playerInfo._statImage.enabled = isEnabled;

        if (isEnabled)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerInfo._statPoint.ToString());
            CStringBuilder._sb.Append("+");

            _playerInfo._statText.text = CStringBuilder._sb.ToString();
            _playerInfo._statAnimator.Play("GetStat");
        }
        else _playerInfo._statText.text = "";
    }
}
