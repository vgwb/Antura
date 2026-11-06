---
title: The school system (fr_02) - Script
hide:
---

# The school system (fr_02) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://github.com/vgwb/Antura/discussions/629)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1873232287#gid=1873232287)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_02%20Angers%20School/FR_02%20Angers%20School%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_02 | Schools (Angers)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">tags: </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $got_backpack = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack false&gt;&gt;</span>
<span class="yarn-line">Welcome to Angers! Angers is a city in western France. Today is the first day of school! It's back to school!</span> <span class="yarn-meta">#line:014887e </span>
<span class="yarn-line">School starts again in September. You are 10 years old and you are going into CM2.</span> <span class="yarn-meta">#line:063e8e0 </span>
<span class="yarn-line">Find your school and class!</span> <span class="yarn-meta">#line:0f65a1b #task:TASK_SCHOOL</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_SCHOOL task_find_school_done&gt;&gt;</span>

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
<span class="yarn-line">The game is over! Congratulations!</span> <span class="yarn-meta">#line:022962d </span>
<span class="yarn-line">It's back to school!</span> <span class="yarn-meta">#line:038dacc </span>
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
<span class="yarn-line">Draw your classroom.</span> <span class="yarn-meta">#line:09ac4f1 </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-school-done"></a>

## task_find_school_done

<div class="yarn-node" data-title="task_find_school_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You've found your school!</span> <span class="yarn-meta">#line:03ed76a </span>

</code>
</pre>
</div>

<a id="ys-node-task-find-school-desc"></a>

## task_find_school_desc

<div class="yarn-node" data-title="task_find_school_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your school!</span> <span class="yarn-meta">#line:0da284c </span>

</code>
</pre>
</div>

<a id="ys-node-school-1"></a>

## school_1

<div class="yarn-node" data-title="school_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_ecole_maternelle_fr&gt;&gt;</span>
<span class="yarn-line">Welcome! This is a preschool. It's a school for children aged 3 to 5. There are three classes in preschool: the youngest class, the middle class, and the oldest class.</span> <span class="yarn-meta">#line:096b721 </span>
<span class="yarn-line">You're too old. The elementary school is right next door. The students are between 6 and 10 years old.</span> <span class="yarn-meta">#line:07ed07b </span>

</code>
</pre>
</div>

<a id="ys-node-school-2"></a>

## school_2

<div class="yarn-node" data-title="school_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $got_backpack&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_talk&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump school_2_welcome&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-2-welcome"></a>

## school_2_welcome

<div class="yarn-node" data-title="school_2_welcome">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_ecole_primaire_fr&gt;&gt;</span>
<span class="yarn-line">Hello! Welcome! You've found your school. You're in elementary school. There are 5 classes: CP, CE1, CE2, CM1 and CM2.</span> <span class="yarn-meta">#line:092b309</span>
<span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">But you forgot to take your schoolbag.</span> <span class="yarn-meta">#line:0dd7977 </span>
<span class="yarn-line">You need your pencil case, your pencils, your notebooks and your books.</span> <span class="yarn-meta">#line:044385f </span>
<span class="yarn-cmd">&lt;&lt;jump task_backpack&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-2-talk"></a>

## school_2_talk

<div class="yarn-node" data-title="school_2_talk">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">And there you go! Enter the classroom.</span> <span class="yarn-meta">#line:0624437 </span>
<span class="yarn-cmd">&lt;&lt;action door_open_2&gt;&gt;</span>
<span class="yarn-line">Come in! Your classroom is at the end of the corridor.</span> <span class="yarn-meta">#line:0b91268 </span>
<span class="yarn-cmd">&lt;&lt;jump task_find_classroom &gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-school-3"></a>

## school_3

<div class="yarn-node" data-title="school_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_lycee_fr&gt;&gt;</span>
<span class="yarn-line">Hello! This is the school. The students are between 11 and 14 years old.</span> <span class="yarn-meta">#line:06478e9 </span>
<span class="yarn-line">After CM2, students enter middle school. They start middle school at age 11. They enter 6th grade.</span> <span class="yarn-meta">#line:0df97f8 </span>

</code>
</pre>
</div>

<a id="ys-node-school-4"></a>

## school_4

<div class="yarn-node" data-title="school_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card education_college_fr&gt;&gt;</span>
<span class="yarn-line">There are four grades in middle school: 6th, 5th, 4th and 3rd.</span> <span class="yarn-meta">#line:04a0e4b </span>
<span class="yarn-line">The students are called high school students. There are three classes in high school.</span> <span class="yarn-meta">#line:027eba1 </span>
<span class="yarn-line">You are too young to enter middle or high school.</span> <span class="yarn-meta">#line:0630d14 </span>

