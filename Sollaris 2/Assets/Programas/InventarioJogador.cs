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
            Debug.Log("Clique detetado! A tentar usar o item...");
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

    public bool TentarAdicionar(string nomeDoItem, Sprite icone, bool podeConsumir2, float fomeRecuperada){
        foreach(ItemSlot slot in slotsRapidos){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                return true;
            }
        }
        foreach(ItemSlot slot in inventario){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
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
                return true;
            }
        }
        if(slotsRapidos.Count < limiteTiposSlotsRapidos){
            ItemSlot novoSlot = new ItemSlot{ nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone, podeConsumir = podeConsumir2, valorFome = fomeRecuperada };
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
                return true;
            }
        }
        if(inventario.Count < limiteTiposInventario){
            ItemSlot novoSlot = new ItemSlot{ nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone, podeConsumir = podeConsumir2, valorFome = fomeRecuperada };
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
            quantidadeItem = 1
        };
        slotAtivo.quantidadeItem--;
        if(slotAtivo.quantidadeItem <= 0){
            slotAtivo.nomeItem = "";
            slotAtivo.iconeItem = null;
            slotAtivo.quantidadeItem = 0;
            slotAtivo.podeConsumir = false;
            slotAtivo.valorFome = 0f;
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
                }
                AbrirInventario ui = Object.FindAnyObjectByType<AbrirInventario>();
                if(ui != null){
                    ui.AtualizarUI();
                }
                Debug.Log("Sucesso! Item consumido e fome aumentada.");
            }
            else{
                Debug.LogWarning("Não foi possível encontrar o FomeJogador no objeto com a tag 'Player'!");
            }
        }
    }

    public void SalvarInventario(){
        PlayerPrefs.SetInt("SlotsRapidos_Count", slotsRapidos.Count);
        for(int i = 0; i < slotsRapidos.Count; i++){
            PlayerPrefs.SetString("SlotRapido_" + i + "_Nome", slotsRapidos[i].nomeItem);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Qtd", slotsRapidos[i].quantidadeItem);
        }
        PlayerPrefs.SetInt("Inventario_Count", inventario.Count);
        for(int i = 0; i < inventario.Count; i++){
            PlayerPrefs.SetString("SlotNormal_" + i + "_Nome", inventario[i].nomeItem);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Qtd", inventario[i].quantidadeItem);
        }
        PlayerPrefs.Save();
        Debug.Log("Ta funcionando!");
    }

    public void CarregarInventario(){
        if(PlayerPrefs.HasKey("SlotsRapidos_Count")){
            int totalRapidos = PlayerPrefs.GetInt("SlotsRapidos_Count");
            slotsRapidos.Clear();

            for(int i = 0; i < totalRapidos; i++){
                string nome = PlayerPrefs.GetString("SlotRapido_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotRapido_" + i + "_Qtd", 0);
                Sprite icone = CarregarIconePorNome(nome);

                slotsRapidos.Add(new ItemSlot{
                    nomeItem = nome,
                    quantidadeItem = qtd,
                    iconeItem = icone
                });
            }
        }
        if(PlayerPrefs.HasKey("Inventario_Count")){
            int totalNormal = PlayerPrefs.GetInt("Inventario_Count");
            inventario.Clear();

            for(int i = 0; i < totalNormal; i++){
                string nome = PlayerPrefs.GetString("SlotNormal_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotNormal_" + i + "_Qtd", 0);
                Sprite icone = CarregarIconePorNome(nome);

                inventario.Add(new ItemSlot{
                    nomeItem = nome,
                    quantidadeItem = qtd,
                    iconeItem = icone
                });
            }
        }
        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if(ui != null){
            ui.AtualizarUI();
        }

        Debug.Log("Inventário Completo Carregado!");
    }

    private Sprite CarregarIconePorNome(string nomeItem){
        if(string.IsNullOrEmpty(nomeItem)){
            return null;
        }
        Sprite spriteCarregado = Resources.Load<Sprite>("Icones/" + nomeItem);

        if(spriteCarregado == null){
            Debug.LogWarning("Não foi possível encontrar o ícone para o item: " + nomeItem + " na pasta Resources/Icones/");
        }

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