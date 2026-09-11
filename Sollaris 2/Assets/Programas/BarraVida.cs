using UnityEngine;
using UnityEngine.UI;
public class BarraVida : MonoBehaviour
{
    private Slider slider;
    private void Awake(){
        slider = GetComponent<Slider>();
    }
    public void AtualizarBarraVida(float vidaAtual, float vidaMaxima){
    if (slider != null && vidaMaxima > 0){
        slider.value = vidaAtual / vidaMaxima;
        } 
    }
}
