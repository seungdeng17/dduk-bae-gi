using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCrewAnimManager : MonoBehaviour {

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 용병 애니메이션 델리게이트 이벤트
        CPlayerAttack.OnCrewAnimMove += this.OnCrewAnimMove;
        CPlayerAttack.OnCrewAnimIdle += this.OnCrewAnimIdle;
        CPlayerState.OnCrewAnimDie += this.OnCrewAnimDie;
    }

    private void OnDisable()
    {
        // 용병 애니메이션 델리게이트 이벤트
        CPlayerAttack.OnCrewAnimMove -= this.OnCrewAnimMove;
        CPlayerAttack.OnCrewAnimIdle -= this.OnCrewAnimIdle;
        CPlayerState.OnCrewAnimDie -= this.OnCrewAnimDie;
    }

    // 용병 애니메이션 델리게이트 이벤트
    public void OnCrewAnimIdle()
    {
        _animator.SetBool("PlayerAttack", true);
    }

    public void OnCrewAnimMove()
    {
        _animator.SetBool("PlayerAttack", false);
    }

    public void OnCrewAnimDie()
    {
        //gameObject.GetComponent<CCrewState>().CancelInvoke();
        _animator.SetTrigger("PlayerDie");
    }
}
