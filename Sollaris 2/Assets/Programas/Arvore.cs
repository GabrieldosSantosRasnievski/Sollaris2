using UnityEngine;

public class Arvore : MonoBehaviour {
    public float vidaArvore = 100f;
    public GameObject prefabItemMadeira;
    public int minMadeiras = 2;
    public int maxMadeiras = 6;

    public void ReceberDano(float dano, bool ehMachado) {
        if (!ehMachado) {
            Debug.Log("Ataque ignorado! Você precisa usar um machado para cortar a árvore.");
            return;
        }
        vidaArvore = vidaArvore - dano;
        Debug.Log("Árvore atingida! Vida restante: " + vidaArvore);
        if (vidaArvore <= 0f) {
            DroparMadeira();
            DestruirArvore();
        }
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