using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    private GridManager gridEnemigo;
    private GridManager gridAliado;
    private GameObject playerSelected;

    private List<SeleccionableManager> seleccionables = new List<SeleccionableManager>();

    private int turnosJugados=0;

    [SerializeField] private AudioClip clip;

    private Dictionary<int, List<Color>> colores = new Dictionary<int, List<Color>>();
    [SerializeField] private CeldaManager cellSelected;

    [SerializeField] private List<AudioClip> efectosSonido;

    [Header("Fondo")]
    [SerializeField] private List<Sprite> fondos;
    [SerializeField] private SpriteRenderer fondo;
    int keyDic = 0;

    private void Awake()
    {
        GameManager.instance.setClip(clip);
    }
    private void Start()
    {
        var obj = FindObjectOfType<NivelDataHandler>();

        fondo.sprite = fondos[obj.GetFondo()];


        gridEnemigo = GetComponent<LevelFlow>().GetGridIA();
        gridAliado = GetComponent<LevelFlow>().GetGridPlayer();
    }
    void Update()
    {
        if (GetComponent<LevelFlow>().isInitialized())
        {
            if (turnosJugados < GetComponent<LevelFlow>().ejercitoJugador.Count)
            {

                if (Input.GetMouseButtonUp(0))
                {
                    var pointer = Input.mousePosition;
                    inputController(pointer);
                }
                /*else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
                {
                    var pointer = Input.GetTouch(0).position;
                    inputController(pointer);
                }*/
            }
        }
        
        
    }

    private void inputController(Vector3 position)
    {
        
        var pos = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);
        foreach(var hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.gameObject.GetComponent<SeleccionableManager>().isSelectable())
                {
                    if (playerSelected != null)
                    {
                        //Debug.Log("Hola");
                        keyDic--;
                        var lista = colores[keyDic];
                        var indice = 0;
                        foreach (var sprite in playerSelected.GetComponentsInChildren<SpriteRenderer>())
                        {
                            sprite.color = lista[indice];
                            indice++;
                        }
                        colores.Remove(keyDic);

                        if(cellSelected != null)
                        {
                            cellSelected = null;
                            resetResalto();
                        }


                    }
                    GetComponent<LevelFlow>().GetScriptAudio().PlaySound(efectosSonido[0]);
                    playerSelected = hit.collider.gameObject;
                    var listacolores = new List<Color>();
                    foreach (var sprite in playerSelected.GetComponentsInChildren<SpriteRenderer>())
                    {
                        listacolores.Add(sprite.color);
                        if (!sprite.transform.name.Equals("Ataque"))
                            sprite.color = Color.blue;
                    }
                    colores.Add(keyDic, listacolores);
                    keyDic++;
                }
                else
                {
                    Debug.Log("Personaje no seleccionable");
                }   

            }
            else if (hit.collider.CompareTag("CellEnemy"))
            {
                if (playerSelected != null && playerSelected.GetComponent<PlayerController>().getPersonaje().GetTipoAtaque()
                    != TipoAtaque.HEAL && cellSelected == null)
                {
                    cellSelected = hit.collider.gameObject.GetComponent<CeldaManager>();
                    resaltar();
                }

                else if (cellSelected != null)
                {
                    var celda = hit.collider.gameObject.GetComponent<CeldaManager>();
                    var coincide = false;
                    switch (playerSelected.GetComponent<PlayerController>().getPersonaje().GetTipoAtaque())
                    {
                        case TipoAtaque.SINGLE:
                            coincide = comprobarSingle(cellSelected, celda);
                            break;
                        case TipoAtaque.COLUMN:
                            coincide = comprobarColumn(cellSelected, celda);
                            break;
                        case TipoAtaque.ROW:
                            coincide = comprobarRow(cellSelected, celda);
                            break;
                        case TipoAtaque.GRID:
                            coincide = true;
                            break;
                    }
                    if (coincide)
                    {
                        playerSelected.GetComponent<Attack>().performAttack(gridEnemigo, cellSelected.getCelda());
                        turnosJugados++;
                        seleccionables.Add(playerSelected.GetComponent<SeleccionableManager>());
                        playerSelected.GetComponent<SeleccionableManager>().notSelectable();

                        foreach (var sprite in playerSelected.GetComponentsInChildren<SpriteRenderer>())
                        {
                            sprite.color = Color.gray;
                        }
                        playerSelected = null;
                        cellSelected = null;
                        resetResalto();
                    }
                    else
                    {
                        cellSelected = celda;
                        resetResalto();
                        resaltar();
                    }

                }


            }
            else if (hit.collider.CompareTag("CellPlayer"))
            {
                if (playerSelected != null
                    && playerSelected.GetComponent<PlayerController>().getPersonaje().GetTipoAtaque()
                    == TipoAtaque.HEAL && cellSelected == null)
                {
                    var celda = hit.collider.gameObject.GetComponent<CeldaManager>();
                    cellSelected = hit.collider.gameObject.GetComponent<CeldaManager>();
                    resaltarHeal(cellSelected);
                }

                else if (cellSelected != null)
                {
                    var celda = hit.collider.gameObject.GetComponent<CeldaManager>();
                    var coincide = comprobarSingle(cellSelected,celda);
                    if(coincide)
                    {
                        playerSelected.GetComponent<Attack>().performAttack(gridAliado, cellSelected.getCelda());
                        turnosJugados++;
                        seleccionables.Add(playerSelected.GetComponent<SeleccionableManager>());
                        playerSelected.GetComponent<SeleccionableManager>().notSelectable();
                        foreach (var sprite in playerSelected.GetComponentsInChildren<SpriteRenderer>())
                        {
                            sprite.color = Color.gray;
                        }
                        playerSelected = null;
                        cellSelected = null;
                        resetResalto();
                    }
                    else
                    {
                        cellSelected = celda;
                        resetResalto();
                        resaltarHeal(cellSelected);
                    }
                    
                }
            }
        }
    }

    public int getTurnos() { return turnosJugados; }

    public void resetTurno() 
    { 
        turnosJugados = 0;
        var indice = 0;
        foreach(var personaje in seleccionables)
        {
            personaje.canSelectable();
            var indiceColores = 0;
            if(colores.Count > 0)
            {

                if (colores.ContainsKey(indice))
                {
                    var lista = colores[indice];
                    foreach (var sprite in personaje.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sprite.color = lista[indiceColores];
                        indiceColores++;
                    }
                }
                
                indice++;
            }
            
        }
        keyDic = 0;
        colores.Clear();
        seleccionables.Clear();
    }

    public void resetColores(SeleccionableManager muerto)
    {
        Dictionary<int, List<Color>> copia = new Dictionary<int, List<Color>>();

        var indice = 0;
        var key = 0;

        foreach (var personaje in seleccionables)
        {
            if (personaje.Equals(muerto))
            {
                
            }
            else
            {
                copia.Add(key, colores[indice]);
                key++;
            }
            indice++;

        }
        Debug.Log(copia.Keys);
        colores.Clear();
        colores = copia;
        Debug.Log(colores.Keys);
    }

    private void resaltarSingle(CeldaManager celda) 
    {
        celda.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void resaltarRow(CeldaManager celda) 
    {
        foreach(var cell in gridEnemigo.getCeldas())
        {
            if(cell.getCelda().GetX() == celda.getCelda().GetX())
            {
                cell.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void resaltarColumn(CeldaManager celda) 
    {
        foreach (var cell in gridEnemigo.getCeldas())
        {
            if (cell.getCelda().GetY() == celda.getCelda().GetY())
            {
                cell.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private void resaltarGrid() 
    { 
        foreach(var cell in gridEnemigo.getCeldas())
        {
            cell.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void resaltarHeal(CeldaManager celda)
    {
        celda.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void resetResalto() 
    {
        foreach(var cell in gridAliado.getCeldas())
        {
            cell.GetComponent<SpriteRenderer>().color = Color.white;
        }

        foreach (var cell in gridEnemigo.getCeldas())
        {
            cell.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private bool comprobarSingle(CeldaManager celdaSelected, CeldaManager cellNew)
    {
        return celdaSelected.getCelda().GetX() == cellNew.getCelda().GetX()
            && celdaSelected.getCelda().GetY() == cellNew.getCelda().GetY();
    }

    private bool comprobarColumn(CeldaManager celdaSelected, CeldaManager cellNew)
    {
        return celdaSelected.getCelda().GetY() == cellNew.getCelda().GetY();
    }

    private bool comprobarRow(CeldaManager celdaSelected, CeldaManager cellNew)
    {
        return celdaSelected.getCelda().GetX() == cellNew.getCelda().GetX();
    }

    private void resaltar()
    {
        switch (playerSelected.GetComponent<PlayerController>().getPersonaje().GetTipoAtaque())
        {
            case TipoAtaque.SINGLE:
                resaltarSingle(cellSelected);
                break;
            case TipoAtaque.COLUMN:
                resaltarColumn(cellSelected);
                break;
            case TipoAtaque.ROW:
                resaltarRow(cellSelected);
                break;
            case TipoAtaque.GRID:
                resaltarGrid();
                break;
        }
    }

    public void eliminarMuerto(SeleccionableManager muerto)
    {
        seleccionables.Remove(muerto);
    }
}
