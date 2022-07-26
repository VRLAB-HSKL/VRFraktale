using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public Collider player;
    public GameObject canvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other == player)
        {
            canvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == player)
        {
            canvas.SetActive(false);
        }
    }
}
