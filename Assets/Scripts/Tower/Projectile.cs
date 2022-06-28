using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private CircleCollider2D AoeRadius;
    public GameObject target;
    public Rigidbody2D rb;
    public float damageValue;
    public float projectileSpeed;
    public float attackRange;
    public float slowAmt;
    public string damageType;
    public bool isWeapon;

    void Awake()
    {
        if (isWeapon)
        {
            damageValue = transform.parent.GetComponent<Weapon>().damage;
            projectileSpeed = transform.parent.GetComponent<Weapon>().projectileSpeed;
            damageType = transform.parent.GetComponent<Weapon>().damageType;
            attackRange = transform.parent.GetComponent<Weapon>().attackRange;
            target = transform.parent.GetComponent<Weapon>().currentTarget;
            slowAmt = transform.parent.GetComponent<Weapon>().slowAmt;
        }

        else
        {
            damageValue = transform.parent.GetComponent<Tower>().damage;
            projectileSpeed = transform.parent.GetComponent<Tower>().projectileSpeed;
            damageType = transform.parent.GetComponent<Tower>().damageType;
            attackRange = transform.parent.GetComponent<Tower>().attackRange;
            target = transform.parent.GetComponent<Tower>().currentTarget;
            slowAmt = transform.parent.GetComponent<Tower>().slowAmt;
        }

        if (projectileSpeed == 0)
        {
            AoeRadius.radius = attackRange;
        }
        //Debug.Log("currentTarget target position: " + target.transform.position);
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
                else
                {
                    transform.parent.GetComponent<Tower>().attackCd = 0;
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
                else
                {
                    transform.parent.GetComponent<Tower>().attackCd = 0;
                }
            }
        }

        else if (projectileSpeed == 0)
        {
            //The projectile is destroyed after the damage value is received by the monster (See Monster.cs OnTriggerEnter2D function)
            //Need to use invoke to give time for the damage to be dealt
            Invoke("destroyProjectile", 0.1f);
        }
    }

    public void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
