using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePatrol : MonoBehaviour
{
    [Header("Waypoints (definidos pelo Spawner)")]
    public Vector3[] waypoints;

    [Header("Configurações")]
    public float patrolSpeed = 0.8f;
    public float rotationSpeed = 3f;
    public float waypointRadius = 0.8f;
    public float waitAtPoint = 1.5f;

    private int currentWaypoint = 0;
    private bool isChasing = false;
    private bool isWaiting = false;
    private Animator anim;
    private Rigidbody rb;
    private CharacterController cc;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Update()
    {
        if (isChasing || waypoints == null || waypoints.Length == 0 || isWaiting) return;
        PatrolStep();
    }

    void PatrolStep()
    {
        Vector3 target = waypoints[currentWaypoint];
        Vector3 dir = target - transform.position;
        dir.y = 0;

        if (dir.magnitude <= waypointRadius)
        {
            StartCoroutine(WaitAndNext());
            return;
        }

        // Rotação suave
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                rotationSpeed * Time.deltaTime);
        }

        Vector3 move = dir.normalized * patrolSpeed * Time.deltaTime;

        // Move usando o componente disponível
        if (cc != null && cc.enabled)
        {
            cc.Move(move);
        }
        else if (rb != null)
        {
            rb.MovePosition(transform.position + move);
        }
        else
        {
            transform.position += move;
        }

        // Animação de andar
        if (anim != null)
        {
            anim.SetBool("idleZombie", false);
            anim.SetBool("attackZombie", false);
            anim.SetBool("runZombie", true);
        }
    }

    IEnumerator WaitAndNext()
    {
        isWaiting = true;

        // Para o movimento
        if (rb != null) rb.linearVelocity = Vector3.zero;

        if (anim != null)
        {
            anim.SetBool("runZombie", false);
            anim.SetBool("idleZombie", true);
        }

        yield return new WaitForSeconds(waitAtPoint);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        isWaiting = false;
    }

    public void StartChasing()
    {
        isChasing = true;
        StopAllCoroutines();
        isWaiting = false;
    }

    public void StopChasing()
    {
        isChasing = false;
    }
}