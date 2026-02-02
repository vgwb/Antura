---
title: Paris ! (fr_01) - Script
hide:
---

# Paris ! (fr_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-01-paris/23/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $BAGUETTE_STEP = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $met_tutor = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">Nous sommes à Paris, la capitale de la France.</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Aujourd'hui, nous allons explorer la Tour Eiffel</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">et Notre-Dame</span> <span class="yarn-meta">#line:start_1a</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">Mais d'abord, prenons une baguette !</span> <span class="yarn-meta">#line:start_1b</span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>
<span class="yarn-line">Êtes-vous prêts ? C'est parti !</span> <span class="yarn-meta">#line:start_2</span>

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Félicitations!</span> <span class="yarn-meta">#line:0d11596 </span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">Paris vous a plu ?</span> <span class="yarn-meta">#line:0d11596a</span>
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
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Veux-tu dessiner la Tour Eiffel ?</span> <span class="yarn-meta">#line:002620f</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $met_tutor == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $met_tutor = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">    Bonjour ! Êtes-vous déjà venu à Paris ?</span> <span class="yarn-meta">#line:talk_tutor_0</span>
<span class="yarn-line">    Oui!</span> <span class="yarn-meta">#line:talk_tutor_0b</span>
<span class="yarn-line">        Super ! Voyons si vous vous souvenez de ces endroits.</span> <span class="yarn-meta">#line:talk_tutor_0c</span>
<span class="yarn-line">    Non.</span> <span class="yarn-meta">#line:talk_tutor_0d</span>
<span class="yarn-line">        J'espère que vous viendrez ici un jour !</span> <span class="yarn-meta">#line:talk_tutor_0e</span>
<span class="yarn-line">    J'ai vu Antura aller chez le boulanger. Allons-y !</span> <span class="yarn-meta">#line:talk_tutor</span>
    <span class="yarn-cmd">&lt;&lt;area area_bakery&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_man&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-baker"></a>

## talk_baker

<div class="yarn-node" data-title="talk_baker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $BAGUETTE_STEP == 1&gt;&gt;</span>
<span class="yarn-line">    Super ! Nous avons de la farine.</span> <span class="yarn-meta">#line:baker_r1</span>
    <span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">    Maintenant, il me faut du sel.</span> <span class="yarn-meta">#line:06cccc0 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_notredame&gt;&gt;</span>
<span class="yarn-line">    Allez à Notre-Dame.</span> <span class="yarn-meta">#line:baker_r2 #task:go_notredame</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_notredame&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_notredame&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
<span class="yarn-line">    Du sel ! Bravo.</span> <span class="yarn-meta">#line:baker_r3</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_louvre&gt;&gt;</span>
<span class="yarn-line">    Antura prit l'eau et se rendit au Louvre.</span> <span class="yarn-meta">#line:baker_r4 #task:go_louvre</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_louvre&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
<span class="yarn-line">    Parfait ! Nous avons de l'eau.</span> <span class="yarn-meta">#line:baker_r5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_arc&gt;&gt;</span>
<span class="yarn-line">    Peut-être que la levure se trouve à l'Arc de Triomphe.</span> <span class="yarn-meta">#line:baker_r6 #task:go_arc</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_arc&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_arc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump baker_finish&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card person_baker&gt;&gt;</span>
<span class="yarn-line">    Bonjour ! Je suis le boulanger. Je fais du pain tous les jours.</span> <span class="yarn-meta">#line:baker_0</span>
    <span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">    Aujourd'hui, j'ai envie de faire une baguette...</span> <span class="yarn-meta">#line:baker_1</span>
<span class="yarn-line">    Mais j'ai perdu les ingrédients !</span> <span class="yarn-meta">#line:baker_2</span>
    <span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">    Un gros chien bleu a volé ma farine !</span> <span class="yarn-meta">#line:baker_3</span>
<span class="yarn-line">    Pouvez-vous m'aider à les trouver ?</span> <span class="yarn-meta">#line:baker_4</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_eiffell&gt;&gt;</span>
<span class="yarn-line">    Allez à la Tour Eiffel.</span> <span class="yarn-meta">#line:baker_5 #task:go_eiffell</span>
    <span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_eiffell&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-finish"></a>

