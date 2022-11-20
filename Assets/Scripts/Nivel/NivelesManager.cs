using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VNCreator;

public class NivelesManager : MonoBehaviour
{
    // Atributos

    [SerializeField] private NivelesDataStream nivelesDS;
    private List<SerializableLevel> niveles;

    private int seleccion = 0;

    [SerializeField] private GameObject infoNivel;
    [SerializeField] private List<Button> botonesSeleccion;

    [SerializeField] private Sprite imgBloqueado;
    [SerializeField] private Sprite imgDesbloqueado;

    [SerializeField] private GameObject historiaManager;
    [SerializeField] private List<String> historias;


    // GETTERS & SETTERS

    public NivelesDataStream GetNivelesDS() { return this.nivelesDS; }
    public List<SerializableLevel> GetNiveles() { return this.niveles; }
    public int GetSeleccion() { return this.seleccion; }

    public void SetNivelesDS(NivelesDataStream nivelesDS) { this.nivelesDS = nivelesDS; }
    public void SetNiveles(List<SerializableLevel> niveles) { this.niveles = niveles; }
    public void SetSeleccion(int seleccion) { this.seleccion = seleccion - 1; }


    // CONSTRUCTORES

    public NivelesManager() { }
    public NivelesManager(NivelesDataStream nivelesDS, List<SerializableLevel> niveles)
    {
        this.nivelesDS = nivelesDS;
        this.niveles = niveles;
    }



    // Awake se llama al cargar la instancia del script
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("Estados Niveles");
        // Si no existe la tabla de estados en el PlayerPrefs
        if (!PlayerPrefs.HasKey("Estados Niveles"))
        {
            // Creamos el array de estados
            SerializableEstadoList estados = new SerializableEstadoList();
            int contador = 0;

            //Rellenamos el array(primera posición no jugado el resto bloqueados)
            estados.list.Add(Estado.NO_JUGADO);
            contador++;

            while (contador < 10)
            {
                estados.list.Add(Estado.BLOQUEADO);
                contador++;
            }

            // Parseamos el array a un formato string
            string estadosString = JsonUtility.ToJson(estados);
            Debug.Log(estadosString);

            // Guardamos la información en los PlayerPrefs
            PlayerPrefs.SetString("Estados Niveles", estadosString);
        }
        niveles = nivelesDS.ObtenerLista();
    }

    // Start is called before the first frame update
    void Start()
    {
        string estadosString = PlayerPrefs.GetString("Estados Niveles");

        SerializableEstadoList estados = JsonUtility.FromJson<SerializableEstadoList>(estadosString);

        for (int i = 0; i < botonesSeleccion.Count; i++)
        {
            var imagen = botonesSeleccion[i].GetComponent<Image>() as Image;

            if (estados.list[i] == 0)
            {
                imagen.sprite = imgBloqueado;

                botonesSeleccion[i].enabled = false;
            } else
            {
                imagen.sprite = imgDesbloqueado;
            }
        }
    }


    // METODOS

    public void ActivaInfo(int idx)
    {
        SetSeleccion(idx);

        var manager = historiaManager.GetComponent<HistoriaManager>() as HistoriaManager;
        var idxHistoria = niveles[this.seleccion].historia;
        if (niveles[this.seleccion].historia != 0)
        {
            manager.SetTieneHistoria(true);
            manager.SetHistoria(historias[idxHistoria - 1]);
        }

        RellenaInfo();
        infoNivel.SetActive(true);
    }

    public void DesactivaInfo()
    {
        var manager = historiaManager.GetComponent<HistoriaManager>() as HistoriaManager;
        manager.SetTieneHistoria(false);

        infoNivel.SetActive(false);
    }

    private void RellenaInfo()
    {
        SerializableLevel sl = niveles[this.seleccion];

        var mundo = infoNivel.transform.Find("Mundo");
        var id = infoNivel.transform.Find("ID");
        var nombre = infoNivel.transform.Find("Nombre");

        mundo.GetComponent<TextMeshProUGUI>().SetText("Mundo: " + sl.mundo);
        id.GetComponent<TextMeshProUGUI>().SetText("ID: " + sl.id);
        nombre.GetComponent<TextMeshProUGUI>().SetText("Nombre: " + sl.nombre);
    }
}

[Serializable]
public class SerializableEstadoList
{
    public List<Estado> list = new List<Estado>();
}