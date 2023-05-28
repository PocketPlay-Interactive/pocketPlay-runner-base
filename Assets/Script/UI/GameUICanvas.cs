using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUICanvas : UICanvas
{
    public Text scoreText;
    public override void Show()
    {
        base.Show();
        GameController.gameState = GameState.Play;
    }

    public void OnPause()
    {
        GameController.gameState = GameState.Pause;
        UIHelper.FindScript<PauseUICanvas>().Show();
    }

    private void Update()
    {
        scoreText.text = GameController.Instance.GetScore().ToString();
    }
}
