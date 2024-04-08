
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MM;

[MM.Manager(DisplayName = "Game Manager")]
public class GameManager : SingletonManager<GameManager>
{
    private enum GameState {
        MainMenu,
        Gameplay,
        Paused,
        Quitting
    }

    private GameState gameState = GameState.MainMenu;
    private StateMachine<GameState> stateMachine = new StateMachine<GameState>();

    
}

