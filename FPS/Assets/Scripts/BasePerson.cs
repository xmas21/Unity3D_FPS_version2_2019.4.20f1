using UnityEngine;

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

    public Animator ani;

    private float hpMax; // 最大血量
    private bool ismove; // 是否在移動

    private Rigidbody rig;
    private AudioSource aud;
    private Transform target;

    private void Start()
    {
        GetC();
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
    /// 抓取元件
    /// </summary>
    private void GetC()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
        target = transform.Find("目標物件");
    }


    private void AmimatorMove()
    {
        if (rig.velocity.x != 0 || rig.velocity.z != 0) ismove = true;
        ani.SetBool("走路觸發", ismove);
    }
}
