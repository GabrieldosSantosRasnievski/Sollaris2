using UnityEngine;
using System.Collections;

public class TesteMovimento : MonoBehaviour{
    public float velocidade = 5f;
    public float velocidadeDefendendo = 2f;
    public float velocidadeDash = 15f;
    public float duracaoDash = 0.5f;
    public float recargaDash = 1f;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteHomem;
    public Sprite spriteMulher;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    private Vector2 ultimaDirecaoDash = Vector2.right;
    private bool realizandoDash = false;
    private bool consegueDash = true;
    [HideInInspector] public bool estaEmDash = false;
    public ParticleSystem particulaDash;
    public float emissaomaxima = 60f;
    public bool estaTomandoDano = false;
    public Animator animacaoTeste;
    public GameObject prefabItemArremessado;
    public GameObject objetoEscudoVisual;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        if(particulaDash != null){
            particulaDash.Stop();
        }
        playerCollider = GetComponent<Collider2D>();
        if(objetoEscudoVisual != null){
            objetoEscudoVisual.SetActive(false);
        }
    }
    public void AtualizarGenero(){
        string generoEscolhido = PlayerPrefs.GetString("GeneroPlayer", "Homem");
        if(generoEscolhido == "Homem"){
            if(estaTomandoDano){
                return;
            }
            if(animacaoTeste == null){
                return;
            }
            if(Input.GetKey(KeyCode.S)){
                animacaoTeste.Play("Bora");
            }
            else if(!Input.anyKey){
                animacaoTeste.Play("HomemParado");
            }
        }
        else if(generoEscolhido == "Mulher"){
            spriteRenderer.sprite = spriteMulher;
        }
    }
    void Update(){
        bool defendendo = Input.GetKey(KeyCode.F);
        if(Input.GetKeyDown(KeyCode.F)){
            if(objetoEscudoVisual != null){
                objetoEscudoVisual.SetActive(true);
            }
            VidaJogador vida = GetComponent<VidaJogador>();
            if(vida != null){
                vida.AdicionarDefesaTemporaria(5f);
            }
        }
        if(Input.GetKeyUp(KeyCode.F)){
            if(objetoEscudoVisual != null){
                objetoEscudoVisual.SetActive(false);
            }
            VidaJogador vida = GetComponent<VidaJogador>();
            if(vida != null){
                vida.AdicionarDefesaTemporaria(-5f);
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && consegueDash && !defendendo){
            StartCoroutine(DarDash());
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            TentarArremessar();
        }
        AtualizarGenero();
    }
    void FixedUpdate(){
        Vector2 direcaoInput = Vector2.zero;
        if(Input.GetKey(KeyCode.W)){
            direcaoInput.y = direcaoInput.y + 1f;
        }
        if(Input.GetKey(KeyCode.S)){
            direcaoInput.y = direcaoInput.y - 1f;
        }
        if(Input.GetKey(KeyCode.D)){
            direcaoInput.x = direcaoInput.x + 1f;
        }
        if(Input.GetKey(KeyCode.A)){ 
            direcaoInput.x = direcaoInput.x - 1f;
        }

        if(direcaoInput != Vector2.zero){
            ultimaDirecaoDash = direcaoInput.normalized;
        }
        bool defendendo = Input.GetKey(KeyCode.F);
        float velocidadeAtualMovimento = defendendo ? velocidadeDefendendo : velocidade;

        if(realizandoDash){
            rb.MovePosition(rb.position + ultimaDirecaoDash * velocidadeDash * Time.fixedDeltaTime);
        }
        else{
            rb.MovePosition(rb.position + direcaoInput.normalized * velocidadeAtualMovimento * Time.fixedDeltaTime);
        }
    }
    private IEnumerator DarDash(){
        consegueDash = false;
        realizandoDash = true;
        estaEmDash = true;
        InimigoIA[] inimigosNaCena = FindObjectsOfType<InimigoIA>();
        foreach(var inimigo in inimigosNaCena){
            if(Vector2.Distance(transform.position, inimigo.transform.position) < 5f){
                inimigo.PararAtaque();
                Collider2D colisorInimigo = inimigo.GetComponent<Collider2D>();
                if(colisorInimigo != null && playerCollider != null){
                    Physics2D.IgnoreCollision(playerCollider, colisorInimigo, true);
                }
            }
        }
        if(particulaDash != null){
            particulaDash.Play();
        }
        float tempoColapso = 0f;
        ParticleSystem.EmissionModule emission = particulaDash.emission;
        while(tempoColapso < duracaoDash){
            tempoColapso = tempoColapso + Time.deltaTime;
            float progresso = 1f - (tempoColapso / duracaoDash);
            emission.rateOverTime = emissaomaxima * progresso;
            yield return null;
        }
        if(particulaDash != null){
            particulaDash.Stop();
        }
        foreach(var inimigo in inimigosNaCena){
            if(inimigo != null){
                Collider2D colisorInimigo = inimigo.GetComponent<Collider2D>();
                if(colisorInimigo != null && playerCollider != null){
                    Physics2D.IgnoreCollision(playerCollider, colisorInimigo, false);
                }
            }
        }
        realizandoDash = false;
        estaEmDash = false;
        yield return new WaitForSeconds(recargaDash);
        consegueDash = true;
    }
    private void TentarArremessar(){
        if(InventarioJogador.Instance == null){
            return;
        }
        var slotAtivo = InventarioJogador.Instance.ObterItemSelecionado();
        if(slotAtivo == null || string.IsNullOrEmpty(slotAtivo.nomeItem) || slotAtivo.quantidadeItem <= 0){
            return;
        }
        bool podeConsumir = slotAtivo.podeConsumir;
        float valorFome = slotAtivo.valorFome;
        float danoItem = slotAtivo.danoItem;
        Vector2 tamanhoHitbox = slotAtivo.tamanhoHitbox;
        var itemRemovido = InventarioJogador.Instance.ArremessarItemSelecionado();
        if(itemRemovido == null){
            return;
        }
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;
        Vector2 direcaoArremesso = (posicaoMouse - transform.position).normalized;
        if(prefabItemArremessado != null){
            GameObject itemObjeto = Instantiate(prefabItemArremessado, transform.position, Quaternion.identity);
            ItemArremessado scriptArremesso = itemObjeto.GetComponent<ItemArremessado>();
            if(scriptArremesso != null){
                scriptArremesso.Inicializar(direcaoArremesso, itemRemovido.iconeItem, itemRemovido.nomeItem, podeConsumir, valorFome, danoItem, tamanhoHitbox, itemRemovido.quebraAoAtingir);
            }
        }
    }
}