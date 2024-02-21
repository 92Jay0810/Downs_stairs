using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpped;
    [SerializeField] float jumpForce;
    // Start is called before the first frame update
    [SerializeField] int HP;
    int Score;
    SpriteRenderer spireRender;
    Animator animate;
    Rigidbody2D rb;
    [SerializeField] TextMeshProUGUI HP_text;
    [SerializeField] TextMeshProUGUI Score_text;
    [SerializeField] TextMeshProUGUI Score_text_InMainMenu;
    GameObject currentFloor;
    [SerializeField] GameObject retryMenu;
    [SerializeField] float disappearTime;
    void Start()
    {
        moveSpped = 5.0f;
        jumpForce = 7f;
        HP = 10;
        Score = 0;
        spireRender = GetComponent<SpriteRenderer>();
        animate = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        disappearTime = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpped * Time.deltaTime, 0, 0);
            spireRender.flipX = false;
            animate.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpped * Time.deltaTime, 0, 0);
            spireRender.flipX = true;
            animate.SetBool("run", true);
        }
        else
        {
            animate.SetBool("run", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Normal")
        {
            //確保是碰到上面，利用接觸點的法向量判斷，當碰到上面時，接觸點的法向量會與接觸面垂直
            if (collision.contacts[0].normal == new Vector2(0.0f, 1.0f))
            {
                Debug.Log("撞到一般階梯");
                currentFloor = collision.gameObject;
                ModifyHP(1);
                Update_HP_Text();
                Score += 1;
                Update_Score_Text();
            }

        }
        else if (collision.gameObject.tag == "Nail")
        {
            if (collision.contacts[0].normal == new Vector2(0.0f, 1.0f))
            {
                Debug.Log("撞到釘子階梯");
                currentFloor = collision.gameObject;
                ModifyHP(-2);
                animate.SetTrigger("hurt");
                Update_HP_Text();
                Score += 1;
                Update_Score_Text();
            }
        }
        else if (collision.gameObject.tag == "trampoline")
        {
            if (collision.contacts[0].normal == new Vector2(0.0f, 1.0f))
            {
                Debug.Log("撞到彈跳版");
                currentFloor = collision.gameObject;
                ModifyHP(1);
                Update_HP_Text();
                Score += 1;
                Update_Score_Text();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else if (collision.gameObject.tag == "Fake")
        {
            if (collision.contacts[0].normal == new Vector2(0.0f, 1.0f))
            {
                Debug.Log("撞到沙梯");
                currentFloor = collision.gameObject;
                ModifyHP(1);
                Update_HP_Text();
                Score += 1;
                Update_Score_Text();
                StartCoroutine(Disappear_Fake_AfterTime(collision.gameObject)); // 啟動多執行緒的計時器
            }
        }
        else if (collision.gameObject.tag == "Ceilling")
        {
            Debug.Log("撞到天花板");
            ModifyHP(-2);
            animate.SetTrigger("hurt");
            Update_HP_Text();
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeadLine")
        {
            Debug.Log("你輸了!!");
            Die();
        }else if (collision.gameObject.tag == "sting")
        {
            Debug.Log("碰到釘子");
            ModifyHP(-2);
            animate.SetTrigger("hurt");
            Update_HP_Text();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "love")
        {
            Debug.Log("碰到愛心");
            ModifyHP(2);
            Update_HP_Text();
            Destroy(collision.gameObject);
        }
    }
    void ModifyHP(int num)
    {
        HP += num;
        if (HP >= 10)
        {
            HP = 10;
        }
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }
    void Update_HP_Text()
    {
        HP_text.text = HP.ToString();
    }
    void Update_Score_Text()
    {
        Score_text.text = Score.ToString();
        Score_text_InMainMenu.text = Score.ToString();
    }
    IEnumerator Disappear_Fake_AfterTime(GameObject fakeStair)
    {
        yield return new WaitForSeconds(disappearTime); //等待的時間
        if (fakeStair != null)
        {
            fakeStair.transform.position=new Vector3(0f, 4.5f, 0);
        }
    }
    void Die()
    {
        Time.timeScale = 0f;
        retryMenu.SetActive(true);
    }
    public void RetryScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
