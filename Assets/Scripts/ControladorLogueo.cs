using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlador para las escenas de inicio y logueo
/// </summary>
public class ControladorLogueo : MonoBehaviour
{
    //Elementos del escenario de la escena del logueo
    public GameObject LogueoCuadroAyuda;
    public Text LogueoNombreUsuarioError;
    public GameObject LogueoNombreUsuarioCuadroError;
    public Text LogueoNombreUsuarioTexto;
    public InputField LogueoContrasenaTexto;
    public Text LogueoContrasenaError;
    public GameObject LogueoContrasenaCuadroError;
    public Text LogueoErrorTexto;
    public Text LogueoAyudaGeneral;
    
    
    //Elementos del escenario de la escena del registro
    public GameObject RegistroCuadroAyuda;
    public Text RegistroNombreUsuarioError;
    public GameObject RegistroNombreUsuarioCuadroError;
    public Text RegistroNombreUsuarioTexto;
    public Text RegistroNombreError;
    public GameObject RegistroNombreCuadroError;
    public Text RegistroNombreTexto;
    public Text RegistroEdadError;
    public GameObject RegistroEdadCuadroError;
    public Text RegistroEdadTexto;
    public InputField RegistroContrasenaTexto;
    public Text RegistroErrorTexto;
    public GameObject RegistroCuadroError;
    public Text RegistroAyudaGeneral;

    //Nombre de las escenas
    private string _nombreEscenaRegistro = "EscenaRegistro";
    private string _nombreEscenaLogueo = "EscenaLogueo";
    private string _nombreEscenaSiguiente = "EscenaRecorrido";
    private string _nombreEscenaInicio = "EscenaInicio";

    // Objeto de interfaz para interactuar con la lógica
    private InterfazLogicaLogueo _interfaz = new InterfazLogicaLogueo();
    /// <summary>
    /// Método que se lanza apenas se crea la escena, se agregan los validadores y ayudas necesarias
    /// </summary>
    public void Start()
    {
        //Screen.orientation = ScreenOrientation.Portrait;

        ValidadorEntradas.AgregarValidador("EscenaLogueo", "NombreUsuario", "^([A-Za-z0-9])+$", "No se permiten espacios ni simbolos.");
        ControlAyudas.AgregarAyuda("EscenaLogueo", null, "Pantalla de logueo, introduce tu usuario y contraseña, si no estás en el sistema aún, puedes registrarte en la opción correspondiente.");

        ValidadorEntradas.AgregarValidador("EscenaRegistro", "NombreUsuario", "^([A-Za-z0-9])+$", "No se permiten espacios ni simbolos.");
        ValidadorEntradas.AgregarValidador("EscenaRegistro", "Nombre", "^([A-Za-záéíóúÁÉÍÓÚ\\s])+$", "No se permiten números ni símbolos.");
        ValidadorEntradas.AgregarValidador("EscenaRegistro", "Edad", "^([0-9]{1,2})$", "La edad debe ser un número positivo");
        ControlAyudas.AgregarAyuda("EscenaRegistro", null, "Pantalla de registro, introduce tus datos, un usuario y una contraseña para poder registrarte en el sistema.");

        if(RegistroNombreUsuarioTexto!=null && !string.IsNullOrEmpty(Usuario.nombreUsuario)){
            Debug.Log(Usuario.nombreUsuario);
            RegistroNombreUsuarioTexto.text = Usuario.nombreUsuario;
        }

    }
    /// <summary>
    /// Método para realizar el respectivo logueo, se toma el usuario y la contraseña de los respectivos elementos
    /// y se llama el logueo de la interfaz, se retorna un mensaje de error que puede estar vacio(si se logueo 
    /// correctamente) o puede traer el respectivo error, para ponerlo en el elemento de la interfaz necesario
    /// </summary>
    public void Logueo()
    {
        if (LogueoListo())
        {
            //LogueoErrorTexto.text = "";
            LogueoContrasenaError.text = "";
            LogueoContrasenaCuadroError.SetActive(false);

            string nombreUsuario = LogueoNombreUsuarioTexto.text;
            string contrasenaUsuario = LogueoContrasenaTexto.text;
            
            string error = _interfaz.Logueo(nombreUsuario, contrasenaUsuario);

            if (error != "")
            {
                //LogueoErrorTexto.text = error;
                LogueoContrasenaError.text = error;
                LogueoContrasenaCuadroError.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(_nombreEscenaSiguiente);
            }
        }
        else
        {
            //LogueoErrorTexto.text = "No se puede iniciar sesion.";
            LogueoContrasenaError.text = "No se puede iniciar sesion.";
            LogueoContrasenaCuadroError.SetActive(true);
        }
    }

