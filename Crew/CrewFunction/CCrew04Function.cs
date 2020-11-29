using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrew04Function : CCrewState
{
    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew04Function", _crewInfoManager._crewDelayTime[4]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew04Function");
    }

    private void Crew04Function()
    {
        if (!_playerState._isDie)
        {
            _animator.SetTrigger("CrewFunction");
        }
    }

    private Transform _crew04Fuction_MoveObject;
    public void Crew04FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[4] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 이펙트
        _crew04Fuction_MoveObject = Pooly.Spawn("Crew04Function", gameObject.transform.position, Quaternion.identity);
        _crew04Fuction_MoveObject.GetComponentInChildren<CCrew04Function_MoveObject>().crewFunction_value = this.crewFunction_value;
        _crew04Fuction_MoveObject.GetComponentInChildren<CCrew04Function_MoveObject>().crewFunction_color = this.crewFunction_color;

        // 대기시간
        Invoke("Crew04Function", _crewInfoManager._crewDelayTime[4]);
    }
}
