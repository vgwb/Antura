---
title: Fleuve Oder (pl_03) - Script
hide:
---

# Fleuve Oder (pl_03) - Script
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
<span class="yarn-line">Nous sommes à Wrocław, la « Ville aux cent ponts ».</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-line">Aujourd'hui, nous allons explorer la rivière, les ponts et les bateaux.</span> <span class="yarn-meta">#line:start_2</span>
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
<span class="yarn-line">Bien joué!</span> <span class="yarn-meta">#line:02a257c </span>
<span class="yarn-cmd">&lt;&lt;card bridge&gt;&gt;</span>
<span class="yarn-line">Nous avons découvert différents types de ponts.</span> <span class="yarn-meta">#line:end_1</span>
<span class="yarn-cmd">&lt;&lt;card boat&gt;&gt;</span>
<span class="yarn-line">Nous avons découvert différents types de bateaux.</span> <span class="yarn-meta">#line:end_2</span>
<span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">Nous avons exploré la rivière ODRA.</span> <span class="yarn-meta">#line:end_3</span>
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
<span class="yarn-line">Dessinez une carte simple de votre voyage.</span> <span class="yarn-meta">#line:0265d07 </span>
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
<span class="yarn-line">    Dziękuję! Vous avez trouvé toutes mes photos !</span> <span class="yarn-meta">#line:prot_1</span>
<span class="yarn-line">    Je peux maintenant poursuivre la visite. Une dernière question…</span> <span class="yarn-meta">#line:prot_2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("collect_cards")&gt;&gt;</span>
<span class="yarn-line">    Vous êtes courageux ! Mais les photos de nos merveilles fluviales me manquent toujours.</span> <span class="yarn-meta">#line:006ab27 </span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">    Cherchez les coffres ! Les gens au bord de la rivière les surveillent.</span> <span class="yarn-meta">#line:prot_4 </span>
<span class="yarn-line">    Si vous ne les trouvez pas, utilisez la carte !</span> <span class="yarn-meta">#line:0bfcb05 </span>
    <span class="yarn-cmd">&lt;&lt;task_start find_photos task_find_photos_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "find_photos" or GetCurrentTask() == "collect_cards"&gt;&gt;</span>
<span class="yarn-line">    Tu te débrouilles bien !</span> <span class="yarn-meta">#line:03ea48b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Au secours ! Antura a pris mon guide de Wrocław pour un os !</span> <span class="yarn-meta">#line:prot_5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_intro&gt;&gt;</span>
<span class="yarn-line">    Les pages sont éparpillées. Pouvez-vous les retrouver ?</span> <span class="yarn-meta">#line:prot_6 #task:collect_cards</span>
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
<span class="yarn-line">Vous avez trouvé toutes les photos !</span> <span class="yarn-meta">#line:found_photos</span>
<span class="yarn-line">Retournez parler au guide.</span> <span class="yarn-meta">#line:go_back #task:go_back </span>
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
<span class="yarn-line">Vous avez trouvé toutes les photos !</span> <span class="yarn-meta">#shadow:found_photos</span>
<span class="yarn-line">Retournez parler au guide.</span> <span class="yarn-meta">#shadow:go_back #task:go_back</span>
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
<span class="yarn-line">L'Oder est un long fleuve. Vous souvenez-vous où finit toute cette eau ?</span> <span class="yarn-meta">#line:quiz1_intro</span>
<span class="yarn-line">La mer Baltique</span> <span class="yarn-meta">#line:quiz_a1</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    Tak ! Exact ! L'Oder coule vers le nord à travers la Pologne et se jette dans la mer Baltique.</span> <span class="yarn-meta">#line:quiz1_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz_2&gt;&gt;</span>
<span class="yarn-line">La mer Méditerranée</span> <span class="yarn-meta">#line:quiz_a2</span>
<span class="yarn-line">    Hmm... c'est beaucoup trop au sud !</span> <span class="yarn-meta">#line:quiz1_fail1</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">La mer Noire</span> <span class="yarn-meta">#line:quiz_a3</span>
<span class="yarn-line">    Pas tout à fait. L'Odra coule vers le nord, et non vers le sud !</span> <span class="yarn-meta">#line:quiz1_fail2</span>
    <span class="yarn-cmd">&lt;&lt;jump final_quiz&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#line:dontknow</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span> 
