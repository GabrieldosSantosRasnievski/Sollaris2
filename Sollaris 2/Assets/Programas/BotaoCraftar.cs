using UnityEngine;
using UnityEngine.UI;

public class VisualizadorBotaoCrafting : MonoBehaviour{
    public ReceitaCrafting receita;
    public Image imagemBotao;
    public Color corDisponivel = Color.white;
    public Color corIndisponivel = Color.red;
    void Update(){
        if(gameObject.activeInHierarchy && receita != null && InventarioJogador.Instance != null){
            if(TemIngredientesSuficientes(receita)){
                if(imagemBotao != null) imagemBotao.color = corDisponivel;
            }
            else{
                if(imagemBotao != null) imagemBotao.color = Color.red;
            }
        }
    }

    private bool TemIngredientesSuficientes(ReceitaCrafting rec){
        foreach(var ingrediente in rec.ingredientes){
            int total = ObterQuantidadeTotalItem(ingrediente.nomeItem);
            if(total < ingrediente.quantidade){
                return false;
            }
        }
        return true;
    }

    private int ObterQuantidadeTotalItem(string nomeItem){
        int total = 0;
        foreach(var slot in InventarioJogador.Instance.slotsRapidos){
            if(slot.nomeItem == nomeItem){
                total += slot.quantidadeItem;
            }
        }
        foreach(var slot in InventarioJogador.Instance.inventario){
            if(slot.nomeItem == nomeItem){
                total += slot.quantidadeItem;
            }
        }
        return total;
    }
}