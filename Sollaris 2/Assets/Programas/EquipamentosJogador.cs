using UnityEngine;

public class EquipamentosJogador : MonoBehaviour
{
    public InventarioJogador.ItemSlot slotCapacete;
    public InventarioJogador.ItemSlot slotTorso;
    public InventarioJogador.ItemSlot slotCintura;
    public InventarioJogador.ItemSlot slotCalca;
    public InventarioJogador.ItemSlot slotBota;
    private VidaJogador vidaJogador;

    void Start()
    {
        vidaJogador = GetComponent<VidaJogador>();
        AtualizarDefesaEquipamentos();
    }
    public void AtualizarDefesaEquipamentos()
    {
        if (vidaJogador == null){
            return;
        }
        float defesaTotalEquips = 0f;
        if (slotCapacete != null){
            defesaTotalEquips = defesaTotalEquips + slotCapacete.defesaItem;
        }
        if (slotTorso != null){
            defesaTotalEquips = defesaTotalEquips + slotTorso.defesaItem;
        }
        if (slotCintura != null){
            defesaTotalEquips = defesaTotalEquips + slotCintura.defesaItem;
        }
        if (slotCalca != null){
            defesaTotalEquips = defesaTotalEquips + slotCalca.defesaItem;
        }
        if (slotBota != null){
            defesaTotalEquips = defesaTotalEquips + slotBota.defesaItem;
        }
        vidaJogador.DefinirDefesaEquipamento(defesaTotalEquips);
    }
}