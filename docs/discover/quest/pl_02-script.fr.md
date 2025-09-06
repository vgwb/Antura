---
title: Le grand sauvetage des nains de Wrocław (pl_02) - Script
hide:
---

# Le grand sauvetage des nains de Wrocław (pl_02) - Script
[Quest Index](./index.fr.md) - Language: [english](./pl_02-script.md) - french - [polish](./pl_02-script.pl.md) - [italian](./pl_02-script.it.md)

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
<span class="yarn-cmd">&lt;&lt;declare $dwarf_1_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_2_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_3_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_4_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_5_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_6_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_7_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_8_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_9_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $dwarf_10_found = false&gt;&gt;</span>

<span class="yarn-line">Welcome to Wrocław!</span>

</code></pre></div>

<a id="ys-node-npg-task-dwarves"></a>
## npg_task_dwarves

<div class="yarn-node" data-title="npg_task_dwarves"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oh no! Antura is stuck at the top of the SKY TOWER. <span class="yarn-meta">#line:0a18e6c </span></span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_sky_tower zoom&gt;&gt;</span>
<span class="yarn-line">The dwarves locked the elevator. <span class="yarn-meta">#line:08bcfba </span></span>
<span class="yarn-line">Find 10 dwarves around Wrocław. They will help us. <span class="yarn-meta">#line:07471d4 </span></span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map zoom&gt;&gt;</span>
<span class="yarn-line">Explore the city. Talk to each dwarf you find. <span class="yarn-meta">#line:09148d9 </span></span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_DWARVES task_dwarves_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-task-dwarves-done"></a>
## task_dwarves_done

<div class="yarn-node" data-title="task_dwarves_done"><pre class="yarn-code"><code><span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the dwarves!</span>
<span class="yarn-line">Now go up the elevator!</span>

</code></pre></div>

<a id="ys-node-dwarf-1-origin"></a>
## dwarf_1_origin

<div class="yarn-node" data-title="dwarf_1_origin"><pre class="yarn-code"><code><span class="yarn-header-dim">// 1) The origin of dwarves</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarf_statue zoom&gt;&gt;</span>
<span class="yarn-line">Wrocław is the city of dwarfs. You can find them everywhere. <span class="yarn-meta">#line:007686b </span></span>
<span class="yarn-line">They are small and kind. They like to play tricks. <span class="yarn-meta">#line:0143908 </span></span>
<span class="yarn-line">Do you want to play with me?.</span>
<span class="yarn-line">-&gt; yes</span>
	<span class="yarn-cmd">&lt;&lt;activity jigsaw_dwarf_origin dwarf_1_origin_done&gt;&gt;</span>
<span class="yarn-line">-&gt; no</span>
<span class="yarn-line">	Oh, okay. Maybe later.</span>
	<span class="yarn-cmd">&lt;&lt;SetActive dwarf_1 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-1-origin-done"></a>
## dwarf_1_origin_done

<div class="yarn-node" data-title="dwarf_1_origin_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bravo! You solved the puzzle.</span>
<span class="yarn-line">Now i come with you</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_1_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_1 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-2-town-hall"></a>
## dwarf_2_town_hall

<div class="yarn-node" data-title="dwarf_2_town_hall"><pre class="yarn-code"><code><span class="yarn-header-dim">// 2) Old Town Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;asset wroclaw_old_town_hall&gt;&gt;</span>
<span class="yarn-line">This is the Old Town Hall. City leaders work here. <span class="yarn-meta">#line:02cbbf0 </span></span>
<span class="yarn-line">Meetings happen inside. The clock is very old. <span class="yarn-meta">#line:0ca6131 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_town_hall dwarf_2_town_hall_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-2-town-hall-done"></a>
## dwarf_2_town_hall_done

<div class="yarn-node" data-title="dwarf_2_town_hall_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done!</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_2_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_2 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-3-cathedral"></a>
## dwarf_3_cathedral

<div class="yarn-node" data-title="dwarf_3_cathedral"><pre class="yarn-code"><code><span class="yarn-header-dim">// 3) Cathedral with quiz</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card bishop_dwarf&gt;&gt;</span>
<span class="yarn-line">This is the Cathedral. It is a large and important church. <span class="yarn-meta">#line:00f2132 </span></span>
<span class="yarn-line">Do you know what is a CHURCH?</span>
<span class="yarn-line">-&gt; Yes</span>
<span class="yarn-line">-&gt; No</span>
 <span class="yarn-cmd">&lt;&lt;detour info_church&gt;&gt;</span>
<span class="yarn-line">It has tall towers and colorful stained-glass windows. <span class="yarn-meta">#line:08871e4 </span></span>
<span class="yarn-cmd">&lt;&lt;jump bishop_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-info-church"></a>
## info_church

<div class="yarn-node" data-title="info_church"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card church&gt;&gt;</span>
<span class="yarn-line">A church is a place where people pray. <span class="yarn-meta">#line:0b1f4e1</span></span>

</code></pre></div>

<a id="ys-node-dwarf-3-quiz"></a>
## dwarf_3__quiz

