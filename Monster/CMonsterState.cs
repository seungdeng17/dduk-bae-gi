using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using EnergyBarToolkit;
using CodeStage.AntiCheat.ObscuredTypes;


public class CMonsterState : CCharacterState
{
    protected CMonsterManager _monsterManager; // 몬스터 목록 관리 매니저
    protected CStageManager _stageManager; // 스테이지 관리 매니저
    protected CPlayerState _playerState; // 플레이어 스테이트 참조 (경험치 전달)
    protected CDirectMove _directMove; // 몬스터 움직임 방향, 속도 관련
    //protected CAttack _attack;

    [Header("< 현재 몬스터 고유 능력치 >")]
    public ObscuredFloat _exp; // 사망시 플레이어에게 줄 경험치
    protected ObscuredFloat _originExp; // 원래 경험치 (몬스터 스탯 저장용)
    public ObscuredInt _coinNum = 1; // 생성 할 코인 갯수

    [Header("< 추가 기타 >")]
    public GameObject _monsterUICanvas; // 머리위 hp를 표시해주는 UI캔버스
    public Image2 _hpImage; // hp바 이미지
    public Image2 _burnImage; // hp바 번효과 이미지
    public SpriteRenderer _monsterShadow; // 몬스터 그림자

    protected SpriteRenderer _spriteRenderer; // 소팅 오더, 컬러값 설정
    protected BoxCollider2D _boxCol2d; // 콜라이더

    protected Transform _backGroundTr; // 죽으면 배경의 자식으로
    protected Color _color = new Color(1f, 1f, 1f, 1f); // 죽으면 원래 색으로

    protected ObscuredFloat _originHpStageValue; // 스테이지가 올라가면서 증가된 원래 체력을 저장 할 변수


    protected override void Awake()
    {
        base.Awake();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCol2d = GetComponent<BoxCollider2D>();
        _directMove = GetComponent<CDirectMove>();
        //_attack = GetComponent<CAttack>();

        // 원래 스탯 정보 저장
        OriginSetting();

        // 공격력 범위, 공격 속도 세팅
        _attackDamageRange = _attackDamage * 0.5f;
        _animator.SetFloat("AttackSpeed", _attackSpeed);
    }


    protected override void Start()
    {
        _backGroundTr = GameObject.FindGameObjectWithTag("AnimBackGround").GetComponent<Transform>();
        _playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<CPlayerState>();
    }


    protected virtual void OnSpawned()
    {
        // 지정된 방향과 속도로 이동
        _directMove._rigidbody2d.velocity = _directMove._direction * _directMove._speed;

        if (_monsterManager == null) // 스폰 되면서 변경해야 할 정보는 스폰 할 때 캐싱해야 됨
        {
            _monsterManager = GameObject.FindGameObjectWithTag("MonsterManager").GetComponent<CMonsterManager>();
            _stageManager = GameObject.FindGameObjectWithTag("StageManager").GetComponent<CStageManager>();
        }

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

        // 소팅오더 세팅
        SortingOrderSetting();

        // 다음 몬스터 스폰
        _monsterManager._monsterNum += 1;
        if (_monsterManager._monsterNum <= _monsterManager._monsterMaxNum && _monsterManager.gameObject.activeSelf)
        {
            _monsterManager.MonsterSpawn();
        }
    }


    // 원래 스텟 저장
    protected override void OriginSetting()
    {
        base.OriginSetting();
        _originExp = _exp;
    }


    // 소팅 오더 세팅
    protected virtual void SortingOrderSetting()
    {
        if (transform.position.y == -0.357f)
        {
            _spriteRenderer.sortingOrder = -5;
        }
        else if (transform.position.y == -0.557f)
        {
            _spriteRenderer.sortingOrder = 0;
        }
        else
        {
            _spriteRenderer.sortingOrder = 5;
        }
    }


    // 체력 감소 처리, 보스 몬스터와 내용 다른지 주의
    public override ObscuredFloat HpDown(ObscuredFloat damage)
    {
        _hp -= damage;

        HpBarRefresh(_hp);

        // 확장 : 사망시 변경점 추가
        if (_hpBar.ValueF <= 0f)
        {
            // 몬스터 사망 즉시 발생 이벤트
            MonsterDieEvent();

            // 확장 : 스폰수 감소, 다음 몬스터 스폰
            _monsterManager._monsterNum -= 1;
            if (_monsterManager._monsterNum == _monsterManager._monsterMaxNum && _monsterManager.gameObject.activeSelf)
            {
                _monsterManager.MonsterSpawn();
            }
        }
        return _hp;
    }


