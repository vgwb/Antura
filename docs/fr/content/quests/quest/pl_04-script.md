---
title: Le zoo (pl_04) - Script
hide:
---

# Le zoo (pl_04) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-04-the-zoo/35/1)  
> **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=819047762#gid=819047762)  
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
<span class="yarn-header-dim">image: centennial_hall_empty_flag</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $talked_animals = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $elephant_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $giraffe_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $lion_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $monkey_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $penguin_completed = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $found_chest_cookies = false&gt;&gt;</span>
<span class="yarn-line">Bienvenue au ZOO de WROCŁAW.</span> <span class="yarn-meta">#line:0fe55d1 </span>
<span class="yarn-line">Il y a beaucoup d'ANIMAUX ici !</span> <span class="yarn-meta">#line:005dd46 </span>
<span class="yarn-cmd">&lt;&lt;target target_director&gt;&gt;</span>

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
<span class="yarn-line">Dessine ton ANIMAL préféré.</span> <span class="yarn-meta">#line:0809ac5 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-talk"></a>

## director_talk

<div class="yarn-node" data-title="director_talk">
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
<span class="yarn-line">Oh non !</span> <span class="yarn-meta">#line:0c71176 </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Flagpole&gt;&gt;</span>
<span class="yarn-line">Oh non ! Le DRAPEAU a disparu !</span> <span class="yarn-meta">#line:09c6bf7 </span>
<span class="yarn-cmd">&lt;&lt;card iglica&gt;&gt;</span>
<span class="yarn-line">C'était sur l'IGLICA au CENTENNIAL HALL.</span> <span class="yarn-meta">#line:02f35e4 </span>
<span class="yarn-line">[MISSING TRANSLATION: which is a famous symbol of our city.]</span> <span class="yarn-meta">#line:0335bf7 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-line">Trouvez le DRAPEAU !</span> <span class="yarn-meta">#line:0da284c #task:TASK_ANIMALS</span>
<span class="yarn-line">Parlez aux ANIMAUX. L'un d'eux pourrait l'avoir.</span> <span class="yarn-meta">#line:012b933 </span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_ANIMALS task_animals_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-animal-peacock"></a>

## animal_peacock

<div class="yarn-node" data-title="animal_peacock">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">// ENTRANCE – AMBIENCE</span>
<span class="yarn-header-dim">//--------------------------------------------</span>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Je n'ai pas besoin d'un DRAPEAU.</span> <span class="yarn-meta">#line:0085a8a </span>
<span class="yarn-line">Ma queue est un DRAPEAU !</span> <span class="yarn-meta">#line:04fff6b </span>

</code>
</pre>
</div>

<a id="ys-node-animal-parrot"></a>

## animal_parrot

<div class="yarn-node" data-title="animal_parrot">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ZOO</span>
<span class="yarn-header-dim">actor: SPECIAL</span>
<span class="yarn-header-dim">image: zoo_gate</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Crac ! Rouge et jaune sont passés en courant !</span> <span class="yarn-meta">#line:0e84545 </span>

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
<span class="yarn-line">Hé hé ! Je ne viens pas vraiment du zoo.</span> <span class="yarn-meta">#line:03c4553 </span>
<span class="yarn-cmd">&lt;&lt;card animal_fox zoom&gt;&gt;</span>
<span class="yarn-line">Je n'ai vu aucun drapeau.</span> <span class="yarn-meta">#line:074fe29 </span>

</code>
</pre>
</div>

<a id="ys-node-item-chest"></a>

## item_chest

<div class="yarn-node" data-title="item_chest">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">group: actions</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $found_chest_cookies&gt;&gt;</span>
<span class="yarn-line">    Il n'y en a plus maintenant.</span> <span class="yarn-meta">#line:0810771 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;action open_chest&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $found_chest_cookies = true&gt;&gt;</span>
<span class="yarn-line">    Tu as trouvé des biscuits !</span> <span class="yarn-meta">#line:03321d5 </span>
    <span class="yarn-cmd">&lt;&lt;cookies_add 5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-talk"></a>

## elephant_talk

<div class="yarn-node" data-title="elephant_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 1 – ELEPHANT</span>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: elephant_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("jigsaw_zoo_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Tu as bonne mémoire !</span> <span class="yarn-meta">#line:092cdc9 </span>
<span class="yarn-line">    Tu veux réessayer ?</span> <span class="yarn-meta">#line:09523fa </span>
    <span class="yarn-cmd">&lt;&lt;activity memory_elephant_settings_hard elephant_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis le plus grand ANIMAL terrestre.</span> <span class="yarn-meta">#line:027b51f </span>
    <span class="yarn-cmd">&lt;&lt;card animal_elephant zoom&gt;&gt;</span>
