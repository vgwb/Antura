---
title: Découvrir Varsovie (pl_01) - Script
hide:
---

# Découvrir Varsovie (pl_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-01-discover-warszawa/32/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1983275331#gid=1983275331)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_01%20Warsaw/PL_01%20Warsaw%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_01 | Warsaw</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $current_place = "center"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_1_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_2_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_3_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_4_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_5_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_6_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chest_7_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $quiz_score = 0&gt;&gt;</span>

<span class="yarn-line">Bienvenue à Varsovie, la capitale de la Pologne !</span> <span class="yarn-meta">#line:05d7d48 </span>
<span class="yarn-cmd">&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_intro&gt;&gt;</span>

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
<span class="yarn-line">Excellent travail ! Vous avez reconstitué l'épée de la sirène !</span> <span class="yarn-meta">#line:04012cb </span>
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
<span class="yarn-line">La sirène t'a donné le premier morceau.</span> <span class="yarn-meta">#line:071f088 </span>
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa&gt;&gt;</span>
<span class="yarn-line">Wars et Sawa vous ont donné la deuxième pièce.</span> <span class="yarn-meta">#line:02100a1 </span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
<span class="yarn-line">Le roi Sigismond vous a donné le troisième morceau.</span> <span class="yarn-meta">#line:0bc4b13 </span>
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
<span class="yarn-line">Au Parlement, vous avez trouvé la quatrième pièce.</span> <span class="yarn-meta">#line:0127674 </span>
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
<span class="yarn-line">Chopin vous a donné le cinquième morceau.</span> <span class="yarn-meta">#line:0fa0a42 </span>
<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
<span class="yarn-line">Au palais, vous avez aidé Maria et trouvé la sixième pièce.</span> <span class="yarn-meta">#line:06dd6d4 </span>
<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
<span class="yarn-line">Au stade, vous avez récupéré la dernière pièce.</span> <span class="yarn-meta">#line:0ae654b </span>
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
<span class="yarn-line">Dessine ton endroit préféré à Varsovie !</span> <span class="yarn-meta">#line:0daaef6 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-quiz-final"></a>

## quiz_final

<div class="yarn-node" data-title="quiz_final">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $quiz_score = 0&gt;&gt;</span>
<span class="yarn-line">Quelles sont les couleurs du drapeau polonais ?</span> <span class="yarn-meta">#line:04e2b98 </span>
<span class="yarn-choice">-&gt; Bleu et rouge.</span> <span class="yarn-meta">#line:0085ea3 </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#line:are_you_sure</span>
<span class="yarn-choice">-&gt; Blanc et rouge.</span> <span class="yarn-meta">#line:03df89b </span>
	<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
<span class="yarn-line">	Correct.</span> <span class="yarn-meta">#line:correct</span>
<span class="yarn-choice">-&gt; Vert et blanc.</span> <span class="yarn-meta">#line:0c11fbb </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#shadow:are_you_sure</span>
<span class="yarn-line">Qui était Chopin ?</span> <span class="yarn-meta">#line:02d07ad </span>
<span class="yarn-choice">-&gt; Un joueur de football.</span> <span class="yarn-meta">#line:0689209 </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#shadow:are_you_sure</span>
<span class="yarn-choice">-&gt; Un roi.</span> <span class="yarn-meta">#line:08fc511 </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#shadow:are_you_sure</span>
<span class="yarn-choice">-&gt; Un compositeur.</span> <span class="yarn-meta">#line:04d5c5f </span>
	<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
<span class="yarn-line">	Correct.</span> <span class="yarn-meta">#shadow:correct</span>
<span class="yarn-line">Qu'est-ce que la rivière Wisła ?</span> <span class="yarn-meta">#line:0e17c6d </span>
<span class="yarn-choice">-&gt; Un palais.</span> <span class="yarn-meta">#line:002ec2d </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#shadow:are_you_sure</span>
<span class="yarn-choice">-&gt; Un stade.</span> <span class="yarn-meta">#line:0b7ff07 </span>
<span class="yarn-line">	Es-tu sûr?</span> <span class="yarn-meta">#shadow:are_you_sure</span>
<span class="yarn-choice">-&gt; Une rivière.</span> <span class="yarn-meta">#line:038bb24 </span>
	<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
<span class="yarn-line">	Correct.</span> <span class="yarn-meta">#shadow:correct</span>
&lt;&lt;if $quiz_score &gt;= 3&gt;&gt;
<span class="yarn-line">	Bravo. Vous en savez assez sur Varsovie.</span> <span class="yarn-meta">#line:0c03be3 </span>
	<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Vous n'avez pas répondu correctement à toutes les questions. Réessayez !</span> <span class="yarn-meta">#line:045a4dd </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-teleport"></a>

## teleport

