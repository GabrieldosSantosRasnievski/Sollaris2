using UnityEngine;
using UnityEngine.Events;

public class VidaJogador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public UnityEvent<float, float> OnVidaAlterada;
    public TelaMorte telaMorte;
    public Transform pontoRespawn;
    private void Awake(){
        if(PlayerPrefs.HasKey("VidaSalva")){
            vidaAtual = PlayerPrefs.GetFloat("VidaSalva");
        }
        else{
            vidaAtual = vidaMaxima;
            SalvarVida();
        }
    }
    private void Start(){
        if(OnVidaAlterada != null){
            OnVidaAlterada.Invoke(vidaAtual, vidaMaxima);
        }
        if(pontoRespawn == null){
            pontoRespawn = new GameObject("PontoRespawnPadrao").transform;
            pontoRespawn.position = transform.position;
        }
    }
    public void TomarDano(float quantidade){
            vidaAtual = vidaAtual - quantidade;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if(OnVidaAlterada != null){
            OnVidaAlterada.Invoke(vidaAtual, vidaMaxima);
        }

            if (vidaAtual <= 0){
                Morrer();
            }
    }
        public void Curar(float quantidade){
            vidaAtual = vidaAtual + quantidade;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if(OnVidaAlterada != null){
            OnVidaAlterada.Invoke(vidaAtual, vidaMaxima);
        }
        }
        private void Morrer(){
            if(telaMorte != null){
                telaMorte.ExibirTelaMorte(this);
            }
        }
        public void Respawnar(){
            transform.position = pontoRespawn.position;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if(rb != null){
                rb.linearVelocity = Vector2.zero;
            }
            Curar(vidaMaxima/2);
            FomeJogador fome = GetComponent<FomeJogador>();
            if(fome != null){
                fome.RespawnMetadeFome();
            }
        }
        public void SalvarVida(){
            PlayerPrefs.SetFloat("VidaSalva", vidaAtual);
            PlayerPrefs.Save();
        }


        // private void Update(){
        //     if(Input.GetKeyDown(KeyCode.Space)){
        //         TomarDano(20f);
        //     }
        // }
    }

