---
title: Mont Blanc i góry (fr_08) - Script
hide:
---

# Mont Blanc i góry (fr_08) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_08-script.md) - [french](./fr_08-script.fr.md) - polish - [italian](./fr_08-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-08-mont-blanc-mountains/27/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=736863861#gid=736863861)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// Quest: fr_08 | Mont Blanc</span>
<span class="yarn-header-dim">// </span>
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
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: The game is complete! Congratulations!]
[MISSING TRANSLATION: Did you like it?]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Now draw your favourite mountain?]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-talk-tutor"></a>
## talk_tutor

<div class="yarn-node" data-title="talk_tutor"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=mont_blanc</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: You have to go up the Mont Blanc] <span class="yarn-meta">#line:0f4644b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: the highest mountain in Europe] <span class="yarn-meta">#line:07d23cb </span></span>
<span class="yarn-line">[MISSING TRANSLATION: and put the 3 flags correctly] <span class="yarn-meta">#line:07f2699 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-flag-france"></a>
## flag_france

<div class="yarn-node" data-title="flag_france"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_france</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find the French flag.] <span class="yarn-meta">#line:0d23529 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-flag-italy"></a>
## flag_italy

<div class="yarn-node" data-title="flag_italy"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_italy</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find the Italian flag.] <span class="yarn-meta">#line:050fe70 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-flag-swiss"></a>
## flag_swiss

<div class="yarn-node" data-title="flag_swiss"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:  asset=flag_swiss</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find the Swiss flag.] <span class="yarn-meta">#line:03db010 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-tourist"></a>
## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; I love the mountains!]
[MISSING TRANSLATION: =&gt; The Alps are beautiful!]
[MISSING TRANSLATION: =&gt; Mont Blanc is the highest mountain in Europe!]
[MISSING TRANSLATION: =&gt; I want to climb Mont Blanc one day!]
[MISSING TRANSLATION: =&gt; I hope to see a marmot!]
[MISSING TRANSLATION: =&gt; The view from the summit must be amazing!]
[MISSING TRANSLATION: =&gt; Don't forget to bring warm clothes!]
[MISSING TRANSLATION: =&gt; Mountains can be dangerous, be careful!]
[MISSING TRANSLATION: ]
</code></pre></div>


