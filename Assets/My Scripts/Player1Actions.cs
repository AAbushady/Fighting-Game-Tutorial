using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Actions : MonoBehaviour
{
    public float JumpSpeed = 1f;
    public GameObject Player1;

    public void JumpUp()
    {
        Player1.transform.Translate(0, JumpSpeed, 0);
    }
}