## baker_finish

<div class="yarn-node" data-title="baker_finish">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé tous les ingrédients !</span> <span class="yarn-meta">#line:baker_finish_0</span>
<span class="yarn-line">Maintenant, je peux faire une baguette.</span> <span class="yarn-meta">#line:baker_finish_1</span>
<span class="yarn-line">Voyons si vous vous souvenez de ce que vous avez appris à Paris.</span> <span class="yarn-meta">#line:076b3e3 </span>
<span class="yarn-cmd">&lt;&lt;activity match_paris_final final_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-activity-done"></a>

## final_activity_done

<div class="yarn-node" data-title="final_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super ! Vous avez résolu l'énigme.</span> <span class="yarn-meta">#line:puzzle_done</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-flour"></a>

## baguette_flour

<div class="yarn-node" data-title="baguette_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé de la farine !</span> <span class="yarn-meta">#line:06022b0 </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 1&gt;&gt;</span>
<span class="yarn-line">Retournez chez le boulanger.</span> <span class="yarn-meta">#line:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-salt"></a>

## baguette_salt

<div class="yarn-node" data-title="baguette_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé du sel.</span> <span class="yarn-meta">#line:00f1d2f </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 2&gt;&gt;</span>
<span class="yarn-line">Retournez chez le boulanger.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker </span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-water"></a>

## baguette_water

<div class="yarn-node" data-title="baguette_water">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_water&gt;&gt;</span>
<span class="yarn-line">Ceci est de l'eau.</span> <span class="yarn-meta">#line:0c4d1f6</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 3&gt;&gt;</span>
<span class="yarn-line">Retournez chez le boulanger.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-yeast"></a>

## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_yeast&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé de la levure !</span> <span class="yarn-meta">#line:025865d</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 4&gt;&gt;</span>
<span class="yarn-line">Retournez chez le boulanger.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-ticket"></a>

## talk_eiffell_ticket

<div class="yarn-node" data-title="talk_eiffell_ticket">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">////////// EIFFEL TOWER: pay 5 coins -&gt; roof -&gt; chest flour</span>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-line">Bonjour. Que désirez-vous ?</span> <span class="yarn-meta">#line:09e454b </span>
<span class="yarn-line">Un billet pour monter en haut de la Tour Eiffel.</span> <span class="yarn-meta">#line:0141851 </span>
    <span class="yarn-cmd">&lt;&lt;if HasCompletedTask("collect_coins")&gt;&gt;</span>
<span class="yarn-line">        Sélectionnez le montant à payer.</span> <span class="yarn-meta">#line:0f44ea7 </span>
        <span class="yarn-cmd">&lt;&lt;activity money_elevator ticket_payment_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "collect_coins"&gt;&gt;</span>
<span class="yarn-line">        Le billet coûte 5 pièces</span> <span class="yarn-meta">#line:069cbb3</span>
<span class="yarn-line">        Regardez autour de vous et ramassez les pièces.</span> <span class="yarn-meta">#shadow:0097a65</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        Le billet coûte 5 pièces</span> <span class="yarn-meta">#shadow:069cbb3</span>
<span class="yarn-line">        Regardez autour de vous et ramassez les pièces.</span> <span class="yarn-meta">#line:0097a65 #task:collect_coins</span>
        <span class="yarn-cmd">&lt;&lt;task_start collect_coins coins_collected&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">Une baguette.</span> <span class="yarn-meta">#line:03dc852 </span>
<span class="yarn-line">   Il y a une boulangerie près d'ici. Mais elle ouvre tard.</span> <span class="yarn-meta">#line:0cbdcce </span>
<span class="yarn-line">Juste pour regarder autour de soi.</span> <span class="yarn-meta">#line:0718e4a </span>
<span class="yarn-line">   Profitez de votre visite !</span> <span class="yarn-meta">#line:006fcf2 </span>

</code>
</pre>
</div>

<a id="ys-node-ticket-payment-done"></a>

## ticket_payment_done

