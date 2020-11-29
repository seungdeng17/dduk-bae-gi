using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrew01Function : CCrewState {

    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew01Function", _crewInfoManager._crewDelayTime[1]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew01Function");
    }

    private void Crew01Function()
    {
        if (!_playerState._isDie)
        {
            functionCount = -0.6f;
            _animator.SetTrigger("CrewFunction");
        }
    }

    public void Crew01FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[1] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 데미지를 보내는 코루틴
        StartCoroutine(OnFunction());

        // 대기시간
        Invoke("Crew01Function", _crewInfoManager._crewDelayTime[1]);
    }

    private WaitForSeconds functionDelayTime = new WaitForSeconds(0.3f);
    private IEnumerator OnFunction()
    {
        while (functionCount < 2f)
        {
            // 기능 정보 넣기 (공격)
            colliders = Physics2D.OverlapCircleAll(_crewAttackPoint.position + new Vector3((functionCount * 1f), 0.25f), 0.75f, _playerAttack._targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);
            }

            // 이펙트, 카메라 애니메이션
            Pooly.Spawn("Crew01Function", _crewAttackPoint.position + new Vector3((functionCount * 1f), 0.25f), Quaternion.identity);
            if (functionCount >= 1f) _playerState._cameraDoAnim.DORestartById("Clear");

            yield return functionDelayTime;
            functionCount += 1f;
        }
    }
}