</code>
</pre>
</div>

<a id="ys-node-school-4-talk-man"></a>

## school_4_talk_man

<div class="yarn-node" data-title="school_4_talk_man">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_schools_settings") &gt; 0&gt;&gt;
<span class="yarn-line">  Now, find your school!</span> <span class="yarn-meta">#line:0b22d07 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
  <span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">  Hello! I found a schoolbag.</span> <span class="yarn-meta">#line:0c3b4fe </span>
<span class="yarn-line">  These books are useful when learning to read. You learn the alphabet and letters with these books.</span> <span class="yarn-meta">#line:04da48b </span>
<span class="yarn-line">  Is this perhaps your book?</span> <span class="yarn-meta">#line:0ce9646 </span>
<span class="yarn-line">  But first, you have to win it!</span> <span class="yarn-meta">#line:094eace </span>
<span class="yarn-line">  Put the classes back in order.</span> <span class="yarn-meta">#line:092fccb </span>
  <span class="yarn-cmd">&lt;&lt;activity order_schools_settings schools_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-schools-activity-done"></a>

## schools_activity_done

<div class="yarn-node" data-title="schools_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("order_schools_settings") &gt; 0&gt;&gt;
<span class="yarn-line">    Well done!</span> <span class="yarn-meta">#line:07e8390 </span>
<span class="yarn-line">    Take it!</span> <span class="yarn-meta">#line:078fff9 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetActivityResult("order_schools_settings") == 0&gt;&gt;</span>
<span class="yarn-line">    Sorry! That's not the right answer. Try again.</span> <span class="yarn-meta">#line:0e1ff90 </span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-find-classroom"></a>

## task_find_classroom

<div class="yarn-node" data-title="task_find_classroom">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your class.</span> <span class="yarn-meta">#line:058ddbc #task:TASK_CLASSROOM</span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_CLASSROOM task_class_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-classroom-desc"></a>

## task_classroom_desc

<div class="yarn-node" data-title="task_classroom_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your class.</span> <span class="yarn-meta">#line:00e71a9 </span>

</code>
</pre>
</div>

<a id="ys-node-task-class-done"></a>

## task_class_done

<div class="yarn-node" data-title="task_class_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You can take your seat. Here's your table and chair.</span> <span class="yarn-meta">#line:093e91d </span>

</code>
</pre>
</div>

<a id="ys-node-classroom-1"></a>

## classroom_1

<div class="yarn-node" data-title="classroom_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the first grade class.</span> <span class="yarn-meta">#line:08b2774 </span>
<span class="yarn-line">Children start primary school at age 6 in CP.</span> <span class="yarn-meta">#line:09bfe7f </span>
<span class="yarn-line">This isn't your class. You're 10 years old. So you're in CM2.</span> <span class="yarn-meta">#line:068e48b </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-2"></a>

## classroom_2

<div class="yarn-node" data-title="classroom_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the CE1 class.</span> <span class="yarn-meta">#line:08f3ab2 </span>
<span class="yarn-line">First grade isn't your class. You're in fifth grade. Try again!</span> <span class="yarn-meta">#line:029fd9c </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-3"></a>

## classroom_3

<div class="yarn-node" data-title="classroom_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! This is the CE2 class.</span> <span class="yarn-meta">#line:0bce01a </span>
<span class="yarn-line">The students are 8 years old.</span> <span class="yarn-meta">#line:0b411ce </span>
<span class="yarn-line">This isn't your class!</span> <span class="yarn-meta">#line:0e4aa95 </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-4"></a>

## classroom_4

<div class="yarn-node" data-title="classroom_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">CM1 is the class after CE2.</span> <span class="yarn-meta">#line:0532330 </span>
<span class="yarn-line">Your classroom is very close!</span> <span class="yarn-meta">#line:00f6294 </span>
<span class="yarn-cmd">&lt;&lt;action DOOR_OPEN_5&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-classroom-5"></a>

## classroom_5

<div class="yarn-node" data-title="classroom_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">And there you have it! This is your class. Welcome to CM2!</span> <span class="yarn-meta">#line:02797f8 </span>
<span class="yarn-line">Take a seat at your table.</span> <span class="yarn-meta">#line:0f90123 </span>
<span class="yarn-line">Take your pencil case and your notebook.</span> <span class="yarn-meta">#line:0071a0d </span>
<span class="yarn-line">The teacher is there to help you learn.</span> <span class="yarn-meta">#line:0f8995f </span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">You are going to do mathematics. You are going to learn to count, to calculate.</span> <span class="yarn-meta">#line:0365921 </span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-geometry"></a>

## activity_match_geometry

