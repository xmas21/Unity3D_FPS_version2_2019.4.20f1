using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("敵人狀態")]
    public stateAI state;
    [Header("等待幾秒切換狀態")]
    public Vector2 v2Idle_to_Random = new Vector2(2, 6);
    [Header("隨機走動半徑")]
    public float radiusRandom = 10;
    [Header("旋轉面相物件的速度")]
    public float turn_speed = 10;
    [Header("檢查是否看到玩家的資訊 : 方體前方未移以及方體的尺寸")]
    public float check_cube_offset_forward = 5;
    public Vector3 check_cube_size = new Vector3(1, 1, 20);

    private float distanceStop = 2;
    private float probabilityStop = 0.3f;
    private float angle_Fire = 2f;
    private float timerFire = 0.5f;
    public float fire_offset = 0.05f;
    private float fire_interval = 0.25f;
    public Vector2 fire_limit = new Vector2(0.8f, 1.3f);

    private bool is_ready_to_random;
    private bool is_random_walking;

    private Vector3 random_position;
    private Vector3 posMove;  // 隨機目標
    private BasePerson basePerson;
    private NavMeshAgent nav;
    private NavMeshHit nmh;
    private Transform player;

    private void Start()
    {
        basePerson = GetComponent<BasePerson>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.Find("玩家").transform;
        posMove = transform.position;
    }

    private void Update()
    {
        if (basePerson.isDead) return;

        CheckState();
        if (CheckPlayerInCube() && state != stateAI.Fire) state = stateAI.Track;
    }

    private void FixedUpdate()
    {
        if (basePerson.isDead) return;

        if (is_random_walking) basePerson.Move(transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radiusRandom);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(random_position, 0.5f);

        Gizmos.color = new Color(0, 0, 1, 0.3f);
        Gizmos.DrawSphere(posMove, 0.5f);

        //****************************************//

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        // 矩陣 = 矩陣.座標腳尺寸(座標，角度，尺寸)
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * check_cube_offset_forward, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, check_cube_size);
    }

    /// <summary>
    /// 切換狀態
    /// </summary>
    private void CheckState()
    {
        switch (state)
        {
            case stateAI.Idle:
                Idle();
                break;
            case stateAI.RandomWalk:
                RandomWalk();
                break;
            case stateAI.Track:
                Track();
                break;
            case stateAI.Fire:
                Fire();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 等待
    /// </summary>
    private void Idle()
    {
        if (!is_ready_to_random)
        {
            float random = Random.Range(v2Idle_to_Random.x, v2Idle_to_Random.y);
            is_ready_to_random = true;
            CancelInvoke("Idle_to_random");
            Invoke("Idle_to_random", random);
        }
    }

    /// <summary>
    /// 等待轉到隨機
    /// </summary>
    private void Idle_to_random()
    {
        state = stateAI.RandomWalk;
        is_ready_to_random = false;
    }

    /// <summary>
    /// 隨機走
    /// </summary>
    private void RandomWalk()
    {
        if (!is_random_walking)
        {
            random_position = Random.insideUnitSphere * radiusRandom + transform.position;

            NavMesh.SamplePosition(random_position, out nmh, radiusRandom, NavMesh.AllAreas);
            random_position = nmh.position;

            is_random_walking = true;
        }
        else if (is_random_walking)
        {
            if (Vector3.Distance(transform.position, random_position) > distanceStop)
            {
                posMove = Vector3.MoveTowards(transform.position, random_position, 1);

                basePerson.ani.SetBool("走路觸發", true);

                LookTarget(posMove);
            }
            else
            {
                float r = Random.Range(0, 1);
                if (r < probabilityStop)
                {
                    state = stateAI.Idle;
                }
                else
                {
                    state = stateAI.RandomWalk;
                }

                is_random_walking = false;
            }

        }
    }

    /// <summary>
    /// 看相目標
    /// </summary>
    /// <param name="posTarget">目標座標</param>
    private Quaternion LookTarget(Vector3 posTarget)
    {
        Quaternion qualook = Quaternion.LookRotation(posTarget - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, qualook, turn_speed * Time.deltaTime);

        return qualook;
    }

    /// <summary>
    /// 檢查玩家是否在立方體
    /// </summary>
    private bool CheckPlayerInCube()
    {
        Collider[] hit = Physics.OverlapBox(transform.position + transform.forward * check_cube_offset_forward, check_cube_size / 2, Quaternion.identity, 1 << 9);

        bool player_in_cube;

        if (hit.Length > 0) player_in_cube = true;
        else player_in_cube = false;

        return player_in_cube;
    }

    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        is_random_walking = false;
        basePerson.ani.SetBool("走路觸發", false);

        Quaternion anglelook = LookTarget(player.position);
        float angle = Quaternion.Angle(transform.rotation, anglelook);

        if (angle <= angle_Fire) state = stateAI.Fire;

    }

    /// <summary>
    /// 開槍
    /// </summary>
    private void Fire()
    {
        LookTarget(player.position);

        if (basePerson.bullet_Curret == 0)
        {
            basePerson.Reload_count();
        }
        else
        {
            basePerson.ani.SetBool("走路觸發", false);

            basePerson.Fire();

            if (timerFire < fire_interval)
            {
                timerFire += Time.deltaTime;
            }
            else
            {
                Vector3 posTargerPoint = basePerson.target.position;
                posTargerPoint.y += (float)(Random.Range(-0.5f, 0.5f) * fire_offset);
                posTargerPoint.y = Mathf.Clamp(posTargerPoint.y, fire_limit.x, fire_limit.y);
                basePerson.fire_pos.localPosition = posTargerPoint;
                timerFire = 0;
            }
        }
    }
}


/// <summary>
/// 四種狀態 : 等待 隨機走路 追蹤 開槍
/// </summary>
public enum stateAI
{
    Idle, RandomWalk, Track, Fire
}