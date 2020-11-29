using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using Ez.Pooly;

public class CCrew04Function_MoveObject : MonoBehaviour {

    public ObscuredFloat crewFunction_value;
    public Color crewFunction_color;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("BossMonster"))
        {
            collision.GetComponent<CMonsterDamage>().CrewDamage(crewFunction_value, crewFunction_color);

            // 이펙트
            Pooly.Spawn("Crew04FunctionHitEffect", collision.transform.position + new Vector3(0f, 0.25f, 0f), Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ScreenOut"))
        {
            Pooly.Despawn(gameObject.transform);
        }
    }
}
