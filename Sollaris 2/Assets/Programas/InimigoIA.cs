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

    [Header("Configuração de Desvio de Obstáculos")]
    public LayerMask oclusoresMask; 
    public float distanciaDeteccao = 2.5f;

    private Rigidbody2D rb;
    private Vector2 direcaoDesvioAtual;
    private float tempoDesvioRestante = 0f;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.freezeRotation = true;
        }

        if (objetoHitbox != null){
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;
        }
        if (objetoExclamacao != null){
            objetoExclamacao.SetActive(false); 
        }
    }

    private void FixedUpdate() {
        if (playerNaAreaDeVisao && playerAlvo != null){
            float distanciaAtual = Vector2.Distance(transform.position, playerAlvo.transform.position);

            if (distanciaAtual > distanciaAtaque){
                Vector2 direcaoParaPlayer = ((Vector2)playerAlvo.transform.position - (Vector2)transform.position).normalized;
                Vector2 direcaoFinal = direcaoParaPlayer;

                // Se ainda estivermos no tempo de compromisso com o desvio, mantém a curva para evitar o loop
                if (tempoDesvioRestante > 0f) {
                    tempoDesvioRestante -= Time.fixedDeltaTime;
                    direcaoFinal = direcaoDesvioAtual;
                } else {
                    // Lança raios em leque (frente, levemente à esquerda e levemente à direita) para detecção antecipada
                    Vector2 direcaoEsquerda = new Vector2(-direcaoParaPlayer.y, direcaoParaPlayer.x);
                    Vector2 direcaoDireita = new Vector2(direcaoParaPlayer.y, -direcaoParaPlayer.x);

                    RaycastHit2D hitFrente = Physics2D.Raycast(transform.position, direcaoParaPlayer, distanciaDeteccao, oclusoresMask);
                    RaycastHit2D hitFrenteEsquerda = Physics2D.Raycast(transform.position, (direcaoParaPlayer + direcaoEsquerda * 0.5f).normalized, distanciaDeteccao * 0.8f, oclusoresMask);
                    RaycastHit2D hitFrenteDireita = Physics2D.Raycast(transform.position, (direcaoParaPlayer + direcaoDireita * 0.5f).normalized, distanciaDeteccao * 0.8f, oclusoresMask);
                    
                    if (hitFrente.collider != null || hitFrenteEsquerda.collider != null || hitFrenteDireita.collider != null) {
                        // Verifica qual lado está mais livre para contornar
                        RaycastHit2D hitLateralEsquerda = Physics2D.Raycast(transform.position, direcaoEsquerda, 1.5f, oclusoresMask);
                        RaycastHit2D hitLateralDireita = Physics2D.Raycast(transform.position, direcaoDireita, 1.5f, oclusoresMask);

                        if (hitLateralEsquerda.collider == null) {
                            direcaoDesvioAtual = direcaoEsquerda;
                        } else if (hitLateralDireita.collider == null) {
                            direcaoDesvioAtual = direcaoDireita;
                        } else {
                            direcaoDesvioAtual = direcaoEsquerda; // Padrão de segurança se ambos tiverem colisão
                        }

                        // Força o inimigo a manter o desvio por 0.4 segundos para contornar o tronco de forma fluida
                        tempoDesvioRestante = 0.4f;
                        direcaoFinal = direcaoDesvioAtual;
                    } else {
                        direcaoDesvioAtual = Vector2.zero;
                        direcaoFinal = direcaoParaPlayer;
                    }
                }

                // Move o Rigidbody com a direção calculada
                if (rb != null) {
                    Vector2 novaPosicao = rb.position + direcaoFinal * velocidadeMovimento * Time.fixedDeltaTime;
                    rb.MovePosition(novaPosicao);
                }
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