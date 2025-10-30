---
title: Les voisins de la Pologne (pl_00) - Script
hide:
---

# Les voisins de la Pologne (pl_00) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-00-the-neighbors-of-poland/31/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1929643794#gid=1929643794)  
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
<span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 7&gt;&gt;</span>
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
<span class="yarn-cmd">&lt;&lt;action area_small&gt;&gt;</span>

<span class="yarn-line">Bienvenue en Pologne !</span> <span class="yarn-meta">#line:046db1f</span>
<span class="yarn-line">Nous sommes en Europe</span> <span class="yarn-meta">#line:002aafd</span>
<span class="yarn-line">Rencontrons d'autres enfants des pays voisins.</span> <span class="yarn-meta">#line:0862117</span>

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
<span class="yarn-line">Cette quête est terminée.</span> <span class="yarn-meta">#line:085bc39</span>
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
<span class="yarn-line">Pourquoi ne dessines-tu pas ton drapeau maintenant ?</span> <span class="yarn-meta">#line:01f830b </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

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
&lt;&lt;if $CURRENT_PROGRESS &gt;= $MAX_PROGRESS&gt;&gt;
    <span class="yarn-cmd">&lt;&lt;jump poland_npc_win&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $poland_completed == true&gt;&gt;</span>
<span class="yarn-line">    Merci de m'aider !</span> <span class="yarn-meta">#line:0a4a56f </span>
<span class="yarn-line">    Peux-tu aider mes autres amis ?</span> <span class="yarn-meta">#line:097eeb8 </span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM == "flag_poland"&gt;&gt;</span>
<span class="yarn-line">    Oui, c'est mon drapeau ! Merci !</span> <span class="yarn-meta">#line:0c9fa89 </span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland remove&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_end FIND_polish_FLAG&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_ITEM = ""&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
<span class="yarn-line">    Pouvez-vous trouver le drapeau allemand ?</span> <span class="yarn-meta">#line:04bd4db </span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action germany_active&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_GE&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action area_bigger&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany  &gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Ce n'est pas mon drapeau. Le mien est blanc et rouge.</span> <span class="yarn-meta">#line:0f967da </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Bonjour ! Je viens de Pologne !</span> <span class="yarn-meta">#line:017ad80</span>
<span class="yarn-line">    Antura a fait un gâchis et tous les drapeaux ont été mélangés !</span> <span class="yarn-meta">#line:0b9e31d</span>
<span class="yarn-line">    Mon drapeau, celui polonais, est blanc et rouge.</span> <span class="yarn-meta">#line:08a2f6d</span>
<span class="yarn-line">    Pouvez-vous m'aider?</span> <span class="yarn-meta">#line:0fe57ef</span>
    <span class="yarn-cmd">&lt;&lt;set $poland_met = true&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau polonais.</span> <span class="yarn-meta">#line:09e3b54 #task:FIND_polish_FLAG</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_PL&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_polish_FLAG task_poland&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de la Pologne.</span> <span class="yarn-meta">#line:07ca581</span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_poland"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_poland add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-poland-npc-win"></a>

## poland_npc_win

<div class="yarn-node" data-title="poland_npc_win">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bravo ! Vous avez réussi !</span> <span class="yarn-meta">#line:0ba3c4c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map&gt;&gt;</span>
<span class="yarn-line">Vous avez découvert une partie de l'Europe Centrale !</span> <span class="yarn-meta">#line:031f72c </span>
<span class="yarn-cmd">&lt;&lt;card concept_europe_map zoom&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

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
    <span class="yarn-cmd">&lt;&lt;card flag_germany&gt;&gt;</span>
<span class="yarn-line">    Merci de m'aider !</span> <span class="yarn-meta">#line:0eaf07d </span>
<span class="yarn-line">    Berlin est la capitale de l'Allemagne.</span> <span class="yarn-meta">#line:0446f03 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_germany")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_germany"&gt;&gt;</span>
<span class="yarn-line">        Merci ! C'est mon drapeau !</span> <span class="yarn-meta">#line:0ba8707</span>
<span class="yarn-line">        Pouvez-vous aider mon ami biélorusse ?</span> <span class="yarn-meta">#line:06c463a </span>
        <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_germany remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_GERMAN_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action belarus_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $germany_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_BR&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire !</span> <span class="yarn-meta">#line:0e48014 </span>
