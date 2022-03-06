using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

/// <summary>
/// Clase para ejecutar operaciones REST
/// </summary>
public class ServicioREST
{

    //public static string direccionServicio = "http://127.0.0.1:5000";
    public static string direccionServicio = "http://35.233.219.175:8013";
    /// <summary>
    /// Clase para ejecutar una operación REST donde se necesite enviar información en formato JSON
    /// </summary>
    /// <param name="direccionAPI">Direccion del servicio</param>
    /// <param name="operacion">Operacion a ejecutar (GET,POST,PUT,DELETE)</param>
    /// <param name="json">JSON a enviar</param>
    /// <returns>Texto en formato JSON recibido del servicio</returns>
    public static string EjecutarOperacion(string direccionAPI,string operacion,string json)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(direccionAPI);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = operacion;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
        }
        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var responseText = streamReader.ReadToEnd();
            return responseText;
        }
    }

    /// <summary>
    /// Clase para ejecutar una operación REST donde no se necesite enviar información JSON
    /// </summary>
    /// <param name="direccionAPI">Direccion del servicio</param>
    /// <param name="operacion">Operacion a ejecutar (GET,POST,PUT,DELETE)</param>
    /// <returns>Texto en formato JSON recibido del servicio</returns>
	public static string EjecutarOperacion(string direccionAPI,string operacion)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(direccionAPI);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = operacion;

        HttpWebResponse httpResponse;
        try{
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }catch (WebException ex){
            Debug.Log("Error encontrado, Operación: "+operacion+", Dirección API "+direccionAPI);
            Debug.Log(ex);
            return null;
        }
        
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var responseText = streamReader.ReadToEnd();
            return responseText;
        }
    }
}
