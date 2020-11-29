using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

// 용병 초기화, 용병 텍스트 갱신
public class CCrewInfoManager : MonoBehaviour {

    [Header("< 용병 게임 오브젝트 >")]
    public GameObject[] _crewGameObject;

    [Header("< 용병 레벨 >")]
    public ObscuredInt[] _crewLevelArray;

    [Header("< 용병 업그레이드 필요 루비 >")]
    public ObscuredInt[] _crewUpNeedRubyArray;

    [Header("< 용병 업그레이드 루비 증가량 >")]
    public ObscuredInt[] _crewUpRaitoRubyArray;

    [Header("< 용병 기술 기능 >")]
    public ObscuredFloat[] _crewFunction;

    [Header("< 용병 레벨업시 기능 증가량 >")]
    public ObscuredFloat[] _crewIncrementRatio;

    [Header("< 용병 기술 쿨타임 >")]
    public ObscuredFloat[] _crewDelayTime;
    
    [Header("< 용병 레벨업시 쿨타임 감소량 >")]
    public ObscuredFloat[] _crewDecrementTime;

    [Header("< 용병 쿨타임 최소값 >")]
    public ObscuredFloat[] _crewDelayTimeMinimum;

    [Header("< 용병 부모 오브젝트 >")]
    public Transform _crewParent;

    [Header("< 용병 위치 >")]
    public Transform[] _crewPosition;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CCrewManager _crewManager;


    // 용병 레벨에 해당하는 가격, 기능 정보 갱신 후 표시
    public void CrewLevelFunctionRefresh()
    {
        CrewLevelApplyRuby();
        CrewLevelApplyFunctionRatio();
        CrewEnable();
    }

    // 용병이 구매 상태면 필요 루비 갯수로 갱신, 컨디션 체커 제거, 이미지 활성
    public void CrewLevelApplyRuby()
    {
        for (int i = 0; i < _crewLevelArray.Length; i++)
        {
            if (_crewGameObject[i].GetComponentInChildren<CCrewState>()._isBuy)
            {
                _crewUpNeedRubyArray[i] += (_crewLevelArray[i] * _crewUpRaitoRubyArray[i]);
                _crewManager._crewUpNeedRubyText[i].text = _playerInfo._playerState.CommaText(_crewUpNeedRubyArray[i]).ToString();

                Destroy(_crewManager._crewConditionCheckButton[i]);
                _crewManager._crewImage[i].color = _crewManager._crewColor;
            }
        }
    }

    // 용병이 구매 상태면 기능 비율과 이름, 레벨 텍스트 갱신
    public void CrewLevelApplyFunctionRatio()
    {
        for (int i = 0; i < _crewLevelArray.Length; i++)
        {
            if (_crewGameObject[i].GetComponentInChildren<CCrewState>()._isBuy)
            {
                _crewFunction[i] += (_crewLevelArray[i] * _crewIncrementRatio[i]);

                _crewDelayTime[i] -= (_crewLevelArray[i] * _crewDecrementTime[i]);
                if (_crewDelayTime[i] <= _crewDelayTimeMinimum[i]) _crewDelayTime[i] = _crewDelayTimeMinimum[i];
            }

            CrewNameLevelTextRefresh(i);
        }
    }

    // 용병이 구매 상태이고 선택 상태이면 표시
    public void CrewEnable()
    {
        for (int i = 0; i < _crewLevelArray.Length; i++)
        {
            if (_crewGameObject[i].GetComponentInChildren<CCrewState>()._isBuy && _crewGameObject[i].GetComponentInChildren<CCrewState>()._isSelect)
            {
                for (int j = 0; j < _crewPosition.Length; j++)
                {
                    if (!_crewPosition[j].GetComponent<CCrewPositionState>()._isCrewOn && !_crewGameObject[i].activeSelf)
                    {
                        _crewPosition[j].GetComponent<CCrewPositionState>()._isCrewOn = true;
                        _crewGameObject[i].transform.position = _crewPosition[j].position;
                        _crewGameObject[i].transform.parent = _crewPosition[j];
                        _crewGameObject[i].SetActive(true);
                        _crewManager._crewSelectCheckText[i].enabled = true;
                    }
                }
            }
        }
    }


    // 용병 레벨 갱신
    // 00. 용병
    public void Crew00TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("치돌이 + ");
        CStringBuilder._sb.Append(_crewLevelArray[0].ToString());
        _crewManager._crewNameLevelText[0].text = CStringBuilder._sb.ToString();
    }

    public void Crew01TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("레드 쟌 + ");
        CStringBuilder._sb.Append(_crewLevelArray[1].ToString());
        _crewManager._crewNameLevelText[1].text = CStringBuilder._sb.ToString();
    }

    public void Crew02TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("매실 + ");
        CStringBuilder._sb.Append(_crewLevelArray[2].ToString());
        _crewManager._crewNameLevelText[2].text = CStringBuilder._sb.ToString();
    }

    public void Crew03TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(" + ");
        CStringBuilder._sb.Append(_crewLevelArray[3].ToString());
        _crewManager._crewNameLevelText[3].text = CStringBuilder._sb.ToString();
    }

    public void Crew04TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(" + ");
        CStringBuilder._sb.Append(_crewLevelArray[4].ToString());
        _crewManager._crewNameLevelText[4].text = CStringBuilder._sb.ToString();
    }

    public void Crew05TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(" + ");
        CStringBuilder._sb.Append(_crewLevelArray[5].ToString());
        _crewManager._crewNameLevelText[5].text = CStringBuilder._sb.ToString();
    }

    public void Crew06TextRefresh()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append(" + ");
        CStringBuilder._sb.Append(_crewLevelArray[6].ToString());
        _crewManager._crewNameLevelText[6].text = CStringBuilder._sb.ToString();
    }


    // 용병 이름, 레벨 텍스트 갱신
    public void CrewNameLevelTextRefresh(int crewNum)
    {
        switch (crewNum)
        {
            case 0: Crew00TextRefresh(); break;
            case 1: Crew01TextRefresh(); break;
            case 2: Crew02TextRefresh(); break;
            case 3: Crew03TextRefresh(); break;
            case 4: Crew04TextRefresh(); break;
            case 5: Crew05TextRefresh(); break;
            case 6: Crew06TextRefresh(); break;
        }
    }
}
