using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPath : MonoBehaviour
{
    [Header("Waypoints (definidos pelo Spawner ou no Inspector)")]
    public Vector3[] waypoints;

    [Header("Configurações")]
    public float movementSpeed = 0.5f;
    public float rotationSpeed = 5f;
    public float waypointRadius = 0.6f;

    private int currentWaypoint = 0;
    private Animator personAnim;
    private Rigidbody rb;
    private CharacterController cc;

    void Start()
    {
        personAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();

        // Se tiver Rigidbody, congela rotação para não tombar
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Vector3 target = waypoints[currentWaypoint];
        Vector3 dir = target - transform.position;
        dir.y = 0;

        float dist = dir.magnitude;

        // Chegou no waypoint — vai para o próximo
        if (dist <= waypointRadius)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            return;
        }

        // Rotação suave
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                                                     rotationSpeed * Time.deltaTime);
        }

        Vector3 move = dir.normalized * movementSpeed * Time.deltaTime;

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

        // Animação
        if (personAnim != null)
        {
            personAnim.SetBool("runPerson", movementSpeed > 1f);
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i], 0.25f);
            Gizmos.DrawLine(waypoints[i], waypoints[(i + 1) % waypoints.Length]);
        }
    }
}