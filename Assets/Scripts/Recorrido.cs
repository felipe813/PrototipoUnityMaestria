using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Recorrido 
{
    private static List<Imagen> _listaImagenes;
    public static List<Imagen> listaImagenes
    {
        get
        {
            return _listaImagenes;
        }
        set
        {
            _listaImagenes = value;
        }
    }

    private static int _id;
    public static int id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    private static System.DateTime _fechaRecorrido;
    public static System.DateTime fechaRecorrido
    {
        get
        {
            return _fechaRecorrido;
        }
        set
        {
            _fechaRecorrido = value;
        }
    }

    private static int _imagenActual;
    public static int imagenActual
    {
        get
        {
            return _imagenActual;
        }
        set
        {
            _imagenActual = value;
        }
    }

    public static void RemoverImagenByUrl(string url){
        _listaImagenes.RemoveAll(x=>x.direccionImagen == url);
    }

    public static void RemoverImagenById(int id){
        _listaImagenes.RemoveAll(x=>x.idImagen == id);
    }

    public static void AgregarGameObjectImagen(GameObject go, int id){
        Imagen img = _listaImagenes.Where(x=>x.idImagen == id).FirstOrDefault();
        if(img!=null){
            img.componenteImagen = go;
        }
    }

    public static void MostrarImagen(int id){
        Imagen img = _listaImagenes.Where(x=>x.idImagen == id).FirstOrDefault();
        if(img!=null){
            if(img.componenteImagen != null)
                img.componenteImagen.SetActive(true);
        }
    }

    public static void OcultarImagen(int id){
        Imagen img = _listaImagenes.Where(x=>x.idImagen == id).FirstOrDefault();
        if(img!=null){
            if(img.componenteImagen != null)
                img.componenteImagen.SetActive(false);
        }
    }

    public static void SiguienteImagen(){
        int cantidad = _listaImagenes.Count;
        //OcultarImagen(_imagenActual);
        if(cantidad != 0){
            Imagen imgActual = _listaImagenes.Where(x=>x.idImagen == _imagenActual).FirstOrDefault();                
            int index = _listaImagenes.IndexOf(imgActual);
            index ++;
            if(index == cantidad)
                index = 0;
            imgActual = _listaImagenes[index];
            _imagenActual = imgActual.idImagen;
        }
        //MostrarImagen(_imagenActual);
    }

    public static void AnteriorImagen(){
        int cantidad = _listaImagenes.Count;
        //OcultarImagen(_imagenActual);
        if(cantidad != 0){           
            Imagen imgActual = _listaImagenes.Where(x=>x.idImagen == _imagenActual).FirstOrDefault();                
            int index = _listaImagenes.IndexOf(imgActual);
            index --;
            if(index == -1)
                index = cantidad-1;
            imgActual = _listaImagenes[index];
            _imagenActual = imgActual.idImagen;
        }
        //MostrarImagen(_imagenActual);
    }

    public static void CalificarImagen(int idImagen, int calificacion){
        Imagen imgActual = _listaImagenes.Where(x=>x.idImagen == idImagen).FirstOrDefault();  
        if(imgActual!=null){
            imgActual.calificacion = calificacion;
        }else{
            Debug.Log("La imágen no existe "+idImagen);
        }
    }

    public static int ObtenerCalificacionImagenActual(){
        Imagen imgActual = _listaImagenes.Where(x=>x.idImagen == _imagenActual).FirstOrDefault();  
        if(imgActual!=null){    
            return imgActual.calificacion;
        } 
        else{
            Debug.Log("La imágen no existe "+_imagenActual);
        }
        return 0;
    }
}
