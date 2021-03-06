using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    private readonly Color DefaultColor = new Color(1, 1, 1, 1);
    private readonly Color UnbreakableColor = new Color(1, 1, 1, 0.5f);

    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    private const float AttackDelayTime = 0.1f;
    public const float UnbreakableTime = 3f;
    public const float DamageUpTime = 5f;
    private float _timer;
    public float RemainDamageUpTime { get; set; }

    private Coroutine _unbreakableCoroutine;
    
    #region UnbreakableRoutine
    
    private void Update()
    {
        if (!GameManager.Instance.IsForcedUnbreakableChanged) return;

        GameManager.Instance.IsForcedUnbreakableChanged = false;

        GameManager.Instance.IsUnbreakable = false;
        if (_unbreakableCoroutine != null)
        {
            StopCoroutine(_unbreakableCoroutine);
        }

        if (GameManager.Instance.ForcedUnbreakable)
        {
            playerSpriteRenderer.color = UnbreakableColor;
        }
        else
        {
            playerSpriteRenderer.color = DefaultColor;
        }
    }

    public void GetUnbreakableItem()
    {
        SetUnbreakableMode(UnbreakableTime);
    }
    
    public void SetUnbreakableMode(float time)
    {
        if (GameManager.Instance.ForcedUnbreakable)
        {
            return;
        }
        if (GameManager.Instance.IsUnbreakable)
        {
            StopCoroutine(_unbreakableCoroutine);
        }

        _unbreakableCoroutine = StartCoroutine(UnbreakableRoutine(time));
    }

    private IEnumerator UnbreakableRoutine(float time)
    {
        GameManager.Instance.IsUnbreakable = true;
        playerSpriteRenderer.color = UnbreakableColor;
        
        yield return new WaitForSeconds(time - 0.5f);
        
        playerSpriteRenderer.color = DefaultColor;
        
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.IsUnbreakable = false;
    }
    
    #endregion

    #region DamageUpRoutine
    public void GetDamageUpItem()
    {
        if (RemainDamageUpTime > 0)
        {
            RemainDamageUpTime += DamageUpTime;
            return;
        }

        StartCoroutine(DamageUpRoutine());
    }

    private IEnumerator DamageUpRoutine()
    {
        Damage += 3;
        RemainDamageUpTime += DamageUpTime;
        
        while (RemainDamageUpTime > 0)
        {
            var waitTime = RemainDamageUpTime;
            yield return new WaitForSeconds(RemainDamageUpTime);
            RemainDamageUpTime -= waitTime;
        }

        RemainDamageUpTime = 0;
        Damage -= 3;
    }
    
    #endregion
    
    void Start()
    {
        InitializeBaseData();
        StartCoroutine(AttackRoutine());
    }
    
    public override void InitializeBaseData()
    {
        GameManager.Instance.Power = 1;
        Speed = 0.2f;
        Damage = 1;
        BulletType = PoolCode.PlayerBullet;
    }
    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * Speed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * Speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed);
        }
    }
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Attack();
                yield return new WaitForSeconds(AttackDelayTime);
            }

            yield return null;
        }
    }

    protected override void Attack()
    {
        if (GameManager.Instance.Power <= 1)
        {
            SingleShoot();
            return;
        }
        MultiShoot();
    }

    private void SingleShoot()
    {
        var bullet = PoolManager.Instance.CreatPrefab(BulletType);
        Shoot(bullet, transform.position);
    }
    
    private void MultiShoot()
    {
        // Player ??????, ?????? ???????????? ????????? ?????? ?????? ?????????
        // ??? ?????? ????????? 0.125??? ?????????
        // 0.125??? ??????????????? ???????????? ???????????? *8??? 1??? ???????????? ?????????
        // ????????? ????????? ????????? normalize????????? ??????
        
        const float n = 8f; // ?????? ????????? ??????
        const int d = 2;    // ????????? ??????
        var position = 1 - GameManager.Instance.Power;   // ?????? ??????
        
        for (var i = 0; i < GameManager.Instance.Power; i++)
        {
            var setPosition = new Vector3(position / n , 0, 0);
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            Shoot(bullet, transform.position + setPosition);
            position += d;
        }
    }
    
    private void Shoot(GameObject bullet, Vector3 position)
    {        
        var bulletScript = bullet.GetComponent<PlayerBullet>();
        bulletScript.BulletDamage = Damage;
        bulletScript.SetBulletColor();
        bulletScript.BulletSpeed = 0.2f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
    }
    
    public override void Damaged(float damage)
    {
        Debug.LogError("Do Not Use Damage(float damage) method in Player. Please use Player.Damaged(float damage, int damageType)");
        throw new NotImplementedException();
    }

    public void Damaged(float damage, DamageType damageType)
    {
        if (damageType == DamageType.Pain)
        {
            GameManager.Instance.Damaged(damage, DamageType.Pain);
            return;
        }

        if (GameManager.Instance.ForcedUnbreakable) { return; }
        if (GameManager.Instance.IsUnbreakable) { return; }
        
        SetUnbreakableMode(UnbreakableTime);
        GameManager.Instance.Damaged(damage, DamageType.Hp);
        if (GameManager.Instance.Hp <= 0)
        {
            Killed();
        }
    }

    public override void Killed()
    {
        return;
    }
    
    protected override void OnBecameInvisible() { return; }
}
