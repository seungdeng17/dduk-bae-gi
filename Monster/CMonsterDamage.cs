using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ez.Pooly;
//using TMPro;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;


public class CMonsterDamage : CCharacterDamage
{
    public Transform _hitPoint;  // 타격 위치
    private Vector2 _damageTextPos = Vector2.zero;
    private Vector3 _damageTextRot = Vector3.zero;
    private float _textPosX = 0f;

    private Text d_text;
    private Text c_text;

    private Transform d_damageText;
    private Transform c_damageText;


    // 플레이어 일반 데미지
    public override void Damage(ObscuredFloat damage, string hitEffectName = null)
    {
        if (_characterState._hp > 0)
        {
            damage -= _characterState._defensive;
            if (damage <= 0) damage = 1f;

            _characterState.HpDown(damage);
            _animator.Play("Damage", _animator.GetLayerIndex("Damage Layer"));
        }

        // 데미지 텍스트의 위치와 각도를 세팅
        _damageTextRot = transform.localRotation.eulerAngles;
        _textPosX = Random.Range(-0.1f, 0.2f);

        if (_textPosX >= 0.1)
        {
            _damageTextRot.z = Random.Range(-10f, -5f);
        }
        else if (_textPosX <= 0)
        {
            _damageTextRot.z = Random.Range(5f, 10f);
        }

        _damageTextPos = new Vector2(transform.position.x + _textPosX, transform.position.y + Random.Range(0.3f, 0.4f));

        // 타격 이펙트 스폰
        if (!string.IsNullOrEmpty(hitEffectName))
        {
            Pooly.Spawn(hitEffectName, _hitPoint.position, Quaternion.identity);
        }

        // 데미지 텍스트 스폰
        d_damageText = Pooly.Spawn("DamageTextUICanvas", _damageTextPos, _damageTextRot, gameObject.transform);
        //TMP_Text _mtext = _damageText.GetComponentInChildren<TMP_Text>();
        //_mtext.text = CommaText(damage).ToString();
        d_text = d_damageText.GetComponentInChildren<Text>();
        d_text.text = _characterState.CommaText(damage).ToString();

        d_damageText.GetComponentInChildren<Animator>().Play("DamageText");
    }


    // 플레이어 크리티컬 데미지
    public override void CriticalDamage(ObscuredFloat c_damage, string hitEffectName = null)
    {
        if (_characterState._hp > 0)
        {
            c_damage -= _characterState._defensive;
            if (c_damage <= 0) c_damage = 2f;

            _characterState.HpDown(c_damage);
            _animator.Play("Damage", _animator.GetLayerIndex("Damage Layer"));
        }

        // 데미지 텍스트의 위치와 각도를 세팅
        _damageTextRot = transform.localRotation.eulerAngles;
        _textPosX = Random.Range(-0.1f, 0.2f);

        if (_textPosX >= 0.1)
        {
            _damageTextRot.z = Random.Range(-10f, -5f);
        }
        else if (_textPosX <= 0)
        {
            _damageTextRot.z = Random.Range(5f, 10f);
        }

        _damageTextPos = new Vector2(transform.position.x + _textPosX, transform.position.y + Random.Range(0.3f, 0.4f));

        // 타격 이펙트 스폰
        if (!string.IsNullOrEmpty(hitEffectName))
        {
            Pooly.Spawn(hitEffectName, _hitPoint.position, Quaternion.identity);
        }

        // 데미지 텍스트 스폰
        c_damageText = Pooly.Spawn("CriticalDamageUICanvas", _damageTextPos, _damageTextRot, gameObject.transform);
        //TMP_Text _mtext = _damageText.GetComponentInChildren<TMP_Text>();
        //_mtext.text = CommaText(c_damage).ToString();
        c_text = c_damageText.GetComponentInChildren<Text>();
        c_text.text = _characterState.CommaText(c_damage).ToString();

        c_damageText.GetComponentInChildren<Animator>().Play("CriticalDamageText");
    }


    // 용병 데미지
    public void CrewDamage(ObscuredFloat crewFunction_value, Color crewFunction_color, string hitEffectName = null)
    {
        if (_characterState._hp > 0)
        {
            crewFunction_value -= _characterState._defensive;
            if (crewFunction_value <= 0) crewFunction_value = 1f;

            _characterState.HpDown(crewFunction_value);
            _animator.Play("Damage", _animator.GetLayerIndex("Damage Layer"));
        }

        // 데미지 텍스트의 위치와 각도를 세팅
        _damageTextRot = transform.localRotation.eulerAngles;
        _textPosX = Random.Range(-0.1f, 0.1f);

        if (_textPosX >= 0.05)
        {
            _damageTextRot.z = Random.Range(-10f, -5f);
        }
        else if (_textPosX <= -0.05)
        {
            _damageTextRot.z = Random.Range(5f, 10f);
        }

        _damageTextPos = new Vector2(transform.position.x + _textPosX, transform.position.y + Random.Range(0.3f, 0.4f));

        // 타격 이펙트 스폰
        if (!string.IsNullOrEmpty(hitEffectName))
        {
            Pooly.Spawn(hitEffectName, _hitPoint.position, Quaternion.identity);
        }

        // 데미지 텍스트 스폰
        d_damageText = Pooly.Spawn("CrewFunctionDamageTextUICanvas", _damageTextPos, _damageTextRot, gameObject.transform);
        //TMP_Text _mtext = _damageText.GetComponentInChildren<TMP_Text>();
        //_mtext.text = CommaText(damage).ToString();
        d_text = d_damageText.GetComponentInChildren<Text>();
        d_text.color = crewFunction_color;
        d_text.text = _characterState.CommaText(crewFunction_value).ToString();

        d_damageText.GetComponentInChildren<Animator>().Play("DamageText");
    }
}
