using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelper : MonoSingleton<UIHelper>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public static T FindScript<T>()
    {
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<T>();
            if (_childScript != null)
                return _childScript;
        }

        return default(T);
    }

    public static T FindScript<T>(bool isHide)
    {
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<T>();
            if (_childScript != null)
                return _childScript;
            else
                _child.gameObject.SetActive(isHide);
        }

        return default(T);
    }
}
