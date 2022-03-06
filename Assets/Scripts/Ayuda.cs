using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ayuda
{
    /// <summary>
    /// Nombre de la vista o escena a la que pertenece la ayuda
    /// </summary>
    private string _nombreVista;
    /// <summary>
    /// Nombre del elemento de la vista al que pertenece la ayuda, si el valor es nulo
    /// significa que la ayuda es general de la vista
    /// </summary>
    private string _nombreElemento;
    /// <summary>
    /// Texto con la ayuda
    /// </summary>
    private string _ayuda;

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

    public string nombreElemento
    {
        get
        {
            return _nombreElemento;
        }
        set
        {
            _nombreElemento = value;
        }
    }

    public string ayuda
    {
        get
        {
            return _ayuda;
        }
        set
        {
            _ayuda = value;
        }
    }
}
