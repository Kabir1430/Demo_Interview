using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject[] Wave;

    [Header("Timer")]
    [SerializeField] private int Current_Wave;
    [SerializeField] public float[] Timer_Of_Wave;

    [Header("Animation")]
    [SerializeField] private Animator[] Wave_Anim;

    [Header("Wave Timer And Current Wave")]
    [SerializeField] private TextMeshProUGUI Wave_Timer;
    [SerializeField] private TextMeshProUGUI Current_Waves;

    [Header("Menu Pause")]
    [SerializeField] public GameObject[] Panel;
    [SerializeField] private bool GameIsPaused;
    [SerializeField] private bool Win;

    [Header("LineRender")]
    [SerializeField] private Laser_0[] Laser_0_Script;

    void Start()
    {
        Time.timeScale = 1f;
        Current_Wave = 0;
        StartCoroutine(WaveFunction());
    }

    void Update()
    {
        Timer();
        Wave_Timer_Fun();
    }

    IEnumerator WaveFunction()
    {
        while (Current_Wave < Timer_Of_Wave.Length)
        {
            if (Current_Wave == 0)
            {
                yield return new WaitForSeconds(1.6f);
                Wave_Anim[0].SetBool("Rotate", true);
                Laser_0_Script[0].Laser.enabled = true;
            }
            else if (Current_Wave == 1)
            {
                Wave_Anim[0].speed = 1.1f;
                Wave[0].SetActive(true);
                yield return new WaitForSeconds(1.6f);
                Laser_0_Script[1].Laser.enabled = true;
                Wave_Anim[1].SetBool("Rotate", true);
            }
            else if (Current_Wave == 2)
            {
                Wave_Anim[0].speed = 1.2f;
                Wave_Anim[1].speed = 1.1f;
                Wave[1].SetActive(true);
                yield return new WaitForSeconds(1.6f);
                Laser_0_Script[2].Laser.enabled = true;
                Wave_Anim[2].SetBool("Rotate", true);
            }
            else if (Current_Wave == 3)
            {
                Wave_Anim[0].speed = 1.3f;
                Wave_Anim[1].speed = 1.2f;
                Wave_Anim[2].speed = 1.1f;
                Wave[2].SetActive(true);
                yield return new WaitForSeconds(1.6f);
                Laser_0_Script[3].Laser.enabled = true;
                Wave_Anim[3].SetBool("Rotate", true);
            }
            else if (Current_Wave == 4)
            {
                Wave_Anim[0].speed = 1.4f;
                Wave_Anim[1].speed = 1.3f;
                Wave_Anim[2].speed = 1.2f;
                Wave_Anim[3].speed = 1.1f;
                Wave[3].SetActive(true);
                yield return new WaitForSeconds(1.6f);
                Laser_0_Script[4].Laser.enabled = true;
                Wave_Anim[4].SetBool("Rotate", true);
            }
        }
    }

    public void Timer()
    {
        if (Current_Wave < Timer_Of_Wave.Length)
        {
            Timer_Of_Wave[Current_Wave] -= Time.deltaTime;
            if (Timer_Of_Wave[Current_Wave] <= 0)
            {
                Current_Wave++;
                if (Current_Wave < Timer_Of_Wave.Length)
                    StartCoroutine(WaveFunction());
            }

            if (Current_Wave == Timer_Of_Wave.Length && !Win)
            {
                Win = true;
                Debug.Log("Win");
                // You can add win condition logic here, such as displaying a win message or triggering other game logic.
            }
        }
    }

    public void Wave_Timer_Fun()
    {
        if (Current_Wave < Timer_Of_Wave.Length)
        {
            Wave_Timer.text = Timer_Of_Wave[Current_Wave].ToString("F2");
            Current_Waves.text = (Current_Wave + 1).ToString();
        }
        else
        {
            Debug.LogWarning("Current_Wave index is out of bounds.");
        }

        if (Win)
        {
            Panel[0].SetActive(true);
            Panel[2].SetActive(false);
            Pause();
        }
        else
        {
            Panel[0].SetActive(false);
            Panel[2].SetActive(true);
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Pause_Check()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        Panel[1].SetActive(false); // Assuming Panel[1] is the pause menu
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
      //  Panel[2].SetActive(true); // Assuming Panel[1] is the pause menu
    }
}