<span class="yarn-line">        C'est jaune, noir et rouge, sélectionnez-le et parlez-moi à nouveau.</span> <span class="yarn-meta">#line:0d78194 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    HOMME : Notre drapeau a des bandes horizontales noires, rouges et jaunes.</span> <span class="yarn-meta">#line:0cd7024 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $germany_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $germany_met = true&gt;&gt;</span>
<span class="yarn-line">    Bonjour ! Je viens d'Allemagne !</span> <span class="yarn-meta">#line:0068fe1 </span>
<span class="yarn-line">    Nous sommes célèbres pour nos châteaux, nos forêts et nos trains !</span> <span class="yarn-meta">#line:04dc97a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_germany&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau allemand et apportez-le à l'Allemand.</span> <span class="yarn-meta">#line:029ee72 #task:FIND_GERMAN_FLAG</span>
<span class="yarn-line">Il a des rayures horizontales noires, rouges et jaunes.</span> <span class="yarn-meta">#line:0f95ef2 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_GE&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_GERMAN_FLAG task_germany&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de l'Allemagne.</span> <span class="yarn-meta">#line:05ff51a </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_germany"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_germany add&gt;&gt;</span>
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
    <span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">    Merci de m'aider !</span> <span class="yarn-meta">#line:0a5c214 </span>
<span class="yarn-line">    Minsk est la capitale de la Biélorussie !</span> <span class="yarn-meta">#line:0aecb59</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_belarus")&gt;&gt;</span> 
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_belarus"&gt;&gt;</span>
<span class="yarn-line">        C'est mon drapeau !</span> <span class="yarn-meta">#line:0c57e40 </span>
<span class="yarn-line">        Merci, peux-tu donner son drapeau à mon ami tchèque ?</span> <span class="yarn-meta">#line:021e1a2 </span>
        <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_belarus remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_belarusian_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action czech_republic_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $belarus_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_CZ&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire !</span> <span class="yarn-meta">#line:079c8f0 </span>
<span class="yarn-line">        C'est rouge et vert, avec un motif sur la gauche.</span> <span class="yarn-meta">#line:0e78a13 </span>
<span class="yarn-line">        Sélectionnez-le et apportez-le-moi.</span> <span class="yarn-meta">#line:0b57b74 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Mon drapeau est rouge et vert avec un motif rouge sur la gauche.</span> <span class="yarn-meta">#line:0653fae </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $belarus_met == false &gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $belarus_met = true&gt;&gt;</span>
<span class="yarn-line">    Bonjour ! Je viens de Biélorussie !</span> <span class="yarn-meta">#line:0ccf58d </span>
<span class="yarn-line">    Nous avons une forêt primitive, elle pousse depuis des siècles !</span> <span class="yarn-meta">#line:0c6ac62 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_belarus&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-task-belarus"></a>

## task_belarus

<div class="yarn-node" data-title="task_belarus">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: belarus</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_belarus&gt;&gt;</span>
<span class="yarn-line">Trouvez le drapeau biélorusse.</span> <span class="yarn-meta">#line:0c00afe #task:FIND_belarusian_FLAG</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_BR&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_belarusian_FLAG task_belarus&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de la Biélorussie</span> <span class="yarn-meta">#line:006ce10 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_belarus"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_belarus add&gt;&gt;</span>
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
    <span class="yarn-cmd">&lt;&lt;card flag_czech_republic&gt;&gt;</span>
