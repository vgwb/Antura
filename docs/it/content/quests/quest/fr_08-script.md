---
title: Monte Bianco e montagne (fr_08) - Script
hide:
---

# Monte Bianco e montagne (fr_08) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-08-mont-blanc-mountains/27/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=736863861#gid=736863861)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_08%20Mont%20Blanc/FR_08%20Mont%20Blanc%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_08 | Mont Blanc</span>
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
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $COLLECTED_ITEMS = 0&gt;&gt;</span>
[MISSING TRANSLATION: Welcome to the Mont Blanc!]

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai raggiunto il Monte Bianco e hai imparato i fatti sulla vetta!</span> <span class="yarn-meta">#line:0b99e77 </span>
<span class="yarn-line">Hai posizionato tutte e tre le bandiere.</span> <span class="yarn-meta">#line:057afc7 </span>
<span class="yarn-cmd">&lt;&lt;card mountain&gt;&gt;</span>
<span class="yarn-line">Ti sei preparato con indumenti caldi.</span> <span class="yarn-meta">#line:0dda4d7 </span>
<span class="yarn-line">Coraggioso esploratore di montagna!</span> <span class="yarn-meta">#line:04f90c6 </span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Perché non disegni il Monte Bianco con le tre bandiere?</span> <span class="yarn-meta">#line:0232ab7 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:  asset=mont_blanc</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bisogna salire sul Monte Bianco</span> <span class="yarn-meta">#line:0f4644b </span>
<span class="yarn-line">la montagna più alta d'Europa</span> <span class="yarn-meta">#line:07d23cb </span>
<span class="yarn-line">e posiziona correttamente le 3 bandiere</span> <span class="yarn-meta">#line:07f2699 </span>

</code>
</pre>
</div>

<a id="ys-node-item-backpack"></a>

## item_backpack

<div class="yarn-node" data-title="item_backpack">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card backpack&gt;&gt;</span>
<span class="yarn-line">Questo zaino contiene cibo, acqua e una mappa.</span> <span class="yarn-meta">#line:05e80cb </span>
<span class="yarn-line">Permette di avere le mani libere durante le escursioni.</span> <span class="yarn-meta">#line:06d7fcf </span>

</code>
</pre>
</div>

<a id="ys-node-item-coat"></a>

## item_coat

<div class="yarn-node" data-title="item_coat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card coat&gt;&gt;</span>
<span class="yarn-line">Questo cappotto mantiene il corpo caldo anche in caso di vento e neve.</span> <span class="yarn-meta">#line:0e23f1f </span>
<span class="yarn-line">Chiudi sempre la cerniera in alto.</span> <span class="yarn-meta">#line:06fe689 </span>

</code>
</pre>
</div>

<a id="ys-node-item-gloves"></a>

## item_gloves

<div class="yarn-node" data-title="item_gloves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gloves&gt;&gt;</span>
<span class="yarn-line">I guanti mantengono le dita calde e asciutte.</span> <span class="yarn-meta">#line:0f2aac7 </span>
<span class="yarn-line">Le mani fredde rendono difficile arrampicarsi.</span> <span class="yarn-meta">#line:0d01879 </span>

</code>
</pre>
</div>

<a id="ys-node-item-hat"></a>

## item_hat

<div class="yarn-node" data-title="item_hat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card hat&gt;&gt;</span>
<span class="yarn-line">Un cappello caldo impedisce al calore di fuoriuscire dalla testa.</span> <span class="yarn-meta">#line:000fc05 </span>
<span class="yarn-line">Indossalo anche sotto il sole splendente.</span> <span class="yarn-meta">#line:059d744 </span>

</code>
</pre>
</div>

<a id="ys-node-item-rope"></a>

## item_rope

<div class="yarn-node" data-title="item_rope">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">La corda aiuta gli scalatori a rimanere al sicuro sul ghiaccio e sulla roccia.</span> <span class="yarn-meta">#line:038d1bc </span>
<span class="yarn-line">Tagliarlo sempre correttamente.</span> <span class="yarn-meta">#line:0b3d07e </span>

</code>
</pre>
</div>

<a id="ys-node-item-scarf"></a>

## item_scarf

<div class="yarn-node" data-title="item_scarf">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card scarf&gt;&gt;</span>
<span class="yarn-line">Una sciarpa blocca il vento sul collo.</span> <span class="yarn-meta">#line:0afb2dc </span>
<span class="yarn-line">Ripiegalo in modo che non svolazzi.</span> <span class="yarn-meta">#line:07cb76f </span>

