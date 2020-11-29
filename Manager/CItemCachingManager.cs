using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;

public class CItemCachingManager : MonoBehaviour {

    public Transform _cachingPoint;
    private Transform _coin;

    public void Start()
    {
        // 코인 캐싱
        for (int i = 0; i < 15; i++)
        {
            _coin = Pooly.Spawn("Coin", _cachingPoint.position, Quaternion.identity);
            Pooly.Despawn(_coin);
            _coin.GetComponent<CCoin>()._isInit = true;
        }
    }
}
