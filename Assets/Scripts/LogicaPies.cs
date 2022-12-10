using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPies : MonoBehaviour
{
    public float rebote;
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
        if(c1.tag=="plataforma")
        {
            Jugador.EnPiso=true;
            Jugador.miAnimacion.SetFloat("salto",0);
            Jugador.Caida.Play();
        }
        if (c1.tag=="enemigo"){
            Jugador.Salto.Play();
            Destroy(c1.gameObject);
            Jugador.JugadorBody.velocity=new Vector2(Jugador.JugadorBody.velocity.x, rebote);
        }
    }

    private void OnTriggerExit2D(Collider2D c2){
        if(c2.tag=="plataforma"){
            Jugador.EnPiso=false;
        }
    }
}
