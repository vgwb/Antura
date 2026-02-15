---
title: Fiume Odra (pl_03) - Script
hide:
---

# Fiume Odra (pl_03) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-03-a-voyage-on-the-odra-river/34/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=106202032#gid=106202032)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_03%20Wroclaw%20River/PL_03%20Wroclaw%20River%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_03 | Odra River (Wroclaw)</span>
<span class="yarn-header-dim">// PL_03 - A Voyage on the Odra River</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 8&gt;&gt;</span>
<span class="yarn-comment">// State variables for the 8 chests</span>
<span class="yarn-cmd">&lt;&lt;declare $map_odra = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $river_sign = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_tumski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_redzinski = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $bridge_train = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_house = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_tourist = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $boat_barge = 0&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;card place_odra_river&gt;&gt;</span>
<span class="yarn-line">Ci troviamo a Breslavia, la "Città dei cento ponti".</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-line">Oggi esploreremo il fiume, i ponti e le barche.</span> <span class="yarn-meta">#line:start_2</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ben fatto!</span> <span class="yarn-meta">#line:02a257c </span>
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">Abbiamo imparato a conoscere i diversi tipi di ponti.</span> <span class="yarn-meta">#line:end_1</span>
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">Abbiamo imparato a conoscere i diversi tipi di imbarcazioni.</span> <span class="yarn-meta">#line:end_2</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">Abbiamo esplorato il fiume ODRA.</span> <span class="yarn-meta">#line:end_3</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Disegna una semplice MAPPA del tuo viaggio.</span> <span class="yarn-meta">#line:0265d07 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-protagonist"></a>

## protagonist

<div class="yarn-node" data-title="protagonist">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("find_photos")&gt;&gt;</span>
<span class="yarn-line">    Dziękuję! Hai trovato tutte le mie foto!</span> <span class="yarn-meta">#line:prot_1</span>
<span class="yarn-line">    Ora posso continuare la visita. Un'ultima domanda...</span> <span class="yarn-meta">#line:prot_2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    Sei coraggioso! Ma mi mancano ancora le foto delle meraviglie del nostro fiume.</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Cercate i forzieri! La gente lungo il fiume li sorveglia.</span> <span class="yarn-meta">#line:prot_4 </span>
<span class="yarn-line">    Se non riesci a trovarli, usa la mappa!</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    Stai andando bene!</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Aiuto! Antura pensava che la mia guida di Breslavia fosse un osso!</span> <span class="yarn-meta">#line:prot_5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    Le pagine sono sparse in giro. Riesci a ritrovarle?</span> <span class="yarn-meta">#line:prot_6 #task:collect_cards</span>
    <span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;SetActive antura false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_tutorial&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start collect_cards task_collect_cards_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-photos-done"></a>

## task_find_photos_done

<div class="yarn-node" data-title="task_find_photos_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutte le foto!</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Torna indietro e parla con la guida.</span> <span class="yarn-meta">#line:go_back #task:go_back </span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-collect-cards-done"></a>

## task_collect_cards_done

<div class="yarn-node" data-title="task_collect_cards_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hai trovato tutte le foto!</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Torna indietro e parla con la guida.</span> <span class="yarn-meta">#shadow:go_back #task:go_back</span>
<span class="yarn-cmd">&lt;&lt;target protagonist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_back&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## final_quiz

<div class="yarn-node" data-title="final_quiz">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'Odra è un fiume lungo. Ti ricordi dove finisce tutta quell'acqua?</span> <span class="yarn-meta">#line:quiz1_intro</span>
<span class="yarn-line">Il Mar Baltico</span> <span class="yarn-meta">#line:quiz_a1</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    Tak! Esatto! L'Odra scorre verso nord attraverso la Polonia e sfocia nel Mar Baltico.</span> <span class="yarn-meta">#line:quiz1_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>
<span class="yarn-line">Il Mar Mediterraneo</span> <span class="yarn-meta">#line:quiz_a2</span>
<span class="yarn-line">    Hmm... è troppo a sud!</span> <span class="yarn-meta">#line:quiz1_fail1</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">Il Mar Nero</span> <span class="yarn-meta">#line:quiz_a3</span>
<span class="yarn-line">    Non proprio. L'Odra si dirige verso nord, non verso sud!</span> <span class="yarn-meta">#line:quiz1_fail2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#line:dontknow</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span> 
<span class="yarn-line">    Nessun problema! Guarda la linea blu sulla mappa.</span> <span class="yarn-meta">#line:quiz1_hint</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz-2"></a>

