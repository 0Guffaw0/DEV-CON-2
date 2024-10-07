using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public GameObject bushPrefab;
    public Animator anim;
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI endMessageText; 

    private Vector3 previousPosition;
    public float timer = 15f;
    private bool timeUp = false;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        previousPosition = transform.position;
        endMessageText.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
                    {
                        previousPosition = movePoint.position;
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        GameObject Bush = Instantiate(bushPrefab, previousPosition, Quaternion.identity);
                    }
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                    {
                        previousPosition = movePoint.position;
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        GameObject Bush = Instantiate(bushPrefab, previousPosition, Quaternion.identity);
                    }
                }

                anim.SetBool("moving", false);
            }
            else
            {
                anim.SetBool("moving", true);
            }

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                TimeUp();
            }

            timerText.text = "Time Left: " + Mathf.Max(timer, 0).ToString("F2");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "endGate")
        {
            SceneManager.LoadScene("Level2");
        }

        if (collision.gameObject.tag == "endGate1")
        {
            SceneManager.LoadScene("Level3");
        }
    }

    private void TimeUp()
    {
        timeUp = true;
        endMessageText.gameObject.SetActive(true); 
    }
}

