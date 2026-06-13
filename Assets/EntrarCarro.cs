using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntrarCarro : MonoBehaviour
{
    [Header("Referências")]
    public GameObject personagem;
    public Camera cameraCarro;
    public Camera cameraPersonagem;
    public GameObject textEntrarCarro;

    [Header("Configurações")]
    public float distanciaEntrar = 5f;

    private bool dentroDoRaio = false;
    private bool dentroDoCarro = false;
    private Carro scriptCarro;

    void Start()
    {
        scriptCarro = GetComponent<Carro>();
    }

    void Update()
    {
        if (personagem == null) return;

        float dist = Vector3.Distance(transform.position, personagem.transform.position);

        if (dist <= distanciaEntrar && !dentroDoCarro)
        {
            dentroDoRaio = true;
            if (textEntrarCarro != null)
                textEntrarCarro.SetActive(true);
        }
        else if (!dentroDoCarro)
        {
            dentroDoRaio = false;
            if (textEntrarCarro != null)
                textEntrarCarro.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dentroDoRaio && !dentroDoCarro)
                EntrarNoCarro();
            else if (dentroDoCarro)
                SairDoCarro();
        }
    }

    void EntrarNoCarro()
    {
        dentroDoCarro = true;
        dentroDoRaio = false;

        personagem.SetActive(false);

        if (cameraPersonagem != null) cameraPersonagem.gameObject.SetActive(false);
        if (cameraCarro != null) cameraCarro.gameObject.SetActive(true);

        if (scriptCarro != null) scriptCarro.enabled = true;

        if (textEntrarCarro != null) textEntrarCarro.SetActive(false);

        Debug.Log("Entrou no carro!");
    }

    void SairDoCarro()
    {
        dentroDoCarro = false;

        personagem.SetActive(true);
        personagem.transform.position = transform.position + transform.right * 3f;

        if (cameraCarro != null) cameraCarro.gameObject.SetActive(false);
        if (cameraPersonagem != null) cameraPersonagem.gameObject.SetActive(true);

        if (scriptCarro != null) scriptCarro.enabled = false;

        Debug.Log("Saiu do carro!");
    }
}