<span class="yarn-line">    Merci ! Notre capitale est Minsk !</span> <span class="yarn-meta">#line:08473de </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_czech_republic")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_czech_republic"&gt;&gt;</span>
<span class="yarn-line">        Merci ! C'est mon drapeau !</span> <span class="yarn-meta">#line:07ba10f </span>
<span class="yarn-line">        Aidez-les à trouver le drapeau lituanien et apportez-le-leur !</span> <span class="yarn-meta">#line:0a1e0a3 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_czech_republic_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $czech_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action lithuania_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_LI&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire !</span> <span class="yarn-meta">#line:0278e65 </span>
<span class="yarn-line">        C'est rouge, bleu et blanc ! Sélectionne-le et reparle-moi.</span> <span class="yarn-meta">#line:07f7a61 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Mon drapeau est différent ! Il est blanc et rouge avec un triangle bleu.</span> <span class="yarn-meta">#line:0000741 </span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $czech_met == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $czech_met = true&gt;&gt;</span>
<span class="yarn-line">    Salut ! Je viens de République tchèque !</span> <span class="yarn-meta">#line:0ded863 </span>
<span class="yarn-line">    Nous avons le plus de châteaux d'Europe !</span> <span class="yarn-meta">#line:03dc09c </span>
    <span class="yarn-cmd">&lt;&lt;jump task_czech_republic&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau tchèque.</span> <span class="yarn-meta">#line:0ff23aa #task:FIND_czech_republic_FLAG</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_CZ&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_czech_republic_FLAG task_czech_republic&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de la République tchèque.</span> <span class="yarn-meta">#line:0fdc68b </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_czech_republic"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_czech_republic add&gt;&gt;</span>
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
    <span class="yarn-cmd">&lt;&lt;card flag_lithuania&gt;&gt;</span>
<span class="yarn-line">    Merci de nous aider !</span> <span class="yarn-meta">#line:06cf019 </span>
<span class="yarn-line">    Bruxelles est la capitale de la Lituanie.</span> <span class="yarn-meta">#line:0acbb04 </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_lithuania")&gt;&gt;</span>   
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_lithuania"&gt;&gt;</span>
<span class="yarn-line">        Merci, mon beau drapeau est de retour !</span> <span class="yarn-meta">#line:0d2f54c </span>
<span class="yarn-line">        Pouvez-vous aider mon ami ukrainien ?</span> <span class="yarn-meta">#line:0bcf83b </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_lithuania_FLAG &gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action ukraine_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_UK&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire !</span> <span class="yarn-meta">#line:0036d67 </span>
<span class="yarn-line">        C'est rouge, jaune et vert. Sélectionnez-le et reparlez-moi.</span> <span class="yarn-meta">#line:078ab98 </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    N'oubliez pas, mon drapeau est rouge, vert et jaune.</span> <span class="yarn-meta">#line:00af906 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $lithuania_met = true&gt;&gt;</span>
<span class="yarn-line">    Salut ! Je viens de Lituanie !</span> <span class="yarn-meta">#line:0534a85 </span>
<span class="yarn-line">    Un tiers de notre pays est couvert de forêts !</span> <span class="yarn-meta">#line:0465c31 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_lithuania&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau lituanien.</span> <span class="yarn-meta">#line:0b88326 #task:FIND_lithuania_FLAG</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_LI&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_lithuania_FLAG task_lithuania&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de la Lituanie.</span> <span class="yarn-meta">#line:0942331 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_lithuania"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_lithuania add&gt;&gt;</span>
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
    <span class="yarn-cmd">&lt;&lt;card flag_ukraine&gt;&gt;</span>
<span class="yarn-line">    Merci de nous aider !</span> <span class="yarn-meta">#line:02114ba </span>
<span class="yarn-line">    Notre capitale est Kiev.</span> <span class="yarn-meta">#line:01b1e6d </span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_ukraine")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_ukraine"&gt;&gt;</span>
<span class="yarn-line">        Merci ! C'est mon drapeau.</span> <span class="yarn-meta">#line:05de5ab </span>
<span class="yarn-line">        Pouvez-vous aider mon ami slovaque ?</span> <span class="yarn-meta">#line:0aa87ef </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_ukraine_FLAG&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $ukraine_completed = true&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;action slovakia_active&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_SL&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire.</span> <span class="yarn-meta">#line:0a4b7da </span>
<span class="yarn-line">        C'est jaune et bleu, sélectionnez-le et parlez-moi à nouveau !</span> <span class="yarn-meta">#line:089138a </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Non ! Notre drapeau est bleu et jaune.</span> <span class="yarn-meta">#line:0a94866 </span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $ukraine_met = true&gt;&gt;</span>
<span class="yarn-line">    Moien ! Je viens d’Ukraine !</span> <span class="yarn-meta">#line:07ea99a </span>
<span class="yarn-line">    On nous appelle le « grenier à blé » de l’Europe parce que nous produisons beaucoup de céréales !</span> <span class="yarn-meta">#line:0dbd4af </span>
    <span class="yarn-cmd">&lt;&lt;jump task_ukraine&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau de l'Ukraine.</span> <span class="yarn-meta">#line:07c148b #task:FIND_ukraine_FLAG</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_Flag_UK&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_ukraine_FLAG task_ukraine&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de l'Ukraine.</span> <span class="yarn-meta">#line:0805b90 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_ukraine"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_ukraine add&gt;&gt;</span>
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
    <span class="yarn-cmd">&lt;&lt;card flag_slovakia&gt;&gt;</span>