<div class="yarn-node" data-title="teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Où veux-tu aller ?</span> <span class="yarn-meta">#line:where_to_go</span>
-&gt; Center <span class="yarn-cmd">&lt;&lt;if $current_place == "stadium" or $current_place == "parliament"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_center</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_center&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_center&gt;&gt;</span>
-&gt; Parliament <span class="yarn-cmd">&lt;&lt;if $current_place == "center" or $current_place == "chopin"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_parliament</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_parliament&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_parliament&gt;&gt;</span>
-&gt; Chopin <span class="yarn-cmd">&lt;&lt;if $current_place == "parliament" or $current_place == "palace_culture"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_chopin</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_chopin&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_chopin&gt;&gt;</span>
-&gt; Culture Palace <span class="yarn-cmd">&lt;&lt;if $current_place == "chopin" or $current_place == "stadium"&gt;&gt;</span>  <span class="yarn-meta">#line:tram_palace</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_palace_culture&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_palace_culture&gt;&gt;</span>
-&gt; Stadium <span class="yarn-cmd">&lt;&lt;if $current_place == "palace_culture" &gt;&gt;</span>  <span class="yarn-meta">#line:tram_stadium</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_stadium&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;wait 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump intro_area_stadium&gt;&gt;</span>
<span class="yarn-choice">-&gt; Restez ici</span> <span class="yarn-meta">#line:stay_here #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-npc-0"></a>

## npc_0

<div class="yarn-node" data-title="npc_0">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 0 | INTRO</span>
<span class="yarn-header-dim">group: intro</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour. Savez-vous où nous sommes ?</span> <span class="yarn-meta">#line:0ead8c8 </span>
<span class="yarn-choice">-&gt; À Torun</span> <span class="yarn-meta">#line:0871944 </span>
<span class="yarn-choice">-&gt; À Varsovie</span> <span class="yarn-meta">#line:036987c </span>
<span class="yarn-line">	Oui ! Écoutez attentivement et lisez toujours les dialogues dans ce jeu.</span> <span class="yarn-meta">#line:0221e36 </span>
	<span class="yarn-cmd">&lt;&lt;camera_focus camera_mermaid&gt;&gt;</span>
<span class="yarn-line">	Nous avons un problème à résoudre. Allez voir la Sirène !</span> <span class="yarn-meta">#line:081d27b </span>
	<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target npc_1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;area area_center&gt;&gt;</span>
<span class="yarn-choice">-&gt; À Wroclaw</span> <span class="yarn-meta">#line:0ab7f3f </span>
<span class="yarn-choice">-&gt; Je ne sais pas</span> <span class="yarn-meta">#line:dontknow </span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-center"></a>

## intro_area_center

<div class="yarn-node" data-title="intro_area_center">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CENTER</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "center"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_center&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Nous sommes en centre-ville.</span> <span class="yarn-meta">#line:0dc948e </span>

</code>
</pre>
</div>

<a id="ys-node-npc-1"></a>

## npc_1

<div class="yarn-node" data-title="npc_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 1 | MERMAID OF WARSAW</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if item_count("mermaids_sword") &gt;= 7&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;card mermaids_sword&gt;&gt;</span>
<span class="yarn-line">	Tu as rassemblé tous les morceaux de l'épée ! Maintenant, je peux la reconstituer !</span> <span class="yarn-meta">#line:003290a </span>
<span class="yarn-line">	La sirène est un symbole de Varsovie et protège la ville.</span> <span class="yarn-meta">#line:0311f38 </span>
<span class="yarn-line">	Voyons maintenant ce dont vous vous souvenez de cette aventure.</span> <span class="yarn-meta">#line:0df9245 </span>
	<span class="yarn-cmd">&lt;&lt;jump quiz_final&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $chest_1_done&gt;&gt;</span>
<span class="yarn-line">	Explorez Varsovie et retrouvez tous les morceaux de l'épée !</span> <span class="yarn-meta">#line:explore</span>
    <span class="yarn-cmd">&lt;&lt;jump next_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_1"&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump info_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_1")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target chest_1&gt;&gt;</span>
<span class="yarn-line">    Ouvrez le coffre !</span> <span class="yarn-meta">#line:010f57b </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card mermaids_sword&gt;&gt;</span>
<span class="yarn-line">	Antura a brisé l'épée en 7 morceaux et a enfermé chacun d'eux dans un coffre à travers Varsovie !</span> <span class="yarn-meta">#line:06660d7</span>
<span class="yarn-line">	Parlez à tous les rouges.</span> <span class="yarn-meta">#line:talk_red_yellow #task:task_1</span>
<span class="yarn-line">	Ils vous expliqueront comment les gens se déplacent en ville.</span> <span class="yarn-meta">#line:064abc4 </span>
<span class="yarn-line">	Et récupérez toutes les clés pour ouvrir le coffre !</span> <span class="yarn-meta">#line:get_all_keys</span>
    <span class="yarn-cmd">&lt;&lt;task_start task_1 task_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-1"></a>