    public void LogueoNuevo()
    {
        if (LogueoListo())
        {
            //LogueoErrorTexto.text = "";
            LogueoContrasenaError.text = "";
            LogueoContrasenaCuadroError.SetActive(false);

            string nombreUsuario = LogueoNombreUsuarioTexto.text;
            //string contrasenaUsuario = LogueoContrasenaTexto.text;
            string contrasenaUsuario = "default";
                    

            _interfaz.UsuarioExiste(nombreUsuario, contrasenaUsuario);

            if (_interfaz.UsuarioExiste(nombreUsuario, contrasenaUsuario))
            {
                SceneManager.LoadScene(_nombreEscenaSiguiente);
            }
            else
            {
                SceneManager.LoadScene(_nombreEscenaRegistro);
                Usuario.nombreUsuario = LogueoNombreUsuarioTexto.text;
            }
        }
        else
        {
            //LogueoErrorTexto.text = "No se puede iniciar sesion.";
            LogueoContrasenaError.text = "No se puede iniciar sesion.";
            LogueoContrasenaCuadroError.SetActive(true);
        }
    }

    /// <summary>
    /// Método para realizar el respectivo logueo, se toma el usuario y la contraseña de los respectivos elementos
    /// y se llama el logueo de la interfaz, se retorna un mensaje de error que puede estar vacio(si se logueo 
    /// correctamente) o puede traer el respectivo error, para ponerlo en el elemento de la interfaz necesario
    /// </summary>
    public void LogueoInvitado()
    {
        //if (LogueoListo())
        //{
            //LogueoErrorTexto.text = "";
            //LogueoContrasenaError.text = "";
            //LogueoContrasenaCuadroError.SetActive(false);

            //Datos de logueo para usuario de prueba
            string nombreUsuario = "UsuarioTest";
            string contrasenaUsuario = "default";
            
            string error = _interfaz.Logueo(nombreUsuario, contrasenaUsuario);

            if (error != "")
            {
                //LogueoErrorTexto.text = error;
                LogueoContrasenaError.text = error;
                LogueoContrasenaCuadroError.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(_nombreEscenaSiguiente);
            }
        //}
        //else
        //{
        //   //LogueoErrorTexto.text = "No se puede iniciar sesion.";
        //    LogueoContrasenaError.text = "No se puede iniciar sesion.";
        //   LogueoContrasenaCuadroError.SetActive(true);
        //}
    }

