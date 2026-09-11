using UnityEngine;
using TMPro;
using System.Collections;
public class CicloDiaNoite : MonoBehaviour
{
    public float tempoTroca = 600f;
    public float duracaoTexto = 3f;
    public TextMeshProUGUI textoDiaNoite;
    public GameObject painelNoite;
    private float temporizador;
    private bool taDia = true;
    private int contadorDias = 1;
    private Coroutine corrotinaTexto;
    private int numeroCiclo;
    void Start(){
        temporizador = tempoTroca;
        CarregarCiclo();
        if(PlayerPrefs.HasKey("ContadorDias")){
        AlternarFase();
        }
        else{
            AtualizarEstado();
        }
        SalvarCiclo();
    }
    void Update(){
        temporizador = temporizador - Time.deltaTime;
        if(temporizador <= 0){
            AlternarFase();
            SalvarCiclo();
            temporizador = tempoTroca;
        }
    }
    void AlternarFase(){
        if(taDia){
            taDia = false;
        }
        else{
            taDia = true;
            contadorDias++;
        }
        AtualizarEstado();
    }
    void AtualizarEstado(){
        if(painelNoite != null){
            painelNoite.SetActive(!taDia);
        }
        string mensagem;
        if(textoDiaNoite != null){
            if(taDia){
                mensagem = "Dia " + contadorDias;
            }
            else {
                mensagem = "Noite " + contadorDias;
            }
            if (corrotinaTexto != null){
                StopCoroutine(corrotinaTexto);
            }
            corrotinaTexto = StartCoroutine(ExibirTextoTemporario(mensagem));
        }
    }
    IEnumerator ExibirTextoTemporario(string mensagem){
        textoDiaNoite.text = mensagem;
        textoDiaNoite.gameObject.SetActive(true);
        yield return new WaitForSeconds(duracaoTexto);
        textoDiaNoite.gameObject.SetActive(false);
    }
    void SalvarCiclo(){
        PlayerPrefs.SetInt("ContadorDias", contadorDias);
        if (taDia){
            numeroCiclo = 1;
        }
        else{
            numeroCiclo = 0;
        }
        PlayerPrefs.SetInt("NumeroCiclo", numeroCiclo);
        PlayerPrefs.Save();
    }
    void CarregarCiclo(){
        if(PlayerPrefs.HasKey("ContadorDias")){
            contadorDias = PlayerPrefs.GetInt("ContadorDias");
            int cicloSalvo = PlayerPrefs.GetInt("NumeroCiclo", 1);
            taDia = (cicloSalvo == 1);
        }
        else{
            contadorDias = 1;
            taDia = true;
        }
    }
}
