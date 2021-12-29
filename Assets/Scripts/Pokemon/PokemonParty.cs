using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    // トレーナーのポケモンを管理する
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get => pokemons;}

    private void Start()
    {
        // ゲーム開始時に初期化
        foreach (Pokemon pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    // 戦えるポケモンを渡す(HP>0のポケモンを返す)
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(monster => monster.HP > 0).FirstOrDefault();
    }
}