<span class="yarn-line">    J'ai une bonne MÉMOIRE.</span> <span class="yarn-meta">#line:03a150c </span>
<span class="yarn-line">    Avez-vous une bonne MÉMOIRE ?</span> <span class="yarn-meta">#line:0f98478 </span>
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
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">Un drapeau ? Je n'en ai pas.</span> <span class="yarn-meta">#line:0b79d01 </span>
<span class="yarn-line">Si je le prenais, je m'en souviendrais !</span> <span class="yarn-meta">#line:0f124bf </span>
<span class="yarn-cmd">&lt;&lt;set $elephant_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_animals_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elephant-sign"></a>

## ELEPHANT_SIGN

<div class="yarn-node" data-title="ELEPHANT_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">image: elephant_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_elephant&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Elephants have thick skin and big ears.]</span> <span class="yarn-meta">#line:06c3bba </span>
<span class="yarn-line">[MISSING TRANSLATION: They are the largest land animals.]</span> <span class="yarn-meta">#line:0bc4ca4 </span>

</code>
</pre>
</div>

<a id="ys-node-elephant-kid"></a>

## ELEPHANT_KID

<div class="yarn-node" data-title="ELEPHANT_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: ELEPHANT</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">image: kid_elephant</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ses OREILLES sont plus grandes que moi !</span> <span class="yarn-meta">#line:0fc78ad </span>
<span class="yarn-line">Est-ce que ça peut me ventiler en été ?</span> <span class="yarn-meta">#line:004abc7 </span>


</code>
</pre>
</div>

<a id="ys-node-giraffe-talk"></a>

## giraffe_talk

<div class="yarn-node" data-title="giraffe_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 2 – GIRAFFE</span>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("canvas_giraffe_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Cool, non ?</span> <span class="yarn-meta">#line:04d057e </span>
<span class="yarn-line">    Tu veux réessayer ?</span> <span class="yarn-meta">#line:04f37fa </span>
    <span class="yarn-cmd">&lt;&lt;activity canvas_giraffe_settings_hard giraffe_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis l'ANIMAL le plus grand.</span> <span class="yarn-meta">#line:0d5c607 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_giraffe zoom&gt;&gt;</span>
<span class="yarn-line">    Mon long COU m'aide à atteindre les feuilles.</span> <span class="yarn-meta">#line:0a4d24e </span>
<span class="yarn-line">    Je mange des feuilles d'ACAILLON !</span> <span class="yarn-meta">#line:04b42f2 </span>
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
<span class="yarn-header-dim">image: giraffe_keeper</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;inventory animal_elephant add&gt;&gt;</span>
<span class="yarn-line">Je n'ai pas pris le DRAPEAU.</span> <span class="yarn-meta">#line:0877d6f </span>
<span class="yarn-line">C'est trop haut pour moi !</span> <span class="yarn-meta">#line:02d00e2 </span>
<span class="yarn-cmd">&lt;&lt;set $giraffe_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_animals_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-sign"></a>

## GIRAFFE_SIGN

<div class="yarn-node" data-title="GIRAFFE_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">image: giraffe_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_giraffe&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Giraffes are the tallest animals.]</span> <span class="yarn-meta">#line:03b36c0 </span>
<span class="yarn-line">[MISSING TRANSLATION: They have long necks to eat leaves from tall trees.]</span> <span class="yarn-meta">#line:0d9d52f </span>

</code>
</pre>
</div>

<a id="ys-node-giraffe-kid"></a>

## GIRAFFE_KID

<div class="yarn-node" data-title="GIRAFFE_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: GIRAFFE</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">image: kid_giraffe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Avec un COU comme ça !</span> <span class="yarn-meta">#line:068daeb </span>
<span class="yarn-line">Je pouvais voir ma maison d'ici.</span> <span class="yarn-meta">#line:0bee484 </span>


</code>
</pre>
</div>

<a id="ys-node-lion-talk"></a>

## lion_talk

<div class="yarn-node" data-title="lion_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 3 – LION</span>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: lion_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_lion_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Hé, réessaie.</span> <span class="yarn-meta">#line:0e53f7d </span>
<span class="yarn-line">    Je vais rendre ça plus difficile.</span> <span class="yarn-meta">#line:080d627 </span>
    <span class="yarn-cmd">&lt;&lt;activity order_lion_settings_hard lion_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un LION en AFRIQUE.</span> <span class="yarn-meta">#line:07f2e15 </span>
    <span class="yarn-cmd">&lt;&lt;card  animal_lion zoom&gt;&gt;</span>
