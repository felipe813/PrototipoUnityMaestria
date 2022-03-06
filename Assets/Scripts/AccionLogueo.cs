using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acción para loguear una usuario en el sistema
/// </summary>
public class AccionLogueo : IComando
{
    private string _nombreAccion = "Logueo";

    // Datos de logueo 
    private string _nombreUsuario;
    private string _contrasenaUsuario;

    // Variable para saber si el usuario quedó o no logueado en el sistema
    private bool _usuarioLogueado;
    public bool usuarioLogueado
    {
        get
        {
            return _usuarioLogueado;
        }
        set
        {
            _usuarioLogueado = value;
        }
    }

    /// <summary>
    /// Constructor de la acción
    /// </summary>
    /// <param name="NombreUsuario">Nombre del usuario</param>
    /// <param name="ContrasenaUsuario">Contraseña del usuario</param>
    public AccionLogueo(string NombreUsuario, string ContrasenaUsuario){
        
        this._nombreUsuario = NombreUsuario;
        this._contrasenaUsuario = ContrasenaUsuario;
        this._usuarioLogueado = false;
    }

    /// <summary>
    /// Acción de loguear, cambia el parametro de usuario logueado de verdadero o falso según
    /// corresponda
    /// </summary>
    public void EjecutarAccion(){
        InterfazProgreso.ActualizarAccion(_nombreAccion,20,"Logueando usuario");
        DAOUsuario daoUsuario = new DAOUsuario();

        this._usuarioLogueado = daoUsuario.Logueo(_nombreUsuario,_contrasenaUsuario);
        InterfazProgreso.ActualizarAccion(_nombreAccion,100,"Usuario logueado con éxito");
    }
    
    /// <summary>
    /// Acción para reversar la opción de logueo (desloguear) cambia el parametro de usuario logueado a falso
    /// </summary>
    public void DeshacerAccion(){
        InterfazProgreso.ActualizarAccion(_nombreAccion,20,"Deslogueando usuario");
        DAOUsuario daoUsuario = new DAOUsuario();
        this._usuarioLogueado = daoUsuario.Deslogueo();
        InterfazProgreso.ActualizarAccion(_nombreAccion,100,"Usuario deslogueado con éxito");
    }

}
