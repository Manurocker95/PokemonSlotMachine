using PokemonCasino.Setup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokemonCasino
{
    public class PC_Init : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Screen.fullScreen = false;
            PC_SceneManager.LoadScene(1);
        } 
    }

}
