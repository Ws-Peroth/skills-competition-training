using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    [SerializeField] private Rigidbody2D playerRigidBody;
    public int power;
    public bool isUnbreakable;
    public float unbreakableTime;
    private const float AttackDelayTime = 0.1f;
    private float _timer;

    void Start()
    {
        InitializeBaseData();
        StartCoroutine(AttackRoutine());
    }
    
    public override void InitializeBaseData()
    {
        power = 1;
        // Hp = 100;
        Speed = 0.2f;
        Damage = 1;
        BulletType = PoolCode.PlayerBullet;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
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

    protected override void Attack()
    {
        if (power <= 1)
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
        // Player 크기, 또는 소환시킬 위치에 따라 값이 변경됨
        // 각 탄의 간격을 0.125로 하였다
        // 0.125를 사용하기엔 가독성이 나빠져서 *8로 1로 만들어서 계산함
        // 계산한 만큼을 나눠줄 normalize변수를 이용
        
        const float n = 8f; // 단위 일반화 변수
        const int d = 2;    // 위치의 공차
        var position = 1 - power;   // 초기 위치
        
        for (var i = 0; i < power; i++)
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
        bulletScript.BulletDamage = 1;
        bulletScript.BulletSpeed = 0.2f;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
    }

    public override void Damaged(float damage)
    {
        Debug.LogError("Do Not Use Damage(float damage) method in Player. Please use GameManager.Instance.Damaged(float damage, DamageType damageType)");
        throw new NotImplementedException();
    }
    
    protected override void OnBecameInvisible() { return; }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            var item = col.GetComponent<Item>();
            item.Effect(this);
        }
    }
}
