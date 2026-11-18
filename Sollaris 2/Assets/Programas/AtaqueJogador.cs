using UnityEngine;
using System.Collections;

public class AtaqueJogador : MonoBehaviour {
    public float distanciaAtaque = 1.5f;
    public float tempoEntreAtaques = 0.5f;
    private float proximoAtaque = 0f;
    private bool aAtacar = false;
    public Transform pivotAtaque; 
    public GameObject objetoHitbox;

    [Header("Configuração de Alcance do Ataque")]
    public float offsetHitbox = 0.8f; // Distância exata em que a hitbox surge à frente do jogador

    private void Start(){
        if (objetoHitbox != null){
            // Garante que a hitbox começa estritamente desligada ao iniciar o jogo
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;
            objetoHitbox.SetActive(false);
        }
    }

    private void Update() {
        // Se o inventário estiver aberto ou construindo, bloqueia totalmente o ataque e a rotação da arma
        if (AbrirInventario.InventarioAberto || ColocarObjeto.ConstrucaoAtiva) {
            return;
        }

        if (pivotAtaque != null && !aAtacar){
            Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicaoMouse.z = 0f;
            Vector2 direcao = posicaoMouse - pivotAtaque.position;
            float anguloDoAtaque = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            pivotAtaque.rotation = Quaternion.Euler(new Vector3(0, 0, anguloDoAtaque));
        }

        if (Input.GetMouseButtonDown(0)){
            // Proteção dupla para garantir o bloqueio caso clique com UI ou Construção ativas
            if (AbrirInventario.InventarioAberto || ColocarObjeto.ConstrucaoAtiva) {
                return;
            }

            if (Time.time >= proximoAtaque && !aAtacar){
                TentarAtacar();
                proximoAtaque = Time.time + tempoEntreAtaques;
            }
            else{
                Debug.Log("Ataque em cooldown! Espere um pouco.");
            }
        }
    }

    private void TentarAtacar(){
        if (InventarioJogador.Instance == null){
            Debug.Log("Erro: InventarioJogador.Instance está nulo.");
            return;
        }
        var slotAtivo = InventarioJogador.Instance.ObterItemSelecionado();
        if (slotAtivo != null && !string.IsNullOrEmpty(slotAtivo.nomeItem) && slotAtivo.podeConsumir){
            Debug.Log("Ataque ignorado: O item selecionado é consumível.");
            return;
        }

        StartCoroutine(AtacarComHitbox(slotAtivo));
    }

    private IEnumerator AtacarComHitbox(InventarioJogador.ItemSlot slotAtivo){
        aAtacar = true;
        float danoFinal = 1f;
        bool ehMachado = false;
        bool ehCortante = false;
        if (slotAtivo != null && !string.IsNullOrEmpty(slotAtivo.nomeItem)){
            danoFinal = slotAtivo.danoItem;
            ehMachado = slotAtivo.ehMachado;
            ehCortante = slotAtivo.ehCortante;
        }

        if (objetoHitbox != null && pivotAtaque != null){
            // --- POSICIONAMENTO DINÂMICO DA HITBOX POR CÓDIGO ---
            // Em vez de usar a distância fixa da hierarquia, calculamos o offset exato na direção do pivot/mouse
            Vector2 direcaoAtaque = pivotAtaque.right; 
            objetoHitbox.transform.position = (Vector2)transform.position + (direcaoAtaque * offsetHitbox);
            // ----------------------------------------------------

            objetoHitbox.SetActive(true);

            Animator animHitbox = objetoHitbox.GetComponent<Animator>();
            if (animHitbox != null){
                if (ehCortante){
                    animHitbox.SetTrigger("Corte");
                }
                else{
                    animHitbox.SetTrigger("Impacto");
                }
            }
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = true;
            yield return new WaitForSeconds(0.05f);

            Collider2D[] alvosAtingidos = Physics2D.OverlapBoxAll(
                objetoHitbox.transform.position, 
                col != null ? col.size : new Vector2(1f, 1f), 
                objetoHitbox.transform.rotation.eulerAngles.z
            );

            foreach(Collider2D alvo in alvosAtingidos){
                if(alvo.gameObject == gameObject) continue;

                // --- TRAVA DE DIREÇÃO: Impede de acertar inimigos nas costas ---
                Vector2 direcaoAteAlvo = ((Vector2)alvo.transform.position - (Vector2)transform.position).normalized;
                float produtoEscalar = Vector2.Dot(direcaoAtaque, direcaoAteAlvo);
                if (produtoEscalar < 0.2f) { 
                    continue; // Ignora se o alvo estiver nas costas ou fora do arco frontal
                }
                // ----------------------------------------------------------------

                VidaInimigo vida = alvo.GetComponent<VidaInimigo>();
                if(vida != null){
                    vida.TomarDano(danoFinal);
                    Debug.Log("Dano aplicado com sucesso ao inimigo!");
                }

                Arvore arvore = alvo.GetComponent<Arvore>();
                if(arvore != null){
                    arvore.ReceberDano(danoFinal, ehMachado);
                    Debug.Log("Dano aplicado com sucesso à árvore!");
                }
            }
            yield return new WaitForSeconds(0.3f);

            if (col != null) col.enabled = false;
            objetoHitbox.SetActive(false);
        }
        aAtacar = false;
    }
}