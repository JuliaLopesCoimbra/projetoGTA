using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacoAtaque : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Taco colidiu com: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (other.gameObject.tag == "Zumbi")
        {
            ZombieVida vida = other.GetComponent<ZombieVida>();
            if (vida != null)
            {
                vida.TomarDano();
                Debug.Log("Dano causado!");
            }
            else
            {
                Debug.Log("ZombieVida nao encontrado!");
            }
        }
    }
}