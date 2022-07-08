using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed;
    Rigidbody2D playerRigidbody;
    float inputX;
    float inputY;

    public LayerMask wallLayer;
    public float rayLength;
    bool canJump;
    public float jumpHeight; 

    bool hurt;
    public float maxHealth;
    [SerializeField]
    float health;
    public float timeBetweenHurt;

    float iframe;

    Animator anim;
    SpriteRenderer rend;

    int coins = 0;
    int chaos = 0;


    public Image healthImage;
    public Text coinsText;
    public Text chaosText;

    public GameObject gameoverUI;
    bool gameover;

    Color c;

    public AudioClip ringsAudio;
    public float volumeRings;
    
    public AudioClip chaosAudio;
        public float volumeChaos;

    public AudioClip invincibilityAudio; 
    public float volumeInvincibility;
    public AudioClip badnikDefeat; 
    public float volumeBadnik;
    public AudioClip levelCleared; 
    public float volumeLevelCleared;

    public AudioClip deathSpike; 
    public float volumeDeathSpike;
    public AudioClip tailsJump; 
    public float volumeTailsJump;
    AudioSource src;

    GameController cont;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        canJump = true;
        health = maxHealth;
        hurt = false;
        iframe = timeBetweenHurt;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        gameover = false;
        c = rend.material.color;
        src = GetComponent<AudioSource>();
        cont = FindObjectOfType<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX !=  0)
            playerRigidbody.AddForce(Vector2.right * inputX * speed * Time.deltaTime);

        rend.flipX = (inputX < 0);

        rend.flipY = (inputY < 0);


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, wallLayer);

        if (hit.collider != null)
        {
            canJump = true;

        }


        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            src.PlayOneShot(tailsJump, volumeTailsJump);
            playerRigidbody.AddForce(Vector2.up * jumpHeight);
            canJump = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * rayLength);


        if (iframe > 0) iframe -= Time.deltaTime;

        // test Damage
        if (!hurt && Input.GetKeyDown(KeyCode.LeftControl))
            Damage(2);

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, health/maxHealth, Time.deltaTime * 10f);
        coinsText.text = "X " + coins.ToString() + "/50";
        chaosText.text = "X " + chaos.ToString() + "/7";


        anim.SetBool("moving", inputX != 0);
        anim.SetBool("canJump", canJump);
        anim.SetBool("hurt", hurt);

        if (gameover && Input.anyKeyDown)
        {
            SceneManager.LoadScene("SampleScene");
            Time.timeScale = 1f;
        }
    }


    public void Damage(float amt)
    {
        if(iframe < 0)
        {
            health -= amt;
            hurt = true;
            Invoke("ResetHurt", 0.2f);
            if (health <= 0)
            {
                GameOver();
            }
            iframe = timeBetweenHurt;
        }
    
    }

    public void GameOver()
    {
        gameover = true;
        gameoverUI.SetActive(true);
        Time.timeScale = 0;
    }

    void ResetHurt()
    {
        hurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            src.PlayOneShot(ringsAudio, volumeRings);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Chaos"))
        {
            chaos++;
            src.PlayOneShot(chaosAudio, volumeChaos);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Monitor") && health > 0)
        {
            collision.gameObject.SetActive(false);
            StartCoroutine ("GetInvulnerable");
        }
        if(collision.gameObject.CompareTag("Goal") && chaos >= cont.chaosEmeralds && coins >= cont.rings)
        {
                gameover = true;
                src.PlayOneShot(levelCleared, volumeLevelCleared);
                cont.EndGame();
        }
        if(collision.gameObject.CompareTag("Spike"))
        {
            src.PlayOneShot(deathSpike, volumeDeathSpike);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerRigidbody.velocity.y <= 0)
        {
            float boundsY = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            if(transform.position.y > collision.gameObject.transform.position.y + boundsY)
            {
                playerRigidbody.AddForceAtPosition(-playerRigidbody.velocity.normalized * jumpHeight/2, playerRigidbody.position);
                collision.gameObject.GetComponent<EnemyController>().Damage(5f);
                src.PlayOneShot(badnikDefeat, volumeBadnik);

            }
        }
    }
    IEnumerator GetInvulnerable()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        Physics2D.IgnoreLayerCollision(8, 9, true);
        c.a = 0.5f;
        rend.material.color = c;
        src.PlayOneShot(invincibilityAudio, volumeInvincibility);
        Invoke("StopAudio", 8f);
        yield return new WaitForSeconds (8f);
        Physics2D.IgnoreLayerCollision(7, 8, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        c.a = 1f;
        rend.material.color = c;
    }
    private void StopAudio()
    {
        GetComponent<AudioSource>().Stop();
    }
    
}
