using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("結束畫面 : 群組")]
    public CanvasGroup group_Fianl;

    public static GameManager instance;
    public static bool isGameover;

    private Text final_Text;
    private int AI_Count;


    private void Start()
    {
        instance = this;

        isGameover = false;

        final_Text = GameObject.Find("結束標題").GetComponent<Text>();
    }

    private void Update()
    {
        AI_Count = GameObject.FindGameObjectsWithTag("敵人").Length;
        if (Input.GetKey(KeyCode.P)) DetectDead(PeopleType.ai);
        ReplayGame();
        Quit();
    }

    /// <summary>
    /// 死亡後的行為
    /// </summary>
    /// <param name="type">死亡類型</param>
    public void DetectDead(PeopleType type)
    {
        switch (type)
        {
            case PeopleType.player:
                StartCoroutine(ShowFinal("你死亡了"));
                break;
            case PeopleType.ai:
                StartCoroutine(ShowFinal("您勝利了"));
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 顯示結束畫面
    /// </summary>
    /// <param name="endText">結束標題</param>
    /// <returns></returns>
    public IEnumerator ShowFinal(string endText)
    {
        final_Text.text = endText;

        for (int i = 0; i < 20; i++)
        {
            group_Fianl.alpha += 0.05f;
        }
        yield return new WaitForSeconds(0.02f);
    }

    private void ReplayGame()
    {
        if (isGameover && Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene("遊戲場景");
    }

    private void Quit()
    {
        if (isGameover && Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}

public enum PeopleType
{
    player, ai
}
