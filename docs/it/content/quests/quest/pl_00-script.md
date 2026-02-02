---
title: I vicini della Polonia (pl_00) - Script
hide:
---

# I vicini della Polonia (pl_00) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-00-the-neighbors-of-poland/31/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1929643794#gid=1929643794)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_00%20Geo%20Poland/PL_00%20Geo%20Poland%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_00 | Poland GEO</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $COUNTRIES_COMPLETED = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $TOTAL_COUNTRIES = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $poland_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belarus_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $ukraine_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $slovakia_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lithuania_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $czech_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $russia_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $russia_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card country_poland&gt;&gt;</span>
<span class="yarn-line">Benvenuti in Polonia!</span> <span class="yarn-meta">#line:046db1f</span>
<span class="yarn-line">Siamo in Europa</span> <span class="yarn-meta">#line:002aafd</span>
<span class="yarn-line">Incontriamo amici provenienti dai paesi vicini.</span> <span class="yarn-meta">#line:0862117</span>
<span class="yarn-cmd">&lt;&lt;target npc_poland&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Questa missione è completata.</span> <span class="yarn-meta">#line:085bc39</span>
<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-post-quest-activity"></a>

## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Perché non disegni la tua bandiera adesso?</span> <span class="yarn-meta">#line:01f830b </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-win"></a>

## win

<div class="yarn-node" data-title="win">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_end talk_npc_poland_final&gt;&gt;</span>
<span class="yarn-line">Ottimo lavoro! Ce l'hai fatta!</span> <span class="yarn-meta">#line:0ba3c4c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">Hai scoperto una parte dell'Europa centrale!</span> <span class="yarn-meta">#line:031f72c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-poland-npc"></a>

## poland_npc

<div class="yarn-node" data-title="poland_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// poland</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COUNTRIES_COMPLETED &gt;= $TOTAL_COUNTRIES&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $poland_completed == true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_warsaw&gt;&gt;</span>
<span class="yarn-line">    Varsavia è la capitale della Polonia!</span> <span class="yarn-meta">#line:09d7771 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_poland")&gt;&gt;</span>
<span class="yarn-line">    Sì, quella è la mia bandiera! Grazie!</span> <span class="yarn-meta">#line:0c9fa89</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_poland&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_europe&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_germany&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_germany&gt;&gt;</span>
<span class="yarn-line">    Riesci a trovare la bandiera tedesca?</span> <span class="yarn-meta">#line:04bd4db #task:talk_npc_germany</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Quella non è la mia bandiera. La mia è bianca e rossa.</span> <span class="yarn-meta">#line:0f967da </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
<span class="yarn-line">    Ciao! Vengo dalla Polonia!</span> <span class="yarn-meta">#line:017ad80</span>
<span class="yarn-line">    Antura ha fatto un pasticcio e tutte le bandiere sono state mischiate!</span> <span class="yarn-meta">#line:0b9e31d</span>
<span class="yarn-line">    Mi potete aiutare?</span> <span class="yarn-meta">#line:08a2f6d</span>
    <span class="yarn-cmd">&lt;&lt;jump task_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-poland"></a>

## task_poland

<div class="yarn-node" data-title="task_poland">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera polacca.</span> <span class="yarn-meta">#line:09e3b54 #task:find_flag_poland</span>
<span class="yarn-cmd">&lt;&lt;target flag_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_poland task_poland_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-poland-done"></a>

## task_poland_done

<div class="yarn-node" data-title="task_poland_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_poland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_poland&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera polacca! Torna all'inizio.</span> <span class="yarn-meta">#line:0891273 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-poland"></a>

## item_flag_poland

<div class="yarn-node" data-title="item_flag_poland">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: poland</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_poland"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera polacca! Torna all'inizio.</span> <span class="yarn-meta">#shadow:0891273 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_poland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Polonia.</span> <span class="yarn-meta">#line:07ca581</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-germany-npc"></a>

## germany_npc

<div class="yarn-node" data-title="germany_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// GERMANY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $germany_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_berlin&gt;&gt;</span>
<span class="yarn-line">    Berlino è la capitale della Germania.</span> <span class="yarn-meta">#line:0446f03 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_germany")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Quella è la mia bandiera!</span> <span class="yarn-meta">#line:0ba8707</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_germany&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action belarus_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Belarus&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_belarus&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico bielorusso?</span> <span class="yarn-meta">#line:06c463a #task:talk_npc_belarus</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_germany&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono dalla Germania!</span> <span class="yarn-meta">#line:0068fe1 </span>
<span class="yarn-line">    Siamo famosi per i castelli, le foreste e i treni!</span> <span class="yarn-meta">#line:04dc97a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La nostra bandiera ha strisce orizzontali nere, rosse e gialle.</span> <span class="yarn-meta">#line:0cd7024 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-germany"></a>

