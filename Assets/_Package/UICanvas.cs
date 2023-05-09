using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public enum TypeCanvas
    {
        Screen,
        Popup,
        Scroll,
        Animation
    }

    [Header("---- BASE -----")]
    public TypeCanvas typeCanvas = TypeCanvas.Screen;
    public Transform employee;
    float showTime = 0.6f, hideTime = 0.3f;
    Ease showEase = Ease.OutBack, hideEase = Ease.InBack;

    public Animator animator;
    public string openId;
    public string closeId;
    [Header("---- PROTECHED -----")]
    public float baseTimer;

    public virtual void Show()
    {
        switch(typeCanvas)
        {
            case TypeCanvas.Screen:
                this.gameObject.SetActive(true);
                break;
            case TypeCanvas.Popup:
                this.employee.localScale = Vector3.one;
                this.gameObject.SetActive(true);
                this.employee.DOKill();
                this.employee.DOPunchScale(Vector3.one * 0.2f, showTime, 1, 1f).SetEase(showEase);
                break;
            case TypeCanvas.Scroll:
                this.gameObject.SetActive(true);
                var rectEmployee = this.employee.GetComponent<RectTransform>();
                rectEmployee.anchoredPosition = rectEmployee.anchoredPosition.WithX(1000);
                rectEmployee.DOAnchorPos(Vector2.zero, 0.75f).OnComplete(() => { });
                break;
            case TypeCanvas.Animation:
                this.gameObject.SetActive(true);
                this.animator.speed = 1;
                this.animator.Play(openId);
                break;
        }
    }

    public virtual void Hide()
    {
        switch (typeCanvas)
        {
            case TypeCanvas.Screen:
                this.gameObject.SetActive(false);
                break;
            case TypeCanvas.Popup:
                this.employee.DOKill();
                this.employee.DOScale(0, hideTime).SetEase(hideEase).OnComplete(() => { this.gameObject.SetActive(false); });
                break;
            case TypeCanvas.Scroll:
                var rectEmployee = this.employee.GetComponent<RectTransform>();
                rectEmployee.DOAnchorPosX(-1000, 0.75f).OnComplete(() => { this.gameObject.SetActive(false); });
                break;
            case TypeCanvas.Animation:
                this.animator.speed = 1;
                this.animator.Play(closeId);
                break;
        }
    }

    public virtual void Deactive()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void Active()
    {

    }

    public virtual void AutoHide()
    {
        if (this.gameObject.activeInHierarchy)
            Hide();
    }
}