<div class="yarn-node" data-title="activity_match_geometry">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Rearrange the letters of the alphabet to spell the word SCHOOL.</span> <span class="yarn-meta">#line:052d22a </span>
<span class="yarn-cmd">&lt;&lt;activity match_shapes activity_match_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-done"></a>

## activity_match_done

<div class="yarn-node" data-title="activity_match_done">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("match_shapes") &gt; 0&gt;&gt;
<span class="yarn-line">    Well done! You succeeded!</span> <span class="yarn-meta">#line:0e3f1fa </span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">That's not entirely accurate. Try again!</span> <span class="yarn-meta">#line:0a5c8f1</span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack"></a>

## task_backpack

<div class="yarn-node" data-title="task_backpack">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">tags:  task</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">Find your schoolbag and then come back to see me.</span> <span class="yarn-meta">#line:0e3ad75 #task:TASK_BACKPACK</span>
<span class="yarn-line">Maybe someone took your book.</span> <span class="yarn-meta">#line:0b43290 </span>
<span class="yarn-line">Go ask the people around you.</span> <span class="yarn-meta">#line:0c282e5 </span>
<span class="yarn-cmd">&lt;&lt;task_start TASK_BACKPACK task_backpack_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack-done"></a>

## task_backpack_done

<div class="yarn-node" data-title="task_backpack_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done! You wrote the word SCHOOL. You can go into the school.</span> <span class="yarn-meta">#line:063f354  </span>
<span class="yarn-cmd">&lt;&lt;set $got_backpack = true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-backpack-desc"></a>

## task_backpack_desc

<div class="yarn-node" data-title="task_backpack_desc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">type: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Find your schoolbag so you can go to school.</span> <span class="yarn-meta">#line:00faf2f </span>

</code>
</pre>
</div>

<a id="ys-node-school-canteen"></a>

## school_canteen

<div class="yarn-node" data-title="school_canteen">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">tags:  </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello! At lunchtime, you can eat at the canteen or at home.</span> <span class="yarn-meta">#line:021132d </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>
<span class="yarn-line">Look at the cafeteria menu.</span> <span class="yarn-meta">#line:0978619 </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu zoom&gt;&gt;</span>
<span class="yarn-line">Enjoy your food!</span> <span class="yarn-meta">#line:0671097 </span>

</code>
</pre>
</div>

<a id="ys-node-school-charte"></a>

## school_charte

<div class="yarn-node" data-title="school_charte">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: </span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">tags: item</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card concept_charter_of_secularism&gt;&gt;</span>
<span class="yarn-line">At school, everyone is welcome: girls and boys,</span> <span class="yarn-meta">#line:02977c9 </span>
<span class="yarn-line">regardless of their beliefs.</span> <span class="yarn-meta">#line:09c12ac </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In the morning and afternoon, there is a recess. During recess, the children play in the playground.</span> <span class="yarn-meta">#line:027d036 </span>
<span class="yarn-line">I like to draw and color.</span> <span class="yarn-meta">#line:0879a44 </span>
<span class="yarn-line">I love music and singing with my class.</span> <span class="yarn-meta">#line:0cee587 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-m"></a>

## spawned_kid_m

<div class="yarn-node" data-title="spawned_kid_m">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_M</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I like mathematics because I love doing calculations.</span> <span class="yarn-meta">#line:01b23d9 </span>
<span class="yarn-line">I play football with my friends in the playground.</span> <span class="yarn-meta">#line:0b82cb5 </span>
<span class="yarn-line">I love doing sports with my class. Sometimes we go to the swimming pool.</span> <span class="yarn-meta">#line:0aeea98 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-adult"></a>

## spawned_adult

<div class="yarn-node" data-title="spawned_adult">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: adults </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">In France, school starts in September and ends in June.</span> <span class="yarn-meta">#line:037e288 </span>
<span class="yarn-line">There are holidays during the school year.</span> <span class="yarn-meta">#line:028f2bf </span>
<span class="yarn-line">At school, we learn to read, write, count and speak new languages.</span> <span class="yarn-meta">#line:0d8d4aa </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-teacher"></a>

## spawned_teacher

<div class="yarn-node" data-title="spawned_teacher">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: teachers </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Classes are held from 8 a.m. to 4 p.m.</span> <span class="yarn-meta">#line:0f6b039 </span>
<span class="yarn-line">At lunchtime, you can eat in the canteen or go home to eat.</span> <span class="yarn-meta">#line:008ffb5 </span>
<span class="yarn-line">At school, we learn to read and write.</span> <span class="yarn-meta">#line:0d88d5b #card:concept_cursive_writing</span>

</code>
</pre>
</div>


