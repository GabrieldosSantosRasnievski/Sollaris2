using UnityEngine;
using UnityEngine.UI;
public class BarraFome : MonoBehaviour
{
    private Slider slider;
    private void Awake(){
        slider = GetComponent<Slider>();
    }
    public void AtualizarBarraFome(float fomeAtual, float fomeMaxima){
    if (slider != null && fomeMaxima > 0){
        slider.value = fomeAtual / fomeMaxima;
        } 
    }
}
