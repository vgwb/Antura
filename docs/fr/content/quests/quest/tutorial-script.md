---
title: Tutoriel (tutorial) - Script
hide:
---

# Tutoriel (tutorial) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/quest-tutorial/41)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=631129787#gid=631129787)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/_TUTORIAL/Tutorial%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// tutorial | Tutorial</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bienvenue dans le tutoriel !</span> <span class="yarn-meta">#line:021793f </span>
<span class="yarn-line">Ici, nous apprenons à jouer.</span> <span class="yarn-meta">#line:0588c17</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset keys_wasd zoom&gt;&gt;</span>
<span class="yarn-line">    Utilisez les touches WASD pour marcher.</span> <span class="yarn-meta">#line:037d71d </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_move zoom&gt;&gt;</span>
<span class="yarn-line">    Utilisez votre doigt gauche pour marcher</span> <span class="yarn-meta">#line:0e55bc4 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor-1"></a>

## talk_tutor_1

<div class="yarn-node" data-title="talk_tutor_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Touchez ce texte pour l'entendre dans votre langue.</span> <span class="yarn-meta">#line:03dbfa7 #native</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_goon&gt;&gt;</span>
<span class="yarn-line">Utilisez ce bouton pour faire avancer la boîte de dialogue.</span> <span class="yarn-meta">#line:0f4f069 </span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_image&gt;&gt;</span>
<span class="yarn-line">Utilisez ce bouton pour voir la photo</span> <span class="yarn-meta">#line:0784704 </span>
<span class="yarn-line">Allez parler au prochain tuteur !</span> <span class="yarn-meta">#line:0eb85aa </span>
<span class="yarn-cmd">&lt;&lt;action area_medium&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-camera"></a>

## tutor_camera

<div class="yarn-node" data-title="tutor_camera">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Appuyez sur le bouton droit de la souris pour déplacer la caméra.</span> <span class="yarn-meta">#line:0e633a2 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_camera&gt;&gt;</span>
<span class="yarn-line">    Utilisez votre doigt droit pour déplacer la caméra.</span> <span class="yarn-meta">#line:0aa47cb </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_large&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-act"></a>

## tutor_act

<div class="yarn-node" data-title="tutor_act">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_act&gt;&gt;</span>
<span class="yarn-line">Utilisez ce bouton pour parler ou interagir</span> <span class="yarn-meta">#line:0c14f65 </span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Ou appuyez sur la touche ESPACE.</span> <span class="yarn-meta">#line:0c18f6b </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-tutor-4-jump"></a>

## tutor_4_jump

<div class="yarn-node" data-title="tutor_4_jump">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: asset=tutorial_move</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $IS_DESKTOP&gt;&gt;</span>
<span class="yarn-line">    Appuyez sur la touche ESPACE pour sauter</span> <span class="yarn-meta">#line:07940cf </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;asset tutorial_jump&gt;&gt;</span>
<span class="yarn-line">    Utilisez ce bouton pour sauter</span> <span class="yarn-meta">#line:0b9c1fa </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action area_intro_large&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-tutor-6-cookies"></a>

## tutor_6_cookies

<div class="yarn-node" data-title="tutor_6_cookies">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cookies&gt;&gt;</span>
<span class="yarn-line">Prenez tous les biscuits que vous trouvez. Ils peuvent être utiles.</span> <span class="yarn-meta">#line:0f50a6e </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-7-map"></a>

## tutor_7_map

<div class="yarn-node" data-title="tutor_7_map">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_map&gt;&gt;</span>
<span class="yarn-line">Ce bouton ouvre la carte !</span> <span class="yarn-meta">#line:01777e4 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-8-interact"></a>

## tutor_8_interact

<div class="yarn-node" data-title="tutor_8_interact">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_actions&gt;&gt;</span>
<span class="yarn-line">Explorez tous les objets qui ont cette icône.</span> <span class="yarn-meta">#line:0139142 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-9-pushball"></a>

## tutor_9_pushball

<div class="yarn-node" data-title="tutor_9_pushball">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset tutorial_ball&gt;&gt;</span>
<span class="yarn-line">Essayez de pousser cette balle.</span> <span class="yarn-meta">#line:02253ae </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-10-follow"></a>