## final_quiz_2

<div class="yarn-node" data-title="final_quiz_2">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'Odra è il fiume *più lungo* della Polonia?</span> <span class="yarn-meta">#line:quiz2_intro</span>
<span class="yarn-line">No, la Vistola è più lunga</span> <span class="yarn-meta">#line:quiz2_a1</span>
    <span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">    Perfetto! La Vistola è la numero uno, l'Odra è la seconda più lunga.</span> <span class="yarn-meta">#line:quiz2_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-line">Sì, è il più lungo</span> <span class="yarn-meta">#line:quiz2_a2</span>
<span class="yarn-line">    È molto grande, ma c'è un fiume che è ancora più lungo.</span> <span class="yarn-meta">#line:quiz2_fail</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-river-sign"></a>

## npc_river_sign

<div class="yarn-node" data-title="npc_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- RIVER SIGN</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $river_sign &lt; 10&gt;&gt;
<span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">Guarda il grande cartello blu vicino al ponte.</span> <span class="yarn-meta">#line:sign_1</span>
<span class="yarn-line">Cosa ci dicono le linee bianche ondulate?</span> <span class="yarn-meta">#line:sign_3</span>
<span class="yarn-line">C'è un fiume che scorre qui</span> <span class="yarn-meta">#line:sign_4</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_river_sign&gt;&gt;</span>
<span class="yarn-line">    Tak! Quelle onde blu sono il segno universale di un fiume.</span> <span class="yarn-meta">#line:sign_5</span>
<span class="yarn-line">Il ponte si muove</span> <span class="yarn-meta">#line:sign_6</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#line:try_again </span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#line:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-river-sign"></a>

## chest_river_sign

<div class="yarn-node" data-title="chest_river_sign">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $river_sign == 1&gt;&gt;</span>
<span class="yarn-line">    Antura ha coperto il cartello di fango! Puliscilo per vedere le onde.</span> <span class="yarn-meta">#line:ch_sign1</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity clean_river_sign chest_river_sign&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_sign&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">    Ottimo lavoro! Ora saprai sempre quando stai attraversando un fiume in Europa.</span> <span class="yarn-meta">#line:ch_sign2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#line:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-odra-map"></a>

## npc_odra_map

<div class="yarn-node" data-title="npc_odra_map">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">// ---------- ODRA RIVER MAP</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">L'Odra è il secondo fiume più lungo della Polonia.</span> <span class="yarn-meta">#line:map_1</span>
&lt;&lt;if $map_odra &lt; 10&gt;&gt;
<span class="yarn-line">L'Odra sfocia nelle montagne o nel mare?</span> <span class="yarn-meta">#line:map_2</span>
<span class="yarn-line">Il Mar Baltico</span> <span class="yarn-meta">#line:map_3</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_odra_map&gt;&gt;</span>
<span class="yarn-line">    Esatto! Scorre fino a nord.</span> <span class="yarn-meta">#line:map_4</span>
<span class="yarn-line">Il Mar Mediterraneo</span> <span class="yarn-meta">#line:map_5</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_odra_map&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-odra-map"></a>

## chest_odra_map

<div class="yarn-node" data-title="chest_odra_map">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $map_odra == 1&gt;&gt;</span>
<span class="yarn-line">    Dimostra di sapere dove scorre il fiume!</span> <span class="yarn-meta">#line:ch_map1</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_odra_facts chest_odra_river_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $map_odra == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_map&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    La mappa è tornata! Mostra l'Oder che sfocia nel Mar Baltico.</span> <span class="yarn-meta">#line:ch_map2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-tumski-bridge"></a>

## npc_tumski_bridge

