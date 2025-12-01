---
title: Le zoo (pl_04) - Script
hide:
---

# Le zoo (pl_04) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-04-the-zoo/35/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=819047762#gid=819047762)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_04%20Zoo/PL_04%20Zoo%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_04 | Zoo (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M, </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $elephant_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $giraffe_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lion_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $monkey_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $penguin_completed = false&gt;&gt;</span>

<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $found_chest_cookies = false&gt;&gt;</span>
<span class="yarn-line">Bienvenue au zoo de WROCŁAW.</span> <span class="yarn-meta">#line:0fe55d1 </span>
<span class="yarn-line">Le plus grand zoo de Pologne !</span> <span class="yarn-meta">#line:005dd46</span>
<span class="yarn-cmd">&lt;&lt;target director&gt;&gt;</span>

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
<span class="yarn-line">Cette quête est terminée.</span> <span class="yarn-meta">#line:0bcc257 </span>
<span class="yarn-line">Vous avez appris des choses sur les ANIMAUX DU ZOO.</span> <span class="yarn-meta">#line:054cc37 </span>
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
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">Dessine ton ANIMAL préféré ou le HALL DU CENTENAIRE.</span> <span class="yarn-meta">#line:0809ac5</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director"></a>

## director

<div class="yarn-node" data-title="director">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump director_task&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task"></a>

## director_task

<div class="yarn-node" data-title="director_task">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
<span class="yarn-line">Bonjour, je suis le directeur du zoo.</span> <span class="yarn-meta">#line:0815405 </span>
<span class="yarn-line">Aujourd'hui, nous avons un problème.</span> <span class="yarn-meta">#line:0c71176 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-line">Notre drapeau a disparu !</span> <span class="yarn-meta">#line:09c6bf7 </span>
<span class="yarn-cmd">&lt;&lt;card iglica&gt;&gt;</span>
<span class="yarn-line">C'était sur l'IGLICA au CENTENNIAL HALL.</span> <span class="yarn-meta">#line:02f35e4 </span>
<span class="yarn-line">C'est la célèbre sculpture métallique, un symbole de notre ville.</span> <span class="yarn-meta">#line:0335bf7 </span>
<span class="yarn-line">Il mesure 90 mètres de haut !</span> <span class="yarn-meta">#line:001cba3 </span>
<span class="yarn-line">Qui a bien pu le prendre ?</span> <span class="yarn-meta">#line:0fc7e35 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Trouvez le DRAPEAU. Parlez aux ANIMAUX.</span> <span class="yarn-meta">#line:0da284c #task:TASK_ANIMALS</span>
<span class="yarn-line">Peut-être que l'un d'eux l'a.</span> <span class="yarn-meta">#line:012b933</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-animals-done"></a>

## task_animals_done

<div class="yarn-node" data-title="task_animals_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vous avez parlé à tous les ANIMAUX.</span> <span class="yarn-meta">#shadow:032811f</span>
<span class="yarn-line">Retournez maintenant voir le DIRECTEUR DU ZOO.</span> <span class="yarn-meta">#line:0a93d9b #task:back_to_director</span>
<span class="yarn-cmd">&lt;&lt;target director&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start back_to_director&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $talked_animals = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-task-done"></a>

## director_task_done

<div class="yarn-node" data-title="director_task_done">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">group: Intro</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vous avez parlé à tous les ANIMAUX.</span> <span class="yarn-meta">#line:032811f </span>
<span class="yarn-line">Et ils n'ont pas pris le DRAPEAU.</span> <span class="yarn-meta">#line:0364f30 </span>
<span class="yarn-line">Retournons au Centennial Hall.</span> <span class="yarn-meta">#line:0bc6238 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_zoo_settings jigsaw_zoo_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-jigsaw-zoo-done"></a>

## jigsaw_zoo_done

