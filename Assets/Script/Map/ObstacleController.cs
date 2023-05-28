using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Transform obstacleTranform;

    private Transform characterTranform;
    private int spaceObstacle = 10;

    public GameObject prefabsObstacle;

    public float[] verticalPostios = new float[] { -1.5f, 0, 1.5f };
    private float[] obstacleHigh = new float[] { 0f, 1.5f };

    private void Awake()
    {
        characterTranform = GameController.Instance.characterGameObject.transform;
    }

    private void Start()
    {

    }

    public void Create()
    {
        obstacleTranform.ForChild((_child) =>
        {
            PoolByID.Instance.PushToPool(_child.gameObject);
        });

        stepObstacle = 0;
        for (int i = 0; i < 5; i++)
        {
            var _obsObject = PoolByID.Instance.GetPrefab(prefabsObstacle);
            _obsObject.transform.parent = obstacleTranform;
            _obsObject.transform.localPosition = Vector3.zero.WithX(verticalPostios[Random.Range(0, verticalPostios.Length)]).WithY(obstacleHigh[Random.Range(0, obstacleHigh.Length)]).AddZ(30 + i * spaceObstacle);
            _obsObject.transform.SetAsLastSibling();
        }
    }

    private void Update()
    {
        if (GameController.gameState == GameState.Pause)
            return;
        UpdateObstacle();
    }

    private int stepObstacle = 0;
    private void UpdateObstacle()
    {
        var _stepObstacle = (int)characterTranform.position.z / spaceObstacle;
        if (stepObstacle != _stepObstacle)
        {
            stepObstacle = _stepObstacle;
            var lastObs = obstacleTranform.GetChild(obstacleTranform.childCount - 1);
            var _obsObject = PoolByID.Instance.GetPrefab(prefabsObstacle);
            _obsObject.transform.parent = obstacleTranform;
            _obsObject.transform.localPosition = lastObs.localPosition.WithX(verticalPostios[Random.Range(0, verticalPostios.Length)]).WithY(obstacleHigh[Random.Range(0, obstacleHigh.Length)]).AddZ(spaceObstacle);
            _obsObject.transform.SetAsLastSibling();
            var firstObj = obstacleTranform.GetChild(0);
            if (characterTranform.position.z - firstObj.position.z >= spaceObstacle)
                PoolByID.Instance.PushToPool(firstObj.gameObject);
        }
    }
}
