using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallController : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.Init();
    }
}