## task_germany

<div class="yarn-node" data-title="task_germany">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera tedesca. È nera, rossa e gialla.</span> <span class="yarn-meta">#line:029ee72 #task:find_flag_germany</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_germany task_germany_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-germany-done"></a>

## task_germany_done

<div class="yarn-node" data-title="task_germany_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Germany&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera tedesca! Torna in Germania.</span> <span class="yarn-meta">#shadow:05f0ed2</span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-germany"></a>

## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_germany"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera tedesca! Torna in Germania.</span> <span class="yarn-meta">#line:05f0ed2</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Germania.</span> <span class="yarn-meta">#line:05ff51a</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-belarus-npc"></a>

## belarus_npc

<div class="yarn-node" data-title="belarus_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// belarus</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belarus_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_minsk&gt;&gt;</span>
<span class="yarn-line">    Minsk è la capitale della Bielorussia!</span> <span class="yarn-meta">#line:0aecb59</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_belarus")&gt;&gt;</span> 
<span class="yarn-line">    Quella è la mia bandiera!</span> <span class="yarn-meta">#line:0c57e40 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_belarus&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span> 
    <span class="yarn-cmd">&lt;&lt;action czech_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_czech&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_czech&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico ceco?</span> <span class="yarn-meta">#line:021e1a2 #task:talk_npc_czech</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_czech&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $belarus_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_belarus&gt;&gt;</span>
<span class="yarn-line">    Ciao! Vengo dalla Bielorussia!</span> <span class="yarn-meta">#line:0ccf58d </span>
<span class="yarn-line">    Abbiamo una foresta primordiale, che cresce da secoli!</span> <span class="yarn-meta">#line:0c6ac62 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La mia bandiera è rossa e verde con un motivo rosso sulla sinistra.</span> <span class="yarn-meta">#line:0653fae</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-belarus"></a>

## task_belarus

<div class="yarn-node" data-title="task_belarus">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera bielorussa e riportala indietro.</span> <span class="yarn-meta">#line:0c00afe #task:find_flag_belarus</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_belarus task_belarus_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-belarus-done"></a>

## task_belarus_done

<div class="yarn-node" data-title="task_belarus_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_belarus add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Belarus&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera bielorussa! Torna alla Bielorussia.</span> <span class="yarn-meta">#shadow:0157c15 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-belarus"></a>

## item_flag_belarus

<div class="yarn-node" data-title="item_flag_belarus">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_belarus"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera bielorussa! Torna alla Bielorussia.</span> <span class="yarn-meta">#line:0157c15 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Bielorussia</span> <span class="yarn-meta">#line:006ce10</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-czech-republic-npc"></a>

## czech_republic_npc

<div class="yarn-node" data-title="czech_republic_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// czech_republic</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $czech_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_prague&gt;&gt;</span>
<span class="yarn-line">    Grazie! La nostra capitale è Minsk!</span> <span class="yarn-meta">#line:08473de </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_czech_republic")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Quella è la mia bandiera!</span> <span class="yarn-meta">#line:07ba10f </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_czech&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action lithuania_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_lithuania&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_lithuania&gt;&gt;</span>
<span class="yarn-line">    Aiuta i nostri amici lituani!</span> <span class="yarn-meta">#line:0a1e0a3 #task:talk_npc_lithuania</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $czech_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_czech&gt;&gt;</span>
<span class="yarn-line">    Ciao! Vengo dalla Repubblica Ceca!</span> <span class="yarn-meta">#line:0ded863 </span>
<span class="yarn-line">    La Repubblica Ceca è famosa per i suoi splendidi castelli, il cristallo e i deliziosi gnocchi.</span> <span class="yarn-meta">#line:03dc09c </span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La mia bandiera è diversa! È bianca e rossa con un triangolo blu.</span> <span class="yarn-meta">#line:0000741 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-czech-republic"></a>

## task_czech_republic

<div class="yarn-node" data-title="task_czech_republic">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera ceca e riportala indietro.</span> <span class="yarn-meta">#line:0ff23aa #task:find_flag_czech</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_czech find_flag_czech_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-find-flag-czech-done"></a>

## find_flag_czech_done

<div class="yarn-node" data-title="find_flag_czech_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Czech_Republic&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera ceca! Torna alla Repubblica Ceca.</span> <span class="yarn-meta">#shadow:0a134f5 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-czech-republic"></a>

## item_flag_czech_republic

<div class="yarn-node" data-title="item_flag_czech_republic">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: czech_republic</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_czech"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera ceca! Torna alla Repubblica Ceca.</span> <span class="yarn-meta">#line:0a134f5 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Czech&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Repubblica Ceca.</span> <span class="yarn-meta">#line:0fdc68b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lithuania-npc"></a>

