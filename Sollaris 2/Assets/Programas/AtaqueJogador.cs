using UnityEngine;
using System.Collections;

public class AtaqueJogador : MonoBehaviour {
    public float distanciaAtaque = 1.5f;
    public float tempoEntreAtaques = 0.5f;
    private float proximoAtaque = 0f;
    private bool aAtacar = false;
    public Transform pivotAtaque; 
    public GameObject objetoHitbox;

    private void Start(){
        if (objetoHitbox != null){
            BoxCollider2D col = objetoHitbox.GetComponent<BoxCollider2D>();
            if (col != null) col.enabled = false;
        }
    }

    private void Update() {
        if (pivotAtaque != null && !aAtacar){
            Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicaoMouse.z = 0f;
            Vector2 direcao = posicaoMouse - pivotAtaque.position;
            float anguloDoAtaque = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            pivotAtaque.rotation = Quaternion.Euler(new Vector3(0, 0, anguloDoAtaque));
        }
        if (Input.GetMouseButtonDown(0)){
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

        if (objetoHitbox != null){
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