---
title: System edukacji (fr_02) - Script
hide:
---

# System edukacji (fr_02) - Script
[Quest Index](./index.pl.md) - Language: [english](./fr_02-script.md) - [french](./fr_02-script.fr.md) - polish - [italian](./fr_02-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/fr-02-the-school-system/24/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1873232287#gid=1873232287)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_02%20Angers%20School/FR_02%20Angers%20School%20-%20Yarn%20Script.yarn)  

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">// fr_02 | Schools (Angers)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $got_backpack = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack false&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome Angers! It's the first day of school!] <span class="yarn-meta">#line:014887e </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You are 10 years old and in the last year of elementary school.] <span class="yarn-meta">#line:063e8e0 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Find your school and your classroom!] <span class="yarn-meta">#line:0f65a1b </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_FIND_SCHOOL task_find_school_done&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-the-end"></a>
## the_end

<div class="yarn-node" data-title="the_end"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel_endgame</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: The game is complete! Congratulations!]
[MISSING TRANSLATION: This is our school day]
[MISSING TRANSLATION: ]
<span class="yarn-cmd">&lt;&lt;jump quest_proposal&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-quest-proposal"></a>
## quest_proposal

<div class="yarn-node" data-title="quest_proposal"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">panel: panel</span>
<span class="yarn-header-dim">tags: proposal</span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: Now draw a map of your classroom!]
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-find-school-done"></a>
## task_find_school_done

<div class="yarn-node" data-title="task_find_school_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: TASK COMPLETED!] <span class="yarn-meta">#line:03ed76a </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-find-school-desc"></a>
## task_find_school_desc

<div class="yarn-node" data-title="task_find_school_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find your school!] <span class="yarn-meta">#line:0da284c </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-1"></a>
## school_1

<div class="yarn-node" data-title="school_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Welcome! This is a nursery school, for our youngest pupils aged 3 to 5.] <span class="yarn-meta">#line:096b721 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You are too big for our small chairs! Your school is nearby.] <span class="yarn-meta">#line:07ed07b </span></span>
[MISSING TRANSLATION: ]
[MISSING TRANSLATION: ]
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
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-2-welcome"></a>
## school_2_welcome

<div class="yarn-node" data-title="school_2_welcome"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Bonjour! Welcome! You've found it. This is your elementary school.] <span class="yarn-meta">#line:092b309 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: It looks like you forgot your backpack.] <span class="yarn-meta">#line:0dd7977 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You'll need your books for the lesson.] <span class="yarn-meta">#line:044385f </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_backpack&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-3"></a>
## school_3

<div class="yarn-node" data-title="school_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=KID_M</span>
<span class="yarn-header-dim">actor: KID_MALE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hi! This is a middle school for ages 11 to 15.] <span class="yarn-meta">#line:06478e9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You're almost old enough, but not yet. Keep looking!] <span class="yarn-meta">#line:0df97f8 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-4"></a>
## school_4

<div class="yarn-node" data-title="school_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Bonjour! This is a high school for ages 16 to 18.] <span class="yarn-meta">#line:04a0e4b </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Students study for the Baccalauréat exam here before university.] <span class="yarn-meta">#line:027eba1 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: You're a bit too young. Try another school.] <span class="yarn-meta">#line:0630d14 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-4-talk-man"></a>
## school_4_talk_man

<div class="yarn-node" data-title="school_4_talk_man"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello! I found a backpack on my way here.] <span class="yarn-meta">#line:0c3b4fe </span></span>
<span class="yarn-line">[MISSING TRANSLATION: These books are simpler than the ones my students use,] <span class="yarn-meta">#line:0d3aba1 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: so I don't think it belongs to them.] <span class="yarn-meta">#line:09c5297 </span></span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Could it be yours?] <span class="yarn-meta">#line:0f44535 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-2-talk"></a>
## school_2_talk

<div class="yarn-node" data-title="school_2_talk"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: There it is! Let's go inside and start the lesson.] <span class="yarn-meta">#line:0624437 </span></span>
<span class="yarn-cmd">&lt;&lt;action door_open_2&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Come in! Your classroom is down the hall.] <span class="yarn-meta">#line:0b91268 </span></span>
<span class="yarn-cmd">&lt;&lt;jump task_find_classroom &gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-find-classroom"></a>
## task_find_classroom

<div class="yarn-node" data-title="task_find_classroom"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=TUTOR, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: TASK: Find your classroom.] <span class="yarn-meta">#line:058ddbc </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_CLASSROOM task_class_done&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-classroom-desc"></a>
## task_classroom_desc

<div class="yarn-node" data-title="task_classroom_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find your class so you can start the lesson.] <span class="yarn-meta">#line:00e71a9 </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-class-done"></a>
## task_class_done

<div class="yarn-node" data-title="task_class_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: YES! You can now start the lesson.] <span class="yarn-meta">#line:093e91d </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-classroom-1"></a>
## classroom_1

<div class="yarn-node" data-title="classroom_1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=MAN</span>
<span class="yarn-header-dim">actor: MAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This is the CP classroom.] <span class="yarn-meta">#line:08b2774 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Children start primary school at 6 years old.] <span class="yarn-meta">#line:09bfe7f </span></span>
<span class="yarn-line">[MISSING TRANSLATION: This is not your class. Try the next door!] <span class="yarn-meta">#line:068e48b </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_2&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-classroom-2"></a>
## classroom_2

<div class="yarn-node" data-title="classroom_2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: This is CE1, the second year of primary school.] <span class="yarn-meta">#line:08f3ab2 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: This is not your class. Try again!] <span class="yarn-meta">#line:029fd9c </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_3&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-classroom-3"></a>
## classroom_3

