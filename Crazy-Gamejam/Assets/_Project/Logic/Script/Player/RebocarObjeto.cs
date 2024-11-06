using UnityEngine;

public class RebocarObjeto : MonoBehaviour
{
    public Rigidbody objetoParaRebocar; // arraste o objeto a ser rebocado aqui

    private FixedJoint joint;

    void Start()
    {
        // Adiciona a FixedJoint ao caminh�o
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = objetoParaRebocar;
        joint.autoConfigureConnectedAnchor = false;

        // Configura os pontos de ancoragem, se necess�rio
        joint.anchor = Vector3.zero; // ponto de ancoragem no caminh�o
        joint.connectedAnchor = Vector3.zero; // ponto de ancoragem no objeto rebocado
    }
}
