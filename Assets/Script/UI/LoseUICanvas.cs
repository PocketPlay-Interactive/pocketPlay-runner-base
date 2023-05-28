using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseUICanvas : UICanvas
{
    public Text scoreText;

    public override void Show()
    {
        base.Show();
        scoreText.text = GameController.Instance.GetScore().ToString();
    }

    public void OnHome()
    {
        Hide();
        GameController.Instance.CreateNewGame();
    }
}