<div class="yarn-node" data-title="npc_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TUMSKI BRIDGE (The Romantic Bridge)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Questo è il ponte Tumski. Collega la parte più antica della città.</span> <span class="yarn-meta">#line:tum_1</span>
<span class="yarn-line">Ogni sera, un uomo accende a mano le 102 lanterne a gas!</span> <span class="yarn-meta">#line:tum_2</span>
&lt;&lt;if $bridge_tumski &lt; 10&gt;&gt;
<span class="yarn-line">Cosa appendono le coppie a questo ponte per simboleggiare fortuna e amore?</span> <span class="yarn-meta">#line:tum_3</span>
<span class="yarn-line">Lucchetti</span> <span class="yarn-meta">#line:tum_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Sì! Anche se sono molto pesanti e verranno rimossi!</span> <span class="yarn-meta">#line:tum_5</span>
<span class="yarn-line">Calzini bagnati</span> <span class="yarn-meta">#line:tum_6</span>
<span class="yarn-line">    Non sarebbe molto romantico! Riprova.</span> <span class="yarn-meta">#line:fail_tum</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-tumski-bridge"></a>

## chest_tumski_bridge

<div class="yarn-node" data-title="chest_tumski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_tumski == 1&gt;&gt;</span>
<span class="yarn-line">    Pulisci la ruggine da questo vecchio ponte di ferro!</span> <span class="yarn-meta">#line:ch_tum1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tumski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-redzinski-bridge"></a>

## npc_redzinski_bridge

<div class="yarn-node" data-title="npc_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- RĘDZIŃSKI BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Il ponte Rędziński è il più alto e il più lungo della Polonia.</span> <span class="yarn-meta">#line:redz_1</span>
<span class="yarn-line">È alta 122 metri. Più alta della Cattedrale!</span> <span class="yarn-meta">#line:redz_2</span>
&lt;&lt;if $bridge_redzinski &lt; 10&gt;&gt;
<span class="yarn-line">Cosa sostiene questo enorme ponte?</span> <span class="yarn-meta">#line:redz_3</span>
<span class="yarn-line">cavi d'acciaio</span> <span class="yarn-meta">#line:redz_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Esatto! Questo ponte gigante è sorretto da corde robuste. Le auto lo usano per spostarsi in città.</span> <span class="yarn-meta">#line:redz_5</span>
<span class="yarn-line">Magneti e magia</span> <span class="yarn-meta">#line:redz_6</span>
<span class="yarn-line">    Sembra magia, ma in realtà è ingegneria!</span> <span class="yarn-meta">#line:fail_redz</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-redzinski-bridge"></a>

## chest_redzinski_bridge

<div class="yarn-node" data-title="chest_redzinski_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_redzinski == 1&gt;&gt;</span>
<span class="yarn-line">    Ricostruiamo il pilone più alto di Breslavia!</span> <span class="yarn-meta">#line:ch_redz1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_redzinski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-train-bridge"></a>

## npc_train_bridge

<div class="yarn-node" data-title="npc_train_bridge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- TRAIN BRIDGE</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">Da oltre 150 anni i treni attraversano l'Oder a Breslavia.</span> <span class="yarn-meta">#line:train_1</span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">Perché i ponti ferroviari sono fatti di acciaio così pesante?</span> <span class="yarn-meta">#line:train_2</span>
<span class="yarn-line">Perché i treni sono molto pesanti</span> <span class="yarn-meta">#line:train_3</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Sì! Deve essere abbastanza resistente per i treni pesanti.</span> <span class="yarn-meta">#line:train_4</span>
<span class="yarn-line">Per fare un rumore forte</span> <span class="yarn-meta">#line:train_5</span>
<span class="yarn-line">    Sono rumorosi, ma non è per questo!</span> <span class="yarn-meta">#line:fail_train</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_train_bridge&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-bridge-train"></a>

## chest_bridge_train

<div class="yarn-node" data-title="chest_bridge_train">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $bridge_train == 1&gt;&gt;</span>
<span class="yarn-line">    Abbina le scatole pesanti ai binari del treno!</span> <span class="yarn-meta">#line:ch_train1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-houseboat"></a>

## npc_houseboat

<div class="yarn-node" data-title="npc_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- HOUSEBOAT</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">A Breslavia, alcuni chiamano il fiume la loro "strada di casa".</span> <span class="yarn-meta">#line:house_1</span>
<span class="yarn-line">Anche i nani adorerebbero una casa galleggiante!</span> <span class="yarn-meta">#line:house_2</span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">Se vivi su una casa galleggiante, cosa usi come giardino?</span> <span class="yarn-meta">#line:house_3</span>
<span class="yarn-line">Il fiume Odra</span> <span class="yarn-meta">#line:house_4</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_houseboat&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
<span class="yarn-line">Una foresta sui tetti</span> <span class="yarn-meta">#line:house_6</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_houseboat&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-houseboat"></a>