<span class="yarn-line">    Pas de souci ! Regardez la ligne bleue sur la carte.</span> <span class="yarn-meta">#line:quiz1_hint</span>
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
<span class="yarn-line">L'Oder est-elle la *plus longue* rivière de Pologne ?</span> <span class="yarn-meta">#line:quiz2_intro</span>
<span class="yarn-line">Non, la Wisła est plus longue</span> <span class="yarn-meta">#line:quiz2_a1</span>
    <span class="yarn-cmd">&lt;&lt;card place_vistula_river&gt;&gt;</span>
<span class="yarn-line">    Parfait ! La Vistule est la première, l'Odra est la deuxième plus longue.</span> <span class="yarn-meta">#line:quiz2_ok</span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-line">Oui, c'est le plus long</span> <span class="yarn-meta">#line:quiz2_a2</span>
<span class="yarn-line">    Elle est très grande, mais il existe une rivière encore plus longue.</span> <span class="yarn-meta">#line:quiz2_fail</span>
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
<span class="yarn-line">Regardez le grand panneau bleu près du pont.</span> <span class="yarn-meta">#line:sign_1</span>
<span class="yarn-line">Que nous indiquent les lignes blanches ondulées ?</span> <span class="yarn-meta">#line:sign_3</span>
<span class="yarn-line">Il y a une rivière qui coule ici</span> <span class="yarn-meta">#line:sign_4</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_river_sign&gt;&gt;</span>
<span class="yarn-line">    Tak ! Ces vagues bleues sont le signe universel d'une rivière.</span> <span class="yarn-meta">#line:sign_5</span>
<span class="yarn-line">Le pont bouge</span> <span class="yarn-meta">#line:sign_6</span>
<span class="yarn-line">    Non. Réessayez.</span> <span class="yarn-meta">#line:try_again </span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#line:dont_know #highlight</span>
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
<span class="yarn-line">    Antura a recouvert le panneau de boue ! Essuyez-le pour voir les vagues.</span> <span class="yarn-meta">#line:ch_sign1</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity clean_river_sign chest_river_sign&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_sign&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card river_sign&gt;&gt;</span>
<span class="yarn-line">    Excellent travail ! Désormais, vous saurez toujours quand vous traversez une rivière en Europe.</span> <span class="yarn-meta">#line:ch_sign2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $river_sign = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $river_sign == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#line:chest_locked </span>
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
<span class="yarn-line">L'Oder est le deuxième plus long fleuve de Pologne.</span> <span class="yarn-meta">#line:map_1</span>
&lt;&lt;if $map_odra &lt; 10&gt;&gt;
<span class="yarn-line">L'Odra se jette-t-elle dans les montagnes ou dans la mer ?</span> <span class="yarn-meta">#line:map_2</span>
<span class="yarn-line">La mer Baltique</span> <span class="yarn-meta">#line:map_3</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_odra_map&gt;&gt;</span>
<span class="yarn-line">    Exactement ! Il coule jusqu'au nord.</span> <span class="yarn-meta">#line:map_4</span>
<span class="yarn-line">La mer Méditerranée</span> <span class="yarn-meta">#line:map_5</span>
<span class="yarn-line">    Non. Réessayez.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_odra_map&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight </span>
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
<span class="yarn-line">    Prouvez que vous savez où coule la rivière !</span> <span class="yarn-meta">#line:ch_map1</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_odra_facts chest_odra_river_map&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $map_odra == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_map&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card odra_river_map&gt;&gt;</span>
<span class="yarn-line">    La carte est de retour ! Elle montre l'Oder se jetant dans la mer Baltique.</span> <span class="yarn-meta">#line:ch_map2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $map_odra = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Voici le pont Tumski. Il mène à la partie la plus ancienne de la ville.</span> <span class="yarn-meta">#line:tum_1</span>
<span class="yarn-line">Chaque soir, un homme allume à la main les 102 lanternes à gaz qui se trouvent ici !</span> <span class="yarn-meta">#line:tum_2</span>
&lt;&lt;if $bridge_tumski &lt; 10&gt;&gt;
<span class="yarn-line">Que suspendent les couples à ce pont pour porter chance et amour ?</span> <span class="yarn-meta">#line:tum_3</span>
<span class="yarn-line">Cadenas</span> <span class="yarn-meta">#line:tum_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Oui ! Bien qu'ils soient très lourds et qu'ils soient enlevés !</span> <span class="yarn-meta">#line:tum_5</span>
<span class="yarn-line">Chaussettes mouillées</span> <span class="yarn-meta">#line:tum_6</span>
<span class="yarn-line">    Ce ne serait pas très romantique ! Réessayez.</span> <span class="yarn-meta">#line:fail_tum</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_tumski_bridge&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Nettoyez la rouille de ce vieux pont en fer !</span> <span class="yarn-meta">#line:ch_tum1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity cleancanvas odra_footbridge chest_tumski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tumski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card tumski_bridge&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_tumski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_tumski == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Le pont Rędziński est le plus haut et le plus long de Pologne.</span> <span class="yarn-meta">#line:redz_1</span>
<span class="yarn-line">Elle mesure 122 mètres de haut. Plus haute que la cathédrale !</span> <span class="yarn-meta">#line:redz_2</span>
&lt;&lt;if $bridge_redzinski &lt; 10&gt;&gt;
<span class="yarn-line">Qu'est-ce qui soutient cet immense pont ?</span> <span class="yarn-meta">#line:redz_3</span>
<span class="yarn-line">câbles en acier</span> <span class="yarn-meta">#line:redz_4</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Exactement ! De solides câbles soutiennent ce pont géant. Les voitures l'empruntent pour circuler en ville.</span> <span class="yarn-meta">#line:redz_5</span>
<span class="yarn-line">Aimants et magie</span> <span class="yarn-meta">#line:redz_6</span>
<span class="yarn-line">    Ça a l'air magique, mais c'est en fait de l'ingénierie !</span> <span class="yarn-meta">#line:fail_redz</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Reconstruisez le plus haut pylône de Wrocław !</span> <span class="yarn-meta">#line:ch_redz1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_pont chest_redzinski_bridge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_redzinski&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card redzinski_bridge&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_redzinski = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_redzinski == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Des trains traversent l'Odra à Wrocław depuis plus de 150 ans.</span> <span class="yarn-meta">#line:train_1</span>
&lt;&lt;if $bridge_train &lt; 10&gt;&gt;
<span class="yarn-line">Pourquoi les ponts ferroviaires sont-ils construits en acier si lourd ?</span> <span class="yarn-meta">#line:train_2</span>
<span class="yarn-line">Parce que les trains sont très lourds</span> <span class="yarn-meta">#line:train_3</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_bridge_train&gt;&gt;</span>
<span class="yarn-line">    Oui ! Il doit être suffisamment solide pour supporter le poids des trains.</span> <span class="yarn-meta">#line:train_4</span>
<span class="yarn-line">Faire du bruit</span> <span class="yarn-meta">#line:train_5</span>
<span class="yarn-line">    Ils sont bruyants, mais ce n'est pas la raison !</span> <span class="yarn-meta">#line:fail_train</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_train_bridge&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Alignez les lourdes caisses avec les rails du train !</span> <span class="yarn-meta">#line:ch_train1</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_bridges chest_bridge_train&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_train&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card train_bridge&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $bridge_train = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $bridge_train == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">À Wrocław, certains appellent la rivière leur « rue principale ».</span> <span class="yarn-meta">#line:house_1</span>
<span class="yarn-line">Même les nains rêveraient d'une maison flottante !</span> <span class="yarn-meta">#line:house_2</span>
&lt;&lt;if $boat_house &lt; 10&gt;&gt;
<span class="yarn-line">Si vous vivez sur une péniche, qu'est-ce qui vous fait office de jardin ?</span> <span class="yarn-meta">#line:house_3</span>
<span class="yarn-line">La rivière Odra</span> <span class="yarn-meta">#line:house_4</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_houseboat&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#shadow:chest_opens </span>
<span class="yarn-line">Une forêt sur un toit</span> <span class="yarn-meta">#line:house_6</span>
<span class="yarn-line">    Non. Réessayez.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_houseboat&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Réparez les fenêtres de la maison flottante !</span> <span class="yarn-meta">#line:ch_house1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_boat_house chest_houseboat&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_houseboat&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card houseboat&gt;&gt;</span>
<span class="yarn-line">    Une maison confortable sur les rives de l'Odra ! Photo prise sur place.</span> <span class="yarn-meta">#line:ch_house2</span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_house = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_house == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Des bateaux touristiques emmènent les visiteurs visiter le zoo et la cathédrale.</span> <span class="yarn-meta">#line:tour_1</span>
&lt;&lt;if $boat_tourist &lt; 10&gt;&gt;
<span class="yarn-line">De quoi les gens qui se trouvent sur ces bateaux profitent-ils pour admirer les paysages ?</span> <span class="yarn-meta">#line:tour_2</span>
<span class="yarn-line">Leurs yeux et leurs caméras</span> <span class="yarn-meta">#line:tour_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_boat_people&gt;&gt;</span>
<span class="yarn-line">    Oui ! Souriez pour la photo !</span> <span class="yarn-meta">#line:tour_4</span>
<span class="yarn-line">Un périscope</span> <span class="yarn-meta">#line:tour_5</span>
<span class="yarn-line">    Nous ne sommes pas encore sous l'eau ! Veuillez réessayer.</span> <span class="yarn-meta">#line:fail_tour</span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_people&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Trouvez les touristes cachés sur le pont !</span> <span class="yarn-meta">#line:ch_tour1</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_people&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_tourist&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card boat_people&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#shadow:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_tourist = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_tourist == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Des barges (Barki) ont transporté du charbon et du sable sur l'Odra pendant des siècles.</span> <span class="yarn-meta">#line:barge_1</span>
&lt;&lt;if $boat_barge &lt; 10&gt;&gt;
<span class="yarn-line">Une péniche est très plate. Pourquoi ?</span> <span class="yarn-meta">#line:barge_2</span>
<span class="yarn-line">Transporter des objets lourds même lorsque l'eau n'est pas profonde.</span> <span class="yarn-meta">#line:barge_3</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_barge&gt;&gt;</span>
<span class="yarn-line">    Exactement ! C'est un camion qui flotte.</span> <span class="yarn-meta">#line:barge_4</span>
<span class="yarn-line">Ainsi, il peut se cacher des nains.</span> <span class="yarn-meta">#line:barge_5</span>
<span class="yarn-line">    Non. Réessayez.</span> <span class="yarn-meta">#shadow:try_again </span>
    <span class="yarn-cmd">&lt;&lt;jump npc_boat_barge&gt;&gt;</span>