## tutor_10_follow

<div class="yarn-node" data-title="tutor_10_follow">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target target_11&gt;&gt;</span>
<span class="yarn-line">Si vous vous perdez, suivez cette icône.</span> <span class="yarn-meta">#line:06c117d </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-11-mission"></a>

## tutor_11_mission

<div class="yarn-node" data-title="tutor_11_mission">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;action area_all&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;camera_focus camera_coin&gt;&gt;</span>
<span class="yarn-line">Laisse-moi voir si tu as appris : monte les escaliers et récupère cette pièce !</span> <span class="yarn-meta">#line:0fe9efe </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-teleport"></a>

## tutor_teleport

<div class="yarn-node" data-title="tutor_teleport">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_portal&gt;&gt;</span>
<span class="yarn-line">Utilisez les portails pour voyager rapidement !</span> <span class="yarn-meta">#line:0f753b5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-killzone"></a>

## tutor_killzone

<div class="yarn-node" data-title="tutor_killzone">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_danger&gt;&gt;</span>
<span class="yarn-line">Faites attention à ne pas tomber dans l'eau !</span> <span class="yarn-meta">#line:0e9b5a9 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-livingletter"></a>

## tutor_livingletter

<div class="yarn-node" data-title="tutor_livingletter">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_livingletter&gt;&gt;</span>
<span class="yarn-line">Ce sont des Lettres Vivantes. Parlez-leur pour apprendre de nouveaux mots !</span> <span class="yarn-meta">#line:06e500f </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-blocky-character"></a>

## tutor_blocky_character

<div class="yarn-node" data-title="tutor_blocky_character">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_blocky_character&gt;&gt;</span>
<span class="yarn-line">Ces gens sont nos amis. Parlez-leur pour en apprendre davantage sur le monde !</span> <span class="yarn-meta">#line:0be283b </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-card"></a>

## tutor_card

<div class="yarn-node" data-title="tutor_card">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_card&gt;&gt;</span>
<span class="yarn-line">Ceci est une CARTE. Elle possède des connaissances et des pouvoirs. Collectionnez-les toutes !</span> <span class="yarn-meta">#line:0ac4c18 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-cat"></a>

## tutor_cat

<div class="yarn-node" data-title="tutor_cat">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_cat&gt;&gt;</span>
<span class="yarn-line">Oui. C'est toi ! En jouant jusqu'au bout, tu pourras changer de look !</span> <span class="yarn-meta">#line:0eb1890 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-antura"></a>

## tutor_antura

<div class="yarn-node" data-title="tutor_antura">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura&gt;&gt;</span>
<span class="yarn-line">Voici notre ami Antura. Il vous accompagnera dans votre aventure !</span> <span class="yarn-meta">#line:015cdc5 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-inventory"></a>

## tutor_inventory

<div class="yarn-node" data-title="tutor_inventory">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_inventory&gt;&gt;</span>
<span class="yarn-line">Ceci est votre inventaire. Cliquez sur un objet pour l'utiliser.</span> <span class="yarn-meta">#line:02a6cfa </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-progress"></a>

## tutor_progress

<div class="yarn-node" data-title="tutor_progress">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_progress&gt;&gt;</span>
<span class="yarn-line">Voici la progression du jeu. Jouez bien et obtenez 3 étoiles !</span> <span class="yarn-meta">#line:0b606b8 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-tasks"></a>

## tutor_tasks

<div class="yarn-node" data-title="tutor_tasks">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_tasks&gt;&gt;</span>
<span class="yarn-line">Ce panneau vous indique ce que vous devez faire.</span> <span class="yarn-meta">#line:012d967 </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-target"></a>

## tutor_target

<div class="yarn-node" data-title="tutor_target">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card antura_target&gt;&gt;</span>
<span class="yarn-line">Ce symbole vous indique où aller.</span> <span class="yarn-meta">#line:0864faf </span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory"></a>

## activity_memory

