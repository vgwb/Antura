---
title: Le système scolaire (fr_02) - Script
hide:
---

# Le système scolaire (fr_02) - Script
[Quest Index](./index.fr.md) - Language: [english](./fr_02-script.md) - french - [polish](./fr_02-script.pl.md) - [italian](./fr_02-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-02-the-school-system/24/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1873232287#gid=1873232287)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_02%20Angers%20School/FR_02%20Angers%20School%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>
## quest_start

<div class="yarn-node" data-title="quest_start"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_02 | Schools (Angers)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $got_backpack = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack false&gt;&gt;</span>
<span class="yarn-line">Bienvenue à Angers ! C'est le premier jour d'école ! <span class="yarn-meta">#line:014887e </span></span>
<span class="yarn-line">Vous avez 10 ans et vous êtes en dernière année d’école primaire. <span class="yarn-meta">#line:063e8e0 </span></span>
<span class="yarn-line">Trouvez votre école et votre classe ! <span class="yarn-meta">#line:0f65a1b </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_FIND_SCHOOL task_find_school_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-quest-end"></a>
## quest_end

<div class="yarn-node" data-title="quest_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Le jeu est terminé ! Félicitations ! <span class="yarn-meta">#line:022962d </span></span>
<span class="yarn-line">C'est notre jour d'école <span class="yarn-meta">#line:038dacc </span></span>

<span class="yarn-cmd">&lt;&lt;jump post_quest_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-post-quest-activity"></a>
## post_quest_activity

<div class="yarn-node" data-title="post_quest_activity"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Dessinez maintenant un plan de votre classe ! <span class="yarn-meta">#line:09ac4f1 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-school-done"></a>
## task_find_school_done

<div class="yarn-node" data-title="task_find_school_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Vous avez trouvé votre école ! <span class="yarn-meta">#line:03ed76a </span></span>

</code></pre></div>

<a id="ys-node-task-find-school-desc"></a>
## task_find_school_desc

<div class="yarn-node" data-title="task_find_school_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trouvez votre école ! <span class="yarn-meta">#line:0da284c </span></span>

</code></pre></div>

<a id="ys-node-school-1"></a>
## school_1

<div class="yarn-node" data-title="school_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bienvenue ! C'est une école maternelle pour les enfants de 3 à 5 ans. <span class="yarn-meta">#line:096b721 </span></span>
<span class="yarn-line">Tu es trop grand pour nos petites chaises. Ton école est à proximité. <span class="yarn-meta">#line:07ed07b </span></span>


</code></pre></div>

<a id="ys-node-school-2"></a>
## school_2

<div class="yarn-node" data-title="school_2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_backpack&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_talk&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-school-2-welcome"></a>
## school_2_welcome

<div class="yarn-node" data-title="school_2_welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! Bienvenue ! Vous avez trouvé. C'est votre école primaire. <span class="yarn-meta">#line:092b309 </span></span>
<span class="yarn-line">On dirait que tu as oublié ton sac à dos. <span class="yarn-meta">#line:0dd7977 </span></span>
<span class="yarn-line">Vous avez besoin de vos livres pour la leçon. <span class="yarn-meta">#line:044385f </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_backpack&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-school-3"></a>
## school_3

<div class="yarn-node" data-title="school_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">actor: KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! C'est un collège pour les élèves de 11 à 15 ans. <span class="yarn-meta">#line:06478e9 </span></span>
<span class="yarn-line">Tu es presque assez grand, mais pas encore. Continue à chercher ! <span class="yarn-meta">#line:0df97f8 </span></span>

</code></pre></div>

<a id="ys-node-school-4"></a>
## school_4

<div class="yarn-node" data-title="school_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! C'est un lycée pour les 16-18 ans. <span class="yarn-meta">#line:04a0e4b </span></span>
<span class="yarn-line">Les étudiants y préparent l'examen du baccalauréat avant d'aller à l'université. <span class="yarn-meta">#line:027eba1 </span></span>
<span class="yarn-line">Tu es trop jeune. Essaie une autre école. <span class="yarn-meta">#line:0630d14 </span></span>

</code></pre></div>

<a id="ys-node-school-4-talk-man"></a>
## school_4_talk_man

<div class="yarn-node" data-title="school_4_talk_man"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! J'ai trouvé un sac à dos en venant ici. <span class="yarn-meta">#line:0c3b4fe </span></span>
<span class="yarn-line">Ces livres sont plus simples que ceux que mes élèves utilisent, <span class="yarn-meta">#line:0d3aba1 </span></span>
<span class="yarn-line">donc je ne pense pas que cela leur appartient. <span class="yarn-meta">#line:09c5297 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-line">Est-ce que ça pourrait être le tien ? <span class="yarn-meta">#line:0f44535 </span></span>

</code></pre></div>

<a id="ys-node-school-2-talk"></a>
## school_2_talk

<div class="yarn-node" data-title="school_2_talk"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Et voilà ! Entrons et commençons la leçon. <span class="yarn-meta">#line:0624437 </span></span>
<span class="yarn-cmd">&lt;&lt;action door_open_2&gt;&gt;</span>
<span class="yarn-line">Entrez ! Votre salle de classe est au bout du couloir. <span class="yarn-meta">#line:0b91268 </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_find_classroom &gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-classroom"></a>
## task_find_classroom

<div class="yarn-node" data-title="task_find_classroom"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TÂCHE : Trouvez votre salle de classe. <span class="yarn-meta">#line:058ddbc </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_CLASSROOM task_class_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-classroom-desc"></a>
## task_classroom_desc

<div class="yarn-node" data-title="task_classroom_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trouvez votre classe pour pouvoir commencer la leçon. <span class="yarn-meta">#line:00e71a9 </span></span>

</code></pre></div>

<a id="ys-node-task-class-done"></a>
## task_class_done

<div class="yarn-node" data-title="task_class_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oui ! Vous pouvez commencer la leçon maintenant. <span class="yarn-meta">#line:093e91d </span></span>

</code></pre></div>

<a id="ys-node-classroom-1"></a>
## classroom_1

<div class="yarn-node" data-title="classroom_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">C'est la salle de classe du CP. <span class="yarn-meta">#line:08b2774 </span></span>
<span class="yarn-line">Les enfants commencent l’école primaire à 6 ans. <span class="yarn-meta">#line:09bfe7f </span></span>
<span class="yarn-line">Ce n'est pas votre cours. Essayez le cours d'à côté ! <span class="yarn-meta">#line:068e48b </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-2"></a>
## classroom_2

<div class="yarn-node" data-title="classroom_2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nous sommes en CE1, la deuxième année de l'école primaire. <span class="yarn-meta">#line:08f3ab2 </span></span>
<span class="yarn-line">Ce cours n'est pas pour vous. Réessayez ! <span class="yarn-meta">#line:029fd9c </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-3"></a>
## classroom_3

<div class="yarn-node" data-title="classroom_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! Ici la classe de CE2. <span class="yarn-meta">#line:0bce01a </span></span>
<span class="yarn-line">pour les élèves de 8 ans. <span class="yarn-meta">#line:0b411ce </span></span>
<span class="yarn-line">Ce n'est pas votre cours, mais vous en êtes proche ! <span class="yarn-meta">#line:0e4aa95 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_4&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-4"></a>
## classroom_4

<div class="yarn-node" data-title="classroom_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! Nous sommes en CM1, l'avant-dernière année de primaire. <span class="yarn-meta">#line:0532330 </span></span>
<span class="yarn-line">Votre cours est juste là-bas ! <span class="yarn-meta">#line:00f6294 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_5&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-5"></a>
## classroom_5

<div class="yarn-node" data-title="classroom_5"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Et voilà ! Bienvenue en CM2 ! <span class="yarn-meta">#line:02797f8 </span></span>
<span class="yarn-line">Commençons. <span class="yarn-meta">#line:0f90123 </span></span>
<span class="yarn-line">C'est une leçon d'écriture. <span class="yarn-meta">#line:0071a0d </span></span>
<span class="yarn-line">En France, on apprend à écrire en cursive. <span class="yarn-meta">#line:0f8995f </span></span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">Maintenant, nous faisons de la géométrie ! <span class="yarn-meta">#line:0365921 </span></span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match-geometry"></a>
## activity_match_geometry

<div class="yarn-node" data-title="activity_match_geometry"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">Associez chaque outil à la forme qu’il dessine. <span class="yarn-meta">#line:052d22a </span></span>
<span class="yarn-cmd">&lt;&lt;activity match_shapes activity_match_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match-done"></a>
## activity_match_done

<div class="yarn-node" data-title="activity_match_done"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_shapes") &gt; 0&gt;&gt;
<span class="yarn-line">    Excellent travail ! <span class="yarn-meta">#line:0e3f1fa </span></span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Ce n'est pas tout à fait exact. Réessayez. <span class="yarn-meta">#line:0a5c8f1</span></span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-backpack"></a>
## task_backpack

<div class="yarn-node" data-title="task_backpack"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=GUIDE, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trouvez votre sac à dos et revenez ensuite. <span class="yarn-meta">#line:0e3ad75 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_BACKPACK task_backpack_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-backpack-done"></a>
## task_backpack_done

<div class="yarn-node" data-title="task_backpack_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Tâche accomplie ! Vous pouvez entrer dans l'école. <span class="yarn-meta">#line:063f354  </span></span>
<span class="yarn-cmd">&lt;&lt;set $got_backpack = true&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-backpack-desc"></a>
## task_backpack_desc

<div class="yarn-node" data-title="task_backpack_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Trouve ton sac à dos pour pouvoir aller à l'école. <span class="yarn-meta">#line:00faf2f </span></span>

</code></pre></div>

<a id="ys-node-school-canteen"></a>
## school_canteen

<div class="yarn-node" data-title="school_canteen"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=WOMAN_OLD, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour ! J'espère que vous êtes inscrit à la cantine. <span class="yarn-meta">#line:021132d </span></span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>
<span class="yarn-line">Regardez le menu d'aujourd'hui ! <span class="yarn-meta">#line:0978619 </span></span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu zoom&gt;&gt;</span>
<span class="yarn-line">Bon appétit! <span class="yarn-meta">#line:0671097 </span></span>

</code></pre></div>

<a id="ys-node-school-charte"></a>
## school_charte

<div class="yarn-node" data-title="school_charte"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card concept_charter_of_secularism&gt;&gt;</span>
<span class="yarn-line">À l’école, tout le monde est le bienvenu et respecté, <span class="yarn-meta">#line:02977c9 </span></span>
<span class="yarn-line">peu importe leurs croyances. <span class="yarn-meta">#line:09c12ac </span></span>

</code></pre></div>

<a id="ys-node-spawned-kid-f"></a>
## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Nous avons une pause de jeu le matin et l'après-midi. <span class="yarn-meta">#line:027d036 </span></span>
<span class="yarn-line">J'aime dessiner et colorier. <span class="yarn-meta">#line:0879a44 </span></span>
<span class="yarn-line">Ma matière préférée est l'art. <span class="yarn-meta">#line:0cee587 </span></span>

</code></pre></div>

<a id="ys-node-spawned-kid-m"></a>
## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">J'aime les mathématiques à cause des chiffres. <span class="yarn-meta">#line:01b23d9 </span></span>
<span class="yarn-line">Je joue au football avec des amis. <span class="yarn-meta">#line:0b82cb5 </span></span>
<span class="yarn-line">J'adore les jeux vidéo ! <span class="yarn-meta">#line:0aeea98 </span></span>

</code></pre></div>

<a id="ys-node-spawned-adult"></a>
## spawned_adult

<div class="yarn-node" data-title="spawned_adult"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: adults </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">En France, l'école commence en septembre et se termine en juin. <span class="yarn-meta">#line:037e288 </span></span>
<span class="yarn-line">Il y a de nombreuses vacances dans l’année scolaire. <span class="yarn-meta">#line:028f2bf </span></span>
<span class="yarn-line">J'aime quand mes enfants me disent ce qu'ils ont appris. <span class="yarn-meta">#line:0d8d4aa </span></span>

</code></pre></div>

<a id="ys-node-spawned-teacher"></a>
## spawned_teacher

<div class="yarn-node" data-title="spawned_teacher"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: teachers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">L'école a lieu de 8h à 16h. <span class="yarn-meta">#line:0f6b039 </span></span>
<span class="yarn-line">Apportez votre déjeuner ou mangez à la cantine. <span class="yarn-meta">#line:008ffb5 </span></span>
<span class="yarn-line">Nous apprenons l’écriture cursive. <span class="yarn-meta">#line:0d88d5b </span></span>

</code></pre></div>


