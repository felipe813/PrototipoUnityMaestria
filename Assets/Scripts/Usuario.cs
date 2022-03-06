using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Usuario
{
    private static int _idUsuario;
    public static int idUsuario
    {
        get
        {
            return _idUsuario;
        }
        set
        {
            _idUsuario = value;
        }
    }
    private static string _nombreUsuario;
    public static string nombreUsuario
    {
        get
        {
            return _nombreUsuario;
        }
        set
        {
            _nombreUsuario = value;
        }
    }
    private static string _nombre;
    public static string nombre
    {
        get
        {
            return _nombre;
        }
        set
        {
            _nombre = value;
        }
    }
    private static string _contrasenaUsuario;
    public static string contrasenaUsuario
    {
        get
        {
            return _contrasenaUsuario;
        }
        set
        {
            _contrasenaUsuario = value;
        }
    }

    private static int _edad;
    public static int edad
    {
        get
        {
            return _edad;
        }
        set
        {
            _edad = value;
        }
    }

    private static bool _registroNuevo;
    public static bool registroNuevo
    {
        get
        {
            return _registroNuevo;
        }
        set
        {
            _registroNuevo = value;
        }
    }
}