## info_1

<div class="yarn-node" data-title="info_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
<span class="yarn-line">La sirène de Varsovie s'appelle la Syrenka.</span> <span class="yarn-meta">#line:05be7da </span>
<span class="yarn-line">Elle est mi-femme, mi-poisson, et porte une épée et un bouclier.</span> <span class="yarn-meta">#line:08618f7 </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1-1"></a>

## ll_key_1_1

<div class="yarn-node" data-title="ll_key_1_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
<span class="yarn-line">Je prends généralement le tram.</span> <span class="yarn-meta">#line:03375d9 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#line:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1-2"></a>

## ll_key_1_2

<div class="yarn-node" data-title="ll_key_1_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card car&gt;&gt;</span>
<span class="yarn-line">J'aime me déplacer en voiture.</span> <span class="yarn-meta">#line:0887c23 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1-3"></a>

## ll_key_1_3

<div class="yarn-node" data-title="ll_key_1_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
<span class="yarn-line">J'aime les vélos, donc je me déplace à vélo.</span> <span class="yarn-meta">#line:054a9e5 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1-4"></a>

## ll_key_1_4

<div class="yarn-node" data-title="ll_key_1_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card train&gt;&gt;</span>
<span class="yarn-line">J'adore voyager en train.</span> <span class="yarn-meta">#line:08b9fff </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-1-5"></a>

## ll_key_1_5

<div class="yarn-node" data-title="ll_key_1_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
<span class="yarn-line">Je pense que le bus est la meilleure solution.</span> <span class="yarn-meta">#line:0d76e35 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-1-done"></a>

## task_1_done

<div class="yarn-node" data-title="task_1_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_1&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#line:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-1"></a>

## chest_1

<div class="yarn-node" data-title="chest_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_1_done&gt;&gt;</span>
<span class="yarn-line">    Le coffre est vide.</span> <span class="yarn-meta">#line:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 5)&gt;&gt;</span>
<span class="yarn-line">    Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#line:solve_to_unlock</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_1 activity_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#line:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-1-done"></a>

## activity_1_done

<div class="yarn-node" data-title="activity_1_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_1_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_1_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_1_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_1 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 5&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#line:pieace_sword	</span>
		<span class="yarn-cmd">&lt;&lt;jump next_1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#line:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-1"></a>

## next_1

<div class="yarn-node" data-title="next_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rendez-vous à la statue de Wars et Sawa, au bord de la rivière Vistule.</span> <span class="yarn-meta">#line:0b7f4de </span>
<span class="yarn-cmd">&lt;&lt;target npc_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-2"></a>

## info_2

<div class="yarn-node" data-title="info_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 2 | WARS AND SAWA BY THE WISLA</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa_statue&gt;&gt;</span>
<span class="yarn-line">Wars et Sawa sont des personnages d'une vieille légende sur Varsovie.</span> <span class="yarn-meta">#line:0974e04 </span>
<span class="yarn-line">Certains disent que leurs noms sont cachés à l'intérieur du nom « Varsovie ».</span> <span class="yarn-meta">#line:078a321 </span>
<span class="yarn-line">La rivière Vistule traverse Varsovie jusqu'à la mer Baltique.</span> <span class="yarn-meta">#line:07b0a55 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-2"></a>

## npc_2

<div class="yarn-node" data-title="npc_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_2_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est ouvert. Continuez !</span> <span class="yarn-meta">#line:0852298 </span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_2")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_2&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:0ca9818 </span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_2"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_2&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card wars_and_sawa_statue&gt;&gt;</span>
<span class="yarn-line">	Je suis Wars. Notre statue se dresse au bord de la Vistule.</span> <span class="yarn-meta">#line:0bb88fb </span>
<span class="yarn-line">	Parlez aux personnes en bleu au bord de la rivière et apprenez-en plus sur notre ville.</span> <span class="yarn-meta">#line:0a8f793 #task:task_2</span>
	<span class="yarn-cmd">&lt;&lt;task_start task_2 task_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-2-1"></a>

## ll_key_2_1

<div class="yarn-node" data-title="ll_key_2_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">La Vistule est le plus long fleuve de Pologne.</span> <span class="yarn-meta">#line:05030bf </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-2-2"></a>

## ll_key_2_2

<div class="yarn-node" data-title="ll_key_2_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa_statue&gt;&gt;</span>
<span class="yarn-line">Wars et Sawa sont issus d'une vieille légende sur Varsovie.</span> <span class="yarn-meta">#line:050161f </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-2-3"></a>

## ll_key_2_3

