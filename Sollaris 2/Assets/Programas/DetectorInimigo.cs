using UnityEngine;

public class DetectorInimigo : MonoBehaviour {
    public float danoInimigo = 10f;
    public float tempoEntreAtaques = 1.5f;
    private float proximoAtaque;

    private void OnTriggerStay2D(Collider2D outro) {
        if (outro.CompareTag("Player")) {
            if (Time.time >= proximoAtaque) {
                AtacarPlayer(outro.gameObject);
                proximoAtaque = Time.time + tempoEntreAtaques;
            }
        }
    }

    void AtacarPlayer(GameObject playerObj) {
        VidaJogador vidaPlayer = playerObj.GetComponent<VidaJogador>();
        if (vidaPlayer != null) {
            vidaPlayer.TomarDano(danoInimigo);
        }
    }
}