## lithuania_npc

<div class="yarn-node" data-title="lithuania_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// lithuania</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lithuania_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_vilnius&gt;&gt;</span>
<span class="yarn-line">    Vilnius è la capitale della Lituania.</span> <span class="yarn-meta">#line:0acbb04 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_lithuania")&gt;&gt;</span>   
<span class="yarn-line">    Grazie, la mia bellissima bandiera è tornata!</span> <span class="yarn-meta">#line:0d2f54c </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_lithuania&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action ukraine_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_ukraine&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico ucraino?</span> <span class="yarn-meta">#line:0bcf83b #task:talk_npc_ukraine</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $lithuania_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-line">    Ciao! Vengo dalla Lituania!</span> <span class="yarn-meta">#line:0534a85 </span>
<span class="yarn-line">    Un terzo del nostro Paese è ricoperto da foreste!</span> <span class="yarn-meta">#line:0465c31 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Ricordate, la mia bandiera è rossa, verde e gialla.</span> <span class="yarn-meta">#line:00af906 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lithuania"></a>

## task_lithuania

<div class="yarn-node" data-title="task_lithuania">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera lituana.</span> <span class="yarn-meta">#line:0b88326 #task:find_flag_lithuania</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_lithuania task_lithuania_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lithuania-done"></a>

## task_lithuania_done

<div class="yarn-node" data-title="task_lithuania_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_lithuania add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Lithuania&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera lituana! Torna in Lituania.</span> <span class="yarn-meta">#shadow:069f1a0 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-lithuania"></a>

## item_flag_lithuania

<div class="yarn-node" data-title="item_flag_lithuania">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: lithuania</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_lithuania"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera lituana! Torna in Lituania.</span> <span class="yarn-meta">#line:069f1a0 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_lithuania&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Lituania.</span> <span class="yarn-meta">#line:0a3e2f1</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ukraine-npc"></a>

## ukraine_npc

<div class="yarn-node" data-title="ukraine_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ukraine</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $ukraine_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_kyiv&gt;&gt;</span>
<span class="yarn-line">    La nostra capitale è Kiev.</span> <span class="yarn-meta">#line:01b1e6d </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_ukraine")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Quella è la mia bandiera.</span> <span class="yarn-meta">#line:05de5ab </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_ukraine&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action slovakia_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_slovakia&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card  country_slovakia&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico slovacco?</span> <span class="yarn-meta">#line:0aa87ef #task:talk_npc_slovakia</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $ukraine_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
<span class="yarn-line">    Ciao! Vengo dall'Ucraina!</span> <span class="yarn-meta">#line:07ea99a </span>
<span class="yarn-line">    Siamo chiamati il ​​"granaio" d'Europa perché produciamo molto grano!</span> <span class="yarn-meta">#line:0dbd4af </span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    No! La nostra bandiera è blu e gialla.</span> <span class="yarn-meta">#line:0a94866 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ukraine"></a>

## task_ukraine

<div class="yarn-node" data-title="task_ukraine">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera dell'Ucraina. È blu e gialla.</span> <span class="yarn-meta">#line:07c148b #task:find_flag_ukraine</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_UK&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_ukraine task_ukraine_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-ukraine-done"></a>

## task_ukraine_done

<div class="yarn-node" data-title="task_ukraine_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_ukraine add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_ukraine&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera ucraina! Torna in Ucraina.</span> <span class="yarn-meta">#shadow:0f8bc9d </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-ukraine"></a>

## item_flag_ukraine

<div class="yarn-node" data-title="item_flag_ukraine">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: ukraine</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_ukraine"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera ucraina! Torna in Ucraina.</span> <span class="yarn-meta">#line:0f8bc9d </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera dell'Ucraina.</span> <span class="yarn-meta">#line:0805b90</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-slovakia"></a>

## npc_slovakia

<div class="yarn-node" data-title="npc_slovakia">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// slovakia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $slovakia_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_bratislava&gt;&gt;</span>
<span class="yarn-line">    La capitale della Slovacchia è Bratislava!</span> <span class="yarn-meta">#line:0891aba</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_slovakia")&gt;&gt;</span>
<span class="yarn-line">    Grazie per avermi riportato la bandiera!</span> <span class="yarn-meta">#line:0d453e9 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end find_flag_slovakia&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;action russia_active&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;target npc_russia&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;card country_russia&gt;&gt;</span>
<span class="yarn-comment">    // Can you help our russian neighbour?</span> <span class="yarn-meta">#line:04bf6d1 #task:talk_npc_russia</span>