<div class="yarn-node" data-title="ll_key_2_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card warsaw_wisla_river&gt;&gt;</span>
<span class="yarn-line">La rivière Vistule se jette dans la mer Baltique.</span> <span class="yarn-meta">#line:0dc574e </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-2-done"></a>

## task_2_done

<div class="yarn-node" data-title="task_2_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_2&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-2"></a>

## chest_2

<div class="yarn-node" data-title="chest_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_2_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 3)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_2 activity_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-2-done"></a>

## activity_2_done

<div class="yarn-node" data-title="activity_2_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_2_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_2_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_2_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_2 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 3&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#shadow:pieace_sword</span>
		<span class="yarn-cmd">&lt;&lt;jump next_2&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-2"></a>

## next_2

<div class="yarn-node" data-title="next_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rendez-vous sur la place de la vieille ville et parlez au roi Sigismond.</span> <span class="yarn-meta">#line:024c223 </span>
<span class="yarn-cmd">&lt;&lt;target npc_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-3"></a>

## npc_3

<div class="yarn-node" data-title="npc_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 3 | KING SIGISMUND'S COLUMN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_3_done&gt;&gt;</span>
<span class="yarn-line">	La place est de nouveau calme. Continuez et trouvez le reste de l'épée !</span> <span class="yarn-meta">#line:07fd6e8 </span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_3")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_3&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:02c4bab </span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_3"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_3&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
<span class="yarn-line">	Parlez aux personnes en jaune sur la place et apprenez-en plus sur notre histoire.</span> <span class="yarn-meta">#line:03531ce #task:task_3</span>
	<span class="yarn-cmd">&lt;&lt;task_start task_3 task_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-3"></a>

## info_3

<div class="yarn-node" data-title="info_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
<span class="yarn-line">Le roi Sigismond III Vasa a transféré la capitale de la Pologne de Cracovie à Varsovie en 1596.</span> <span class="yarn-meta">#line:0dbddf7 </span>
<span class="yarn-line">Sa colonne sur la place du Château est l'un des plus anciens monuments de Varsovie.</span> <span class="yarn-meta">#line:0d7ec0c </span>
<span class="yarn-cmd">&lt;&lt;card royal_castle_warsaw&gt;&gt;</span>
<span class="yarn-line">Le château royal se dresse juste à côté de la colonne.</span> <span class="yarn-meta">#line:0ec0459 </span>
<span class="yarn-line">Il fut détruit pendant la Seconde Guerre mondiale et reconstruit par la suite.</span> <span class="yarn-meta">#line:002e60c </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-3-1"></a>

## ll_key_3_1

<div class="yarn-node" data-title="ll_key_3_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
<span class="yarn-line">Le roi Sigismond a transféré la capitale de Cracovie à Varsovie.</span> <span class="yarn-meta">#line:08fe036 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-3-2"></a>

## ll_key_3_2

<div class="yarn-node" data-title="ll_key_3_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
<span class="yarn-line">Cette colonne se dresse sur la place depuis très longtemps.</span> <span class="yarn-meta">#line:0719709 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-3-3"></a>

## ll_key_3_3

<div class="yarn-node" data-title="ll_key_3_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card royal_castle_warsaw&gt;&gt;</span>
<span class="yarn-line">Le château royal se trouve juste à côté de la colonne.</span> <span class="yarn-meta">#line:0053598 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-3-4"></a>

## ll_key_3_4

<div class="yarn-node" data-title="ll_key_3_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
<span class="yarn-line">Aujourd'hui, c'est le Parlement qui fait les lois en Pologne.</span> <span class="yarn-meta">#line:00cd963 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-3-done"></a>

## task_3_done

<div class="yarn-node" data-title="task_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_3&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-3"></a>

## chest_3

<div class="yarn-node" data-title="chest_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_3_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 4)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_3 activity_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-3-done"></a>

## activity_3_done

<div class="yarn-node" data-title="activity_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_3_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_3_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_3_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_3 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 4&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#shadow:pieace_sword</span>
		<span class="yarn-cmd">&lt;&lt;jump next_3&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-3"></a>

## next_3

<div class="yarn-node" data-title="next_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Continuez vers l'est jusqu'au Parlement.</span> <span class="yarn-meta">#line:0e18d75 </span>
<span class="yarn-cmd">&lt;&lt;target npc_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-parliament"></a>

## intro_area_parliament

<div class="yarn-node" data-title="intro_area_parliament">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA PARLIAMENT</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "parliament"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_parliament&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Nous sommes devant le Parlement.</span> <span class="yarn-meta">#line:01c1231 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-4"></a>

## npc_4

