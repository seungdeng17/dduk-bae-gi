using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;


// 하드 보스 몬스터 목록 관리
public class CHardBossMonsterManager : MonoBehaviour {

    [Header("< 스폰 딜레이 타임 >")]
    public ObscuredFloat _spawnDealyTime = 3f; // 스폰 딜레이 타임

    [Header("< 현재 하드 보스 몬스터 스폰수 >")]
    public int _hardBossMonsterNum = 0; // 현재 보스 몬스터 스폰수

    [Header("< 하드 보스 몬스터 최대 스폰수 >")]
    public int _hardBossMonsterMaxNum; // 몬스터 최대 스폰수

    [Header("< 하드 보스 몬스터 스폰 위치 >")]
    public Transform _hardBossMonsterSpawnPosition; // 보스 몬스터 스폰 위치

    [Header("< 하드 보스 몬스터 목록 >")]
    public string[] _hardBossMonsters; // 보스 몬스터 목록


    private void OnEnable()
    {
        HardBossMonsterSpawn();
    }


    // 다음 하드 보스를 스폰
    public void HardBossMonsterSpawn()
    {
        StartCoroutine(NextHardBossMonsterSpawn(_hardBossMonsters[Random.Range(0, _hardBossMonsters.Length)]));
    }

    private IEnumerator NextHardBossMonsterSpawn(string hardBossMonsterName)
    {
        yield return new WaitForSeconds(_spawnDealyTime);
        _hardBossMonsterNum += 1;
        Pooly.Spawn(hardBossMonsterName, _hardBossMonsterSpawnPosition.position, Quaternion.identity);
    }
}
