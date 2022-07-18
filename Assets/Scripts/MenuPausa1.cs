using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPausa1 : MonoBehaviour
{
    // Script de la pantalla de pausa
    
    [SerializeField] private GameObject menuPausa;

    // Cerrar menú de pausa y continuar la partida
    public void Reanudar()
    {
        CharacterController.isPaused = false;
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
    }

    // reiniciar la partida
    public void Reiniciar()
    {
        CharacterController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Regresar al menú de inicio
    public void Cerrar()
    {
        SceneManager.LoadScene(0);
    }

    // Cerrar el juego
    public void Exit()
    {
        Application.Quit();
    }
}

