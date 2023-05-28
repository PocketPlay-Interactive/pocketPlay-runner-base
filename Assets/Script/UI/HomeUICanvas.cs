using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUICanvas : UICanvas
{
    public void OnPlay()
    {
        Hide();
        UIHelper.FindScript<GameUICanvas>().Show();
    }
}
