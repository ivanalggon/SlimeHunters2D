using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemies : MonoBehaviour
{
    public float velocidad = 1f; // Velocidad del enemigo
    public float rangoDeteccion = 5f; // Rango dentro del cual el enemigo persigue al jugador

    private Rigidbody2D rigidbody2D; // Referencia al Rigidbody2D del enemigo
    private Animator animacion; // Referencia al Animator del enemigo
    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animacion = GetComponent<Animator>();

        // Encuentra al jugador una vez al iniciar
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            jugador = player.transform;
        }
        else
        {
            Debug.LogError("No se encontr� un objeto llamado 'Player' en la escena.");
        }
    }

    void FixedUpdate()
    {
        if (jugador == null) return; // Si no hay jugador, no hacer nada

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            MovimientoEnemigo();
        }
        else
        {
            // Detener movimiento y animaci�n si est� fuera del rango
            rigidbody2D.velocity = Vector2.zero;
            animacion.SetBool("isWalking", false);
            animacion.SetBool("isIdle", true);
        }
    }

    private void MovimientoEnemigo()
    {
        // Calcular direcci�n hacia el jugador
        Vector2 direccion = (jugador.position - transform.position).normalized;

        // Establecer la velocidad del Rigidbody2D
        rigidbody2D.velocity = direccion * velocidad;

        // Manejar animaciones
        animacion.SetBool("isWalking", true);
        animacion.SetBool("isIdle", false);

        // Opcional: Ajustar la direcci�n de la animaci�n para que el enemigo mire hacia el jugador
        if (direccion.x > 0)
            transform.localScale = new Vector3(-1, 1, 1); // Mirar hacia la derecha
        else if (direccion.x < 0)
            transform.localScale = new Vector3(1, 1, 1); // Mirar hacia la izquierda
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Verificamos que el objeto en colisi�n sea el jugador
        {
            // Obtener el Rigidbody2D del jugador
            Rigidbody2D jugadorRB = collision.gameObject.GetComponent<Rigidbody2D>();

            if (jugadorRB != null) // Verificamos que el jugador tenga un Rigidbody2D
            {
                // Calcular la direcci�n de empuje, apuntando desde el enemigo al jugador
                Vector2 direccionEmpuje = (collision.transform.position - transform.position).normalized;

                // Aplicar la fuerza de empuje al jugador usando AddForce en lugar de modificar directamente la velocidad
                float fuerzaEmpuje = 1f; // Ajusta esta fuerza seg�n lo que desees
                jugadorRB.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse); // Aplicamos un impulso al jugador
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Detener el movimiento del jugador cuando la colisi�n termina
            Rigidbody2D jugadorRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (jugadorRB != null)
            {
                jugadorRB.velocity = Vector2.zero; // Detener el movimiento del jugador al salir de la colisi�n
            }
        }
    }
}
