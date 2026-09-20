using UnityEngine;

public class VidaInimigo : MonoBehaviour{
    public float vidaAtual = 30f;

    public void TomarDano(float quantidade){
        Debug.Log("Recebeu dano de: " + quantidade);
        vidaAtual = vidaAtual - quantidade;
        Debug.Log("Inimigo atingido! Vida restante: " + vidaAtual);

        if(vidaAtual <= 0){
            Morrer();
        }
    }

    void Morrer(){
        Debug.Log("Inimigo derrotado!");
        Destroy(gameObject);
    }
}