using UnityEngine;

/// <summary>
/// 玩家控制腳本
/// </summary>
public class ControllerPlayer : MonoBehaviour
{
    private Vector3 v3Move; // 移動的目標位置
    private Vector3 v3Turn; // 旋轉的目標位置
    private BasePerson basePerson;

    private void Start()
    {
        basePerson = GetComponent<BasePerson>();
    }

    private void Update()
    {
        GetMoveInput();
        GetTurnInput();
    }

    private void FixedUpdate() //有跟鋼體有關的都要採用FixedUpdate
    {
        basePerson.Move(v3Move);
        basePerson.Turn(v3Turn);
    }

    private void GetMoveInput() // 偵測鍵盤輸入
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        v3Move.x = h;
        v3Move.z = v;
    }

    private void GetTurnInput() // 偵測滑鼠旋轉輸入
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        v3Turn.x = -mouseY;
        v3Turn.y = mouseX;
    }
}
