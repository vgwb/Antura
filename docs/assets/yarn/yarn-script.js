// yarn.script.js: Tokenize and render a Yarn file into HTML with jump support
(function(){
  // Optional globals; can be overridden per-page via data attributes or meta tags
  window.YARN_FILE_URL = window.YARN_FILE_URL || '';
  window.YARN_TITLE = window.YARN_TITLE || '';
  window._YARN_TEXT_CACHE = window._YARN_TEXT_CACHE || null;

  function escapeHtml(s){return s.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');}
  function slug(t){return (t||'').toLowerCase().replace(/[^a-z0-9]+/g,'-').replace(/(^-|-$)/g,'');}

  function buildHTML(script){
    const parts = script.split('===');
    const blocks = [];
    for (let i=0;i<parts.length;i++){
      const raw = parts[i];
      if (!raw || !raw.trim()) continue;
      const m = raw.match(/title:\s*(.+)/);
      const title = m ? m[1].trim() : '';
    const id = title ? `ys-node-${slug(title)}` : '';
  const cm = raw.match(/\n\s*color:\s*([^\n]+)/i);
  const nodeColor = cm ? cm[1].trim() : '';
      const lines = escapeHtml(raw).split('\n');
      let out = [];
      // Insert opening marker dimmed; trim header empty lines until title
      out.push('<span class="yarn-header-dim">===</span>');
      let inHeader = true, seenTitle = false;
      for (let k=0;k<lines.length;k++){
        let line = lines[k];
        const t = line.trim();
        if (inHeader){
          if (!seenTitle && t.length === 0) { continue; }
      if (t === '---') { out.push('<span class="yarn-header-dim">---</span>'); inHeader = false; continue; }
      if (t.startsWith('title:')) { seenTitle = true; continue; }
          // dim other header lines (position:, tags:, color:, blank)
          if (t.length === 0) { out.push(line); continue; }
          out.push(`<span class=\"yarn-header-dim\">${line}</span>`);
        } else {
          // Body: dim commands and #line tags; keep rest normal
          let processed = line
            .replace(/&lt;&lt;[^&]*?&gt;&gt;/g,(m)=>`<span class=\"yarn-cmd\">${m}</span>`)
            .replace(/#line:[^\n]*/g,(m)=>`<span class=\"yarn-meta\">${m}</span>`);
          // Line-start comments dimmed
          if (t.startsWith('//')) processed = `<span class=\"yarn-comment\">${processed}</span>`;
          out.push(processed);
        }
      }
  const preStyle = nodeColor ? ` style=\"background:${nodeColor}\"` : '';
  const heading = title ? `<h2>${escapeHtml(title)}</h2>` : '';
  const dataTitle = title ? ` data-title=\"${escapeHtml(title)}\"` : '';
  const idAttr = id ? ` id=\"${id}\"` : '';
  blocks.push(`<div class=\"yarn-node\"${idAttr}${dataTitle}>${heading}<pre class=\"yarn-code\"${preStyle}><code>${out.join('\n')}</code></pre></div>`);
    }
    return blocks.join('\n');
  }

  function jumpToNodeByTitle(title){
    if (!title) return;
    const targetId = `ys-node-${slug(title)}`;
    const el = document.getElementById(targetId);
    if (el){
      el.scrollIntoView({behavior:'smooth', block:'start'});
      // optional: highlight briefly
      el.classList.add('yarn-jump-highlight');
      setTimeout(()=>el.classList.remove('yarn-jump-highlight'), 900);
    }
  }

  function wireJumpDelegation(container){
    container.addEventListener('click', (ev)=>{
      const t = ev.target;
      if (!(t instanceof Element)) return;
      const cmd = t.closest('.yarn-cmd');
      if (!cmd) return;
      const m = cmd.textContent && cmd.textContent.match(/<<\s*jump\s+([^>]+)\s*>>/i);
      if (!m) return;
      const nodeTitle = m[1].trim();
      jumpToNodeByTitle(nodeTitle);
    });
  }

  function injectSecondaryTOC(container){
    // Collect node wrappers and derive titles/targets
    const nodes = Array.from(container.querySelectorAll('.yarn-node'));
    if (!nodes.length) return;
    const items = nodes.map(node => {
      // Prefer explicit wrapper id; ensure one exists
      let id = node.id;
      let text = node.querySelector('.yarn-header-title')?.textContent || node.getAttribute('data-title') || node.querySelector('h2')?.textContent || id;
      if (!id && text){ id = `ys-node-${slug(text)}`; node.id = id; }
      if (!id) return '';
      const label = text || id;
      return `<li class=\"md-nav__item\"><a href=\"#${id}\" class=\"md-nav__link\"><span class=\"md-ellipsis\">${escapeHtml(label)}</span></a></li>`;
    }).filter(Boolean).join('');
    if (!items) return;
    const ul = `<ul class=\"md-nav__list\" data-md-component=\"toc\" data-md-scrollfix>${items}</ul>`;

    function placeTOC(){
      const nav = document.querySelector('nav.md-nav.md-nav--secondary');
      if (nav){ nav.innerHTML = ul; return true; }
      const sideInner = document.querySelector('.md-sidebar--secondary .md-sidebar__inner');
      if (sideInner){
        let wrapper = sideInner.querySelector('nav.md-nav.md-nav--secondary');
        if (!wrapper){
          wrapper = document.createElement('nav');
          wrapper.className = 'md-nav md-nav--secondary';
          sideInner.appendChild(wrapper);
        }
        wrapper.innerHTML = ul;
        return true;
      }
      return false;
    }

    if (!placeTOC()){
      // Wait for the sidebar/nav to be created by the theme
      const obs = new MutationObserver(()=>{ if (placeTOC()) obs.disconnect(); });
      obs.observe(document.body, { childList: true, subtree: true });
      // Safety timeout
      setTimeout(()=>obs.disconnect(), 5000);
    }
  }

  document.addEventListener('DOMContentLoaded', async function(){
    const container = document.getElementById('yarn-script');
    if (!container) return; // Only run on pages that have a yarn script container
    // Page-level config: prefer data attributes, then meta tags, then globals, then fallback
    const metaUrl = document.querySelector('meta[name="yarn:file"]')?.content;
    const metaTitle = document.querySelector('meta[name="yarn:title"]')?.content;
    const url = container.getAttribute('data-url') || metaUrl || window.YARN_FILE_URL || '/assets/quests/sample.yarn';
    const title = container.getAttribute('data-title') || metaTitle || window.YARN_TITLE || 'Script';
    window.YARN_TITLE = title; // keep for popup title
    const btnOpen = document.getElementById('open-yarn-graph');

    try{
      const res = await fetch(url);
      const txt = await res.text();
      window._YARN_TEXT_CACHE = txt;
  container.innerHTML = buildHTML(txt);
      wireJumpDelegation(container);
  // Inject TOC; if sidebar not ready, observer will retry
  injectSecondaryTOC(container);
    }catch(e){
      container.innerHTML = '<p>Failed to load yarn file.</p>';
    }

    // Popup window with only the graph
    btnOpen?.addEventListener('click', () => {
      const w = window.open('', 'yarn-graph', 'width=1200,height=800');
      if (!w) return;
  const titleTxt = (window.YARN_TITLE || 'Graph') + ' — Graph';
  const html = `<!doctype html><html><head><meta charset=\"utf-8\"><title>${titleTxt}</title>
        <style>html,body{height:100%;margin:0} #graph{width:100%;height:100%;}</style>
      </head><body>
        <div id=\"graph\"></div>
        <script>(function(){
          var Renderer = window.opener && window.opener.YarnGraphRenderer;
          var text = (window.opener && window.opener._YARN_TEXT_CACHE) || '';
          if (!Renderer) { document.body.innerHTML = '<p style=\\'padding:16px;font-family:system-ui,sans-serif\\'>Graph renderer not available.</p>'; return; }
          var r = new Renderer('graph', { height: '100%' });
          r.renderGraph(text);
        })();<\/script>
      </body></html>`;
      w.document.open();
      w.document.write(html);
      w.document.close();
    });
  });
})();