<div class="yarn-node" data-title="npc_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 4 | HOUSES OF PARLIAMENT</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_4_done&gt;&gt;</span>
<span class="yarn-line">	Le drapeau est trié. Continuez et trouvez le reste de l'épée !</span> <span class="yarn-meta">#line:0023131 </span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_4")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_4&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:033dd37 </span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_4"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
<span class="yarn-line">	Bienvenue au Parlement polonais.</span> <span class="yarn-meta">#line:0c74368 </span>
	<span class="yarn-cmd">&lt;&lt;detour info_4&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-line">	Discutez avec les personnes en rouge et en jaune pour en apprendre davantage sur notre drapeau et nos lois.</span> <span class="yarn-meta">#line:0ae0694 #task:task_4</span>
	<span class="yarn-cmd">&lt;&lt;task_start task_4 task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-4"></a>

## info_4

<div class="yarn-node" data-title="info_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
<span class="yarn-line">Le parlement polonais s'appelle la Sejm et le Sénat.</span> <span class="yarn-meta">#line:07a700a </span>
<span class="yarn-line">Elle se réunit à Varsovie et élabore les lois pour la Pologne.</span> <span class="yarn-meta">#line:0635522 </span>
<span class="yarn-cmd">&lt;&lt;card presidential_palace&gt;&gt;</span>
<span class="yarn-line">Le palais présidentiel se trouve à proximité, sur Krakowskie Przedmieście.</span> <span class="yarn-meta">#line:0ad1b24 </span>
<span class="yarn-line">C'est là que travaille le président de la Pologne.</span> <span class="yarn-meta">#line:0e8cdad </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-4-1"></a>

## ll_key_4_1

<div class="yarn-node" data-title="ll_key_4_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card flag_poland&gt;&gt;</span>
<span class="yarn-line">Le drapeau polonais est blanc en haut et rouge en bas.</span> <span class="yarn-meta">#line:08dafff </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-4-2"></a>

## ll_key_4_2

<div class="yarn-node" data-title="ll_key_4_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
<span class="yarn-line">La Diète (Sejm) est l'assemblée qui élabore les lois en Pologne.</span> <span class="yarn-meta">#line:0538ac3 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-4-3"></a>

## ll_key_4_3

<div class="yarn-node" data-title="ll_key_4_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
<span class="yarn-line">La Constitution du 3 mai est une date importante en Pologne.</span> <span class="yarn-meta">#line:00e7623 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-4-4"></a>

## ll_key_4_4

<div class="yarn-node" data-title="ll_key_4_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card independence_day_poland&gt;&gt;</span>
<span class="yarn-line">La fête nationale polonaise est célébrée le 11 novembre.</span> <span class="yarn-meta">#line:0765c40 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-4-done"></a>

## task_4_done

<div class="yarn-node" data-title="task_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_4&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-4"></a>

## chest_4

<div class="yarn-node" data-title="chest_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_4_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 4)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_4 activity_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-4-done"></a>

## activity_4_done

<div class="yarn-node" data-title="activity_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_4_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_4_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
<span class="yarn-line">		La Constitution du 3 mai est une date importante en Pologne.</span> <span class="yarn-meta">#line:0bb8e27 </span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_4_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_4 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 4&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#shadow:pieace_sword</span>
		<span class="yarn-cmd">&lt;&lt;jump next_4&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-4"></a>

## next_4

<div class="yarn-node" data-title="next_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Continuez jusqu'au monument Chopin.</span> <span class="yarn-meta">#line:0b73a62 </span>
<span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-chopin"></a>

## intro_area_chopin

<div class="yarn-node" data-title="intro_area_chopin">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CHOPIN</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "chopin"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_chopin&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npc_5&gt;&gt;</span>
<span class="yarn-line">Nous sommes au parc Chopin.</span> <span class="yarn-meta">#line:0eff643 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-5"></a>

## npc_5

<div class="yarn-node" data-title="npc_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 5 | CHOPIN MONUMENT</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_4_done == false&gt;&gt;</span>
<span class="yarn-line">	Le Parlement a encore besoin de votre aide. Allez-y en premier.</span> <span class="yarn-meta">#line:0a1d72c </span>
<span class="yarn-cmd">&lt;&lt;elseif $chest_5_done&gt;&gt;</span>
<span class="yarn-line">	La mélodie est rétablie. Le coffre est à vous.</span> <span class="yarn-meta">#line:08c63cf </span>
	<span class="yarn-cmd">&lt;&lt;jump chest_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_5")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_5&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:0b312ea </span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_5"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_5&gt;&gt;</span>
<span class="yarn-line">	Parlez aux écologistes autour de vous et renseignez-vous sur Chopin.</span> <span class="yarn-meta">#line:0bad021 #task:task_5</span>
	<span class="yarn-cmd">&lt;&lt;task_start task_5 task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-5"></a>

## info_5

