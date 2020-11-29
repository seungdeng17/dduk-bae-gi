using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Ez.Pooly;
using CodeStage.AntiCheat.ObscuredTypes;


public class CCoin : MonoBehaviour {

    private Animator _animator;

    [Header("< 코인 정보 >")]
    public ObscuredFloat _coinValueMin;
    public ObscuredFloat _originCoinValueMin;
    public ObscuredFloat _coinValueMaxRatio;

    [Header("< 코인 정보 갱신시 필요 오브젝트 >")]
    public CStageManager _stageManager;
    public CHardStageManager _hardStageManager;
    public GameObject _nomal_Stage;
    public GameObject _hard_Stage;

    [Header("< 참조 스크립트 >")]
    public CPlayerInfo _playerInfo;
    public CPlayerState _playerState;

    [Header("< 추가 기타 >")]
    public DOTweenAnimation _coinUIDoAnim;

    //[HideInInspector]
    public ObscuredBool _isInit = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void OnSpawned()
    {
        if (_nomal_Stage == null || _hard_Stage == null) // 필요한 컴포넌트를 한 번만 캐싱
        {
            _stageManager = GameObject.FindGameObjectWithTag("NomalStageParent").transform.GetChild(2).GetComponent<CStageManager>();
            _hardStageManager = GameObject.FindGameObjectWithTag("HardStageParent").transform.GetChild(1).GetComponent<CHardStageManager>();

            _nomal_Stage = GameObject.FindGameObjectWithTag("NomalStageParent").transform.GetChild(0).gameObject;
            _hard_Stage = GameObject.FindGameObjectWithTag("HardStageParent").transform.GetChild(0).gameObject;

            _coinUIDoAnim = GameObject.FindGameObjectWithTag("CoinUI").GetComponent<DOTweenAnimation>();
            _playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<CPlayerInfo>();
            _playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<CPlayerState>();
        }

        if (_nomal_Stage.activeSelf)
        {
            _coinValueMin = _stageManager._coinValueMin;
            _coinValueMaxRatio = _stageManager._coinValueMaxRatio;
            _originCoinValueMin = _coinValueMin;

            _stageManager.StageBalanceMoney(this, _stageManager._stage - 1f);
        }
        else if (_hard_Stage.activeSelf)
        {
            _coinValueMin = _hardStageManager._coinValueMin;
            _coinValueMaxRatio = _hardStageManager._coinValueMaxRatio;
            _originCoinValueMin = _coinValueMin;

            _hardStageManager.HardStageBalanceMoney(this, _hardStageManager._hardStage - 1f);
        }

        if (_isInit) StartCoroutine(DoMoveCoroutine());
    }

    private WaitForSeconds _coinWaitTime1 = new WaitForSeconds(0.7f);
    private WaitForSeconds _coinWaitTime2 = new WaitForSeconds(0.35f);
    private WaitForSeconds _coinWaitTime3 = new WaitForSeconds(1.35f);
    private IEnumerator DoMoveCoroutine()
    {
        // 튕김
        transform.DOMove(new Vector2(transform.position.x + Random.Range(-1.75f, 2f), Random.Range(1f, 2.25f)), 0.6f);
        yield return _coinWaitTime1;

        // 플레이어에게로
        transform.DOMove(new Vector2(1f, -0.25f), 0.35f);
        yield return _coinWaitTime2;
        
        // 위쪽 UI로
        _animator.SetTrigger("GoToDestination");
        transform.DOMove(_playerState._effectPositionManager._coinDestinationPosition, 1.35f);
         yield return _coinWaitTime3;

        // 코인UI 애니메이션 재생
        _coinUIDoAnim.DORestart();

        // 플레이어 코인를 올려줌
        _playerInfo._my_Coin += (int)RandomCoin();
        if (_playerInfo._my_Coin > 9200000000000000000) _playerInfo._my_Coin = 9200000000000000000;
        _playerInfo._my_CoinText.DOText(_playerState.CommaText(_playerInfo._my_Coin).ToString(), 0.3f, true, ScrambleMode.Numerals, null);

        // 디스폰
        Pooly.Despawn(this.gameObject.transform);
    }


    private void OnDespawned()
    {
        _coinValueMin = _originCoinValueMin; // 코인 벨류 초기화
    }


    private ObscuredFloat RandomCoin()
    {
        ObscuredFloat _randomCoin = Random.Range(_coinValueMin, _coinValueMin * _coinValueMaxRatio);
        _randomCoin *= _playerState._addCoin;
        return _randomCoin;
    }
}
