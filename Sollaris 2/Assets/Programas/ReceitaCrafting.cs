using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NovaReceita", menuName = "Crafting/Receita")]
public class ReceitaCrafting : ScriptableObject {
    [System.Serializable]
    public struct Ingrediente {
        public string nomeItem;
        public int quantidade;
    }
    public List<Ingrediente> ingredientes = new List<Ingrediente>();
    public string nomeItemResultado;
    public Sprite iconeResultado;
    public int quantidadeResultado = 1;
    public bool podeConsumirResultado = false;
    public int valorFomeResultado = 0;
    public int danoResultado = 0;
    public Vector2 tamanhoHitboxResultado = new Vector2(1f, 1f);
    public bool quebraAoAtingirResultado = false;
    public bool ehMachadoResultado = false;
    public bool ehPicaretaResultado = false;
    public bool ehCortanteResultado = false;
    public GameObject prefabParaConstruir;
}