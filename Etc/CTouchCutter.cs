using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTouchCutter : MonoBehaviour {

    [Header("< 지속기술, 용병정보 팝업창 >")]
    public GameObject _popup; // 지속기술, 용병정보 팝업창

    [Header("< 용병 고용 휴식 버튼 >")]
    public GameObject _crewInBtn; // 용병 고용 버튼
    public GameObject _crewOutBtn; // 용병 휴식 버튼

    [Header("< 환생 확인 팝업창 >")]
    public GameObject _reincarnationpopup; // 환생 팝업창

    [Header("< 던전 이동 확인 팝업창 >")]
    public GameObject _dungeonpopup; // 던전 팝업창
    public GameObject _goToNomalStageButton;
    public GameObject _goToHardStageButton;


    // 터치커터 클릭시
    public void OnTouchCutterClick()
    {

        if (_popup.activeSelf) // 지속기술, 용병정보 팝업창
        {
            _popup.SetActive(false);

            if (_crewInBtn.activeSelf)
            {
                _crewInBtn.SetActive(false); // 용병 고용 버튼
                _crewOutBtn.SetActive(false); // 용병 휴식 버튼
            }
        }
        else if (_reincarnationpopup.activeSelf) // 환생 팝업창
        {
            _reincarnationpopup.SetActive(false);
        }
        else if (_dungeonpopup.activeSelf) // 던전 팝업창
        {
            _dungeonpopup.SetActive(false);
            _goToNomalStageButton.SetActive(false);
            _goToHardStageButton.SetActive(false);
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
