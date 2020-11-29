using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class CPlayerAttack : CAttack {

    protected Animator _backGroundsAnimator;
    protected CPlayerState _playerState;
    protected CPlayerInfo _playerInfo;

    public Animator _zeroTrAnimator;

    private ObscuredFloat minDist = 0;
    private GameObject targetEnemy = null;
    private ObscuredFloat dist;

    protected Collider2D[] colliders = null;
    protected ObscuredFloat[] _damageRanges = null;
    protected ObscuredFloat[] _damages = null;
    protected ObscuredFloat _isCritical;

    protected List<GameObject> targetList = new List<GameObject>();

    protected ObscuredInt _monsterCount;

    protected Vector2 endPos;
    protected RaycastHit2D hitInfo;

    public SpriteRenderer _weaponTailRender; // 무기 이펙트 랜더
    public Sprite _tail1;
    public Sprite _tail2;

    public string _damageHitEffectName;
    public string _criticalHitEffectName;

    // 용병 애니메이션 델리게이트 이벤트
    public delegate void CrewAnimMoveHandler();
    public static event CrewAnimMoveHandler OnCrewAnimMove;

    public delegate void CrewAnimIdleHandler();
    public static event CrewAnimIdleHandler OnCrewAnimIdle;


    // 무기 이펙트 랜더의 스프라이트를 바꾸는 애니메이션 이벤트
    public void AttackTail1AnimationEvent()
    {
        _weaponTailRender.sprite = _tail1;
    }

    public void AttackTail2AnimationEvent()
    {
        _weaponTailRender.sprite = _tail2;
    }

    public void AttackTail3AnimationEvent()
    {
        _weaponTailRender.sprite = null;
    }


    // 공격시 배경 애니메이션 정지
    public void PlayAndStopBackGroundAnim()
    {
        if (_characterState._state == CCharacterState.State.ATTACK)
        {
            _backGroundsAnimator.speed = 0;
            _zeroTrAnimator.speed = 0f;
        }
        else if (_characterState._state == CCharacterState.State.MOVE)
        {
            _backGroundsAnimator.speed = 1;
            _zeroTrAnimator.speed = 1f;
        }
        else if (_characterState._state == CCharacterState.State.IDLE)
        {
            _backGroundsAnimator.speed = 0;
            _zeroTrAnimator.speed = 0f;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        _playerState = GetComponent<CPlayerState>();
        _playerInfo = GetComponent<CPlayerInfo>();
        endPos = _attackPoint.position;
        endPos.x += 0.1f;
    }


    protected override void Start()
    {
        base.Start();
        _backGroundsAnimator = GameObject.FindGameObjectWithTag("AnimBackGround").GetComponent<Animator>();
    }


    public override void Attack()
    {
        base.Attack();
        PlayAndStopBackGroundAnim();
        if (_playerInfo._crewInfoManager._crewPosition[0].childCount > 0 || _playerInfo._crewInfoManager._crewPosition[1].childCount > 0)
        {
            OnCrewAnimIdle();
        }
    }


    public override void AttackAnimationEvent()
    {
        // 공격 속도 받음
        //if (_characterState._attackSpeed != _characterState._originAttackSpeed)
        //{
        //    _animator.SetFloat("AttackSpeed", _characterState._attackSpeed);
        //    _characterState._originAttackSpeed = _characterState._attackSpeed;
        //}

        colliders = Physics2D.OverlapCircleAll(_attackPoint.position, 0.25f, _targetLayer);
        if (colliders.Length <= 0) return;

        minDist = 0;
        targetEnemy = null;

        // 복수 타격 갯수 받음
        _monsterCount = _playerState._attackCount;
        //List<GameObject> targetList = new List<GameObject>();

        // 가장 가까운 적 하나만 피격
        for (int i = 0; i < colliders.Length; i++)
        {
            dist = Vector2.Distance(_attackPoint.position, colliders[i].transform.position);

            if (targetEnemy == null)
            {
                targetEnemy = colliders[i].gameObject;
                //
                targetList.Add(targetEnemy);
                minDist = dist;
                continue;
            }
            //
            else
            {
                targetList.Add(colliders[i].gameObject);
            }

            if (minDist > dist)
            {
                targetEnemy = colliders[i].gameObject;
                minDist = dist;
            }
        }

        //
        if (colliders.Length < _monsterCount)
        {
            _monsterCount = colliders.Length;
        }

        _damageRanges = new ObscuredFloat[_monsterCount];
        _damages = new ObscuredFloat[_monsterCount];

        _isCritical = Random.Range(0f, _playerInfo._criticalPerMaximum);

        if (_playerState._criticalPer < _isCritical)
        {
            // 일반 데미지 호출
            for (int i = 0; i < _monsterCount; i++)
            {
                _damageRanges[i] = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
                _damages[i] = _characterState._attackDamage + _damageRanges[i];
                if (_damages[i] >= 2100000000f) _damages[i] = 2100000000f;

                targetList[i].GetComponent<CCharacterDamage>().Damage(_damages[i], _damageHitEffectName);
            }
            targetList.RemoveRange(0, targetList.Count);
        }
        else
        {
            // 크리티컬 데미지 호출
            for (int i = 0; i < _monsterCount; i++)
            {
                _damageRanges[i] = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
                _damages[i] = (_characterState._attackDamage * _playerState._criticalDamage) + (_damageRanges[i] * 0.5f);
                if (_damages[i] >= 2100000000f) _damages[i] = 2100000000f;

                //_playerState._cameraDoAnim.DORestartById("Critical");
                targetList[i].GetComponent<CCharacterDamage>().CriticalDamage(_damages[i], _criticalHitEffectName);
            }
            targetList.RemoveRange(0, targetList.Count);

            // 체력 흡수 처리
            if (_playerInfo._buffSkillInfoManager._buffSkillLevelArray[8] > 0) // 체력 흡수 버프의 레벨이 0보다 클때
            {
                _playerState.HpAbscorption(_damages[0]);
            }
        }


        //// 가장 가까운 적 하나만 피격
        //foreach (Collider2D enemy in colliders)
        //{
        //    float dist = Vector2.Distance(_attackPoint.position, enemy.transform.position);

        //    if (targetEnemy == null)
        //    {
        //        targetEnemy = enemy.gameObject;
        //        //
        //        targetList.Add(targetEnemy);
        //        minDist = dist;
        //        continue;
        //    }
        //    //
        //    else
        //    {
        //        targetList.Add(enemy.gameObject);
        //    }

        //    if (minDist > dist)
        //    {
        //        targetEnemy = enemy.gameObject;
        //        minDist = dist;
        //    }
        //}

        ////
        //if (colliders.Length < _monsterCount)
        //{
        //    _monsterCount = colliders.Length;
        //}

        //float[] _damageRanges = new float[_monsterCount];
        //float[] _damage = new float[_monsterCount];

        //float _isCritical = Random.Range(0f, 200f);

        //if (_playerState._criticalPer < _isCritical)
        //{
        //    for (int i = 0; i < _monsterCount; i++)
        //    {
        //        _damageRanges[i] = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
        //        _damage[i] = _characterState._attackDamage + _damageRanges[i];

        //        targetList[i].GetComponent<CCharacterDamage>().Damage(_damage[i]);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < _monsterCount; i++)
        //    {
        //        _damageRanges[i] = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
        //        _damage[i] = (_characterState._attackDamage * _playerState._criticalDamage) + _damageRanges[i];

        //        targetList[i].GetComponent<CCharacterDamage>().CriticalDamage(_damage[i]);
        //    }
        //}
        ///*
        //// colliders 배열에 수집된 모든 적들 피격
        //foreach (Collider2D enemy in colliders)
        //{
        //    enemy.SendMessage("Damage", 1f");
        //}
        //*/
    }


    // 공격 후 앞에 몬스터가 있는지 체크
    public void AttackFinish()
    {
        hitInfo = Physics2D.Linecast(_attackPoint.position, endPos, _targetLayer);
        //Debug.DrawLine(_attackPoint.position, endPos, Color.red);

        if (hitInfo.collider == null)
        {
            _characterState._state = CCharacterState.State.MOVE;
            _animator.SetBool("IsTargetOn", false);
            PlayAndStopBackGroundAnim();
            _targetCheck.TargetChecker(this);
            if (_playerInfo._crewInfoManager._crewPosition[0].childCount > 0 || _playerInfo._crewInfoManager._crewPosition[1].childCount > 0)
            {
                OnCrewAnimMove();
            }
        }
    }
}
