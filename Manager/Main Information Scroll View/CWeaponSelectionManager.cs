using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;
using Ez.Pooly;


// 무기 선택 관리
public class CWeaponSelectionManager : MonoBehaviour {

    [Header("< 무기 토글 버튼 >")]
    public Toggle[] _weaponToggles; // 착용중인 무기 토글

    [Header("< 토글의 무기 이미지 >")]
    public Image[] _weaponToggleImages; // 토글의 무기 이미지

    [Header("< 토글의 무기 이름 텍스트 >")]
    public Text[] _weaponToggleNameTexts; // 토글의 무기 이름 텍스트

    [Header("< 토글의 무기 기능 텍스트 >")]
    public Text[] _weaponToggleFunctionTexts; // 토글의 무기 기능 텍스트

    [Header("< 강화 버튼 필요 코인 텍스트 >")]
    public Text[] _weaponNeedCoinText; // 업그레이드 필요 코인 텍스트

    [Header("< 무기 착용 조건 확인 버튼 오브젝트 >")]
    public GameObject[] _weaponConditionCheckButton; // 무기 착용 조건 확인 버튼 오브젝트

    [Header("< 착용중인 무기 실제 오브젝트 (Player 자식) >")]
    public GameObject[] _weaponGameObjects; // 착용중인 무기 오브젝트

    [Header("< 무기 착용 조건 확인 텍스트 >")]
    public Text[] _weaponConditionText; // 무기 착용 조건 확인 텍스트

    [Header("< 무기 착용 조건 확인 텍스트 내용 >")]
    public string[] _weaponCondition; // 무기 착용 조건 확인 텍스트 내용 

    [Header("< 참조 스크립트 >")]
    public CWeaponInfoManager _weaponInfoManager;
    public CPlayerInfo _playerInfo;

    [Header("< 추가 기타 >")]
    public Transform _weaponUpgradeEffectParent; 
   
    [HideInInspector]
    public Color _weaponIconColor = new Color(1f, 1f, 1f);

    private int _chiledCount;


    // 무기 토글버튼 선택시 메서드
    public void OnWeaponToggle(ObscuredInt weaponNum)
    {
        if (_weaponToggles[weaponNum].isOn)
        {
            // 무기 능력치 옵션 적용
            _weaponInfoManager.OnWeaponFunction(true, weaponNum);

            _playerInfo._selectWeaponNum = weaponNum;
        }
        else
        {
            // 무기 능력치 옵션 적용 해제
            _weaponInfoManager.OnWeaponFunction(false, weaponNum);
        }
    }


    // 레벨업시 필요 코인 증가 메서드
    public ObscuredDouble PlayerCoinDown(ObscuredBool isCoinDown, ObscuredBool isUpgradeSuccess, Text needCoinText, ObscuredDouble needCoin, ObscuredFloat ratio)
    {
        if (isCoinDown)
        {
            _playerInfo._my_Coin -= (long)needCoin;
        }
        _playerInfo._my_CoinText.DOText(_playerInfo._playerState.CommaText(_playerInfo._my_Coin).ToString(), 0.3f, true, ScrambleMode.Numerals, null);
        if (isUpgradeSuccess)
        {
            needCoin *= ratio;
            if (needCoin > 9100000000000000000f) needCoin = 9100000000000000000f;
            needCoinText.text = _playerInfo._playerState.CommaText((long)needCoin).ToString();
        }
        return needCoin;
    }


    // 업그레이드 이펙트 스폰 메서드
    public void SpawnWeaponUpEffect(ObscuredInt weaponNum, string particleName)
    {
        //_chiledCount = _weaponToggleImages[weaponNum].transform.transform.childCount;
        _chiledCount = _weaponUpgradeEffectParent.childCount;

        if (_chiledCount > 2)
        {
            //Pooly.Despawn(_weaponToggleImages[weaponNum].transform.GetChild(0).transform);
            Pooly.Despawn(_weaponUpgradeEffectParent.GetChild(0).transform);
        }
        //Pooly.Spawn(particleName, _weaponToggleImages[weaponNum].transform.position, Quaternion.identity, _weaponToggleImages[weaponNum].transform);
        Pooly.Spawn(particleName, _weaponToggleImages[weaponNum].transform.position, Quaternion.identity, _weaponUpgradeEffectParent);
    }