    // 몬스터 사망 즉시 발생 이벤트
    protected virtual void MonsterDieEvent()
    {
        gameObject.transform.parent = _backGroundTr;
        _directMove._rigidbody2d.velocity = Vector2.zero;

        for (int i = 0; i < _coinNum; i++)
        {
            Pooly.Spawn("Coin", transform.position, Quaternion.identity);
        }

        _boxCol2d.enabled = false; // 콜라이더 비활성화
        _monsterShadow.enabled = false; // 몬스터 그림자 비활성화
        _animator.SetTrigger("Die"); // 죽음 애니메이션 실행
        _isDie = true; // 사망 체크
        _playerState.PlayerExpUp(_exp); // 경험치 전달
    }


    // Hp바 다운
    public override void HpBarRefresh(ObscuredFloat _hp)
    {
        // 변경 : _originHp -> _originHpStageValue
        _hpBar.ValueF = _hp / _originHpStageValue;

        // 확장 : Hp바 갱신, 번 효과
        _hpImage.fillValue = _hpBar.ValueF;
        StartCoroutine(BurnCoroutine());
    }


    // 사망 이벤트
    public virtual void DieAnimationEvent()
    {
        _monsterUICanvas.SetActive(false); // 몬스터 UI 캔버스 비활성화

        // 죽으면서 변경 된 정보 초기화 코루틴
        StartCoroutine(DespawnMonsterCoroutine());
    }


    protected WaitForSeconds _monsterDespawnWaitTime = new WaitForSeconds(2f);
    protected virtual IEnumerator DespawnMonsterCoroutine()
    {
        yield return _monsterDespawnWaitTime;

        // 디스폰
        Pooly.Despawn(gameObject.transform);
    }


    // Hp바 번 효과
    private WaitForSeconds _burnWaitTime1 = new WaitForSeconds(0.08f);
    private WaitForSeconds _burnWaitTime2 = new WaitForSeconds(0.015f);
    private IEnumerator BurnCoroutine()
    {
        yield return _burnWaitTime1;

        while (_hpImage.fillValue <= _burnImage.fillValue)
        {
            _burnImage.fillValue -= 0.065f;
            yield return _burnWaitTime2;
        }
        if (_burnImage.fillValue < _hpImage.fillValue) _burnImage.fillValue = _hpImage.fillValue;
    }


    // 디스폰시 정보 초기화
    protected virtual void OnDespawned()
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

        _boxCol2d.enabled = enabled; // 박스콜라이더 초기화
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
    protected virtual void OnMonsterDespawn()
    {
        // 디스폰
        Pooly.Despawn(gameObject.transform);

        _monsterManager._monsterNum = 0;
    }


    // 처음 프레임 드랍 상황 대비 (플레이어 뚫고 지나감)
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 디스폰
            Pooly.Despawn(gameObject.transform);

            _monsterManager._monsterNum -= 1;
            if (_monsterManager._monsterNum == _monsterManager._monsterMaxNum && _monsterManager.gameObject.activeSelf)
            {
                _monsterManager.MonsterSpawn();
            }
        }
    }

    
    // 버그 몬스터 체크
    //protected ObscuredFloat _bugCheckHp;
    //protected virtual IEnumerator BugMonsterCheck()
    //{
    //    yield return new WaitForSeconds(15f);
    //    _bugCheckHp = _hp;
    //    yield return new WaitForSeconds(5f);
    //    if (_playerState._state != CPlayerState.State.DIE && _hp == _bugCheckHp || _hp <= 0)
    //    {
    //        // 디스폰
    //        Pooly.Despawn(gameObject.transform);

    //        _monsterManager._monsterNum -= 1;
    //        if (_monsterManager._monsterNum == _monsterManager._monsterMaxNum && _monsterManager.gameObject.activeSelf)
    //        {
    //            _monsterManager.MonsterSpawn();
    //        }
    //    }
    //}
}
