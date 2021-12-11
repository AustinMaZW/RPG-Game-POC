using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Tilemap tileMap;

    private Vector3 bottomLeftEdge;
    private Vector3 topRightEdge;

    // Start is called before the first frame update
    void Start()
    {
        bottomLeftEdge = tileMap.localBounds.min + new Vector3(1f, 1f, 0f);
        topRightEdge = tileMap.localBounds.max + new Vector3(-1f, -1f, 0f);

        Player.instance.SetLimit(bottomLeftEdge, topRightEdge);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