<div class="yarn-node" data-title="info_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
<span class="yarn-line">Frédéric Chopin est né près de Varsovie.</span> <span class="yarn-meta">#line:025c001 </span>
<span class="yarn-line">Il est l'un des compositeurs les plus célèbres au monde.</span> <span class="yarn-meta">#line:050e728 </span>
<span class="yarn-cmd">&lt;&lt;card chopin_monument&gt;&gt;</span>
<span class="yarn-line">Ce monument se trouve dans le parc Łazienki à Varsovie.</span> <span class="yarn-meta">#line:0911e93 </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-5-1"></a>

## ll_key_5_1

<div class="yarn-node" data-title="ll_key_5_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
<span class="yarn-line">Chopin est né près de Varsovie en 1810.</span> <span class="yarn-meta">#line:05797e1 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-5-2"></a>

## ll_key_5_2

<div class="yarn-node" data-title="ll_key_5_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
<span class="yarn-line">Il a composé des musiques célèbres.</span> <span class="yarn-meta">#line:0b09016 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-5-3"></a>

## ll_key_5_3

<div class="yarn-node" data-title="ll_key_5_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card chopin_monument&gt;&gt;</span>
<span class="yarn-line">Des concerts gratuits sont organisés ici tous les dimanches d'été.</span> <span class="yarn-meta">#line:068a58b </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-5-4"></a>

## ll_key_5_4

<div class="yarn-node" data-title="ll_key_5_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card chopin_monument&gt;&gt;</span>
<span class="yarn-line">Ce monument se trouve dans le parc Łazienki à Varsovie.</span> <span class="yarn-meta">#line:088b9f3 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-5-done"></a>

## task_5_done

<div class="yarn-node" data-title="task_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_5&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-5"></a>

## chest_5

<div class="yarn-node" data-title="chest_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_5_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 4)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_5 activity_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-5-done"></a>

## activity_5_done

<div class="yarn-node" data-title="activity_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_5_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_5_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
<span class="yarn-line">		Chopin était un compositeur polonais.</span> <span class="yarn-meta">#line:0478dfe </span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_5_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_5 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 4&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#shadow:pieace_sword</span>
		<span class="yarn-cmd">&lt;&lt;jump next_5&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Bien essayé ! Reviens et rejoue la mélodie.</span> <span class="yarn-meta">#line:0465dd4 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-5"></a>

## next_5

<div class="yarn-node" data-title="next_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Maintenant, rendez-vous au Palais de la Culture et de la Science.</span> <span class="yarn-meta">#line:092186b </span>
<span class="yarn-cmd">&lt;&lt;target npc_6&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-palace-culture"></a>

## intro_area_palace_culture

<div class="yarn-node" data-title="intro_area_palace_culture">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA CULTURE PALACE</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "palace_culture"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_palace_culture&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Nous sommes au Palais de la Culture.</span> <span class="yarn-meta">#line:06bc9bd </span>

</code>
</pre>
</div>

<a id="ys-node-npc-6"></a>

## npc_6

<div class="yarn-node" data-title="npc_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 6 | PALACE OF CULTURE AND SCIENCE</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_6_done&gt;&gt;</span>
<span class="yarn-line">	Vous avez trouvé mon portefeuille. Le coffre est ouvert. Merci !</span> <span class="yarn-meta">#line:0997cdf</span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_6")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_6&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:0691066</span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_6"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_6&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
<span class="yarn-line">	Parlez aux personnes en violet autour du Palais.</span> <span class="yarn-meta">#line:087abc4 #task:task_6</span>
<span class="yarn-line">	Découvrez la science et le złoty.</span> <span class="yarn-meta">#line:033fd4f </span>
	<span class="yarn-cmd">&lt;&lt;task_start task_6 task_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-6"></a>

## info_6

<div class="yarn-node" data-title="info_6">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
<span class="yarn-line">Le Palais de la Culture et de la Science est le plus haut bâtiment de Pologne.</span> <span class="yarn-meta">#line:0e1b42a </span>
<span class="yarn-line">Il a été construit dans les années 1950 et offre une vue imprenable depuis son sommet.</span> <span class="yarn-meta">#line:0a22a2b </span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Maria Skłodowska-Curie était une scientifique polonaise.</span> <span class="yarn-meta">#line:0c866e7</span>
<span class="yarn-line">Elle fut la première femme à remporter un prix Nobel, et elle en a remporté deux.</span> <span class="yarn-meta">#line:079022b </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-6-1"></a>

## ll_key_6_1

<div class="yarn-node" data-title="ll_key_6_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Maria Skłodowska-Curie est née à Varsovie en 1867.</span> <span class="yarn-meta">#line:077a6ff </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-6-2"></a>

## ll_key_6_2

<div class="yarn-node" data-title="ll_key_6_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">Elle fut la première femme à remporter un prix Nobel.</span> <span class="yarn-meta">#line:097093a </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-6-3"></a>

## ll_key_6_3

