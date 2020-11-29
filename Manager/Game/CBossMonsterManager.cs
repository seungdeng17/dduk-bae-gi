using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;


// 보스 몬스터 목록 관리
public class CBossMonsterManager : MonoBehaviour {

    [Header("< 스폰 딜레이 타임 >")]
    public ObscuredFloat _spawnDealyTime = 3f; // 스폰 딜레이 타임

    [Header("< 현재 보스 몬스터 스폰수 >")]
    public int _bossMonsterNum = 0; // 현재 보스 몬스터 스폰수

    [Header("< 보스 몬스터 최대 스폰수 >")]
    public int _bossMonsterMaxNum; // 몬스터 최대 스폰수

    [Header("< 보스 몬스터 스폰 위치 >")]
    public Transform _bossMonsterSpawnPosition; // 보스 몬스터 스폰 위치

    [Header("< 보스 몬스터 목록 >")]
    public string[] _bossMonsters; // 보스 몬스터 목록


    private void OnEnable()
    {
        BossMonsterSpawn();
    }


    // 다음 보스를 스폰
    public void BossMonsterSpawn()
    {
        StartCoroutine(NextBossMonsterSpawn(_bossMonsters[Random.Range(0, _bossMonsters.Length)]));
    }

    private IEnumerator NextBossMonsterSpawn(string bossMonsterName)
    {
        yield return new WaitForSeconds(_spawnDealyTime);
        _bossMonsterNum += 1;
        Pooly.Spawn(bossMonsterName, _bossMonsterSpawnPosition.position, Quaternion.identity);
    }
}
