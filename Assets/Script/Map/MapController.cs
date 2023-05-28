using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform roadTranform;
    //public Transform obstacle;

    private Transform characterTranform;
    private int spaceMap = 50;
    //private int spaceObstacle = 10;

    private void Awake()
    {
        characterTranform = GameController.Instance.characterGameObject.transform;
    }

    private void Start()
    {

    }

    public void Create()
    {
        stepRoad = 0;
        roadTranform.ForChild((i, _child) =>
        {
            _child.transform.localPosition = Vector3.zero.AddZ(20 + i * spaceMap);
        });
    }

    private void Update()
    {
        if (GameController.gameState == GameState.Pause)
            return;
        UpdateRoad();
    }

    private int stepRoad = 0;
    private void UpdateRoad()
    {
        //this.transform.position -= Vector3.forward * 5 * Time.deltaTime;

        var _stepRoad = (int)characterTranform.position.z / spaceMap;
        if (stepRoad != _stepRoad)
        {
            stepRoad = _stepRoad;
            var firstMap = roadTranform.GetChild(0);
            var lastMap = roadTranform.GetChild(this.transform.childCount - 1);
            firstMap.SetAsLastSibling();
            firstMap.position = lastMap.position.AddZ(spaceMap);
        }
    }
}
