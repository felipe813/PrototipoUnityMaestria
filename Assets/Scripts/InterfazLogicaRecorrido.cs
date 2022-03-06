using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase de comunicación entre el controlador y la lógica para la funcionalidad de recorrido
/// </summary>
public class InterfazLogicaRecorrido
{
    private ControlDeshacer _controlDeshacer = new ControlDeshacer();

    public void CambiarImagen(bool esSiguiente){
        AccionCambiarImagen accionCambiar = this._controlDeshacer.CambiarImagen(esSiguiente);
        accionCambiar.EjecutarAccion();      
    }

    public void CalificarImagen(int calificacion){
        AccionCalificarImagen accionCalificar = new AccionCalificarImagen(calificacion);
        accionCalificar.EjecutarAccion();
        
    }


}