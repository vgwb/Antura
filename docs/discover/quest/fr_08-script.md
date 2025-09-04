---
title: Mont Blanc & Mountains (fr_08) - Script
hide:
---

# Mont Blanc & Mountains (fr_08) - Script
[Quest Index](./index.md) - Language: english - [french](./fr_08-script.fr.md) - [polish](./fr_08-script.pl.md) - [italian](./fr_08-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  
    **Improve translations**: [comment here](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// FR_08 MONT_BLANC_MOUNTAINS - Mont Blanc &amp; Mountains  </span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - mont_blanc (geographical landmark)</span>
<span class="yarn-header-dim">// - flag_france (national symbol)</span>
<span class="yarn-header-dim">// - flag_italy (national symbol) </span>
<span class="yarn-header-dim">// - flag_swiss (national symbol)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - Place 3 flags correctly on Mont Blanc summit</span>
<span class="yarn-header-dim">// Activities:</span>
<span class="yarn-header-dim">// - Basic flag placement</span>
<span class="yarn-header-dim">// - Order activity for mountain equipment preparation</span>
<span class="yarn-header-dim">// - Memory game for Alpine animal identification</span>
<span class="yarn-header-dim">// - Quiz on mountain safety and geography</span>
<span class="yarn-header-dim">// Words used:Europe, flags, France, Italy, Switzerland, Alps, altitude, summit, glacier, avalanche, marmot, crampons, rope, weather, safety</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR, asset=mont_blanc</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You have to go up the Mont Blanc</span>
<span class="yarn-line">the highest mountain in Europe <span class="yarn-meta">#line:07d23cb </span></span>
<span class="yarn-line">and put the 3 flags correctly <span class="yarn-meta">#line:07f2699 </span></span>

</code></pre></div>

<a id="ys-node-flag-france"></a>
## flag_france

<div class="yarn-node" data-title="flag_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR, asset=flag_france</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find the French flag. <span class="yarn-meta">#line:0d23529 </span></span>

</code></pre></div>

<a id="ys-node-flag-italy"></a>
## flag_italy

<div class="yarn-node" data-title="flag_italy"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR, asset=flag_italy</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find the Italian flag. <span class="yarn-meta">#line:050fe70 </span></span>

</code></pre></div>

<a id="ys-node-flag-swiss"></a>
## flag_swiss

<div class="yarn-node" data-title="flag_swiss"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actorUID=TUTOR, asset=flag_swiss</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find the Swiss flag. <span class="yarn-meta">#line:03db010 </span></span>

</code></pre></div>


