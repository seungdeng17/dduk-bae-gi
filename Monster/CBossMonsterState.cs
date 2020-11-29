using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using Ez.Pooly;


public class CBossMonsterState : CMonsterState
{
    protected CBossMonsterManager _bossMonsterManager; // 보스 몬스터 목록 관리 매니저

    // HpBar 애니메이터
    protected Animator _hpBarAnimator;

    // 보스몬스터 Hp 텍스트, 애니메이션
    protected Text _monsterHpText;
    protected DOTweenAnimation _hpTextDoAnim;

    // 보스 몬스터 텍스트, 애니메이션
    //protected Text _monsterNameText;
    //protected DOTweenAnimation _nameTextDoAnim;

    // 보스 몬스터 이름 세팅
    //public string _monsterName;



    protected override void OnSpawned()
    {
        //_boxCol2d.enabled = false; // 체력바 세팅 전 까지 콜라이더 비활성
        _directMove._rigidbody2d.velocity = Vector2.zero; // 체력바 세팅 전 까지 멈춰서 대기

        // 확장 : 필요한 컴포넌트 캐싱
        if (_hpBar == null)  // 스폰 되면서 변경해야 할 정보는 스폰 할 때 캐싱해야 됨
        {
            _hpBar = GameObject.FindGameObjectWithTag("BossMonsterHpBarUI").GetComponent<EnergyBar>();
            _hpBarAnimator = GameObject.FindGameObjectWithTag("BossMonsterHpBarUI").GetComponent<Animator>();

            _monsterHpText = GameObject.FindGameObjectWithTag("BossMonsterHpTextUI").GetComponent<Text>();
            _hpTextDoAnim = GameObject.FindGameObjectWithTag("BossMonsterHpTextUI").GetComponent<DOTweenAnimation>();

            //_monsterNameText = GameObject.FindGameObjectWithTag("BossMonsterNameTextUI").GetComponent<Text>();
            //_nameTextDoAnim = GameObject.FindGameObjectWithTag("BossMonsterNameTextUI").GetComponent<DOTweenAnimation>();

            _bossMonsterManager = GameObject.FindGameObjectWithTag("BossMonsterManager").GetComponent<CBossMonsterManager>();
            _stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<CStageManager>();
        }


        ////////////////////////// 필요한(중복되는) OnSpawned() 내용 //////////////////////////
        CReincarnationManager.OnMonsterDespawn += this.OnMonsterDespawn;
        CPlayerState.OnMonsterDespawn_PlayerDie += this.OnMonsterDespawn;
        CDungeonManager.OnMonsterDespawn_DungeonChange += this.OnMonsterDespawn;

        // 원래 스텟 저장
        OriginSetting();

        // 스테이지에 맞게 스텟 세팅
        _stageManager.StageBalanceMonster(this, _stageManager._stage - 1f);

        _originHpStageValue = _hp;
        _attackDamageRange = _attackDamage * 0.5f;

        // 체력바 세팅
        HpBarSetting();

        // 공격 속도 받아 옴
        _animator.SetFloat("AttackSpeed", _attackSpeed);

        // 버그 몬스터 체크
        //StartCoroutine(BugMonsterCheck());
        ////////////////////////// 필요한(중복되는) OnSpawned() 내용 //////////////////////////


        // 변경 : 소팅오더 세팅 SortingOrderSetting() -> _spriteRenderer.sortingOrder = -1;
        _spriteRenderer.sortingOrder = -1;

        _stageManager.StageTextRefresh();
    }


    // 체력바 세팅
    protected override void HpBarSetting()
    {
        _hpBar.valueMax = (int)_hp;

        // 변경 : 스폰시 체력바 벨류 1f -> 서서히 차올라서 1f
        _hpBarAnimator.Play("BossMonsterHpBar");

        // 체력바 세팅 후 체력, 이름 표시 코루틴
        StartCoroutine("BossMonsterUICoroutine");
    }


