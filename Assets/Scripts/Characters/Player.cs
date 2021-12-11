using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    [SerializeField] float moveSpeed = 2f;

    [SerializeField] Rigidbody2D playerRigidBody;
    [SerializeField] Animator playerAnimator;


    public string transitionName;
    private Vector3 bottomLeftEdge;
    private Vector3 topRightEdge;

    public bool deactivateMovement;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (deactivateMovement) { return; }
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        playerRigidBody.velocity = new Vector2(horizontalMovement, verticalMovement) * moveSpeed;

        
            playerAnimator.SetFloat("movementX", playerRigidBody.velocity.x);
            playerAnimator.SetFloat("movementY", playerRigidBody.velocity.y);

            if (horizontalMovement == 1 || horizontalMovement == -1 || verticalMovement == 1 || verticalMovement == -1)
            {
                playerAnimator.SetFloat("lastX", horizontalMovement);
                playerAnimator.SetFloat("lastY", verticalMovement);
            }

        float clampX = Mathf.Clamp(transform.position.x, bottomLeftEdge.x, topRightEdge.x);
        float clampY = Mathf.Clamp(transform.position.y, bottomLeftEdge.y, topRightEdge.y);
        float clampZ = Mathf.Clamp(transform.position.z, bottomLeftEdge.z, topRightEdge.z);

        transform.position = new Vector3(clampX, clampY, clampZ);
    }

    public void SetLimit(Vector3 bottomEdgeToSet, Vector3 topEdgeToSet)
    {
        bottomLeftEdge = bottomEdgeToSet;
        topRightEdge = topEdgeToSet;
    }
}
