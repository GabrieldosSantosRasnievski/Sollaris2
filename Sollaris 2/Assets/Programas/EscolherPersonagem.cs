using UnityEngine;

public class EscolherPersonagem : MonoBehaviour
{
    public GameObject painelSelecao;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PersonagemEscolhido")){
            painelSelecao.SetActive(true);
            Time.timeScale = 0f;
        } 
        else{
           painelSelecao.SetActive(false);
        }
    }
    public void SelecionarHomem(){
        SalvarEscolha("Homem");
        Time.timeScale = 1f;
    }

    public void SelecionarMulher(){
        SalvarEscolha("Mulher");
        Time.timeScale = 1f;
    }

    private void SalvarEscolha(string genero){
        PlayerPrefs.SetString("GeneroPlayer", genero);
        PlayerPrefs.SetInt("PersonagemEscolhido", 1);
        PlayerPrefs.Save();
        TesteMovimento jogador = FindFirstObjectByType<TesteMovimento>();
        if (jogador != null){
            jogador.AtualizarGenero();
        }
        painelSelecao.SetActive(false);
    }
}
