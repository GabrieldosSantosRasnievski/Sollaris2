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
    public float distanciaDeteccao = 3.0f;

    [Header("Configuração de Patrulha por Distância")]
    public float distanciaAndar = 3.0f;
    public float tempoPausa = 2.0f;
    private Vector2 posicaoInicialPatrulha;
    private Vector2 direcaoPatrulhaAtual;
    private float distanciaPercorrida = 0f;
    private bool estaPausadoPatrulhando = false;
    private float temporizadorPausa = 0f;

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

        posicaoInicialPatrulha = transform.position;
        DefinirNovaDirecaoPatrulha();
    }

    private void FixedUpdate() {
        Vector2 direcaoFinal = Vector2.zero;

        if (playerNaAreaDeVisao && playerAlvo != null){
            float distanciaAtual = Vector2.Distance(transform.position, playerAlvo.transform.position);

            if (distanciaAtual > distanciaAtaque){
                Vector2 direcaoParaPlayer = ((Vector2)playerAlvo.transform.position - (Vector2)transform.position).normalized;
                direcaoFinal = direcaoParaPlayer;

                if (tempoDesvioRestante > 0f) {
                    tempoDesvioRestante -= Time.fixedDeltaTime;
                    direcaoFinal = direcaoDesvioAtual;
                } else {
                    RaycastHit2D hitFrente = Physics2D.Raycast(transform.position, direcaoParaPlayer, distanciaDeteccao, oclusoresMask);
                    
                    if (hitFrente.collider != null) {
                        Vector2 direcaoEsquerda = new Vector2(-direcaoParaPlayer.y, direcaoParaPlayer.x);
                        Vector2 direcaoDireita = new Vector2(direcaoParaPlayer.y, -direcaoParaPlayer.x);

                        RaycastHit2D hitEsquerda = Physics2D.Raycast(transform.position, direcaoEsquerda, 2.0f, oclusoresMask);
                        RaycastHit2D hitDireita = Physics2D.Raycast(transform.position, direcaoDireita, 2.0f, oclusoresMask);

                        if (hitEsquerda.collider == null) {
                            direcaoDesvioAtual = direcaoEsquerda;
                        } else if (hitDireita.collider == null) {
                            direcaoDesvioAtual = direcaoDireita;
                        } else {
                            direcaoDesvioAtual = direcaoEsquerda;
                        }

                        tempoDesvioRestante = 0.8f;
                        direcaoFinal = direcaoDesvioAtual;
                    } else {
                        tempoDesvioRestante = 0f;
                        direcaoDesvioAtual = Vector2.zero;
                        direcaoFinal = direcaoParaPlayer;
                    }
                }

                if (rb != null) {
                    Vector2 novaPosicao = rb.position + direcaoFinal * velocidadeMovimento * Time.fixedDeltaTime;
                    rb.MovePosition(novaPosicao);
                }
            } else {
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
        else {
            if (estaPausadoPatrulhando) {
                temporizadorPausa -= Time.fixedDeltaTime;
                if (temporizadorPausa <= 0f) {
                    estaPausadoPatrulhando = false;
                    posicaoInicialPatrulha = transform.position;
                    distanciaPercorrida = 0f;
                    DefinirNovaDirecaoPatrulha();
                }
            } else {
                if (rb != null) {
                    Vector2 passoMovimento = direcaoPatrulhaAtual * (velocidadeMovimento * 0.4f) * Time.fixedDeltaTime;
                    Vector2 novaPosicao = rb.position + passoMovimento;
                    
                    distanciaPercorrida += passoMovimento.magnitude;

                    RaycastHit2D hitPatrulha = Physics2D.Raycast(transform.position, direcaoPatrulhaAtual, 1.0f, oclusoresMask);
                    if (hitPatrulha.collider != null || distanciaPercorrida >= distanciaAndar) {
                        estaPausadoPatrulhando = true;
                        temporizadorPausa = tempoPausa;
                    } else {
                        rb.MovePosition(novaPosicao);
                    }
                }
            }
        }
    }

    private void DefinirNovaDirecaoPatrulha() {
        float anguloAleatorio = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direcaoPatrulhaAtual = new Vector2(Mathf.Cos(anguloAleatorio), Mathf.Sin(anguloAleatorio)).normalized;
    }

    private void ProcessarVisaoPlayer(GameObject outroObjeto) {
        Vector2 direcaoAtePlayer = (Vector2)outroObjeto.transform.position - (Vector2)transform.position;
        float distanciaReal = direcaoAtePlayer.magnitude;

        // Lança um raio para ver se há obstáculo entre o inimigo e o player
        RaycastHit2D hitVisao = Physics2D.Raycast(transform.position, direcaoAtePlayer.normalized, distanciaReal, oclusoresMask);

        if (hitVisao.collider == null) {
            // Caminho totalmente livre, inimigo vê o player
            if (!playerNaAreaDeVisao) {
                playerNaAreaDeVisao = true;
                playerAlvo = outroObjeto;
                if (objetoExclamacao != null && !aPiscarExclamacao){
                    StartCoroutine(PiscarExclamacao());
                }
            }
        } else {
            // O player está na zona, mas escondido atrás da árvore
            if (playerNaAreaDeVisao) {
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
    }

    private void OnTriggerStay2D(Collider2D outro){
        if (outro.CompareTag("Player")){
            ProcessarVisaoPlayer(outro.gameObject);
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
            posicaoInicialPatrulha = transform.position;
            distanciaPercorrida = 0f;
            DefinirNovaDirecaoPatrulha();
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