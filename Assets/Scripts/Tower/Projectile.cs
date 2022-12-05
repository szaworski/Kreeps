using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private CircleCollider2D AoeRadius;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public GameObject target;
    public Rigidbody2D rb;
    public float damageValue;
    public float bonusHpDamage;
    public float bonusArmorDamage;
    public float projectileSpeed;
    public float attackRange;
    public float slowAmt;
    public float critChance;
    public string damageType;
    public bool isWeapon;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isWeapon)
        {
            damageValue = transform.parent.GetComponent<Weapon>().damage;
            bonusArmorDamage = transform.parent.GetComponent<Weapon>().bonusArmorDmg;
            projectileSpeed = transform.parent.GetComponent<Weapon>().projectileSpeed;
            damageType = transform.parent.GetComponent<Weapon>().damageType;
            attackRange = transform.parent.GetComponent<Weapon>().attackRange;
            target = transform.parent.GetComponent<Weapon>().currentTarget;
            slowAmt = transform.parent.GetComponent<Weapon>().slowAmt;
            critChance = transform.parent.GetComponent<Weapon>().critChance;

            if (projectileSpeed == 0)
            {
                AoeRadius.radius = attackRange;
            }
        }

        else
        {
            damageValue = transform.parent.GetComponent<Tower>().damage;
            bonusArmorDamage = transform.parent.GetComponent<Tower>().bonusArmorDmg;
            projectileSpeed = transform.parent.GetComponent<Tower>().projectileSpeed;
            damageType = transform.parent.GetComponent<Tower>().damageType;
            attackRange = transform.parent.GetComponent<Tower>().attackRange;
            target = transform.parent.GetComponent<Tower>().currentTarget;
            slowAmt = transform.parent.GetComponent<Tower>().slowAmt;
            critChance = transform.parent.GetComponent<Tower>().critChance;
        }

        //Debug.Log("currentTarget target position: " + target.transform.position);
    }

    void Start()
    {
        if (projectileSpeed != 0)
        {
            if (target.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            }

            Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
    }

    void Update()
    {
        MoveProjectile();
    }

    public void MoveProjectile()
    {
        if (projectileSpeed == 1)
        {
            if (target != null)
            {
                //Teleport the projectile on top of the target
                transform.position = target.transform.position;
            }

            else
            {
                //If the target is not null, The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
                DestroyProjectile();

                if (isWeapon)
                {
                    transform.parent.GetComponent<Weapon>().attackCd = 0;
                }
            }
        }

        else if (projectileSpeed > 1)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
            }

            else
            {
                //If the target is not null, The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
                DestroyProjectile();

                if (isWeapon)
                {
                    transform.parent.GetComponent<Weapon>().attackCd = 0;
                }
            }
        }

        else if (projectileSpeed == 0)
        {
            //The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
            //Invoke("destroyProjectile", 0.01f);
            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
