using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 백그라운드에 차일드 된 오브젝트를 ZeroTr로 옮김
public class CNoChiled : MonoBehaviour {

    private int _chiledCount;
    public Transform _zeroTr;
    public Animator _zeroTrAnimator;

    public void BackGroundEndAnimationEvent1()
    {
        _zeroTrAnimator.Play("ZeroTr");
    }

    public void BackGroundEndAnimationEvent2()
    {
        _chiledCount = transform.childCount;

        for (int i = 0; i < _chiledCount; i++)
        {
            if (transform.GetChild(0) != null)
            {
                transform.GetChild(0).parent = _zeroTr;
            }
        }
    }
}
