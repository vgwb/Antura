---
title: I vicini della Francia (fr_00) - Script
hide:
---

# I vicini della Francia (fr_00) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-00-the-neighbors-of-france/22)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1044148815#gid=1044148815)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_00%20Geo%20France/FR_00%20Geo%20France%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_00 | France GEO</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags: Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $COUNTRIES_COMPLETED = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $TOTAL_COUNTRIES = 7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $france_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $germany_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $italy_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $swiss_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lux_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $belgium_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $spain_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card country_france&gt;&gt;</span>
<span class="yarn-line">Benvenuti in Francia!</span> <span class="yarn-meta">#line:046db1f </span>
<span class="yarn-line">Siamo in Europa.</span> <span class="yarn-meta">#line:08a8ce8 </span>
<span class="yarn-line">Incontriamo amici provenienti dai paesi vicini.</span> <span class="yarn-meta">#line:08a09de </span>
<span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>

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
<span class="yarn-line">La missione è completata.</span> <span class="yarn-meta">#line:0432689 </span>
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
<span class="yarn-line">Ora puoi disegnare la tua bandiera?</span> <span class="yarn-meta">#line:0c48a14 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-win"></a>

## win

<div class="yarn-node" data-title="win">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// FRANCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">type:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_end talk_npc_french_final&gt;&gt;</span>
<span class="yarn-line">Ottimo lavoro! Ce l'hai fatta!</span> <span class="yarn-meta">#line:0ba3c4c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">Hai trovato una parte d'Europa!</span> <span class="yarn-meta">#line:06e1dd4 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-france-npc"></a>

## france_npc

<div class="yarn-node" data-title="france_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $COUNTRIES_COMPLETED &gt;= $TOTAL_COUNTRIES&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $france_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">    Parigi è la capitale della Francia.</span> <span class="yarn-meta">#line:055df11 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_france")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_france"&gt;&gt;</span>
<span class="yarn-line">        Sì, quella è la mia bandiera! Merci!</span> <span class="yarn-meta">#line:01e24a8 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_france remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_FRENCH_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $france_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;area area_europe&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;target NPC_Germany&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;card country_germany&gt;&gt;</span>
<span class="yarn-line">        Ora puoi aiutare i nostri amici tedeschi?</span> <span class="yarn-meta">#line:04bd4db #task:talk_npc_german</span>
        <span class="yarn-cmd">&lt;&lt;task_start talk_npc_german&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Quella non è la mia bandiera. La mia è blu, bianca e rossa.</span> <span class="yarn-meta">#line:04e0432 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $france_met = true&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono dalla Francia!</span> <span class="yarn-meta">#line:06dfdd6</span>
<span class="yarn-line">    Antura ha confuso tutte le bandiere!</span> <span class="yarn-meta">#line:0a20eed </span>
<span class="yarn-line">    Mi potete aiutare?</span> <span class="yarn-meta">#line:0a9f34e </span>
    <span class="yarn-cmd">&lt;&lt;jump task_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-france"></a>

## task_france

<div class="yarn-node" data-title="task_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset flag_france&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera francese.</span> <span class="yarn-meta">#line:0e35434 #task:FIND_FRENCH_FLAG</span>
<span class="yarn-line">La mia bandiera è blu, bianca e rossa.</span> <span class="yarn-meta">#line:0868737 </span>
<span class="yarn-cmd">&lt;&lt;target Flag_France&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_FRENCH_FLAG task_france_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-france-done"></a>

## task_france_done

