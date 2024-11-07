using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DirectionalLightChange : MonoBehaviour
{
    [SerializeField] float speedRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("rotaciona");
    }

    IEnumerator rotaciona()
    {
        yield return new WaitForSeconds(0);
        transform.DORotate(transform.localRotation.eulerAngles + new Vector3(0, speedRotation, 0), speedRotation);

        StartCoroutine("rotaciona");
    }
}
