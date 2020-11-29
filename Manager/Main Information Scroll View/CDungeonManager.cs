using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDungeonManager : MonoBehaviour {

    [Header("< 보통맛 던전 오브젝트 >")]
    public CBossMonsterManager _nomal_BossMonsterManager;
    public CMonsterManager _nomal_MonsterManager;
    public CStageManager _nomal_StageManager;

    [Header("< 매운맛 던전 오브젝트 >")]
    public CHardBossMonsterManager _hard_HardBossMonsterManager;
    public CHardStageManager _hard_StageManager;

    [Header("< 던전 팝업창 >")]
    public GameObject _touchCutter;
    public GameObject _dungeonpopup;
    public GameObject _goToNomalStageButton;
    public GameObject _goToHardStageButton;

    [Header("< 사냥중 텍스트 >")]
    public Text _nomalDungeonCheckText;
    public Text _hardDungeonCheckText;

    [Header("< 던전 이동 중 필요한 오브젝트 >")]
    public GameObject _noneButtonTouchCutter;
    public GameObject _dungeonChangeFadeInOut;
    public Animator _bossMonsterHpBarAnimator;
    public Text _bossMonsterHpText;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CPlayerState _playerState;
    public CBackGroundDataManager _backGroundDataManager;

    [Header("< 추가 기타 >")]
    public Animator _playerAnimator;

    // 몬스터 디스폰 델리게이트
    public delegate void MonsterDespawnHandler_DungeonChange();
    public static event MonsterDespawnHandler_DungeonChange OnMonsterDespawn_DungeonChange;


    // 보통맛 던전 입장 버튼 클릭
    public void OnNomalDungeonSelectClick()
    {
        if (!_playerState._isDie && _playerState._isHardDungeon)
        {
            _touchCutter.SetActive(true);
            _dungeonpopup.SetActive(true);
            _goToNomalStageButton.SetActive(true);
        }
    }

    // 매운맛 던전 입장 버튼 클릭
    public void OnHardDungeonSelectClick()
    {
        if (!_playerState._isDie && !_playerState._isHardDungeon)
        {
            _touchCutter.SetActive(true);
            _dungeonpopup.SetActive(true);
            _goToHardStageButton.SetActive(true);
        }
    }

    // 던전 이동 확인 팝업창 취소 버튼 클릭
    public void OnCancelButtonClick()
    {
        if (!_playerState._isDie)
        {
            _touchCutter.SetActive(false);
            _dungeonpopup.SetActive(false);
            _goToNomalStageButton.SetActive(false);
            _goToHardStageButton.SetActive(false);
        }
    }


    // 보통맛 던전 입장 확인 버튼 클릭
    public void OnGoToNomalDungeonClick()
    {
        if (!_playerState._isDie && _playerState._isHardDungeon)
        {
            _playerState._isHardDungeon = false;
            _touchCutter.SetActive(false);
            _dungeonpopup.SetActive(false);
            _goToNomalStageButton.SetActive(false);
            _hardDungeonCheckText.enabled = false;
            _nomalDungeonCheckText.enabled = true;

            // 스폰 된 모든 몬스터 디스폰
            if (_nomal_BossMonsterManager._bossMonsterNum > 0 || _nomal_MonsterManager._monsterNum > 1 || _hard_HardBossMonsterManager._hardBossMonsterNum > 0)
            {
                OnMonsterDespawn_DungeonChange();
            }

            _hard_HardBossMonsterManager.gameObject.SetActive(false);
            _hard_StageManager.gameObject.SetActive(false);

            StartCoroutine(NomalDungeonChangeCoroutine());
        }
    }

    private IEnumerator NomalDungeonChangeCoroutine()
    {
        _noneButtonTouchCutter.SetActive(true);
        _dungeonChangeFadeInOut.GetComponent<Animator>().speed = 0.5f;
        _dungeonChangeFadeInOut.SetActive(true);

        yield return new WaitForSeconds(1f);
        _playerAnimator.SetBool("IsTargetOn", false);

        // 보스 체력바, 체력 텍스트 초기화
        _bossMonsterHpBarAnimator.Play("BossMonsterHpBarZero");
        _bossMonsterHpText.text = "";

        // 스테이지 텍스트 갱신
        _backGroundDataManager.BackGroundDataChange(Random.Range(0, _backGroundDataManager._backGround_AnimCtrlArray.Length), true);
        _nomal_StageManager.gameObject.SetActive(true);
        _nomal_StageManager.StageTextRefresh();
        yield return new WaitForSeconds(1f);

        _noneButtonTouchCutter.SetActive(false);
        _dungeonChangeFadeInOut.GetComponent<Animator>().speed = 1f;
        _dungeonChangeFadeInOut.SetActive(false);

        // 몬스터 스포너 활성
        _nomal_BossMonsterManager.gameObject.SetActive(true);
        _nomal_MonsterManager.gameObject.SetActive(true);
    }


    // 매운맛 던전 입장 확인 버튼 클릭
    public void OnGoToHardDungeonClick()
    {
        if (!_playerState._isDie && !_playerState._isHardDungeon)
        {
            _playerState._isHardDungeon = true;
            _touchCutter.SetActive(false);
            _dungeonpopup.SetActive(false);
            _goToHardStageButton.SetActive(false);
            _nomalDungeonCheckText.enabled = false;
            _hardDungeonCheckText.enabled = true;

            // 스폰 된 모든 몬스터 디스폰
            if (_nomal_BossMonsterManager._bossMonsterNum > 0 || _nomal_MonsterManager._monsterNum > 1 || _hard_HardBossMonsterManager._hardBossMonsterNum > 0)
            {
                OnMonsterDespawn_DungeonChange();
            }

            _nomal_BossMonsterManager.gameObject.SetActive(false);
            _nomal_MonsterManager.gameObject.SetActive(false);
            _nomal_StageManager.gameObject.SetActive(false);

            StartCoroutine(HardDungeonChangeCoroutine());
        }
    }

    private IEnumerator HardDungeonChangeCoroutine()
    {
        _noneButtonTouchCutter.SetActive(true);
        _dungeonChangeFadeInOut.GetComponent<Animator>().speed = 0.5f;
        _dungeonChangeFadeInOut.SetActive(true);

        yield return new WaitForSeconds(1f);
        _playerAnimator.SetBool("IsTargetOn", false);

        // 보스 체력바, 체력 텍스트 초기화
        _bossMonsterHpBarAnimator.Play("BossMonsterHpBarZero");
        _bossMonsterHpText.text = "";

        // 스테이지 텍스트 갱신
        _backGroundDataManager.BackGroundDataChange(Random.Range(0, _backGroundDataManager._backGround_AnimCtrlArray.Length), true);
        _hard_StageManager.gameObject.SetActive(true);
        _hard_StageManager.HardStageTextRefresh();
        yield return new WaitForSeconds(1f);

        _noneButtonTouchCutter.SetActive(false);
        _dungeonChangeFadeInOut.GetComponent<Animator>().speed = 1f;
        _dungeonChangeFadeInOut.SetActive(false);

        // 몬스터 스포너 활성
        _hard_HardBossMonsterManager.gameObject.SetActive(true);
    }
}