<div class="yarn-node" data-title="dwarf_3__quiz"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do people do in a cathedral? <span class="yarn-meta">#line:05ea00d </span></span>
<span class="yarn-line">-&gt; They shop for food. <span class="yarn-meta">#line:0c891cc </span></span>
<span class="yarn-line">   Not here. Try again. <span class="yarn-meta">#line:0d84e58 </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>
<span class="yarn-line">-&gt; They pray. <span class="yarn-meta">#line:01ac8e6 </span></span>
<span class="yarn-line">   Yes. You may collect the dwarf. <span class="yarn-meta">#line:071c92a </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_activity&gt;&gt;</span>
<span class="yarn-line">-&gt; They fly planes. <span class="yarn-meta">#line:09d9b34 </span></span>
<span class="yarn-line">   No. Try again. <span class="yarn-meta">#line:06ebc0c </span></span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-3-activity"></a>
## dwarf_3_activity

<div class="yarn-node" data-title="dwarf_3_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let' play a game</span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_wroclaw_cathedral dwarf_3_activity_done&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-dwarf-3-activity-done"></a>
## dwarf_3_activity_done

<div class="yarn-node" data-title="dwarf_3_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! He is a another Dwarf</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_3_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_3 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-zoo"></a>
## dwarf_4_zoo

<div class="yarn-node" data-title="dwarf_4_zoo"><pre class="yarn-code"><code><span class="yarn-header-dim">// 4) ZOO</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Visit the Wrocław ZOO, the biggest in Poland. <span class="yarn-meta">#line:0198e80 </span></span>
<span class="yarn-line">It has many animals from around the world. <span class="yarn-meta">#line:01b020e </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_4_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-activity"></a>
## dwarf_4_activity

<div class="yarn-node" data-title="dwarf_4_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play a game</span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_wroclaw_zoo dwarf_4_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-4-activity-done"></a>
## dwarf_4_activity_done

<div class="yarn-node" data-title="dwarf_4_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_4_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_4 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5"></a>
## dwarf_5

<div class="yarn-node" data-title="dwarf_5"><pre class="yarn-code"><code><span class="yarn-header-dim">// 5) Centennial Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Centennial Hall is huge. Inside you see shows and concerts. <span class="yarn-meta">#line:0ebb952 </span></span>
<span class="yarn-line">The roof looks like a giant dome. <span class="yarn-meta">#line:0f4189b </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_5_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5-activity"></a>
## dwarf_5_activity

<div class="yarn-node" data-title="dwarf_5_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's do something with it </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle centennial_hall dwarf_5_activity_done&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-5-activity-done"></a>
## dwarf_5_activity_done

<div class="yarn-node" data-title="dwarf_5_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_5_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_5 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-6"></a>
## dwarf_6

<div class="yarn-node" data-title="dwarf_6"><pre class="yarn-code"><code><span class="yarn-header-dim">// 6) Multimedia Fountain</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Water dances here with music and lights. <span class="yarn-meta">#line:0daf76d </span></span>
<span class="yarn-line">The shows are beautiful on summer nights. <span class="yarn-meta">#line:006ed40 </span></span>
<span class="yarn-cmd">&lt;&lt;jump dwarf_6_activity&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-6-activity"></a>
## dwarf_6_activity

<div class="yarn-node" data-title="dwarf_6_activity"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's see how you play this</span>
<span class="yarn-cmd">&lt;&lt;activity jigsaw_multimedia_fountain dwarf_6_activity_done&gt;&gt;</span>


</code></pre></div>

<a id="ys-node-dwarf-6-activity-done"></a>
## dwarf_6_activity_done

<div class="yarn-node" data-title="dwarf_6_activity_done"><pre class="yarn-code"><code><span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_6_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_6 false&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-7"></a>
## dwarf_7

<div class="yarn-node" data-title="dwarf_7"><pre class="yarn-code"><code><span class="yarn-header-dim">// 7) Panorama Racławicka</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A giant battle painting goes all around you. <span class="yarn-meta">#line:0de7f17 </span></span>
<span class="yarn-line">You stand inside the story. <span class="yarn-meta">#line:0f8436b </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle panorama_raclawicka tutorial&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-8"></a>
## dwarf_8

<div class="yarn-node" data-title="dwarf_8"><pre class="yarn-code"><code><span class="yarn-header-dim">// 8) Olga Tokarczuk</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Olga Tokarczuk is a famous writer who lives in Wrocław. <span class="yarn-meta">#line:0496de5 </span></span>
<span class="yarn-line">She won the Nobel Prize in Literature. <span class="yarn-meta">#line:00ae354 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle olga_tokarczuk tutorial&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-9"></a>
## dwarf_9

<div class="yarn-node" data-title="dwarf_9"><pre class="yarn-code"><code><span class="yarn-header-dim">// 9) Sky Tower Plaza Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">This is the modern plaza near the Sky Tower. <span class="yarn-meta">#line:096a9ee </span></span>
<span class="yarn-line">People meet here to talk and play. <span class="yarn-meta">#line:0c899a5 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle plaza_dwarf tutorial&gt;&gt;</span>

</code></pre></div>

<a id="ys-node-dwarf-10"></a>
## dwarf_10

<div class="yarn-node" data-title="dwarf_10"><pre class="yarn-code"><code><span class="yarn-header-dim">// 9) Sky Tower Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">tags: actor=DWARF</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The Sky Tower is very tall. <span class="yarn-meta">#line:0ccd434 </span></span>
<span class="yarn-line">You can see very far from the top. <span class="yarn-meta">#line:0170463 </span></span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle sky_tower_dwarf tutorial&gt;&gt;</span>

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


