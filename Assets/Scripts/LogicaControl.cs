using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Web;
// using UnityEngine.UnityWebRequest;


public class LogicaControl : MonoBehaviour
{
    public AudioSource Musica;
    public LogicaJugador Jugador;
    public GameObject PuntoInicio;
    public GameObject[] NivelPreFab;
    private int indicenivel;
    private GameObject ObjetoNivel;
    public Text TextodelJuego;
    public bool Escucho=false;
    public InputField txtid;
    public Button btnenviar;
    public Image image;
    public Text TextData;
    // Start is called before the first frame update
    void Start()
    {
        ObjetoNivel=Instantiate(NivelPreFab[indicenivel]);
        ObjetoNivel.transform.SetParent(this.transform);

        if ( btnenviar )
        {
            btnenviar.GetComponent<Button>().onClick.AddListener(
                () => { StartCoroutine( getRequest( "http://localhost:5000/player/" + txtid.text ) );
                btnenviar.enabled = false;
                btnenviar.gameObject.SetActive( false );
                txtid.enabled = false;
                txtid.gameObject.SetActive( false );
                }
            );
        }

    }

    IEnumerator getRequest( string uri ){
        UnityWebRequest uwr = UnityWebRequest.Get( uri );
        yield return uwr.SendWebRequest();
        if ( uwr.isNetworkError ){
            Debug.Log( "Error de conexión: " + uwr.error );
        } else {
            Player player = JsonUtility.FromJson<Player>( uwr.downloadHandler.text );
            TextData.text = "Nombre: " + player.name + "\n Correo: " + player.email + "\n Usuario: " + player.nickname;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture( player.avatar_url );
            yield return request.SendWebRequest();
       

        if( request.isNetworkError || request.isHttpError ) {
            Debug.Log(request.error );
        } else {
            Texture2D myTexture = ( ( DownloadHandlerTexture ) request.downloadHandler ).texture;
            Sprite newSprite = Sprite.Create( myTexture, new Rect( 0, 0, myTexture.width, myTexture.height ), new Vector2( 0.5f, 0.5f ) );
            image.sprite = newSprite;
        }
    } }

    IEnumerator postRequest(string uri, string bodyjsonString){
        var request = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw =  System.Text.Encoding.UTF8.GetBytes(bodyjsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        // Debug.Log("Status Code : " + request.responseCode);
    }

    IEnumerator putRequest(string uri, string bodyjsonString){
        var request = new UnityWebRequest(uri, "PUT");
        byte[] bodyRaw =  System.Text.Encoding.UTF8.GetBytes(bodyjsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        // Debug.Log("Status Code : " + request.responseCode);
    }   

    [ System.Serializable ]
    public class Game {
        public int score;
        public int coins;
    }
    [ System.Serializable ]
    public class Player {
        public string  email;
        public string  password;
        public string  name;
        public string  birthdate;
        public string  nickname;
        public string  avatar_url;
    }

    // Update is called once per frame
    void Update()
    {
        Game objeto = new Game();
        objeto.score = Jugador.zanahoria;
        objeto.coins = Jugador.coins;
        string jsonString = JsonUtility.ToJson(objeto);

        StartCoroutine(putRequest("http://localhost:5000/game/update/"+txtid.text, jsonString));

        TextodelJuego.text="Nivel: "+(indicenivel+1)+"\nPuntaje: "+Jugador.zanahoria;
        if(Jugador.Perdiste){
            TextodelJuego.text="Nivel"+(indicenivel+1)+"\nPuntaje "+
            Jugador.zanahoria+"\nPerdiste"+"\nPulsa R para continuar";
            if(Input.GetKeyDown("r")){
                Jugador.zanahoria=0;
                Jugador.JugadorBody.constraints=RigidbodyConstraints2D.None;
                Jugador.JugadorBody.constraints=RigidbodyConstraints2D.FreezeRotation;
                Jugador.gameObject.transform.position=PuntoInicio.transform.position;
                Destroy(ObjetoNivel);
                ObjetoNivel=Instantiate(NivelPreFab[indicenivel]);
                ObjetoNivel.transform.SetParent(this.transform);
                Jugador.Perdiste=false;
             //   TextodelJuego.text="Nivel: "+(indicenivel+1)+"\nPuntaje: "+Jugador.zanahoria;
            }
        }else if(Jugador.siguienteNivel && Jugador.zanahoria==15){
            
            TextodelJuego.text="Nivel"+(indicenivel+1)+"\nPuntaje "+
            Jugador.zanahoria+"\nCompletaste el nivel"+"\nPulsa R para avanzar";
            if(indicenivel==NivelPreFab.Length-1){
            TextodelJuego.text="Juego Terminado\nPulsa R para reiniciar el juego";
            if(Input.GetKeyDown("r")){

                Jugador.zanahoria=0;
                Jugador.JugadorBody.constraints = RigidbodyConstraints2D.None;
                Jugador.JugadorBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Jugador.gameObject.transform.position=PuntoInicio.transform.position;
                Destroy(ObjetoNivel);
                indicenivel=0;
                ObjetoNivel=Instantiate(NivelPreFab[indicenivel]);
                ObjetoNivel.transform.SetParent(this.transform);
                Jugador.siguienteNivel=false;
                
            }
        }else if(Input.GetKeyDown("r")){
                Jugador.zanahoria=0;
                Jugador.JugadorBody.constraints=RigidbodyConstraints2D.None;
                Jugador.JugadorBody.constraints=RigidbodyConstraints2D.FreezeRotation;
                Jugador.gameObject.transform.position=PuntoInicio.transform.position;
                Destroy(ObjetoNivel);
                indicenivel+=1;
                ObjetoNivel=Instantiate(NivelPreFab[indicenivel]);
                ObjetoNivel.transform.SetParent(this.transform);
                Jugador.siguienteNivel=false;
            }
        }else{
            TextodelJuego.text="Nivel"+(indicenivel+1)+"\nPuntaje "+
            Jugador.zanahoria+"\nTe falta completar "+(15-Jugador.zanahoria)+" puntos\npara pasar el nivel";
        }
    }
}
