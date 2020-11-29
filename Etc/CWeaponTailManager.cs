using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무기 이펙트 관리
public class CWeaponTailManager : MonoBehaviour {

    public Sprite _tail1; // 1번 이펙트
    public Sprite _tail2; // 2번 이펙트

    private CPlayerAttack _playerAttack; // 플레이어 어택 참조

    private void Awake()
    {
        _playerAttack = GetComponentInParent<CPlayerAttack>();
    }

    private void OnEnable()
    {
        _playerAttack._tail1 = this._tail1;
        _playerAttack._tail2 = this._tail2;
    }
}