<div class="yarn-node" data-title="ticket_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_ticket&gt;&gt;</span>
<span class="yarn-line">Voici votre billet.</span> <span class="yarn-meta">#line:04e74ad </span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-line">J'ai vu Antura monter au sommet de la tour.</span> <span class="yarn-meta">#line:089abda</span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_roof&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">Prenez l'ascenseur ou utilisez les escaliers !</span> <span class="yarn-meta">#line:0585a5e </span>

</code>
</pre>
</div>

<a id="ys-node-coins-collected"></a>

## coins_collected

<div class="yarn-node" data-title="coins_collected">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vous avez maintenant assez de pièces pour acheter le billet.</span> <span class="yarn-meta">#line:0ba42cd </span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-guide"></a>

## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour. Que voulez-vous savoir ?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">Qu'est-ce que la tour Eiffel ?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    Une haute tour en fer, d'environ 300 mètres de haut.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    C'est un symbole emblématique de Paris !</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Où sommes-nous?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    Nous sommes à Paris.</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Cet endroit existe-t-il vraiment ?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Oui ! Pourquoi me posez-vous cette question ?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    Eh bien… on dirait un jeu vidéo.</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Rien. Au revoir.</span> <span class="yarn-meta">#line:0fe0732 #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-roof"></a>

## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Bienvenue au sommet de la Tour Eiffel !</span> <span class="yarn-meta">#line:0da46e8 </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">La tour Eiffel mesure 300 mètres de haut.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;card gustave_eiffel&gt;&gt;</span>
<span class="yarn-line">Gustave Eiffel l'a construit en 1887.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;card iron_material&gt;&gt;</span>
<span class="yarn-line">Il est en fer !</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-cmd">&lt;&lt;card worlds_fair_1889&gt;&gt;</span>
<span class="yarn-line">Il a été construit il y a longtemps pour une grande foire.</span> <span class="yarn-meta">#line:0d59ade_fair</span>
&lt;&lt;if GetActivityResult("memory_eiffell") &lt; 1 &gt;&gt;
<span class="yarn-line">    Pour ouvrir le coffre, résolvez l'énigme !</span> <span class="yarn-meta">#line:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_eiffell eiffell_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eiffell-activity-done"></a>

## eiffell_activity_done

<div class="yarn-node" data-title="eiffell_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super ! Vous avez résolu l'énigme.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Le coffre est maintenant déverrouillé.</span> <span class="yarn-meta">#line:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_flour true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame"></a>

## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 1&gt;&gt;
<span class="yarn-line">    Revenez plus tard.</span> <span class="yarn-meta">#line:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">    Voici la cathédrale Notre-Dame.</span> <span class="yarn-meta">#line:06f3fa2 </span>
    <span class="yarn-cmd">&lt;&lt;card cathedral&gt;&gt;</span>
<span class="yarn-line">    Cathédrale signifie une très grande église.</span> <span class="yarn-meta">#line:06f3fa2_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;card church&gt;&gt;</span>
<span class="yarn-line">    Une église est un lieu où les gens vont prier.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>
<span class="yarn-line">    C'est une très vieille église. Elle a été construite il y a très longtemps.</span> <span class="yarn-meta">#line:0ac5a72 </span>
<span class="yarn-line">    Monte sur le toit avec ce portail !</span> <span class="yarn-meta">#line:083dfcc</span>
    <span class="yarn-cmd">&lt;&lt;action activate_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Nous avons déjà résolu cette partie.</span> <span class="yarn-meta">#line:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame-roof"></a>

## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris_fire&gt;&gt;</span>
<span class="yarn-line">Il y a eu un gros incendie en 2019, mais nous l'avons réparé.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">Pour ouvrir le coffre, résolvez l'énigme !</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
<span class="yarn-cmd">&lt;&lt;activity memory_notredame notredame_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-notredame-activity-done"></a>

## notredame_activity_done

<div class="yarn-node" data-title="notredame_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super ! Vous avez résolu l'énigme.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Le coffre est maintenant déverrouillé.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_salt true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_salt&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">Regardez cette statue ! Elle n'est pas effrayante ?</span> <span class="yarn-meta">#line:0f7f9d8 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre"></a>

## talk_louvre

