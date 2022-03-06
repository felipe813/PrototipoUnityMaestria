using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para guardar y poder navegar (hacer y deshacer) dentro de un conjunto de acciones
/// específicas
/// </summary>
public class ControlDeshacer
{
    //Pilas para guardar los elementos a deshacer y rehacer
    private Stack<IComando> _Undocommands;
    private Stack<IComando> _Redocommands;

    public ControlDeshacer(){
        this._Undocommands = new Stack<IComando>();
        this._Redocommands = new Stack<IComando>();
    }
    // Método para rehacer la última acción hecha
    public void Rehacer(int levels)
    {
        for (int i = 1; i <= levels; i++)
        {
            if (_Redocommands.Count != 0)
            {
                IComando command = _Redocommands.Pop();
                command.EjecutarAccion();
                _Undocommands.Push(command);
            }

        }
    }

    // Método para deshacer la última acción hecha
    public void Deshacer(int levels)
    {
        for (int i = 1; i <= levels; i++)
        {
            if (_Undocommands.Count != 0)
            {
                IComando command = _Undocommands.Pop();
                command.DeshacerAccion();
                _Redocommands.Push(command);
            }

        }
    }

    // Método para crear y agregar una nueva acción de logueo

    public AccionLogueo Logueo(string NombreUsuario, string ContrasenaUsuario)
    {
        AccionLogueo logueo = new AccionLogueo(NombreUsuario,ContrasenaUsuario);
        _Undocommands.Push(logueo);
        _Redocommands.Clear();
        return logueo;
    }

    // Método para cambiar imagen
    public AccionCambiarImagen CambiarImagen(bool esSiguiente)
    {
        AccionCambiarImagen cambiarImagen = new AccionCambiarImagen(esSiguiente);
        _Undocommands.Push(cambiarImagen);
        _Redocommands.Clear();
        return cambiarImagen;
    }
}
