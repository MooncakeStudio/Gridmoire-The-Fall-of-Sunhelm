using System.Collections.Generic;
using UnityEngine;

public class NivelesDataStream : MonoBehaviour
{

    private string nivelesJson = "{\"list\":[{\"mundo\":1,\"id\":1,\"nombre\":\"\",\"enemigos\":[0,0,0],\"monedas\":25,\"xp\":100,\"historia\":0},{\"mundo\":1,\"id\":2,\"nombre\":\"\",\"enemigos\":[0,0,0],\"monedas\":30,\"xp\":225,\"historia\":1},{\"mundo\":1,\"id\":3,\"nombre\":\"\",\"enemigos\":[0,0,0],\"monedas\":35,\"xp\":350,\"historia\":0},{\"mundo\":1,\"id\":4,\"nombre\":\"\",\"enemigos\":[0,0,0],\"monedas\":40,\"xp\":475,\"historia\":0},{\"mundo\":1,\"id\":5,\"nombre\":\"\",\"enemigos\":[0,1,0],\"monedas\":45,\"xp\":600,\"historia\":2},{\"mundo\":1,\"id\":6,\"nombre\":\"\",\"enemigos\":[1,0,0],\"monedas\":50,\"xp\":725,\"historia\":0},{\"mundo\":1,\"id\":7,\"nombre\":\"\",\"enemigos\":[0,0,0],\"monedas\":55,\"xp\":850,\"historia\":0},{\"mundo\":1,\"id\":8,\"nombre\":\"\",\"enemigos\":[0,1,0],\"monedas\":60,\"xp\":975,\"historia\":0},{\"mundo\":1,\"id\":9,\"nombre\":\"\",\"enemigos\":[0,1,0],\"monedas\":65,\"xp\":1100,\"historia\":0},{\"mundo\":1,\"id\":10,\"nombre\":\"\",\"enemigos\":[1,0,1],\"monedas\":70,\"xp\":1225,\"historia\":3}]}";
    private ListaLevelSerializable lls = new ListaLevelSerializable();

    // Start is called before the first frame update

    public List<SerializableLevel> ObtenerLista()
    {
        if (!string.IsNullOrEmpty(nivelesJson))
        {
            lls = JsonUtility.FromJson<ListaLevelSerializable>(nivelesJson);
        }

        return this.lls.list;
    }
}