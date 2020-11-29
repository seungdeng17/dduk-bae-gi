using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;


public class CDirectMove : MonoBehaviour {

    public Vector2 _direction;

    public ObscuredFloat _speed;
    [HideInInspector]
    public ObscuredFloat _originSpeed;
    [HideInInspector]
    public Rigidbody2D _rigidbody2d;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _originSpeed = _speed;
        //_rigidbody2d.velocity = _direction * _speed;
    }

    //private void OnSpawned()
    //{
    //    _rigidbody2d.velocity = _direction * _speed;
    //}
}
