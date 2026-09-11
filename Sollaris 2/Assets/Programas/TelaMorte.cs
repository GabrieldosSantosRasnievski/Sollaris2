using UnityEngine;

public class TelaMorte : MonoBehaviour
{
    private VidaJogador jogador;

    public void ExibirTelaMorte(VidaJogador jogador2){
        jogador = jogador2;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Respawnar(){
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        if(jogador != null){
            jogador.Respawnar();
        }
    }
}
