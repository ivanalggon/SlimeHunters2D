using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPlayer : MonoBehaviour
{
    public float velocidad = 5f;
    public GameObject proyectilPrefab;
    public Transform puntoDisparo;

    private Animator animacion;
    private float vida = 5f; // Vida del jugador
    private bool invulnerable = false; // Indica si el jugador es invulnerable

    // imagenes de la vida del jugador en HUD
    public GameObject[] vidaHUD;

    // Sonidos
    public AudioClip sonidoDisparo;
    public AudioClip sonidoDaño;
    public AudioClip sonidoMuerte;

    void Start()
    {
        // desocultar el gameobject de la vida del jugador de la posicion 5
        vidaHUD[5].SetActive(true);
        vidaHUD[4].SetActive(false);
        vidaHUD[3].SetActive(false);
        vidaHUD[2].SetActive(false);
        vidaHUD[1].SetActive(false);
        vidaHUD[0].SetActive(false);

        animacion = GetComponent<Animator>();

        Time.timeScale = 1;
    }
    void Update()
    {
        MovimientoPlayer();

        // Detectar clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0)) // 0 es para el botón izquierdo del ratón
        {
            Disparar();
        }
    }

    void Disparar()
    {
        // si el juego esta pausado no se puede disparar
        if (Time.timeScale == 0)
        {
            return;
        }
        else
        {
            // Reproducir el sonido del disparo
            AudioSource.PlayClipAtPoint(sonidoDisparo, transform.position);

            // Obtener la posición del ratón en el mundo 2D
            Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicionRaton.z = 0; // Asegurarte de que el ratón esté en el plano 2D

            // Asegurarte de que el punto de disparo también esté en 2D
            Vector3 posicionPuntoDisparo = puntoDisparo.position;
            posicionPuntoDisparo.z = 0;

            // Calcular la dirección desde el punto de disparo hacia el ratón
            Vector2 direccionDisparo = (posicionRaton - posicionPuntoDisparo).normalized;

            // Crear el proyectil en el punto de disparo
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

            // Configurar la dirección del proyectil
            Proyectil scriptProyectil = proyectil.GetComponent<Proyectil>();
            if (scriptProyectil != null)
            {
                scriptProyectil.ConfigurarDireccion(direccionDisparo);
            }
        }
    }


    private void MovimientoPlayer()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        Vector2 movimiento = new Vector2(movimientoHorizontal, movimientoVertical) * velocidad * Time.deltaTime;
        transform.Translate(movimiento);

        if (movimientoHorizontal < 0)
        {
            animacion.SetBool("isMovingLeft", true);
            animacion.SetBool("isMovingRight", false);
            animacion.SetBool("isMoving", false);
            animacion.SetBool("isIdle", false);
        }
        // si el movimiento es solamente derecha (tecla D)
        else if (movimientoHorizontal > 0)
        {
            animacion.SetBool("isMovingRight", true);
            animacion.SetBool("isMovingLeft", false);
            animacion.SetBool("isMoving", false);
            animacion.SetBool("isIdle", false);
        }
        // Arriba o abajo (tecla W o S)
        else if (movimientoVertical > 0 || movimientoVertical < 0)
        {
            animacion.SetBool("isMoving", true);
            animacion.SetBool("isMovingLeft", false);
            animacion.SetBool("isMovingRight", false);
            animacion.SetBool("isIdle", false);
        }
        // si no se esta moviendo (Idle)
        else
        {
            animacion.SetBool("isIdle", true);
            animacion.SetBool("isMoving", false);
            animacion.SetBool("isMovingLeft", false);
            animacion.SetBool("isMovingRight", false);
        }
    }

    private void RecibirDaño()
    {
        if (invulnerable == false)
        {
            vida -= 1;
        }

        // Actualizar HUD
        for (int i = 5; i >= 0; i--)
        {
            vidaHUD[i].SetActive(i == vida);
        }

        if (vida > 0 && invulnerable == false)
        {
            // Reproducir sonido de daño
            AudioSource.PlayClipAtPoint(sonidoDaño, transform.position);
            //Tiempo de invulnerabilidad despues de recibir daño
            StartCoroutine(Invulnerable());
        }

        // Si la vida llega a 0
        if (vida == 0)
        {
            vidaHUD[0].SetActive(true);
            // Reproducir sonido de muerte
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
            //esperar 2 segundos antes de cambiar de escena
            StartCoroutine(EsperarGameOver());
        }
    }

    IEnumerator Invulnerable()
    {
        
        invulnerable = true; // El jugador es invulnerable
        // cambiar el color del jugador al recibir daño
        GetComponent<SpriteRenderer>().color = Color.red;

        // Esperar 2 segundos
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().color = Color.white;
        invulnerable = false; // El jugador ya no es invulnerable

    }

    IEnumerator EsperarGameOver()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2); // Espera 2 segundos
        // Después de esperar, carga la escena de Game Over
        
        SceneManager.LoadScene("GameOver");
        Time.timeScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDaño();
        }
    }
}
