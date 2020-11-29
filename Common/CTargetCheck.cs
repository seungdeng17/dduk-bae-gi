using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CTargetCheck : MonoBehaviour {

    protected CCharacterState _characterState;

    public Transform _attackPoint;
    public float distance; // 탐지 거리
    public LayerMask _targetLayer; // 타겟 레이어

    protected Vector2 endPos;
    protected RaycastHit2D hitInfo;

    protected virtual void Awake()
    {
        _characterState = GetComponent<CCharacterState>();
    }


    public virtual void TargetChecker(CAttack attack)
    {
        StartCoroutine(TargetCheckerCoroutine(attack));
    }


    protected virtual IEnumerator TargetCheckerCoroutine(CAttack attack)
    {
        while (_characterState._state == CCharacterState.State.MOVE)
        {
            endPos = new Vector2(_attackPoint.position.x + ((_characterState._isRightDir) ? distance : -distance), _attackPoint.position.y);
            //Debug.DrawLine(_attackPoint.position, endPos, Color.red);

            hitInfo = Physics2D.Linecast(_attackPoint.position, endPos, _targetLayer);
            if (hitInfo.collider != null)
            {
                attack.Attack();
            }
            yield return null;
        }
    }
}
