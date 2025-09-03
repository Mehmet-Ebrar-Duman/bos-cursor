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
    btnAddScore: document.getElementById('btn-addscore'),
    nameInput: document.getElementById('player-name'),
    btnSubmit: document.getElementById('btn-submit'),
    leaderboard: document.getElementById('leaderboard')
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

// Simple backend integration
(function(){
  const API = `${location.origin}`;
  async function loadLeaderboard(){
    try {
      const res = await fetch(`${API}/api/leaderboard`);
      const list = await res.json();
      const box = document.getElementById('leaderboard');
      if (!box) return;
      box.innerHTML = '';
      list.slice(0, 20).forEach((e, i) => {
        const li = document.createElement('li');
        li.textContent = `${i+1}. ${e.name} — ${e.score}`;
        box.appendChild(li);
      });
    } catch(e){ console.warn('loadLeaderboard', e); }
  }

  async function submitScore(){
    const name = document.getElementById('player-name')?.value || 'Anon';
    try {
      await fetch(`${API}/api/leaderboard`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, score: Number(document.getElementById('score')?.textContent || 0) })
      });
      await loadLeaderboard();
      window.app.toast('Skor kaydedildi');
    } catch(e){ console.warn('submitScore', e); }
  }

  document.getElementById('btn-submit')?.addEventListener('click', submitScore);
  loadLeaderboard();
})();

