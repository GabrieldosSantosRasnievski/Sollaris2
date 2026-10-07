using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventarioJogador : MonoBehaviour {
    public static InventarioJogador Instance;

    [System.Serializable]
    public class ItemSlot{
        public string nomeItem = "";
        public int quantidadeItem = 0;
        public Sprite iconeItem = null;
        public bool podeConsumir = false;
        public float valorFome = 0f;
        public float danoItem = 0f;
        public float defesaItem = 0f;
        public Vector2 tamanhoHitbox = new Vector2(1f, 1f);
        public bool quebraAoAtingir = false;
        public bool podeColocarCabeca = false;
        public bool podeColocarTorso = false;
        public bool podeColocarCintura = false;
        public bool podeColocarCalca = false;
        public bool podeColocarBota = false;
        public bool ehMachado = false;
        public bool ehPicareta = false;

        public ItemSlot() { }
        
        public ItemSlot(string nome, int qtd, Sprite icone, bool consumir, float fome, float dano, float defesa, Vector2 hitbox, bool quebra, bool cabeca = false, bool torso = false, bool cintura = false, bool calca = false, bool bota = false, bool machado = false, bool picareta = false){
            nomeItem = nome;
            quantidadeItem = qtd;
            iconeItem = icone;
            podeConsumir = consumir;
            valorFome = fome;
            danoItem = dano;
            defesaItem = defesa;
            tamanhoHitbox = hitbox;
            quebraAoAtingir = quebra;
            podeColocarCabeca = cabeca;
            podeColocarTorso = torso;
            podeColocarCintura = cintura;
            podeColocarCalca = calca;
            podeColocarBota = bota;
            ehMachado = machado;
            ehPicareta = picareta;
        }
    }

    public List<ItemSlot> slotsRapidos = new List<ItemSlot>();
    public int limiteTiposSlotsRapidos = 9;
    public List<ItemSlot> inventario = new List<ItemSlot>();
    public int limiteTiposInventario = 20;
    public int slotSelecionado = 0;
    public GameObject prefabItemArremessado;
    public ItemSlot slotCabeca = new ItemSlot();
    public ItemSlot slotTorso = new ItemSlot();
    public ItemSlot slotCintura = new ItemSlot();
    public ItemSlot slotCalca = new ItemSlot();
    public ItemSlot slotBota = new ItemSlot();

    private void Awake(){
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        while (slotsRapidos.Count < limiteTiposSlotsRapidos){
            slotsRapidos.Add(new ItemSlot());
        }
        while (inventario.Count < limiteTiposInventario) {
            inventario.Add(new ItemSlot());
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            UsarItemSelecionado();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            slotSelecionado = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            slotSelecionado = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            slotSelecionado = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)){
            slotSelecionado = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)){
            slotSelecionado = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)){
            slotSelecionado = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)){
            slotSelecionado = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)){
            slotSelecionado = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)){
            slotSelecionado = 8;
        }

        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if (ui != null) {
            ui.AtualizarUI();
        }
    }
    public ItemSlot ObterItemSelecionado(){
        if (slotSelecionado >= 0 && slotSelecionado < slotsRapidos.Count){
            return slotsRapidos[slotSelecionado];
        }
        return null;
    }
    public bool TentarAdicionar(string nomeDoItem, Sprite icone, bool podeConsumir2, float fomeRecuperada, float danoRecebido, Vector2 tamanhoCaixa, bool quebraInimigo, float defesaRecebida = 0f, bool cabeca = false, bool torso = false, bool cintura = false, bool calca = false, bool bota = false, bool machado = false, bool picareta = false) {
        foreach (ItemSlot slot in slotsRapidos) {
            if (slot.nomeItem == nomeDoItem) {
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.defesaItem = defesaRecebida;
                slot.tamanhoHitbox = tamanhoCaixa;
                slot.quebraAoAtingir = quebraInimigo;
                slot.podeColocarCabeca = cabeca;
                slot.podeColocarTorso = torso;
                slot.podeColocarCintura = cintura;
                slot.podeColocarCalca = calca;
                slot.podeColocarBota = bota;
                slot.ehMachado = machado;
                slot.ehPicareta = picareta;
                return true;
            }
        }
        foreach (ItemSlot slot in inventario) {
            if (slot.nomeItem == nomeDoItem) {
                slot.quantidadeItem++;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.defesaItem = defesaRecebida;
                slot.tamanhoHitbox = tamanhoCaixa;
                slot.quebraAoAtingir = quebraInimigo;
                slot.podeColocarCabeca = cabeca;
                slot.podeColocarTorso = torso;
                slot.podeColocarCintura = cintura;
                slot.podeColocarCalca = calca;
                slot.podeColocarBota = bota;
                slot.ehMachado = machado;
                slot.ehPicareta = picareta;
                return true;
            }
        }
        foreach (ItemSlot slot in slotsRapidos) {
            if (string.IsNullOrEmpty(slot.nomeItem)) {
                slot.nomeItem = nomeDoItem;
                slot.iconeItem = icone;
                slot.quantidadeItem = 1;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.defesaItem = defesaRecebida;
                slot.tamanhoHitbox = tamanhoCaixa;
                slot.quebraAoAtingir = quebraInimigo;
                slot.podeColocarCabeca = cabeca;
                slot.podeColocarTorso = torso;
                slot.podeColocarCintura = cintura;
                slot.podeColocarCalca = calca;
                slot.podeColocarBota = bota;
                slot.ehMachado = machado;
                slot.ehPicareta = picareta;
                return true;
            }
        }
        foreach (ItemSlot slot in inventario) {
            if (string.IsNullOrEmpty(slot.nomeItem)) {
                slot.nomeItem = nomeDoItem;
                slot.iconeItem = icone;
                slot.quantidadeItem = 1;
                slot.podeConsumir = podeConsumir2;
                slot.valorFome = fomeRecuperada;
                slot.danoItem = danoRecebido;
                slot.defesaItem = defesaRecebida;
                slot.tamanhoHitbox = tamanhoCaixa;
                slot.quebraAoAtingir = quebraInimigo;
                slot.podeColocarCabeca = cabeca;
                slot.podeColocarTorso = torso;
                slot.podeColocarCintura = cintura;
                slot.podeColocarCalca = calca;
                slot.podeColocarBota = bota;
                slot.ehMachado = machado;
                slot.ehPicareta = picareta;
                return true;
            }
        }

        return false;
    }
    public ItemSlot ArremessarItemSelecionado(){
        if (slotsRapidos == null || slotSelecionado < 0 || slotSelecionado >= slotsRapidos.Count){
            return null;
        }
        ItemSlot slotAtivo = slotsRapidos[slotSelecionado];
        if (slotAtivo == null || string.IsNullOrEmpty(slotAtivo.nomeItem) || slotAtivo.quantidadeItem <= 0){
            return null;
        }
        ItemSlot itemArremessado = new ItemSlot{
            nomeItem = slotAtivo.nomeItem,
            iconeItem = slotAtivo.iconeItem,
            quantidadeItem = 1,
            podeConsumir = slotAtivo.podeConsumir,
            valorFome = slotAtivo.valorFome,
            danoItem = slotAtivo.danoItem,
            defesaItem = slotAtivo.defesaItem,
            tamanhoHitbox = slotAtivo.tamanhoHitbox,
            quebraAoAtingir = slotAtivo.quebraAoAtingir,
            podeColocarCabeca = slotAtivo.podeColocarCabeca,
            podeColocarTorso = slotAtivo.podeColocarTorso,
            podeColocarCintura = slotAtivo.podeColocarCintura,
            podeColocarCalca = slotAtivo.podeColocarCalca,
            podeColocarBota = slotAtivo.podeColocarBota,
            ehMachado = slotAtivo.ehMachado,
            ehPicareta = slotAtivo.ehPicareta
        };
        GameObject jogador = GameObject.FindWithTag("Player");
        if (jogador != null && prefabItemArremessado != null) {
            Vector2 direcaoArremesso = Vector2.right;
            GameObject objInstanciado = Instantiate(prefabItemArremessado, jogador.transform.position, Quaternion.identity);
            ItemArremessado scriptArremesso = objInstanciado.GetComponent<ItemArremessado>();
            if (scriptArremesso != null) {
                scriptArremesso.Inicializar(
                    direcaoArremesso, slotAtivo.iconeItem, slotAtivo.nomeItem, slotAtivo.podeConsumir, 
                    slotAtivo.valorFome, slotAtivo.danoItem, slotAtivo.tamanhoHitbox, slotAtivo.quebraAoAtingir, 
                    slotAtivo.defesaItem, slotAtivo.podeColocarCabeca, slotAtivo.podeColocarTorso, 
                    slotAtivo.podeColocarCintura, slotAtivo.podeColocarCalca, slotAtivo.podeColocarBota,
                    slotAtivo.ehMachado, slotAtivo.ehPicareta
                );
            }
        }
        slotAtivo.quantidadeItem--;
        if (slotAtivo.quantidadeItem <= 0){
            LimparSlot(slotAtivo);
        }
        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if (ui != null) {
            ui.AtualizarUI();
        }
        return itemArremessado;
    }
    public void UsarItemSelecionado(){
        ItemSlot slotAtivo = ObterItemSelecionado();
        if (slotAtivo != null && slotAtivo.podeConsumir && slotAtivo.quantidadeItem > 0){
            GameObject jogador = GameObject.FindWithTag("Player");
            FomeJogador sistemaFome = null;
            if (jogador != null){
                sistemaFome = jogador.GetComponent<FomeJogador>();
            }
            if (sistemaFome != null){
                sistemaFome.Comer(slotAtivo.valorFome);
                slotAtivo.quantidadeItem--;
                if (slotAtivo.quantidadeItem <= 0){
                    LimparSlot(slotAtivo);
                }
                AbrirInventario ui = FindObjectOfType<AbrirInventario>();
                if (ui != null) {
                    ui.AtualizarUI();
                }
            }
        }
    }
    private void LimparSlot(ItemSlot slot) {
        slot.nomeItem = "";
        slot.iconeItem = null;
        slot.quantidadeItem = 0;
        slot.podeConsumir = false;
        slot.valorFome = 0f;
        slot.danoItem = 0f;
        slot.defesaItem = 0f;
        slot.tamanhoHitbox = new Vector2(1f, 1f);
        slot.quebraAoAtingir = false;
        slot.podeColocarCabeca = false;
        slot.podeColocarTorso = false;
        slot.podeColocarCintura = false;
        slot.podeColocarCalca = false;
        slot.podeColocarBota = false;
        slot.ehMachado = false;
        slot.ehPicareta = false;
    }

    public void SalvarInventario(){
        PlayerPrefs.SetInt("SlotsRapidos_Count", slotsRapidos.Count);
        for (int i = 0; i < slotsRapidos.Count; i++){
            PlayerPrefs.SetString("SlotRapido_" + i + "_Nome", slotsRapidos[i].nomeItem);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Qtd", slotsRapidos[i].quantidadeItem);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Consumir", slotsRapidos[i].podeConsumir ? 1 : 0);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_Fome", slotsRapidos[i].valorFome);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_Dano", slotsRapidos[i].danoItem);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_Defesa", slotsRapidos[i].defesaItem);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_HitboxX", slotsRapidos[i].tamanhoHitbox.x);
            PlayerPrefs.SetFloat("SlotRapido_" + i + "_HitboxY", slotsRapidos[i].tamanhoHitbox.y);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Quebra", slotsRapidos[i].quebraAoAtingir ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Cabeca", slotsRapidos[i].podeColocarCabeca ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Torso", slotsRapidos[i].podeColocarTorso ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Cintura", slotsRapidos[i].podeColocarCintura ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Calca", slotsRapidos[i].podeColocarCalca ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Bota", slotsRapidos[i].podeColocarBota ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Machado", slotsRapidos[i].ehMachado ? 1 : 0);
            PlayerPrefs.SetInt("SlotRapido_" + i + "_Picareta", slotsRapidos[i].ehPicareta ? 1 : 0);
        }
        PlayerPrefs.SetInt("Inventario_Count", inventario.Count);
        for (int i = 0; i < inventario.Count; i++){
            PlayerPrefs.SetString("SlotNormal_" + i + "_Nome", inventario[i].nomeItem);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Qtd", inventario[i].quantidadeItem);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Consumir", inventario[i].podeConsumir ? 1 : 0);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_Fome", inventario[i].valorFome);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_Dano", inventario[i].danoItem);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_Defesa", inventario[i].defesaItem);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_HitboxX", inventario[i].tamanhoHitbox.x);
            PlayerPrefs.SetFloat("SlotNormal_" + i + "_HitboxY", inventario[i].tamanhoHitbox.y);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Quebra", inventario[i].quebraAoAtingir ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Cabeca", inventario[i].podeColocarCabeca ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Torso", inventario[i].podeColocarTorso ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Cintura", inventario[i].podeColocarCintura ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Calca", inventario[i].podeColocarCalca ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Bota", inventario[i].podeColocarBota ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Machado", inventario[i].ehMachado ? 1 : 0);
            PlayerPrefs.SetInt("SlotNormal_" + i + "_Picareta", inventario[i].ehPicareta ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void CarregarInventario() {
        if (PlayerPrefs.HasKey("SlotsRapidos_Count")){
            int totalRapidos = PlayerPrefs.GetInt("SlotsRapidos_Count");
            slotsRapidos.Clear();
            for (int i = 0; i < totalRapidos; i++){
                string nome = PlayerPrefs.GetString("SlotRapido_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotRapido_" + i + "_Qtd", 0);
                bool consumir = PlayerPrefs.GetInt("SlotRapido_" + i + "_Consumir", 0) == 1;
                float fome = PlayerPrefs.GetFloat("SlotRapido_" + i + "_Fome", 0f);
                float dano = PlayerPrefs.GetFloat("SlotRapido_" + i + "_Dano", 0f);
                float defesa = PlayerPrefs.GetFloat("SlotRapido_" + i + "_Defesa", 0f);
                float hx = PlayerPrefs.GetFloat("SlotRapido_" + i + "_HitboxX", 1f);
                float hy = PlayerPrefs.GetFloat("SlotRapido_" + i + "_HitboxY", 1f);
                bool quebra = PlayerPrefs.GetInt("SlotRapido_" + i + "_Quebra", 0) == 1;
                bool cabeca = PlayerPrefs.GetInt("SlotRapido_" + i + "_Cabeca", 0) == 1;
                bool torso = PlayerPrefs.GetInt("SlotRapido_" + i + "_Torso", 0) == 1;
                bool cintura = PlayerPrefs.GetInt("SlotRapido_" + i + "_Cintura", 0) == 1;
                bool calca = PlayerPrefs.GetInt("SlotRapido_" + i + "_Calca", 0) == 1;
                bool bota = PlayerPrefs.GetInt("SlotRapido_" + i + "_Bota", 0) == 1;
                bool machado = PlayerPrefs.GetInt("SlotRapido_" + i + "_Machado", 0) == 1;
                bool picareta = PlayerPrefs.GetInt("SlotRapido_" + i + "_Picareta", 0) == 1;
                Sprite icone = CarregarIconePorNome(nome);
                slotsRapidos.Add(new ItemSlot(nome, qtd, icone, consumir, fome, dano, defesa, new Vector2(hx, hy), quebra, cabeca, torso, cintura, calca, bota, machado, picareta));
            }
        }
        if (PlayerPrefs.HasKey("Inventario_Count")){
            int totalNormal = PlayerPrefs.GetInt("Inventario_Count");
            inventario.Clear();
            for (int i = 0; i < totalNormal; i++){
                string nome = PlayerPrefs.GetString("SlotNormal_" + i + "_Nome", "");
                int qtd = PlayerPrefs.GetInt("SlotNormal_" + i + "_Qtd", 0);
                bool consumir = PlayerPrefs.GetInt("SlotNormal_" + i + "_Consumir", 0) == 1;
                float fome = PlayerPrefs.GetFloat("SlotNormal_" + i + "_Fome", 0f);
                float dano = PlayerPrefs.GetFloat("SlotNormal_" + i + "_Dano", 0f);
                float defesa = PlayerPrefs.GetFloat("SlotNormal_" + i + "_Defesa", 0f);
                float hx = PlayerPrefs.GetFloat("SlotNormal_" + i + "_HitboxX", 1f);
                float hy = PlayerPrefs.GetFloat("SlotNormal_" + i + "_HitboxY", 1f);
                bool quebra = PlayerPrefs.GetInt("SlotNormal_" + i + "_Quebra", 0) == 1;
                bool cabeca = PlayerPrefs.GetInt("SlotNormal_" + i + "_Cabeca", 0) == 1;
                bool torso = PlayerPrefs.GetInt("SlotNormal_" + i + "_Torso", 0) == 1;
                bool cintura = PlayerPrefs.GetInt("SlotNormal_" + i + "_Cintura", 0) == 1;
                bool calca = PlayerPrefs.GetInt("SlotNormal_" + i + "_Calca", 0) == 1;
                bool bota = PlayerPrefs.GetInt("SlotNormal_" + i + "_Bota", 0) == 1;
                bool machado = PlayerPrefs.GetInt("SlotNormal_" + i + "_Machado", 0) == 1;
                bool picareta = PlayerPrefs.GetInt("SlotNormal_" + i + "_Picareta", 0) == 1;
                Sprite icone = CarregarIconePorNome(nome);
                inventario.Add(new ItemSlot(nome, qtd, icone, consumir, fome, dano, defesa, new Vector2(hx, hy), quebra, cabeca, torso, cintura, calca, bota, machado, picareta));
            }
        }
        AbrirInventario ui = FindObjectOfType<AbrirInventario>();
        if (ui != null) {
            ui.AtualizarUI();
        }
    }
    private Sprite CarregarIconePorNome(string nomeItem){
        if (string.IsNullOrEmpty(nomeItem)){
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
    public void MoverItem(ArrasteSlot.TipoSlot tipoOrigem, int indexOrigem, ArrasteSlot.TipoSlot tipoDestino, int indexDestino) {
        if (tipoOrigem == tipoDestino && indexOrigem == indexDestino) {
            return;
        }
        ItemSlot slotOrigemObj = ObterSlotPorTipo(tipoOrigem, indexOrigem);
        ItemSlot slotDestinoObj = ObterSlotPorTipo(tipoDestino, indexDestino);
        if (slotOrigemObj == null || slotDestinoObj == null || string.IsNullOrEmpty(slotOrigemObj.nomeItem)) {
            return;
        }
        if (tipoDestino == ArrasteSlot.TipoSlot.Cabeca && !slotOrigemObj.podeColocarCabeca) {
            return;
        }
        if (tipoDestino == ArrasteSlot.TipoSlot.Torso && !slotOrigemObj.podeColocarTorso) {
            return;
        }
        if (tipoDestino == ArrasteSlot.TipoSlot.Cintura && !slotOrigemObj.podeColocarCintura) {
            return;
        }
        if (tipoDestino == ArrasteSlot.TipoSlot.Calca && !slotOrigemObj.podeColocarCalca) {
            return;
        }
        if (tipoDestino == ArrasteSlot.TipoSlot.Bota && !slotOrigemObj.podeColocarBota) {
            return;
        }
        bool destinoEhEquipamento = (tipoDestino == ArrasteSlot.TipoSlot.Cabeca || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Torso || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Cintura || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Calca || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Bota);
        if (destinoEhEquipamento && slotOrigemObj.quantidadeItem > 1 && string.IsNullOrEmpty(slotDestinoObj.nomeItem)) {
            slotDestinoObj.nomeItem = slotOrigemObj.nomeItem;
            slotDestinoObj.iconeItem = slotOrigemObj.iconeItem;
            slotDestinoObj.quantidadeItem = 1;
            slotDestinoObj.podeConsumir = slotOrigemObj.podeConsumir;
            slotDestinoObj.valorFome = slotOrigemObj.valorFome;
            slotDestinoObj.danoItem = slotOrigemObj.danoItem;
            slotDestinoObj.defesaItem = slotOrigemObj.defesaItem;
            slotDestinoObj.tamanhoHitbox = slotOrigemObj.tamanhoHitbox;
            slotDestinoObj.quebraAoAtingir = slotOrigemObj.quebraAoAtingir;
            slotDestinoObj.podeColocarCabeca = slotOrigemObj.podeColocarCabeca;
            slotDestinoObj.podeColocarTorso = slotOrigemObj.podeColocarTorso;
            slotDestinoObj.podeColocarCintura = slotOrigemObj.podeColocarCintura;
            slotDestinoObj.podeColocarCalca = slotOrigemObj.podeColocarCalca;
            slotDestinoObj.podeColocarBota = slotOrigemObj.podeColocarBota;
            slotDestinoObj.ehMachado = slotOrigemObj.ehMachado;
            slotDestinoObj.ehPicareta = slotOrigemObj.ehPicareta;
            slotOrigemObj.quantidadeItem--;
            return;
        }
        if (!string.IsNullOrEmpty(slotDestinoObj.nomeItem) && slotDestinoObj.nomeItem == slotOrigemObj.nomeItem) {
            slotDestinoObj.quantidadeItem += slotOrigemObj.quantidadeItem;
            LimparSlot(slotOrigemObj);
            return;
        }
        ItemSlot temp = new ItemSlot(
            slotDestinoObj.nomeItem, slotDestinoObj.quantidadeItem, slotDestinoObj.iconeItem,
            slotDestinoObj.podeConsumir, slotDestinoObj.valorFome, slotDestinoObj.danoItem,
            slotDestinoObj.defesaItem, slotDestinoObj.tamanhoHitbox, slotDestinoObj.quebraAoAtingir,
            slotDestinoObj.podeColocarCabeca, slotDestinoObj.podeColocarTorso, slotDestinoObj.podeColocarCintura,
            slotDestinoObj.podeColocarCalca, slotDestinoObj.podeColocarBota, slotDestinoObj.ehMachado, slotDestinoObj.ehPicareta
        );
        slotDestinoObj.nomeItem = slotOrigemObj.nomeItem;
        slotDestinoObj.quantidadeItem = slotOrigemObj.quantidadeItem;
        slotDestinoObj.iconeItem = slotOrigemObj.iconeItem;
        slotDestinoObj.podeConsumir = slotOrigemObj.podeConsumir;
        slotDestinoObj.valorFome = slotOrigemObj.valorFome;
        slotDestinoObj.danoItem = slotOrigemObj.danoItem;
        slotDestinoObj.defesaItem = slotOrigemObj.defesaItem;
        slotDestinoObj.tamanhoHitbox = slotOrigemObj.tamanhoHitbox;
        slotDestinoObj.quebraAoAtingir = slotOrigemObj.quebraAoAtingir;
        slotDestinoObj.podeColocarCabeca = slotOrigemObj.podeColocarCabeca;
        slotDestinoObj.podeColocarTorso = slotOrigemObj.podeColocarTorso;
        slotDestinoObj.podeColocarCintura = slotOrigemObj.podeColocarCintura;
        slotDestinoObj.podeColocarCalca = slotOrigemObj.podeColocarCalca;
        slotDestinoObj.podeColocarBota = slotOrigemObj.podeColocarBota;
        slotDestinoObj.ehMachado = slotOrigemObj.ehMachado;
        slotDestinoObj.ehPicareta = slotOrigemObj.ehPicareta;

        slotOrigemObj.nomeItem = temp.nomeItem;
        slotOrigemObj.quantidadeItem = temp.quantidadeItem;
        slotOrigemObj.iconeItem = temp.iconeItem;
        slotOrigemObj.podeConsumir = temp.podeConsumir;
        slotOrigemObj.valorFome = temp.valorFome;
        slotOrigemObj.danoItem = temp.danoItem;
        slotOrigemObj.defesaItem = temp.defesaItem;
        slotOrigemObj.tamanhoHitbox = temp.tamanhoHitbox;
        slotOrigemObj.quebraAoAtingir = temp.quebraAoAtingir;
        slotOrigemObj.podeColocarCabeca = temp.podeColocarCabeca;
        slotOrigemObj.podeColocarTorso = temp.podeColocarTorso;
        slotOrigemObj.podeColocarCintura = temp.podeColocarCintura;
        slotOrigemObj.podeColocarCalca = temp.podeColocarCalca;
        slotOrigemObj.podeColocarBota = temp.podeColocarBota;
        slotOrigemObj.ehMachado = temp.ehMachado;
        slotOrigemObj.ehPicareta = temp.ehPicareta;
    }
    public void MoverUmItem(ArrasteSlot.TipoSlot tipoOrigem, int indexOrigem, ArrasteSlot.TipoSlot tipoDestino, int indexDestino) {
        if (tipoOrigem == tipoDestino && indexOrigem == indexDestino) {
            return;
        }
        ItemSlot slotOrigemObj = ObterSlotPorTipo(tipoOrigem, indexOrigem);
        ItemSlot slotDestinoObj = ObterSlotPorTipo(tipoDestino, indexDestino);
        if (slotOrigemObj == null || slotDestinoObj == null || string.IsNullOrEmpty(slotOrigemObj.nomeItem)) {
            return;
        }
        bool destinoEhEquipamento = (tipoDestino == ArrasteSlot.TipoSlot.Cabeca || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Torso || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Cintura || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Calca || 
                                   tipoDestino == ArrasteSlot.TipoSlot.Bota);
        if (destinoEhEquipamento) {
            if (tipoDestino == ArrasteSlot.TipoSlot.Cabeca && !slotOrigemObj.podeColocarCabeca) {
                return;
            }
            if (tipoDestino == ArrasteSlot.TipoSlot.Torso && !slotOrigemObj.podeColocarTorso) {
                return;
            }
            if (tipoDestino == ArrasteSlot.TipoSlot.Cintura && !slotOrigemObj.podeColocarCintura) {
                return;
            }
            if (tipoDestino == ArrasteSlot.TipoSlot.Calca && !slotOrigemObj.podeColocarCalca) {
                return;
            }
            if (tipoDestino == ArrasteSlot.TipoSlot.Bota && !slotOrigemObj.podeColocarBota) {
                return;
            }
        }
        if (string.IsNullOrEmpty(slotDestinoObj.nomeItem)) {
            slotDestinoObj.nomeItem = slotOrigemObj.nomeItem;
            slotDestinoObj.iconeItem = slotOrigemObj.iconeItem;
            slotDestinoObj.quantidadeItem = 1;
            slotDestinoObj.podeConsumir = slotOrigemObj.podeConsumir;
            slotDestinoObj.valorFome = slotOrigemObj.valorFome;
            slotDestinoObj.danoItem = slotOrigemObj.danoItem;
            slotDestinoObj.defesaItem = slotOrigemObj.defesaItem;
            slotDestinoObj.tamanhoHitbox = slotOrigemObj.tamanhoHitbox;
            slotDestinoObj.quebraAoAtingir = slotOrigemObj.quebraAoAtingir;
            slotDestinoObj.podeColocarCabeca = slotOrigemObj.podeColocarCabeca;
            slotDestinoObj.podeColocarTorso = slotOrigemObj.podeColocarTorso;
            slotDestinoObj.podeColocarCintura = slotOrigemObj.podeColocarCintura;
            slotDestinoObj.podeColocarCalca = slotOrigemObj.podeColocarCalca;
            slotDestinoObj.podeColocarBota = slotOrigemObj.podeColocarBota;
            slotDestinoObj.ehMachado = slotOrigemObj.ehMachado;
            slotDestinoObj.ehPicareta = slotOrigemObj.ehPicareta;
            slotOrigemObj.quantidadeItem--;
            if (slotOrigemObj.quantidadeItem <= 0) {
                LimparSlot(slotOrigemObj);
            }
            return;
        }
        if (slotDestinoObj.nomeItem == slotOrigemObj.nomeItem && !destinoEhEquipamento) {
            slotDestinoObj.quantidadeItem++;
            slotOrigemObj.quantidadeItem--;
            if (slotOrigemObj.quantidadeItem <= 0) {
                LimparSlot(slotOrigemObj);
            }
            return;
        }
    }
    private ItemSlot ObterSlotPorTipo(ArrasteSlot.TipoSlot tipo, int indice){
        switch (tipo) {
            case ArrasteSlot.TipoSlot.Rapido: return slotsRapidos[indice];
            case ArrasteSlot.TipoSlot.Inventario: return inventario[indice];
            case ArrasteSlot.TipoSlot.Cabeca: return slotCabeca;
            case ArrasteSlot.TipoSlot.Torso: return slotTorso;
            case ArrasteSlot.TipoSlot.Cintura: return slotCintura;
            case ArrasteSlot.TipoSlot.Calca: return slotCalca;
            case ArrasteSlot.TipoSlot.Bota: return slotBota;
            default: return null;
        }
    }
    private List<ItemSlot> ObterListaPorTipo(ArrasteSlot.TipoSlot tipo){
        if (tipo == ArrasteSlot.TipoSlot.Rapido) return slotsRapidos;
        if (tipo == ArrasteSlot.TipoSlot.Inventario) return inventario;
        return null;
    }
}