using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackDistance;
    [SerializeField] float attackSpeed;
    WaitForSeconds waitAttackTime;      //等待攻擊協程時間
    bool isOnAttack;
    WaitForSeconds waitFreezeTime;
    bool isOnFreeze;
    WaitForSeconds waitAttackAmTime;        //等待攻擊動畫協程時間
    
    [SerializeField] Transform m_playerTrsf;   //獲取玩家位置
    [SerializeField] AudioData sFX_Die;
    [SerializeField] Volume enVolume;       //怪物體積(使用自訂結構體,包含了v3,f,f)

    Collider2D em_collider2D;

    void Awake()
    {
        em_collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        //m_playerTrsf = GameObject.FindWithTag("Player").GetComponent<Transform>();        //舊版使用查找獲取玩家-取消
        m_playerTrsf = Player.player.GetComponent<Transform>();
        waitAttackTime = new WaitForSeconds(attackSpeed);
        waitAttackAmTime = new WaitForSeconds(0.52f);
        waitFreezeTime = new WaitForSeconds(3);
    }
    void OnEnable()
    {
        enVolume.Pos = transform.position;
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(enVolume, m_playerTrsf.position);        
    }

    void OnDisable()
    {
        StopAllCoroutines();
        isOnFreeze = false;
        isOnAttack = false;
    }

    //生成動畫結束後調用移動協程
    public void GenerateOver()
    {
        StartCoroutine(nameof(MoveCoroutine));
        _animator.SetBool("Move",true);
    }

    IEnumerator MoveCoroutine()
    {
        em_collider2D.enabled = true;
        while(true && !isOnFreeze)
        {
            if(Vector2.Distance(transform.position, m_playerTrsf.position) < attackDistance)
            {
                _animator.SetTrigger("Attack");
                yield return waitAttackAmTime;
            }else
            {
                if(transform.position.x > m_playerTrsf.position.x) _spriteRenderer.flipX = false;
                if(transform.position.x < m_playerTrsf.position.x) _spriteRenderer.flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, m_playerTrsf.position, moveSpeed * Time.deltaTime);                
                yield return null;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D collider2D)
    {
        if(collider2D.tag == "Player")
        {
            if(!isOnAttack && !isOnFreeze)
            {
                isOnAttack = true;
                StartCoroutine(nameof(AttackCoolDown));
            }
        }       
    }
    void OnTriggerExit2D (Collider2D collider2D)
    {
        if(collider2D.tag == "Player")
        {
            isOnAttack = false;
            StopCoroutine(nameof(AttackCoolDown));
        }       
    }
    
    IEnumerator AttackCoolDown()
    {
        Player.player.Injuried();
        while(true)
        {
            yield return waitAttackTime;
            Player.player.Injuried();
        }
    }
    
    //死亡後
    public void Death()
    {
        em_collider2D.enabled = false;
        _animator.SetTrigger("Death");
        AudioManager.Instance.PlaySFX(sFX_Die);
        StopAllCoroutines();
        StartCoroutine(nameof(DeathCoroutine));
    }
    IEnumerator DeathCoroutine()
    {
        EnemyManager.Instance.ReleaseEnemy();
        EnemyManager.Instance.killEnemyAmount++;
        yield return waitAttackTime;
        
    }

    public void Freeze()
    {        
        if(isOnFreeze) return;
        StopCoroutine(nameof(MoveCoroutine));
        StopCoroutine(nameof(AttackCoolDown));
        StartCoroutine(nameof(FreezeTimeCoroutine));
    }
    IEnumerator FreezeTimeCoroutine()
    {
        isOnFreeze = true;
        _animator.SetBool("Freeze", true);
        yield return waitFreezeTime;
        isOnFreeze = false;
        _animator.SetBool("Freeze", false);
        StartCoroutine(MoveCoroutine());
    }



}

