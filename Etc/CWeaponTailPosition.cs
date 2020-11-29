using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기에 따라서 무기 이펙트의 위치가 달라지게 + 무기 이펙트 설정
public class CWeaponTailPosition : MonoBehaviour {

    private CPlayerAttack _playerAttack;

    [Header("< 무기에 해당하는 이펙트 >")]
    public GameObject _weaponTail; // 무기에 해당하는 이펙트

    [Header("< 이펙트가 위치 할 포지션 >")]
    public Transform _weaponTailPosition; // 이펙트가 위치 할 포지션

    [Header("< 위치가 안맞을 경우 변경 할 포지션 >")]
    public Vector2 _changeWeaponTailPosition; // 위치가 안맞을 경우 변경 할 포지션

    [Header("< 타격 이펙트>")]
    // 타격 이펙트
    public string _criticalHitEffectName;
    public string _damageHitEffectName;

    private void Awake()
    {
        _playerAttack = GetComponentInParent<CPlayerAttack>();
    }

    private void OnEnable()
    {
        _weaponTail.SetActive(true);
        _weaponTailPosition.localPosition = _changeWeaponTailPosition;

        _playerAttack._criticalHitEffectName = _criticalHitEffectName;
        _playerAttack._damageHitEffectName = _damageHitEffectName;
    }
}
