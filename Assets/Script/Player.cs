using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player player;
    [SerializeField] HeadUi headUi;
    [SerializeField] PlayerInput input;
    Vector2 mouseInWorldPos;

    [Header("---能力值設置---")]
    [SerializeField] float maxHp = 10;
    float nowHp;
    [SerializeField] float maxMP = 3;
    float nowMP;
    WaitForSeconds recoverMPTime;
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    WaitForSeconds dashTime;
    bool isOnDash;
    Vector2 originalMoveVelocity;       //存取原始移動動能(攻擊後還原動能用)
    [SerializeField] float attackSpeed;
    WaitForSeconds attackSpeedTime;
    [HideInInspector] public float ArrowSpeed = 1;
    WaitForSeconds arrowSpeedUpTime;
    bool isOnInvincible;
    bool isOnAttack;    

    [Header("---箭矢設置-箭矢預制體---")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int maxArrowAmount;
    public int MaxArrowAmount => maxArrowAmount;
    int arrowAmount;
    public int ArrowAmount => arrowAmount;
    List<GameObject> arrowList;     //箭矢列表，方便控制以射出的箭矢
    public void RemoveFromList(GameObject arrow) => arrowList.Remove(arrow);
    Vector2 shotDirection;      //射擊方向

    [Header("---圖片大小配置---")]
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float paddingX;
    [SerializeField] float paddingY;
    Animator _animator;
    
    [Header("---特效配置---")]
    [SerializeField] GameObject playerAfterimagePrefabR;
    [SerializeField] GameObject playerAfterimagePrefabL;
    [SerializeField] GameObject _eff_invincible;
    [SerializeField] GameObject _eff_HealingLife;
    Rigidbody2D _rigidbody2D;
    Vector2 movementHV;
    [Header("---音效配置---")]
    [SerializeField] AudioData sFX_shotArrow;
    [SerializeField] AudioData sFX_death;

    void OnEnable()
    {
        input.onMove += Move;
        input.onStopMove += StopMove;
        input.onAttack += Attack;
        input.onDodge += Dash;
        input.getMousePos += GetMousePos;

        nowHp = maxHp;
        nowMP = maxMP;
        arrowAmount = maxArrowAmount;
        headUi.InitializeHpBar(nowHp, maxHp);
    }
    void OnDisable()
    {        
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onAttack -= Attack;
        input.onDodge -= Dash;
        input.getMousePos -= GetMousePos;
    }
    void Awake()
    {
        arrowList = new List<GameObject>();
        player = this.GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        attackSpeedTime = new WaitForSeconds(attackSpeed);
        arrowSpeedUpTime = new WaitForSeconds(5);
        recoverMPTime = new WaitForSeconds(5);
        dashTime = new WaitForSeconds(0.2f);                
        _rigidbody2D.gravityScale = 0f;
        input.EnableGameplayInput();
        StartCoroutine(nameof(RecoverMagicPointCoroutine));
    }

    public void Die()
    {
        if(GameManager.GameState == GameState.GameOver) return;
        nowHp = 0f;
        _rigidbody2D.velocity = Vector2.zero;
        StopAllCoroutines();
        AudioManager.Instance.PlaySFX(sFX_death);
        _animator.SetTrigger("OnDeath");
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        headUi.InitializeHpBar(0, maxHp);
    }

    public Transform PlayerTransformGet()
    {
        return transform;
    }

    void Move(Vector2 moveInput)
    {
        if(GameManager.GameState != GameState.Playing) return;
        originalMoveVelocity = moveInput * moveSpeed;
        if(isOnAttack) return;
        if(moveInput.x > 0) _spriteRenderer.flipX = false;
        if(moveInput.x < 0) _spriteRenderer.flipX = true;
        
        _animator.SetBool("IsOnRunning", true);
        _rigidbody2D.velocity = originalMoveVelocity;
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }
    void StopMove()
    {
        if(GameManager.GameState != GameState.Playing) return;
        _animator.SetBool("IsOnRunning", false);
        originalMoveVelocity = Vector2.zero;
        _rigidbody2D.velocity = originalMoveVelocity;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }

    //限制玩家移動協程
    IEnumerator MovePositionLimitCoroutine()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveblePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }

    //衝刺閃避
    void Dash()
    {
        if(GameManager.GameState != GameState.Playing) return;
        if(nowMP==0) return;
        if(isOnAttack) return;
        if(isOnDash) return;
        StartCoroutine(DashCoroutine());
        StartCoroutine(AfterimageCoroutine());
    }
    IEnumerator DashCoroutine()
    {
        _animator.SetTrigger("OnDash");
        isOnDash = true;
        nowMP --;
        headUi.InitializeMpBar(nowMP, maxMP);
        _rigidbody2D.velocity = originalMoveVelocity * dashSpeed;
        yield return dashTime;
        _rigidbody2D.velocity = originalMoveVelocity;
        yield return dashTime;
        isOnDash = false;
    }
    // 殘影協程
    IEnumerator AfterimageCoroutine()
    { 
        for (int i = 0; i < 5; i++)
        {
            if(!_spriteRenderer.flipX) PoolManager.Release(playerAfterimagePrefabR,transform.position);
            if(_spriteRenderer.flipX) PoolManager.Release(playerAfterimagePrefabL,transform.position);
            yield return new WaitForSeconds(0.05f);
        }
    }

    //持續回魔-常開
    IEnumerator RecoverMagicPointCoroutine()
    {
        while(true)
        {
            if(nowMP < maxMP)
            {   
                yield return recoverMPTime;
                nowMP++;
                headUi.InitializeMpBar(nowMP, maxMP);
            }
            yield return dashTime;
        }
    }

    void Attack()
    {
        if(GameManager.GameState != GameState.Playing) return;
        if(isOnDash) return;
        if(isOnAttack) return;
        _animator.SetTrigger("OnAttack");
        StartCoroutine(nameof(AttackCoroutine));
        isOnAttack = true;
    }
    void GetMousePos(Vector2 mousepos)
    {
        mouseInWorldPos = mousepos;
    }
    /// <summary>
    /// 攻擊協程(單次攻擊)
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine()
    {
        originalMoveVelocity = _rigidbody2D.velocity;
        _rigidbody2D.velocity = Vector2.zero;
        if(Camera.main.ScreenToWorldPoint(mouseInWorldPos).x > transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }else if(Camera.main.ScreenToWorldPoint(mouseInWorldPos).x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        
        //計算射擊方向
        shotDirection = (Camera.main.ScreenToWorldPoint(mouseInWorldPos) - transform.position).normalized;
        
        //等待滑鼠放開(後面考慮看要不要做成拉弓射擊)
        //yield return new WaitUntil(()=> Input.GetMouseButtonUp(0));

        //等待攻擊動畫時間
        yield return attackSpeedTime;
        //對象池實例化箭矢(箭矢上有自動飛行腳本)
        if(arrowAmount != 0){
            arrowList.Add(PoolManager.Release(arrowPrefab, transform.position, Quaternion.Euler(0 ,0 , Mathf.Atan2(shotDirection.y ,shotDirection.x)* Mathf.Rad2Deg)));
            arrowAmount--;
            ArrowTubeUi.arrowTubeUi.ConsumeArrow();
            AudioManager.Instance.PlaySFX(sFX_shotArrow);
        }else
        {
            headUi.ShowPhiz();
        }

        if(originalMoveVelocity.x > 0)
        {
            _spriteRenderer.flipX = false;
            _animator.SetBool("IsOnRunning", true);
        }else if(originalMoveVelocity.x < 0)
        {
            _spriteRenderer.flipX = true;
            _animator.SetBool("IsOnRunning", true);
        }       
        _rigidbody2D.velocity = originalMoveVelocity;
        isOnAttack = false;
    }

    /// <summary>
    /// 增加箭矢
    /// </summary>
    public void AddArrowAmount()
    {
        if(arrowAmount >= maxArrowAmount) return;
        ArrowTubeUi.arrowTubeUi.ReplenishArrow();
        arrowAmount ++;
    }
    
    /// <summary>
    /// 玩家受傷
    /// </summary>
    public void Injuried()
    {
        if(GameManager.GameState != GameState.Playing) return;
        if(isOnDash) return;
        if(isOnInvincible) return;
        nowHp --;
        headUi.InitializeHpBar(nowHp, maxHp);
        if(nowHp <= 0f)
        {
            Die();
            RemoveAllArrow();
        }
        StartCoroutine(SpriteRendereFlickerCoroutine());
    }
    IEnumerator SpriteRendereFlickerCoroutine()
    {        
        _spriteRenderer.color = Color.red;
        yield return dashTime;
        _spriteRenderer.color = Color.white;
    }

    //---------------------卡片效果實現----------------------------//
    /// <summary>
    /// 生命全滿
    /// </summary>
    public void HealingLife()
    {
        nowHp = maxHp;
        headUi.InitializeHpBar(nowHp, maxHp);
        _eff_HealingLife.SetActive(true);        
    }

    /// <summary>
    /// 無敵狀態
    /// </summary>
    public void Invincible(float time)
    {
        StopCoroutine(nameof(InvincibleTimeCoroutine));
        StartCoroutine(InvincibleTimeCoroutine(time));
    }
    IEnumerator InvincibleTimeCoroutine(float time)
    {
        isOnInvincible = true;
        _eff_invincible.SetActive(true);
        yield return new WaitForSeconds(time);
        _eff_invincible.SetActive(false);
        isOnInvincible = false;
    }

    //魔力全滿
    public void FullMagicPoint()
    {
        nowMP = maxMP;
        headUi.InitializeMpBar(nowMP ,maxMP);
    }

    /// <summary>
    /// 箭矢全回收+獲得2張卡
    /// </summary>
    public void RemoveAllArrow()
    {
        float size;
        size = arrowList.Count;
        for (int i = 0; i < size; i++)
        {
            arrowList[0].GetComponent<Arrow>().RemoveArrow();   
        }
        if(GameManager.GameState != GameState.Playing) return;  //死掉時回收卡片不再觸發抽卡
        SkillCardUi.Instance.ReplenishSkillCard();
        SkillCardUi.Instance.ReplenishSkillCard();
    }

    /// <summary>
    /// 箭矢超加速
    /// </summary>
    public void ArrowSpeedUp()
    {
        StopCoroutine(nameof(ArrowSpeedUpTimeCoroutine));
        StartCoroutine(nameof(ArrowSpeedUpTimeCoroutine));
    }
    IEnumerator ArrowSpeedUpTimeCoroutine()
    {
        ArrowSpeed = 3f;
        yield return arrowSpeedUpTime;
        ArrowSpeed = 1;
    }
}

