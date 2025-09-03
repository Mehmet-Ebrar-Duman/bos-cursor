mergeInto(LibraryManager.library, {
  WebGL_SendState: function(score, waterCurrent, waterMax, seeds, saplings) {
    try {
      var detail = {
        score: score|0,
        water: { current: waterCurrent, max: waterMax },
        inventory: { seeds: seeds|0, saplings: saplings|0 }
      };
      if (typeof window !== 'undefined') {
        window.dispatchEvent(new CustomEvent('unity-state', { detail: detail }));
        if (window.app && typeof window.app.updateUI === 'function') {
          window.app.updateUI(detail);
        }
      }
    } catch (e) {
      console.error('WebGL_SendState error', e);
    }
  },
  WebGL_ShowToast: function(msgPtr) {
    try {
      var msg = UTF8ToString(msgPtr);
      if (typeof window !== 'undefined') {
        if (window.app && typeof window.app.toast === 'function') {
          window.app.toast(msg);
        } else {
          console.log('[Unity]', msg);
        }
      }
    } catch (e) {
      console.error('WebGL_ShowToast error', e);
    }
  }
});

