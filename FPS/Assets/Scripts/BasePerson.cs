﻿using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class BasePerson : MonoBehaviour
{
    [Header("移動速度"), Range(0, 10)]
    public float move_speed = 0.2f;
    [Header("左右旋轉速度"), Range(0, 1000)]
    public float LR_speed = 160;
    [Header("上下旋轉速度"), Range(0, 100)]
    public float UD_speed = 1.3f;
    [Header("跳躍高度"), Range(0, 1000)]
    public float jump = 100;
    [Header("血量"), Range(0, 1000)]
    public float hp = 100;
    [Header("攻擊力"), Range(0, 500)]
    public float attack = 20;
    [Header("限制攝影機上下位置")]
    public Vector2 target_limit = new Vector2(-0.5f, 3);

    [Header("發射子彈位置")]
    public Transform fire_pos;
    [Header("子彈物件")]
    public GameObject bullet;
    [Header("子彈速度"), Range(0, 3000)]
    public float bullet_speed = 1000;
    [Header("開槍間隔"), Range(0, 1)]
    public float bullet_interval = 0.1f;
    [Header("目前子彈數量"), Range(0, 1)]
    public int bullet_Curret = 30;
    [Header("彈夾數量"), Range(0, 1)]
    public int bullet_Clip = 30;
    [Header("總子彈數"), Range(0, 1)]
    public int bullet_Total = 150;
    [Header("空彈音效")]
    public AudioClip no_ammo_Sound;

    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Animator ani;

    private float hpMax;                // 最大血量
    private float timerFire;            // 射擊計時器
    private bool ismove;                // 是否在移動

    private Rigidbody rig;
    private AudioSource aud;
    private AudioClip fire_Sound;
    private Rig rigging;

    private void Start()
    {
        GetC();
    }

    private void Update()
    {
        print(rig.velocity.x);
        print(rig.velocity.z);
        AnimatorMove();
    }

    /// <summary>
    /// 換彈設定布林直
    /// </summary>
    /// <returns></returns>
    private IEnumerator Reloading()
    {
        rigging.weight = 0.2f;
        ani.SetBool("換彈開關", true);
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length * 0.9f);

        ani.SetBool("換彈開關", false);
        rigging.weight = 1;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="movePosition">要移動到的位置</param>
    public void Move(Vector3 movePosition)
    {
        rig.MovePosition(transform.position + movePosition * move_speed);
    }

    /// <summary>
    /// 旋轉鏡頭
    /// </summary>
    /// <param name="turnValue">旋轉值</param>
    /// <param name="moveTarget">位移目標的值</param>
    public void Turn(float LR_Value, float UD_Value)
    {
        // 左右旋轉 : 旋轉(上方 * 控制值 * 旋轉速度) 
        transform.Rotate(transform.up * LR_Value * LR_speed * Time.deltaTime);
        // 上下旋轉 : 目標.位移(x, y, z)
        target.Translate(0, UD_Value * UD_speed * Time.deltaTime, 0);
        // 取得 目標物件 的 區域座標
        Vector3 posTarget = target.localPosition;
        posTarget.y = Mathf.Clamp(posTarget.y, target_limit.x, target_limit.y);
        target.localPosition = posTarget;
    }

    /// <summary>
    /// 開槍
    /// </summary>
    public void Fire()
    {
        if (ani.GetBool("換彈開關")) return;

        if (timerFire < bullet_interval) timerFire += Time.deltaTime;
        else
        {
            if (bullet_Curret > 0)
            {
                bullet_Curret--;
                timerFire = 0;
                aud.PlayOneShot(fire_Sound, Random.Range(0.3f, 1f));
                GameObject tempBullet = Instantiate(bullet, fire_pos.position, Quaternion.identity);
                tempBullet.GetComponent<Rigidbody>().AddForce(-fire_pos.forward * bullet_speed);
            }
            else
            {
                aud.PlayOneShot(no_ammo_Sound, Random.Range(0.3f, 0.8f));
                timerFire = 0;
            }
        }
    }

    /// <summary>
    /// 換彈夾
    /// </summary>
    public void Reload_count()
    {
        if (bullet_Curret == bullet_Clip || bullet_Total == 0) return;

        StartCoroutine(Reloading());

        int bulletGetCount = bullet_Clip - bullet_Curret;

        if (bullet_Total >= bulletGetCount)
        {
            bullet_Total -= bulletGetCount;
            bullet_Curret += bulletGetCount;
        }
        else
        {
            bullet_Curret += bullet_Total;
            bullet_Total = 0;
        }
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    public void Jump()
    {
        rigging.weight = 0;
        rig.AddForce(0, jump, 0);
        ani.SetBool("跳躍開關", true);
    }

    /// <summary>
    /// 抓取元件
    /// </summary>
    private void GetC()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
        rigging = transform.GetChild(4).GetComponent<Rig>();
        target = transform.Find("目標物件");
        fire_Sound = aud.clip;
    }

    /// <summary>
    /// 動畫移動
    /// </summary>
    private void AnimatorMove()
    {
        if (rig.velocity.x != 0 || rig.velocity.z != 0) ismove = true;
        //ani.SetBool("走路觸發", ismove);
    }
}
