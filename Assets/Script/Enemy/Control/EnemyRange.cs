using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    #region SO 데이터(레인지 & 버퍼)
    [HideInInspector] public float distanceToPlayerMax; // 플레이어와의 최대 거리
    [HideInInspector] public bool isRanged; // 근거리 원거리 공격 체크
    [HideInInspector] public bool isBuffer; // 버퍼인지 체크
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
