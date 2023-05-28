using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUICanvas : UICanvas
{
    public void OnBack()
    {
        Hide();
        UIHelper.FindScript<HomeUICanvas>().Show();
    }
    // I think using a delegate may not be necessary in this part, but I included it for completeness.
    public void OnPrev()
    {
        GameEvent.OnChangeSkin(-1);
    }
    
    public void OnNext()
    {
        GameEvent.OnChangeSkin(1);
    }    
}
