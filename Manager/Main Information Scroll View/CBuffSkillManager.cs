using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using DG.Tweening;

// 버프 스킬 이미지 터치 팝업창, 버프 스킬 업그레이드 관련 
public class CBuffSkillManager : MonoBehaviour {

    [Header("< 버프 스킬 이미지 >")]
    public Image[] _buffSkillImage;

    [Header("< 버프 스킬 필요 루비 텍스트 >")]
    public Text[] _buffSkillUpNeedRubyText;

    [Header("< 버프 스킬 이름, 레벨 텍스트 >")]
    public Text[] _buffSkillNameLevelText;

    [Header("< 버프 스킬 팝업창 관련 >")]
    public GameObject _touchCutter;
    public GameObject _buffSkillpopup;
    public Image _buffSkillpopupImage;
    public Text _buffSkillpopupNameLevelText;
    public Text _buffSkillpopupFunctionText;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CBuffSkillInfoManager _buffSkillInfoManager;


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


    // 버프 스킬 업그레이드
    public void BuffSkillUpgrade(ObscuredInt buffSkillNum, ObscuredFloat incrementBuffTime)
    {
        // 플레이어가 죽지 않았고 보유 루비가 필요 루비보다 많거나 같을 때
        if (!_playerInfo._playerState._isDie && _playerInfo._my_Ruby >= _buffSkillInfoManager._buffSkillUpNeedRubyArray[buffSkillNum])
        {
            // 레벨업
            _buffSkillInfoManager._buffSkillLevelArray[buffSkillNum] += 1;

            // 루비 차감
            _buffSkillInfoManager._buffSkillUpNeedRubyArray[buffSkillNum] =
                PlayerRubyDown(true, _buffSkillUpNeedRubyText[buffSkillNum], _buffSkillInfoManager._buffSkillUpNeedRubyArray[buffSkillNum], _buffSkillInfoManager._buffSkillUpRaitoRubyArray[buffSkillNum]);

            // 기능 반영
            _buffSkillInfoManager._buffSkillFunction[buffSkillNum] += _buffSkillInfoManager._buffSkillIncrementRatio[buffSkillNum];
            if (_buffSkillInfoManager._buffSkillTime[buffSkillNum] != 0f)
            {
                _buffSkillInfoManager._buffSkillTime[buffSkillNum] += incrementBuffTime;
            }
        }
    }


    // 스킬 이미지 터치시 팝업창에 정보 전달
    public void BuffSkillInfoDelivery(Image image, Text nameLevel, string function)
    {
        _buffSkillpopupImage.sprite = image.sprite;
        _buffSkillpopupNameLevelText.text = nameLevel.text;
        _buffSkillpopupFunctionText.text = function;

        _touchCutter.SetActive(true);
        _buffSkillpopup.SetActive(true);
    }

