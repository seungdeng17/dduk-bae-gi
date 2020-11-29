using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class CCharacterState : MonoBehaviour {

    [Header("< 현재 일반 능력치 >")]
    public ObscuredFloat _attackDamage; // 공격력
    [HideInInspector]
    public ObscuredFloat _attackDamageRange; // 공격력 차이
    public ObscuredFloat _hp; // 체력
    public ObscuredFloat _defensive; // 방어력
    public ObscuredFloat _attackSpeed = 1f; // 공격 속도

    [HideInInspector]
    public ObscuredFloat _originAttackDamage; // 원래 공격력 (몬스터 스탯 저장용)
    [HideInInspector]
    public ObscuredFloat _originHp; // 원래 체력 (플레이어, 몬스터 스탯 저장용)
    [HideInInspector]
    public ObscuredFloat _originDefensive; // 원래 방어력 (몬스터 스탯 저장용)
    [HideInInspector]
    public ObscuredFloat _originAttackSpeed; // 원래 공격 속도 (몬스터 스탯 저장용)

    [Header("< 사망 여부 >")]
    public ObscuredBool _isDie = false;

    [Header("< 오른쪽을 보고 있는지 여부 >")]
    public ObscuredBool _isRightDir = false;

    public enum State { MOVE, IDLE, ATTACK, DIE }; // 캐릭터 상태
    [Header("< 현재 캐릭터 상태 >")]
    public State _state;

    [Header("< 추가 기타 >")]
    public EnergyBar _hpBar; // 에너지 바 참조

    [HideInInspector]
    public Animator _animator;


    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    protected virtual void Start()
    {
        // 원래 스탯 정보 저장
        OriginSetting();

        // 공격력 범위, 공격 속도 세팅
        _attackDamageRange = _attackDamage * 0.5f;
        _animator.SetFloat("AttackSpeed", _attackSpeed);

        // 체력바 세팅
        HpBarSetting();
    }


    // 원래 스텟 정보 저장 메서드
    protected virtual void OriginSetting()
    {
        _originHp = _hp;
        _originDefensive = _defensive;
        _originAttackDamage = _attackDamage;
        _originAttackSpeed = _attackSpeed;
    }


    // 체력바 세팅
    protected virtual void HpBarSetting()
    {
        _hpBar.valueMax = (int)_hp;
        HpBarRefresh(_hp);
    }


    // 체력 감소 처리
    public virtual ObscuredFloat HpDown(ObscuredFloat damage)
    {
        // 체력 감소
        _hp -= damage;

        // 체력 바 갱신
        HpBarRefresh(_hp);

        return _hp;
    }


    // Hp바 갱신
    public virtual void HpBarRefresh(ObscuredFloat _hp)
    {
        _hpBar.ValueF = _hp / _originHp;
    }


    // 정수 단위 콤마 (1,000)
    public string CommaText(double num)
    {
        if (num <= 0)
        {
            return "0";
        }
        else
        {
            return string.Format("{0:#,###}", num);
        }
    }

    // 소수점 단위 콤마 (1,000.00)
    public string CommaText2(double num)
    {
        if (num <= 0)
        {
            return "0";
        }
        else if (num < 1)
        {
            return string.Format("{0:0.00}", num);
        }
        else
        {
            return string.Format("{0:#,###.##}", num);
        }
    }
}
