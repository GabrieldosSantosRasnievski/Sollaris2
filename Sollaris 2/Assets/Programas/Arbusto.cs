using UnityEngine;

public class Arbusto : MonoBehaviour {
    public Sprite spriteComFrutas;
    public Sprite spriteSemFrutas;
    private SpriteRenderer spriteRenderer;
    public GameObject prefabFruta; 
    private bool temFruta = true;
    private bool playerNaArea = false;
    public GameObject textoInteracao; 
    private static bool jogadorOcupado = false;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteComFrutas != null){
            spriteRenderer.sprite = spriteComFrutas;
        }

        if (textoInteracao != null){
            textoInteracao.SetActive(false);
        }
    }
    private void Update(){
        if (playerNaArea && temFruta){
            if (Input.GetKeyDown(KeyCode.E) && !jogadorOcupado){
                StartCoroutine(ColherFrutaCorotina());
            }
        }
    }
    private System.Collections.IEnumerator ColherFrutaCorotina(){
        jogadorOcupado = true;
        ColherFruta();
        yield return null;
        jogadorOcupado = false;
    }
    private void ColherFruta(){
        temFruta = false;
        
        if (spriteRenderer != null && spriteSemFrutas != null){
            spriteRenderer.sprite = spriteSemFrutas;
        }   
        if (textoInteracao != null){
            textoInteracao.SetActive(false);
        }
        if (prefabFruta != null){
            int quantidadeFrutas = Random.Range(1, 6);
            for (int i = 0; i < quantidadeFrutas; i++){
                float offsetX = Random.Range(-1.1f, 1.1f);
                float offsetY = Random.Range(-1.1f, 1.1f); 
                Vector3 posicaoSpawn = transform.position + new Vector3(offsetX, offsetY, 0f);
                Instantiate(prefabFruta, posicaoSpawn, Quaternion.identity);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D outro){
        if (outro.CompareTag("Player") && temFruta){
            playerNaArea = true;
            if (textoInteracao != null && !jogadorOcupado){
                textoInteracao.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            playerNaArea = false;
            if (textoInteracao != null){
                textoInteracao.SetActive(false);
            }
        }
    }
}