using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carro : MonoBehaviour
{
    [SerializeField] WheelCollider RodaTraseiraDireita;
    [SerializeField] WheelCollider RodaFrenteDireita;
    [SerializeField] WheelCollider RodaFrenteEsquerda;
    [SerializeField] WheelCollider RodaTraseiraEsquerda;

    public float aceleracao = 4000f;
    public float freio = 300f;
    public float anguloTorque = 30f;

    private float aceleracaoAtual = 0f;
    private float freioAtual = 0f;
    private float anguloTorqueAtual = 0f;

    public Light luzDirecional;
    private bool isNoite = false;

    public Light farolEsquerdo;
    public Light farolDireito;
    private bool farolAtivo = false;
    public Light luzFreio;
    private Vector3 cam1Pos = new Vector3(0f, 1.5f, 5f);
    private Vector3 cam1Rot = new Vector3(10f, 180f, 0f);

    private Vector3 cam2Pos = new Vector3(0f, 0.7f, 0.8f);
    private Vector3 cam2Rot = new Vector3(0f, 180f, 0f);
    private Vector3 cam3Pos = new Vector3(2f, 0.3f, 2f);
    private Vector3 cam3Rot = new Vector3(0f, -90f, 0f);

    public Camera mainCamera;
    private void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        aceleracaoAtual = aceleracao * vertical;
        RodaFrenteDireita.motorTorque = aceleracaoAtual;
        RodaFrenteEsquerda.motorTorque = aceleracaoAtual;

        anguloTorqueAtual = anguloTorque * horizontal;
        RodaFrenteDireita.steerAngle = anguloTorqueAtual;
        RodaFrenteEsquerda.steerAngle = anguloTorqueAtual;

        if (Input.GetKey(KeyCode.Space))
        {
            freioAtual = freio;
        }
        else
        {
            freioAtual = 0f;
        }

        RodaFrenteDireita.brakeTorque = freioAtual;
        RodaFrenteEsquerda.brakeTorque = freioAtual;
        RodaTraseiraDireita.brakeTorque = freioAtual;
        RodaTraseiraEsquerda.brakeTorque = freioAtual;
        if (Input.GetKey(KeyCode.Space))
        {
            freioAtual = freio;
            luzFreio.enabled = true;
        }
        else
        {
            freioAtual = 0f;
            luzFreio.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isNoite = !isNoite;
            if (isNoite)
            {
                luzDirecional.intensity = 0f;
                RenderSettings.ambientIntensity = 0f;
            }
            else
            {
                luzDirecional.intensity = 1f;
                RenderSettings.ambientIntensity = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            farolAtivo = !farolAtivo;
            farolEsquerdo.enabled = farolAtivo;
            farolDireito.enabled = farolAtivo;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W pressionado!");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.transform.localPosition = cam1Pos;
            mainCamera.transform.localEulerAngles = cam1Rot;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainCamera.transform.localPosition = cam2Pos;
            mainCamera.transform.localEulerAngles = cam2Rot;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mainCamera.transform.localPosition = cam3Pos;
            mainCamera.transform.localEulerAngles = cam3Rot;
        }
    }
    private void Start()
    {
        luzFreio.enabled = false;
        farolEsquerdo.enabled = false;
        farolDireito.enabled = false;
    }

}