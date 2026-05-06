using UnityEngine;

public class PalancaInteractiva : MonoBehaviour
{
    public Transform palanca;
    public float anguloBajada = -45f;
    public float velocidad = 2f;

    private bool jugadorCerca = false;
    private bool activada = false;
    private Quaternion rotacionInicial;
    private Quaternion rotacionFinal;

    void Start()
    {
        rotacionInicial = palanca.rotation;
        rotacionFinal = Quaternion.Euler(
            palanca.eulerAngles.x + anguloBajada,
            palanca.eulerAngles.y,
            palanca.eulerAngles.z
        );
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            activada = !activada;
        }

        if (activada)
        {
            palanca.rotation = Quaternion.Lerp(
                palanca.rotation,
                rotacionFinal,
                Time.deltaTime * velocidad
            );
        }
        else
        {
            palanca.rotation = Quaternion.Lerp(
                palanca.rotation,
                rotacionInicial,
                Time.deltaTime * velocidad
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}