using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotEquipamento : MonoBehaviour, IDropHandler {
    public enum TipoEquipamento { Cabeca, Torso, Cintura, Calca, Bota }
    public TipoEquipamento tipoSlot;

    public void OnDrop(PointerEventData eventData) {
        GameObject objetoArrastado = eventData.pointerDrag;
        if (objetoArrastado == null){
            return;
        }
        ArrasteSlot slotOrigemUI = objetoArrastado.GetComponent<ArrasteSlot>();
        if (slotOrigemUI == null){
            return;
        }

        List<InventarioJogador.ItemSlot> listaOrigem = (slotOrigemUI.tipoSlot == ArrasteSlot.TipoSlot.Rapido) ? 
            InventarioJogador.Instance.slotsRapidos : InventarioJogador.Instance.inventario;

        if (slotOrigemUI.indiceSlot < 0 || slotOrigemUI.indiceSlot >= listaOrigem.Count){
            return;
        }
        InventarioJogador.ItemSlot itemMovido = listaOrigem[slotOrigemUI.indiceSlot];
        if (string.IsNullOrEmpty(itemMovido.nomeItem)){
            return;
        }
        if (VerificarSePodeEquipar(itemMovido)) {
            InventarioJogador.ItemSlot slotDestino = ObterSlotEquipamentoCorrespondente();
            if (slotDestino == null){
                return;
            }
            InventarioJogador.ItemSlot itemAntigo = new InventarioJogador.ItemSlot(
                slotDestino.nomeItem, slotDestino.quantidadeItem, slotDestino.iconeItem,
                slotDestino.podeConsumir, slotDestino.valorFome, slotDestino.danoItem,
                slotDestino.defesaItem, slotDestino.tamanhoHitbox, slotDestino.quebraAoAtingir,
                slotDestino.podeColocarCabeca, slotDestino.podeColocarTorso,
                slotDestino.podeColocarCintura, slotDestino.podeColocarCalca, slotDestino.podeColocarBota
            );
            slotDestino.nomeItem = itemMovido.nomeItem;
            slotDestino.quantidadeItem = itemMovido.quantidadeItem;
            slotDestino.iconeItem = itemMovido.iconeItem;
            slotDestino.podeConsumir = itemMovido.podeConsumir;
            slotDestino.valorFome = itemMovido.valorFome;
            slotDestino.danoItem = itemMovido.danoItem;
            slotDestino.defesaItem = itemMovido.defesaItem;
            slotDestino.tamanhoHitbox = itemMovido.tamanhoHitbox;
            slotDestino.quebraAoAtingir = itemMovido.quebraAoAtingir;
            slotDestino.podeColocarCabeca = itemMovido.podeColocarCabeca;
            slotDestino.podeColocarTorso = itemMovido.podeColocarTorso;
            slotDestino.podeColocarCintura = itemMovido.podeColocarCintura;
            slotDestino.podeColocarCalca = itemMovido.podeColocarCalca;
            slotDestino.podeColocarBota = itemMovido.podeColocarBota;
            if (!string.IsNullOrEmpty(itemAntigo.nomeItem)) {
                listaOrigem[slotOrigemUI.indiceSlot] = itemAntigo;
            } else {
                listaOrigem[slotOrigemUI.indiceSlot] = new InventarioJogador.ItemSlot();
            }
            AbrirInventario ui = FindObjectOfType<AbrirInventario>();
            if (ui != null) ui.AtualizarUI();
        } else {
            Debug.LogWarning("Este item não pode ser equipado neste espaço!");
        }
    }

    bool VerificarSePodeEquipar(InventarioJogador.ItemSlot item) {
        switch (tipoSlot) {
            case TipoEquipamento.Cabeca: return item.podeColocarCabeca;
            case TipoEquipamento.Torso: return item.podeColocarTorso;
            case TipoEquipamento.Cintura: return item.podeColocarCintura;
            case TipoEquipamento.Calca: return item.podeColocarCalca;
            case TipoEquipamento.Bota: return item.podeColocarBota;
            default: return false;
        }
    }

    InventarioJogador.ItemSlot ObterSlotEquipamentoCorrespondente() {
        var inv = InventarioJogador.Instance;
        switch (tipoSlot) {
            case TipoEquipamento.Cabeca: return inv.slotCabeca;
            case TipoEquipamento.Torso: return inv.slotTorso;
            case TipoEquipamento.Cintura: return inv.slotCintura;
            case TipoEquipamento.Calca: return inv.slotCalca;
            case TipoEquipamento.Bota: return inv.slotBota;
            default: return null;
        }
    }
}