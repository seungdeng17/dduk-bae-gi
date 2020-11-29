using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 캐릭터 정보창 관리
public class CCharacterStateManager : MonoBehaviour {

    public CPlayerState _playerState;
    public CPlayerInfo _playerInfo;

    // 캐릭터 정보에 표시 할 텍스트
    [Header("< 캐릭터 정보에 표시 할 텍스트 >")]
    public Text _nameTextContent; // 이름
    public Text _levelTextContent; // 레벨
    public Text _ageTextContent; // 나이
    public Text _attackDamageTextContent; // 평균 공격력
    public Text _OriginHpTextContent; // 최대체력
    public Text _defensiveTextContent; // 방어력
    public Text _attackSpeedTextContent; // 공격속도
    public Text _criticalPerTextContent; // 치명타 확률
    public Text _criticalDamageTextContent; // 치명타 데미지
    public Text _addEXPTextContent; // 추가 경험치
    public Text _addCoinTextContent; // 추가 코인


    private void OnEnable()
    {
        // 캐릭터 정보 표시
        _nameTextContent.text = _playerInfo._playerName; // 이름 표시

        _levelTextContent.text = _playerState.CommaText(_playerInfo._playerLevel).ToString(); // 레벨 표시

        _ageTextContent.text = _playerState.CommaText(_playerInfo._playerAge).ToString(); // 나이 표시

        _attackDamageTextContent.text = _playerState.CommaText(_playerState._attackDamage).ToString(); // 평균 공격력 표시

        _OriginHpTextContent.text = _playerState.CommaText2(_playerState._originHp).ToString(); // 최대체력 표시

        _defensiveTextContent.text = _playerState.CommaText2(_playerState._defensive).ToString(); // 방어력 표시

        // 공격속도 표시
        if (_playerState._attackSpeed == _playerInfo._attackSpeedMaximum)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerState.CommaText2(_playerState._attackSpeed).ToString());
            CStringBuilder._sb.Append(" (MAX)");
            _attackSpeedTextContent.text = CStringBuilder._sb.ToString();
        }
        else _attackSpeedTextContent.text = _playerState.CommaText2(_playerState._attackSpeed).ToString();

        // 치명타 확률 표시
        if (_playerState._criticalPer == _playerInfo._criticalPerMaximum)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerState.CommaText2(_playerState._criticalPer).ToString());
            CStringBuilder._sb.Append(" (MAX)");
            _criticalPerTextContent.text = CStringBuilder._sb.ToString();
        }
        else _criticalPerTextContent.text = _playerState.CommaText2((_playerState._criticalPer / 3.0f)).ToString();

        // 치명타 데미지 표시
        if (_playerState._criticalDamage == _playerInfo._criticalDamageMaximum)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerState.CommaText2(_playerState._criticalDamage).ToString());
            CStringBuilder._sb.Append(" (MAX)");
            _criticalDamageTextContent.text = CStringBuilder._sb.ToString();
        }
        else _criticalDamageTextContent.text = _playerState.CommaText2(_playerState._criticalDamage).ToString();

        // 추가 경험치 표시
        if (_playerState._addEXP == _playerInfo._addExpMaximum)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerState.CommaText2(_playerState._addEXP).ToString());
            CStringBuilder._sb.Append(" (MAX)");
            _addEXPTextContent.text = CStringBuilder._sb.ToString();
        }
        else _addEXPTextContent.text = _playerState.CommaText2(_playerState._addEXP).ToString();

        // 추가 코인 표시
        if (_playerState._addCoin == _playerInfo._addCoinMaximum)
        {
            CStringBuilder.StringBuilderRefresh();
            CStringBuilder._sb.Append(_playerState.CommaText2(_playerState._addCoin).ToString());
            CStringBuilder._sb.Append(" (MAX)");
            _addCoinTextContent.text = CStringBuilder._sb.ToString();
        }
        else _addCoinTextContent.text = _playerState.CommaText2(_playerState._addCoin).ToString(); 
    }
}
