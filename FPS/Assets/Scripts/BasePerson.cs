using UnityEngine;

public class BasePerson : MonoBehaviour
{
    [Header("移動速度"), Range(0, 10)]
    public float move_speed = 0.2f;
    [Header("鏡頭旋轉速度"), Range(0, 1000)]
    public float turn_speed = 3;
    [Header("跳躍高度"), Range(0, 1000)]
    public float jump = 100;
    [Header("血量"), Range(0, 1000)]
    public float hp = 100;
    [Header("攻擊力"), Range(0, 500)]
    public float attack = 20;

    private float hpMax; // 最大血量
    private Animator ani;
    private Rigidbody rig;
    private AudioSource aud;

    private void Start()
    {
        GetC();
    }

    private void GetC() // 抓取元件
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
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
    /// 鏡頭旋轉
    /// </summary>
    /// <param name="turnValue">旋轉值</param>
    public void Turn(Vector3 turnValue)
    {
        transform.Rotate(turnValue * turn_speed);
    }
}
