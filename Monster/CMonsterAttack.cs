using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CMonsterAttack : CAttack {

    protected CDirectMove _directMove;

    [Header("< 플레이어 데미지 스크립트 >")]
    public CCharacterDamage _playerDamage;


    protected override void Awake()
    {
        base.Awake();
        _directMove = GetComponent<CDirectMove>();
    }


    protected void OnSpawned()
    {
        if (_playerDamage == null)
        {
            _playerDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<CCharacterDamage>();
        }

        _targetCheck.TargetChecker(this);

        //_isChecking = true;
        //if (_isChecking)
        //{
        //    _targetCheck.TargetChecker(this);
        //}
    }


    public override void Attack()
    {
        _directMove._rigidbody2d.velocity = Vector2.zero;
        base.Attack();
    }


    public override void AttackAnimationEvent()
    {
        // 데미지 세팅
        _damageRange = Random.Range(-_characterState._attackDamageRange, _characterState._attackDamageRange);
        _damage = _characterState._attackDamage + _damageRange;
        if (_damage > 2100000000f) _damage = 2100000000f;

        _playerDamage.Damage(_damage);
    }
}
