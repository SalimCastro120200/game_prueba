using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaZanahoria : MonoBehaviour
{
    public AudioSource sonido;
    public LogicaControl Jugador;
    public int puntaje;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D c1)
    {
        if(c1.tag=="cuerpo")
        {
            Destroy(gameObject);
            // Jugador.score = 1;
            Debug.Log("Se ha añadido un punto");
            
            sonido.Play();
        }
    }
}
