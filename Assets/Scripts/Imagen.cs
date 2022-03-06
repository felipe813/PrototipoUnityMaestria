using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Imagen
{
    private  int _idImagen;
    public  int idImagen
    {
        get
        {
            return _idImagen;
        }
        set
        {
            _idImagen = value;
        }
    }

    private  int _calificacion;
    public  int calificacion
    {
        get
        {
            return _calificacion;
        }
        set
        {
            _calificacion = value;
        }
    }
    private  string _nombreImagen;
    public  string nombreImagen
    {
        get
        {
            return _nombreImagen;
        }
        set
        {
            _nombreImagen = value;
        }
    }
    private  string _direccionImagen;
    public  string direccionImagen
    {
        get
        {
            return _direccionImagen;
        }
        set
        {
            _direccionImagen = value;
        }
    }
    private  GameObject _componenteImagen;
    public  GameObject componenteImagen
    {
        get
        {
            return _componenteImagen;
        }
        set
        {
            _componenteImagen = value;
        }
    }
    
}
