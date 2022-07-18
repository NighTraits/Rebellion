using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Reiniciar la partida
    public void Reiniciar()
    {
        // reanudar controles del player
        CharacterController.isPaused = false;
        Time.timeScale = 1;

        // carga la escena actual
        // PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Health.dead = false;

    }

    // Regresar al menú de inicio
    public void Cerrar()
    {
        // carga la escena 0 (Menu inicial)
        CharacterController.isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Health.dead = false;
    }
}
