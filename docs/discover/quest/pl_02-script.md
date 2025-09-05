---
title: The great Wrocław dwarf rescue (pl_02) - Script
hide:
---

# The great Wrocław dwarf rescue (pl_02) - Script
[Quest Index](./index.md) - Language: english - [french](./pl_02-script.fr.md) - [polish](./pl_02-script.pl.md) - [italian](./pl_02-script.it.md)

!!! note "Educators & Designers: help improving this quest!"
    **Comments and feedback**: [discuss in the Forum](https://vgwb.discourse.group/t/pl-02-the-great-wroclaw-dwarf-rescue/33/1)  
    **Improve translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1721014062#gid=1721014062)  
    **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_02%20Wroclaw%20Dwarves/PL_02%20Wroclaw%20Dwarves%20-%20Yarn%20Script.yarn)  

<div class="yarn-node"><pre class="yarn-code"><code><span class="yarn-header-dim">// PL_02_WROCLAW_DWARVES - The great Wrocław dwarf rescue// </span>
</code></pre></div>

<a id="ys-node-init"></a>
## init

<div class="yarn-node" data-title="init"><pre class="yarn-code" style="--node-color:red"><code><span class="yarn-header-dim">=</span>
<span class="yarn-header-dim">// Location: Wrocław, Poland - Old Town and Market Square</span>
<span class="yarn-header-dim">// Cards:</span>
<span class="yarn-header-dim">// - wroclaw_dwarf_statue (cultural symbol)</span>
<span class="yarn-header-dim">// - PolishDwarf (cultural tradition)</span>
<span class="yarn-header-dim">// - wroclaw_market_square (city center)</span>
<span class="yarn-header-dim">// - WroclawOldTownHall (historical building)</span>
<span class="yarn-header-dim">// - WroclawCathedral (religious architecture)</span>
<span class="yarn-header-dim">// - WroclawSkyTower (modern landmark)</span>
<span class="yarn-header-dim">// Tasks:</span>
<span class="yarn-header-dim">// - dwarf statue collection throughout city</span>
<span class="yarn-header-dim">// Words used:Wrocław, dwarf, statue, Market Square, Old Town, cathedral, tradition, collection, city, Poland</span>
<span class="yarn-header-dim">tags: type=Start</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $found = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $need = 10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_0 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_1 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_2 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_3 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_4 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_5 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_6 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_7 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_8 = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_9 = false&gt;&gt;</span>
<span class="yarn-line">Oh no! Antura is stuck at the top of the SKY TOWER. <span class="yarn-meta">#line:0a18e6c </span></span>
<span class="yarn-line">The dwarves locked the elevator. <span class="yarn-meta">#line:08bcfba </span></span>

</code></pre></div>

<a id="ys-node-task-dwarves"></a>
## task_dwarves

<div class="yarn-node" data-title="task_dwarves"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_DWARVES&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;asset wroclaw_dwarves_overview&gt;&gt;</span>
<span class="yarn-line">Find 10 dwarves around Wrocław. They will help us. <span class="yarn-meta">#line:07471d4 </span></span>
<span class="yarn-line">Explore the city. Talk to each dwarf you find. <span class="yarn-meta">#line:09148d9 </span></span>

</code></pre></div>

<a id="ys-node-dwarf-origin"></a>
## dwarf_origin

<div class="yarn-node" data-title="dwarf_origin"><pre class="yarn-code"><code><span class="yarn-header-dim">// 0) The origin of dwarves</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wrocław is the city of dwarfs. You can find them everywhere. <span class="yarn-meta">#line:007686b </span></span>
<span class="yarn-line">They are small and kind. They like to play tricks. <span class="yarn-meta">#line:0143908 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_dwarf_origin tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_0&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_0 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:0059028 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-town-hall"></a>
## dwarf_town_hall

<div class="yarn-node" data-title="dwarf_town_hall"><pre class="yarn-code"><code><span class="yarn-header-dim">// 1) Old Town Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the Old Town Hall. City leaders work here. <span class="yarn-meta">#line:02cbbf0 </span></span>
<span class="yarn-line">Meetings happen inside. The clock is very old. <span class="yarn-meta">#line:0ca6131 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_town_hall tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_1 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:07867ac </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-cathedral"></a>
## dwarf_cathedral