<div class="yarn-node" data-title="jigsaw_zoo_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Si tous les ANIMAUX sont innocents...</span> <span class="yarn-meta">#line:0bc2b46 </span>
<span class="yarn-line">Qui a pris le DRAPEAU ?</span> <span class="yarn-meta">#line:0fc0ab7 </span>
<span class="yarn-cmd">&lt;&lt;SetActive anturaFlag&gt;&gt;</span>
<span class="yarn-line">Attendez, regardez !</span> <span class="yarn-meta">#line:0b3d05f </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura&gt;&gt;</span>
<span class="yarn-line">Antura a le DRAPEAU !</span> <span class="yarn-meta">#line:0e24973 </span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_flag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive iglica_flag true&gt;&gt;</span>
<span class="yarn-line">Le mystère est résolu.</span> <span class="yarn-meta">#line:0bf024f</span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_centennial_hall&gt;&gt;</span>
<span class="yarn-line">Le drapeau est de retour.</span> <span class="yarn-meta">#line:0d91701 </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey"></a>

## monkey

<div class="yarn-node" data-title="monkey">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ########################## MONKEY</span>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_monkey_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Voulez-vous rejouer ?</span> <span class="yarn-meta">#line:play_again</span>
<span class="yarn-line">    Oui</span> <span class="yarn-meta">#line:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings&gt;&gt;</span>
<span class="yarn-line">    Oui, mais plus difficile.</span> <span class="yarn-meta">#line:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings_hard&gt;&gt;</span>
<span class="yarn-line">    Non</span> <span class="yarn-meta">#line:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un chimpanzé. Je grimpe aux arbres.</span> <span class="yarn-meta">#line:0867233 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_chimpanzee&gt;&gt;</span>
<span class="yarn-line">    J'adore manger des fruits !</span> <span class="yarn-meta">#line:0eaefd6 </span>
<span class="yarn-line">    Pouvez-vous deviner quel arbre je dois escalader pour obtenir...</span> <span class="yarn-meta">#line:0617de7 </span>
<span class="yarn-line">    Une pomme</span> <span class="yarn-meta">#line:0c6c41a </span>
<span class="yarn-line">    Une orange</span> <span class="yarn-meta">#line:08c7ea3 </span>
<span class="yarn-line">    Et une banane ?</span> <span class="yarn-meta">#line:0ce7b11 </span>
<span class="yarn-line">    Associez les fruits à leurs arbres !</span> <span class="yarn-meta">#line:079451b</span>
    <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings monkey_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey-activity-done"></a>

## monkey_activity_done

<div class="yarn-node" data-title="monkey_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_chimpanzee collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $monkey_completed = true&gt;&gt;</span>
<span class="yarn-line">Ce serait amusant de grimper à ce poteau.</span> <span class="yarn-meta">#line:0a43c85</span>
<span class="yarn-line">Mais je n'ai pas pris le DRAPEAU !</span> <span class="yarn-meta">#line:0c53945 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-tutor"></a>

## monkey_tutor

<div class="yarn-node" data-title="monkey_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_chimpanzee&gt;&gt;</span>
<span class="yarn-line">Les chimpanzés sont d'excellents grimpeurs.</span> <span class="yarn-meta">#line:01d80fc </span>
<span class="yarn-cmd">&lt;&lt;card food_apple&gt;&gt;</span>
<span class="yarn-line">Ils mangent beaucoup de fruits.</span> <span class="yarn-meta">#line:0883b4b </span>
<span class="yarn-cmd">&lt;&lt;card plant_apple&gt;&gt;</span>
<span class="yarn-line">Chaque fruit pousse sur un arbre différent.</span> <span class="yarn-meta">#line:0cda3c2 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-kid"></a>

## monkey_kid

<div class="yarn-node" data-title="monkey_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Les chimpanzés ont de longs bras pour se balancer dans les arbres.</span> <span class="yarn-meta">#line:0bf2346 </span>
<span class="yarn-line">Les chimpanzés sont intelligents !</span> <span class="yarn-meta">#line:0eecdf9 </span>

</code>
</pre>
</div>

<a id="ys-node-lion"></a>

## lion

