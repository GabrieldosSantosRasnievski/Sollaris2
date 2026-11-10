using UnityEngine;
using System.Collections.Generic;

public class ColocarObjeto : MonoBehaviour {
    [System.Serializable]
    public struct AssociacaoItemPrefab {
        public string nomeItem;
        public GameObject prefabConstrucao;
    }

    public List<AssociacaoItemPrefab> itensConstruiveis;
    private GameObject objetoFantasma;
    private SpriteRenderer[] spritesFantasma;
    public LayerMask camadasBloqueio;
    public Color corValida = new Color(0f, 1f, 0f, 0.6f);
    public Color corInvalida = new Color(1f, 0f, 0f, 0.6f);

    // Propriedade estática para avisar o sistema de combate que estamos a construir
    public static bool ConstrucaoAtiva { get; private set; } = false;

    private void Update() {
        GameObject prefabAtivo = ObterPrefabSlotSelecionado();

        if (prefabAtivo != null) {
            ConstrucaoAtiva = true;
            GerenciarFantasma(prefabAtivo);
            AtualizarPosicaoEEstadoFantasma();

            if (Input.GetMouseButtonDown(0)) {
                TentarConstruir(prefabAtivo);
            }
        } 
        else {
            ConstrucaoAtiva = false;
            DestruirFantasma();
        }
    }

    private GameObject ObterPrefabSlotSelecionado() {
        if (InventarioJogador.Instance == null) {
            return null;
        }

        int slotAtivo = InventarioJogador.Instance.slotSelecionado;
        if (slotAtivo < 0 || slotAtivo >= InventarioJogador.Instance.slotsRapidos.Count) {
            return null;
        }

        var slot = InventarioJogador.Instance.slotsRapidos[slotAtivo];
        if (string.IsNullOrEmpty(slot.nomeItem) || slot.quantidadeItem <= 0) {
            return null;
        }

        foreach (var associacao in itensConstruiveis) {
            if (associacao.nomeItem == slot.nomeItem) {
                return associacao.prefabConstrucao;
            }
        }

        return null;
    }

    private void GerenciarFantasma(GameObject prefabAlvo) {
        if (objetoFantasma == null) {
            objetoFantasma = Instantiate(prefabAlvo);
            
            Collider2D[] colisores = objetoFantasma.GetComponentsInChildren<Collider2D>();
            foreach (Collider2D colisor in colisores) {
                colisor.enabled = false;
            }

            MonoBehaviour[] scripts = objetoFantasma.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts) {
                if (script != this) {
                    script.enabled = false;
                }
            }

            spritesFantasma = objetoFantasma.GetComponentsInChildren<SpriteRenderer>();
        }
        
        if (!objetoFantasma.activeSelf) {
            objetoFantasma.SetActive(true);
        }
    }

    private void AtualizarPosicaoEEstadoFantasma() {
        if (objetoFantasma != null) {
            Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicaoMouse.z = 0f;
            objetoFantasma.transform.position = posicaoMouse;

            bool localValido = VerificarLocalizacaoValida(posicaoMouse);

            Color corAlvo = localValido ? corValida : corInvalida;
            foreach (SpriteRenderer sr in spritesFantasma) {
                if (sr != null) {
                    sr.color = corAlvo;
                }
            }
        }
    }

    private bool VerificarLocalizacaoValida(Vector2 posicao) {
        Collider2D[] collidersAtingidos = Physics2D.OverlapCircleAll(posicao, 0.4f, camadasBloqueio);

        foreach (Collider2D col in collidersAtingidos) {
            if (!col.isTrigger) {
                return false;
            }
        }

        return true;
    }

    private void TentarConstruir(GameObject prefabParaConstruir) {
        Vector3 posicaoConstrucao = objetoFantasma.transform.position;

        if (VerificarLocalizacaoValida(posicaoConstrucao)) {
            Instantiate(prefabParaConstruir, posicaoConstrucao, Quaternion.identity);

            var inv = InventarioJogador.Instance;
            int slotAtivo = inv.slotSelecionado;
            inv.slotsRapidos[slotAtivo].quantidadeItem -= 1;

            if (inv.slotsRapidos[slotAtivo].quantidadeItem <= 0) {
                inv.slotsRapidos[slotAtivo].nomeItem = "";
                inv.slotsRapidos[slotAtivo].iconeItem = null;
            }

            Debug.Log("Objeto posicionado e consumido do inventário com sucesso!");
        } 
        else {
            Debug.Log("Não é possível construir aqui! O local está ocupado por um obstáculo sólido.");
        }
    }

    private void DestruirFantasma() {
        if (objetoFantasma != null) {
            Destroy(objetoFantasma);
            objetoFantasma = null;
            spritesFantasma = null;
        }
    }
}