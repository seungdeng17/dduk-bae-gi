using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDestroyer : MonoBehaviour {

    public float _destroyDelayTime; // 파괴 지연 시간
    public bool _isAutoDestyroy; // 자동 파괴 여부

    protected virtual void Start()
    {
        if (_isAutoDestyroy) Destroy();
    }

    public virtual void Destroy()
    {
        Destroy(transform.root.gameObject, _destroyDelayTime);
    }
}
