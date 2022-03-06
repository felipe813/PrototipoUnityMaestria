using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase de comunicación entre el controlador y la lógica para la funcionalidad de logueo
/// </summary>
public class InterfazLogicaLogueo
{
    private ControlDeshacer _controlDeshacer = new ControlDeshacer();

    /// <summary>
    /// Método apra ejecutar el logueo de un usuario
    /// </summary>
    /// <param name="NombreUsuario">Nombre de usuario</param>
    /// <param name="ContrasenaUsuario">Contraseña de usuario</param>
    /// <returns>Se retorna una cadena de texto, con el error de logueo si lo hay, si es una cadena vacia
    /// signfica que se hizo el logueo correctamente</returns>
    public string Logueo(string NombreUsuario, string ContrasenaUsuario){
        Debug.Log("Logueo Interfaz");
        AccionLogueo accionLogueo = this._controlDeshacer.Logueo(NombreUsuario,ContrasenaUsuario);
        accionLogueo.EjecutarAccion();
        if(accionLogueo.usuarioLogueado){
            Usuario.registroNuevo = false;
            return "";
        }else{
            return "Nombre de usuario o contraseña incorrectos";
        }        
    }

    public bool UsuarioExiste(string NombreUsuario, string ContrasenaUsuario){
        Debug.Log("Logueo Interfaz");
        AccionLogueo accionLogueo = this._controlDeshacer.Logueo(NombreUsuario,ContrasenaUsuario);
        accionLogueo.EjecutarAccion();
        return accionLogueo.usuarioLogueado;
               
    }
    /// <summary>
    /// Método para ejecutar el registro de un usuario
    /// </summary>
    /// <param name="NombreUsuario">Nombre de usuario</param>
    /// <param name="ContrasenaUsuario">Contraseña</param>
    /// <param name="Nombre">Nombre de la persona</param>
    /// <param name="Edad">Edad de la persona</param>
    /// <returns>Se retorna una cadena de texto, con el error de registro si lo hay, si es una cadena vacia
    /// signfica que se hizo el registro correctamente</returns>

    public string Registro(string NombreUsuario, string ContrasenaUsuario, string Nombre, string Edad){
        AccionRegistroUsuario accionRegistro = new AccionRegistroUsuario(NombreUsuario,ContrasenaUsuario, Nombre, Edad);
        accionRegistro.EjecutarAccion();       
        if(accionRegistro.usuarioRegistrado){  
            Usuario.registroNuevo = true;   
            return "";
        }else{    
            return "No se pudo hacer el registro, intente nuevamente por favor.";
        }
    }


}
