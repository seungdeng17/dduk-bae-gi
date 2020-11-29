using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;

public class CBackGroundDataManager : MonoBehaviour {

    [Header("< 백그라운드 애니메이션 컨트롤러 >")]
    public RuntimeAnimatorController[] _backGround_AnimCtrlArray;

    [Header("< 백그라운드 스프라이트 >")]
    public Sprite[] _backGround_SpriteArray;

    [Header("< 백그라운드 스프라이트 사이즈 X >")]
    public float[] _backGround_SpriteSizeX;

    [Header("< 백그라운드 스프라이트 사이즈 Y >")]
    public float[] _backGround_SpriteSizeY;

    [Header("< 백그라운드 >")]
    public Transform _backGround;
    private int _chiledCount;

    [Header("< 백그라운드 애니메이터 >")]
    public Animator _backGround_Animator;

    [Header("< 백그라운드 스프라이트 랜더러 >")]
    public SpriteRenderer _backGround_SpriteRenderer;

    [Header("< 페이드인아웃 게임오브젝트 >")]
    public GameObject _fadeInOut;

    [Header("< 현재 백그라운드 >")]
    public int _backGroundNum;


    private void Awake()
    {
        BackGroundDataChange(Random.Range(0, _backGround_AnimCtrlArray.Length), true);
    }

    // 백그라운드 애니메이션 컨트롤러와 스프라이트, 사이즈를 바꾸는 함수
    public void BackGroundDataChange(int backGroundNum, bool isDirectChange)
    {
        if (isDirectChange)
        {
            BackGroundSetting(backGroundNum, false);
            _backGroundNum = backGroundNum;
        }
        else
        {
            if (_backGroundNum != backGroundNum)
            {
                Invoke("FadeInOutOn", 0.55f);
                _backGroundNum = backGroundNum;
                StartCoroutine(WaitChange(backGroundNum, true));
            }
            else
            {
                StartCoroutine(WaitChange(backGroundNum, false));
            }
        }

        Invoke("FadeInOutOff", 2f);
    }


    private WaitForSeconds _backGroundChageTime = new WaitForSeconds(0.95f);
    private IEnumerator WaitChange(int backGroundNum, bool isNoChiled)
    {
        yield return _backGroundChageTime;

        BackGroundSetting(backGroundNum, isNoChiled);
    }


    private void BackGroundSetting(int backGroundNum, bool isNoChiled)
    {
        if (isNoChiled)
        {
            _chiledCount = _backGround.childCount;
            for (int i = 0; i < _chiledCount; i++)
            {
                if (_backGround.GetChild(0) != null)
                {
                    Pooly.Despawn(_backGround.GetChild(0));
                }
            }
        }

        _backGround_Animator.runtimeAnimatorController = _backGround_AnimCtrlArray[backGroundNum];

        _backGround_SpriteRenderer.sprite = _backGround_SpriteArray[backGroundNum];
        _backGround_SpriteRenderer.size = new Vector2(_backGround_SpriteSizeX[backGroundNum], _backGround_SpriteSizeY[backGroundNum]);
    }


    private void FadeInOutOn()
    {
        _fadeInOut.SetActive(true);
    }

    private void FadeInOutOff()
    {
        _fadeInOut.SetActive(false);
    }
}
