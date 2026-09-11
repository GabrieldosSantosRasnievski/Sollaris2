using UnityEngine;
using UnityEngine.Events;
public class FomeJogador : MonoBehaviour
{
    public float fomeMaxima = 100f;
    public float fomeAtual;
    public float ganhoFome = 0.5f;
    public float danoFome = 2f;
    public float intevaloDano = 2f;
    private float temporizadorFome = 2f;
    private VidaJogador vidaJogador;
    public UnityEvent<float, float> OnFomeAlterada;
    public float porcentagemRegen = 0.8f;
    public float curaFome = 5f;
    public float intervaloCura = 2f;
    private float temporizadorCura = 2f;
    private void Awake(){
        vidaJogador = GetComponent<VidaJogador>();
        if (!PlayerPrefs.HasKey("FomeSalva")){
            fomeAtual = fomeMaxima;
            SalvarFome();
        }
        else{
            fomeAtual = PlayerPrefs.GetFloat("FomeSalva");
        }
    }
    private void Start(){
        if(OnFomeAlterada != null){
            OnFomeAlterada.Invoke(fomeAtual, fomeMaxima);
        }
    }
    private void Update(){
        if (fomeAtual > 0){
            fomeAtual = fomeAtual - ganhoFome * Time.deltaTime;
            fomeAtual = Mathf.Clamp(fomeAtual, 0, fomeMaxima);
            SalvarFome();
            if(OnFomeAlterada != null){
                OnFomeAlterada.Invoke(fomeAtual, fomeMaxima);
            }
            float limiteRegen = fomeMaxima * porcentagemRegen;
            if(fomeAtual >= limiteRegen){
                if(vidaJogador != null && vidaJogador.vidaAtual < vidaJogador.vidaMaxima){
                    temporizadorCura = temporizadorCura + Time.deltaTime;
                    if (temporizadorCura >= intervaloCura){
                        vidaJogador.Curar(curaFome);
                        temporizadorCura = 0f;
                    }
                }
            }
            else{
                temporizadorCura = 0f;
            }

        }
        else{
            temporizadorCura = 0f;
            temporizadorFome = temporizadorFome + Time.deltaTime;
            if(temporizadorFome >= intevaloDano){
                if(vidaJogador != null){
                    vidaJogador.TomarDano(danoFome);
                }
                temporizadorFome = 0f;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Comer(20f);
        }
    }
    public void Comer(float quantidade){
        fomeAtual = fomeAtual + quantidade;
        fomeAtual = Mathf.Clamp(fomeAtual, 0, fomeMaxima);
        temporizadorFome = 0f;
        temporizadorCura = 0f;
        if(OnFomeAlterada != null){
            OnFomeAlterada.Invoke(fomeAtual, fomeMaxima);
        }
    }
    public void RespawnMetadeFome(){
        fomeAtual = fomeMaxima / 2f;
        temporizadorFome = 0f;
        temporizadorCura = 0f;
        SalvarFome();
        if(OnFomeAlterada != null){
            OnFomeAlterada.Invoke(fomeAtual, fomeMaxima);
        }
    }
    public void SalvarFome(){
        PlayerPrefs.SetFloat("FomeSalva", fomeAtual);
        PlayerPrefs.Save();
    }
}
