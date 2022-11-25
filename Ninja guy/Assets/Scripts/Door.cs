using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)//if the player is coming from the left go to next room
                cam.MoveToNewRoom(nextRoom);
            else//otherwise, player is coming from the right so go to the previous room
                cam.MoveToNewRoom(previousRoom);
        }
    }
}
