using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public delegate void onChangeSkin(int director);
    public static onChangeSkin OnChangeSkin;
    public static void OnChangeLanguageMethod(int director)
    {
        if (OnChangeSkin != null)
            OnChangeSkin?.Invoke(director);
    }
}