<span class="yarn-line">Je ne sais pas</span> <span class="yarn-meta">#shadow:dont_know #highlight</span>
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
<span class="yarn-line">    Jouez à un mini-jeu pour ouvrir le coffre !</span> <span class="yarn-meta">#line:chest_minigame</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_boats chest_boat_barge&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest_boat_barge&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card barge&gt;&gt;</span>
<span class="yarn-line">    Le coffre s'ouvre. Vous trouvez une photo !</span> <span class="yarn-meta">#line:chest_opens </span>
    <span class="yarn-cmd">&lt;&lt;collect photo&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $boat_barge = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $boat_barge == 10&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Le coffre est verrouillé.</span> <span class="yarn-meta">#shadow:chest_locked </span>
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
<span class="yarn-line">Je veux faire du kayak.</span> <span class="yarn-meta">#line:0f36b7f #card:kayak</span>
<span class="yarn-line">Les kayaks sont parfaits pour explorer la nature !</span> <span class="yarn-meta">#line:kayak_2 #card:kayak</span>
<span class="yarn-line">Le paddle est amusant et un bon exercice !</span> <span class="yarn-meta">#line:kayak_3</span>

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
<span class="yarn-line">Il y a tellement de ponts dans cette ville.</span> <span class="yarn-meta">#line:0577d80 </span>
<span class="yarn-line">Wroclaw est vraiment magnifique.</span> <span class="yarn-meta">#line:089ea37 </span>
<span class="yarn-line">J'adore les pierogis !</span> <span class="yarn-meta">#line:07ff8c5 </span>
<span class="yarn-line">L'île de la Cathédrale est magique la nuit.</span> <span class="yarn-meta">#line:tourist_4</span>

</code>
</pre>
</div>