## chest_houseboat

<div class="yarn-node" data-title="chest_houseboat">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_house == 1&gt;&gt;</span>
<span class="yarn-line">    Riparate le finestre della casa galleggiante!</span> <span class="yarn-meta">#line:ch_house1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_houseboat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_houseboat&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">    Una casa accogliente sull'Odra! Foto raccolta.</span> <span class="yarn-meta">#line:ch_house2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-people"></a>

## npc_boat_people

<div class="yarn-node" data-title="npc_boat_people">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BOAT PEOPLE (Tourist Boats)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">Le imbarcazioni turistiche portano i turisti a visitare lo zoo e la cattedrale.</span> <span class="yarn-meta">#line:tour_1</span>
&lt;&lt;if $boat_tourist &lt; 10&gt;&gt;
<span class="yarn-line">Cosa usano le persone su queste barche per vedere i luoghi d'interesse?</span> <span class="yarn-meta">#line:tour_2</span>
<span class="yarn-line">I loro occhi e le loro telecamere</span> <span class="yarn-meta">#line:tour_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Sì! Sorridi per la foto!</span> <span class="yarn-meta">#line:tour_4</span>
<span class="yarn-line">Un periscopio</span> <span class="yarn-meta">#line:tour_5</span>
<span class="yarn-line">    Non siamo ancora sott'acqua! Riprova.</span> <span class="yarn-meta">#line:fail_tour</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-people"></a>

## chest_boat_people

<div class="yarn-node" data-title="chest_boat_people">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_tourist == 1&gt;&gt;</span>
<span class="yarn-line">    Trova i turisti nascosti sul ponte!</span> <span class="yarn-meta">#line:ch_tour1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tourist&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-boat-barge"></a>

## npc_boat_barge

<div class="yarn-node" data-title="npc_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">// ---------- BARGE (Cargo)</span>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">Per secoli le chiatte (Barki) hanno trasportato carbone e sabbia sull'Odra.</span> <span class="yarn-meta">#line:barge_1</span>
&lt;&lt;if $boat_barge &lt; 10&gt;&gt;
<span class="yarn-line">Una chiatta è molto piatta. Perché?</span> <span class="yarn-meta">#line:barge_2</span>
<span class="yarn-line">Per trasportare oggetti pesanti anche quando l'acqua non è profonda.</span> <span class="yarn-meta">#line:barge_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_barge&gt;&gt;</span>
<span class="yarn-line">    Esatto! È un camion che galleggia.</span> <span class="yarn-meta">#line:barge_4</span>
<span class="yarn-line">Così può nascondersi dai nani</span> <span class="yarn-meta">#line:barge_5</span>
<span class="yarn-line">    No. Riprova.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_barge&gt;&gt;</span>
<span class="yarn-line">Non lo so</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_tourist&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-boat-barge"></a>

## chest_boat_barge

<div class="yarn-node" data-title="chest_boat_barge">
<pre class="yarn-code" style="--node-color:actor:"><code>
<span class="yarn-header-dim">color: </span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $boat_barge == 1&gt;&gt;</span>
<span class="yarn-line">    Gioca a un minigioco per aprire lo scrigno!</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_barge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_barge&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">    Si apre il baule. Trovi una foto!</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Il baule è vuoto.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Il baule è chiuso a chiave.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kayak"></a>

## spawned_kayak

<div class="yarn-node" data-title="spawned_kayak">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Voglio guidare un piccolo KAYAK.</span> <span class="yarn-meta">#line:0f36b7f #card:kayak</span>
<span class="yarn-line">I kayak sono fantastici per esplorare la natura!</span> <span class="yarn-meta">#line:kayak_2 #card:kayak</span>
<span class="yarn-line">Pagaiare è divertente e un buon esercizio!</span> <span class="yarn-meta">#line:kayak_3</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Spawned</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ci sono così tanti PONTI in questa città.</span> <span class="yarn-meta">#line:0577d80 </span>
<span class="yarn-line">Breslavia è davvero bellissima.</span> <span class="yarn-meta">#line:089ea37 </span>
<span class="yarn-line">Adoro i pierogi!</span> <span class="yarn-meta">#line:07ff8c5 </span>
<span class="yarn-line">L'isola della Cattedrale è magica di notte.</span> <span class="yarn-meta">#line:tourist_4</span>

</code>
</pre>
</div>


