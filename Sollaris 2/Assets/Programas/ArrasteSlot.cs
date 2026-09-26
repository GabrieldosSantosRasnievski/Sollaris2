using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControladorSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler{
    public enum TipoSlot { Rapido, Inventario }
    public TipoSlot tipoSlot;
    public int indiceSlot;
    private static GameObject iconeArrastadoObj;
    private static int indiceOrigem = -1;
    private static TipoSlot origemTipo;
    
    public void OnBeginDrag(PointerEventData eventData){

        var inventario = InventarioJogador.Instance;
        if(inventario == null) return;
        var slotDados = (tipoSlot == TipoSlot.Rapido) ? inventario.slotsRapidos[indiceSlot] : inventario.inventario[indiceSlot];
        if(slotDados == null || string.IsNullOrEmpty(slotDados.nomeItem) || slotDados.quantidadeItem <= 0){
            return;
        }
        indiceOrigem = indiceSlot;
        origemTipo = tipoSlot;
        iconeArrastadoObj = new GameObject("IconeArrastado");
        iconeArrastadoObj.transform.SetParent(transform.root);
        iconeArrastadoObj.transform.SetAsLastSibling();
        Image img = iconeArrastadoObj.AddComponent<Image>();
        img.sprite = slotDados.iconeItem;
        img.raycastTarget = false; // Para não bloquear o drop
        RectTransform rect = iconeArrastadoObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50f, 50f);
    }
    public void OnDrag(PointerEventData eventData){
        if(iconeArrastadoObj != null){
            iconeArrastadoObj.transform.position = Input.mousePosition;
        }
    }
    public void OnDrop(PointerEventData eventData){
        if(indiceOrigem == -1) return;
        var inventario = InventarioJogador.Instance;
        if(inventario != null){
            inventario.MoverItem(origemTipo, indiceOrigem, tipoSlot, indiceSlot);
        }
        LimparArrasto();
        FindObjectOfType<AbrirInventario>()?.AtualizarUI();
    }
    public void OnEndDrag(PointerEventData eventData){
        LimparArrasto();
    }
    private void LimparArrasto(){
        if(iconeArrastadoObj != null){
            Destroy(iconeArrastadoObj);
        }
        indiceOrigem = -1;
    }
}