<div class="yarn-node" data-title="activity_memory">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons à l'activité MÉMOIRE !</span> <span class="yarn-meta">#line:05e100e </span>
<span class="yarn-line">Trouvez les paires de cartes.</span> <span class="yarn-meta">#line:0ef88ae </span>
<span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-memory-result"></a>

## activity_memory_result

<div class="yarn-node" data-title="activity_memory_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est bon d'exercer notre mémoire !</span> <span class="yarn-meta">#line:00e6a04 </span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:0c78a9e </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:0cd0316 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:07cbfdd </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:0e3251c </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_tutorial activity_memory_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:010515b </span>

</code>
</pre>
</div>

<a id="ys-node-activity-canvas"></a>

## activity_canvas

<div class="yarn-node" data-title="activity_canvas">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons à l'activité CANVAS</span> <span class="yarn-meta">#line:07041b3 </span>
<span class="yarn-line">Vous devez nettoyer tout l'écran sans toucher Antura !</span> <span class="yarn-meta">#line:0c80adc </span>
<span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result tutorial&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-activity-canvas-result"></a>

## activity_canvas_result

<div class="yarn-node" data-title="activity_canvas_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est agréable de nettoyer, n'est-ce pas ?</span> <span class="yarn-meta">#line:059323a</span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:0cb9ba3 </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:04f5fb1 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:02fef35 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:02eec35 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_tutorial activity_canvas_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:0e863cb </span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw"></a>

## activity_jigsaw

<div class="yarn-node" data-title="activity_jigsaw">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons au puzzle d'activité !</span> <span class="yarn-meta">#line:0fe648a </span>
<span class="yarn-line">Complétez l'image.</span> <span class="yarn-meta">#line:0bc50ca </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-jigsaw-result"></a>

## activity_jigsaw_result

<div class="yarn-node" data-title="activity_jigsaw_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'adore les puzzles !</span> <span class="yarn-meta">#line:0fc42d8 </span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:0c4ff24 </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:0954fe5 </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:000c3d6 </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:059ec4a </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_tutorial activity_jigsaw_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:0532802 </span>

</code>
</pre>
</div>

<a id="ys-node-activity-match"></a>

## activity_match

<div class="yarn-node" data-title="activity_match">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons à Activity MATCH !</span> <span class="yarn-meta">#line:07c8447 </span>
<span class="yarn-line">Associez les cartes similaires.</span> <span class="yarn-meta">#line:02aec90 </span>
<span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-result"></a>

## activity_match_result

<div class="yarn-node" data-title="activity_match_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Belle association !</span> <span class="yarn-meta">#line:0f0ee2e</span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:004c965 </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:01edc32 </span>
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:07dd1f5 </span>
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:03da90e </span>
    <span class="yarn-cmd">&lt;&lt;activity match_tutorial activity_match_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:0f413e8 </span>

</code>
</pre>
</div>

<a id="ys-node-activity-money"></a>

## activity_money

<div class="yarn-node" data-title="activity_money">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons à l'activité COMPTEZ L'ARGENT !</span> <span class="yarn-meta">#line:06b295f </span>
<span class="yarn-line">Vous devez donner le montant correct de pièces.</span> <span class="yarn-meta">#line:0f00e5d </span>
<span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-money-result"></a>

## activity_money_result

<div class="yarn-node" data-title="activity_money_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il est important d’apprendre à utiliser l’argent !</span> <span class="yarn-meta">#line:06136c1</span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:0f807bd </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:084b4a8 </span>
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:00f0e0f </span>
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:0a78802 </span>
    <span class="yarn-cmd">&lt;&lt;activity money_tutorial activity_money_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:02c75c6 </span>

</code>
</pre>
</div>

<a id="ys-node-activity-order"></a>

## activity_order

<div class="yarn-node" data-title="activity_order">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Jouons à l'activité ORDRE !</span> <span class="yarn-meta">#line:015a3ea </span>
<span class="yarn-line">Mettez les éléments dans le bon ordre.</span> <span class="yarn-meta">#line:0ed152d </span>
<span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-order-result"></a>

## activity_order_result