    // 무기 업그레이드 메서드 -> WeaponUpgrade(무기 번호, 증가할 공격력 기능) : 무기 번호의 공격력을 증가할 공격력 만큼 올려라
    public void WeaponUpgrade(ObscuredInt weaponNum, ObscuredFloat upgradeSTRNum)
    {
        if (!_playerInfo._playerState._isDie && _playerInfo._selectWeaponNum == weaponNum)
        {
            // 보유 골드가 필요 골드보다 많고 성공 확률일때

            // 업그레이드 성공
            if (_playerInfo._my_Coin >= (long)_weaponInfoManager._weaponUpNeedCoinArray[weaponNum] && _weaponInfoManager._weaponUpChanceNum <= _weaponInfoManager._weaponUpPercent[weaponNum])
            {
                // 레벨업
                _weaponInfoManager._weaponLevelArray[weaponNum] += 1;

                // 골드 차감
                _weaponInfoManager._weaponUpNeedCoinArray[weaponNum] = 
                    PlayerCoinDown(true, true, _weaponNeedCoinText[weaponNum], _weaponInfoManager._weaponUpNeedCoinArray[weaponNum], _weaponInfoManager._weaponUpRatioArray[weaponNum]);

                // 확률 감소
                _weaponInfoManager._weaponUpPercent[weaponNum] -= 0.25f;
                _weaponInfoManager._weaponUpPercent[weaponNum] = Mathf.Clamp(_weaponInfoManager._weaponUpPercent[weaponNum], 20f, 100f); // 최대 20까지 감소

                // 기능 반영 (공격력)
                _weaponInfoManager._weaponFunctionSTRArray[weaponNum] += upgradeSTRNum;
                _weaponInfoManager.OnWeaponFunction(true, weaponNum);

                // 말풍선
                _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponUpSuccess[Random.Range(0, _playerInfo._speechBubbleManager._weaponUpSuccess.Length)]);

                // 이펙트
                SpawnWeaponUpEffect(weaponNum, "WeaponUpgradeSuccess");
            }

            // 업그레이드 실패
            else if (_playerInfo._my_Coin >= (long)_weaponInfoManager._weaponUpNeedCoinArray[weaponNum] && _weaponInfoManager._weaponUpChanceNum > _weaponInfoManager._weaponUpPercent[weaponNum])
            {
                // 골드 차감
                _weaponInfoManager._weaponUpNeedCoinArray[weaponNum] = 
                    PlayerCoinDown(true, false, _weaponNeedCoinText[weaponNum], _weaponInfoManager._weaponUpNeedCoinArray[weaponNum], _weaponInfoManager._weaponUpRatioArray[weaponNum]);

                // 말풍선
                _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponUpFaile[Random.Range(0, _playerInfo._speechBubbleManager._weaponUpFaile.Length)]);

                // 이펙트
                SpawnWeaponUpEffect(weaponNum, "WeaponUpgradeFaile");
            }

            // 코인 부족
            else if (_playerInfo._my_Coin < (long)_weaponInfoManager._weaponUpNeedCoinArray[weaponNum])
            {
                // 말풍선
                _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotEnoughCoin);
            }
        }

        // 착용중이 아님
        else if (!_playerInfo._playerState._isDie && _playerInfo._selectWeaponNum != weaponNum)
        {
            // 말풍선
            _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._weaponNotWear);
        }
    }


    // 토글 선택
    // 00. 나무 몽둥이 선택
    public void OnToggleWeapon00()
    {
        OnWeaponToggle(0);
    }

    // 01. 식칼 선택
    public void OnToggleWeapon01()
    {
        OnWeaponToggle(1);
    }

    // 02. 작지만 영롱해 단검 선택
    public void OnToggleWeapon02()
    {
        OnWeaponToggle(2);
    }

    // 03. 만들다 만 선택
    public void OnToggleWeapon03()
    {
        OnWeaponToggle(3);
    }

    // 04. 좀 멋진 선택
    public void OnToggleWeapon04()
    {
        OnWeaponToggle(4);
    }

    // 05. 멋지고 영롱해 선택
    public void OnToggleWeapon05()
    {
        OnWeaponToggle(5);
    }


    // 업그레이드 버튼 클릭
    // 00. 나무 몽둥이 업그레이드
    public void OnUpBtnClickWeapon00()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon00Invoke();
            InvokeRepeating("OnUpBtnClickWeapon00Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon00Invoke");
        }
    }
    public void OnUpBtnClickWeapon00_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon00Invoke");
    }
    private void OnUpBtnClickWeapon00Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(0, 0.5f); // 0번 무기의 공격력 %를 0.5올려라
    }


    // 01. 식칼 업그레이드
    public void OnUpBtnClickWeapon01()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon01Invoke();
            InvokeRepeating("OnUpBtnClickWeapon01Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon01Invoke");
        }
    }
    public void OnUpBtnClickWeapon01_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon01Invoke");
    }
    private void OnUpBtnClickWeapon01Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(1, 1f);
    }


    // 02. 작지만 영롱해 업그레이드
    public void OnUpBtnClickWeapon02()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon02Invoke();
            InvokeRepeating("OnUpBtnClickWeapon02Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon02Invoke");
        }
    }
    public void OnUpBtnClickWeapon02_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon02Invoke");
    }
    private void OnUpBtnClickWeapon02Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(2, 1.5f);
    }


    // 03. 만들다 만 업그레이드
    public void OnUpBtnClickWeapon03()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon03Invoke();
            InvokeRepeating("OnUpBtnClickWeapon03Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon03Invoke");
        }
    }
    public void OnUpBtnClickWeapon03_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon03Invoke");
    }
    private void OnUpBtnClickWeapon03Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(3, 2f);
    }


    // 04. 좀 멋진 업그레이드
    public void OnUpBtnClickWeapon04()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon04Invoke();
            InvokeRepeating("OnUpBtnClickWeapon04Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon04Invoke");
        }
    }
    public void OnUpBtnClickWeapon04_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon04Invoke");
    }
    private void OnUpBtnClickWeapon04Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(4, 2.5f);
    }


    // 05. 멋지고 영롱해 업그레이드
    public void OnUpBtnClickWeapon05()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnUpBtnClickWeapon05Invoke();
            InvokeRepeating("OnUpBtnClickWeapon05Invoke", 0.5f, 0.125f);
        }
        else
        {
            CancelInvoke("OnUpBtnClickWeapon05Invoke");
        }
    }
    public void OnUpBtnClickWeapon05_Cancel()
    {
        CancelInvoke("OnUpBtnClickWeapon05Invoke");
    }
    private void OnUpBtnClickWeapon05Invoke()
    {
        _weaponInfoManager._weaponUpChanceNum = Random.Range(0f, 100f);
        WeaponUpgrade(5, 3f);
    }
}
