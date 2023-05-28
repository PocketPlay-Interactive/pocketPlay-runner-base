using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUICanvas : UICanvas
{
    public Text apologizeText;
    private string txt = "I apologize for submitting late due to an unexpected reason. The 'Extra Asset' folder is not related to the project, I used it to get models for the game. Use A W D S move and jump and slide.";
    public override void Show()
    {
        base.Show();
        apologizeText.DOKill();
        apologizeText.DOText(txt, 7f, true).OnComplete(() =>
        {
            CoroutineUtils.PlayCoroutine(() =>
            {
                Hide();
                UIHelper.FindScript<HomeUICanvas>().Show();
            }, 3f);
        });
    }
}
