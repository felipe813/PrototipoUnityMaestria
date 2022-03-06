using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DAORecorrido
{
    private string cantidadImagenes = "10";
    /// <summary>
    /// Se crea un recorrido nuevo en la base de datos con 
    /// </summary>
    /// <returns></returns>
    public bool CrearNuevoRecorrido()
    {
        
        string respuesta = string.Empty;
        //Se debe obtener el nivel de violencia de preferencia para el usuarioy de acuerdo a este se le muestran las imágenes.
        //string nivelViolencia = "1";
        string nivelViolencia = string.Empty;
        //Si no se tiene un nivel de violencia de preferencia, se le muestran imágenes de cualquier nivel.
        if(!string.IsNullOrEmpty(nivelViolencia))
            respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio + "/api/imagenesRandomViolencia/1/" + cantidadImagenes+"/"+nivelViolencia, "GET");
        else
            respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio + "/api/imagenesRandomViolencia/1/" + cantidadImagenes, "GET");
        if (string.IsNullOrEmpty(respuesta))
        {
            Debug.Log("!!!Error creando el recorrido");
            return false;
        }
        JSONNode listaImagenes = JSON.Parse(respuesta)["Imagenes"];
        string listaImagenesJSON = "[";
        if (listaImagenes != null)
        {
            foreach (var imagen in listaImagenes)
            {
                listaImagenesJSON+="\""+imagen.Value["Id"]+"\""+",";
            }
            listaImagenesJSON = listaImagenesJSON.TrimEnd(',');
            listaImagenesJSON+="]";

            int idUsuario = Usuario.idUsuario;
            //Debug.Log("IdUsuario :"+idUsuario);
            if(idUsuario == 0)
                idUsuario =1;

            string jsonPOST = "{" +
                 "\"IdUsuario\":\"" + idUsuario + "\"," +
                 "\"IdImagenes\": "+listaImagenesJSON+
                 "}";

            respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio + "/api/recorridos", "POST",jsonPOST);
            JSONNode recorrido = JSON.Parse(respuesta)["Recorrido"];
            Recorrido.fechaRecorrido = System.DateTime.Parse(recorrido["FechaRecorrido"]);
            Recorrido.id = recorrido["Id"];
            JSONNode imagenes = recorrido["Imagenes"];
            if(Recorrido.listaImagenes == null) Recorrido.listaImagenes = new List<Imagen>();
            foreach (var imagen in imagenes)
            {
                Imagen img = new Imagen{
                    idImagen = imagen.Value["IdImagen"],
                    direccionImagen = imagen.Value["DireccionImagen"],
                    nombreImagen = imagen.Value["NombreImagen"],
                    calificacion = imagen.Value["Calificacion"],
                };
                Recorrido.listaImagenes.Add(img);
            }            
            return true;
        }
        else
        {
            Debug.Log("!!!Error creando el recorrido");
            return false;
        }
    }


    public bool CalificarImagen(int idImagen,int idRecorrido, int calificacion)
    {
        Debug.Log("Se calificará imagen "+idImagen+" del recorrido "+idRecorrido+" con calificacion "+calificacion);
        if(idImagen == 0 || idRecorrido == 0){
            return false;
        }
        string jsonPUT = "{" +
                "\"IdRecorrido\": \"" + idRecorrido + "\"," +
                "\"IdImagen\": \"" + idImagen + "\"," +
                "\"Calificacion\": \"" + calificacion + "\""+
                "}";
        
        string respuesta = ServicioREST.EjecutarOperacion(ServicioREST.direccionServicio+"/api/recorridos", "PUT",jsonPUT);
        if(respuesta == null){
            Debug.Log("!!!No se pudo hacer la calificacion");
            return false;
        }
        //Debug.Log(respuesta);
        return true;
    }

    
}
