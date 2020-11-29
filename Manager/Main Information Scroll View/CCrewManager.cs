using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;

// 용병 이미지 터치 팝업창, 용병 업그레이드 관련
public class CCrewManager : MonoBehaviour {

    [Header("< 용병 이미지 >")]
    public Image[] _crewImage;

    [Header("< 용병 고용 체크 텍스트 >")]
    public Text[] _crewSelectCheckText;

    [Header("< 용병 필요 루비 텍스트 >")]
    public Text[] _crewUpNeedRubyText;

    [Header("< 용병 이름, 레벨 텍스트 >")]
    public Text[] _crewNameLevelText;

    [Header("< 용병 조건 확인 버튼 오브젝트 >")]
    public GameObject[] _crewConditionCheckButton; // 용병 조건 확인 버튼 오브젝트

    [Header("< 용병 팝업창 관련 >")]
    public GameObject _touchCutter;
    public GameObject _crewpopup;
    public Image _crewpopupImage;
    public Text _crewpopupNameLevelText;
    public Text _crewpopupFunctionText;
    public Button _crewInBtn;
    public Button _crewOutBtn;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CCrewInfoManager _crewInfoManager;

    [HideInInspector]
    public Color _crewColor = new Color(1f, 1f, 1f);


    // 레벨업시 필요 루비 증가 메서드
    public ObscuredInt PlayerRubyDown(ObscuredBool isRubyDown, Text needRubyText, ObscuredInt needRuby, ObscuredInt rubyIncrementRatio)
    {
        if (isRubyDown)
        {
            _playerInfo._my_Ruby -= needRuby;
        }
        _playerInfo._my_RubyText.DOText(_playerInfo._playerState.CommaText(_playerInfo._my_Ruby).ToString(), 0.3f, true, ScrambleMode.Numerals, null);

        needRuby += rubyIncrementRatio;
        if (needRuby > 2000000000) needRuby = 2000000000;
        needRubyText.text = _playerInfo._playerState.CommaText(needRuby).ToString();

        return needRuby;
    }


    // 용병 업그레이드
    public void CrewUpgrade(ObscuredInt crewNum)
    {
        // 플레이어가 죽지 않았고 보유 루비가 필요 루비보다 많거나 같을 때
        if (!_playerInfo._playerState._isDie && _playerInfo._my_Ruby >= _crewInfoManager._crewUpNeedRubyArray[crewNum])
        {
            // 레벨업
            _crewInfoManager._crewLevelArray[crewNum] += 1;

            // 루비 차감
            _crewInfoManager._crewUpNeedRubyArray[crewNum] =
                PlayerRubyDown(true, _crewUpNeedRubyText[crewNum], _crewInfoManager._crewUpNeedRubyArray[crewNum], _crewInfoManager._crewUpRaitoRubyArray[crewNum]);

            // 기능 반영
            _crewInfoManager._crewFunction[crewNum] += _crewInfoManager._crewIncrementRatio[crewNum];
            _crewInfoManager._crewDelayTime[crewNum] -= _crewInfoManager._crewDecrementTime[crewNum];
            if (_crewInfoManager._crewDelayTime[crewNum] <= _crewInfoManager._crewDelayTimeMinimum[crewNum]) _crewInfoManager._crewDelayTime[crewNum] = _crewInfoManager._crewDelayTimeMinimum[crewNum];
        }
    }


    // 용병 이미지 터치시 팝업창에 정보 전달
    public void CrewInfoDelivery(Image image, Text nameLevel, string function)
    {
        _crewpopupImage.sprite = image.sprite;
        _crewpopupNameLevelText.text = nameLevel.text;
        _crewpopupFunctionText.text = function;

        _touchCutter.SetActive(true);
        _crewpopup.SetActive(true);

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.gameObject.SetActive(true);

        _crewOutBtn.onClick.RemoveAllListeners();
        _crewOutBtn.gameObject.SetActive(true);
    }

    // 용병 고용 버튼 이벤트
    private void OnCrewInBtnClick(ObscuredInt CrewNum)
    {
        //_crewInBtn.onClick.RemoveListener(() => { OnCrewInBtnClick(CrewNum); });
        for (int i = 0; i < _crewInfoManager._crewPosition.Length; i++)
        {
            if (!_playerInfo._playerState._isDie && !_crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<CCrewState>()._isSelect && _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<CCrewState>()._isBuy && !_crewInfoManager._crewPosition[i].GetComponent<CCrewPositionState>()._isCrewOn)
            {
                _crewInfoManager._crewPosition[i].GetComponent<CCrewPositionState>()._isCrewOn = true;
                _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<CCrewState>()._isSelect = true;

                _crewInfoManager._crewGameObject[CrewNum].transform.parent = _crewInfoManager._crewPosition[i];

                switch (i)
                {
                    case 0: _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<SpriteRenderer>().sortingOrder = 0; break;
                    case 1: _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<SpriteRenderer>().sortingOrder = 1; break;
                }

                _crewInfoManager._crewGameObject[CrewNum].transform.localPosition = Vector3.zero;
                _crewInfoManager._crewGameObject[CrewNum].SetActive(true);
                _crewSelectCheckText[CrewNum].enabled = true;
                break;
            }
        }
    }

