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

    public static T GetScript<T>()
    {
        T response = default(T);
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<T>();
            if (_childScript != null)
                response = _childScript;
            else
                _child.gameObject.GetComponent<UICanvas>().Hide();
        }

        return response;
    }
}
