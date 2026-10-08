using UnityEngine;
using System.Collections;

public class Arvore : MonoBehaviour {
    public float vidaArvore = 100f;
    public GameObject prefabItemMadeira;
    public int minMadeiras = 2;
    public int maxMadeiras = 6;
    public ParticleSystem particulaMadeira;
    private Vector3 posicaoOriginal;

    void Start() {
        posicaoOriginal = transform.localPosition;
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
        StartCoroutine(TrementoArvore());
        if (particulaMadeira != null) {
            particulaMadeira.gameObject.SetActive(true);
            particulaMadeira.Play();
        }
        if (vidaArvore <= 0f) {
            DroparMadeira();
            DestruirArvore();
        }
    }
    private IEnumerator TrementoArvore() {
        float duracao = 0.15f;
        float magnitude = 0.1f;
        float tempoDecorrido = 0f;
        while (tempoDecorrido < duracao) {
            float deslocamentoX = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = posicaoOriginal + new Vector3(deslocamentoX, 0f, 0f);
            tempoDecorrido += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = posicaoOriginal;
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