<div class="yarn-node" data-title="dwarf_cathedral"><pre class="yarn-code"><code><span class="yarn-header-dim">// 2) Cathedral with quiz</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the Cathedral. It is a large and important church. <span class="yarn-meta">#line:00f2132 </span></span>
<span class="yarn-line">It has tall towers and colorful stained-glass windows. <span class="yarn-meta">#line:08871e4 </span></span>
<span class="yarn-cmd">&lt;&lt;jump bishop_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-bishop-quiz"></a>
## bishop_quiz

<div class="yarn-node" data-title="bishop_quiz"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF, type=Choice</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do people do in a cathedral? <span class="yarn-meta">#line:05ea00d </span></span>
<span class="yarn-line">-&gt; They shop for food. <span class="yarn-meta">#line:0c891cc </span></span>
<span class="yarn-line">	Not here. Try again. <span class="yarn-meta">#line:0d84e58 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump bishop_quiz&gt;&gt;</span>
<span class="yarn-line">-&gt; They pray. <span class="yarn-meta">#line:01ac8e6 </span></span>
<span class="yarn-line">	Yes. You may collect the dwarf. <span class="yarn-meta">#line:071c92a </span></span>
	<span class="yarn-cmd">&lt;&lt;jump cathedral_collect&gt;&gt;</span>
<span class="yarn-line">-&gt; They fly planes. <span class="yarn-meta">#line:09d9b34 </span></span>
<span class="yarn-line">	No. Try again. <span class="yarn-meta">#line:06ebc0c </span></span>
	<span class="yarn-cmd">&lt;&lt;jump bishop_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-cathedral-collect"></a>
## cathedral_collect

<div class="yarn-node" data-title="cathedral_collect"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_cathedral tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_2&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_2 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:0ac6a7d </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-zoo"></a>
## dwarf_zoo

<div class="yarn-node" data-title="dwarf_zoo"><pre class="yarn-code"><code><span class="yarn-header-dim">// 3) ZOO tie-in</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Visit the Wrocław ZOO, the biggest in Poland. <span class="yarn-meta">#line:0198e80 </span></span>
<span class="yarn-line">It has many animals from around the world. <span class="yarn-meta">#line:01b020e </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_zoo tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_3&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_3 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:0f4a3c9 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-centennial-hall"></a>
## dwarf_centennial_hall

<div class="yarn-node" data-title="dwarf_centennial_hall"><pre class="yarn-code"><code><span class="yarn-header-dim">// 4) Centennial Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Centennial Hall is huge. Inside you see shows and concerts. <span class="yarn-meta">#line:0ebb952 </span></span>
<span class="yarn-line">The roof looks like a giant dome. <span class="yarn-meta">#line:0f4189b </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle centennial_hall tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_4&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_4 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:038d2f9 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-fountain"></a>
## dwarf_fountain

<div class="yarn-node" data-title="dwarf_fountain"><pre class="yarn-code"><code><span class="yarn-header-dim">// 5) Multimedia Fountain</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Water dances here with music and lights. <span class="yarn-meta">#line:0daf76d </span></span>
<span class="yarn-line">The shows are beautiful on summer nights. <span class="yarn-meta">#line:006ed40 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle multimedia_fountain tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_5&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_5 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:004a763 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-panorama"></a>
## dwarf_panorama

<div class="yarn-node" data-title="dwarf_panorama"><pre class="yarn-code"><code><span class="yarn-header-dim">// 6) Panorama Racławicka</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A giant battle painting goes all around you. <span class="yarn-meta">#line:0de7f17 </span></span>
<span class="yarn-line">You stand inside the story. <span class="yarn-meta">#line:0f8436b </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle panorama_raclawicka tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_6&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_6 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:0130584 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-olga"></a>
## dwarf_olga

<div class="yarn-node" data-title="dwarf_olga"><pre class="yarn-code"><code><span class="yarn-header-dim">// 7) Olga Tokarczuk</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Olga Tokarczuk is a famous writer who lives in Wrocław. <span class="yarn-meta">#line:0496de5 </span></span>
<span class="yarn-line">She won the Nobel Prize in Literature. <span class="yarn-meta">#line:00ae354 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle olga_tokarczuk tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_7&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_7 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:067f80a </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-plaza"></a>
## dwarf_plaza

