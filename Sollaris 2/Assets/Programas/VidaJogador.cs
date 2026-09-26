using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class VidaJogador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public float defesaBase = 0f;
    public float defesaEquipamento = 0f;
    public UnityEvent<float, float> OnVidaAlterada;
    public TelaMorte telaMorte;
    public Transform pontoRespawn;
    public Animator animacaoDano;
    public TesteMovimento scriptMovimento;

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
    public float ObterDefesaTotal(){
        return defesaBase + defesaEquipamento;
    }
    public void ModificarDefesaEquipamento(float valor){
        defesaEquipamento += valor;
        Debug.Log("Defesa de equipamento alterada! Defesa Total: " + ObterDefesaTotal());
    }
    public void TomarDano(float quantidadeBruta){
        float defesaTotal = ObterDefesaTotal();
        float quantidadeFinal = Mathf.Max(1f, quantidadeBruta - defesaTotal);
        vidaAtual = vidaAtual - quantidadeFinal;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        if(OnVidaAlterada != null){
            OnVidaAlterada.Invoke(vidaAtual, vidaMaxima);
        }
        if (vidaAtual <= 0){
            Morrer();
        }
        StartCoroutine(RotinaTomarDano());
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
    public IEnumerator RotinaTomarDano()
    {
        scriptMovimento.estaTomandoDano = true;
        animacaoDano.SetTrigger("TomouDano");
        yield return new WaitForSeconds(1.03f); 
        scriptMovimento.estaTomandoDano = false;
    }
}