using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para modelar la validación de un campo
/// </summary>
public class ValidadorCampo
{
    /// <summary>
    /// Nombre de la vista a la que pertenece el campo
    /// </summary>
    private string _nombreVista;
    /// <summary>
    /// Nombre del campo como tal
    /// </summary>
    private string _nombreCampo;
    /// <summary>
    /// Mensaje de error que aparecerá si el campo no está correcto
    /// </summary>
    private string _mensajeError;
    /// <summary>
    /// Expresion regular que verifica la validez del campo
    /// </summary>
    private string _expresionRegular;

    public string nombreVista
    {
        get
        {
            return _nombreVista;
        }
        set
        {
            _nombreVista = value;
        }
    }

    public string nombreCampo
    {
        get
        {
            return _nombreCampo;
        }
        set
        {
            _nombreCampo = value;
        }
    }

    public string mensajeError
    {
        get
        {
            return _mensajeError;
        }
        set
        {
            _mensajeError = value;
        }
    }

    public string expresionRegular
    {
        get
        {
            return _expresionRegular;
        }
        set
        {
            _expresionRegular = value;
        }
    }
}
