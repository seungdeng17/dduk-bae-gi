using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDirectOnSpawnedMove : CDirectMove
{
    private void OnSpawned()
    {
        _rigidbody2d.velocity = _direction * _speed;
    }
}