<div class="yarn-node" data-title="activity_order_result">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: activities</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonne activité.</span> <span class="yarn-meta">#line:0838f7f </span>
<span class="yarn-line">Tu veux rejouer ?</span> <span class="yarn-meta">#line:06bb133 </span>
<span class="yarn-line">Facile</span> <span class="yarn-meta">#line:04c0d23 </span>
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result easy&gt;&gt;</span>
<span class="yarn-line">Normale</span> <span class="yarn-meta">#line:0758cd7 </span>
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result normal&gt;&gt;</span>
<span class="yarn-line">Expert</span> <span class="yarn-meta">#line:0879058 </span>
    <span class="yarn-cmd">&lt;&lt;activity order_tutorial activity_order_result expert&gt;&gt;</span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:0ea4d7e </span>

</code>
</pre>
</div>

<a id="ys-node-tutor-end"></a>

## tutor_end

<div class="yarn-node" data-title="tutor_end">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est la fin du tutoriel.</span> <span class="yarn-meta">#line:0dfdfc5 </span>
<span class="yarn-line">Êtes-vous prêt à jouer au jeu ?</span> <span class="yarn-meta">#line:02fea28 </span>
<span class="yarn-line">Êtes-vous prêt à jouer ?</span> <span class="yarn-meta">#line:0ac17d0 </span>
<span class="yarn-line">Oui</span> <span class="yarn-meta">#line:0b66e60 </span>
<span class="yarn-line">    Super. À bientôt en jeu !</span> <span class="yarn-meta">#line:07498c0 </span>
<span class="yarn-line">    Il y a bien plus à découvrir à Antura.</span> <span class="yarn-meta">#line:0ed06b6 </span>
<span class="yarn-line">Non</span> <span class="yarn-meta">#line:01d5126 </span>
<span class="yarn-line">    Vous pouvez rejouer le didacticiel à tout moment.</span> <span class="yarn-meta">#line:06f9065 </span>
<span class="yarn-line">    Demandez à votre professeur de vous aider.</span> <span class="yarn-meta">#line:0c6bc14 </span>

</code>
</pre>
</div>

<a id="ys-node-ll-skyscraper"></a>

## ll_skyscraper

<div class="yarn-node" data-title="ll_skyscraper">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est merveilleux de voir la ville d'ici !</span> <span class="yarn-meta">#line:0969b2f </span>
<span class="yarn-line">J'aimerais voyager partout dans le monde !</span> <span class="yarn-meta">#line:060db75 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-secrets"></a>

## npc_secrets

<div class="yarn-node" data-title="npc_secrets">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'ai entendu dire qu'il y avait des secrets dans ce niveau...</span> <span class="yarn-meta">#line:0b8d755 </span>
<span class="yarn-line">Et un portail pour voyager sur ce gratte-ciel !</span> <span class="yarn-meta">#line:032af93 </span>
<span class="yarn-cmd">&lt;&lt;jump global_init&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-hallo"></a>

## npc_hallo

<div class="yarn-node" data-title="npc_hallo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour!</span> <span class="yarn-meta">#line:022bd3f</span>
<span class="yarn-line">as-tu vu Antura ?</span> <span class="yarn-meta">#line:00ad419</span>
<span class="yarn-line">J'ai perdu mon cookie !</span> <span class="yarn-meta">#line:0a94666 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-kid"></a>

## npc_kid

<div class="yarn-node" data-title="npc_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Des cookies sont cachés partout ! Trouvez-les !</span> <span class="yarn-meta">#line:0c8d2fa </span>
<span class="yarn-line">J'adore utiliser les portails pour voyager !</span> <span class="yarn-meta">#line:0fb6855 </span>
<span class="yarn-line">Parlez à tous ceux que vous rencontrez !</span> <span class="yarn-meta">#line:0812c45 </span>
<span class="yarn-line">Explorez le monde !</span> <span class="yarn-meta">#line:041d845 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-arcade"></a>

## npc_arcade

<div class="yarn-node" data-title="npc_arcade">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'adore les jeux vidéo !</span> <span class="yarn-meta">#line:0479595 </span>
<span class="yarn-line">Voulez-vous jouer avec moi?</span> <span class="yarn-meta">#line:0d3fdc7 </span>

</code>
</pre>
</div>