<span class="yarn-comment">    // &lt;&lt;task_start talk_npc_russia&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Poland&gt;&gt;</span>
<span class="yarn-line">    Torna all'inizio e prendi il tuo premio!</span> <span class="yarn-meta">#line:07b7f05 #task:talk_npc_poland_final</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_poland_final&gt;&gt;</span> 
<span class="yarn-cmd">&lt;&lt;elseif $slovakia_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_met = true&gt;&gt;</span>
<span class="yarn-line">    Vengo dalla Slovacchia!</span> <span class="yarn-meta">#line:054e1b8 </span>
<span class="yarn-line">    La Slovacchia è nota per le sue splendide montagne e grotte.</span> <span class="yarn-meta">#line:0278f6a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La nostra bandiera è bianca, rossa e blu con uno stemma.</span> <span class="yarn-meta">#line:0af30a1 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-slovakia"></a>

## task_slovakia

<div class="yarn-node" data-title="task_slovakia">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera slovacca.</span> <span class="yarn-meta">#line:04b6692 #task:find_flag_slovakia</span>
<span class="yarn-line">È bianco, rosso e blu con lo stemma.</span> <span class="yarn-meta">#line:0866ee1 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_SL&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_slovakia npc_slovakia_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-slovakia-done"></a>

## npc_slovakia_done

<div class="yarn-node" data-title="npc_slovakia_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_slovakia add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Slovakia&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera slovacca! Torna in Slovacchia.</span> <span class="yarn-meta">#shadow:0c95e0a </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-slovakia"></a>

## item_flag_slovakia

<div class="yarn-node" data-title="item_flag_slovakia">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: slovakia</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_slovakia"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera slovacca! Torna in Slovacchia.</span> <span class="yarn-meta">#line:0c95e0a </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Slovacchia.</span> <span class="yarn-meta">#line:0768ab7 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-russia"></a>

## npc_russia

<div class="yarn-node" data-title="npc_russia">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Russia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $russia_completed&gt;&gt;</span>
<span class="yarn-line">    La capitale della Russia è Mosca!</span> <span class="yarn-meta">#line:0f8817a </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_russia")&gt;&gt;</span>
<span class="yarn-comment">    // Thank you for bringing my flag back!</span> <span class="yarn-meta">#line:0466a59 </span>

<span class="yarn-comment">    // &lt;&lt;inventory flag_russia remove&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;task_end find_flag_russia&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;set $russia_met = true&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;set $russia_completed = true&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
<span class="yarn-comment">    // &lt;&lt;target NPC_Poland&gt;&gt;</span>
<span class="yarn-comment">    // Go back to the start and get your prize!</span> <span class="yarn-meta">#line:07b7f05 #task:talk_npc_poland_final</span>

<span class="yarn-comment">    // &lt;&lt;task_start talk_npc_poland_final&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $russia_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $russia_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_russia&gt;&gt;</span>
<span class="yarn-line">    Ciao, sono Russia.</span> <span class="yarn-meta">#line:065c41c </span>
<span class="yarn-line">    Questa è solo una piccola parte del grande Paese.</span> <span class="yarn-meta">#line:0a4bae4 </span>
<span class="yarn-comment">    // &lt;&lt;jump task_russia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La nostra bandiera è rossa con una croce bianca.</span> <span class="yarn-meta">#line:05e0820 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-russia"></a>

## task_russia

<div class="yarn-node" data-title="task_russia">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera russa</span> <span class="yarn-meta">#line:06442e4 </span>
<span class="yarn-line">È bianco, blu e rosso.</span> <span class="yarn-meta">#line:0c6ba5e </span>
<span class="yarn-cmd">&lt;&lt;task_start find_flag_russia task_russia_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-russia-done"></a>

## task_russia_done

<div class="yarn-node" data-title="task_russia_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_russia add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Russia&gt;&gt;</span>
<span class="yarn-line">Hai trovato la bandiera russa! Torna in Russia.</span> <span class="yarn-meta">#shadow:0d1a793 </span>

</code>
</pre>
</div>

<a id="ys-node-item-flag-russia"></a>

## item_flag_russia

<div class="yarn-node" data-title="item_flag_russia">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "find_flag_russia"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera russa! Torna in Russia.</span> <span class="yarn-meta">#line:0d1a793 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_russia add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Russia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Russia.</span> <span class="yarn-meta">#line:00f1fcc</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-npc"></a>

## spawned_npc

<div class="yarn-node" data-title="spawned_npc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In Europa si parlano più di 50 lingue!</span> <span class="yarn-meta">#line:04f4280 </span>
<span class="yarn-line">In Europa puoi muoverti liberamente tra i Paesi!</span> <span class="yarn-meta">#line:08d624d</span>

</code>
</pre>
</div>


