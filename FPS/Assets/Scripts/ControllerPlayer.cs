using UnityEngine;

/// <summary>
/// 玩家控制腳本
/// </summary>
public class ControllerPlayer : MonoBehaviour
{
    private Vector3 v3Move; // 移動的目標位置
    private Vector3 v3Turn; // 旋轉的目標位置
    private BasePerson basePerson;
    private Transform tar_carema;


    private void Start()
    {
        basePerson = GetComponent<BasePerson>();
        tar_carema = transform.Find("主攝影機");
    }

    private void Update()
    {
        GetMoveInput();
        GetTurnInput();

        Fire();
        TurnCamera();
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

    private void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0)) basePerson.Fire();
    }
}
