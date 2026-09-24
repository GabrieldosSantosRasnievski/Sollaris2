using UnityEngine;
using TMPro;

public class InteragirObjeto : MonoBehaviour{
    public GameObject textoColetar;
    public string nomeItem;
    public Sprite iconeItem;
    public bool podeConsumir;
    public float valorFome;
    public float danoItem;
    public Vector2 tamanhoHitbox = new Vector2(1f, 1f);
    public bool quebraAoAtingir;
    private bool taPerto = false;

    void Update(){
        if(taPerto && Input.GetKeyDown(KeyCode.E)){
            if(InventarioJogador.Instance != null){
                bool pegou = InventarioJogador.Instance.TentarAdicionar(nomeItem, iconeItem, podeConsumir, valorFome, danoItem, tamanhoHitbox, quebraAoAtingir);
                if(pegou){
                    AbrirInventario ui = FindObjectOfType<AbrirInventario>();
                    if(ui != null){
                        ui.AtualizarUI();
                    }

                    if(textoColetar != null){
                        textoColetar.SetActive(false);
                    }
                    taPerto = false;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D outro){
        if(outro.CompareTag("Player")){
            taPerto = true;
            if(textoColetar != null){
                textoColetar.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D outro){
        if(outro.CompareTag("Player")){
            taPerto = false;
            if(textoColetar != null){
                textoColetar.SetActive(false);
            }
        }
    }
}