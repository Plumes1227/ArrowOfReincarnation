using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;
    [Header("---輪迴箭矢模式/-1線性(雙軸輪迴),1非線性(單軸輪迴)/---")]
    [SerializeField] bool samsaraStyle;
    //[SerializeField] Vector3 originalPos;     //箭矢初始位置存取(未使用)
    //[SerializeField] Vector3 loopPos;     //循環點(未完成)
    [SerializeField] int durable;
    void Awake()
    {
        
    }
    void OnEnable()
    {
        durable = 10;
    }
    void Start()
    {
        //向前後打兩條射線，以碰到的邊界點為兩點循環。
    }



    void FixedUpdate()
    {        
        transform.Translate(Vector2.right * speed * Player.player.ArrowSpeed * Time.deltaTime);      

        if(!Viewport.Instance.DetcetIfInViewport(transform.position))
        {   
            //循環儲存箭...(未完成)         
            //if(loopPos == Vector3.zero) loopPos = Viewport.viewport.ArrowSpawnPosition(transform.position);
            transform.position = Viewport.Instance.ArrowSpawnPosition(transform.position);
        }
    }

    /// <summary>
    /// 立刻禁用自身
    /// </summary>
    public void RemoveArrow()
    {        
        gameObject.SetActive(false);
        Player.player.AddArrowAmount();
        Player.player.RemoveFromList(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Enemy")
        {
            durable--;
            collider2D.gameObject.GetComponent<Enemy>().Death();
            ScoreManager.Instance.killAmountAdd();
            
            if(durable == 0)
            {
                SkillCardUi.Instance.ReplenishSkillCard();
                RemoveArrow();
            }
        }
        if(collider2D.tag == "Player")
        {
            durable--;
            collider2D.gameObject.GetComponent<Player>().Injuried();
            
            if(durable == 0)
            {
                SkillCardUi.Instance.ReplenishSkillCard();
                RemoveArrow();
            }
        }
    }
}