<div class="yarn-node" data-title="lion">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  LION</span>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_lion_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Voulez-vous rejouer ?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Oui</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity order_lion_settings&gt;&gt;</span>
<span class="yarn-line">    Oui, mais plus difficile.</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity order_lion_settings_hard&gt;&gt;</span>
<span class="yarn-line">    Non</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un LION d'AFRIQUE.</span> <span class="yarn-meta">#line:07f2e15 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion_cub&gt;&gt;</span>
<span class="yarn-line">    Quand j'étais jeune, je vivais avec mes parents.</span> <span class="yarn-meta">#line:0124e1c</span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion_young_male&gt;&gt;</span>
<span class="yarn-line">    Puis j'ai grandi...</span> <span class="yarn-meta">#line:019539f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_lion&gt;&gt;</span>
<span class="yarn-line">    Maintenant, je suis un LION adulte avec une magnifique crinière.</span> <span class="yarn-meta">#line:0774b92 </span>
<span class="yarn-line">    Pouvez-vous remettre mes photos dans l'ordre ?</span> <span class="yarn-meta">#line:095a71f </span>
    <span class="yarn-cmd">&lt;&lt;activity order_lion_settings lion_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lion-activity-done"></a>

## lion_activity_done

<div class="yarn-node" data-title="lion_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $lion_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card animal_lion collect&gt;&gt;</span>
<span class="yarn-line">J'adore regarder le drapeau flotter là-haut sur l'Iglica.</span> <span class="yarn-meta">#line:01b3593 </span>
<span class="yarn-line">Mais je ne l'ai pas pris. Veuillez retrouver ce DRAPEAU !</span> <span class="yarn-meta">#line:05da6d7 </span>

</code>
</pre>
</div>

<a id="ys-node-lion-tutor"></a>

## lion_tutor

<div class="yarn-node" data-title="lion_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_lion&gt;&gt;</span>
<span class="yarn-line">Les lions vivent en groupes familiaux.</span> <span class="yarn-meta">#line:091881b </span>
<span class="yarn-line">Les mâles adultes ont une crinière.</span> <span class="yarn-meta">#line:02b4a06 </span>
<span class="yarn-cmd">&lt;&lt;card animal_lion_cub&gt;&gt;</span>
<span class="yarn-line">Mais les jeunes lionceaux, non.</span> <span class="yarn-meta">#line:0d378cc </span>

</code>
</pre>
</div>

<a id="ys-node-lion-kid"></a>

## lion_kid

<div class="yarn-node" data-title="lion_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Quel rugissement !</span> <span class="yarn-meta">#line:079f4e0 </span>
<span class="yarn-line">Les lions sont les rois de la jungle !</span> <span class="yarn-meta">#line:020c29f </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe"></a>

## giraffe

