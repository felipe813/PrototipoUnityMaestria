using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class ControladorRecorrido : MonoBehaviour
{

    public List<Toggle> Calificaciones;
    // Objeto de interfaz para interactuar con la lógica
    private InterfazLogicaRecorrido _interfaz = new InterfazLogicaRecorrido();

    private string _nombreEscenaPrueba = "EscenaPrueba";
    void Start()
    {
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        AccionCrearRecorrido crearRecorrido = new AccionCrearRecorrido(this);
        crearRecorrido.EjecutarAccion();        
    }

    public void AnteriorImagen()
    {
        //Debug.Log("Calificacion actual "+ObtenerCalificacion());
        CalificarImagen();
        this._interfaz.CambiarImagen(false);
        ReiniciarNotas();   
        CambiarCalificacionActual();         
    }

    public void SiguienteImagen()
    {
        //Debug.Log("Calificacion actual "+ObtenerCalificacion());
        CalificarImagen();
        this._interfaz.CambiarImagen(true);
        ReiniciarNotas(); 
        CambiarCalificacionActual();           
    }

    public void CalificarImagen()
    {
        int calificacion = ObtenerCalificacion();
        _interfaz.CalificarImagen(calificacion);
    }

    private int ObtenerCalificacion(){
        int calificacionFinal = 0;
        foreach (var calificiacion in Calificaciones)
        {
            if(calificiacion.isOn){
                calificacionFinal++;
            }else{
                break;
            }
        }
        return calificacionFinal;
    }

    private void ReiniciarNotas(){       
        foreach (var calificiacion in Calificaciones)
        {
            calificiacion.SetIsOnWithoutNotify(false);
        }
    }

    public void CambiarCalificacion(int nota){
        if(nota<0)
            nota = 0;
        for (int i = 0; i < nota; i++)
        {
            Calificaciones[i].SetIsOnWithoutNotify(true);
        }

        for (int i = nota; i < Calificaciones.Count; i++)
        {
            Calificaciones[i].SetIsOnWithoutNotify(false);
        }              
    }

    private void CambiarCalificacionActual(){
        CambiarCalificacion(Recorrido.ObtenerCalificacionImagenActual());
    }

    public void Salir(){
        SceneManager.LoadScene(_nombreEscenaPrueba);
    }

}
