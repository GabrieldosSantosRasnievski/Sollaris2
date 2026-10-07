using UnityEngine;

public class SistemaCrafting : MonoBehaviour{
    public static SistemaCrafting Instance;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    public void TentarCraftar(ReceitaCrafting receita){
        if(receita == null || InventarioJogador.Instance == null){
            return;
        }
        if(!TemIngredientesSuficientes(receita)){
            Debug.Log("Ingredientes insuficientes para craftar: " + receita.nomeItemResultado);
            return;
        }
        ConsumirIngredientes(receita);
        bool adicionado = InventarioJogador.Instance.TentarAdicionar(
            receita.nomeItemResultado,
            receita.iconeResultado,
            receita.podeConsumirResultado,
            receita.valorFomeResultado,
            receita.danoResultado,
            receita.tamanhoHitboxResultado,
            receita.quebraAoAtingirResultado,
            0f,
            false, false, false, false, false,
            receita.ehMachadoResultado,
            receita.ehPicaretaResultado
        );
        if(adicionado){
            Debug.Log("Item craftado com sucesso: " + receita.nomeItemResultado);
            AbrirInventario ui = FindObjectOfType<AbrirInventario>();
            if(ui != null){
                ui.AtualizarUI();
            }
        }
        else{
            Debug.Log("Inventário cheio! Não foi possível adicionar o item craftado.");
            DevolverIngredientes(receita);
        }
    }
    private bool TemIngredientesSuficientes(ReceitaCrafting receita){
        foreach(var ingrediente in receita.ingredientes){
            int quantidadeTotalNoInventario = ObterQuantidadeTotalItem(ingrediente.nomeItem);
            if(quantidadeTotalNoInventario < ingrediente.quantidade){
                return false;
            }
        }
        return true;
    }
    private void ConsumirIngredientes(ReceitaCrafting receita){
        foreach(var ingrediente in receita.ingredientes){
            int quantidadeFaltando = ingrediente.quantidade;
            foreach(var slot in InventarioJogador.Instance.slotsRapidos){
                if(quantidadeFaltando <= 0) break;
                if(slot.nomeItem == ingrediente.nomeItem){
                    if(slot.quantidadeItem >= quantidadeFaltando){
                        slot.quantidadeItem -= quantidadeFaltando;
                        quantidadeFaltando = 0;
                    }
                    else{
                        quantidadeFaltando -= slot.quantidadeItem;
                        slot.quantidadeItem = 0;
                    }
                    if(slot.quantidadeItem <= 0){
                        slot.nomeItem = "";
                        slot.iconeItem = null;
                        slot.podeConsumir = false;
                        slot.valorFome = 0f;
                        slot.danoItem = 0f;
                        slot.defesaItem = 0f;
                        slot.tamanhoHitbox = new Vector2(1f, 1f);
                        slot.quebraAoAtingir = false;
                        slot.podeColocarCabeca = false;
                        slot.podeColocarTorso = false;
                        slot.podeColocarCintura = false;
                        slot.podeColocarCalca = false;
                        slot.podeColocarBota = false;
                        slot.ehMachado = false;
                        slot.ehPicareta = false;
                    }
                }
            }
            foreach(var slot in InventarioJogador.Instance.inventario){
                if(quantidadeFaltando <= 0) break;
                if(slot.nomeItem == ingrediente.nomeItem){
                    if(slot.quantidadeItem >= quantidadeFaltando){
                        slot.quantidadeItem -= quantidadeFaltando;
                        quantidadeFaltando = 0;
                    }
                    else{
                        quantidadeFaltando -= slot.quantidadeItem;
                        slot.quantidadeItem = 0;
                    }
                    if(slot.quantidadeItem <= 0){
                        slot.nomeItem = "";
                        slot.iconeItem = null;
                        slot.podeConsumir = false;
                        slot.valorFome = 0f;
                        slot.danoItem = 0f;
                        slot.defesaItem = 0f;
                        slot.tamanhoHitbox = new Vector2(1f, 1f);
                        slot.quebraAoAtingir = false;
                        slot.podeColocarCabeca = false;
                        slot.podeColocarTorso = false;
                        slot.podeColocarCintura = false;
                        slot.podeColocarCalca = false;
                        slot.podeColocarBota = false;
                        slot.ehMachado = false;
                        slot.ehPicareta = false;
                    }
                }
            }
        }
    }
    private void DevolverIngredientes(ReceitaCrafting receita){
        foreach(var ingrediente in receita.ingredientes){
            InventarioJogador.Instance.TentarAdicionar(ingrediente.nomeItem, null, false, 0f, 0f, Vector2.one, false);
        }
    }
    private int ObterQuantidadeTotalItem(string nomeItem){
        int total = 0;
        foreach(var slot in InventarioJogador.Instance.slotsRapidos){
            if(slot.nomeItem == nomeItem){
                total = total + slot.quantidadeItem;
            }
        }
        foreach(var slot in InventarioJogador.Instance.inventario){
            if(slot.nomeItem == nomeItem){
                total = total + slot.quantidadeItem;
            }
        }
        return total;
    }
}