<div class="yarn-node" data-title="giraffe">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  GIRAFFE</span>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("canvas_giraffe_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Voulez-vous rejouer ?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Oui</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings&gt;&gt;</span>
<span class="yarn-line">    Oui, mais plus difficile.</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings_hard&gt;&gt;</span>
<span class="yarn-line">    Non</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis l'ANIMAL le plus grand.</span> <span class="yarn-meta">#line:0d5c607 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_giraffe&gt;&gt;</span>
<span class="yarn-line">    Mon long cou me permet d'atteindre les feuilles.</span> <span class="yarn-meta">#line:0a4d24e </span>
<span class="yarn-line">    J'adore manger des feuilles d'acacia !</span> <span class="yarn-meta">#line:04b42f2 </span>
<span class="yarn-line">    Pouvez-vous nettoyer la photo pour moi ?</span> <span class="yarn-meta">#line:02af551 </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings giraffe_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-activity-done"></a>

## giraffe_activity_done

<div class="yarn-node" data-title="giraffe_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_giraffe collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $giraffe_completed = true&gt;&gt;</span>
<span class="yarn-line">Merci. Je n'ai pas pris le drapeau.</span> <span class="yarn-meta">#line:0877d6f</span>
<span class="yarn-line">Je suis grand, mais pas de 90 mètres comme l'IGLICA !</span> <span class="yarn-meta">#line:02d00e2</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-tutor"></a>

## giraffe_tutor

<div class="yarn-node" data-title="giraffe_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_giraffe&gt;&gt;</span>
<span class="yarn-line">Les girafes sont les animaux les plus grands.</span> <span class="yarn-meta">#line:03b36c0 </span>
<span class="yarn-line">Ils ont un long cou pour manger les feuilles des grands arbres.</span> <span class="yarn-meta">#line:0d9d52f </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-kid"></a>

## giraffe_kid

<div class="yarn-node" data-title="giraffe_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Avec un cou pareil, je pourrais voir ma maison d'ici !</span> <span class="yarn-meta">#line:0bee484 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant"></a>

## elephant

<div class="yarn-node" data-title="elephant">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  ELEPHANT</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $elephant_completed&gt;&gt;</span>
<span class="yarn-line">    Voulez-vous rejouer ?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Oui</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings&gt;&gt;</span>
<span class="yarn-line">    Oui, mais plus difficile.</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings_hard&gt;&gt;</span>
<span class="yarn-line">    Non</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis le plus grand ANIMAL terrestre.</span> <span class="yarn-meta">#line:027b51f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_elephant&gt;&gt;</span>
<span class="yarn-line">    J'ai une excellente MÉMOIRE.</span> <span class="yarn-meta">#line:03a150c </span>
<span class="yarn-line">    Je me souviens de tout !</span> <span class="yarn-meta">#line:01fe852 </span>
<span class="yarn-line">    Avez-vous une bonne mémoire ?</span> <span class="yarn-meta">#line:0f98478 </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings elephant_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-activity-done"></a>

## elephant_activity_done

<div class="yarn-node" data-title="elephant_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_elephant collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $elephant_completed = true&gt;&gt;</span>
<span class="yarn-line">LE DRAPEAU ? Je ne l'ai pas.</span> <span class="yarn-meta">#line:0b79d01</span>
<span class="yarn-line">Si je l'avais pris, je m'en souviendrais !</span> <span class="yarn-meta">#line:0f124bf</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-tutor"></a>

## elephant_tutor

<div class="yarn-node" data-title="elephant_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_elephant&gt;&gt;</span>
<span class="yarn-line">Les éléphants ont la peau épaisse et de grandes oreilles.</span> <span class="yarn-meta">#line:06c3bba </span>
<span class="yarn-line">Ce sont les plus grands animaux terrestres.</span> <span class="yarn-meta">#line:0bc4ca4 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant-kid"></a>

## elephant_kid

<div class="yarn-node" data-title="elephant_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ses oreilles sont plus grandes que moi !</span> <span class="yarn-meta">#line:0fc78ad </span>
<span class="yarn-line">Ces oreilles peuvent-elles me ventiler en été ?</span> <span class="yarn-meta">#line:004abc7 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin"></a>

## penguin

<div class="yarn-node" data-title="penguin">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// ##########################  PENGUIN</span>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("jigsaw_penguin_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Voulez-vous rejouer ?</span> <span class="yarn-meta">#shadow:play_again</span>
<span class="yarn-line">    Oui</span> <span class="yarn-meta">#shadow:yes</span>
        <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings&gt;&gt;</span>
<span class="yarn-line">    Oui, mais plus difficile.</span> <span class="yarn-meta">#shadow:yes_harder</span>
        <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings_hard&gt;&gt;</span>
<span class="yarn-line">    Non</span> <span class="yarn-meta">#shadow:no</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un PINGOUIN, un OISEAU spécial.</span> <span class="yarn-meta">#line:08c70e8 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_penguin&gt;&gt;</span>
<span class="yarn-line">    Je ne sais pas voler, mais je nage bien !</span> <span class="yarn-meta">#line:0540c5a </span>
<span class="yarn-line">    J'adore la glace et le froid.</span> <span class="yarn-meta">#line:0ba1a43 </span>
<span class="yarn-line">    Essayons de résoudre l'énigme ensemble !</span> <span class="yarn-meta">#line:020100a </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings penguin_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-penguin-activity-done"></a>

## penguin_activity_done

<div class="yarn-node" data-title="penguin_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_penguin collect&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $penguin_completed = true&gt;&gt;</span>
<span class="yarn-line">Non, je n'ai pas pris le drapeau.</span> <span class="yarn-meta">#line:078190f </span>
<span class="yarn-line">N'oubliez pas, je ne peux pas voler !</span> <span class="yarn-meta">#line:08568f5 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-tutor"></a>

## penguin_tutor

<div class="yarn-node" data-title="penguin_tutor">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_penguin&gt;&gt;</span>
<span class="yarn-line">Les pingouins sont des oiseaux qui savent nager.</span> <span class="yarn-meta">#line:0920adc </span>
<span class="yarn-cmd">&lt;&lt;card ice_arctic&gt;&gt;</span>
<span class="yarn-line">Ils vivent près des océans et des glaces.</span> <span class="yarn-meta">#line:0baa8d0 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-kid"></a>

## penguin_kid

<div class="yarn-node" data-title="penguin_kid">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Préférez-vous prendre l'avion ou nager ?</span> <span class="yarn-meta">#line:0ae73f3 </span>
<span class="yarn-line">Je ne pourrais pas vivre sur la glace !</span> <span class="yarn-meta">#line:05ac327 </span>

</code>
</pre>
</div>

<a id="ys-node-spawn-tourist"></a>

## spawn_tourist

<div class="yarn-node" data-title="spawn_tourist">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: SPAWN</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card  wroclaw_zoo&gt;&gt;</span>
<span class="yarn-line">Il y a ici plus de 10 000 ANIMAUX.</span> <span class="yarn-meta">#line:0354bea </span>
<span class="yarn-line">Ce zoo est très grand.</span> <span class="yarn-meta">#line:0d80278 </span>
<span class="yarn-line">Je crois avoir aperçu un coffre au trésor quelque part.</span> <span class="yarn-meta">#line:0694a4c </span>

</code>
</pre>
</div>

<a id="ys-node-animal-peacock"></a>

## animal_peacock

<div class="yarn-node" data-title="animal_peacock">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card peacock&gt;&gt;</span>
<span class="yarn-line">    Je n'ai pas besoin de drapeau.</span> <span class="yarn-meta">#line:0085a8a </span>
<span class="yarn-line">    Ma queue ressemble à un DRAPEAU !</span> <span class="yarn-meta">#line:04fff6b </span>

</code>
</pre>
</div>

<a id="ys-node-animal-parrot"></a>

## animal_parrot

<div class="yarn-node" data-title="animal_parrot">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card parrot&gt;&gt;</span>
<span class="yarn-line">Squawk ! J'ai vu passer du rouge et du jaune !</span> <span class="yarn-meta">#line:0e84545 </span>

</code>
</pre>
</div>

<a id="ys-node-animal-fox"></a>

## animal_fox

<div class="yarn-node" data-title="animal_fox">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_fox&gt;&gt;</span>
<span class="yarn-line">Héhé ! Je ne viens pas du zoo, en fait.</span> <span class="yarn-meta">#line:03c4553 </span>
<span class="yarn-line">Je n'ai vu aucun drapeau nulle part.</span> <span class="yarn-meta">#line:074fe29 </span>

</code>
</pre>
</div>

<a id="ys-node-item-chest"></a>

## item_chest

<div class="yarn-node" data-title="item_chest">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">group:</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $found_chest_cookies&gt;&gt;</span>
<span class="yarn-line">    C'est vide maintenant.</span> <span class="yarn-meta">#line:0810771 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $found_chest_cookies = true&gt;&gt;</span>
<span class="yarn-line">    Vous avez trouvé des biscuits !</span> <span class="yarn-meta">#line:03321d5 </span>
    <span class="yarn-cmd">&lt;&lt;cookies_add 5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>


