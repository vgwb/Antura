---
title: Le système scolaire (fr_02) - Script
hide:
---

# Le système scolaire (fr_02) - Script
[Quest Index](./index.fr.md) - Language: [english](./fr_02-script.md) - french - [polish](./fr_02-script.pl.md) - [italian](./fr_02-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_02%20Angers%20School/FR_02%20Angers%20School%20-%20Yarn%20Script.yarn)  
    **Improve translations**: [comment here](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1233127135#gid=1233127135)  

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// </span>
</code></pre></div>

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// FR_02_ANGERS_SCHOOL - Jules Verne</span>
<span class="yarn-header-dim">// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// Words used: school, classroom, backpack, lesson, CP, CE1, CE2, CM1, CM2, Collège, Lycée, Baccalauréat, geometry, cursive, canteen/menu</span>
<span class="yarn-header-dim">// activities in this quest:</span>
<span class="yarn-header-dim">// - find your classroom (task)</span>
<span class="yarn-header-dim">// - find your backpack (task)</span>
<span class="yarn-header-dim">// - match tools to shapes (geometry activity)</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $MAX_PROGRESS = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $got_backpack = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack false&gt;&gt;</span>
<span class="yarn-line">TUTOR: Welcome Angers! It's the first day of school! <span class="yarn-meta">#line:014887e </span></span>
<span class="yarn-line">You are 10 years old and in the last year of elementary school. <span class="yarn-meta">#line:063e8e0 </span></span>
<span class="yarn-line">Find your school and your classroom! <span class="yarn-meta">#line:0f65a1b </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_SCHOOL task_school_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-school-desc"></a>
## task_school_desc

<div class="yarn-node" data-title="task_school_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your school!  <span class="yarn-meta">#line:0da284c </span></span>

</code></pre></div>

<a id="ys-node-school-1"></a>
## school_1

<div class="yarn-node" data-title="school_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Welcome! This is a nursery school, for our youngest pupils aged 3 to 5. <span class="yarn-meta">#line:096b721 </span></span>
<span class="yarn-line">You are too big for our small chairs! Your school is nearby. <span class="yarn-meta">#line:07ed07b </span></span>


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
<span class="yarn-line">Bonjour! Welcome! You've found it. This is your elementary school. <span class="yarn-meta">#line:092b309 </span></span>
<span class="yarn-line">It looks like you forgot your backpack. <span class="yarn-meta">#line:0dd7977 </span></span>
<span class="yarn-line">You'll need your books for the lesson. <span class="yarn-meta">#line:044385f </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_backpack&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-school-3"></a>
## school_3

<div class="yarn-node" data-title="school_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=KID_MALE</span>
<span class="yarn-header-dim">actor: KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hi! This is a middle school for ages 11 to 15. <span class="yarn-meta">#line:06478e9 </span></span>
<span class="yarn-line">You're almost old enough, but not yet. Keep looking! <span class="yarn-meta">#line:0df97f8 </span></span>

</code></pre></div>

<a id="ys-node-school-4"></a>
## school_4

<div class="yarn-node" data-title="school_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour! This is a high school for ages 16 to 18. <span class="yarn-meta">#line:04a0e4b </span></span>
<span class="yarn-line">Students study for the Baccalauréat exam here before university. <span class="yarn-meta">#line:027eba1 </span></span>
<span class="yarn-line">You're a bit too young. Try another school. <span class="yarn-meta">#line:0630d14 </span></span>

</code></pre></div>

<a id="ys-node-school-4-talk-man"></a>
## school_4_talk_man

<div class="yarn-node" data-title="school_4_talk_man"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=OLD_MAN</span>
<span class="yarn-header-dim">actor: OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! I found a backpack on my way here. <span class="yarn-meta">#line:0c3b4fe </span></span>
<span class="yarn-line">These books are simpler than the ones my students use, <span class="yarn-meta">#line:0d3aba1 </span></span>
<span class="yarn-line">so I don't think it belongs to them. <span class="yarn-meta">#line:09c5297 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-line">Could it be yours? <span class="yarn-meta">#line:0f44535 </span></span>

</code></pre></div>

<a id="ys-node-school-2-talk"></a>
## school_2_talk

<div class="yarn-node" data-title="school_2_talk"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">There it is! Let's go inside and start the lesson. <span class="yarn-meta">#line:0624437 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_1&gt;&gt;</span>
<span class="yarn-line">Come in! Your classroom is down the hall. <span class="yarn-meta">#line:0b91268 </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_find_classroom &gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-find-classroom"></a>
## task_find_classroom

<div class="yarn-node" data-title="task_find_classroom"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: TASK: Find your classroom. <span class="yarn-meta">#line:058ddbc </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_CLASSROOM task_class_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-classroom-desc"></a>
## task_classroom_desc

<div class="yarn-node" data-title="task_classroom_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your class so you can start the lesson.  <span class="yarn-meta">#line:00e71a9 </span></span>

</code></pre></div>

<a id="ys-node-classroom-1"></a>
## classroom_1

<div class="yarn-node" data-title="classroom_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the CP classroom. <span class="yarn-meta">#line:08b2774 </span></span>
<span class="yarn-line">Children start primary school at 6 years old. <span class="yarn-meta">#line:09bfe7f </span></span>
<span class="yarn-line">This is not your class. Try the next door! <span class="yarn-meta">#line:068e48b </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-2"></a>
## classroom_2

<div class="yarn-node" data-title="classroom_2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is CE1, <span class="yarn-meta">#line:0f699d3 </span></span>
<span class="yarn-line">the second year of primary school. <span class="yarn-meta">#line:08f3ab2 </span></span>
<span class="yarn-line">This is not your class. Try again! <span class="yarn-meta">#line:029fd9c </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_3&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-3"></a>
## classroom_3

<div class="yarn-node" data-title="classroom_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hi there! This is the CE2 classroom <span class="yarn-meta">#line:0bce01a </span></span>
<span class="yarn-line">for students who are 8 years old. <span class="yarn-meta">#line:0b411ce </span></span>
<span class="yarn-line">This is not your class, but you're getting close! <span class="yarn-meta">#line:0e4aa95 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_4&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-4"></a>
## classroom_4

<div class="yarn-node" data-title="classroom_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=OLD_WOMAN</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! This is CM1, <span class="yarn-meta">#line:043653e </span></span>
<span class="yarn-line">the second-to-last year of primary school. <span class="yarn-meta">#line:0532330 </span></span>
<span class="yarn-line">Your class is just over there! <span class="yarn-meta">#line:00f6294 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_5&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-classroom-5"></a>
## classroom_5

<div class="yarn-node" data-title="classroom_5"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">// asset cursive_writing - CC0 - https://commons.wikimedia.org/wiki/File:BlackBoard_(Blender_classroom_demo).png</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">There you are! Welcome to CM2! <span class="yarn-meta">#line:02797f8 </span></span>
<span class="yarn-line">Let's get to work. <span class="yarn-meta">#line:0f90123 </span></span>
<span class="yarn-line">Look, it's a writing lesson. <span class="yarn-meta">#line:0071a0d </span></span>
<span class="yarn-line">In France, we learn to write in cursive. <span class="yarn-meta">#line:0f8995f </span></span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">Now for geometry! <span class="yarn-meta">#line:0365921 </span></span>
<span class="yarn-line">Match each tool to the shape it draws. <span class="yarn-meta">#line:052d22a </span></span>
<span class="yarn-cmd">&lt;&lt;activity ACTIVITY_MATCH activity_match_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-activity-match-done"></a>
## activity_match_done

<div class="yarn-node" data-title="activity_match_done"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great job! <span class="yarn-meta">#line:0e3f1fa </span></span>
<span class="yarn-line">Every morning and afternoon we have a break to play outside. <span class="yarn-meta">#line:037391b </span></span>
<span class="yarn-line">You're all set. Welcome to the class! <span class="yarn-meta">#line:021df05 </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-school-done"></a>
## task_school_done

<div class="yarn-node" data-title="task_school_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: TASK COMPLETED!  <span class="yarn-meta">#line:03ed76a </span></span>

</code></pre></div>

<a id="ys-node-task-backpack-done"></a>
## task_backpack_done

<div class="yarn-node" data-title="task_backpack_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: TASK COMPLETED! You can now enter school. <span class="yarn-meta">#line:063f354  </span></span>
<span class="yarn-cmd">&lt;&lt;set $got_backpack = true&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-class-done"></a>
## task_class_done

<div class="yarn-node" data-title="task_class_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">TUTOR: TASK COMPLETED! You can now start the lesson. <span class="yarn-meta">#line:093e91d </span></span>

</code></pre></div>

<a id="ys-node-task-backpack"></a>
## task_backpack

<div class="yarn-node" data-title="task_backpack"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=GUIDE, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your backpack and then come back. <span class="yarn-meta">#line:0e3ad75 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_BACKPACK task_backpack_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-backpack-desc"></a>
## task_backpack_desc

<div class="yarn-node" data-title="task_backpack_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your backpack so you can go to school.  <span class="yarn-meta">#line:00faf2f </span></span>

</code></pre></div>

<a id="ys-node-school-canteen"></a>
## school_canteen

<div class="yarn-node" data-title="school_canteen"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=OLD_WOMAN, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! I hope you're registered for the canteen. <span class="yarn-meta">#line:021132d </span></span>
<span class="yarn-line">Look what's on the menu today! <span class="yarn-meta">#line:0978619 </span></span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-school-charte"></a>
## school_charte

<div class="yarn-node" data-title="school_charte"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card concept_charter_of_secularism&gt;&gt;</span>
<span class="yarn-line">At school, everyone is welcome and respected, <span class="yarn-meta">#line:02977c9 </span></span>
<span class="yarn-line">no matter their beliefs. <span class="yarn-meta">#line:09c12ac </span></span>

</code></pre></div>