    // 체력바 세팅 후 체력, 이름 표시, 콜라이더 활성 코루틴
    private WaitForSeconds _bossHpTextWaitTime = new WaitForSeconds(0.875f);
    private IEnumerator BossMonsterUICoroutine()
    {
        yield return _bossHpTextWaitTime;

        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(CommaText(_hpBar.valueMax).ToString());
        CStringBuilder._sb.Append(" / ");
        CStringBuilder._sb.Append(CommaText(_hpBar.valueMax).ToString());
        _monsterHpText.text = CStringBuilder._sb.ToString(); // 체력 텍스트 표시
        //_monsterHpText.text = CommaText(_hpBar.valueMax).ToString() + " / " + CommaText(_hpBar.valueMax).ToString(); // 체력 텍스트 표시

        _hpTextDoAnim.DORestartById("HpText"); // 체력 텍스트 애니메이션
        //_monsterNameText.text = _monsterName;
        //_nameTextDoAnim.DORestart();

        _directMove._rigidbody2d.velocity = _directMove._direction * _directMove._speed; // 지정된 방향과 속도로 이동
        _boxCol2d.enabled = true; // 콜라이더 활성
    }


    // 체력 감소 처리, 일반 몬스터와 내용 다른지 주의
    public override ObscuredFloat HpDown(ObscuredFloat damage)
    {
        _hp -= damage;

        HpBarRefresh(_hp);

        // 확장 : Hp 텍스트 갱신
        HpTextDisplay(_hp);

        if (_hpBar.ValueF <= 0f)
        {
            // 몬스터 사망 즉시 발생 이벤트
            MonsterDieEvent();

            // 확장 : 클리어 이펙트 표시, 카메라 애니메이션, 스테이지 업, 다음 보스 소환
            _stageManager.StageClear();
        }
        return _hp;
    }


    private void HpTextDisplay(ObscuredFloat hp)
    {
        if (hp <= 0f || hp >= 2100000000f)
        {
            hp = Mathf.Clamp(hp, 0f, 2100000000f);
        }
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(CommaText(hp).ToString());
        CStringBuilder._sb.Append(" / ");
        CStringBuilder._sb.Append(CommaText(_hpBar.valueMax).ToString());
        _monsterHpText.text = CStringBuilder._sb.ToString(); // 체력 텍스트 표시
        //_monsterHpText.text = CommaText(hp).ToString() + " / " + CommaText(_hpBar.valueMax).ToString();
        _hpTextDoAnim.DORestartById("Damage");
    }


    public override void DieAnimationEvent()
    {
        // 확장 : 보스 UI 초기화
        _monsterHpText.text = "";
        //_monsterNameText.text = "";
        base.DieAnimationEvent();
    }

    protected override void OnDespawned()
    {
        CReincarnationManager.OnMonsterDespawn -= this.OnMonsterDespawn;
        CPlayerState.OnMonsterDespawn_PlayerDie -= this.OnMonsterDespawn;
        CDungeonManager.OnMonsterDespawn_DungeonChange -= this.OnMonsterDespawn;

        _isDie = false;

        _hp = _originHp; // 체력 초기화
        _defensive = _originDefensive; // 방어력 초기화
        _attackDamage = _originAttackDamage; // 공격력 초기화
        _attackSpeed = _originAttackSpeed; // 공격 속도 초기화
        _exp = _originExp; // 경험치 초기화

        // 변경 : 콜라이더 초기화 삭제
        _directMove._speed = _directMove._originSpeed; // 스피드 초기화
        _state = CMonsterState.State.MOVE; // 상태 초기화

        // 에너지바 초기화
        _monsterUICanvas.SetActive(true);
        _hpImage.fillValue = 1f;
        _burnImage.fillValue = 1f;

        // 컬러값 초기화
        _spriteRenderer.color = _color;

        _monsterShadow.enabled = true; // 몬스터 그림자 활성화
    }


    // 모든 몬스터 디스폰 델리게이트 이벤트
    protected override void OnMonsterDespawn()
    {
        // 디스폰
        Pooly.Despawn(gameObject.transform);

        _bossMonsterManager._bossMonsterNum = 0;
    }


    // 처음 프레임 드랍 상황 대비 (플레이어 뚫고 지나감)
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 디스폰
            Pooly.Despawn(gameObject.transform);

            // 확장 : 클리어 이펙트 표시, 카메라 애니메이션, 스테이지 업
            _stageManager.StageClear();
        }
    }

    //protected override IEnumerator BugMonsterCheck()
    //{
    //    yield return new WaitForSeconds(15f);
    //    _bugCheckHp = _hp;
    //    yield return new WaitForSeconds(5f);
    //    if (_playerState._state != CPlayerState.State.DIE && _hp == _bugCheckHp || _hp <= 0)
    //    {
    //        // 디스폰
    //        Pooly.Despawn(gameObject.transform);

    //        // 확장 : 클리어 이펙트 표시, 카메라 애니메이션, 스테이지 업
    //        _stageManager.StageClear();
    //    }
    //}
}
