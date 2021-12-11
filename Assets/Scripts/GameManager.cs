using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool battleIsActive;
    [SerializeField] PlayerStats[] playerStats;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        playerStats = FindObjectsOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleIsActive)
        {
            Player.instance.deactivateMovement = true;
        }
        else
        {
            Player.instance.deactivateMovement = false;
        }
    }

    public PlayerStats[] GetPlayerStats()
    {
        return playerStats;
    }
}
