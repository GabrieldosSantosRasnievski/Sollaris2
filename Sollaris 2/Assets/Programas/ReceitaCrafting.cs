using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NovaReceita", menuName = "Crafting/Receita")]
public class ReceitaCrafting : ScriptableObject{
    [System.Serializable]
    public struct Ingrediente{
        public string nomeItem;
        public int quantidade;
    }
    public List<Ingrediente> ingredientes = new List<Ingrediente>();
    public string nomeItemResultado;
    public Sprite iconeResultado;
    public int quantidadeResultado = 1;
    public bool podeConsumirResultado;
    public float valorFomeResultado;
    public float danoResultado;
    public Vector2 tamanhoHitboxResultado = new Vector2(1f, 1f);
    public bool quebraAoAtingirResultado;
}