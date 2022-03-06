using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Clase para modelar el control de las ayudas para escneas y elementos de todo el aplicativo
/// </summary>
public class ControlAyudas
{
    /// <summary>
    /// Lista de ayudas que se pueden mostrar a lo largo de todo el aplicativo
    /// </summary>
    /// <typeparam name="Ayuda">Objeto para modelar una ayuda, tiene nombre de vista, de elemento y texto de 
    /// ayuda</typeparam>
    private static List<Ayuda> _ayudas = new List<Ayuda>();

    /// <summary>
    /// Método para crear una ayuda nueva, se necesita el nombre de la vista, del elemento y la ayuda a mostar, si 
    /// ya se tiene una entrada con el mismo nombre de vista y elemento se sobreescribe la ayuda
    /// </summary>
    /// <param name="nombreVista">Nombre de la vista a la que se va a agregar la ayuda</param>
    /// <param name="nombreElemento">Nombre del elemento de la vista al que se agregará la ayuda, si es nulo significa
    /// que la ayuda es general de la vista</param>
    /// <param name="ayuda">Texto de ayuda a mostrar</param>
    public static void AgregarAyuda(string nombreVista, string nombreElemento, string ayuda)
    {
        Ayuda ayudaExistente = _ayudas.Where(x => x.nombreVista == nombreVista && x.nombreElemento == nombreElemento).FirstOrDefault();
        if (ayudaExistente != null)
        {
            ayudaExistente.ayuda = ayuda;
        }
        else
        {
            _ayudas.Add(new Ayuda
            {
                nombreElemento = nombreElemento,
                nombreVista = nombreVista,
                ayuda = ayuda
            });
        }
    }

    /// <summary>
    /// Método para obtener la ayuda de una determinada vista
    /// </summary>
    /// <param name="nombreVista">Nombre de la vista de la cual se quiere obtener la ayuda</param>
    /// <returns>Texto de ayuda de la vista, si es nulo signfica que no se a agregado ayuda</returns>
    public static string GetAyuda(string nombreVista)
    {
        Ayuda ayudaExistente = _ayudas.Where(x => x.nombreVista == nombreVista && x.nombreElemento == null).FirstOrDefault();
        if (ayudaExistente != null)
            return ayudaExistente.ayuda;
        else
            return null;
    }

    /// <summary>
    /// Método para obtener la ayuda de un determinado elemento de una vista
    /// </summary>
    /// <param name="nombreVista">Nombre de la vista a la que pertenece el elemnto del cual se quiere
    /// obtener la ayuda</param>
    /// <param name="nombreElemento">Nombre del elemento del cual se quiere obtener la ayuda</param>
    /// <returns>Texto de ayuda del elemento, si es nulo signfica que no se a agregado ayuda</returns>
    public static string GetAyuda(string nombreVista, string nombreElemento)
    {
        Ayuda ayudaExistente = _ayudas.Where(x => x.nombreVista == nombreVista && x.nombreElemento == nombreElemento).FirstOrDefault();
        if (ayudaExistente != null)
            return ayudaExistente.ayuda;
        else
            return null;
    }
}
