using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acción para loguear una usuario en el sistema
/// </summary>
public class AccionRegistroUsuario
{
    private string _nombreAccion = "RegistroUsuario";

    // Datos necesarios para el registro
    private string _nombreUsuario;
    private string _contrasenaUsuario;
     private string _nombre;
    private string _edad;

    // Variable para saber si el jugador quedo registrado o no en el sistema
    private bool _usuarioRegistrado;
    public bool usuarioRegistrado
    {
        get
        {
            return _usuarioRegistrado;
        }
        set
        {
            _usuarioRegistrado = value;
        }
    }

    /// <summary>
    /// Constructor de la acción registro
    /// </summary>
    /// <param name="NombreUsuario">Nombre de usuario</param>
    /// <param name="ContrasenaUsuario">Contraseña del usuario</param>
    /// <param name="Nombre">Nombre de la persona</param>
    /// <param name="Edad">Edad de la persona</param>
    public AccionRegistroUsuario(string NombreUsuario, string ContrasenaUsuario, string Nombre, string Edad){
        this._nombreUsuario = NombreUsuario;
        this._contrasenaUsuario = ContrasenaUsuario;
        this._nombre = Nombre;
        this._edad = Edad;
        this.usuarioRegistrado = false;
    }
    /// <summary>
    /// Acción de registrar, cambia el valor de usuario registrado a verdadero o falso
    /// </summary>
    public void EjecutarAccion(){
        InterfazProgreso.ActualizarAccion(_nombreAccion,15,"Registrando usuario, espera un momento");
        DAOUsuario daoUsuario = new DAOUsuario();
        
        _usuarioRegistrado = daoUsuario.RegistrarUsuario(_nombreUsuario,_contrasenaUsuario,_nombre,_edad);
        InterfazProgreso.ActualizarAccion(_nombreAccion,100,"Usuario registrado con éxito");
    }
}