<div class="yarn-node" data-title="ll_key_6_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card zoty_coins&gt;&gt;</span>
<span class="yarn-line">Le złoty est la monnaie utilisée en Pologne.</span> <span class="yarn-meta">#line:09e9bab </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-6-done"></a>

## task_6_done

<div class="yarn-node" data-title="task_6_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_6&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-6"></a>

## chest_6

<div class="yarn-node" data-title="chest_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_6_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 3)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_6 activity_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-6-done"></a>

## activity_6_done

<div class="yarn-node" data-title="activity_6_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_6_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_6_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
<span class="yarn-line">		Maria Skłodowska-Curie était une grande scientifique polonaise.</span> <span class="yarn-meta">#line:0e29d15 </span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_6_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_6 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 3&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		Vous avez trouvé un morceau de l'épée !</span> <span class="yarn-meta">#shadow:pieace_sword</span>
		<span class="yarn-cmd">&lt;&lt;jump next_6&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-next-6"></a>

## next_6

<div class="yarn-node" data-title="next_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Traversez la rivière et rendez-vous au Stade national.</span> <span class="yarn-meta">#line:026bbaa </span>
<span class="yarn-cmd">&lt;&lt;target npc_7&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-intro-area-stadium"></a>

## intro_area_stadium

<div class="yarn-node" data-title="intro_area_stadium">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// AREA STADIUM</span>
<span class="yarn-header-dim">group: center</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $current_place = "stadium"&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_stadium&gt;&gt;</span>
<span class="yarn-comment">//&lt;&lt;target npc_0&gt;&gt;</span>
<span class="yarn-line">Nous sommes au stade.</span> <span class="yarn-meta">#line:0204fd0 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-7"></a>

## npc_7

<div class="yarn-node" data-title="npc_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 7 | NATIONAL STADIUM</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_7_done&gt;&gt;</span>
<span class="yarn-line">	Super boulot au stade ! Retourne à la Sirène avec toutes les pièces.</span> <span class="yarn-meta">#line:03af06e </span>
<span class="yarn-cmd">&lt;&lt;elseif HasCompletedTask("task_7")&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;target chest_7&gt;&gt;</span>
<span class="yarn-line">	Ouvrez le coffre !</span> <span class="yarn-meta">#line:034c9f7 </span>
<span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "task_7"&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump info_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_7&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
<span class="yarn-line">	Je suis Robert Lewandowski. Bienvenue au Stade national !</span> <span class="yarn-meta">#line:012efc8</span>
<span class="yarn-line">	Parlez aux personnes en bleu autour du stade.</span> <span class="yarn-meta">#line:0ee090a #task:task_7</span>
	<span class="yarn-cmd">&lt;&lt;task_start task_7 task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-7"></a>

## info_7

<div class="yarn-node" data-title="info_7">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
<span class="yarn-line">Le Stade national se dresse sur la rive droite de la Vistule.</span> <span class="yarn-meta">#line:0e5333a </span>
<span class="yarn-line">Il a ouvert ses portes en 2012 et a accueilli de grands matchs de football.</span> <span class="yarn-meta">#line:05222ae </span>
<span class="yarn-cmd">&lt;&lt;card robert_lewandowski&gt;&gt;</span>
<span class="yarn-line">Robert Lewandowski est l'un des footballeurs polonais les plus célèbres.</span> <span class="yarn-meta">#line:0b81179 </span>
<span class="yarn-line">Il a marqué de nombreux buts pour ses équipes et pour la Pologne.</span> <span class="yarn-meta">#line:026bd53 </span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-7-1"></a>

## ll_key_7_1

<div class="yarn-node" data-title="ll_key_7_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
<span class="yarn-line">Le Stade national a ouvert ses portes en 2012.</span> <span class="yarn-meta">#line:08af39e </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-7-2"></a>

## ll_key_7_2

<div class="yarn-node" data-title="ll_key_7_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card football_soccer&gt;&gt;</span>
<span class="yarn-line">Le football est l'un des sports les plus populaires en Pologne.</span> <span class="yarn-meta">#line:0c81c04 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-7-3"></a>

## ll_key_7_3

<div class="yarn-node" data-title="ll_key_7_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card robert_lewandowski&gt;&gt;</span>
<span class="yarn-line">Lewandowski a marqué de nombreux buts pour la Pologne.</span> <span class="yarn-meta">#line:00e1d35 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ll-key-7-4"></a>

## ll_key_7_4

<div class="yarn-node" data-title="ll_key_7_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mazurek_dabrowskiego&gt;&gt;</span>
<span class="yarn-line">Le Mazurek Dąbrowskiego est l'hymne national de la Pologne.</span> <span class="yarn-meta">#line:0566eb2 </span>
<span class="yarn-cmd">&lt;&lt;inventory key_gold add 1&gt;&gt;</span>
<span class="yarn-line">Voici une clé pour vous !</span> <span class="yarn-meta">#shadow:your_key</span>
<span class="yarn-cmd">&lt;&lt;collect key_gold&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive this false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-7-done"></a>

