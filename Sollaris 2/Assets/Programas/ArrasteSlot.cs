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
    private static PointerEventData.InputButton botaoUtilizado;
    
    public void OnBeginDrag(PointerEventData eventData) {
        var inventario = InventarioJogador.Instance;
        if(inventario == null){
            return;
        }
        origemTipo = tipoSlot;
        indiceOrigem = indiceSlot;
        botaoUtilizado = eventData.button;
        InventarioJogador.ItemSlot slotDados = ObterSlotPorTipo(origemTipo, indiceOrigem);
        if(slotDados == null || string.IsNullOrEmpty(slotDados.nomeItem) || slotDados.quantidadeItem <= 0){
            indiceOrigem = -1;
            return;
        }

        SlotEquipamento[] todosSlotsEquipamento = FindObjectsOfType<SlotEquipamento>();
        foreach (var slotEq in todosSlotsEquipamento) {
            bool compativel = false;
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Cabeca && slotDados.podeColocarCabeca) compativel = true;
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Torso && slotDados.podeColocarTorso) compativel = true;
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Cintura && slotDados.podeColocarCintura) compativel = true;
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Calca && slotDados.podeColocarCalca) compativel = true;
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Bota && slotDados.podeColocarBota) compativel = true;

            if (compativel) {
                InventarioJogador.ItemSlot slotDestinoEquip = ObterSlotEquipamentoDoInventario(slotEq.tipoSlot, inventario);
                if (slotDestinoEquip != null && string.IsNullOrEmpty(slotDestinoEquip.nomeItem)) {
                    slotEq.LigarDestaqueVisual();
                }
            }
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
            if (botaoUtilizado == PointerEventData.InputButton.Right) {
                inventario.MoverUmItem(origemTipo, indiceOrigem, tipoSlot, indiceSlot);
            } else {
                inventario.MoverItem(origemTipo, indiceOrigem, tipoSlot, indiceSlot);
            }
        }
        LimparArrasto();
        FindObjectOfType<AbrirInventario>()?.AtualizarUI();
    }
    public void OnEndDrag(PointerEventData eventData) {
        LimparArrasto();
    }
    private void LimparArrasto() {
        SlotEquipamento[] todosSlotsEquipamento = FindObjectsOfType<SlotEquipamento>();
        foreach (var slotEq in todosSlotsEquipamento) {
            slotEq.DesligarDestaqueVisual();
        }
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
    private InventarioJogador.ItemSlot ObterSlotEquipamentoDoInventario(SlotEquipamento.TipoEquipamento tipo, InventarioJogador inv) {
        switch (tipo) {
            case SlotEquipamento.TipoEquipamento.Cabeca: return inv.slotCabeca;
            case SlotEquipamento.TipoEquipamento.Torso: return inv.slotTorso;
            case SlotEquipamento.TipoEquipamento.Cintura: return inv.slotCintura;
            case SlotEquipamento.TipoEquipamento.Calca: return inv.slotCalca;
            case SlotEquipamento.TipoEquipamento.Bota: return inv.slotBota;
            default: return null;
        }
    }
}