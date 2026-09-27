using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrasteSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler {
    
    public enum TipoSlot { Rapido, Inventario, Cabeca, Torso, Cintura, Calca, Bota }
    
    public TipoSlot tipoSlot;
    public int indiceSlot;
    private static GameObject iconeArrastadoObj;
    private static int indiceOrigem = -1;
    private static TipoSlot origemTipo;
    
    public void OnBeginDrag(PointerEventData eventData) {
        var inventario = InventarioJogador.Instance;
        if(inventario == null){
            return;
        }
        origemTipo = tipoSlot;
        indiceOrigem = indiceSlot;

        InventarioJogador.ItemSlot slotDados = ObterSlotPorTipo(origemTipo, indiceOrigem);

        if(slotDados == null || string.IsNullOrEmpty(slotDados.nomeItem) || slotDados.quantidadeItem <= 0){
            indiceOrigem = -1;
            return;
        }

        iconeArrastadoObj = new GameObject("IconeArrastado");
        iconeArrastadoObj.transform.SetParent(transform.root);
        iconeArrastadoObj.transform.SetAsLastSibling();
        Image img = iconeArrastadoObj.AddComponent<Image>();
        img.sprite = slotDados.iconeItem;
        img.raycastTarget = false; 
        RectTransform rect = iconeArrastadoObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50f, 50f);
    }

    public void OnDrag(PointerEventData eventData) {
        if(iconeArrastadoObj != null){
            iconeArrastadoObj.transform.position = Input.mousePosition;
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if(indiceOrigem == -1){
            return;
        }
        var inventario = InventarioJogador.Instance;
        if(inventario != null){
            inventario.MoverItem(origemTipo, indiceOrigem, tipoSlot, indiceSlot);
        }
        LimparArrasto();
        FindObjectOfType<AbrirInventario>()?.AtualizarUI();
    }

    public void OnEndDrag(PointerEventData eventData) {
        LimparArrasto();
    }

    private void LimparArrasto() {
        if(iconeArrastadoObj != null){
            Destroy(iconeArrastadoObj);
        }
        indiceOrigem = -1;
    }

    private InventarioJogador.ItemSlot ObterSlotPorTipo(TipoSlot tipo, int indice) {
        var inv = InventarioJogador.Instance;
        if (inv == null){
            return null;
        }
        switch (tipo) {
            case TipoSlot.Rapido:
                if (indice >= 0 && indice < inv.slotsRapidos.Count) return inv.slotsRapidos[indice];
                break;
            case TipoSlot.Inventario:
                if (indice >= 0 && indice < inv.inventario.Count) return inv.inventario[indice];
                break;
            case TipoSlot.Cabeca: return inv.slotCabeca;
            case TipoSlot.Torso: return inv.slotTorso;
            case TipoSlot.Cintura: return inv.slotCintura;
            case TipoSlot.Calca: return inv.slotCalca;
            case TipoSlot.Bota: return inv.slotBota;
        }
        return null;
    }
}