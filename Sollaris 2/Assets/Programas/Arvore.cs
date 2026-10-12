using UnityEngine;
using System.Collections;

public class Arvore : MonoBehaviour {
    public float vidaArvore = 100f;
    public GameObject prefabItemMadeira;
    public int minMadeiras = 2;
    public int maxMadeiras = 6;
    public ParticleSystem particulaMadeira;
    
    private Vector3 posicaoOriginal;
    private Quaternion rotacaoOriginal;

    void Start() {
        posicaoOriginal = transform.localPosition;
        rotacaoOriginal = transform.localRotation;

        if (particulaMadeira != null) {
            particulaMadeira.gameObject.SetActive(false);
        }
    }

    public void ReceberDano(float dano, bool ehMachado) {
        if (!ehMachado) {
            Debug.Log("Ataque ignorado! Você precisa usar um machado para cortar a árvore.");
            return;
        }

        vidaArvore = vidaArvore - dano;
        Debug.Log("Árvore atingida! Vida restante: " + vidaArvore);
        StopAllCoroutines();
        StartCoroutine(TrementoArvore(dano));

        if (particulaMadeira != null) {
            particulaMadeira.gameObject.SetActive(true);
            particulaMadeira.Play();
        }
        if (vidaArvore <= 0f) {
            DroparMadeira();
            DestruirArvore();
        }
    }
    private IEnumerator TrementoArvore(float dano) {
        float magnitude = Mathf.Clamp(dano * 0.015f, 0.05f, 0.4f);
        float duracao = Mathf.Clamp(0.1f + (dano * 0.005f), 0.15f, 0.5f);
        float tempoDecorrido = 0f;
        while (tempoDecorrido < duracao) {
            float deslocamentoX = Random.Range(-1f, 1f) * magnitude;
            float anguloInclinacao = Random.Range(-1f, 1f) * (magnitude * 30f);

            transform.localPosition = posicaoOriginal + new Vector3(deslocamentoX, 0f, 0f);
            transform.localRotation = rotacaoOriginal * Quaternion.Euler(0f, 0f, anguloInclinacao);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = posicaoOriginal;
        transform.localRotation = rotacaoOriginal;
    }
    private void DroparMadeira() {
        int quantidadeDrop = Random.Range(minMadeiras, maxMadeiras + 1);
        if (prefabItemMadeira != null) {
            for (int i = 0; i < quantidadeDrop; i++) {
                Vector2 posicaoAleatoria = (Vector2)transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                Instantiate(prefabItemMadeira, posicaoAleatoria, Quaternion.identity);
            }
        } else {
            Debug.LogWarning("Prefab de Madeira não foi atribuído no Inspector da árvore!");
        }
    }
    private void DestruirArvore() {
        Destroy(gameObject);
    }
}