<div class="yarn-node" data-title="talk_louvre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// LOUVRE: puzzle -&gt; chest unlock -&gt; water</span>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 2&gt;&gt;
<span class="yarn-line">    Revenez plus tard.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
<span class="yarn-line">    Voici le Louvre, un musée célèbre.</span> <span class="yarn-meta">#line:louvre_0</span>
<span class="yarn-line">    Pour ouvrir le coffre, résolvez l'énigme !</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_louvre louvre_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Nous avons déjà résolu cette partie.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-activity-done"></a>

## louvre_activity_done

<div class="yarn-node" data-title="louvre_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super ! Vous avez résolu l'énigme.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Le coffre est maintenant déverrouillé.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_water true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-arc"></a>

## talk_arc

<div class="yarn-node" data-title="talk_arc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// ARC DE TRIOMPHE: puzzle -&gt; chest unlock -&gt; yeast</span>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 3&gt;&gt;
<span class="yarn-line">    Revenez plus tard.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card arc_de_triomphe&gt;&gt;</span>
<span class="yarn-line">    Voici l'Arc de Triomphe.</span> <span class="yarn-meta">#line:arc_0</span>
<span class="yarn-line">    Il rend hommage aux personnes qui ont combattu pour la France.</span> <span class="yarn-meta">#line:arc_0a</span>
<span class="yarn-line">    Pour ouvrir le coffre, résolvez l'énigme !</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_arc arc_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Nous avons déjà résolu cette partie.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-arc-activity-done"></a>

## arc_activity_done

<div class="yarn-node" data-title="arc_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Super ! Vous avez résolu l'énigme.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">Le coffre est maintenant déverrouillé.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-line">Il se trouve sur le toit. Utilisez le téléporteur pour y aller.</span> <span class="yarn-meta">#line:0d46853 </span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_yeast true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action activate_arc_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_yeast&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-eiffell-tourist"></a>

## spawned_eiffell_tourist

<div class="yarn-node" data-title="spawned_eiffell_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'aimerais monter en haut de la tour Eiffel.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">Il vous faut un billet pour monter.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">Il y eut une grande foire à Paris en 1889.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    C'était pour célébrer un anniversaire important pour la France.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    La tour Eiffel a été construite pour cette grande fête.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">J'adore Paris !</span> <span class="yarn-meta">#line:0bda18a </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-man"></a>

## spawned_man

<div class="yarn-node" data-title="spawned_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Avez-vous des questions ?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Avez-vous vu Antura ?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    Non. Qui est Antura ?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">Que fais-tu?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    Je vais acheter du pain à la boulangerie.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">D'où viens-tu?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    Je ne suis pas né dans ce pays.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">Au revoir</span> <span class="yarn-meta">#line:0ee51fc #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour !</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ça va ?</span> <span class="yarn-meta">#line:04986a3 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-roof"></a>

## npc_notredame_roof

<div class="yarn-node" data-title="npc_notredame_roof">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame_roof</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Du toit, on peut voir une grande partie de Paris.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Des créatures de pierre appelées gargouilles sont assises ici.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">Ces bras de pierre contribuent à soutenir les murs.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2b #card:flying_buttress</span>
<span class="yarn-line">La grande fenêtre ronde s'appelle une rosace.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2c #card:rose_window</span>

</code>
</pre>
</div>

<a id="ys-node-npc-eiffell-elevator"></a>

## npc_eiffell_elevator

<div class="yarn-node" data-title="npc_eiffell_elevator">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_elevators&gt;&gt;</span>
<span class="yarn-line">L'ascenseur permet aux gens de monter dans la tour.</span> <span class="yarn-meta">#line:fr01_eiffel_elevator_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-paris-region"></a>

## npc_paris_region

<div class="yarn-node" data-title="npc_paris_region">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Paris se trouve dans une région appelée Île-de-France.</span> <span class="yarn-meta">#line:fr01_region_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bakery"></a>

## npc_bakery

<div class="yarn-node" data-title="npc_bakery">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: bakery</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'odeur du pain frais rend les gens heureux.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">Une baguette est composée de farine, d'eau, de levure et de sel.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">La levure permet au pain de lever.</span> <span class="yarn-meta">#line:fr01_bakery_2a</span>
<span class="yarn-line">Les boulangers se lèvent très tôt pour commencer à faire du pain.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


