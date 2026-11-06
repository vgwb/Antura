---
title: System edukacji (fr_02) - Script
hide:
---

# System edukacji (fr_02) - Script
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
<span class="yarn-line">Witamy w Angers! Angers to miasto w zachodniej Francji. Dziś pierwszy dzień szkoły! Czas wracać do szkoły!</span> <span class="yarn-meta">#line:014887e </span>
<span class="yarn-line">Szkoła zaczyna się ponownie we wrześniu. Masz 10 lat i idziesz do CM2.</span> <span class="yarn-meta">#line:063e8e0 </span>
<span class="yarn-line">Znajdź swoją szkołę i klasę!</span> <span class="yarn-meta">#line:0f65a1b #task:TASK_SCHOOL</span>
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
<span class="yarn-line">Gra skończona! Gratulacje!</span> <span class="yarn-meta">#line:022962d </span>
<span class="yarn-line">Wracamy do szkoły!</span> <span class="yarn-meta">#line:038dacc </span>
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
<span class="yarn-line">Narysuj swoją klasę.</span> <span class="yarn-meta">#line:09ac4f1 </span>
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
<span class="yarn-line">Znalazłeś swoją szkołę!</span> <span class="yarn-meta">#line:03ed76a </span>

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
<span class="yarn-line">Znajdź swoją szkołę!</span> <span class="yarn-meta">#line:0da284c </span>

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
<span class="yarn-line">Witamy! To przedszkole. To szkoła dla dzieci w wieku od 3 do 5 lat. W przedszkolu są trzy klasy: najmłodsza, średnia i najstarsza.</span> <span class="yarn-meta">#line:096b721 </span>
<span class="yarn-line">Jesteś za stary. Szkoła podstawowa jest tuż obok. Uczniowie mają od 6 do 10 lat.</span> <span class="yarn-meta">#line:07ed07b </span>

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
<span class="yarn-line">Witaj! Witamy! Znalazłeś swoją szkołę. Jesteś w szkole podstawowej. Jest 5 klas: CP, CE1, CE2, CM1 i CM2.</span> <span class="yarn-meta">#line:092b309</span>
<span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">Ale zapomniałeś zabrać plecaka szkolnego.</span> <span class="yarn-meta">#line:0dd7977 </span>
<span class="yarn-line">Potrzebujesz piórnika, ołówków, zeszytów i książek.</span> <span class="yarn-meta">#line:044385f </span>
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
<span class="yarn-line">I gotowe! Wchodzimy do klasy.</span> <span class="yarn-meta">#line:0624437 </span>
<span class="yarn-cmd">&lt;&lt;action door_open_2&gt;&gt;</span>
<span class="yarn-line">Proszę! Wasza klasa jest na końcu korytarza.</span> <span class="yarn-meta">#line:0b91268 </span>
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
<span class="yarn-line">Cześć! To jest szkoła. Uczniowie mają od 11 do 14 lat.</span> <span class="yarn-meta">#line:06478e9 </span>
<span class="yarn-line">Po ukończeniu CM2 uczniowie rozpoczynają naukę w szkole średniej. Rozpoczynają ją w wieku 11 lat i rozpoczynają szóstą klasę.</span> <span class="yarn-meta">#line:0df97f8 </span>

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
<span class="yarn-line">Szkoła średnia składa się z czterech klas: 6, 5, 4 i 3.</span> <span class="yarn-meta">#line:04a0e4b </span>
<span class="yarn-line">Uczniowie nazywani są uczniami szkół średnich. W szkole średniej są trzy klasy.</span> <span class="yarn-meta">#line:027eba1 </span>
<span class="yarn-line">Jesteś za młody, żeby iść do szkoły średniej lub gimnazjum.</span> <span class="yarn-meta">#line:0630d14 </span>

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
<span class="yarn-line">  Teraz znajdź swoją szkołę!</span> <span class="yarn-meta">#line:0b22d07 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
  <span class="yarn-cmd">&lt;&lt;card school_bag&gt;&gt;</span>
