using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieRadar : MonoBehaviour
{
    private Transform target;
    public float _range = 10f;
    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float attackRange = 1.5f;
    private Animator zombieAnim;
    private Rigidbody rb;

    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        GameObject[] pessoas = GameObject.FindGameObjectsWithTag("Personagem");
        if (pessoas.Length == 0) return;

        GameObject maisProximo = null;
        float menorDist = Mathf.Infinity;
        foreach (var p in pessoas)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);
            if (d < menorDist)
            {
                menorDist = d;
                maisProximo = p;
            }
        }

        if (maisProximo == null) return;

        if (menorDist <= _range)
        {
            target = maisProximo.transform;
            Vector3 direction = target.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime);
            }

            float dist = Vector3.Distance(
                new Vector3(target.position.x, 0, target.position.z),
                new Vector3(transform.position.x, 0, transform.position.z));

            if (dist <= attackRange)
            {
                zombieAnim.SetBool("runZombie", false);
                zombieAnim.SetBool("idleZombie", false);
                zombieAnim.SetBool("attackZombie", true);
                rb.linearVelocity = Vector3.zero;

          
            }
            else
            {
                zombieAnim.SetBool("runZombie", true);
                zombieAnim.SetBool("idleZombie", false);
                zombieAnim.SetBool("attackZombie", false);
                rb.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);

             

                followPath fp = maisProximo.GetComponent<followPath>();
                if (fp != null)
                    fp.movementSpeed = Random.Range(1f, 4f);
            }
        }
        else
        {
            if (zombieAnim != null)
            {
                zombieAnim.SetBool("runZombie", false);
                zombieAnim.SetBool("idleZombie", true);
                zombieAnim.SetBool("attackZombie", false);
            }
            rb.linearVelocity = Vector3.zero;
            target = null;
        }
    }
}