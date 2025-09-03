window.app = (function(){
  const els = {
    score: document.getElementById('score'),
    water: document.getElementById('water'),
    inv: document.getElementById('inv'),
    toast: document.getElementById('toast'),
    btnPlant: document.getElementById('btn-plant'),
    btnSeed: document.getElementById('btn-buy-seed'),
    btnSapling: document.getElementById('btn-buy-sapling'),
    btnRefill: document.getElementById('btn-refill'),
    btnAddScore: document.getElementById('btn-addscore')
  };

  function updateUI(detail) {
    if (!detail) return;
    if (els.score) els.score.textContent = String(detail.score ?? 0);
    if (els.water) {
      const c = Math.round(detail.water?.current ?? 0);
      const m = Math.round(detail.water?.max ?? 0);
      els.water.textContent = `${c}/${m}`;
    }
    if (els.inv) {
      const s = detail.inventory?.seeds ?? 0;
      const sp = detail.inventory?.saplings ?? 0;
      els.inv.textContent = `Tohum ${s} | Fidan ${sp}`;
    }
  }

  function toast(message) {
    if (!els.toast) return;
    els.toast.textContent = message;
    els.toast.hidden = false;
    els.toast.classList.add('show');
    setTimeout(() => {
      els.toast.classList.remove('show');
      els.toast.hidden = true;
    }, 1800);
  }

  // Unity -> Web events
  window.addEventListener('unity-state', (ev) => {
    updateUI(ev.detail);
  });

  // Web -> Unity calls (through SendMessage)
  function sendMessage(obj, method, arg) {
    try {
      if (typeof unityInstance !== 'undefined' && unityInstance?.SendMessage) {
        unityInstance.SendMessage(obj, method, arg ?? '');
      } else {
        console.log('[SIM] SendMessage', obj, method, arg);
      }
    } catch(e) {
      console.error('SendMessage error', e);
    }
  }

  // Button bindings
  els.btnPlant?.addEventListener('click', () => sendMessage('WebBridge', 'JS_TogglePlantMode', ''));
  els.btnSeed?.addEventListener('click', () => sendMessage('WebBridge', 'JS_BuySeed', ''));
  els.btnSapling?.addEventListener('click', () => sendMessage('WebBridge', 'JS_BuySapling', ''));
  els.btnRefill?.addEventListener('click', () => sendMessage('WebBridge', 'JS_RefillWater', '100'));
  els.btnAddScore?.addEventListener('click', () => sendMessage('WebBridge', 'JS_AddScore', '10'));

  return { updateUI, toast };
})();

