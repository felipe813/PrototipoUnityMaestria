using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface para modelar acciones que se pueden deshacer y rehacer
/// </summary>
public interface IComando
{
    /// <summary>
    /// Método para ejecutar la acción
    /// </summary>
    void EjecutarAccion();

    /// <summary>
    /// Método para deshacer la acción
    /// </summary>
    void DeshacerAccion();
}
