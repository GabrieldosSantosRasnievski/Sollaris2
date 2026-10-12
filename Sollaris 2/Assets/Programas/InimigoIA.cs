using UnityEngine;
using System.Collections;

public class InimigoIA : MonoBehaviour {
    public float velocidadeMovimento = 2f;
    public float distanciaAtaque = 1.5f;
    public float tempoEntreAtaques = 1.5f;
    private float proximoAtaque;
    private bool playerNaAreaDeVisao = false;
    private GameObject playerAlvo;
    private bool aAtacar = false;
    private float anguloDoAtaque;
    public Transform pivotAtaque; 
    public GameObject objetoHitbox;
    public GameObject objetoExclamacao; 
    private bool aPiscarExclamacao = false;
    public float danoInimigo = 10f;

    private void Start(){
        if (objetoHitbox != null){
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;
        }
        if (objetoExclamacao != null){
            objetoExclamacao.SetActive(false); 
        }
    }
    private void Update() {
        if (playerNaAreaDeVisao && playerAlvo != null){
            float distanciaAtual = Vector2.Distance(transform.position, playerAlvo.transform.position);

            if (distanciaAtual > distanciaAtaque){
                transform.position = Vector2.MoveTowards(transform.position, playerAlvo.transform.position, velocidadeMovimento * Time.deltaTime);
            } else{
                if (!aAtacar){
                    Vector2 direcao = playerAlvo.transform.position - pivotAtaque.position;
                    anguloDoAtaque = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
                }
                pivotAtaque.rotation = Quaternion.Euler(new Vector3(0, 0, anguloDoAtaque));
                if (Time.time >= proximoAtaque && !aAtacar){
                    StartCoroutine(AtacarInstantaneo());
                    proximoAtaque = Time.time + tempoEntreAtaques;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            playerNaAreaDeVisao = true;
            playerAlvo = outro.gameObject;
            if (objetoExclamacao != null && !aPiscarExclamacao){
                StartCoroutine(PiscarExclamacao());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            playerNaAreaDeVisao = false;
            playerAlvo = null;
            if (objetoHitbox != null){
                BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
                if (col != null) col.enabled = false;
                objetoHitbox.SetActive(false);
            }
            if (objetoExclamacao != null){
                objetoExclamacao.SetActive(false);
            }
        }
    }
    private IEnumerator PiscarExclamacao(){
        aPiscarExclamacao = true;
        for (int i = 0; i < 3; i++){
            objetoExclamacao.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            objetoExclamacao.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
        aPiscarExclamacao = false;
    }
    private IEnumerator AtacarInstantaneo(){
        aAtacar = true;
        if (objetoHitbox != null){
            objetoHitbox.SetActive(true);
            Animator animHitbox = objetoHitbox.GetComponent<Animator>();
            if (animHitbox != null){
                animHitbox.SetTrigger("Corte");
            }
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null){
                col.enabled = true;
            }
            yield return new WaitForSeconds(0.05f);
            Collider2D[] alvosAtingidos = Physics2D.OverlapBoxAll(
                objetoHitbox.transform.position, 
                col != null ? col.size : new Vector2(1f, 1f), 
                objetoHitbox.transform.rotation.eulerAngles.z
            );
            foreach(Collider2D alvo in alvosAtingidos){
                if(alvo.gameObject == gameObject){
                    continue;
                }
                VidaJogador vidaPlayer = alvo.GetComponent<VidaJogador>();
                if(vidaPlayer != null){
                    vidaPlayer.TomarDano(danoInimigo);
                    Debug.Log("Inimigo acertou o player com Corte!");
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f); 
            if (col != null){
                col.enabled = false;
            }
            objetoHitbox.SetActive(false);
        }
        aAtacar = false;
    }
    public void PararAtaque(){
        if(aAtacar){
            StopAllCoroutines();
            if(objetoHitbox != null){
                BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
                if (col != null) col.enabled = false;
                objetoHitbox.SetActive(false);
            }
            aAtacar = false;
        }
    }
}