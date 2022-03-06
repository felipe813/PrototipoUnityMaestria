using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acción para calificar una imagen
/// </summary>
public class AccionCalificarImagen : IComando
{
    private string _nombreAccion = "Calificar";

    private int _calificacion;
    
    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="idImagen"></param>
    /// <param name="calificacion"></param>
    public AccionCalificarImagen(int calificacion){
        
        this._calificacion = calificacion;
    }

    /// <summary>
    /// Acción para calificar una imagen en el sistema
    /// </summary>
    public void EjecutarAccion(){
        int idImagen = Recorrido.imagenActual;
        int idRecorrido = Recorrido.id;
        DAORecorrido daoRecorrido = new DAORecorrido();
        //TODO
        //daoRecorrido.CalificarImagen(idImagen,idRecorrido,_calificacion);
        Recorrido.CalificarImagen(idImagen,_calificacion);
    }

    public void DeshacerAccion(){

    }
    
}
