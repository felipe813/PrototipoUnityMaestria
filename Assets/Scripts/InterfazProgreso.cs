using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que modela la información mostrada durante la carga de un elemento, se tiene un mensaje y un porcentaje
/// de carga
/// </summary>
public class InformacionAccion{

    private double _porcentajeAccion;
    public double porcentajeAccion
    {
        get
        {
            return _porcentajeAccion;
        }
        set
        {
            _porcentajeAccion = value;
        }
    }
    private string _mensajeActual;
    public string mensajeActual
    {
        get
        {
            return _mensajeActual;
        }
        set
        {
            _mensajeActual = value;
        }
    }

    public InformacionAccion(double PorcentajeAccion,string MensajeActual){
        this._porcentajeAccion = PorcentajeAccion;
        this._mensajeActual = MensajeActual;
    }
}

/// <summary>
/// Clase para comunicar las clases de procesamiento y acciones con el controlador por medio de la clase Progreso
/// </summary>
public class InterfazProgreso
{
    /// <summary>
    /// Diccionario con las acciones en curso en determinado momento, tienen como llave el nombre de la acción
    /// y como valor un objeto de tipo InformacionAccion para saber el porcentaje y estado de dicha accion
    /// </summary>
    private static Dictionary<string,InformacionAccion> _accionesEnCurso;
    /// <summary>
    /// Cantidad de acciones en curso
    /// </summary>
    private static int _cantidadAcciones = 0;

    /// <summary>
    ///  Método para actualizar una determinada acción, si se tiene un porcentaje del 100% se remueve la accion
    /// de acciones en curso
    /// </summary>
    /// <param name="NombreAccion">Nombre de la accion a actualizar</param>
    /// <param name="PorcentajeAccion">Porcentaje de dicha acción</param>
    /// <param name="MensajeActual">Mensaje actual</param>
    public static void ActualizarAccion(string NombreAccion,double PorcentajeAccion,string MensajeActual){

        if(PorcentajeAccion == 100){
            if(_accionesEnCurso != null){
                if(_accionesEnCurso.ContainsKey(NombreAccion)){
                    _accionesEnCurso.Remove(NombreAccion);
                }
            }
        }else{
            if(_accionesEnCurso == null){
                _accionesEnCurso = new Dictionary<string, InformacionAccion>();
                _accionesEnCurso.Add(NombreAccion,new InformacionAccion(PorcentajeAccion,MensajeActual));
                _cantidadAcciones++;
            }else{
                if(_accionesEnCurso.ContainsKey(NombreAccion)){
                    _accionesEnCurso[NombreAccion]= new InformacionAccion(PorcentajeAccion,MensajeActual);
                }else{
                    _accionesEnCurso.Add(NombreAccion,new InformacionAccion(PorcentajeAccion,MensajeActual));
                    _cantidadAcciones++;
                }
            }
        }       
        ActualizarProgresso();
    }

    /// <summary>
    /// Método privado para actualizar el progreso según los valores que se tengan en acciones en curso
    /// </summary>
    private static void ActualizarProgresso(){

        int accionesActuales = 0;
        double porcentajeTotal = 0;

        string mensajeActual = "";

        foreach(KeyValuePair<string,InformacionAccion> informacion in _accionesEnCurso){    
            if(mensajeActual=="") mensajeActual = informacion.Value.mensajeActual;        
            accionesActuales ++;
            porcentajeTotal = porcentajeTotal + informacion.Value.porcentajeAccion;
        }

        if(accionesActuales != 0){
            int diferenciaCantidadAcciones = _cantidadAcciones - accionesActuales;
            porcentajeTotal = (porcentajeTotal + diferenciaCantidadAcciones*100) / _cantidadAcciones;
        }else {
            porcentajeTotal = 100;
            _cantidadAcciones = 0;
            _accionesEnCurso = new Dictionary<string, InformacionAccion>();
        }

        Progreso.ActualizarAccion(mensajeActual,porcentajeTotal);
    }
}