    /// <summary>
    /// Métdoo para saber si se puede efectuar el logueo, se deben cumplir ciertas condiciones, por ejemplo que no 
    /// haya un mensaje de error en el usuario
    /// </summary>
    /// <returns>Se retorna un boleano para saber si se puede hacer o no el logueo</returns>
    private bool LogueoListo()
    {
        if (LogueoNombreUsuarioError.text == "")
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Método para realizar el respectivo registro, se toma el usuario,la contraseña, la edad y el nombre de 
    /// los respectivos elementos y se llama el registro de la interfaz, se retorna un mensaje de error que 
    /// puede estar vacio(si se registro correctamente) o puede traer el respectivo error, para ponerlo en el 
    /// elemento de la interfaz necesario
    /// </summary>
    public void Registro()
    {
        if (RegistroListo())
        {
            RegistroErrorTexto.text = "";
            RegistroCuadroError.SetActive(false);
            string nombreUsuario = RegistroNombreUsuarioTexto.text;
           // string contrasenaUsuario = RegistroContrasenaTexto.text;
            string contrasenaUsuario = "default";

            string nombre = RegistroNombreTexto.text;
            string edad = RegistroEdadTexto.text;
            string error = _interfaz.Registro(nombreUsuario, contrasenaUsuario, nombre, edad);
            if (error == "")
            {
                SceneManager.LoadScene(_nombreEscenaSiguiente);               
            }
            else
            {
                RegistroCuadroError.SetActive(true);
                RegistroErrorTexto.text = error;
            }
        }
        else
        {
            RegistroCuadroError.SetActive(true);
            RegistroErrorTexto.text = "No se puede registrar el usuario.";
        }

    }
    /// <summary>
    /// Métdoo para saber si se puede efectuar el registro, se deben cumplir ciertas condiciones, por ejemplo que no 
    /// haya un mensaje de error en el usuario, en el nombre o en la edad
    /// </summary>
    /// <returns>Se retorna un boleano para saber si se puede hacer o no el registro</returns>
    private bool RegistroListo()
    {
        if (RegistroNombreUsuarioError.text == "" && RegistroNombreError.text ==""&&RegistroEdadError.text == "")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Método para cambiar a la pantalla de registro
    /// </summary>
    public void CargarPantallaRegistro()
    {
        SceneManager.LoadScene(_nombreEscenaRegistro);
    }
    /// <summary>
    /// Método para cambiar a la pantalla de logueo
    /// </summary>
    public void CargarPantallaLogueo()
    {
        SceneManager.LoadScene(_nombreEscenaLogueo);
    }

    /// <summary>
    /// Método para validar si en la escena del logueo, el usuario cumple con la estructura necesaria, y así
    /// mostrar o no un mensaje en pantalla
    /// </summary>
    public void ValidarUsuarioLogueo()
    {
        string usuario = LogueoNombreUsuarioTexto.text;
        if (!string.IsNullOrEmpty(usuario))
        {
            string error = ValidadorEntradas.Validar("EscenaLogueo", "NombreUsuario", usuario);
            if (!string.IsNullOrEmpty(error))
            {
                LogueoNombreUsuarioError.text = error;
                LogueoNombreUsuarioCuadroError.SetActive(true);
            }
            else
            {
                LogueoNombreUsuarioError.text = "";
                LogueoNombreUsuarioCuadroError.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Método para validar si en la escena del registro, el usuario cumple con la estructura necesaria, y así
    /// mostrar o no un mensaje en pantalla
    /// </summary>
    public void ValidarUsuarioRegistro()
    {
        RegistroErrorTexto.text = "";
        RegistroCuadroError.SetActive(false);
        string usuario = RegistroNombreUsuarioTexto.text;
        if (!string.IsNullOrEmpty(usuario))
        {
            string error = ValidadorEntradas.Validar("EscenaRegistro", "NombreUsuario", usuario);
            if (!string.IsNullOrEmpty(error))
            {
                RegistroNombreUsuarioCuadroError.SetActive(true);
                RegistroNombreUsuarioError.text = error;
            }
            else
            {
                RegistroNombreUsuarioCuadroError.SetActive(false);
                RegistroNombreUsuarioError.text = "";
            }
        }
    }

     /// <summary>
    /// Método para validar si en la escena del registro, la edad cumple con la estructura necesaria, y así
    /// mostrar o no un mensaje en pantalla
    /// </summary>
    public void ValidarEdadRegistro()
    {
        RegistroErrorTexto.text = "";
        RegistroCuadroError.SetActive(false);
        string edad = RegistroEdadTexto.text;
        if (!string.IsNullOrEmpty(edad))
        {
            string error = ValidadorEntradas.Validar("EscenaRegistro", "Edad", edad);
            if (!string.IsNullOrEmpty(error))
            {
                RegistroEdadCuadroError.SetActive(true);
                RegistroEdadError.text = error;
            }
            else
            {
                RegistroEdadCuadroError.SetActive(false);
                RegistroEdadError.text = "";
            }
        }
    }

      /// <summary>
    /// Método para validar si en la escena del registro, el nombre cumple con la estructura necesaria, y así
    /// mostrar o no un mensaje en pantalla
    /// </summary>
    public void ValidarNombreRegistro()
    {
        RegistroErrorTexto.text = "";
        RegistroCuadroError.SetActive(false);
        string nombre = RegistroNombreTexto.text;
        if (!string.IsNullOrEmpty(nombre))
        {
            string error = ValidadorEntradas.Validar("EscenaRegistro", "Nombre", nombre);
            if (!string.IsNullOrEmpty(error))
            {
                RegistroNombreCuadroError.SetActive(true);
                RegistroNombreError.text = error;
            }
            else
            {
                RegistroNombreCuadroError.SetActive(false);
                RegistroNombreError.text = "";
            }
        }
    }

    /// <summary>
    /// Método para mostrar/ocultar la ayuda de la escena de logueo
    /// </summary>
    public void AccionarAyudaLogueo()
    {
        //LogueoAyudaGeneral.text = ControlAyudas.GetAyuda(_nombreEscenaLogueo);
        if (LogueoCuadroAyuda.activeInHierarchy)
            LogueoCuadroAyuda.SetActive(false);
        else
            LogueoCuadroAyuda.SetActive(true);
    }
    /// <summary>
    /// Método para mostrar/ocultar la ayuda de la escena de registro
    /// </summary>
    public void AccionarAyudaRegistro()
    {
        RegistroAyudaGeneral.text = ControlAyudas.GetAyuda(_nombreEscenaRegistro);
        if (RegistroCuadroAyuda.activeInHierarchy)
            RegistroCuadroAyuda.SetActive(false);
        else
            RegistroCuadroAyuda.SetActive(true);
    }

    public void CargarPantallaInicio(){
        SceneManager.LoadScene(_nombreEscenaInicio);
    }
}