<span class="yarn-line">  Cześć! Znalazłem plecak szkolny.</span> <span class="yarn-meta">#line:0c3b4fe </span>
<span class="yarn-line">  Te książki są przydatne podczas nauki czytania. Dzięki nim nauczysz się alfabetu i liter.</span> <span class="yarn-meta">#line:04da48b </span>
<span class="yarn-line">  Czy to może Twoja książka?</span> <span class="yarn-meta">#line:0ce9646 </span>
<span class="yarn-line">  Ale najpierw musisz wygrać!</span> <span class="yarn-meta">#line:094eace </span>
<span class="yarn-line">  Przywróć klasom właściwą kolejność.</span> <span class="yarn-meta">#line:092fccb </span>
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
<span class="yarn-line">    Dobrze zrobiony!</span> <span class="yarn-meta">#line:07e8390 </span>
<span class="yarn-line">    Weź to!</span> <span class="yarn-meta">#line:078fff9 </span>
    <span class="yarn-cmd">&lt;&lt;SetActive Collect_Backpack&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif GetActivityResult("order_schools_settings") == 0&gt;&gt;</span>
<span class="yarn-line">    Przepraszamy! To nie jest prawidłowa odpowiedź. Spróbuj ponownie.</span> <span class="yarn-meta">#line:0e1ff90 </span>
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
<span class="yarn-line">Znajdź swoją klasę.</span> <span class="yarn-meta">#line:058ddbc #task:TASK_CLASSROOM</span>
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
<span class="yarn-line">Znajdź swoją klasę.</span> <span class="yarn-meta">#line:00e71a9 </span>

</code>
</pre>
</div>

<a id="ys-node-task-class-done"></a>

## task_class_done

<div class="yarn-node" data-title="task_class_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Możesz zająć miejsce. Oto twój stolik i krzesło.</span> <span class="yarn-meta">#line:093e91d </span>

</code>
</pre>
</div>

<a id="ys-node-classroom-1"></a>

## classroom_1

<div class="yarn-node" data-title="classroom_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">To jest klasa pierwsza.</span> <span class="yarn-meta">#line:08b2774 </span>
<span class="yarn-line">Dzieci rozpoczynają naukę w szkole podstawowej w wieku 6 lat w CP.</span> <span class="yarn-meta">#line:09bfe7f </span>
<span class="yarn-line">To nie twoja klasa. Masz 10 lat. Więc jesteś w CM2.</span> <span class="yarn-meta">#line:068e48b </span>
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
<span class="yarn-line">To jest klasa CE1.</span> <span class="yarn-meta">#line:08f3ab2 </span>
<span class="yarn-line">Pierwsza klasa to nie twoja klasa. Jesteś w piątej. Spróbuj jeszcze raz!</span> <span class="yarn-meta">#line:029fd9c </span>
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
<span class="yarn-line">Cześć! Tu klasa CE2.</span> <span class="yarn-meta">#line:0bce01a </span>
<span class="yarn-line">Uczniowie mają 8 lat.</span> <span class="yarn-meta">#line:0b411ce </span>
<span class="yarn-line">To nie są twoje zajęcia!</span> <span class="yarn-meta">#line:0e4aa95 </span>
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
<span class="yarn-line">CM1 to klasa po CE2.</span> <span class="yarn-meta">#line:0532330 </span>
<span class="yarn-line">Twoja klasa jest bardzo blisko!</span> <span class="yarn-meta">#line:00f6294 </span>
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
<span class="yarn-line">I proszę! To wasza klasa. Witamy w CM2!</span> <span class="yarn-meta">#line:02797f8 </span>
<span class="yarn-line">Usiądź przy stole.</span> <span class="yarn-meta">#line:0f90123 </span>
<span class="yarn-line">Weź piórnik i notes.</span> <span class="yarn-meta">#line:0071a0d </span>
<span class="yarn-line">Nauczyciel jest po to, żeby pomagać ci się uczyć.</span> <span class="yarn-meta">#line:0f8995f </span>
<span class="yarn-cmd">&lt;&lt;card concept_cursive_writing zoom&gt;&gt;</span>
<span class="yarn-line">Będziesz uczyć się matematyki. Nauczysz się liczyć, kalkulować.</span> <span class="yarn-meta">#line:0365921 </span>
<span class="yarn-cmd">&lt;&lt;jump activity_match_geometry&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-match-geometry"></a>

