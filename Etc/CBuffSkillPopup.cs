using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CBuffSkillPopup : MonoBehaviour {

    private DOTweenAnimation _buffSkillPopupDoAnim;

    private void Awake()
    {
        _buffSkillPopupDoAnim = GetComponent<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        _buffSkillPopupDoAnim.DORestartById("BuffSkillPopup");
    }
}