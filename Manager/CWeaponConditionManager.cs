using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Ez.Pooly;


// 무기 착용조건이 되는지 확인 -> 되면 버튼 이미지 비활성화
public class CWeaponConditionManager : MonoBehaviour {

    public CPlayerInfo _playerInfo;
    public CWeaponSelectionManager _weaponSelectionManager;
    public CWeaponInfoManager _weaponInfoManager;
    public ObscuredBool _weaponTesting = false; // 테스트용 옵션

    private Vector3 _LockOffPosDown = new Vector3(0f, 0.65f, 0f); 


    // 조건 충족시 메서드
    public void ConditionEnough(ObscuredInt weaponNum)
    {
        Destroy(_weaponSelectionManager._weaponConditionCheckButton[weaponNum]); // 조건 체크용 버튼을 지움
        _weaponSelectionManager._weaponToggleImages[weaponNum].color = _weaponSelectionManager._weaponIconColor; // 토글의 무기 아이콘을 원래 색으로
        if (_weaponSelectionManager._weaponConditionText[weaponNum + 1] != null)
        {
            _weaponSelectionManager._weaponConditionText[weaponNum + 1].text = _weaponSelectionManager._weaponCondition[weaponNum + 1];
        }

        _weaponInfoManager.OnWeaponFunction(false, weaponNum); // 단순 Text 갱신을 위해
        _weaponInfoManager.OnWeaponFunction(true, _playerInfo._selectWeaponNum); // 다시 착용중인 무기로 갱신

        _playerInfo._availableWeaponNum += 1; // 사용 가능 무기 증가 (1로 고정)

        // 잠금 해제 이펙트
        Pooly.Spawn("Lock", _weaponSelectionManager._weaponToggles[weaponNum].transform.position - _LockOffPosDown, Quaternion.identity);
    }


    // 무기 착용 조건 확인
    // 01. 식칼
    public void OnConditionCheckWeapon01()
    {
        if (_weaponInfoManager._weaponLevelArray[0] >= 50 || _playerInfo._availableWeaponNum >= 1 || _weaponTesting)
        {
            ConditionEnough(1);
        }
        else
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCondition);
        }
    }

    // 02. 작지만 영롱해 
    public void OnConditionCheckWeapon02()
    {
        if (_weaponInfoManager._weaponLevelArray[1] >= 50 || _playerInfo._availableWeaponNum >= 2 || _weaponTesting)
        {
            ConditionEnough(2);
        }
        else
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCondition);
        }
    }

    // 03. 만들다 만
    public void OnConditionCheckWeapon03()
    {
        if (_weaponInfoManager._weaponLevelArray[2] >= 50 || _playerInfo._availableWeaponNum >= 3 || _weaponTesting)
        {
            ConditionEnough(3);
        }
        else
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCondition);
        }
    }

    // 04. 좀 멋진
    public void OnConditionCheckWeapon04()
    {
        if (_weaponInfoManager._weaponLevelArray[3] >= 50 || _playerInfo._availableWeaponNum >= 4 || _weaponTesting)
        {
            ConditionEnough(4);
        }
        else
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCondition);
        }
    }

    // 05. 멋지고 영롱해
    public void OnConditionCheckWeapon05()
    {
        if (_weaponInfoManager._weaponLevelArray[4] >= 50 || _playerInfo._availableWeaponNum >= 5 || _weaponTesting)
        {
            ConditionEnough(5);
        }
        else
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCondition);
        }
    }
}
