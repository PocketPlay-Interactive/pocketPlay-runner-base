using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public int Score = 0;
    private int skinIndex = 0;
    private void Awake()
    {

    }

    private void Start()
    {
        this.transform.ForChild((i, _child) =>
        {
            if (i == skinIndex)
                _child.SetActive(true);
            else
                _child.SetActive(false);
        });
    }

    private void OnEnable()
    {
        GameEvent.OnChangeSkin += OnChangeSkin;
    }

    private void OnDisable()
    {
        GameEvent.OnChangeSkin -= OnChangeSkin;
    }

    private void OnChangeSkin(int director)
    {
        var nowSkin = this.transform.GetChild(skinIndex);
        var newSkinIndex = skinIndex + director;
        Transform newSkin = null;
        switch (director)
        {
            case -1:
                if (skinIndex == 0)
                    newSkinIndex = this.transform.childCount - 1;
                newSkin = this.transform.GetChild(newSkinIndex);
                newSkin.localPosition = newSkin.localPosition.WithX(-3);
                newSkin.SetActive(true);
                nowSkin.DOKill();
                nowSkin.DOLocalMoveX(3, 0.25f).OnComplete(() => nowSkin.SetActive(false));
                newSkin.DOKill();
                newSkin.DOLocalMoveX(0, 0.25f).OnComplete(() => skinIndex = newSkinIndex);
                break;
            case 1:
                if (skinIndex == this.transform.childCount - 1)
                    newSkinIndex = 0;
                newSkin = this.transform.GetChild(newSkinIndex);
                newSkin.localPosition = newSkin.localPosition.WithX(3);
                newSkin.SetActive(true);
                nowSkin.DOKill();
                nowSkin.DOLocalMoveX(-3, 0.25f).OnComplete(() => nowSkin.SetActive(false));
                newSkin.DOKill();
                newSkin.DOLocalMoveX(0, 0.25f).OnComplete(() => skinIndex = newSkinIndex);
                break;
        }    
    }    

    public void Create()
    {
        Score = 0;

        isJump = false;
        isSlide = false;

        this.transform.position = Vector3.up;
        this.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        Score = (int)(this.transform.position.z / 10);
        OnUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.name == "Dead")
        {
            GameController.Instance.OnDead();
        }
    }
}
