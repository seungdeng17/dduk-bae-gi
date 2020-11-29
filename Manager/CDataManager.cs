using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class CDataManager : MonoBehaviour {

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CPlayerState _playerState;
    public CStageManager _stageManager;
    public CWeaponInfoManager _weaponInfoManager;
    public CBuffSkillInfoManager _buffSkillInfoManager;
    public CCrewInfoManager _crewInfoManager;


    private void Awake()
    {
        //DataLoad();
    }


    public void DataSave()
    {
        // CPlayerInfo
        ObscuredPrefs.SetString("PlayerName", _playerInfo._playerName); // 플레이어 이름
        ObscuredPrefs.SetInt("PlayerLevel", _playerInfo._playerLevel); // 플레이어 레벨
        ObscuredPrefs.SetFloat("PlayerAge", _playerInfo._playerAge); // 플레이어 나이

        ObscuredPrefs.SetLong("MyCoin", _playerInfo._my_Coin); // 보유 코인
        ObscuredPrefs.SetInt("MyRuby", _playerInfo._my_Ruby); // 보유 루비

        ObscuredPrefs.SetFloat("NowExp", _playerInfo._nowExp); // 현재 경험치
        ObscuredPrefs.SetFloat("NeedExp", _playerInfo._needExp); // 필요 경험치

        ObscuredPrefs.SetInt("StatPoint", _playerInfo._statPoint); // 스탯 포인트
        ObscuredPrefs.SetInt("ACStatPoint", _playerInfo._acStatPoint); // 누적 스탯 포인트

        // 스탯 레벨 정보 (순스탯)
        ObscuredPrefs.SetFloat("OriginSTR", _playerInfo._originSTR);
        ObscuredPrefs.SetFloat("OriginCON", _playerInfo._originCON);
        ObscuredPrefs.SetFloat("OriginDEF", _playerInfo._originDEF);
        ObscuredPrefs.SetFloat("OriginATS", _playerInfo._originATS);
        ObscuredPrefs.SetFloat("OriginCHC", _playerInfo._originCHC);
        ObscuredPrefs.SetFloat("OriginCHD", _playerInfo._originCHD);
        ObscuredPrefs.SetFloat("OriginEXP", _playerInfo._originEXP);
        ObscuredPrefs.SetFloat("OriginCOIN", _playerInfo._originCOIN);

        // 스탯 레벨 정보 (기능으로 올라간 스탯)
        ObscuredPrefs.SetFloat("UpValueSTR", _weaponInfoManager._upValueSTR);
        ObscuredPrefs.SetFloat("UpValueCON", _buffSkillInfoManager._upValueCON);
        ObscuredPrefs.SetFloat("UpValueDEF", _buffSkillInfoManager._upValueDEF);
        ObscuredPrefs.SetFloat("UpValueATS", _buffSkillInfoManager._upValueATS);
        ObscuredPrefs.SetFloat("UpValueCHC", _buffSkillInfoManager._upValueCHC);
        ObscuredPrefs.SetFloat("UpValueCHD", _buffSkillInfoManager._upValueCHD);
        ObscuredPrefs.SetFloat("UpValueEXP", _buffSkillInfoManager._upValueEXP);
        ObscuredPrefs.SetFloat("UpValueCOIN", _buffSkillInfoManager._upValueCOIN);

        // 무기
        ObscuredPrefs.SetInt("SelectWeaponNum", _playerInfo._selectWeaponNum); // 착용중인 무기 번호
        ObscuredPrefs.SetInt("AvailableWeaponNum", _playerInfo._availableWeaponNum); // 착용 가능한 무기 최대 번호

        // CPlayerState

        // CStageManager
        ObscuredPrefs.SetFloat("Stage", _stageManager._stage); // 현재 스테이지

        // CWeaponInfoManager
        // 무기 레벨 정보 배열
        for (int i = 0; i < _weaponInfoManager._weaponLevelArray.Length; i++)
        {
            ObscuredPrefs.SetInt("WeaponLevelArray" + i, _weaponInfoManager._weaponLevelArray[i]);
        }

        // CBuffSkillInfoManager
        // 버프 스킬 레벨 정보 배열
        for (int i = 0; i < _buffSkillInfoManager._buffSkillLevelArray.Length; i++)
        {
            ObscuredPrefs.SetInt("BuffLevelArray" + i, _buffSkillInfoManager._buffSkillLevelArray[i]);
        }

        // CCrewInfoManager
        // 용병 레벨 정보 배열
        for (int i = 0; i < _crewInfoManager._crewLevelArray.Length; i++)
        {
            ObscuredPrefs.SetInt("CrewLevelArray" + i, _crewInfoManager._crewLevelArray[i]);
        }

        // 용병 구매 유무 배열
        for (int i = 0; i < _crewInfoManager._crewGameObject.Length; i++)
        {
            ObscuredPrefs.SetBool("CrewBuyArray" + i, _crewInfoManager._crewGameObject[i].GetComponentInChildren<CCrewState>()._isBuy);
        }

        // 용병 선택 유무 배열
        for (int i = 0; i < _crewInfoManager._crewGameObject.Length; i++)
        {
            ObscuredPrefs.SetBool("CrewSelectArray" + i, _crewInfoManager._crewGameObject[i].GetComponentInChildren<CCrewState>()._isSelect);
        }
    }


    public void DataLoad()
    {
        // CPlayerInfo
        _playerInfo._playerName = ObscuredPrefs.GetString("PlayerName");
        _playerInfo._playerLevel = ObscuredPrefs.GetInt("PlayerLevel");
        if (_playerInfo._playerLevel <= 1) _playerInfo._playerLevel = 1;
        _playerInfo._playerAge = ObscuredPrefs.GetFloat("PlayerAge");

        _playerInfo._my_Coin = ObscuredPrefs.GetLong("MyCoin");
        _playerInfo._my_Ruby = ObscuredPrefs.GetInt("MyRuby");

        _playerInfo._nowExp = ObscuredPrefs.GetFloat("NowExp");
        _playerInfo._needExp = ObscuredPrefs.GetFloat("NeedExp");
        if (_playerInfo._needExp <= 2f) _playerInfo._needExp = 2f;

        _playerInfo._statPoint = ObscuredPrefs.GetInt("StatPoint");
        _playerInfo._acStatPoint = ObscuredPrefs.GetInt("ACStatPoint");

        // 스탯 레벨 정보 (순스탯)
        _playerInfo._originSTR = ObscuredPrefs.GetFloat("OriginSTR");
        _playerInfo._originCON = ObscuredPrefs.GetFloat("OriginCON");
        _playerInfo._originDEF = ObscuredPrefs.GetFloat("OriginDEF");
        _playerInfo._originATS = ObscuredPrefs.GetFloat("OriginATS");
        _playerInfo._originCHC = ObscuredPrefs.GetFloat("OriginCHC");
        _playerInfo._originCHD = ObscuredPrefs.GetFloat("OriginCHD");
        _playerInfo._originEXP = ObscuredPrefs.GetFloat("OriginEXP");
        _playerInfo._originCOIN = ObscuredPrefs.GetFloat("OriginCOIN");

        // 스탯 레벨 정보 (기능으로 올라간 스탯)
        _weaponInfoManager._upValueSTR = ObscuredPrefs.GetFloat("UpValueSTR");
        _buffSkillInfoManager._upValueCON = ObscuredPrefs.GetFloat("UpValueCON");
        _buffSkillInfoManager._upValueDEF = ObscuredPrefs.GetFloat("UpValueDEF");
        _buffSkillInfoManager._upValueATS = ObscuredPrefs.GetFloat("UpValueATS");
        _buffSkillInfoManager._upValueCHC = ObscuredPrefs.GetFloat("UpValueCHC");
        _buffSkillInfoManager._upValueCHD = ObscuredPrefs.GetFloat("UpValueCHD");
        _buffSkillInfoManager._upValueEXP = ObscuredPrefs.GetFloat("UpValueEXP");
        _buffSkillInfoManager._upValueCOIN = ObscuredPrefs.GetFloat("UpValueCOIN");

        // 무기
        _playerInfo._selectWeaponNum = ObscuredPrefs.GetInt("SelectWeaponNum");
        _playerInfo._availableWeaponNum = ObscuredPrefs.GetInt("AvailableWeaponNum");

        // CPlayerState

        // CStageManager
        _stageManager._stage = ObscuredPrefs.GetFloat("Stage");
        if (_stageManager._stage <= 1f) _stageManager._stage = 1f;

        // CWeaponInfoManager
        for (int i = 0; i < _weaponInfoManager._weaponLevelArray.Length; i++)
        {
            _weaponInfoManager._weaponLevelArray[i] = ObscuredPrefs.GetInt("WeaponLevelArray" + i);
        }

        // CBuffSkillInfoManager
        for (int i = 0; i < _buffSkillInfoManager._buffSkillLevelArray.Length; i++)
        {
            _buffSkillInfoManager._buffSkillLevelArray[i] = ObscuredPrefs.GetInt("BuffLevelArray" + i);
        }

        // CCrewInfoManager
        for (int i = 0; i < _crewInfoManager._crewLevelArray.Length; i++)
        {
            _crewInfoManager._crewLevelArray[i] = ObscuredPrefs.GetInt("CrewLevelArray" + i);
        }

        for (int i = 0; i < _crewInfoManager._crewGameObject.Length; i++)
        {
            _crewInfoManager._crewGameObject[i].GetComponentInChildren<CCrewState>()._isBuy = ObscuredPrefs.GetBool("CrewBuyArray" + i);
        }

        for (int i = 0; i < _crewInfoManager._crewGameObject.Length; i++)
        {
            _crewInfoManager._crewGameObject[i].GetComponentInChildren<CCrewState>()._isSelect = ObscuredPrefs.GetBool("CrewSelectArray" + i);
        }
    }
}