## activity_match_geometry

<div class="yarn-node" data-title="activity_match_geometry">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Ułóż litery alfabetu tak, aby powstało słowo SZKOŁA.</span> <span class="yarn-meta">#line:052d22a </span>
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
<span class="yarn-line">    Brawo! Udało Ci się!</span> <span class="yarn-meta">#line:0e3f1fa </span>
    <span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">To nie do końca prawda. Spróbuj jeszcze raz!</span> <span class="yarn-meta">#line:0a5c8f1</span>
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
<span class="yarn-line">Znajdź swój plecak i wróć do mnie.</span> <span class="yarn-meta">#line:0e3ad75 #task:TASK_BACKPACK</span>
<span class="yarn-line">Być może ktoś zabrał Twoją książkę.</span> <span class="yarn-meta">#line:0b43290 </span>
<span class="yarn-line">Zapytaj ludzi wokół siebie.</span> <span class="yarn-meta">#line:0c282e5 </span>
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
<span class="yarn-line">Brawo! Napisałeś słowo SZKOŁA. Możesz wejść do szkoły.</span> <span class="yarn-meta">#line:063f354  </span>
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
<span class="yarn-line">Znajdź swój plecak, żebyś mógł pójść do szkoły.</span> <span class="yarn-meta">#line:00faf2f </span>

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
<span class="yarn-line">Witaj! W porze lunchu możesz zjeść w stołówce lub w domu.</span> <span class="yarn-meta">#line:021132d </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu&gt;&gt;</span>
<span class="yarn-line">Spójrz na menu kawiarni.</span> <span class="yarn-meta">#line:0978619 </span>
<span class="yarn-cmd">&lt;&lt;card object_canteen_menu zoom&gt;&gt;</span>
<span class="yarn-line">Smacznego!</span> <span class="yarn-meta">#line:0671097 </span>

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
<span class="yarn-line">W szkole mile widziani są wszyscy: dziewczęta i chłopcy,</span> <span class="yarn-meta">#line:02977c9 </span>
<span class="yarn-line">niezależnie od ich przekonań.</span> <span class="yarn-meta">#line:09c12ac </span>

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
<span class="yarn-line">Rano i po południu jest przerwa. Podczas przerwy dzieci bawią się na placu zabaw.</span> <span class="yarn-meta">#line:027d036 </span>
<span class="yarn-line">Lubię rysować i kolorować.</span> <span class="yarn-meta">#line:0879a44 </span>
<span class="yarn-line">Uwielbiam muzykę i śpiewanie z moją klasą.</span> <span class="yarn-meta">#line:0cee587 </span>

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
<span class="yarn-line">Lubię matematykę, bo uwielbiam wykonywać obliczenia.</span> <span class="yarn-meta">#line:01b23d9 </span>
<span class="yarn-line">Gram w piłkę nożną z przyjaciółmi na placu zabaw.</span> <span class="yarn-meta">#line:0b82cb5 </span>
<span class="yarn-line">Uwielbiam uprawiać sport z moją klasą. Czasami chodzimy na basen.</span> <span class="yarn-meta">#line:0aeea98 </span>

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
<span class="yarn-line">We Francji szkoła zaczyna się we wrześniu i kończy w czerwcu.</span> <span class="yarn-meta">#line:037e288 </span>
<span class="yarn-line">W ciągu roku szkolnego są święta.</span> <span class="yarn-meta">#line:028f2bf </span>
<span class="yarn-line">W szkole uczymy się czytać, pisać, liczyć i mówić w nowych językach.</span> <span class="yarn-meta">#line:0d8d4aa </span>

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
<span class="yarn-line">Zajęcia odbywają się w godzinach 8:00-16:00.</span> <span class="yarn-meta">#line:0f6b039 </span>
<span class="yarn-line">W porze lunchu możesz zjeść posiłek w stołówce lub pójść do domu.</span> <span class="yarn-meta">#line:008ffb5 </span>
<span class="yarn-line">W szkole uczymy się czytać i pisać.</span> <span class="yarn-meta">#line:0d88d5b #card:concept_cursive_writing</span>

</code>
</pre>
</div>


