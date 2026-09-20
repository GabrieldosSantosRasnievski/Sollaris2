using UnityEngine;

public class AtaqueJogador : MonoBehaviour{
    public Transform pontoAtaque;
    public LayerMask camadaInimigo;
    public float danoBaseSoco = 1f;
    public Vector2 tamanhoSocoPadrao = new Vector2(0.8f, 0.8f);
    public float distanciaAtaque = 1.5f;
    private Vector2 tamanhoHitboxAtual;

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log(">>> CLIQUE DO MOUSE (M1) DETETADO PELO UPDATE! <<<");
            TentarAtacar();
        }
    }

    void TentarAtacar(){
        if(InventarioJogador.Instance == null){
            Debug.Log("Erro: InventarioJogador.Instance está nulo.");
            return;
        }

        var slotAtivo = InventarioJogador.Instance.ObterItemSelecionado();

        if(slotAtivo != null && !string.IsNullOrEmpty(slotAtivo.nomeItem) && slotAtivo.podeConsumir){
            Debug.Log("Ataque ignorado: O item selecionado é consumível.");
            return;
        }

        AtualizarPosicaoPontoAtaque();
        ExecutarAtaque(slotAtivo);
    }

    void AtualizarPosicaoPontoAtaque(){
        if(pontoAtaque == null){
            Debug.Log("Erro: O Transform do PontoAtaque não foi atribuído no Inspetor!");
            return;
        }

        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;

        Vector2 direcao = (posicaoMouse - transform.position).normalized;
        float distanciaReal = Vector2.Distance(transform.position, posicaoMouse);
        
        if(distanciaReal > distanciaAtaque){
            distanciaReal = distanciaAtaque;
        }

        pontoAtaque.position = (Vector2)transform.position + (direcao * distanciaReal);
    }

    void ExecutarAtaque(InventarioJogador.ItemSlot slotAtivo){
        if(pontoAtaque == null){
            return;
        }

        float danoFinal = danoBaseSoco;
        tamanhoHitboxAtual = tamanhoSocoPadrao;

        if(slotAtivo != null && !string.IsNullOrEmpty(slotAtivo.nomeItem)){
            danoFinal = slotAtivo.danoItem;
            tamanhoHitboxAtual = slotAtivo.tamanhoHitbox;
        }

        Collider2D[] inimigosAtingidos = Physics2D.OverlapBoxAll(pontoAtaque.position, tamanhoHitboxAtual, 0f, camadaInimigo);
        Debug.Log("Quantidade de inimigos detetados na área: " + inimigosAtingidos.Length);

        foreach(Collider2D inimigo in inimigosAtingidos){
            VidaInimigo vida = inimigo.GetComponent<VidaInimigo>();
            if(vida != null){
                vida.TomarDano(danoFinal);
                Debug.Log("Dano aplicado com sucesso ao inimigo!");
            }
            else{
                Debug.Log("Aviso: O objeto detetado na Layer de inimigo não tem o script VidaInimigo!");
            }
        }
    }

    private void OnDrawGizmosSelected(){
        if(pontoAtaque != null){
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pontoAtaque.position, tamanhoHitboxAtual);
        }
    }
}