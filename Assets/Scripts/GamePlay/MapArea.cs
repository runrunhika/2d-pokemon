using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    // 野生のポケモンを管理する
    [SerializeField] List<Pokemon> pokemons;

    // ランダムで渡す
    public Pokemon GetRandomWildPokemon()
    {
        int r = Random.Range(0, pokemons.Count);
        Pokemon pokemon = pokemons[r];
        pokemon.Init(); // 出会うたびに初期化
        return pokemon;
    }
}