<span class="yarn-line">    Je vis dans un PRIDE.</span> <span class="yarn-meta">#line:042266c </span>
<span class="yarn-line">    Regardez ce petit ourson !</span> <span class="yarn-meta">#line:0124e1c </span>
    <span class="yarn-cmd">&lt;&lt;card  animal_lion_cub zoom&gt;&gt;</span>
<span class="yarn-line">    Je grandis petit, puis moyen, puis vieux...</span> <span class="yarn-meta">#line:0774b92 </span>
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
<span class="yarn-cmd">&lt;&lt;inventory animal_lion add&gt;&gt;</span>
<span class="yarn-line">Vous devez trouver le DRAPEAU !</span> <span class="yarn-meta">#line:05da6d7 </span>
<span class="yarn-line">J'aime le regarder dans le vent.</span> <span class="yarn-meta">#line:01b3593 </span>
<span class="yarn-cmd">&lt;&lt;set $lion_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_animals_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-lion-sign"></a>

## LION_SIGN

<div class="yarn-node" data-title="LION_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">image: lion_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_lion&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Lions live in family groups.]</span> <span class="yarn-meta">#line:091881b </span>
<span class="yarn-line">[MISSING TRANSLATION: Adult males have manes.]</span> <span class="yarn-meta">#line:02b4a06 </span>
<span class="yarn-cmd">&lt;&lt;card animal_lion_cub&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: But young cubs don't.]</span> <span class="yarn-meta">#line:0d378cc </span>

</code>
</pre>
</div>

<a id="ys-node-lion-kid"></a>

## LION_KID

<div class="yarn-node" data-title="LION_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: LION</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">image: kid_lion</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Quel rugissement !</span> <span class="yarn-meta">#line:079f4e0 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-talk"></a>

## monkey_talk

<div class="yarn-node" data-title="monkey_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 4 – MONKEY</span>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: monkey_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_monkey_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Oh, tu es doué !</span> <span class="yarn-meta">#line:092486d </span>
<span class="yarn-line">    Sois rapide !</span> <span class="yarn-meta">#line:0c33d20 </span>
    <span class="yarn-cmd">&lt;&lt;activity match_monkey_settings_hard monkey_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un SINGE. Je grimpe aux ARBRES.</span> <span class="yarn-meta">#line:0867233 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_chimpanzee zoom&gt;&gt;</span>
<span class="yarn-line">    Nous sommes proches des HUMAINS !</span> <span class="yarn-meta">#line:0eaefd6 </span>
<span class="yarn-line">    Où ai-je trouvé ces FRUITS ?</span> <span class="yarn-meta">#line:079451b </span>
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
<span class="yarn-cmd">&lt;&lt;inventory animal_monkey add&gt;&gt;</span>
<span class="yarn-line">Je n'ai pas pris le DRAPEAU.</span> <span class="yarn-meta">#line:0c53945 </span>
<span class="yarn-line">Ce serait amusant de grimper sur ce poteau.</span> <span class="yarn-meta">#line:0a43c85</span>
<span class="yarn-cmd">&lt;&lt;set $monkey_completed = true&gt;&gt;</span> 
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_animals_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-monkey-sign"></a>

## MONKEY_SIGN

<div class="yarn-node" data-title="MONKEY_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">image: monkey_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_chimpanzee&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Chimpanzees are great climbers.]</span> <span class="yarn-meta">#line:01d80fc </span>
<span class="yarn-cmd">&lt;&lt;card food_apple&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: They eat lots of different fruits.]</span> <span class="yarn-meta">#line:0883b4b </span>
<span class="yarn-cmd">&lt;&lt;card plant_apple&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Every fruit comes from a different tree.]</span> <span class="yarn-meta">#line:0cda3c2 </span>

</code>
</pre>
</div>

<a id="ys-node-monkey-kid"></a>

## MONKEY_KID

<div class="yarn-node" data-title="MONKEY_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: MONKEY</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">image: kid_monkey</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Il a copié ma danse !</span> <span class="yarn-meta">#line:0bf2346 </span>
<span class="yarn-line">Les SINGES font-ils des clins d’œil ?</span> <span class="yarn-meta">#line:0eecdf9 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-talk"></a>

## penguin_talk