<div class="yarn-node" data-title="task_france_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_france add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>
[MISSING TRANSLATION: You found the French flag! Go back to France. #shadow:0a9b166]

</code>
</pre>
</div>

<a id="ys-node-item-flag-france"></a>

## item_flag_france

<div class="yarn-node" data-title="item_flag_france">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: france</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_france&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_FRENCH_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera francese! Torna in Francia.</span> <span class="yarn-meta">#line:0a9b166 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_france add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Francia.</span> <span class="yarn-meta">#line:01d9617  </span>
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
    <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action italy_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_italy&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_italy&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico italiano?</span> <span class="yarn-meta">#line:03684cb #task:talk_npc_italy</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_germany&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono dalla Germania!</span> <span class="yarn-meta">#line:0068fe1 </span>
<span class="yarn-line">    Siamo famosi per i castelli, le foreste e i treni!</span> <span class="yarn-meta">#line:04dc97a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La nostra bandiera ha delle strisce: nera, rossa, gialla.</span> <span class="yarn-meta">#line:0cd7024</span>
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
<span class="yarn-line">Trova la bandiera tedesca e riportala indietro.</span> <span class="yarn-meta">#line:029ee72 #task:FIND_GERMAN_FLAG</span>
<span class="yarn-line">Ha delle strisce: nere, rosse, gialle.</span> <span class="yarn-meta">#line:0f95ef2 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany_done&gt;&gt;</span>

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
[MISSING TRANSLATION: You found the German flag! Go back to Germany. #shadow:05f0ed2]

</code>
</pre>
</div>

<a id="ys-node-item-flag-germany"></a>

## item_flag_germany

<div class="yarn-node" data-title="item_flag_germany">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: germany</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_GERMAN_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera tedesca! Torna in Germania.</span> <span class="yarn-meta">#line:05f0ed2</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Germania.</span> <span class="yarn-meta">#line:05ff51a</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-italy-npc"></a>

## italy_npc

<div class="yarn-node" data-title="italy_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ITALY</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $italy_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_rome&gt;&gt;</span>
<span class="yarn-line">    La nostra capitale è Roma!</span> <span class="yarn-meta">#line:0148edb</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_italy")&gt;&gt;</span> 
<span class="yarn-line">    Grazie! Questa è la mia bandiera!</span> <span class="yarn-meta">#line:081ac66 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_italy remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_ITALIAN_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action belgium_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Belgium&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_belgium&gt;&gt;</span>
<span class="yarn-line">    Aiutateli a trovare la bandiera belga!</span> <span class="yarn-meta">#line:001f54f #task:talk_npc_belgium</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $italy_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $italy_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_italy&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono italiano!</span> <span class="yarn-meta">#line:0bda0fc </span>
    <span class="yarn-cmd">&lt;&lt;jump task_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La mia bandiera è verde, bianca e rossa.</span> <span class="yarn-meta">#line:0dc8623</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-italy"></a>

## task_italy

<div class="yarn-node" data-title="task_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera italiana.</span> <span class="yarn-meta">#line:0ed29f1 #task:FIND_ITALIAN_FLAG</span>
<span class="yarn-line">È verde, bianco e rosso come una pizza!</span> <span class="yarn-meta">#line:0cde44c </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ITALIAN_FLAG task_italy_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-italy-done"></a>

## task_italy_done

<div class="yarn-node" data-title="task_italy_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_italy add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Italy&gt;&gt;</span>
[MISSING TRANSLATION: You found the Italian flag! Go back to Italy. #shadow:0dc2c74]

</code>
</pre>
</div>

<a id="ys-node-item-flag-italy"></a>

## item_flag_italy

<div class="yarn-node" data-title="item_flag_italy">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: italy</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_ITALIAN_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera italiana! Torna in Italia.</span> <span class="yarn-meta">#line:0dc2c74</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_italy add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Italy&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera dell'Italia.</span> <span class="yarn-meta">#line:0fdc68b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-belgium-npc"></a>

## belgium_npc

<div class="yarn-node" data-title="belgium_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// BELGIUM</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $belgium_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_brussels&gt;&gt;</span>
<span class="yarn-line">    Bruxelles è la capitale del Belgio.</span> <span class="yarn-meta">#line:06b3ceb </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_belgium") &gt;&gt;</span>
<span class="yarn-line">    Grazie! La mia bandiera è tornata!</span> <span class="yarn-meta">#line:079096a </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belgium remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_BELGIUM_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action spain_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Spain&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_spain&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico spagnolo?</span> <span class="yarn-meta">#line:03a9b7d #task:talk_npc_spain</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $belgium_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belgium_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_belgium&gt;&gt;</span>
<span class="yarn-line">    Buongiorno! Vengo dal Belgio!</span> <span class="yarn-meta">#line:0a61b67 </span>
<span class="yarn-line">    Parliamo anche francese!</span> <span class="yarn-meta">#line:0b18893 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Ricorda: la mia bandiera ha le strisce: nera, gialla, rossa.</span> <span class="yarn-meta">#line:0141a13 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-belgium"></a>

## task_belgium

<div class="yarn-node" data-title="task_belgium">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera belga.</span> <span class="yarn-meta">#line:0f08126 #task:FIND_BELGIUM_FLAG</span>
<span class="yarn-line">È nero, giallo e rosso con strisce verticali.</span> <span class="yarn-meta">#line:079ecca </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_BE&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_BELGIUM_FLAG task_belgium_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-belgium-done"></a>

## task_belgium_done

<div class="yarn-node" data-title="task_belgium_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_belgium add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Belgium&gt;&gt;</span>
[MISSING TRANSLATION: You found the Belgian flag! Go back to Belgium. #shadow:073fe02]

</code>
</pre>
</div>

<a id="ys-node-item-flag-belgium"></a>

## item_flag_belgium

<div class="yarn-node" data-title="item_flag_belgium">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: belgium</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_BELGIUM_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera belga! Torna in Belgio.</span> <span class="yarn-meta">#line:073fe02 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belgium add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Belgium&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera del Belgio.</span> <span class="yarn-meta">#line:0b11066 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spain-npc"></a>

## spain_npc

<div class="yarn-node" data-title="spain_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SPAIN</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $spain_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_madrid&gt;&gt;</span>
<span class="yarn-line">    Madrid è la capitale della Spagna.</span> <span class="yarn-meta">#line:09cf6c9</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_spain")&gt;&gt;</span>
<span class="yarn-line">    Quella è la mia bandiera!</span> <span class="yarn-meta">#line:0c57e40 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_spain remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_SPANISH_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action lux_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Lux&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_luxembourg&gt;&gt;</span>
<span class="yarn-line">    Grazie. Puoi dare la bandiera al mio amico lussemburghese?</span> <span class="yarn-meta">#line:0930602 #task:talk_npc_luxembourg</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_luxembourg&gt;&gt;</span> 
<span class="yarn-cmd">&lt;&lt;elseif $spain_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $spain_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_spain&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono dalla Spagna!</span> <span class="yarn-meta">#line:0f5bc06 </span>
<span class="yarn-line">    Abbiamo un ballo chiamato flamenco!</span> <span class="yarn-meta">#line:05e6d48 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Non è mia! La nostra bandiera è rossa e gialla.</span> <span class="yarn-meta">#line:0db05da </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-spain"></a>

## task_spain

<div class="yarn-node" data-title="task_spain">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera spagnola.</span> <span class="yarn-meta">#line:091cc7c #task:FIND_SPANISH_FLAG</span>
<span class="yarn-line">È rosso e giallo come il sole.</span> <span class="yarn-meta">#line:09635b4</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_SP&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SPANISH_FLAG task_spain_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-spain-done"></a>

## task_spain_done

<div class="yarn-node" data-title="task_spain_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_spain add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Spain&gt;&gt;</span>
[MISSING TRANSLATION: You found the Spanish flag! Go back to Spain. #shadow:049610c]

</code>
</pre>
</div>

<a id="ys-node-item-flag-spain"></a>

## item_flag_spain

<div class="yarn-node" data-title="item_flag_spain">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">group: spain</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_SPANISH_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera spagnola! Torna in Spagna.</span> <span class="yarn-meta">#line:049610c </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_spain add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Spain&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Spagna</span> <span class="yarn-meta">#line:006ce10 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-luxembourg-npc"></a>

## luxembourg_npc

<div class="yarn-node" data-title="luxembourg_npc">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// LUXEMBURG</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $lux_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_luxembourg_city&gt;&gt;</span>
<span class="yarn-line">    La capitale del Lussemburgo è... Lussemburgo!</span> <span class="yarn-meta">#line:02114ba </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_luxembourg")&gt;&gt;</span>
<span class="yarn-line">    Grazie! Quella è la mia bandiera.</span> <span class="yarn-meta">#line:05de5ab</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_LUX_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action swiss_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Swiss&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_switzerland&gt;&gt;</span>
<span class="yarn-line">    Puoi aiutare il mio amico svizzero?</span> <span class="yarn-meta">#line:0090192 #task:talk_npc_swiss</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $lux_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lux_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_luxembourg&gt;&gt;</span>
<span class="yarn-line">    Moien! Sono del Lussemburgo!</span> <span class="yarn-meta">#line:0b22d58 </span>
<span class="yarn-line">    Siamo piccoli e parliamo tre lingue!</span> <span class="yarn-meta">#line:0a3d0e3 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    No! La nostra bandiera è rossa, bianca e azzurra.</span> <span class="yarn-meta">#line:0529472 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lux"></a>

## task_lux

<div class="yarn-node" data-title="task_lux">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera del Lussemburgo.</span> <span class="yarn-meta">#line:0ed3698 #task:FIND_LUX_FLAG</span>
<span class="yarn-line">È rosso, bianco e azzurro.</span> <span class="yarn-meta">#line:018e4cf </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_LU&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_LUX_FLAG task_lux_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-lux-done"></a>

## task_lux_done

<div class="yarn-node" data-title="task_lux_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Luxembourg&gt;&gt;</span>
[MISSING TRANSLATION: You found the Luxembourg flag! Go back to Luxembourg. #shadow:0324097]

</code>
</pre>
</div>

<a id="ys-node-item-flag-lux"></a>

## item_flag_lux

<div class="yarn-node" data-title="item_flag_lux">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: lux</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_luxembourg&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_LUX_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera del Lussemburgo! Torna a Lussemburgo.</span> <span class="yarn-meta">#line:0324097 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_luxembourg add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Lux&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera del Lussemburgo.</span> <span class="yarn-meta">#line:05badd7 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-swiss"></a>

## npc_swiss

<div class="yarn-node" data-title="npc_swiss">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// SWISS</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $swiss_completed&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_bern&gt;&gt;</span>
<span class="yarn-line">    La capitale della Svizzera è Berna!</span> <span class="yarn-meta">#line:0d2e2d7 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_switzerland")&gt;&gt;</span>
<span class="yarn-line">    Grazie per avermi riportato la bandiera!</span> <span class="yarn-meta">#line:0ca99a0 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_SWISS_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $COUNTRIES_COMPLETED = $COUNTRIES_COMPLETED + 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_France&gt;&gt;</span>
<span class="yarn-line">    Torna all'inizio e prendi il tuo premio!</span> <span class="yarn-meta">#line:0148420 #task:talk_npc_french_final</span>
    <span class="yarn-cmd">&lt;&lt;task_start talk_npc_french_final&gt;&gt;</span> 
<span class="yarn-cmd">&lt;&lt;elseif $swiss_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $swiss_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card country_switzerland&gt;&gt;</span>
<span class="yarn-line">    Ciao! Sono dalla Svizzera!</span> <span class="yarn-meta">#line:09fe8fc </span>
<span class="yarn-line">    Siamo famosi per le montagne innevate e il formaggio.</span> <span class="yarn-meta">#line:0bf6b0d </span>
    <span class="yarn-cmd">&lt;&lt;jump task_swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    La nostra bandiera è rossa con una croce bianca.</span> <span class="yarn-meta">#line:0caad5a </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-swiss"></a>

## task_swiss

<div class="yarn-node" data-title="task_swiss">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-line">Trova la bandiera svizzera.</span> <span class="yarn-meta">#line:0ec7096 #task:FIND_SWISS_FLAG</span>
<span class="yarn-line">È rosso con una grande croce bianca, come un kit di pronto soccorso.</span> <span class="yarn-meta">#line:07cc57e </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_SW&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_SWISS_FLAG task_swiss_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-swiss-done"></a>

## task_swiss_done

<div class="yarn-node" data-title="task_swiss_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;inventory flag_switzerland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target NPC_Switzerland&gt;&gt;</span>
[MISSING TRANSLATION: You found the Swiss flag! Go back to Switzerland. #shadow:0feb2b9]

</code>
</pre>
</div>

<a id="ys-node-item-flag-switzerland"></a>

## item_flag_switzerland

<div class="yarn-node" data-title="item_flag_switzerland">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: swiss</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_switzerland&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if GetCurrentTask() == "FIND_SWISS_FLAG"&gt;&gt;</span>
<span class="yarn-line">    Hai trovato la bandiera svizzera! Torna in Svizzera.</span> <span class="yarn-meta">#line:0feb2b9</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_switzerland add&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target NPC_Swiss&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bandiera della Svizzera.</span> <span class="yarn-meta">#line:0768ab7 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monaco"></a>

## monaco

<div class="yarn-node" data-title="monaco">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// MONACO</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: monaco</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Buongiorno! Sono di Monaco!</span> <span class="yarn-meta">#line:0a3f8f6 </span>
<span class="yarn-line">Siamo piccoli. Abbiamo gare automobilistiche e un palazzo in riva al mare!</span> <span class="yarn-meta">#line:0dc315a </span>
<span class="yarn-cmd">&lt;&lt;asset flag_monaco&gt;&gt;</span>
<span class="yarn-line">La mia bandiera è rossa e bianca.</span> <span class="yarn-meta">#line:00bd939 </span>

</code>
</pre>
</div>

<a id="ys-node-andorra"></a>

## andorra

<div class="yarn-node" data-title="andorra">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ANDORRA</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: andorra</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset flag_andorra&gt;&gt;</span>
<span class="yarn-line">Ciao! Sono di Andorra!</span> <span class="yarn-meta">#line:071f85c </span>
<span class="yarn-line">La mia bandiera è blu, gialla e rossa.</span> <span class="yarn-meta">#line:02846e8 </span>

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
<span class="yarn-line">In Europa si parlano più di 100 lingue!</span> <span class="yarn-meta">#line:04f4280</span>
<span class="yarn-line">L'Europa è composta da circa 45 paesi, tutti vicini tra loro come vicini di casa!</span> <span class="yarn-meta">#line:06e3b06 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-france"></a>

## spawned_france

<div class="yarn-node" data-title="spawned_france">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In Francia si trova la Torre Eiffel, una gigantesca torre di ferro che brilla di notte.</span> <span class="yarn-meta">#line:0c097e4 </span>
<span class="yarn-line">La Francia è il paese più grande dell'Unione Europea.</span> <span class="yarn-meta">#line:0ab3dd9 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-germany"></a>

## spawned_germany

<div class="yarn-node" data-title="spawned_germany">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La Germania è famosa per la birra e le salsicce.</span> <span class="yarn-meta">#line:06722b3 </span>
<span class="yarn-line">La capitale della Germania è Berlino.</span> <span class="yarn-meta">#line:05e9552 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-italy"></a>

## spawned_italy

<div class="yarn-node" data-title="spawned_italy">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: all </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'Italia sembra uno stivale sulla mappa!</span> <span class="yarn-meta">#line:0a23cce </span>
<span class="yarn-line">L'Italia è famosa per la pizza e la pasta.</span> <span class="yarn-meta">#line:0278d00 </span>

</code>
</pre>
</div>


