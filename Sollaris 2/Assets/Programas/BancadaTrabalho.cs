using UnityEngine;

public class BancadaTrabalho : MonoBehaviour {
    public GameObject painelBancadaTrabalho;
    private bool jogadorNaArea = false;

    void Update() {
        if (jogadorNaArea && Input.GetKeyDown(KeyCode.E)) {
            if (painelBancadaTrabalho != null) {
                bool estaAtivo = painelBancadaTrabalho.activeSelf;
                painelBancadaTrabalho.SetActive(!estaAtivo);
                Time.timeScale = estaAtivo ? 1f : 0f;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            jogadorNaArea = true;
            Debug.Log("Pressione 'E' para usar a Bancada de Trabalho.");
        }
    }
    private void OnTriggerExit2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            jogadorNaArea = false;
            if (painelBancadaTrabalho != null) {
                painelBancadaTrabalho.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}