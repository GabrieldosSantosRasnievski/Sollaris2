using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventarioJogador : MonoBehaviour{
    public static InventarioJogador Instance;

    [System.Serializable]
    public class ItemSlot{
        public string nomeItem;
        public int quantidadeItem;
        public Sprite iconeItem;
        public bool podeConsumir;
        public float valorFome;
        public float danoItem;
        public Vector2 tamanhoHitbox = new Vector2(1f, 1f);
    }

    public List<ItemSlot> slotsRapidos = new List<ItemSlot>();
    public int limiteTiposSlotsRapidos = 9;
    public List<ItemSlot> inventario = new List<ItemSlot>();
    public int limiteTiposInventario = 20;
    public int slotSelecionado = 0;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            UsarItemSelecionado();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            slotSelecionado = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            slotSelecionado = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            slotSelecionado = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            slotSelecionado = 3;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            slotSelecionado = 4;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            slotSelecionado = 5;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            slotSelecionado = 6;
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            slotSelecionado = 7;
        }
        if(Input.GetKeyDown(KeyCode.Alpha9)){
            slotSelecionado = 8;
        }
    }

    public ItemSlot ObterItemSelecionado(){
        if(slotSelecionado >= 0 && slotSelecionado < slotsRapidos.Count){
            return slotsRapidos[slotSelecionado];
        }
        return null;
    }

    public bool TentarAdicionar(string nomeDoItem, Sprite icone, bool podeConsumir2, float fomeRecuperada, float danoRecebido, Vector2 tamanhoCaixa){
        foreach(ItemSlot slot in slotsRapidos){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.tamanhoHitbox = tamanhoCaixa;
                return true;
            }
        }
        foreach(ItemSlot slot in inventario){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.tamanhoHitbox = tamanhoCaixa;
                return true;
            }
        }
        foreach(ItemSlot slot in slotsRapidos){
            if(string.IsNullOrEmpty(slot.nomeItem)){
                slot.nomeItem = nomeDoItem;
                slot.iconeItem = icone;
                slot.quantidadeItem = 1;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.tamanhoHitbox = tamanhoCaixa;
                return true;
            }
        }
        if(slotsRapidos.Count < limiteTiposSlotsRapidos){
            ItemSlot novoSlot = new ItemSlot{ nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone, podeConsumir = podeConsumir2, valorFome = fomeRecuperada, danoItem = danoRecebido, tamanhoHitbox = tamanhoCaixa };
            slotsRapidos.Add(novoSlot);
            return true;
        }
        foreach(ItemSlot slot in inventario){
            if(string.IsNullOrEmpty(slot.nomeItem)){
                slot.nomeItem = nomeDoItem;
                slot.iconeItem = icone;
                slot.quantidadeItem = 1;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.tamanhoHitbox = tamanhoCaixa;
                return true;
            }
        }
        if(inventario.Count < limiteTiposInventario){
            ItemSlot novoSlot = new ItemSlot{ nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone, podeConsumir = podeConsumir2, valorFome = fomeRecuperada, danoItem = danoRecebido, tamanhoHitbox = tamanhoCaixa };
            inventario.Add(novoSlot);
            return true;
        }
        return false;
    }

    public ItemSlot ArremessarItemSelecionado(){
        if(slotsRapidos == null || slotSelecionado < 0 || slotSelecionado >= slotsRapidos.Count){
            return null;
        }
        ItemSlot slotAtivo = slotsRapidos[slotSelecionado];
        if(slotAtivo == null || string.IsNullOrEmpty(slotAtivo.nomeItem) || slotAtivo.quantidadeItem <= 0){
            return null;
        }
        ItemSlot ItemArremessado = new ItemSlot{
            nomeItem = slotAtivo.nomeItem,
            iconeItem = slotAtivo.iconeItem,
            quantidadeItem = 1,
            podeConsumir = slotAtivo.podeConsumir,
            valorFome = slotAtivo.valorFome,
            danoItem = slotAtivo.danoItem,
            tamanhoHitbox = slotAtivo.tamanhoHitbox
        };
        slotAtivo.quantidadeItem--;
        if(slotAtivo.quantidadeItem <= 0){
            slotAtivo.nomeItem = "";
            slotAtivo.iconeItem = null;
            slotAtivo.quantidadeItem = 0;
            slotAtivo.podeConsumir = false;
            slotAtivo.valorFome = 0f;
            slotAtivo.danoItem = 0f;
            slotAtivo.tamanhoHitbox = new Vector2(1f, 1f);
        }
        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if(ui != null){
            ui.AtualizarUI();
        }
        return ItemArremessado;
    }

    public void UsarItemSelecionado(){
        ItemSlot slotAtivo = ObterItemSelecionado();
        if(slotAtivo != null && slotAtivo.podeConsumir && slotAtivo.quantidadeItem > 0){
            GameObject jogador = GameObject.FindWithTag("Player");
            FomeJogador sistemaFome = null;
            if(jogador != null){
                sistemaFome = jogador.GetComponent<FomeJogador>();
            }
            if(sistemaFome != null){
                sistemaFome.Comer(slotAtivo.valorFome);
                slotAtivo.quantidadeItem--;
                if(slotAtivo.quantidadeItem <= 0){
                    slotAtivo.nomeItem = "";
                    slotAtivo.iconeItem = null;
                    slotAtivo.quantidadeItem = 0;
                    slotAtivo.podeConsumir = false;
                    slotAtivo.valorFome = 0f;
                    slotAtivo.danoItem = 0f;
                    slotAtivo.tamanhoHitbox = new Vector2(1f, 1f);
                }
                AbrirInventario ui = Object.FindAnyObjectByType<AbrirInventario>();
                if(ui != null){
                    ui.AtualizarUI();
                }
            }
        }
    }

    public void SalvarInventario(){
        PlayerPrefs.SetInt("SlotsRapidos_Count", slotsRapidos.Count);
        for(int i = 0; i < slotsRapidos.Count; i++){
            PlayerPrefs.SetString("SlotRapido_" + i + "_Nome", slotsRapidos[i].nomeItem);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Qtd", slotsRapidos[i].quantidadeItem);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Consumir", slotsRapidos[i].podeConsumir ? 1 : 0);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_Fome", slotsRapidos[i].valorFome);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_Dano", slotsRapidos[i].danoItem);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_HitboxX", slotsRapidos[i].tamanhoHitbox.x);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_HitboxY", slotsRapidos[i].tamanhoHitbox.y);
        }
        PlayerPrefs.SetInt("Inventario_Count", inventario.Count);
        for(int i = 0; i < inventario.Count; i++){
            PlayerPrefs.SetString("SlotNormal_" + i + "_Nome", inventario[i].nomeItem);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Qtd", inventario[i].quantidadeItem);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Consumir", inventario[i].podeConsumir ? 1 : 0);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_Fome", inventario[i].valorFome);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_Dano", inventario[i].danoItem);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_HitboxX", inventario[i].tamanhoHitbox.x);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_HitboxY", inventario[i].tamanhoHitbox.y);
        }
        PlayerPrefs.Save();
    }

    public void CarregarInventario(){
        if(PlayerPrefs.HasKey("SlotsRapidos_Count")){
            int totalRapidos = PlayerPrefs.GetInt("SlotsRapidos_Count");
            slotsRapidos.Clear();
            for(int i = 0; i < totalRapidos; i++){
                string nome = PlayerPrefs.GetString("SlotRapido_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotRapido_" + i + "_Qtd", 0);
                bool consumir = PlayerPrefs.GetInt("SlotRapido_" + i + "_Consumir", 0) == 1;
                float fome = PlayerPrefs.GetFloat("SlotRapido_" + i + "_Fome", 0f);
                float dano = PlayerPrefs.GetFloat("SlotRapido_" + i + "_Dano", 0f);
                float hx = PlayerPrefs.GetFloat("SlotRapido_" + i + "_HitboxX", 1f);
                float hy = PlayerPrefs.GetFloat("SlotRapido_" + i + "_HitboxY", 1f);
                Sprite icone = CarregarIconePorNome(nome);
                
                slotsRapidos.Add(new ItemSlot{
                    nomeItem = nome,
                    quantidadeItem = qtd,
                    iconeItem = icone,
                    podeConsumir = consumir,
                    valorFome = fome,
                    danoItem = dano,
                    tamanhoHitbox = new Vector2(hx, hy)
                });
            }
        }
        if(PlayerPrefs.HasKey("Inventario_Count")){
            int totalNormal = PlayerPrefs.GetInt("Inventario_Count");
            inventario.Clear();
            for(int i = 0; i < totalNormal; i++){
                string nome = PlayerPrefs.GetString("SlotNormal_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotNormal_" + i + "_Qtd", 0);
                bool consumir = PlayerPrefs.GetInt("SlotNormal_" + i + "_Consumir", 0) == 1;
                float fome = PlayerPrefs.GetFloat("SlotNormal_" + i + "_Fome", 0f);
                float dano = PlayerPrefs.GetFloat("SlotNormal_" + i + "_Dano", 0f);
                float hx = PlayerPrefs.GetFloat("SlotNormal_" + i + "_HitboxX", 1f);
                float hy = PlayerPrefs.GetFloat("SlotNormal_" + i + "_HitboxY", 1f);
                Sprite icone = CarregarIconePorNome(nome);
                
                inventario.Add(new ItemSlot{
                    nomeItem = nome,
                    quantidadeItem = qtd,
                    iconeItem = icone,
                    podeConsumir = consumir,
                    valorFome = fome,
                    danoItem = dano,
                    tamanhoHitbox = new Vector2(hx, hy)
                });
            }
        }
        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if(ui != null){
            ui.AtualizarUI();
        }
    }

    private Sprite CarregarIconePorNome(string nomeItem){
        if(string.IsNullOrEmpty(nomeItem)){
            return null;
        }
        Sprite spriteCarregado = Resources.Load<Sprite>("Icones/" + nomeItem);
        return spriteCarregado;
    }

    private void Start(){
        CarregarInventario();
    }

    private void OnDisable(){
        SalvarInventario();
    }

    private void OnApplicationQuit(){
        SalvarInventario();
    }
}