<div class="yarn-node" data-title="classroom_3"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN</span>
<span class="yarn-header-dim">actor: WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hi there! This is the CE2 classroom] <span class="yarn-meta">#line:0bce01a </span></span>
<span class="yarn-line">[MISSING TRANSLATION: for students who are 8 years old.] <span class="yarn-meta">#line:0b411ce </span></span>
<span class="yarn-line">[MISSING TRANSLATION: This is not your class, but you're getting close!] <span class="yarn-meta">#line:0e4aa95 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_4&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-classroom-4"></a>
## classroom_4

<div class="yarn-node" data-title="classroom_4"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=WOMAN_OLD</span>
<span class="yarn-header-dim">actor: OLD_WOMAN</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello! This is CM1, the second-to-last year of primary school.] <span class="yarn-meta">#line:0532330 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Your class is just over there!] <span class="yarn-meta">#line:00f6294 </span></span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_5&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-classroom-5"></a>
## classroom_5

<div class="yarn-node" data-title="classroom_5"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: There you are! Welcome to CM2!] <span class="yarn-meta">#line:02797f8 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Let's get to work.] <span class="yarn-meta">#line:0f90123 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: Look, it's a writing lesson.] <span class="yarn-meta">#line:0071a0d </span></span>
<span class="yarn-line">[MISSING TRANSLATION: In France, we learn to write in cursive.] <span class="yarn-meta">#line:0f8995f </span></span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Now for geometry!] <span class="yarn-meta">#line:0365921 </span></span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-activity-match-geometry"></a>
## activity_match_geometry

<div class="yarn-node" data-title="activity_match_geometry"><pre class="yarn-code"><code><span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Match each tool to the shape it draws.] <span class="yarn-meta">#line:052d22a </span></span>
<span class="yarn-cmd">&lt;&lt;activity match_shapes activity_match_done&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-activity-match-done"></a>
## activity_match_done

<div class="yarn-node" data-title="activity_match_done"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_shapes") &gt; 0&gt;&gt;
<span class="yarn-line">    [MISSING TRANSLATION:     Great job!] <span class="yarn-meta">#line:0e3f1fa </span></span>
    <span class="yarn-cmd">&lt;&lt;jump the_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Not quite right, let's try again.] <span class="yarn-meta">#line:0a5c8f1</span></span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-backpack"></a>
## task_backpack

<div class="yarn-node" data-title="task_backpack"><pre class="yarn-code" style="--node-color:green"><code><span class="yarn-header-dim">tags: actor=GUIDE, task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find your backpack and then come back.] <span class="yarn-meta">#line:0e3ad75 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_BACKPACK task_backpack_done&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-backpack-done"></a>
## task_backpack_done

<div class="yarn-node" data-title="task_backpack_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: TASK COMPLETED! You can now enter school.] <span class="yarn-meta">#line:063f354  </span></span>
<span class="yarn-cmd">&lt;&lt;set $got_backpack = true&gt;&gt;</span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-task-backpack-desc"></a>
## task_backpack_desc

<div class="yarn-node" data-title="task_backpack_desc"><pre class="yarn-code"><code><span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Find your backpack so you can go to school.] <span class="yarn-meta">#line:00faf2f </span></span>
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-canteen"></a>
## school_canteen

<div class="yarn-node" data-title="school_canteen"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=WOMAN_OLD, </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">[MISSING TRANSLATION: Hello! I hope you're registered for the canteen.] <span class="yarn-meta">#line:021132d </span></span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: Look what's on the menu today!] <span class="yarn-meta">#line:0978619 </span></span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu zoom&gt;&gt;</span>
[MISSING TRANSLATION: Enjoy your meal!]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-school-charte"></a>
## school_charte

<div class="yarn-node" data-title="school_charte"><pre class="yarn-code" style="--node-color:yellow"><code><span class="yarn-header-dim">tags: actor=TUTOR</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card concept_charter_of_secularism&gt;&gt;</span>
<span class="yarn-line">[MISSING TRANSLATION: At school, everyone is welcome and respected,] <span class="yarn-meta">#line:02977c9 </span></span>
<span class="yarn-line">[MISSING TRANSLATION: no matter their beliefs.] <span class="yarn-meta">#line:09c12ac </span></span>
[MISSING TRANSLATION: ]
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
[MISSING TRANSLATION: =&gt; Every morning and afternoon we have a break to play outside.]
[MISSING TRANSLATION: =&gt; I like to draw and color.]
[MISSING TRANSLATION: =&gt; My favorite subject is art.]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-kid-m"></a>
## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; I like math because I like numbers.]
[MISSING TRANSLATION: =&gt; I like to play soccer with my friends]
[MISSING TRANSLATION: =&gt; I love videogames!]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-adult"></a>
## spawned_adult

<div class="yarn-node" data-title="spawned_adult"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: adults </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; In france the school year starts in September and ends in June.]
[MISSING TRANSLATION: =&gt; There are several holidays during the school year.]
[MISSING TRANSLATION: =&gt; I love when my kids tell me what they learned at school.]
[MISSING TRANSLATION: ]
</code></pre></div>

<a id="ys-node-spawned-teacher"></a>
## spawned_teacher

<div class="yarn-node" data-title="spawned_teacher"><pre class="yarn-code" style="--node-color:purple"><code><span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: teachers </span>
<span class="yarn-header-dim">---</span>
[MISSING TRANSLATION: =&gt; The school day is from 8am to 4pm.]
[MISSING TRANSLATION: =&gt; You can bring your lunch or eat at the canteen.]
[MISSING TRANSLATION: =&gt; We learn to write in cursive.]
[MISSING TRANSLATION: ]
</code></pre></div>


