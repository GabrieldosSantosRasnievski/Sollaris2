using UnityEngine;

public class ItemArremessado : MonoBehaviour{
    public float velocidade = 10f;
    public float tempoDeVoo = 0.5f;
    public float giroObjeto = 720f;
    private Vector2 direcao;
    private bool voando = true;
    private Rigidbody2D rb;
    private Collider2D colisor2d;
    private string nomeDoItem;
    private Sprite iconeDoItem;
    private bool podeConsumirItem;
    private float valorFomeItem;
    private float danoItemArremessado;
    private float defesaItemArremessado;
    private Vector2 tamanhoHitboxItem;
    private bool quebraItemAoAtingir;
    private bool cabecaItem;
    private bool torsoItem;
    private bool cinturaItem;
    private bool calcaItem;
    private bool botaItem;
    private bool ehMachadoItem;
    private bool ehPicaretaItem;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        colisor2d = GetComponent<Collider2D>();
    }
    public void Inicializar(Vector2 direcaoArremesso, Sprite iconeItem, string nome, bool podeConsumir, float valorFome, float danoItem, Vector2 tamanhoHitbox, bool quebraAoAtingir, float defesaItem = 0f, bool cabeca = false, bool torso = false, bool cintura = false, bool calca = false, bool bota = false, bool ehMachado = false, bool ehPicareta = false){
        direcao = direcaoArremesso.normalized;
        iconeDoItem = iconeItem;
        nomeDoItem = nome;
        podeConsumirItem = podeConsumir;
        valorFomeItem = valorFome;
        danoItemArremessado = danoItem;
        defesaItemArremessado = defesaItem;
        tamanhoHitboxItem = tamanhoHitbox;
        quebraItemAoAtingir = quebraAoAtingir;
        cabecaItem = cabeca;
        torsoItem = torso;
        cinturaItem = cintura;
        calcaItem = calca;
        botaItem = bota;
        ehMachadoItem = ehMachado;
        ehPicaretaItem = ehPicareta;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.sprite = iconeItem;
        }
        InteragirObjeto interacao = GetComponent<InteragirObjeto>();
        if(interacao != null){
            interacao.enabled = false;
        }
        if(rb != null){
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.linearVelocity = direcao * velocidade;
        }
        if(colisor2d != null){
            colisor2d.isTrigger = false;
        }
        Invoke(nameof(PararNoChao), tempoDeVoo);
    }
    void Update(){
        if(voando){
            transform.Rotate(0, 0, giroObjeto * Time.deltaTime);
        }
    }
    void FixedUpdate(){
        if(voando && rb != null){
            rb.linearVelocity = direcao * velocidade;
        }
    }
    void PararNoChao(){
        if(!voando){
            return;
        }
        voando = false;
        transform.rotation = Quaternion.identity;
        if(rb != null){
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if(colisor2d != null){
            colisor2d.isTrigger = true;
            BoxCollider2D boxCol = colisor2d as BoxCollider2D;
            if(boxCol != null){
                boxCol.size = new Vector2(0.2f, 0.2f);
            }
        }
        gameObject.tag = "Objeto";
        InteragirObjeto interacao = GetComponent<InteragirObjeto>();
        if(interacao == null){
            interacao = gameObject.AddComponent<InteragirObjeto>();
        }
        interacao.nomeItem = nomeDoItem;
        interacao.iconeItem = iconeDoItem;
        interacao.podeConsumir = podeConsumirItem;
        interacao.valorFome = valorFomeItem;
        interacao.danoItem = danoItemArremessado;
        interacao.defesaItem = defesaItemArremessado;
        interacao.tamanhoHitbox = tamanhoHitboxItem;
        interacao.quebraAoAtingir = quebraItemAoAtingir;
        interacao.podeColocarCabeca = cabecaItem;
        interacao.podeColocarTorso = torsoItem;
        interacao.podeColocarCintura = cinturaItem;
        interacao.podeColocarCalca = calcaItem;
        interacao.podeColocarBota = botaItem;
        interacao.ehMachado = ehMachadoItem;
        interacao.ehPicareta = ehPicaretaItem;

        interacao.enabled = true;
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(voando){
            if(collision.gameObject.CompareTag("Player")){
                return;
            }
            VidaInimigo vida = collision.gameObject.GetComponent<VidaInimigo>();
            if(vida != null){
                vida.TomarDano(danoItemArremessado);

                if(quebraItemAoAtingir){
                    Destroy(gameObject);
                }
                else{
                    CancelInvoke(nameof(PararNoChao));
                    PararNoChao();
                }
                return;
            }
            CancelInvoke(nameof(PararNoChao));
            PararNoChao();
        }
    }
}