using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam, 
    Battle,
    Dialog,
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;


    GameState state = GameState.FreeRoam;

    private void Start()
    {
        playerController.OnEncounted += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        DialogManager.Instance.OnShowDialog += ShowDialog;
        DialogManager.Instance.OnCloseDialog += CloseDialog;
    }

    void ShowDialog()
    {
        state = GameState.Dialog;
    }
    void CloseDialog()
    {
        if (state == GameState.Dialog)
        {
            state = GameState.FreeRoam;
        }
    }

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        // ??????????????
        PokemonParty playerParty = playerController.GetComponent<PokemonParty>();
        // FindObjectOfType:???????????????????1?????
        Pokemon wildPokemon = FindObjectOfType<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
