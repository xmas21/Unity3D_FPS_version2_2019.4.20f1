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

    private bool is_ready_to_random;
    private bool is_random_walking;

    private Vector3 random_position;
    private BasePerson basePerson;
    private NavMeshAgent nav;
    private NavMeshHit nmh;


    private void Start()
    {
        basePerson = GetComponent<BasePerson>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radiusRandom);
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
                break;
            case stateAI.Fire:
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
    }
}




/// <summary>
/// 四種狀態 : 等待 隨機走路 追蹤 開槍
/// </summary>
public enum stateAI
{
    Idle, RandomWalk, Track, Fire
}