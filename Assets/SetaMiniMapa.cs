using UnityEngine;
using UnityEngine.UI;

public class SetaMiniMapa : MonoBehaviour
{
    public Transform jogador;
    public float escala = 0.5f;

    private RectTransform retTransform;

    void Start()
    {
        retTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Posição no mini-mapa
        retTransform.anchoredPosition = new Vector2(
            jogador.position.x * escala,
            jogador.position.z * escala
        );

        // Rotação acompanha o jogador
        retTransform.localEulerAngles = new Vector3(
            0, 0, -jogador.eulerAngles.y
        );
    }
}