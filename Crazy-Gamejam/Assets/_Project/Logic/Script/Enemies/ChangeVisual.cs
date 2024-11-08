using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class ChangeVisual : MonoBehaviour
{
    [SerializeField] public GameObject[] models;
    
    public MMF_Player selectVisual()
    {
        var index = Random.Range(0, models.Length);
        var model = models[index];
        MMF_Player player;
        model.SetActive(false);

        if(index == 0)
        {
            player = models[1].GetComponentInChildren<MMF_Player>();
        }
        else
        {
            player = models[0].GetComponentInChildren<MMF_Player>();
        }

        return player;
    }
}
