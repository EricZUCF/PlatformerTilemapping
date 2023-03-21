using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    private bool facingRight = true;
    [SerializeField] private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;
    public Text score, lives;
    public GameObject winText, loseText;
    public int scoreValue = 0, livesValue = 3;
    public AudioClip musicClipOne, musicClipTwo, musicClipThree;
    public AudioSource musicSource;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        SetScore();
        SetLives();
        winText.SetActive(false);
        loseText.SetActive(false);
        PlayMusic();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");

        rd2d.AddForce(new Vector2(hozMovement * speed, 0));
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        if(isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            }
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }
    void Update()
    {
        if(isOnGround)
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("State", 1);
            }
            else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }
        if(!isOnGround)
        {
            anim.SetInteger("State", 2);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetScore();
            Destroy(collision.collider.gameObject);
        }
        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetLives();
            Destroy(collision.collider.gameObject);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    void SetScore()
    {
        score.text = "Score: " + scoreValue.ToString();

        if(scoreValue == 4)
        {
            transform.position = new Vector2(31.0f, 1.0f);
            livesValue = 3;
            SetLives();
            PlayMusic();
        }
        if(scoreValue == 8)
        {
            winText.SetActive(true);
            PlayMusic();
        }
    }
    void SetLives()
    {
        lives.text = "Lives: " + livesValue.ToString();

        if(livesValue <= 0)
        {
            winText.SetActive(false);
            loseText.SetActive(true);
            Destroy(gameObject);
        }
    }
    void PlayMusic()
    {
        if(scoreValue < 4)
        {
            musicSource.clip = musicClipOne;
            musicSource.loop = true;
            musicSource.Play();
        }
        if(scoreValue == 4)
        {
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.loop = true;
            musicSource.Play();
        }
        if(scoreValue == 8)
        {
            musicSource.Stop();
            musicSource.clip = musicClipThree;
            musicSource.loop = false;
            musicSource.Play();
        }
    }
}
