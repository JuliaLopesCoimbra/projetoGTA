using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVida : MonoBehaviour
{
    public int vida = 3;
    public AudioClip somMorte;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TomarDano()
    {
        vida--;
        if (vida <= 0)
            StartCoroutine(Morrer());
    }

    IEnumerator Morrer()
    {
        if (anim != null)
        {
            anim.SetBool("runZombie", false);
            anim.SetBool("idleZombie", false);
            anim.SetBool("attackZombie", false);
            anim.SetBool("deathZombie", true);
        }

        zombieRadar radar = GetComponent<zombieRadar>();
        if (radar != null) radar.enabled = false;

        ZombiePatrol patrol = GetComponent<ZombiePatrol>();
        if (patrol != null) patrol.enabled = false;

        // Toca o som antes de destruir
        if (somMorte != null)
        {
            AudioSource.PlayClipAtPoint(somMorte, transform.position);
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}