using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DAOUsuario
{
    // private bool _usuarioLogueado;
    // public bool usuarioLogueado
    // {
    //     get
    //     {
    //         return _usuarioLogueado;
    //     }
    //     set
    //     {
    //         _usuarioLogueado = value;
    //     }
    // }
    // private bool _logueoTerminado;
    // public bool logueoTerminado
    // {
    //     get
    //     {
    //         return _logueoTerminado;
    //     }
    //     set
    //     {
    //         _logueoTerminado = value;
    //     }
    // }
    
    // private bool _usuarioRegistrado;
    // public bool usuarioRegistrado
    // {
    //     get
    //     {
    //         return _usuarioRegistrado;
    //     }
    //     set
    //     {
    //         _usuarioRegistrado = value;
    //     }
    // }
    // private bool _registroTerminado;
    // public bool registroTerminado
    // {
    //     get
    //     {
    //         return _registroTerminado;
    //     }
    //     set
    //     {
    //         _registroTerminado = value;
    //     }
    // }
    public bool Logueo(string NombreUsuario, string Contrasena){

        // string jsonPOST = "{" +
        //         "\"idtipologueologueo\":" + TiposLogueo.getIdTipoLogueo("Normal") + "," +
        //         "\"usuariologueo\": \"" + txt_usuario.text + "\"," +
        //         "\"contrasena\": \"" + txt_password.text + "\"" +
        //         "}";

        string respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio+"/api/usuarios/"+NombreUsuario+"/"+Contrasena, "GET");
        if(respuesta == null){
            Debug.Log("!!!Datos incorrectos");
            return false;
        }
        JSONNode usuario = JSON.Parse(respuesta)["Usuario"];
            if(usuario != null){
                Usuario.nombre = usuario["Nombre"];
                Usuario.edad = usuario["Edad"];
                Usuario.nombreUsuario = usuario["Usuario"];
                Usuario.contrasenaUsuario = usuario["Contraseña"];
                Usuario.idUsuario = usuario["Id"];
                Debug.Log("Usuario logueado: ");
                Debug.Log(usuario);
                return true;
            }else{
                Debug.Log("!!!Datos incorrectos");
                return false;
            }
    }

    public bool RegistrarUsuario(string NombreUsuario, string Contrasena, string Nombre, string Edad){
        if(NombreUsuario == null || NombreUsuario == "" || Contrasena == null || Contrasena == ""
        || Nombre == null || Nombre == "" || Edad == null || Edad == ""){
            Debug.Log("Para registrar no se pueden tener campos vacios");
            return false;
        }
        string jsonPOST = "{" +
                "\"Usuario\": \"" + NombreUsuario + "\"," +
                "\"Contrasena\": \"" + Contrasena + "\"," +
                "\"Nombre\": \"" + Nombre + "\"," +
                "\"Edad\": \"" + Edad + "\"" +
                "}";
        
        string respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio+"/api/usuarios", "POST",jsonPOST);
        if(respuesta == null){
            Debug.Log("!!!No se pudo hacer el registro");
            return false;
        }

        JSONNode usuario = JSON.Parse(respuesta)["Usuario"];
        if(usuario != null){
            Usuario.nombre = usuario["Nombre"];
            Usuario.edad = usuario["Edad"];
            Usuario.nombreUsuario = usuario["Usuario"];
            Usuario.contrasenaUsuario = usuario["Contraseña"];
            Usuario.idUsuario = usuario["Id"];
            Debug.Log("Usuario registrado: ");
            Debug.Log(usuario);
            return true;
        }else{
            Debug.Log("!!!No se pudo hacer el registro");
            return false;
        }
    }

    public bool Deslogueo(){
        return false;
        
    }
}
