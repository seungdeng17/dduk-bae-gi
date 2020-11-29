using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrew02Function : CCrewState
{
    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew02Function", _crewInfoManager._crewDelayTime[2]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew02Function");
    }

    private void Crew02Function()
    {
        if (!_playerState._isDie)
        {
            functionCount = 0f;
            _animator.SetTrigger("CrewFunction");
        }
    }

    public void Crew02FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[2] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 데미지를 보내는 코루틴
        StartCoroutine(OnFunction());

        // 이펙트
        Pooly.Spawn("Crew02Function", _crewAttackPoint.position - new Vector3(0.3f, 0f, 0f), Quaternion.identity);

        // 대기시간
        Invoke("Crew02Function", _crewInfoManager._crewDelayTime[2]);
    }

    private WaitForSeconds functionDelayTime = new WaitForSeconds(0.35f);
    private IEnumerator OnFunction()
    {
        // 기능 정보 넣기 (공격)

        while (functionCount < 8f)
        {
            colliders = Physics2D.OverlapCircleAll(_crewAttackPoint.position - new Vector3(0.3f, 0f, 0f), 0.75f, _playerAttack._targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);
            }

            yield return functionDelayTime;
            functionCount += 1f;
        }
    }
}
