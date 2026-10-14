using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ArrasteSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler {
    
    public enum TipoSlot { Rapido, Inventario, Cabeca, Torso, Cintura, Calca, Bota }
    
    public TipoSlot tipoSlot;
    public int indiceSlot;
    private static GameObject iconeArrastadoObj;
    private static int indiceOrigem = -1;
    private static TipoSlot origemTipo;
    private static PointerEventData.InputButton botaoUtilizado;
    private static Coroutine corrotinaTremorIcone;
    private static List<GameObject> listaFantasmasAtivos = new List<GameObject>();
    public Material materialPixelArt;
    
    public Sprite spriteBrilho; // Arraste aqui no Inspector o sprite da textura de brilho!
    
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
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Cabeca && slotDados.podeColocarCabeca){
                compativel = true;
            }
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Torso && slotDados.podeColocarTorso){
                compativel = true;
            }
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Cintura && slotDados.podeColocarCintura){
                compativel = true;
            }
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Calca && slotDados.podeColocarCalca){
                compativel = true;
            }
            if (slotEq.tipoSlot == SlotEquipamento.TipoEquipamento.Bota && slotDados.podeColocarBota){
                compativel = true;
            }
            if (compativel) {
                InventarioJogador.ItemSlot slotDestinoEquip = ObterSlotEquipamentoDoInventario(slotEq.tipoSlot, inventario);
                if (slotDestinoEquip != null && string.IsNullOrEmpty(slotDestinoEquip.nomeItem)) {
                    slotEq.LigarDestaqueVisual();
                }
            }
        }
        
        LimparFantasmas();

        iconeArrastadoObj = new GameObject("IconeArrastado");
        iconeArrastadoObj.transform.SetParent(transform.root);
        iconeArrastadoObj.transform.SetAsLastSibling();
        Image img = iconeArrastadoObj.AddComponent<Image>();
        img.sprite = slotDados.iconeItem;
        img.raycastTarget = false; 

        RectTransform rect = iconeArrastadoObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(50f, 50f);
        
        ArrasteSlot instanciaBase = FindObjectOfType<ArrasteSlot>();
        if (instanciaBase != null) {
            corrotinaTremorIcone = instanciaBase.StartCoroutine(EfeitoTremerEPosImagem(slotDados.iconeItem, instanciaBase));
        }
    }
    
    private IEnumerator EfeitoTremerEPosImagem(Sprite spriteItem, ArrasteSlot contexto) {
        float magnitude = 3f;
        float timer = 0f;
        
        while (iconeArrastadoObj != null) {
            float offsetX = Mathf.Sin(Time.unscaledTime * 40f) * magnitude;
            float offsetY = Mathf.Cos(Time.unscaledTime * 45f) * magnitude;      
            Vector3 posicaoMouse = Input.mousePosition;
            iconeArrastadoObj.transform.position = posicaoMouse + new Vector3(offsetX, offsetY, 0f);
            
            iconeArrastadoObj.transform.SetAsLastSibling();
            
            timer += Time.unscaledDeltaTime;
            if (timer >= 0.03f) {
                timer = 0f;
                SpawnGhost(iconeArrastadoObj.transform.position, spriteItem, contexto);
            }
            
            yield return null;
        }
    }

    private void SpawnGhost(Vector3 pos, Sprite spr, ArrasteSlot contexto) {
        if (iconeArrastadoObj == null) return;

        // 1. Cria a pós-imagem (fantasma) centralizada no item
        GameObject ghost = new GameObject("IconeGhost");
        ghost.transform.SetParent(transform.root, false);
        ghost.transform.position = pos;
        ghost.transform.SetAsLastSibling();
        
        Image ghostImg = ghost.AddComponent<Image>();
        ghostImg.sprite = spr;
        ghostImg.raycastTarget = false;
        ghostImg.color = new Color(1f, 1f, 1f, 0.4f);
        
        RectTransform ghostRect = ghost.GetComponent<RectTransform>();
        ghostRect.sizeDelta = new Vector2(50f, 50f);
        
        listaFantasmasAtivos.Add(ghost);
        contexto.StartCoroutine(FadeOutGhost(ghost, ghostImg));

        // 2. Cria o brilho BEM MAIS LONGE e MAIS FORTE (sem ficar excessivamente transparente)
        if (spriteBrilho != null) {
            Vector2 anguloAleatorio = Random.insideUnitCircle.normalized;
            float distanciaDoCentro = Random.Range(35f, 65f); // Distância bem maior para espalhar longe do item
            Vector3 offsetEmVolta = new Vector3(anguloAleatorio.x * distanciaDoCentro, anguloAleatorio.y * distanciaDoCentro, 0f);

            GameObject brilhoObj = new GameObject("BrilhoGhost");
            brilhoObj.transform.SetParent(transform.root, false);
            brilhoObj.transform.position = pos + offsetEmVolta;
            brilhoObj.transform.SetAsLastSibling();

            Image brilhoImg = brilhoObj.AddComponent<Image>();
            brilhoImg.sprite = spriteBrilho;
            brilhoImg.raycastTarget = false;
            // Amarelo vibrante com alfa inicial alto (0.9) para brilhar bastante
            brilhoImg.color = new Color(1f, 0.95f, 0.3f, 0.9f);

            RectTransform brilhoRect = brilhoObj.GetComponent<RectTransform>();
            brilhoRect.sizeDelta = new Vector2(16f, 16f);

            listaFantasmasAtivos.Add(brilhoObj);
            contexto.StartCoroutine(FadeOutBrilho(brilhoObj, brilhoImg));
        }

        if (iconeArrastadoObj != null) {
            iconeArrastadoObj.transform.SetAsLastSibling();
        }
    }

    private IEnumerator FadeOutGhost(GameObject ghostObj, Image ghostImg) {
        float duration = 0.25f;
        float elapsed = 0f;
        
        while (elapsed < duration) {
            if (ghostObj == null || ghostImg == null) yield break;
            
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0.4f, 0f, elapsed / duration);
            Color corAtual = ghostImg.color;
            corAtual.a = alpha;
            ghostImg.color = corAtual;
            yield return null;
        }
        
        if (ghostObj != null) {
            listaFantasmasAtivos.Remove(ghostObj);
            Destroy(ghostObj);
        }
    }

    private IEnumerator FadeOutBrilho(GameObject brilhoObj, Image brilhoImg) {
        float duration = 0.3f; // Um pouco mais de duração para o brilho durar bem no rastro
        float elapsed = 0f;
        
        while (elapsed < duration) {
            if (brilhoObj == null || brilhoImg == null) yield break;
            
            elapsed += Time.unscaledDeltaTime;
            // Começa forte (0.9) e cai suavemente, mantendo-se mais opaco que o fantasma
            float alpha = Mathf.Lerp(0.9f, 0.1f, elapsed / duration);
            Color corAtual = brilhoImg.color;
            corAtual.a = alpha;
            brilhoImg.color = corAtual;
            yield return null;
        }
        
        if (brilhoObj != null) {
            listaFantasmasAtivos.Remove(brilhoObj);
            Destroy(brilhoObj);
        }
    }

    private void LimparFantasmas() {
        foreach (var ghost in listaFantasmasAtivos) {
            if (ghost != null) Destroy(ghost);
        }
        listaFantasmasAtivos.Clear();
    }

    public void OnDrag(PointerEventData eventData) {
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
        if (corrotinaTremorIcone != null) {
            StopCoroutine(corrotinaTremorIcone);
            corrotinaTremorIcone = null;
        }
        SlotEquipamento[] todosSlotsEquipamento = FindObjectsOfType<SlotEquipamento>();
        foreach (var slotEq in todosSlotsEquipamento) {
            slotEq.DesligarDestaqueVisual();
        }
        if(iconeArrastadoObj != null){
            Destroy(iconeArrastadoObj);
        }
        LimparFantasmas();
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