using UnityEngine;

[System.Serializable]
public class Outcome
{
    public string label; // "x0", "x2", "x3", "x4" - solo para referencia
    [Range(0, 100)] public float weight; // peso relativo (no necesita sumar 100 exacto)
    public float multiplier;
}

public class SlotMachine : MonoBehaviour
{
    public Outcome[] outcomes = new Outcome[]
    {
        new Outcome { label = "x0", weight = 55, multiplier = 0f },
        new Outcome { label = "x2", weight = 30, multiplier = 2f },
        new Outcome { label = "x3", weight = 10, multiplier = 3f },
        new Outcome { label = "x4", weight = 5,  multiplier = 4f },
    };

    public float Roll()
    {
        float total = 0f;
        foreach (var o in outcomes) total += o.weight;

        float rand = Random.Range(0f, total);
        float acc = 0f;

        foreach (var o in outcomes)
        {
            acc += o.weight;
            if (rand <= acc) return o.multiplier;
        }
        return 0f; // fallback de seguridad
    }
}