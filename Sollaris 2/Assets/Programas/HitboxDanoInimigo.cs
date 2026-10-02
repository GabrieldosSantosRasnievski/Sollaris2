using UnityEngine;

public class HitboxDanoInimigo : MonoBehaviour {
    public float dano = 10f;

    private void OnTriggerEnter2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            VidaJogador vidaPlayer = outro.GetComponent<VidaJogador>();
            if (vidaPlayer != null) {
                vidaPlayer.TomarDano(dano);
            }
        }
    }
}