</code>
</pre>
</div>

<a id="ys-node-item-sunglasses"></a>

## item_sunglasses

<div class="yarn-node" data-title="item_sunglasses">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card sunglasses&gt;&gt;</span>
<span class="yarn-line">La neve e il ghiaccio riflettono la luce del sole.</span> <span class="yarn-meta">#line:02a2ea9 </span>
<span class="yarn-line">Gli occhiali proteggono i tuoi occhi.</span> <span class="yarn-meta">#line:088fb7f </span>

</code>
</pre>
</div>

<a id="ys-node-flag-france"></a>

## flag_france

<div class="yarn-node" data-title="flag_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera francese.</span> <span class="yarn-meta">#line:0d23529 </span>

</code>
</pre>
</div>

<a id="ys-node-flag-italy"></a>

## flag_italy

<div class="yarn-node" data-title="flag_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>

<span class="yarn-line">Trova la bandiera italiana.</span> <span class="yarn-meta">#line:050fe70 </span>

</code>
</pre>
</div>

<a id="ys-node-flag-swiss"></a>

## flag_swiss

<div class="yarn-node" data-title="flag_swiss">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_swiss&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera svizzera.</span> <span class="yarn-meta">#line:03db010 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Adoro le montagne!</span> <span class="yarn-meta">#line:011dc7e </span>
<span class="yarn-line">Le Alpi sono bellissime!</span> <span class="yarn-meta">#line:0e6a8d1 </span>
<span class="yarn-line">Il Monte Bianco è la montagna più alta d'Europa!</span> <span class="yarn-meta">#line:003ecb2 </span>
<span class="yarn-line">Un giorno voglio scalare il Monte Bianco!</span> <span class="yarn-meta">#line:01c1599 </span>
<span class="yarn-line">Spero di vedere una marmotta!</span> <span class="yarn-meta">#line:04bbada </span>
<span class="yarn-line">La vista dalla cima dev'essere spettacolare!</span> <span class="yarn-meta">#line:031feca </span>
<span class="yarn-line">Non dimenticare di portare vestiti caldi!</span> <span class="yarn-meta">#line:0590129 </span>
<span class="yarn-line">Le montagne possono essere pericolose, fate attenzione!</span> <span class="yarn-meta">#line:097ecf0 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-hiker"></a>

## spawned_hiker

<div class="yarn-node" data-title="spawned_hiker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: hikers</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Per fare escursioni in questo luogo sono necessari degli scarponi adatti.</span> <span class="yarn-meta">#line:0dbac73 </span>
<span class="yarn-cmd">&lt;&lt;card hiking&gt;&gt;</span>
<span class="yarn-line">Il vento può cambiare rapidamente.</span> <span class="yarn-meta">#line:0557ef5 </span>
<span class="yarn-cmd">&lt;&lt;card wind&gt;&gt;</span>
<span class="yarn-line">Quando è ghiacciato seguo la linea della corda.</span> <span class="yarn-meta">#line:0bc67f7 </span>
<span class="yarn-cmd">&lt;&lt;card rope&gt;&gt;</span>
<span class="yarn-line">Quel ghiacciaio sembra un fiume ghiacciato.</span> <span class="yarn-meta">#line:01e5f66 </span>
<span class="yarn-cmd">&lt;&lt;card glacier&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-alps-climber"></a>

## spawned_alps_climber

<div class="yarn-node" data-title="spawned_alps_climber">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: alps_climber</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le Alpi sono il mio parco giochi.</span> <span class="yarn-meta">#line:0809dce </span>
<span class="yarn-cmd">&lt;&lt;card alps&gt;&gt;</span>
<span class="yarn-line">Presto in vetta! Sento già il sole.</span> <span class="yarn-meta">#line:0face28 </span>
<span class="yarn-cmd">&lt;&lt;card summit&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card sun&gt;&gt;</span>
<span class="yarn-line">I ramponi mi aiutano sul ghiaccio duro.</span> <span class="yarn-meta">#line:081fbe4 </span>
<span class="yarn-cmd">&lt;&lt;card crampons&gt;&gt;</span>
<span class="yarn-line">Arrampicare con una guida è più sicuro.</span> <span class="yarn-meta">#line:0371d31 </span>
<span class="yarn-cmd">&lt;&lt;card climbing&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card mountain_guide&gt;&gt;</span>

</code>
</pre>
</div>


