using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public GameObject Player;
    public float jumpForce;

    public float speed;
    public Text score;
    private int scoreValue = 0;
    public GameObject winTextObject;
    private int livesValue;
    public Text livesText;
    public GameObject loseTextObject;

    public int level;

    private bool facingRight = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();

        winTextObject.SetActive(false);

        livesValue = 3;
        SetLivesText();
        loseTextObject.SetActive(false);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis ("Vertical");
        
        if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
        else if (facingRight == true && hozMovement < 0)
            {
                Flip();
             }

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
         
    }

    void Update()
    {
         if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


         livesText.text = "Lives: " + livesValue.ToString();
            score.text = "Score: " + scoreValue.ToString();

        

        if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.A))
        {
          anim.SetInteger("State",1);
         }
         
        if (Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A)) 
        {
          anim.SetInteger("State",0);
         }
         if (Input.GetKeyDown(KeyCode.W))
        {
          anim.SetInteger("State",2);
         }
         
        if (Input.GetKeyUp(KeyCode.W)) 
        {
          anim.SetInteger("State",0);
         }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }


    void SetLivesText()
        {
            livesText.text = "Lives: " + livesValue.ToString();

            if (livesValue <= 0)
            {
            loseTextObject.SetActive(true);
            
            }
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        livesText.text = "Lives: " + livesValue.ToString();
        score.text = "Score: " + scoreValue.ToString();
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue == 4 && level == 1)
        {
            level += 1;
            livesValue = 3;
            transform.position = new Vector2(50.0f, 0.0f);
        }
        if (scoreValue == 8)
        {
            musicSource.clip = musicClipOne;
                musicSource.Stop();
         
            musicSource.clip = musicClipTwo;
                musicSource.Play();
            musicSource.loop = false;
            winTextObject.SetActive(true);
            Destroy(Player);
        }
        
        if(collision.collider.tag == "Enemy" && level == 1)
        {
            livesValue = livesValue - 1;
            livesText.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            transform.position = new Vector2(-5.0f, 0.0f);
        }

        if(collision.collider.tag == "Enemy" && level == 2)
        {
            livesValue = livesValue - 1;
            livesText.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            transform.position = new Vector2(50.0f, 0.0f);
        }

        if (livesValue == 0)
        {
            Destroy(Player);
            loseTextObject.SetActive(true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            
            if(Input.GetKey(KeyCode.W))
            {
                
                rd2d.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
            }
        }
    }

    
}
