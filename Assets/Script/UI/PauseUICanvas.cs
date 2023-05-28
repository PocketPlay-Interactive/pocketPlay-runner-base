using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUICanvas : UICanvas
{
    public void OnContinute()
    {
        Hide();
        GameController.gameState = GameState.Play;
    }

    public void OnHome()
    {
        Hide();
        GameController.Instance.CreateNewGame();
    }
}
