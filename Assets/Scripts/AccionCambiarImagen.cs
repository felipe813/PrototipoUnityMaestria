using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionCambiarImagen : IComando
{

    private bool _esSiguiente;

    public AccionCambiarImagen(bool esSiguiente){
        this._esSiguiente = esSiguiente;
    }
    public void EjecutarAccion(){
        if(this._esSiguiente)
            Recorrido.SiguienteImagen();
        else
            Recorrido.AnteriorImagen();
    }
    
    public void DeshacerAccion(){
        if(this._esSiguiente)
            Recorrido.AnteriorImagen();
        else
            Recorrido.SiguienteImagen();
    }
}