<div class="yarn-node" data-title="dwarf_plaza"><pre class="yarn-code"><code><span class="yarn-header-dim">// 8) Sky Tower Plaza Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the modern plaza near the Sky Tower. <span class="yarn-meta">#line:096a9ee </span></span>
<span class="yarn-line">People meet here to talk and play. <span class="yarn-meta">#line:0c899a5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle plaza_dwarf tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_8&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_8 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:0314653 </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-sky-tower"></a>
## dwarf_sky_tower

<div class="yarn-node" data-title="dwarf_sky_tower"><pre class="yarn-code"><code><span class="yarn-header-dim">// 9) Sky Tower Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Sky Tower is very tall. <span class="yarn-meta">#line:0ccd434 </span></span>
<span class="yarn-line">You can see very far from the top. <span class="yarn-meta">#line:0170463 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle sky_tower_dwarf tutorial&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if !$dwarf_9&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $dwarf_9 = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-line">A dwarf joined us. <span class="yarn-meta">#line:060daca </span></span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elevator-keymaster"></a>
## elevator_keymaster

<div class="yarn-node" data-title="elevator_keymaster"><pre class="yarn-code"><code><span class="yarn-header-dim">// Keymaster at the elevator</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I guard the elevator. <span class="yarn-meta">#line:0dd986c </span></span>
&lt;&lt;if $found &lt; $need&gt;&gt;
<span class="yarn-line">You still need more dwarves. Keep exploring the city. <span class="yarn-meta">#line:08dfa28 </span></span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">Great! You found them all. I open the door with my key. <span class="yarn-meta">#line:0fdc177 </span></span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-elevator-up"></a>
## elevator_up

<div class="yarn-node" data-title="elevator_up"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The elevator goes up... Ding! <span class="yarn-meta">#line:0abe973 </span></span>
<span class="yarn-line">The view is beautiful. <span class="yarn-meta">#line:008b500 </span></span>

</code></pre></div>

<a id="ys-node-rescue-top"></a>
## rescue_top

<div class="yarn-node" data-title="rescue_top"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=ANTURA</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I'm here! Thank you! <span class="yarn-meta">#line:066b636 </span></span>
<span class="yarn-line">Wink... POOF! <span class="yarn-meta">#line:0a6619b </span></span>
<span class="yarn-line">Magic! Antura is playful. <span class="yarn-meta">#line:0de1060 </span></span>


</code></pre></div>

<a id="ys-node-assessment-intro"></a>
## assessment_intro

<div class="yarn-node" data-title="assessment_intro"><pre class="yarn-code"><code><span class="yarn-header-dim">// Final Assessment</span>
<span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Quick check. <span class="yarn-meta">#line:0816525 </span></span>
<span class="yarn-line">Two short questions. <span class="yarn-meta">#line:03ddff0 </span></span>
<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-q1"></a>
## assessment_q1

<div class="yarn-node" data-title="assessment_q1"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What is the symbol of Wrocław? <span class="yarn-meta">#line:020c66a </span></span>
<span class="yarn-line">-&gt; a dog <span class="yarn-meta">#line:03ffcee </span></span>
<span class="yarn-line">	Not this. <span class="yarn-meta">#line:07344d2 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">-&gt; a monkey <span class="yarn-meta">#line:0760b20 </span></span>
<span class="yarn-line">	No. <span class="yarn-meta">#line:0868c5e </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">-&gt; a dwarf <span class="yarn-meta">#line:0972bf0 </span></span>
<span class="yarn-line">	Correct! <span class="yarn-meta">#line:088b881 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-q2"></a>
## assessment_q2

<div class="yarn-node" data-title="assessment_q2"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wrocław is the ... biggest city of Poland. <span class="yarn-meta">#line:09d303d </span></span>
<span class="yarn-line">-&gt; first <span class="yarn-meta">#line:0ce9571 </span></span>
<span class="yarn-line">	No. <span class="yarn-meta">#line:039419a </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">-&gt; second <span class="yarn-meta">#line:0fea2e7 </span></span>
<span class="yarn-line">	Not this. <span class="yarn-meta">#line:028148c </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">-&gt; third <span class="yarn-meta">#line:0ebe219 </span></span>
<span class="yarn-line">	Correct! Well done. <span class="yarn-meta">#line:0802440 </span></span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_end&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-assessment-end"></a>
## assessment_end

<div class="yarn-node" data-title="assessment_end"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You finished the quest! <span class="yarn-meta">#line:0fc1bad </span></span>
<span class="yarn-cmd">&lt;&lt;quest_end 3&gt;&gt;</span>

</code></pre></div>