## task_7_done

<div class="yarn-node" data-title="task_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target chest_7&gt;&gt;</span>
<span class="yarn-line">Vous avez trouvé toutes les clés ! Maintenant, ouvrez le coffre !</span> <span class="yarn-meta">#shadow:key_found</span>

</code>
</pre>
</div>

<a id="ys-node-chest-7"></a>

## chest_7

<div class="yarn-node" data-title="chest_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chest_7_done&gt;&gt;</span>
<span class="yarn-line">	Le coffre est vide.</span> <span class="yarn-meta">#shadow:chest_empty</span>
<span class="yarn-cmd">&lt;&lt;elseif has_item_at_least("key_gold", 4)&gt;&gt;</span>
<span class="yarn-line">	Pour ouvrir le coffre, résolvez cette énigme !</span> <span class="yarn-meta">#shadow:solve_to_unlock</span>
	<span class="yarn-cmd">&lt;&lt;activity activity_7 activity_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	C'est verrouillé. Il vous faut toutes les clés !</span> <span class="yarn-meta">#shadow:chest_locked</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-7-done"></a>

## activity_7_done

<div class="yarn-node" data-title="activity_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;if $chest_7_done == false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;set $chest_7_done = true&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;card football_soccer&gt;&gt;</span>
<span class="yarn-line">		Le football est l'un des sports les plus populaires ici.</span> <span class="yarn-meta">#line:0c9dff3 </span>
		<span class="yarn-cmd">&lt;&lt;card ball&gt;&gt;</span>
<span class="yarn-line">		Le ballon est ce que les joueurs envoient au pied sur le terrain.</span> <span class="yarn-meta">#line:0faaf96 </span>
		<span class="yarn-cmd">&lt;&lt;card goal&gt;&gt;</span>
<span class="yarn-line">		Le but est l'endroit où les joueurs tentent de marquer.</span> <span class="yarn-meta">#line:0246131 </span>
		<span class="yarn-cmd">&lt;&lt;card soccer_field&gt;&gt;</span>
<span class="yarn-line">		Le terrain de football est le lieu où se déroule le match.</span> <span class="yarn-meta">#line:0a13fae </span>
		<span class="yarn-cmd">&lt;&lt;card mazurek_dabrowskiego&gt;&gt;</span>
<span class="yarn-line">		Mazurek Dąbrowskiego est l'hymne national de la Pologne.</span> <span class="yarn-meta">#line:047522a </span>
		<span class="yarn-cmd">&lt;&lt;trigger chest_7_open&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;SetInteractable chest_7 false&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory key_gold remove 4&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;inventory mermaids_sword add&gt;&gt;</span>
<span class="yarn-line">		La septième pièce d'épée est à vous ! Vous avez les 7 pièces !</span> <span class="yarn-meta">#line:0a2ac65 </span>
<span class="yarn-line">		Retournez voir la Sirène et réassemblez l'épée.</span> <span class="yarn-meta">#line:0221147 </span>
		<span class="yarn-cmd">&lt;&lt;target npc_1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Essayer à nouveau!</span> <span class="yarn-meta">#shadow:try_again</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-local"></a>

## spawned_warsaw_local

<div class="yarn-node" data-title="spawned_warsaw_local">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_locals</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">La rivière Vistule traverse Varsovie.</span> <span class="yarn-meta">#line:033eb37 </span>
<span class="yarn-line">La sirène est un symbole de la ville.</span> <span class="yarn-meta">#line:0eed37b </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-guide"></a>

## spawned_warsaw_guide

<div class="yarn-node" data-title="spawned_warsaw_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: warsaw_guides</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Chopin était un célèbre compositeur polonais.</span> <span class="yarn-meta">#line:0684a7f </span>
<span class="yarn-line">Le roi Sigismond a transféré la capitale à Varsovie.</span> <span class="yarn-meta">#line:0c02302 </span>
<span class="yarn-line">C'est au Parlement que les lois sont élaborées.</span> <span class="yarn-meta">#line:0ea39a3 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-student"></a>

## spawned_warsaw_student

<div class="yarn-node" data-title="spawned_warsaw_student">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_students</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le drapeau polonais est blanc et rouge.</span> <span class="yarn-meta">#line:0dd6cd4 #card:flag_poland</span>
<span class="yarn-line">Maria Skłodowska-Curie était une scientifique.</span> <span class="yarn-meta">#line:0dbc5be </span>
<span class="yarn-line">La fête de l'indépendance est le 11 novembre.</span> <span class="yarn-meta">#line:0432922 </span>

</code>
</pre>
</div>


