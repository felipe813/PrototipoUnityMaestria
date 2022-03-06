using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlador para la escena del test
/// </summary>
public class ControladorTest : MonoBehaviour
{
    //Elementos del escenario de la escena del test
    public GameObject CanvasIntroduccion;
    public GameObject CanvasPregunta1;
    public Dropdown Dropdown1;
    public GameObject CanvasPregunta2;
    public Dropdown Dropdown2;
    public GameObject CanvasPregunta3;
    public Dropdown Dropdown3;
    public GameObject CanvasPregunta4;
    public Dropdown Dropdown4;
    public GameObject CanvasPregunta5;
    public Dropdown Dropdown5;

    //Nombre de las escenas
    private string _nombreEscenaLogueo = "EscenaLogueo";

    
    /// <summary>
    /// Método que se lanza apenas se crea la escena, se agregan los validadores y ayudas necesarias
    /// </summary>
    public void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;
    }

/// <summary>
/// Método para desactivar todos los canvas de la escena
/// </summary>
    private void DesactivarTodosCanvas(){
        CanvasIntroduccion.SetActive(false);
        CanvasPregunta1.SetActive(false);
        CanvasPregunta2.SetActive(false);
        CanvasPregunta3.SetActive(false);
        CanvasPregunta4.SetActive(false);
        CanvasPregunta5.SetActive(false);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void CargarPregunta1()
    {
        DesactivarTodosCanvas();
        CanvasPregunta1.SetActive(true);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void GuardarPregunta1()
    {
        int idRecorrido = Recorrido.id;
        int respuesta = 5 - Dropdown1.value;
        AgregarRespuesta(idRecorrido, 1, respuesta);

        DesactivarTodosCanvas();
        CanvasPregunta2.SetActive(true);
        Debug.Log("Calificacion: "+Dropdown1.value);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void GuardarPregunta2()
    {
        int idRecorrido = Recorrido.id;
        int respuesta = 5 - Dropdown2.value;
        AgregarRespuesta(idRecorrido, 2, respuesta);

        DesactivarTodosCanvas();
        CanvasPregunta3.SetActive(true);
        Debug.Log("Calificacion: "+Dropdown2.value);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void GuardarPregunta3()
    {
        int idRecorrido = Recorrido.id;
        int respuesta = 5 - Dropdown3.value;
        AgregarRespuesta(idRecorrido, 3, respuesta);

        DesactivarTodosCanvas();
        CanvasPregunta4.SetActive(true);
        Debug.Log("Calificacion: "+Dropdown3.value);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void GuardarPregunta4()
    {
        int idRecorrido = Recorrido.id;
        int respuesta = 5 - Dropdown4.value;
        AgregarRespuesta(idRecorrido, 4, respuesta);


        DesactivarTodosCanvas();
        CanvasPregunta5.SetActive(true);
        Debug.Log("Calificacion: "+Dropdown4.value);
    }

    /// <summary>
    /// Método para guardar la respuesta a la pregunta 1 y pasar a la 2
    /// </summary>
    public void GuardarPregunta5()
    {        
        int idRecorrido = Recorrido.id;
        int respuesta = 5 - Dropdown5.value;
        AgregarRespuesta(idRecorrido, 5, respuesta);
        Debug.Log("Calificacion: "+Dropdown5.value);
        Application.Quit();
        //CargarPantallaLogueo();
    }

    
    /// <summary>
    /// Método para cambiar a la pantalla de logueo
    /// </summary>
    public void CargarPantallaLogueo()
    {
        SceneManager.LoadScene(_nombreEscenaLogueo);
    }  


    private void AgregarRespuesta(int idRecorrido, int idPegunta, int respuestaPregunta){
        string jsonPOST = "{" +
                 "\"IdRecorrido\":\"" + idRecorrido + "\"," +
                 "\"IdPregunta\":\"" + idPegunta + "\"," +
                 "\"Tiempo\":\"" + (int)Time.realtimeSinceStartup + "\"," +
                 "\"Respuesta\": "+respuestaPregunta+
                 "}";

        string respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio + "/api/RespuestasTest", "POST",jsonPOST);
        if (respuesta == null)
        {
            Debug.Log("!!!Error creando respuesta");
        }
    }
}