    // 00. 버프 스킬 이미치 터치
    public void OnBuff00ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 공격력 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[0].ToString("N1"));
        CStringBuilder._sb.Append("</color>배\n\n# 지속 시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillTime[0].ToString("N1"));
        CStringBuilder._sb.Append("</color>초");

        BuffSkillInfoDelivery(_buffSkillImage[0], _buffSkillNameLevelText[0], CStringBuilder._sb.ToString());
    }

    // 01. 버프 스킬 이미치 터치
    public void OnBuff01ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 공격속도 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[1].ToString("N1"));
        CStringBuilder._sb.Append("</color>배\n\n# 지속 시간 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillTime[1].ToString("N1"));
        CStringBuilder._sb.Append("</color>초");

        BuffSkillInfoDelivery(_buffSkillImage[1], _buffSkillNameLevelText[1], CStringBuilder._sb.ToString());
    }

    // 02. 버프 스킬 이미치 터치
    public void OnBuff02ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# CON 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[2].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[2], _buffSkillNameLevelText[2], CStringBuilder._sb.ToString());
    }

    // 03. 버프 스킬 이미치 터치
    public void OnBuff03ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# DEF 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[3].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[3], _buffSkillNameLevelText[3], CStringBuilder._sb.ToString());
    }

    // 04. 버프 스킬 이미치 터치
    public void OnBuff04ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# CHC 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[4].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[4], _buffSkillNameLevelText[4], CStringBuilder._sb.ToString());
    }

    // 05. 버프 스킬 이미치 터치
    public void OnBuff05ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# CHD 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[5].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[5], _buffSkillNameLevelText[5], CStringBuilder._sb.ToString());
    }

    // 06. 버프 스킬 이미치 터치
    public void OnBuff06ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# EXP 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[6].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[6], _buffSkillNameLevelText[6], CStringBuilder._sb.ToString());
    }

    // 07. 버프 스킬 이미치 터치
    public void OnBuff07ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# COIN 증가 <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[7].ToString("N1"));
        CStringBuilder._sb.Append("</color>%\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[7], _buffSkillNameLevelText[7], CStringBuilder._sb.ToString());
    }

    // 08. 버프 스킬 이미치 터치
    public void OnBuff08ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 치명타 데미지의\n   <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[8].ToString("N1"));
        CStringBuilder._sb.Append("</color>% 를 흡수");
        CStringBuilder._sb.Append("\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[8], _buffSkillNameLevelText[8], CStringBuilder._sb.ToString());
    }

    // 09. 버프 스킬 이미치 터치
    public void OnBuff09ImageClick()
    {
        CStringBuilder.StringBuilderRefresh();
        CStringBuilder._sb.Append("# 공격시 근처 몬스터\n   <color=#00F3FFFF>");
        CStringBuilder._sb.Append(_buffSkillInfoManager._buffSkillFunction[9].ToString());
        CStringBuilder._sb.Append("</color>마리 추가 타격");
        CStringBuilder._sb.Append("\n\n# 패시브 기술");

        BuffSkillInfoDelivery(_buffSkillImage[9], _buffSkillNameLevelText[9], CStringBuilder._sb.ToString());
    }


    // 업그레이드 버튼 클릭
    // 00. 버프 스킬
    public void OnUpBtnClickBuff00()
    {
        BuffSkillUpgrade(0, _buffSkillInfoManager._buffSkillIncrementTime[0]);
        _playerInfo.AttackDamageRefresh();
        _buffSkillInfoManager.BuffSkill00TextRefresh();
    }

    // 01. 버프 스킬
    public void OnUpBtnClickBuff01()
    {
        BuffSkillUpgrade(1, _buffSkillInfoManager._buffSkillIncrementTime[1]);
        _playerInfo.AttackSpeedRefresh();
        _buffSkillInfoManager.BuffSkill01TextRefresh();
    }

    // 02. 버프 스킬
    public void OnUpBtnClickBuff02()
    {
        BuffSkillUpgrade(2, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill02On();
        _buffSkillInfoManager.BuffSkill02TextRefresh();
    }

    // 03. 버프 스킬
    public void OnUpBtnClickBuff03()
    {
        BuffSkillUpgrade(3, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill03On();
        _buffSkillInfoManager.BuffSkill03TextRefresh();
    }

    // 04. 버프 스킬
    public void OnUpBtnClickBuff04()
    {
        BuffSkillUpgrade(4, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill04On();
        _buffSkillInfoManager.BuffSkill04TextRefresh();
    }

    // 05. 버프 스킬
    public void OnUpBtnClickBuff05()
    {
        BuffSkillUpgrade(5, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill05On();
        _buffSkillInfoManager.BuffSkill05TextRefresh();
    }

    // 06. 버프 스킬
    public void OnUpBtnClickBuff06()
    {
        BuffSkillUpgrade(6, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill06On();
        _buffSkillInfoManager.BuffSkill06TextRefresh();
    }

    // 07. 버프 스킬
    public void OnUpBtnClickBuff07()
    {
        BuffSkillUpgrade(7, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill07On();
        _buffSkillInfoManager.BuffSkill07TextRefresh();
    }

    // 08. 버프 스킬
    public void OnUpBtnClickBuff08()
    {
        BuffSkillUpgrade(8, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill08On();
        _buffSkillInfoManager.BuffSkill08TextRefresh();
    }

    // 09. 버프 스킬
    public void OnUpBtnClickBuff09()
    {
        BuffSkillUpgrade(9, 0f); // BuffSkillUpgrade(버프 스킬 번호, 증가 할 시간(패시브 해당 x));
        _buffSkillInfoManager.BuffSkill09On();
        _buffSkillInfoManager.BuffSkill09TextRefresh();
    }
}
