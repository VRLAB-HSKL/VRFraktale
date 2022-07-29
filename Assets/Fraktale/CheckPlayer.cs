using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public Collider player;
    public GameObject canvas;

    public void Start()
    {
        if (player == null)
        {
            Debug.Log("Missing object with player task");
            player = GameObject.FindGameObjectWithTag("player").GetComponent<Collider>();
        }
    }


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
