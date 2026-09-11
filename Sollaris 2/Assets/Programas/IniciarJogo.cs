using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class IniciarJogo : MonoBehaviour
{

    public Slider barraCarregamento;

    void Start(){
        StartCoroutine(CarregarFaseAsync("Jogo"));
    }

    IEnumerator CarregarFaseAsync(string Jogo){
        AsyncOperation operacao = SceneManager.LoadSceneAsync(Jogo);

        operacao.allowSceneActivation = false;

        while (!operacao.isDone){
            float progressoDaBarra = Mathf.Clamp01(operacao.progress / 0.9f);

            if (barraCarregamento != null){
                barraCarregamento.value = progressoDaBarra;
            }

            if (operacao.progress >= 0.9f){
                operacao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
