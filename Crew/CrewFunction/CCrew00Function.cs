using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;

public class CCrew00Function : CCrewState {

    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew00Function", _crewInfoManager._crewDelayTime[0]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew00Function");
    }

    private void Crew00Function()
    {
        if (!_playerState._isDie)
        {
            _animator.SetTrigger("CrewFunction");
        }
    }

    public void Crew00FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[0] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 기능 정보 넣기 (공격)
        colliders = Physics2D.OverlapCircleAll(_crewAttackPoint.position, 2f, _playerAttack._targetLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);
        }

        // 이펙트, 카메라 애니메이션
        Pooly.Spawn("Crew00Function", _crewAttackPoint.position, Quaternion.identity);
        _playerState._cameraDoAnim.DORestartById("Clear");

        // 대기시간
        Invoke("Crew00Function", _crewInfoManager._crewDelayTime[0]);
    }
}
