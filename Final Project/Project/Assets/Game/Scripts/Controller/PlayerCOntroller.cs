using UnityEngine;
using System.Collections;

public class PlayerCOntroller : MonoBehaviour {

    public static PlayerCOntroller instance;
  
    [HideInInspector]public Transform player; //used by other scripts to get player position
    public float rotSpeed = 10;               //the speed with the ship rotate
    Vector3 rotate;  
    public float moveSpeed = 3;               //speed with which ship moves  
    [HideInInspector]         
    public bool isPlayer = false;             //keep track is game is on
    private int score;                        //ref to score 
    private int juice;                        // ref to juice 
    [SerializeField]
    private GameObject explosion;             // ref to explosion prefabs
    private SpriteRenderer sprite;
    [SerializeField]
    private SpriteRenderer sprite1;
    [SerializeField]
    private SpriteRenderer sprite2;//ref to ship image 
    private AudioSource audioSource;          // ref to audio source attached to gameobject
    [SerializeField]
    private AudioClip sounds;
    [SerializeField]
    private AudioClip sounds2;
    [SerializeField]
    private AudioClip sounds3;   // for points 
    private int isGameOver;
    //  public int TotalScore=0;
    // Vector3 scalePlus;
    Animator animator;
    
    void Awake()
    {
        //we make this instance so we dont need to give ref to other object
        if (instance == null)
            instance = this;
        //we get the player transform
        player = GetComponent<Transform>();
    }

	// Use this for initialization
	void Start ()
    {
        // transform.GetChild(3).gameObject.transform.position = new Vector3(0, 0, -3);

        GameController.instance.isGameOver = false;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        //scalePlus = new Vector3();
        //scalePlus = transform.localScale;
        //we set basic settings at start
        score = 0;
        GameController.instance.currentScore = score;///////////
        //at start we want out ship to static
        transform.Translate(Vector3.zero);
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();


        sprite1 = GetComponent<SpriteRenderer>();
        sprite2 = GetComponent<SpriteRenderer>();


        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
    
	void Update ()
    {
        //when we 1st time tap the screen the game is on and we then call movement method
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            isPlayer = true;
            Movement();
        }

        if (immortal > 0) immortal--;
        else transform.GetChild(2).gameObject.SetActive(false);

        //scalePlus.x += 0.00000001f;
        //scalePlus.y += 0.00000001f;
        //  transform.localScale = scalePlus;
        //if game is on then only we move the ship



        if (isPlayer == true && GameController.instance.isGameOver == false)
        {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }

    }

    void Movement()
    {
        //we get the position on the screen where the touch happen
        Vector2 pos = Input.mousePosition;

        //if the touch is right side the we turn right and if its left side then we turn left
        if ((pos.x > Screen.width / 2 && pos.x < Screen.width && Input.GetMouseButton(0)) || Input.GetKey(KeyCode.RightArrow))
        {
            //turn right
            RightMovement();
        }
       
        else if( Input.GetKey(KeyCode.LeftArrow))
        {
            LeftMovement();
        }
        else if(Input.GetMouseButton(0))
        {
            LeftMovement();
        }
    }

    //here we add the angle to the ships existing angle to make it turn
    void LeftMovement()
    {
        rotate.z += (1 * rotSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotate.z));
    }

    void RightMovement()
    {
        rotate.z += (-1 * rotSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotate.z));
    }

    //Undethable
    private int immortal = 0;
    public void setImmortalFor(int x)
    {
        immortal = x;
        transform.GetChild(2).gameObject.SetActive(true);

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Juice"))
        {
            
            score++;
            GameController.instance.currentScore = score;
           
            GameController.instance.juice++;
            GameController.instance.Save();
            other.gameObject.SetActive(false);
            if(moveSpeed < 7.7)
              moveSpeed += 0.25f;
          
            this.transform.localScale += new Vector3(0.023f,0.023f);
            setImmortalFor(60);
            if (GameController.instance.currentScore >= 2)
            {
                transform.GetChild(4).gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -3);
            }
            if (GameController.instance.currentScore >= 3)
            {
             
                transform.GetChild(3).gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4);
            }
        }

       


        if (other.CompareTag("Meteor"))
        {
            
            if (transform.GetChild(1).gameObject.active != true && immortal<1)
            {
                sprite.enabled = false;                            //here we disable ship image 
                transform.GetChild(0).gameObject.SetActive(false); //we deactivate the thrust
                GameObject boom = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity); //we activate the explosion
                boom.transform.localScale *= 2.5f;
                gameObject.SetActive(false);
                GameController.instance.isGameOver = true;

               
               
            }
            else if (immortal>=1)
            {
                other.gameObject.SetActive(false);

            }
            else
            {
                other.gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(false);
               // audioSource.mute = true;
               
            //    audioSource.PlayOneShot(sounds);
               
            }
            /*sprite.enabled = false;                            //here we disable ship image 
            transform.GetChild(0).gameObject.SetActive(false); //we deactivate the thrust
            GameObject boom = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity); //we activate the explosion
            gameObject.SetActive(false);
            GameController.instance.isGameOver = true;*/
        }
        else if (other.CompareTag("helmet"))
        {
            transform.GetChild(1).gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }
}
