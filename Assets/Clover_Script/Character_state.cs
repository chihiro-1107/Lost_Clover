using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_state : MonoBehaviour
{
    [SerializeField]
    private int limit_hp = 100;   //体力の上限値
    [SerializeField]
    private int hp       = 100;   //体力値
    [SerializeField]
    private float move_spead = 1.0f; //速度
    [SerializeField]
    private int attack = 1;      //攻撃力
    

    // Start is called before the first frame update
    void Start()
    {
        if (limit_hp < hp)
        {
            //初期設定のhpが上限より大きい場合は上限に設定
            hp = limit_hp;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //攻撃処理
    void Attack()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        if (tr != null)
        {
            tr.emitting = true;
        }
    }
}
