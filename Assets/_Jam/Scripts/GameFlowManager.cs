using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameFlowManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject rain;
    public LightningControl lightningControl;
    public GameObject holesRoot;

    public MonsterController monster;

    public CameraMovement cameraMov;
    public Transform Ship;

    public TextMeshProUGUI surviveText;

    public GameObject GameOverMenu;
    public GameObject MainMenu;
    public GameObject StartButton;
    public GameObject ReplayButton;
    public GameObject RepairHoleCanvas;


    private Vector3 PlayerStartPos;
    private List<Hole> holes = new List<Hole>();
    private Vector3 shipInitialPos;
    float surviveTime;

    private Coroutine LightningCoroutine;

    private void Awake()
    {
        PlayerStartPos = Player.transform.position;

        foreach (Transform item in holesRoot.transform)
        {
            holes.Add(item.GetComponent<Hole>());
        }

        monster.OnAttack += Monster_OnAttack;
        shipInitialPos = Ship.position;

        ResetLevel();
    }



    private void Update()
    {
        surviveTime += Time.deltaTime;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowMonster();
        }
#endif
    }

    private void OnDestroy()
    {
        monster.OnAttack -= Monster_OnAttack;
    }

    private void StartRain()
    {
        rain.SetActive(true);
    }

    private void ResetLevel()
    {
        StopAllCoroutines();

        lightningControl.StopLightning();
        lightningControl.gameObject.SetActive(false);
        //rain.SetActive(false);
        Player.transform.position = PlayerStartPos;
        Player.SetActive(false);
        foreach (Hole item in holes)
        {
            item.gameObject.SetActive(false);
        }

        Ship.position = shipInitialPos;
        monster.gameObject.SetActive(false);
        RepairHoleCanvas.gameObject.SetActive(false);

    }

    public void StartGame()
    {
        Player.transform.position = PlayerStartPos;
        Player.SetActive(true);

        rain.SetActive(true);

        holes[0].transform.position = new Vector3(0, holes[0].transform.position.y, 0);
        holes[0].gameObject.SetActive(true);
        holes[0].ResetHole();

        holes[0].OnFixed += FirstHoleFixed;
        RepairHoleCanvas.gameObject.SetActive(true);

    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void FirstHoleFixed()
    {
        RepairHoleCanvas.gameObject.SetActive(false);

        holes[0].OnFixed -= FirstHoleFixed;
        surviveTime = 0;
        LightningCoroutine = StartCoroutine(AfterFirstHoleRoutine());

    }

    void ShowMonster()
    {
        monster.gameObject.SetActive(true);
        monster.Show();
    }

    IEnumerator AfterFirstHoleRoutine()
    {
        yield return new WaitForSeconds(1);
        rain.SetActive(true);

        yield return new WaitForSeconds(3);

        lightningControl.gameObject.SetActive(true);
        lightningControl.PlayLightning();

        StartCoroutine(PlayLightningRepeatedly());

        yield return new WaitForSeconds(3f);
        ShowMonster();

    }

    private void Monster_OnAttack()
    {
        cameraMov.ShakeCamera();


        List<Hole> shuffled = RandomizeElements(holes);

        for (int i = 0; i < holes.Count; i++)
        {
            if (!shuffled[i].gameObject.activeInHierarchy)
            {
                shuffled[i].gameObject.SetActive(true);
                shuffled[i].ResetHole();
                return;
            }
        }

        if (LightningCoroutine != null)
        {
            StopCoroutine(LightningCoroutine);
        }

        //Debug.Log("game over");
        Invoke("GameOver", 1f);
    }

    void GameOver()
    {

        if (LightningCoroutine != null)
        {
            StopCoroutine(LightningCoroutine);
        }

        surviveText.text = "YOU'VE SURVIVED\n" + ((int)surviveTime).ToString() + " SEC";
        StartCoroutine(ShipSinkRoutine());
        monster.Sink();
        Invoke("ShowGameOverScreen", 2f);
        Invoke("DeactivatePlayer", 3f);
    }

    void DeactivatePlayer()
    {
        Player.SetActive(false);
    }

    void ShowGameOverScreen()
    {
        MainMenu.SetActive(true);
        GameOverMenu.SetActive(true);
        ReplayButton.SetActive(true);
        StartButton.SetActive(false);
    }

    IEnumerator PlayLightningRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20, 30));
            lightningControl.PlayLightning();
        }
    }


    IEnumerator ShipSinkRoutine()
    {
        shipInitialPos = Ship.position;
        Vector3 downPos = shipInitialPos + Vector3.down * 30;
        float sinkSpeed = 1f;

        while ((Ship.position - downPos).sqrMagnitude > 0.5f)
        {
            Ship.position += Vector3.down * Time.deltaTime * sinkSpeed;

            yield return null;
        }


    }

    List<Hole> RandomizeElements(List<Hole> h)
    {
        for (int i = 0; i < h.Count - 1; i++)
        {
            int lucky = Random.Range(i, h.Count);
            Hole temp = h[lucky];
            h[lucky] = h[i];
            h[i] = temp;
        }

        return h;
    }




}
