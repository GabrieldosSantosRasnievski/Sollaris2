using UnityEngine;
using TMPro;

public class BancadaTrabalho : MonoBehaviour {
    public GameObject painelBancadaTrabalho;
    public TextMeshProUGUI textoInteracao;
    private bool jogadorNaArea = false;

    private void Start() {
        if (painelBancadaTrabalho != null) {
            painelBancadaTrabalho.SetActive(false);
        }

        if (textoInteracao != null) {
            textoInteracao.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if (jogadorNaArea) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (painelBancadaTrabalho != null) {
                    bool estaAtivo = painelBancadaTrabalho.activeSelf;

                    if (estaAtivo) {
                        FechandoBancada();
                    } 
                    else {
                        AbrindoBancada();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.B)) {
                if (painelBancadaTrabalho != null) {
                    if (painelBancadaTrabalho.activeSelf) {
                        FechandoBancada();
                    }
                }
            }
        }
    }

    private void AbrindoBancada() {
        painelBancadaTrabalho.SetActive(true);
        Time.timeScale = 0f;
        AbrirInventario.bloqueadoPorOutraUI = true;

        if (textoInteracao != null) {
            textoInteracao.gameObject.SetActive(false);
        }
    }

    private void FechandoBancada() {
        painelBancadaTrabalho.SetActive(false);
        Time.timeScale = 1f;
        
        // Regista o tempo exato em que a bancada foi fechada
        AbrirInventario.RegistrarFechamentoUI();

        if (textoInteracao != null) {
            textoInteracao.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            jogadorNaArea = true;

            if (textoInteracao != null) {
                textoInteracao.gameObject.SetActive(true);
            }

            Debug.Log("Pressione 'E' para usar a Bancada de Trabalho.");
        }
    }

    private void OnTriggerExit2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            jogadorNaArea = false;

            if (textoInteracao != null) {
                textoInteracao.gameObject.SetActive(false);
            }

            if (painelBancadaTrabalho != null) {
                if (painelBancadaTrabalho.activeSelf) {
                    FechandoBancada();
                }
            }
        }
    }
}