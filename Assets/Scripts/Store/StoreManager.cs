using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private GameObject character;

    //[SerializeField] private GameObject ejercito;
    //private EjercitoManager em;
    private GameObject lastCreated;
    private ListaPlayerSerializable spl = new ListaPlayerSerializable();

    private Rareza newRareza;

    [SerializeField] private List<string> nombres;
    [SerializeField] private List<string> titulos;

    [SerializeField] private List<Sprite> flequillos;
    [SerializeField] private List<Sprite> pelos;
    [SerializeField] private List<Sprite> pesta�as;
    [SerializeField] private List<Sprite> orejas;
    [SerializeField] private List<Sprite> narices;
    [SerializeField] private List<Sprite> bocas;
    [SerializeField] private List<Sprite> extras;
    [SerializeField] private List<Sprite> cejas;
    [SerializeField] private List<Sprite> ropas;

    public void Start()
    {
        string json = PlayerPrefs.GetString("ejercito");

        if (!string.IsNullOrEmpty(json))
        {
            spl = JsonUtility.FromJson<ListaPlayerSerializable>(json);
        }
    }
    public void Awake()
    {
        newRareza = Rareza.COMUN;
    }

    public void generateRandomCharacter()
    {
        if(lastCreated != null)
        {
            Destroy(lastCreated);
        }
        var newCharacter = Instantiate(character);

        string newNombre = nombres[Random.Range(0, nombres.Count)];
        string newTitulo = titulos[Random.Range(0, titulos.Count)];
        string newNombrePersonaje = newNombre + " " + newTitulo;

        /////// CHARACTER CUSTOMIZATION ///////

        var newFlequillo = newCharacter.transform.Find("Flequillo").GetComponent<SpriteRenderer>();
        var iFlequillo = Random.Range(0, flequillos.Count);
        newFlequillo.sprite = flequillos[iFlequillo];

        var newPelo = newCharacter.transform.Find("Pelo").GetComponent<SpriteRenderer>();
        var iPelo = Random.Range(0, pelos.Count);
        newPelo.sprite = pelos[iPelo];

        var newPesta�as = newCharacter.transform.Find("Pesta�as").GetComponent<SpriteRenderer>();
        var iPest = Random.Range(0, pesta�as.Count);
        newPesta�as.sprite = pesta�as[iPest];

        var newOrejas = newCharacter.transform.Find("Orejas").GetComponent<SpriteRenderer>();
        var iOrej = Random.Range(0, orejas.Count);
        newOrejas.sprite = orejas[iOrej];

        var newNarices = newCharacter.transform.Find("Nariz").GetComponent<SpriteRenderer>();
        var iNari = Random.Range(0, narices.Count);
        newNarices.sprite = narices[iNari];

        var newBoca = newCharacter.transform.Find("Boca").GetComponent<SpriteRenderer>();
        var iBoca = Random.Range(0, bocas.Count);
        newBoca.sprite = bocas[iBoca];

        var newExtra = newCharacter.transform.Find("Extra").GetComponent<SpriteRenderer>();
        var iExtra = Random.Range(0, extras.Count);
        newExtra.sprite = extras[iExtra];

        var newCejas = newCharacter.transform.Find("Cejas").GetComponent<SpriteRenderer>();
        var iCejas = Random.Range(0, cejas.Count);
        newCejas.sprite = cejas[iCejas];

        var ropa = newCharacter.transform.Find("Ropa").GetComponent<SpriteRenderer>();
        var iRopa = Random.Range(0, ropas.Count);
        ropa.sprite = ropas[iRopa];


        var RP = Random.Range(0, 255) / 255f;
        var GP = Random.Range(0, 255) / 255f;
        var BP = Random.Range(0, 255) / 255f;

        newFlequillo.material.color = new Color(RP, GP, BP);
        newPelo.material.color = new Color(RP, GP, BP);

        var RI = Random.Range(0, 255) / 255f;
        var GI = Random.Range(0, 255) / 255f;
        var BI = Random.Range(0, 255) / 255f;

        var newIris = newCharacter.transform.Find("Ojos").transform.Find("Iris").GetComponent<SpriteRenderer>();
        newIris.material.color = new Color(RI, GI, BI);

        /////// ATTACK SELECTION ///////

        var tipoAtaque = (TipoAtaque)Random.Range(0, 4);

        ///// CHARACTER GENERATION /////

        Personaje pers = new Personaje(newNombrePersonaje, tipoAtaque, this.newRareza);

        newCharacter.GetComponent<PlayerController>().setPersonaje(pers);

        var ataque = newCharacter.transform.Find("Ataque").GetComponent<SpriteRenderer>();

        switch (tipoAtaque)
        {
            case TipoAtaque.SINGLE:
                ataque.color = Color.red;
                break;
            case TipoAtaque.ROW:
                ataque.color = Color.blue;
                break;
            case TipoAtaque.COLUMN:
                ataque.color = Color.yellow;
                break;
            case TipoAtaque.GRID:
                ataque.color = Color.black;
                break;
            case TipoAtaque.HEAL:
                ataque.color = Color.green;
                break;
        }

        lastCreated = newCharacter;

        var sp = new SerializablePlayer(iFlequillo,iPelo,iPest,iOrej,iNari,iBoca,iExtra,iCejas,
            iRopa,RP,GP,BP,RI,GI,BI,pers.GetAtaque(),pers.GetDefensa(),pers.GetVida(), 
            (int)pers.GetTipoAtaque());

        spl.list.Add(sp);

        PlayerPrefs.SetString("ejercito",JsonUtility.ToJson(spl));
        PlayerPrefs.Save();

    }
}
