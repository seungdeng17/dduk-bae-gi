using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CBuffIconManager : MonoBehaviour {

    public Text _buffText;
    public Animator _buffTextAnimator;

    public void OnAttackDamageBuffClick()
    {
        _buffText.text = "공격력 증가";
        _buffText.enabled = true;
        _buffTextAnimator.SetTrigger("Press");
    }

    public void OnAttackSpeedBuffClick()
    {
        _buffText.text = " 공격속도 증가";
        _buffText.enabled = true;
        _buffTextAnimator.SetTrigger("Press");
    }
}
