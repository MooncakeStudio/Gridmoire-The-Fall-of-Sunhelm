using System.Collections.Generic;
using UnityEngine;

public class NivelesDataStream : MonoBehaviour
{

    private string nivelesJson = "{\"list\": [{\"mundo\": 1,\"id\": 1,\"nombre\": \"Patata\",\"enemigos\": [0, 0, 0],\"monedas\": 30,\"xp\": 30},{\"mundo\": 1,\"id\": 2,\"nombre\": \"Zanahoria\",\"enemigos\": [0, 0, 0],\"monedas\": 75,\"xp\": 40},{\"mundo\": 1,\"id\": 3,\"nombre\": \"Puerro\",\"estado\": 2,\"enemigos\": [0, 1, 0],\"monedas\": 100,\"xp\": 50}]}";

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
