using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    private Player playerTarget;
    CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerTarget = FindObjectOfType<Player>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = playerTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