<span class="yarn-line">    Merci de nous aider !</span> <span class="yarn-meta">#line:06a6231 </span>
<span class="yarn-line">    La capitale de la Slovaquie est Bratislava !</span> <span class="yarn-meta">#line:0891aba</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item("flag_slovakia")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM == "flag_slovakia"&gt;&gt;</span>
<span class="yarn-line">        Merci d'avoir ramené mon drapeau !</span> <span class="yarn-meta">#line:0d453e9 </span>
<span class="yarn-line">        Retournez au départ et remportez votre victoire !</span> <span class="yarn-meta">#line:04bf6d1 </span>
        <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia remove&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;task_end FIND_slovakia_FLAG &gt;&gt;</span>
<span class="yarn-comment">        //&lt;&lt;camera_focus camera_NPC_PL&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $CURRENT_PROGRESS = $CURRENT_PROGRESS + 1&gt;&gt;</span>
        <span class="yarn-cmd">&lt;&lt;set $slovakia_completed = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Vous avez mon drapeau dans votre inventaire !</span> <span class="yarn-meta">#line:095b95b </span>
<span class="yarn-line">        Il est rouge, blanc et bleu avec un blason.</span> <span class="yarn-meta">#line:0b0fa74 </span>
<span class="yarn-line">        Sélectionnez-le et parlez-moi à nouveau !</span> <span class="yarn-meta">#line:0a051db </span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $CURRENT_ITEM != ""&gt;&gt;</span>
<span class="yarn-line">    Notre drapeau est blanc, rouge et bleu avec un cercle d'armoiries.</span> <span class="yarn-meta">#line:0af30a1 </span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $slovakia_met = true&gt;&gt;</span>
<span class="yarn-line">    Je viens de Slovaquie !</span> <span class="yarn-meta">#line:054e1b8 </span>
<span class="yarn-line">    Nous étions un pays avec la République tchèque.</span> <span class="yarn-meta">#line:0278f6a </span>
    <span class="yarn-cmd">&lt;&lt;jump task_slovakia&gt;&gt;</span>
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
<span class="yarn-line">Trouvez le drapeau slovaque.</span> <span class="yarn-meta">#line:04b6692 #task:FIND_slovakia_FLAG</span>
<span class="yarn-line">Il est blanc, rouge et bleu avec les armoiries.</span> <span class="yarn-meta">#line:0866ee1 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_NPC_SL&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_slovakia_FLAG npc_slovakia&gt;&gt;</span>

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
<span class="yarn-line">Drapeau de la Slovaquie.</span> <span class="yarn-meta">#line:0768ab7 </span>
<span class="yarn-cmd">&lt;&lt;if $CURRENT_ITEM != "flag_slovakia"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;inventory flag_slovakia add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-russia"></a>

## russia

<div class="yarn-node" data-title="russia">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// Russia</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">group: russia</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour, je suis Russie.</span> <span class="yarn-meta">#line:065c41c </span>
<span class="yarn-line">Ce n’est qu’une petite partie d’un grand pays.</span> <span class="yarn-meta">#line:0a4bae4 </span>
<span class="yarn-cmd">&lt;&lt;card flag_russia&gt;&gt;</span>

</code>
</pre>
</div>


