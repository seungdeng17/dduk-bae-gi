using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrew06Function : CCrewState
{
    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew06Function", _crewInfoManager._crewDelayTime[6]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew06Function");
    }

    private void Crew06Function()
    {
        if (!_playerState._isDie)
        {
            _animator.SetTrigger("CrewFunction");
        }
    }

    public void Crew06FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[6] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 데미지를 보내는 코루틴
        StartCoroutine(OnFunction());

        // 이펙트
        Pooly.Spawn("Crew06Function", _crewAttackPoint.position, Quaternion.identity);
    }

    private WaitForSeconds functionDelayTime = new WaitForSeconds(2.3f);
    private IEnumerator OnFunction()
    {
        yield return functionDelayTime;

        // 기능 정보 넣기 (공격)
        colliders = Physics2D.OverlapCircleAll(_crewAttackPoint.position, 2f, _playerAttack._targetLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);
        }

        // 카메라 애니메이션
        _playerState._cameraDoAnim.DORestartById("Clear");

        // 대기시간
        Invoke("Crew06Function", _crewInfoManager._crewDelayTime[6]);
    }
}
