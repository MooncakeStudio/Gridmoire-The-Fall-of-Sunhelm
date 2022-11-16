using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    // atributos

    //private Grid gridIA = new Grid(3);
    //private Grid gridPlayer = new Grid(3);

    [SerializeField] private GridManager gridIA;
    [SerializeField] private GridManager gridPlayer;

    private IAManager ia = new IAManager();
    private bool initialize = false;

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] public List<PlayerController> ejercitoJugador = new List<PlayerController>();
    [SerializeField] private List<EnemigoController> ejercitoEnemigo = new List<EnemigoController>();

    [SerializeField] private List<float> vidasEnemigos = new List<float>();
    [SerializeField] private List<float> vidasJugador = new List<float>();

    [SerializeField] private TextMeshProUGUI textoTurno;

    private DataToBattle datosBatalla;

    [SerializeField] private CargarPersBatalla datosCargados;

    [SerializeField] private List<VidaEnemigosUI> vidaEnemigos;

    [SerializeField] private List<Sprite> ataques;

    // getter & setters

    public GridManager GetGridIA() { return gridIA; }
    public GridManager GetGridPlayer() { return gridPlayer; }
    public IAManager GetIAManager() { return ia; }
    public bool isInitialized() { return initialize; }
    

    public void SetGridIA(GridManager gridIA) { this.gridIA = gridIA; }
    public void SetGridPlayer(GridManager gridPlayer) { this.gridPlayer = gridPlayer; }
    public void SetIAManager(IAManager ia) { this.ia = ia; }

    // Metodos
    private void Start()
    {
        
    }

    private void Update()
    {
        if (initialize)
        {
            if (QuedanEnemigos(ejercitoEnemigo) && QuedanAliados(ejercitoJugador))
            {
                if (GetComponent<BattleController>().getTurnos() < ejercitoJugador.Count)
                {
                    //Debug.Log(GetComponent<BattleController>().getTurnos());
                    
                }
                else if (QuedanEnemigos(ejercitoEnemigo))
                {
                    foreach (var enemigo in ejercitoEnemigo)
                    {
                        vidasEnemigos.Add(enemigo.getEnemigo().GetVida());
                    }

                    ia.RealizarTurno(gridIA, gridPlayer, textoTurno);

                    foreach (var enemigo in ejercitoJugador)
                    {
                        vidasJugador.Add(enemigo.getPersonaje().GetVida());
                    }
                    GetComponent<BattleController>().resetTurno();

                }
                

            }
            else
            {
                if (QuedanAliados(ejercitoJugador))
                {
                    SceneManager.LoadScene("Win");
                    Destroy(FindObjectOfType<DataToBattle>().gameObject);
                }
                else
                {
                    SceneManager.LoadScene("GameOver");
                    Destroy(FindObjectOfType<DataToBattle>().gameObject);
                }
                
            }
        }
        
    }

    private bool QuedanAliados(List<PlayerController> comprobar)
    {
        var result = false;
        List<PlayerController> persEliminar = new List<PlayerController>();
        foreach(var personaje in comprobar)
        {
            if(personaje.getPersonaje().GetVida() <= 0)
            {
                persEliminar.Add(personaje);
            }
            if(personaje.getPersonaje().GetVida() > 0)
            {
                result = true;      
            }
        }

        foreach(var eliminados in persEliminar)
        {
            comprobar.Remove(eliminados);
            Destroy(eliminados.gameObject);
        }
        return result;
    }

    private bool QuedanEnemigos(List<EnemigoController> comprobar)
    {
        var result = false;
        List<EnemigoController> persEliminar = new List<EnemigoController>();
        foreach (var personaje in comprobar)
        {
            if (personaje.getEnemigo().GetVida() <= 0)
            {
                persEliminar.Add(personaje);
            }
            if (personaje.getEnemigo().GetVida() > 0)
            {
                result = true;
            }
        }

        foreach (var eliminados in persEliminar)
        {
            comprobar.Remove(eliminados);
            Destroy(eliminados.gameObject);
        }
        return result;
    }

    private void rellenarGrid(GridManager grid)
    {
        int x, y;
        int nEnemigos = 0;
        Transform celdaTransform = transform;
        datosBatalla = FindObjectOfType<DataToBattle>();
        Celda cell = new Celda();
        
        while (nEnemigos < 3)
        {
            do
            {
                x = Random.Range(0, 3);
                y = 0;
            } while ( grid.getGridInfo().GetCeldas()[x, y].IsOccupied());

           foreach(var celda in grid.getCeldas())
            {
                if(celda.getCelda().GetX() == x && celda.getCelda().GetY() == y)
                {
                    celdaTransform = celda.gameObject.transform;
                    cell = celda.getCelda();
                }
            }
            var objEnemigo = Instantiate(datosBatalla.getEnemigos()[nEnemigos], celdaTransform.position, Quaternion.identity);
            objEnemigo.transform.SetParent(celdaTransform);
            switch (x)
            {
                case 0:
                    if (y == 0)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x -0.017f, 
                            celdaTransform.position.y + +0.422f, 1f);
                    }
                    else if (y == 1)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.005f, 
                            celdaTransform.position.y + 0.465f, 1f);
                    }
                    else
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.117f, 
                            celdaTransform.position.y + 0.45f, 1f);
                    }
                    objEnemigo.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 0;
                    break;
                case 1:
                    if (y == 0)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x -0.014f, 
                            celdaTransform.position.y + 0.539f, 1f);
                    }
                    else if (y == 1)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.039f, 
                            celdaTransform.position.y + 0.516f, 1f);
                    }
                    else
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.03f, 
                            celdaTransform.position.y + 0.47f, 1f);
                    }
                    objEnemigo.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;
                    break;
                case 2:
                    if (y == 0)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x -0.111f, 
                            celdaTransform.position.y + 0.478f, 1f);
                    }
                    else if (y == 1)
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.07f, 
                            celdaTransform.position.y + 0.655f, 1f);
                    }
                    else
                    {
                        objEnemigo.transform.position = new Vector3(celdaTransform.position.x + 0.044f, 
                            celdaTransform.position.y + 0.6f, 1f);
                    }
                    objEnemigo.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
            }
            objEnemigo.GetComponent<EnemigoController>().crearEnemigo();
            grid.getGridInfo().GetCeldas()[x, y].SetPersonaje(objEnemigo);
            grid.getGridInfo().GetCeldas()[x, y].ChangeOccupied();

            switch (objEnemigo.GetComponent<EnemigoController>().getEnemigo().GetTipoAtaque())
            {
                case TipoAtaque.SINGLE:
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = ataques[0];
                    break;
                case TipoAtaque.COLUMN:
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = ataques[1];
                    break;
                case TipoAtaque.ROW:
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = ataques[2];
                    break;
                case TipoAtaque.GRID:
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = ataques[3];
                    break;
                case TipoAtaque.HEAL:
                    objEnemigo.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = ataques[4];
                    break;
            }
            ejercitoEnemigo.Add(objEnemigo.GetComponent<EnemigoController>());
            nEnemigos++;

            foreach(var barra in vidaEnemigos)
            {
                var celdita = barra.GetCelda().getCelda();
                if(celdita.GetX() == cell.GetX() && celdita.GetY() == cell.GetY())
                {
                    barra.setPlayer(objEnemigo.GetComponent<EnemigoController>());
                    barra.activar();
                }
            }
        }
    }

    public void SimulaPartida()
    {
        

        // Inicialzar grids
        rellenarGrid(gridIA);

        //gridPlayer = GameObject.FindGameObjectWithTag("PlayerGrid").GetComponent<GridManager>();

        gridPlayer = datosCargados.GetGridManager();

        ia.SetEjercito(ejercitoEnemigo);
        initialize = true;
    }


    public void addPersonaje(PlayerController add) { ejercitoJugador.Add(add); }
}
