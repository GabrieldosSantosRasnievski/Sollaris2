using UnityEngine;

public class InteragirObjeto : MonoBehaviour{
    public GameObject textoColetar;
    public string nomeItem;
    public Sprite iconeItem;
    public bool podeConsumir;
    public float valorFome;
    public float danoItem;
    public float defesaItem;
    public Vector2 tamanhoHitbox = new Vector2(1f, 1f);
    public bool quebraAoAtingir;
    public bool podeColocarCabeca;
    public bool podeColocarTorso;
    public bool podeColocarCintura;
    public bool podeColocarCalca;
    public bool podeColocarBota;
    public bool ehMachado;
    public bool ehPicareta;
    private Transform playerTransform;
    public float distanciaInteracao = 1.5f;

    void Start(){
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null){
            playerTransform = player.transform;
        }
        if(textoColetar == null){
            GameObject txt = GameObject.FindWithTag("TextoColetar");
            if(txt != null) {
                textoColetar = txt;
            }
        }
        if(iconeItem == null && !string.IsNullOrEmpty(nomeItem)){
            iconeItem = Resources.Load<Sprite>("Icones/" + nomeItem);
        }
    }
    void Update(){
        if(playerTransform == null) return;
        float distancia = Vector2.Distance(transform.position, playerTransform.position);
        if(distancia <= distanciaInteracao){
            if(!Arbusto.jogadorOcupado){
                if(textoColetar != null && !textoColetar.activeSelf){
                    textoColetar.SetActive(true);
                }
                if(Input.GetKeyDown(KeyCode.E)){
                    if(InventarioJogador.Instance != null){
                        if(iconeItem == null && !string.IsNullOrEmpty(nomeItem)){
                            iconeItem = Resources.Load<Sprite>("Icones/" + nomeItem);
                        }
                        bool pegou = InventarioJogador.Instance.TentarAdicionar(
                            nomeItem, iconeItem, podeConsumir, valorFome, danoItem, 
                            tamanhoHitbox, quebraAoAtingir, defesaItem, 
                            podeColocarCabeca, podeColocarTorso, podeColocarCintura, podeColocarCalca, podeColocarBota
                        );
                        if(pegou){
                            AbrirInventario ui = FindObjectOfType<AbrirInventario>();
                            if(ui != null){
                                ui.AtualizarUI();
                            }
                            if(textoColetar != null){
                                textoColetar.SetActive(false);
                            }
                            Destroy(gameObject);
                        }
                        else{
                            Debug.LogWarning("O inventário recusou o item!");
                        }
                    }
                }
            }
            else{
                if(textoColetar != null && textoColetar.activeSelf){
                    textoColetar.SetActive(false);
                }
            }
        }
        else{
            if(textoColetar != null && textoColetar.activeSelf){
                textoColetar.SetActive(false);
            }
        }
    }
}