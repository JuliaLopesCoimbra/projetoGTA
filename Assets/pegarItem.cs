using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Personagem")
        {
            Transform mao = other.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/R_hand_container");
            if (mao != null)
            {
                transform.SetParent(mao);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            Destroy(GetComponent<FloatingUpDown>());
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            other.GetComponent<ControlePersonagem>().PegouTaco(); // ← aqui!
        }
    }
}