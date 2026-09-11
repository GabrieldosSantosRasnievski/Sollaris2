using UnityEngine;
using TMPro;

public class InteragirObjeto : MonoBehaviour
{
    public GameObject textoColetar;
    public string nomeItem;
    public Sprite iconeItem;
    private bool taPerto = false;
    void Update(){
        if(taPerto && Input.GetKeyDown(KeyCode.E)){
            if(InventarioJogador.Instance != null){
                bool pegou = InventarioJogador.Instance.TentarAdicionar(nomeItem, iconeItem);
                if(pegou){
                    if(textoColetar != null)
                    {
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
            if(textoColetar != null)
            {
                textoColetar.SetActive(true);   
            }
        }
    }
    private void OnTriggerExit2D(Collider2D outro){
        if(outro.CompareTag("Player")){
            taPerto = false;
            if(textoColetar != null)
            {
                textoColetar.SetActive(false);   
             }
        }
    }
}
