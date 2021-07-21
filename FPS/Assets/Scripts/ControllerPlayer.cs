using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家控制腳本
/// </summary>
public class ControllerPlayer : MonoBehaviour
{
    public Vector3 v3Move; // 移動的目標位置

    private Vector3 v3Turn; // 旋轉的目標位置
    private BasePerson basePerson;

    private Transform tar_carema;
    private Text bullet_current_Text;
    private Text bullet_total_Text;

    private void Start()
    {
        basePerson = GetComponent<BasePerson>();
        tar_carema = transform.Find("主攝影機");
        bullet_current_Text = GameObject.Find("當前彈量").GetComponent<Text>();
        bullet_total_Text = GameObject.Find("總彈量").GetComponent<Text>();
    }

    private void Update()
    {
        UpdateBullet();
        GetMoveInput();
        GetTurnInput();

        TurnCamera();
        Fire();
        Reload();
        Jump();

        basePerson.Turn(v3Turn.y, -v3Turn.x);
    }

    /// <summary>
    /// 有跟鋼體有關的都要採用FixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        basePerson.Move(transform.forward * v3Move.z + transform.right * v3Move.x);
    }

    /// <summary>
    /// 偵測鍵盤輸入
    /// </summary>
    private void GetMoveInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        v3Move.x = h;
        v3Move.z = v;
    }

    /// <summary>
    /// 偵測滑鼠旋轉輸入
    /// </summary>
    private void GetTurnInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        v3Turn.x = -mouseY;
        v3Turn.y = mouseX;
    }

    /// <summary>
    /// 攝影機轉向
    /// </summary>
    private void TurnCamera()
    {
        tar_carema.LookAt(basePerson.target);
    }

    /// <summary>
    /// 開火
    /// </summary>
    private void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0)) basePerson.Fire();
    }

    /// <summary>
    /// 裝子彈
    /// </summary>
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            basePerson.Reload_count();
        }
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) basePerson.Jump();
    }

    /// <summary>
    /// 更新子彈資訊
    /// </summary>
    private void UpdateBullet()
    {
        bullet_current_Text.text = basePerson.bullet_Curret.ToString();
        bullet_total_Text.text = basePerson.bullet_Total.ToString();
    }
}