<div class="yarn-node" data-title="penguin_talk">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">// PART 5 – PENGUIN</span>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">image: penguin_keeper</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("jigsaw_penguin_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Bien joué !</span> <span class="yarn-meta">#line:0cd794c </span>
<span class="yarn-line">    Tu peux le refaire ?</span> <span class="yarn-meta">#line:084fedc </span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_penguin_settings_hard penguin_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    Je suis un PINGOUIN, un OISEAU étrange.</span> <span class="yarn-meta">#line:08c70e8 </span>
    <span class="yarn-cmd">&lt;&lt;card animal_penguin zoom&gt;&gt;</span>
<span class="yarn-line">    Je ne sais pas voler, mais je nage bien !</span> <span class="yarn-meta">#line:0540c5a </span>
<span class="yarn-line">    Pouvez-vous trouver le chemin dans ICE ?</span> <span class="yarn-meta">#line:0a3420c </span>
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
<span class="yarn-cmd">&lt;&lt;inventory animal_penguin add&gt;&gt;</span>
<span class="yarn-line">Non, je n'ai pas pris le DRAPEAU.</span> <span class="yarn-meta">#line:078190f </span>
<span class="yarn-line">Je ne peux pas voler, tu te souviens ?</span> <span class="yarn-meta">#line:08568f5 </span>
<span class="yarn-cmd">&lt;&lt;set $penguin_completed = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if HasCompletedTask("TASK_ANIMALS")&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump task_animals_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-penguin-sign"></a>

## PENGUIN_SIGN

<div class="yarn-node" data-title="PENGUIN_SIGN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">image: penguin_sign</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card animal_penguin&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Penguins are birds that can swim.]</span> <span class="yarn-meta">#line:0920adc </span>
<span class="yarn-cmd">&lt;&lt;card ice_arctic&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: They live near oceans and ice.]</span> <span class="yarn-meta">#line:0baa8d0 </span>

</code>
</pre>
</div>

<a id="ys-node-penguin-kid"></a>

## PENGUIN_KID

<div class="yarn-node" data-title="PENGUIN_KID">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: PENGUIN</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">image: kid_penguin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Comme c'est mignon !</span> <span class="yarn-meta">#line:0ae73f3 </span>
<span class="yarn-line">Ça a l'air soigné.</span> <span class="yarn-meta">#line:05ac327 </span>

</code>
</pre>
</div>

<a id="ys-node-task-animals-done"></a>

## task_animals_done

<div class="yarn-node" data-title="task_animals_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TÂCHE ACCOMPLIE ! Retournez voir le DIRECTEUR.</span> <span class="yarn-meta">#line:0a93d9b </span>
<span class="yarn-cmd">&lt;&lt;target target_director&gt;&gt;</span>
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
<span class="yarn-line">Bien. Tu as parlé à tous les ANIMAUX.</span> <span class="yarn-meta">#line:032811f </span>
<span class="yarn-line">Passons maintenant en revue les faits.</span> <span class="yarn-meta">#line:0364f30 </span>
<span class="yarn-line">[MISSING TRANSLATION: Help me put the pieces together.]</span> <span class="yarn-meta">#line:0bc6238 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_zoo_settings director_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-recap-cards"></a>

## RECAP_CARDS

<div class="yarn-node" data-title="RECAP_CARDS">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: Icebox</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: zoo_cards</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Associez maintenant les CARTES.</span> <span class="yarn-meta">#line:0f6f882 </span>
<span class="yarn-cmd">&lt;&lt;activity memory zoo_animal_cards tutorial&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-director-activity-done"></a>

## director_activity_done

<div class="yarn-node" data-title="director_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">image: zoo_recap</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tous les ANIMAUX sont innocents.</span> <span class="yarn-meta">#line:0bc2b46 </span>
<span class="yarn-line">Qui a pris le DRAPEAU ?</span> <span class="yarn-meta">#line:0fc0ab7 </span>
<span class="yarn-cmd">&lt;&lt;SetActive anturaFlag&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump RETURN_DIRECTOR&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-return-director"></a>

## RETURN_DIRECTOR

<div class="yarn-node" data-title="RETURN_DIRECTOR">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// TWIST – FLAG RETURN</span>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">image: centennial_hall_antura_flag</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Attends, regarde !</span> <span class="yarn-meta">#line:0b3d05f </span>
<span class="yarn-cmd">&lt;&lt;camera_focus Antura&gt;&gt;</span>
<span class="yarn-line">ANTURA a le DRAPEAU !</span> <span class="yarn-meta">#line:0e24973 </span>
<span class="yarn-cmd">&lt;&lt;camera_reset&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump CEREMONY_END&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-ceremony-end"></a>

## CEREMONY_END

<div class="yarn-node" data-title="CEREMONY_END">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: End</span>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">image: flag_on_iglica</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_flag&gt;&gt;</span>
<span class="yarn-line">Le DRAPEAU est chez nous.</span> <span class="yarn-meta">#line:0d91701 </span>
<span class="yarn-line">Merci, aide.</span> <span class="yarn-meta">#line:08c71db </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>


