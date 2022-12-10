using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaCuerpo : MonoBehaviour
{
    public LogicaJugador Jugador;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        private void OnTriggerEnter2D(Collider2D c1)
    {
        if(c1.tag=="enemigo"){
            Jugador.Perder.Play();
            Jugador.Perdiste=true;
            Jugador.JugadorBody.constraints=RigidbodyConstraints2D.FreezeAll;
        }
        if(c1.tag=="zanahoria")
        {
            Jugador.zanahoria++;
        }
        if(c1.tag=="siguientenivel"){
           Jugador.siguienteNivel=true;
            Jugador.JugadorBody.constraints=RigidbodyConstraints2D.FreezeAll;
        }
    }
}
