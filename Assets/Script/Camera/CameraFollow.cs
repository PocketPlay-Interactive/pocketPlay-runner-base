using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform characterTranform;
    private Vector3 cameraOffset;
    public float smoothSpeed = 0.1f;
    private void Awake()
    {
        characterTranform = GameController.Instance.characterGameObject.transform;
        cameraOffset = this.transform.position - characterTranform.position;
    }

    public void Create()
    {
        transform.position = characterTranform.position + cameraOffset;
    }

    void Update()
    {
        Vector3 targetPos = characterTranform.position + cameraOffset;
        transform.position = Vector3.MoveTowards(this.transform.position, targetPos.WithY(this.transform.position.y), smoothSpeed * Time.deltaTime);
    }
}
