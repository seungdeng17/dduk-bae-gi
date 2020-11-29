using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CMenuToggleManager : MonoBehaviour {

    public ScrollRect _mainScroll; // 메인 정보 스크롤뷰 참조

    private void OnInertia()
    {
        _mainScroll.inertia = true;
    }

    // 내 정보 토글
    [Header("< 내 정보 토글 >")]
    public Toggle _characterStateToggle;
    public Animator _characterStateAnimator;
    public RectTransform _characterStateContents;

    public void OnCharacterStateToggle()
    {
        if (_characterStateToggle.isOn)
        {
            _characterStateAnimator.Play("ToggleOn");
            _mainScroll.content = _characterStateContents;
            _characterStateContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _characterStateAnimator.Play("ToggleOff");
            _characterStateContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 스탯 토글
    [Header("< 스탯 토글 >")]
    public Toggle _statToggle; // 메뉴 토글 버튼
    public Animator _statAnimator; // 토글 버튼 애니메이션
    public RectTransform _statContents; // 토글 선택시 표시할 콘텐츠

    public void OnStatToggle()
    {
        if (_statToggle.isOn)
        {
            _statAnimator.Play("ToggleOn");
            _mainScroll.content = _statContents;
            _statContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _statAnimator.Play("ToggleOff");
            _statContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 무기고 토글
    [Header("< 무기고 토글 >")]
    public Toggle _weaponToggle; // 메뉴 토글 버튼
    public Animator _weaponAnimator; // 토글 버튼 애니메이션
    public RectTransform _weaponContents; // 토글 선택시 표시할 콘텐츠

    public void OnWeaponToggle()
    {
        if (_weaponToggle.isOn)
        {
            _weaponAnimator.Play("ToggleOn");
            _mainScroll.content = _weaponContents;
            _weaponContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _weaponAnimator.Play("ToggleOff");
            _weaponContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 지속기술 토글
    [Header("< 지속기술 토글 >")]
    public Toggle _buffSkillToggle; // 메뉴 토글 버튼
    public Animator _buffSkillAnimator; // 토글 버튼 애니메이션
    public RectTransform _buffSkillContents; // 토글 선택시 표시할 콘텐츠

    public void OnBuffSkillToggle()
    {
        if (_buffSkillToggle.isOn)
        {
            _buffSkillAnimator.Play("ToggleOn");
            _mainScroll.content = _buffSkillContents;
            _buffSkillContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _buffSkillAnimator.Play("ToggleOff");
            _buffSkillContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 동료 토글
    [Header("< 동료 토글 >")]
    public Toggle _crewToggle; // 메뉴 토글 버튼
    public Animator _crewAnimator; // 토글 버튼 애니메이션
    public RectTransform _crewContents; // 토글 선택시 표시할 콘텐츠

    public void OnCrewToggle()
    {
        if (_crewToggle.isOn)
        {
            _crewAnimator.Play("ToggleOn");
            _mainScroll.content = _crewContents;
            _crewContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _crewAnimator.Play("ToggleOff");
            _crewContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 환생 토글
    [Header("< 환생 토글 >")]
    public Toggle _reincarnationToggle; // 메뉴 토글 버튼
    public Animator _reincarnationAnimator; // 토글 버튼 애니메이션
    public RectTransform _reincarnationContents; // 토글 선택시 표시할 콘텐츠

    public void OnReincarnationToggle()
    {
        if (_reincarnationToggle.isOn)
        {
            _reincarnationAnimator.Play("ToggleOn");
            _mainScroll.content = _reincarnationContents;
            _reincarnationContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _reincarnationAnimator.Play("ToggleOff");
            _reincarnationContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 던전 토글
    [Header("< 던전 토글 >")]
    public Toggle _dungeonToggle; // 메뉴 토글 버튼
    public Animator _dungeonAnimator; // 토글 버튼 애니메이션
    public RectTransform _dungeonContents; // 토글 선택시 표시할 콘텐츠

    public void OnDungeonToggle()
    {
        if (_dungeonToggle.isOn)
        {
            _dungeonAnimator.Play("ToggleOn");
            _mainScroll.content = _dungeonContents;
            _dungeonContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _dungeonAnimator.Play("ToggleOff");
            _dungeonContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }


    // 상점 토글
    [Header("< 상점 토글 >")]
    public Toggle _storeToggle; // 메뉴 토글 버튼
    public Animator _storeAnimator; // 토글 버튼 애니메이션
    public RectTransform _storeContents; // 토글 선택시 표시할 콘텐츠

    public void OnStoreToggle()
    {
        if (_storeToggle.isOn)
        {
            _storeAnimator.Play("ToggleOn");
            _mainScroll.content = _storeContents;
            _storeContents.gameObject.SetActive(true);
            Invoke("OnInertia", 0.1f);
        }
        else
        {
            _storeAnimator.Play("ToggleOff");
            _storeContents.gameObject.SetActive(false);
            _mainScroll.inertia = false;
        }
    }
}
