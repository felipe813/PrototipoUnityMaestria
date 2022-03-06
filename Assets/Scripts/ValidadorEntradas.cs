
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

/// <summary>
/// Clase que controla la validación de entradas del aplicativo
/// </summary>
public class ValidadorEntradas
{
    /// <summary>
    /// Lista de objetos de validación
    /// </summary>
    /// <typeparam name="ValidadorCampo"></typeparam>
    /// <returns></returns>
    private static List<ValidadorCampo> _validadoresCampo = new List<ValidadorCampo>();
    /// <summary>
    /// Error que se mostrará si no se tiene error de validación en determinado campo
    /// </summary>
    private static string _errorDefault = "Error en el formato del campo";

    /// <summary>
    /// Se agrega o actualiza una validación a un campo 
    /// </summary>
    /// <param name="nombreVista">Nombre de la vista a la cual pertenece el campo</param>
    /// <param name="nombreElemento">Nombre del campo a validar</param>
    /// <param name="expresionRegular">Expresión regular para saber si el campo es valido o no</param>
    /// <param name="mensajeError">Mensaje de error a mostrar si el elemento no es válido</param>
    public static void AgregarValidador(string nombreVista,string nombreElemento,string expresionRegular,string mensajeError){
        ValidadorCampo validadorExistente = _validadoresCampo.Where(x=> x.nombreVista == nombreVista && x.nombreCampo == nombreElemento).FirstOrDefault();
        if(validadorExistente!=null){
            validadorExistente.mensajeError = mensajeError;
            validadorExistente.expresionRegular = expresionRegular;
        }else{
                _validadoresCampo.Add(new ValidadorCampo{
                nombreCampo = nombreElemento,
                nombreVista = nombreVista,
                mensajeError = mensajeError,
                expresionRegular = expresionRegular
            });
        }
    }

    /// <summary>
    /// Método que valida si un campo está correcto o no
    /// </summary>
    /// <param name="nombreVista">Nombre de la vista a la que pertenece el campo</param>
    /// <param name="nombreCampo">Nombre del campo a validar</param>
    /// <param name="valorCampo">Valor que se va a validar</param>
    /// <returns>Se retorna el error si se tiene o un valor nulo si está bien el campo</returns>
    public static string Validar(string nombreVista,string nombreCampo, string valorCampo){
        ValidadorCampo validadorExistente = _validadoresCampo.Where(x=> x.nombreVista == nombreVista && x.nombreCampo == nombreCampo).FirstOrDefault();
        //Debug.Log("Iniciando validación");
        if(validadorExistente!=null){           
            Regex reg = new Regex(validadorExistente.expresionRegular);
            if(reg.IsMatch(valorCampo)){
                return null;
            }else{
                if(!string.IsNullOrEmpty(validadorExistente.mensajeError)){
                    return validadorExistente.mensajeError;
                }else
                    return _errorDefault;
            }                      
        }else{
            Debug.Log("No existe validación, Vista: "+nombreVista+", nombre campo: "+nombreCampo+", valor: "+valorCampo);
            return null;
        }
    }
}
