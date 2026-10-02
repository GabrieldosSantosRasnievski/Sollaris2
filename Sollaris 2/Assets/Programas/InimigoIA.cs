using UnityEngine;

public class InimigoIA : MonoBehaviour {
    public float tempoEntreAtaques = 1.5f;
    private float proximoAtaque;
    private bool playerNaArea = false;
    private GameObject playerAlvo;
    private bool aAtacar = false;
    private float anguloDoAtaque;
    public Transform pivotAtaque; 
    public GameObject objetoHitbox; 
    private Vector3 posicaoLocalOriginalHitbox;

    private void Start() {
        if (objetoHitbox != null) {
            objetoHitbox.SetActive(false);
            posicaoLocalOriginalHitbox = objetoHitbox.transform.localPosition;
        }
    }
    private void Update() {
        if (playerNaArea && playerAlvo != null){
            if (!aAtacar) {
                Vector2 direcao = playerAlvo.transform.position - pivotAtaque.position;
                anguloDoAtaque = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            }
            pivotAtaque.rotation = Quaternion.Euler(new Vector3(0, 0, anguloDoAtaque));
            if (Time.time >= proximoAtaque && !aAtacar){
                StartCoroutine(AtacarComAvanco());
                proximoAtaque = Time.time + tempoEntreAtaques;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            playerNaArea = true;
            playerAlvo = outro.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            playerNaArea = false;
            playerAlvo = null;
            if (objetoHitbox != null){
                objetoHitbox.SetActive(false);
            }
        }
    }
    private System.Collections.IEnumerator AtacarComAvanco(){
        aAtacar = true;
        if (objetoHitbox != null){
            objetoHitbox.SetActive(true);
            Transform tHitbox = objetoHitbox.transform;
            float duracao = 0.2f;
            float tempoPassado = 0f;
            float distanciaAvanco = 1.2f; 
            Vector3 posInicial = posicaoLocalOriginalHitbox;
            Vector3 posFinal = posInicial + new Vector3(distanciaAvanco, 0, 0);
            while (tempoPassado < duracao){
                tempoPassado += Time.deltaTime;
                float progresso = tempoPassado / duracao;
                tHitbox.localPosition = Vector3.Lerp(posInicial, posFinal, progresso);
                yield return null;
            }
            yield return new WaitForSeconds(0.05f);
            objetoHitbox.SetActive(false);
            tHitbox.localPosition = posInicial;
        }

        aAtacar = false;
    }
}