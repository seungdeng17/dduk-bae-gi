using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;


// 몬스터 목록 관리
public class CMonsterManager : MonoBehaviour {

    [Header("< 스폰 딜레이 타임 >")]
    public ObscuredFloat _spawnDealyTime = 1f; // 스폰 딜레이 타임

    [Header("< 현재 몬스터 스폰수 (-1 해야 됨) >")]
    public int _monsterNum = 0; // 현재 몬스터 스폰수

    [Header("< 몬스터 최대 스폰수 >")]
    public int _monsterMaxNum; // 몬스터 최대 스폰수

    [Header("< 몬스터 스폰 위치 >")]
    public Transform[] _monsterSpawnPosition; // 몬스터 스폰 위치

    [Header("< 몬스터 목록 >")]
    public string[] _monsters; // 몬스터 목록


    private void OnEnable()
    {
        _monsterNum += 1;
        MonsterSpawn();
    }


    // 다음 몬스터를 스폰
    public void MonsterSpawn()
    {
        StartCoroutine(NextMonsterSpawn(_monsters[Random.Range(0, _monsters.Length)]));
    }

    private IEnumerator NextMonsterSpawn(string monsterName)
    {
        yield return new WaitForSeconds(_spawnDealyTime);
        Pooly.Spawn(monsterName, _monsterSpawnPosition[Random.Range(0, _monsterSpawnPosition.Length)].position, Quaternion.identity);
    }
}
