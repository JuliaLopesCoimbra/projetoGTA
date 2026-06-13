using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePersonagem : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    public float attackRadius = 20f;
    private Animator heroi;
    private bool temTaco = false;

    [Header("Sons")]
    public AudioClip somGolpe;
    public AudioClip somPegarTaco;
    private AudioSource audioSource;

    void Start()
    {
        speed = 5.0f;
        heroi = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            heroi.SetBool("Correndo", true);
            heroi.SetBool("Parado", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
            heroi.SetBool("Correndo", true);
            heroi.SetBool("Parado", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0);
            heroi.SetBool("Correndo", true);
            heroi.SetBool("Parado", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0);
            heroi.SetBool("Correndo", true);
            heroi.SetBool("Parado", false);
        }
        if (!Input.anyKey)
        {
            heroi.SetBool("Correndo", false);
            heroi.SetBool("Parado", true);
            heroi.SetBool("Atacando", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && temTaco)
        {
            heroi.SetBool("Atacando", true);
            heroi.SetBool("Correndo", false);
            heroi.SetBool("Parado", false);
            AtacarZumbis();

            if (somGolpe != null)
                audioSource.PlayOneShot(somGolpe);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            heroi.SetBool("Atacando", false);
        }
    }

    void AtacarZumbis()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (Collider col in colliders)
        {
            ZombieVida vida = col.gameObject.GetComponentInParent<ZombieVida>();
            if (vida != null)
                vida.TomarDano();
        }
    }

    public void PegouTaco()
    {
        temTaco = true;
        if (somPegarTaco != null)
            audioSource.PlayOneShot(somPegarTaco);
        Debug.Log("Personagem pegou o taco!");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}