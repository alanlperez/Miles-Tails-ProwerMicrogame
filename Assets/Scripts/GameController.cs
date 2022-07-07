using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int chaosEmeralds;
    public int rings;
    public Text winText;
    bool endGame = false;

    public Text countdown;
    public float timeToStart = 3f;
    public bool started = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToStart > 0)
        {
            timeToStart -= Time.deltaTime;
            countdown.text = "Collect All 7 Chaos Emeralds\n and All 50 Rings";
        }
        else
        {
            started = true;
            countdown.gameObject.SetActive(false);
        }

        if (endGame && Input.anyKeyDown)
            SceneManager.LoadScene("SampleScene");
    }

    public void EndGame()
    {
        endGame = true;
        winText.gameObject.SetActive(true);
        winText.text = "Tails wins!\nPress Any Key to Restart!";
        Time.timeScale = 0;
    }
}
