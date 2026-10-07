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

    private void Start(){
        if (objetoHitbox != null){
            objetoHitbox.SetActive(false);
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
            Animator animHitbox = objetoHitbox.GetComponent<Animator>();
            if (animHitbox != null){
                animHitbox.SetTrigger("Atacar");
            }
            objetoHitbox.SetActive(true);
            yield return new WaitForSeconds(0.2f); 
            objetoHitbox.SetActive(false);
        }
        aAtacar = false;
    }
    public void PararAtaque(){
        if(aAtacar){
            StopAllCoroutines();
            if(objetoHitbox != null){
                objetoHitbox.SetActive(false);
            }
            aAtacar = false;
        }
    }
}