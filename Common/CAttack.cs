using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class CAttack : MonoBehaviour {

    protected CCharacterState _characterState;
    protected CTargetCheck _targetCheck;
    protected Animator _animator;
    //public bool _isChecking = false;

    protected ObscuredFloat _damageRange;
    protected ObscuredFloat _damage;

    public Transform _attackPoint;
    public LayerMask _targetLayer; // 타겟 레이어

    private Collider2D _attackCollider;


    protected virtual void Awake()
    {
        _characterState = GetComponent<CCharacterState>();
        _targetCheck = GetComponent<CTargetCheck>();
        _animator = GetComponent<Animator>();
    }


    protected virtual void Start()
    {
        _targetCheck.TargetChecker(this);

        //_isChecking = true;
        //if (_isChecking)
        //{
        //    _targetCheck.TargetChecker(this);
        //}
    }


    public virtual void Attack()
    {
        _characterState._state = CCharacterState.State.ATTACK;
        _animator.SetBool("IsTargetOn", true);
    }


    public virtual void AttackAnimationEvent()
    {
        _attackCollider = Physics2D.OverlapCircle(_attackPoint.position, 0.3f, _targetLayer);
        if (_attackCollider == null) return;

        // 데미지 세팅
        _damageRange = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
        _damage = _characterState._attackDamage + _damageRange;
        if (_damage > 2100000000f) _damage = 2100000000f;

        _attackCollider.GetComponent<CCharacterDamage>().Damage(_damage);

        //// 가장 가까운 적 하나만 피격
        //foreach (Collider2D enemy in colliders)
        //{
        //    float dist = Vector2.Distance(_attackPoint.position, enemy.transform.position);

        //    if (targetEnemy == null)
        //    {
        //        targetEnemy = enemy.gameObject;
        //        minDist = dist;
        //        continue;
        //    }

        //    if (minDist > dist)
        //    {
        //        targetEnemy = enemy.gameObject;
        //        minDist = dist;
        //    }
        //}
        //// 데미지 세팅
        //float _damageRange = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
        //float _damage = _characterState._attackDamage + _damageRange;

        ////targetEnemy.SendMessage("Damage", Mathf.Abs(_damage), SendMessageOptions.DontRequireReceiver);
        //targetEnemy.GetComponent<CCharacterDamage>().Damage(_damage);

        ///*
        //// colliders 배열에 수집된 모든 적들 피격
        //foreach (Collider2D enemy in colliders)
        //{
        //    enemy.SendMessage("Damage", 1f");
        //}
        //*/
    }
}
