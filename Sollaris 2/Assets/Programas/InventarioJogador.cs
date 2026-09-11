using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InventarioJogador : MonoBehaviour
{
    public static InventarioJogador Instance;
    public class ItemSlot{
        public string nomeItem;
        public int quantidadeItem;
        public Sprite iconeItem;
    }
    public List<ItemSlot> slotsRapidos = new List<ItemSlot>();
    public int limiteTiposSlotsRapidos = 9;
    public List<ItemSlot> inventario = new List<ItemSlot>();
    public int limiteTiposInventario = 20;
    public int slotSelecionado = 0;
    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            slotSelecionado = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            slotSelecionado = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            slotSelecionado = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            slotSelecionado = 3;
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            slotSelecionado = 4;
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            slotSelecionado = 5;
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            slotSelecionado = 6;
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            slotSelecionado = 7;
        }
        if(Input.GetKeyDown(KeyCode.Alpha9)){
            slotSelecionado = 8;
        }
    }
    public ItemSlot ObterItemSelecionado(){
        if(slotSelecionado >= 0 &&slotSelecionado < slotsRapidos.Count){
            return slotsRapidos[slotSelecionado];
        }
        return null;
    }
    public bool TentarAdicionar(string nomeDoItem, Sprite icone){
        foreach (ItemSlot slot in slotsRapidos){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                return true;
            }
        }
        foreach (ItemSlot slot in inventario){
            if(slot.nomeItem == nomeDoItem){
                slot.quantidadeItem++;
                return true;
            }
        }
        if (slotsRapidos.Count < limiteTiposSlotsRapidos){
            ItemSlot novoSlot = new ItemSlot {nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone};
            slotsRapidos.Add(novoSlot);
            return true;
        }
        if(inventario.Count < limiteTiposInventario){
            ItemSlot novoSlot = new ItemSlot {nomeItem = nomeDoItem, quantidadeItem = 1, iconeItem = icone};
            inventario.Add(novoSlot);
            return true;
        }
        return false;
    }
}
