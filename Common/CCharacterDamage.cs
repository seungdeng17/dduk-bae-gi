using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class CCharacterDamage : MonoBehaviour {

    protected CCharacterState _characterState;
    protected Animator _animator;


    protected virtual void Awake()
    {
        _characterState = GetComponent<CCharacterState>(); // 캐릭터 상태 정보
        _animator = GetComponent<Animator>(); // 애니메이터
    }


    // 피격 처리
    public virtual void Damage(ObscuredFloat damage, string hitEffectName = null)
    {
        if (_characterState._hp > 0 && _characterState._state != CCharacterState.State.DIE)
        {
            damage -= _characterState._defensive;
            if (damage <= 0) damage = 1f;

            _characterState.HpDown(damage);
            _animator.Play("Damage", _animator.GetLayerIndex("Damage Layer"));
        }
    }


    // 크리티컬 피격
    public virtual void CriticalDamage(ObscuredFloat c_damage, string hitEffectName = null)
    {
        if (_characterState._hp > 0)
        {
            c_damage -= _characterState._defensive;
            if (c_damage <= 0) c_damage = 2f;

            _characterState.HpDown(c_damage);
            _animator.Play("Damage", _animator.GetLayerIndex("Damage Layer"));
        }
    }
}