    // 용병 휴식 버튼 이벤트
    private void OnCrewOutBtnClick(ObscuredInt CrewNum)
    {
        //_crewOutBtn.onClick.RemoveListener(() => { OnCrewOutBtnClick(CrewNum); });
        if (!_playerInfo._playerState._isDie && _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<CCrewState>()._isSelect)
        {
            _crewInfoManager._crewGameObject[CrewNum].GetComponentInParent<CCrewPositionState>()._isCrewOn = false;
            _crewInfoManager._crewGameObject[CrewNum].GetComponentInChildren<CCrewState>()._isSelect = false;

            _crewInfoManager._crewGameObject[CrewNum].transform.parent = _crewInfoManager._crewParent;

            _crewInfoManager._crewGameObject[CrewNum].SetActive(false);
            _crewSelectCheckText[CrewNum].enabled = false;
        }
    }


    // 용병 구매하지 않았을 때 말풍선
    public void OnClickCrewConditionCheck()
    {
        // 말풍선
        _playerInfo._speechBubbleManager.SpawnSpeechBubble(_playerInfo._speechBubbleManager._crewNotEnoughCondition);
    }


    // 00. 용병 이미치 터치
    public void OnCrew00ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 번개를 내려쳐 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[0].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 광역 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[0].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[0], _crewNameLevelText[0], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(0); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(0); });
    }

    // 01. 용병 이미치 터치
    public void OnCrew01ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 폭발을 일으켜 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[1].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 광역 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[1].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[1], _crewNameLevelText[1], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(1); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(1); });
    }

    // 02. 용병 이미치 터치
    public void OnCrew02ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 독화살을 날려 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[2].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 지속 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[2].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[2], _crewNameLevelText[2], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(2); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(2); });
    }

    // 03. 용병 이미치 터치
    public void OnCrew03ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 힘세고 강한 펀치로 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[3].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 연타 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[3].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[3], _crewNameLevelText[3], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(3); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(3); });
    }

    // 04. 용병 이미치 터치
    public void OnCrew04ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 빛의 수리검을 던져 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[4].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 관통 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[4].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[4], _crewNameLevelText[4], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(4); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(4); });
    }

    // 05. 용병 이미치 터치
    public void OnCrew05ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 낙엽을 흩날려 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[5].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 지속 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[5].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[5], _crewNameLevelText[5], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(5); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(5); });
    }

    // 06. 용병 이미치 터치
    public void OnCrew06ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("<size=25># 응축된 검은 기운을 폭발시켜 평균 공격력의 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewFunction[6].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 만큼 광역 피해를 줍니다.\n\n# 대기시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_crewInfoManager._crewDelayTime[6].ToString("N1"));
        CStringBuilder._sb.Append("</color>초</size>");

        CrewInfoDelivery(_crewImage[6], _crewNameLevelText[6], CStringBuilder._sb.ToString());

        _crewInBtn.onClick.RemoveAllListeners();
        _crewInBtn.onClick.AddListener(() => { OnCrewInBtnClick(6); });
        _crewOutBtn.onClick.AddListener(() => { OnCrewOutBtnClick(6); });
    }


    // 업그레이드 버튼 클릭
    // 00. 용병
    public void OnUpBtnClickCrew00()
    {
        CrewUpgrade(0);
        _crewInfoManager.Crew00TextRefresh();
    }

    // 01. 용병
    public void OnUpBtnClickCrew01()
    {
        CrewUpgrade(1);
        _crewInfoManager.Crew01TextRefresh();
    }

    // 02. 용병
    public void OnUpBtnClickCrew02()
    {
        CrewUpgrade(2);
        _crewInfoManager.Crew02TextRefresh();
    }

    // 03. 용병
    public void OnUpBtnClickCrew03()
    {
        CrewUpgrade(3);
        _crewInfoManager.Crew03TextRefresh();
    }

    // 04. 용병
    public void OnUpBtnClickCrew04()
    {
        CrewUpgrade(4);
        _crewInfoManager.Crew04TextRefresh();
    }

    // 05. 용병
    public void OnUpBtnClickCrew05()
    {
        CrewUpgrade(5);
        _crewInfoManager.Crew05TextRefresh();
    }

    // 06. 용병
    public void OnUpBtnClickCrew06()
    {
        CrewUpgrade(6);
        _crewInfoManager.Crew06TextRefresh();
    }
}
