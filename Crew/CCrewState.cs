using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrewState : MonoBehaviour {

    public Transform _crewAttackPoint;
    public CCrewInfoManager _crewInfoManager;

    public CPlayerInfo _playerInfo;
    public CPlayerState _playerState;
    public CPlayerAttack _playerAttack;

    public ObscuredBool _isBuy = false;
    public ObscuredBool _isSelect = false;

    protected Collider2D[] colliders;
    protected ObscuredFloat functionCount = 0f;

    public Color crewFunction_color;
    protected ObscuredFloat crewFunction_value;

    protected Animator _animator;
    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayerStateChek()
    {
        switch (_playerState._state)
        {
            case CCharacterState.State.MOVE:
                _animator.SetBool("PlayerAttack", false);
                break;
            case CCharacterState.State.ATTACK:
                _animator.SetBool("PlayerAttack", true);
                break;
            case CCharacterState.State.DIE:
                _animator.SetTrigger("PlayerDie");
                break;
        }
    }
}
