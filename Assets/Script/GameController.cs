using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public static GameState gameState = GameState.Pause;
    
    public GameObject characterGameObject;
    private PlayerController playerController;
    public int GetScore()
    {
        return playerController.Score;
    }    

    public GameObject cameraGameObject;
    private CameraFollow cameraFollow;

    public GameObject mapGameObject;
    MapController mapController;
    ObstacleController obstacleController;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;

        playerController = characterGameObject.GetComponent<PlayerController>();
        cameraFollow = cameraGameObject.GetComponent<CameraFollow>();
        mapController = mapGameObject.GetComponent<MapController>();
        obstacleController = mapGameObject.GetComponent<ObstacleController>();
    }

    private void Start()
    {
        CreateNewGame();
    }

    public void CreateNewGame()
    {
        playerController.Create();
        cameraFollow.Create();

        mapGameObject.transform.position = Vector3.zero;
        mapController.Create();
        obstacleController.Create();

        UIHelper.GetScript<HomeUICanvas>().Show();
    }

    public void OnDead()
    {
        GameController.gameState = GameState.Pause;
        UIHelper.FindScript<LoseUICanvas>().Show();
    }
}
