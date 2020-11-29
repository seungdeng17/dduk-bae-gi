using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;

public class CCrew03Function : CCrewState
{
    private void OnEnable()
    {
        PlayerStateChek();
        Invoke("Crew03Function", _crewInfoManager._crewDelayTime[3]);
    }

    private void OnDisable()
    {
        CancelInvoke("Crew03Function");
    }

    private void Crew03Function()
    {
        if (!_playerState._isDie)
        {
            functionCount = 0f;
            _animator.SetTrigger("CrewFunction");
        }
    }

    public void Crew03FunctionAnimationEvent()
    {
        // 데미지 세팅
        crewFunction_value = _playerState._attackDamage * (_crewInfoManager._crewFunction[3] * 0.01f);
        if (crewFunction_value >= 2100000000f) crewFunction_value = 2100000000f;

        // 데미지를 보내는 코루틴
        StartCoroutine(OnFunction());

        // 대기시간
        Invoke("Crew03Function", _crewInfoManager._crewDelayTime[3]);
    }

    private WaitForSeconds functionDelayTime = new WaitForSeconds(0.35f);
    private IEnumerator OnFunction()
    {
        while (functionCount < 3f)
        {
            // 기능 정보 넣기 (공격)
            colliders = Physics2D.OverlapCircleAll(_crewAttackPoint.position - new Vector3(0.6f, 0f, 0f), 0.5f, _playerAttack._targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);

                // 이펙트
                Pooly.Spawn("Crew03Function", colliders[i].transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
            }

            // 카메라 애니메이션
            if (colliders.Length > 0 && functionCount == 2f) _playerState._cameraDoAnim.DORestartById("Clear");

            yield return functionDelayTime;
            functionCount += 1f;
        }
    }
}
