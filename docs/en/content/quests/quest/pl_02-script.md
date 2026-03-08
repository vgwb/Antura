---
title: The great Wrocław dwarf rescue (pl_02) - Script
hide:
---

# The great Wrocław dwarf rescue (pl_02) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-02-the-great-wroclaw-dwarf-rescue/33/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1721014062#gid=1721014062)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_02%20Wroclaw%20Dwarves/PL_02%20Wroclaw%20Dwarves%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_02 | Dwarves (Wroclaw)</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">// ---------------------------------------------</span>
<span class="yarn-header-dim">// WANTED:</span>
<span class="yarn-header-dim">// - wroclaw_dwarf_statue (cultural symbol)</span>
<span class="yarn-header-dim">// - PolishDwarf (cultural tradition)</span>
<span class="yarn-header-dim">// - wroclaw_market_square (city center)</span>
<span class="yarn-header-dim">// - WroclawOldTownHall (historical building)</span>
<span class="yarn-header-dim">// - WroclawCathedral (religious architecture)</span>
<span class="yarn-header-dim">// - WroclawSkyTower (modern landmark)</span>
<span class="yarn-header-dim">tags:</span>
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
<span class="yarn-cmd">&lt;&lt;declare $top_met = false&gt;&gt;</span>
<span class="yarn-line">Welcome to Wrocław!</span> <span class="yarn-meta">#line:023b330 </span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map&gt;&gt;</span>
<span class="yarn-line">Let's explore the city and find the dwarves.</span> <span class="yarn-meta">#line:pl02_start_1</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target npg_task_dwarves&gt;&gt;</span>

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
<span class="yarn-line">This quest is complete.</span> <span class="yarn-meta">#line:04eb3f4 </span>
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
<span class="yarn-line">Why don't you draw your Dwarf?</span> <span class="yarn-meta">#line:0e7c76a </span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-magic-tram"></a>

## magic_tram

<div class="yarn-node" data-title="magic_tram">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Where do you want to jump to?</span> <span class="yarn-meta">#line:portal_intro</span>
<span class="yarn-line">The Old Town</span> <span class="yarn-meta">#line:portal_town</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_old_town&gt;&gt;</span>
<span class="yarn-line">The Centennial Hall</span> <span class="yarn-meta">#line:portal_park</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_centennial_hall&gt;&gt;</span>
<span class="yarn-line">The Sky Tower</span> <span class="yarn-meta">#line:portal_skytower</span>
    <span class="yarn-cmd">&lt;&lt;teleport tram_sky_tower&gt;&gt;</span>
<span class="yarn-line">Stay here</span> <span class="yarn-meta">#line:portal_cancel #highlight</span>
    <span class="yarn-cmd">&lt;&lt;stop&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npg-task-dwarves"></a>

## npg_task_dwarves

<div class="yarn-node" data-title="npg_task_dwarves">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Oh no! Antura is stuck at the top of the Sky Tower.</span> <span class="yarn-meta">#line:0a18e6c </span>
<span class="yarn-cmd">&lt;&lt;card wroclaw_sky_tower zoom&gt;&gt;</span>
<span class="yarn-line">The dwarves locked the elevator.</span> <span class="yarn-meta">#line:08bcfba </span>
<span class="yarn-line">Find 10 dwarves around Wrocław. They will help us.</span> <span class="yarn-meta">#line:07471d4 #task:FIND_DWARVES</span>
<span class="yarn-cmd">&lt;&lt;card wroklaw_map zoom&gt;&gt;</span>
<span class="yarn-line">Explore the city. Talk to each dwarf you find.</span> <span class="yarn-meta">#line:09148d9 </span>
<span class="yarn-cmd">&lt;&lt;task_start FIND_DWARVES task_dwarves_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-dwarves-done"></a>

## task_dwarves_done

<div class="yarn-node" data-title="task_dwarves_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: task</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You found all the dwarves!</span> <span class="yarn-meta">#line:03df148 </span>
<span class="yarn-line">Now go up the elevator!</span> <span class="yarn-meta">#line:0364c03 </span>
<span class="yarn-cmd">&lt;&lt;task_end FIND_DWARVES&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-1-origin"></a>

## dwarf_1_origin

<div class="yarn-node" data-title="dwarf_1_origin">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 1) The origin of dwarves</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_1_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! Let's find the other dwarves.</span> <span class="yarn-meta">#line:pl02_d1_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card wroclaw_dwarf_statue zoom&gt;&gt;</span>
<span class="yarn-line">	Wrocław is the city of dwarves.</span> <span class="yarn-meta">#line:007686b </span>
<span class="yarn-line">	Dwarves are small and playful.</span> <span class="yarn-meta">#line:pl02_d1_1</span>
	
<span class="yarn-line">	What is the symbol of Wrocław?</span> <span class="yarn-meta">#line:pl02_d1_q</span>
<span class="yarn-line">	A dwarf</span> <span class="yarn-meta">#line:pl02_d1_a1</span>
<span class="yarn-line">		Correct! Let's play a puzzle.</span> <span class="yarn-meta">#line:pl02_d1_ok</span>
		<span class="yarn-cmd">&lt;&lt;activity jigsaw_dwarf_origin dwarf_1_origin_done&gt;&gt;</span>
<span class="yarn-line">	A dog</span> <span class="yarn-meta">#line:pl02_d1_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d1_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_1_origin&gt;&gt;</span>
<span class="yarn-line">	A car</span> <span class="yarn-meta">#line:pl02_d1_a3</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d1_no2</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_1_origin&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-1-origin-done"></a>

## dwarf_1_origin_done

<div class="yarn-node" data-title="dwarf_1_origin_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job! You solved the puzzle.</span> <span class="yarn-meta">#line:0a87d03 </span>
<span class="yarn-line">Now I come with you.</span> <span class="yarn-meta">#line:0f49b50 </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_1_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_1 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-2-town-hall"></a>

## dwarf_2_town_hall

<div class="yarn-node" data-title="dwarf_2_town_hall">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 2) Old Town Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_2_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! Keep exploring.</span> <span class="yarn-meta">#line:pl02_d2_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;asset wroclaw_old_town_hall&gt;&gt;</span>
<span class="yarn-line">	This is the Old Town Hall.</span> <span class="yarn-meta">#line:02cbbf0 </span>
<span class="yarn-line">	City leaders work here.</span> <span class="yarn-meta">#line:pl02_d2_1</span>
	
<span class="yarn-line">	What do city leaders do here?</span> <span class="yarn-meta">#line:pl02_d2_q</span>
<span class="yarn-line">	They work and meet</span> <span class="yarn-meta">#line:pl02_d2_a1</span>
<span class="yarn-line">		Yes! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d2_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_2_activity&gt;&gt;</span>
<span class="yarn-line">	They swim</span> <span class="yarn-meta">#line:pl02_d2_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d2_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_2_town_hall&gt;&gt;</span>
<span class="yarn-line">	They fly planes</span> <span class="yarn-meta">#line:pl02_d2_a3</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d2_no2</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_2_town_hall&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-2-activity"></a>

## dwarf_2_activity

<div class="yarn-node" data-title="dwarf_2_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_old_town_hall dwarf_2_town_hall_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-2-town-hall-done"></a>

## dwarf_2_town_hall_done

<div class="yarn-node" data-title="dwarf_2_town_hall_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Well done!</span> <span class="yarn-meta">#line:004110a </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_2_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_2 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-3-cathedral"></a>

## dwarf_3_cathedral

<div class="yarn-node" data-title="dwarf_3_cathedral">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 3) Cathedral with quiz</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_3_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! The cathedral is beautiful.</span> <span class="yarn-meta">#line:pl02_d3_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card bishop_dwarf&gt;&gt;</span>
<span class="yarn-line">	This is the cathedral.</span> <span class="yarn-meta">#line:00f2132 </span>
<span class="yarn-line">	It is a big church.</span> <span class="yarn-meta">#line:pl02_d3_1</span>
	
<span class="yarn-line">	Do you know what a church is?</span> <span class="yarn-meta">#line:080f821 </span>
<span class="yarn-line">	Yes</span> <span class="yarn-meta">#line:07550a5 </span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>
<span class="yarn-line">	No</span> <span class="yarn-meta">#line:09e7452 </span>
		<span class="yarn-cmd">&lt;&lt;detour info_church&gt;&gt;</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-info-church"></a>

## info_church

<div class="yarn-node" data-title="info_church">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">A church is a place where people pray.</span> <span class="yarn-meta">#line:0b1f4e1</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-3-quiz"></a>

## dwarf_3_quiz

<div class="yarn-node" data-title="dwarf_3_quiz">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What do people do in a cathedral?</span> <span class="yarn-meta">#line:05ea00d </span>
<span class="yarn-line">They shop for food.</span> <span class="yarn-meta">#line:0c891cc </span>
<span class="yarn-line">   Not here. Try again.</span> <span class="yarn-meta">#line:0d84e58 </span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>
<span class="yarn-line">They pray.</span> <span class="yarn-meta">#line:01ac8e6 </span>
<span class="yarn-line">   Yes. You may collect the dwarf.</span> <span class="yarn-meta">#line:071c92a </span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_activity&gt;&gt;</span>
<span class="yarn-line">They fly planes.</span> <span class="yarn-meta">#line:09d9b34 </span>
<span class="yarn-line">   No. Try again.</span> <span class="yarn-meta">#line:06ebc0c </span>
   <span class="yarn-cmd">&lt;&lt;jump dwarf_3_quiz&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-3-activity"></a>

## dwarf_3_activity

<div class="yarn-node" data-title="dwarf_3_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play a game.</span> <span class="yarn-meta">#line:0c5d8d3 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_cathedral dwarf_3_activity_done&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-dwarf-3-activity-done"></a>

## dwarf_3_activity_done

<div class="yarn-node" data-title="dwarf_3_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! He is another dwarf.</span> <span class="yarn-meta">#line:04886ba </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_3_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_3 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-4-zoo"></a>

## dwarf_4_zoo

<div class="yarn-node" data-title="dwarf_4_zoo">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 4) ZOO</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_4_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! I love animals.</span> <span class="yarn-meta">#line:pl02_d4_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Visit the Wrocław zoo.</span> <span class="yarn-meta">#line:0198e80 </span>
<span class="yarn-line">	It is the biggest zoo in Poland.</span> <span class="yarn-meta">#line:pl02_d4_1</span>
	
<span class="yarn-line">	The zoo has many...</span> <span class="yarn-meta">#line:pl02_d4_q</span>
<span class="yarn-line">	animals</span> <span class="yarn-meta">#line:pl02_d4_a1</span>
<span class="yarn-line">		Yes! Let's play a puzzle.</span> <span class="yarn-meta">#line:pl02_d4_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_4_activity&gt;&gt;</span>
<span class="yarn-line">	cars</span> <span class="yarn-meta">#line:pl02_d4_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d4_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_4_zoo&gt;&gt;</span>
<span class="yarn-line">	clouds</span> <span class="yarn-meta">#line:pl02_d4_a3</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d4_no2</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_4_zoo&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-4-activity"></a>

## dwarf_4_activity

<div class="yarn-node" data-title="dwarf_4_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play a game.</span> <span class="yarn-meta">#line:0f921e7 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_zoo dwarf_4_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-4-activity-done"></a>

## dwarf_4_activity_done

<div class="yarn-node" data-title="dwarf_4_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span> <span class="yarn-meta">#line:0c852a7 </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_4_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_4 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-5"></a>

## dwarf_5

<div class="yarn-node" data-title="dwarf_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 5) Centennial Hall</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_5_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! See you at the Sky Tower.</span> <span class="yarn-meta">#line:pl02_d5_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	The Centennial Hall is huge.</span> <span class="yarn-meta">#line:0ebb952 </span>
<span class="yarn-line">	You can see shows inside.</span> <span class="yarn-meta">#line:pl02_d5_1</span>
	
<span class="yarn-line">	What can you see inside?</span> <span class="yarn-meta">#line:pl02_d5_q</span>
<span class="yarn-line">	shows</span> <span class="yarn-meta">#line:pl02_d5_a1</span>
<span class="yarn-line">		Correct! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d5_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_5_activity&gt;&gt;</span>
<span class="yarn-line">	fish</span> <span class="yarn-meta">#line:pl02_d5_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d5_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_5&gt;&gt;</span>
<span class="yarn-line">	snow</span> <span class="yarn-meta">#line:pl02_d5_a3</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d5_no2</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-5-activity"></a>

## dwarf_5_activity

<div class="yarn-node" data-title="dwarf_5_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play a game.</span> <span class="yarn-meta">#line:06b4a16 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle centennial_hall dwarf_5_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-5-activity-done"></a>

## dwarf_5_activity_done

<div class="yarn-node" data-title="dwarf_5_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span> <span class="yarn-meta">#line:0abf458 </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_5_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_5 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-6"></a>

## dwarf_6

<div class="yarn-node" data-title="dwarf_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 6) Multimedia Fountain</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_6_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! The fountain is fun.</span> <span class="yarn-meta">#line:pl02_d6_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Water dances here with music and lights.</span> <span class="yarn-meta">#line:0daf76d </span>
	
<span class="yarn-line">	What does the water do?</span> <span class="yarn-meta">#line:pl02_d6_q</span>
<span class="yarn-line">	It dances</span> <span class="yarn-meta">#line:pl02_d6_a1</span>
<span class="yarn-line">		Yes! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d6_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_6_activity&gt;&gt;</span>
<span class="yarn-line">	It sleeps</span> <span class="yarn-meta">#line:pl02_d6_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d6_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_6&gt;&gt;</span>
<span class="yarn-line">	It eats</span> <span class="yarn-meta">#line:pl02_d6_a3</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d6_no2</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-6-activity"></a>

## dwarf_6_activity

<div class="yarn-node" data-title="dwarf_6_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Let's play a game.</span> <span class="yarn-meta">#line:0cdb4f5 </span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle wroclaw_multimedia_fountain dwarf_6_activity_done&gt;&gt;</span>


</code>
</pre>
</div>

<a id="ys-node-dwarf-6-activity-done"></a>

## dwarf_6_activity_done

<div class="yarn-node" data-title="dwarf_6_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great!</span> <span class="yarn-meta">#line:0818eec </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_6_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_6 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-7"></a>

## dwarf_7

<div class="yarn-node" data-title="dwarf_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 7) Panorama Racławicka</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_7_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! The painting is still giant.</span> <span class="yarn-meta">#line:pl02_d7_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	A giant painting goes all around you.</span> <span class="yarn-meta">#line:0de7f17 </span>
	
<span class="yarn-line">	Is this painting small or giant?</span> <span class="yarn-meta">#line:pl02_d7_q</span>
<span class="yarn-line">	Giant</span> <span class="yarn-meta">#line:pl02_d7_a1</span>
<span class="yarn-line">		Correct! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d7_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_7_activity&gt;&gt;</span>
<span class="yarn-line">	Small</span> <span class="yarn-meta">#line:pl02_d7_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d7_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-7-activity"></a>

## dwarf_7_activity

<div class="yarn-node" data-title="dwarf_7_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle panorama_raclawicka dwarf_7_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-8"></a>

## dwarf_8

<div class="yarn-node" data-title="dwarf_8">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 8) Olga Tokarczuk</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_8_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! Keep reading books.</span> <span class="yarn-meta">#line:pl02_d8_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Olga Tokarczuk is a famous writer in Wrocław.</span> <span class="yarn-meta">#line:0496de5 </span>
	
<span class="yarn-line">	Olga Tokarczuk is a...</span> <span class="yarn-meta">#line:pl02_d8_q</span>
<span class="yarn-line">	writer</span> <span class="yarn-meta">#line:pl02_d8_a1</span>
<span class="yarn-line">		Yes! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d8_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_8_activity&gt;&gt;</span>
<span class="yarn-line">	pilot</span> <span class="yarn-meta">#line:pl02_d8_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d8_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_8&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-8-activity"></a>

## dwarf_8_activity

<div class="yarn-node" data-title="dwarf_8_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle olga_tokarczuk dwarf_8_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-9"></a>

## dwarf_9

<div class="yarn-node" data-title="dwarf_9">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 9) Sky Tower Plaza Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_9_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! The plaza is busy.</span> <span class="yarn-meta">#line:pl02_d9_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	This is the modern plaza near the Sky Tower.</span> <span class="yarn-meta">#line:096a9ee </span>
	
<span class="yarn-line">	Where do people meet?</span> <span class="yarn-meta">#line:pl02_d9_q</span>
<span class="yarn-line">	In the plaza</span> <span class="yarn-meta">#line:pl02_d9_a1</span>
<span class="yarn-line">		Correct! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d9_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_9_activity&gt;&gt;</span>
<span class="yarn-line">	Under the sea</span> <span class="yarn-meta">#line:pl02_d9_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d9_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_9&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-9-activity"></a>

## dwarf_9_activity

<div class="yarn-node" data-title="dwarf_9_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle plaza_dwarf dwarf_9_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-10"></a>

## dwarf_10

<div class="yarn-node" data-title="dwarf_10">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// 9) Sky Tower Dwarf</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $dwarf_10_found&gt;&gt;</span>
<span class="yarn-line">	Hi again! The Sky Tower is still tall.</span> <span class="yarn-meta">#line:pl02_d10_repeat</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	The Sky Tower is very tall.</span> <span class="yarn-meta">#line:0ccd434 </span>
	
<span class="yarn-line">	Can you see far from the top?</span> <span class="yarn-meta">#line:pl02_d10_q</span>
<span class="yarn-line">	Yes</span> <span class="yarn-meta">#line:pl02_d10_a1</span>
<span class="yarn-line">		Correct! Let's do a puzzle.</span> <span class="yarn-meta">#line:pl02_d10_ok</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_10_activity&gt;&gt;</span>
<span class="yarn-line">	No</span> <span class="yarn-meta">#line:pl02_d10_a2</span>
<span class="yarn-line">		No. Try again.</span> <span class="yarn-meta">#line:pl02_d10_no</span>
		<span class="yarn-cmd">&lt;&lt;jump dwarf_10&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-10-activity"></a>

## dwarf_10_activity

<div class="yarn-node" data-title="dwarf_10_activity">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;activity jigsawpuzzle sky_tower_dwarf dwarf_10_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elevator-keymaster"></a>

## elevator_keymaster

<div class="yarn-node" data-title="elevator_keymaster">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Keymaster at the elevator</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I guard the elevator.</span> <span class="yarn-meta">#line:0dd986c </span>
&lt;&lt;if $found &lt; $need&gt;&gt;
<span class="yarn-line">	You found {0} / {1} dwarves. Keep exploring.</span> <span class="yarn-meta">#line:08dfa28 </span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">	Great! You found them all. I open the door with my key.</span> <span class="yarn-meta">#line:0fdc177 </span>
	<span class="yarn-cmd">&lt;&lt;task_end FIND_DWARVES&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-elevator-up"></a>

## elevator_up

<div class="yarn-node" data-title="elevator_up">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The elevator goes up. Ding!</span> <span class="yarn-meta">#line:0abe973 </span>
<span class="yarn-line">The view is beautiful.</span> <span class="yarn-meta">#line:008b500 </span>

</code>
</pre>
</div>

<a id="ys-node-npg-rescue-top"></a>

## npg_rescue_top

<div class="yarn-node" data-title="npg_rescue_top">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $top_met&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $top_met = true&gt;&gt;</span>
<span class="yarn-line">	Antura was here!</span> <span class="yarn-meta">#line:0708555 </span>
<span class="yarn-line">	But it ran away.</span> <span class="yarn-meta">#line:0f710dd </span>
<span class="yarn-line">	The view is beautiful!</span> <span class="yarn-meta">#line:079ea46 </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_intro&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-intro"></a>

## assessment_intro

<div class="yarn-node" data-title="assessment_intro">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Final Assessment</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Would you like to answer two short questions?</span> <span class="yarn-meta">#line:07982da </span>
<span class="yarn-line">Yes</span> <span class="yarn-meta">#line:012a7d1 </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">No</span> <span class="yarn-meta">#line:0d0b965 </span>
<span class="yarn-line">	Ok. Come back to me to end the game</span> <span class="yarn-meta">#line:0b51184 </span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q1"></a>

## assessment_q1

<div class="yarn-node" data-title="assessment_q1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">What is the symbol of Wrocław?</span> <span class="yarn-meta">#line:020c66a </span>
<span class="yarn-line">a dog</span> <span class="yarn-meta">#line:03ffcee </span>
<span class="yarn-line">	Not this.</span> <span class="yarn-meta">#line:07344d2 </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">a monkey</span> <span class="yarn-meta">#line:0760b20 </span>
<span class="yarn-line">	No.</span> <span class="yarn-meta">#line:0868c5e </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q1&gt;&gt;</span>
<span class="yarn-line">a dwarf</span> <span class="yarn-meta">#line:0972bf0 </span>
<span class="yarn-line">	Correct!</span> <span class="yarn-meta">#line:088b881 </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-q2"></a>

## assessment_q2

<div class="yarn-node" data-title="assessment_q2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">tags: assessment, actor=GUIDE</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Wrocław is the ... biggest city in Poland.</span> <span class="yarn-meta">#line:09d303d </span>
<span class="yarn-line">first</span> <span class="yarn-meta">#line:0ce9571 </span>
<span class="yarn-line">	No.</span> <span class="yarn-meta">#line:039419a </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">second</span> <span class="yarn-meta">#line:0fea2e7 </span>
<span class="yarn-line">	Not this.</span> <span class="yarn-meta">#line:028148c </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_q2&gt;&gt;</span>
<span class="yarn-line">third</span> <span class="yarn-meta">#line:0ebe219 </span>
<span class="yarn-line">	Correct! Well done.</span> <span class="yarn-meta">#line:0802440 </span>
	<span class="yarn-cmd">&lt;&lt;jump assessment_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-assessment-end"></a>

## assessment_end

<div class="yarn-node" data-title="assessment_end">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You finished the quest!</span> <span class="yarn-meta">#line:0fc1bad </span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-7-activity-done"></a>

## dwarf_7_activity_done

<div class="yarn-node" data-title="dwarf_7_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// Added completion nodes for dwarves 7-10</span>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job!</span> <span class="yarn-meta">#line:074a28b </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_7_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_7 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-8-activity-done"></a>

## dwarf_8_activity_done

<div class="yarn-node" data-title="dwarf_8_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job!</span> <span class="yarn-meta">#line:0ac938a </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_8_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_8 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-9-activity-done"></a>

## dwarf_9_activity_done

<div class="yarn-node" data-title="dwarf_9_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job!</span> <span class="yarn-meta">#line:0f70de9 </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_9_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_9 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-dwarf-10-activity-done"></a>

## dwarf_10_activity_done

<div class="yarn-node" data-title="dwarf_10_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: dwarves</span>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Good job!</span> <span class="yarn-meta">#line:054d57b </span>
<span class="yarn-cmd">&lt;&lt;inventory wroclaw_dwarfs add&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $dwarf_10_found = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;set $found = $found + 1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetActive dwarf_10 false&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-child"></a>

## spawned_child

<div class="yarn-node" data-title="spawned_child">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawned automatically in the scene</span>
<span class="yarn-header-dim">// each time you meet them they say one random line</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I like counting dwarves with my friend.</span> <span class="yarn-meta">#line:05166a6 </span>
<span class="yarn-line">We look up at the Sky Tower.</span> <span class="yarn-meta">#line:068b00a </span>
<span class="yarn-line">The fountain show is bright at night.</span> <span class="yarn-meta">#line:096ce85 </span>
<span class="yarn-line">The zoo has animals from many lands.</span> <span class="yarn-meta">#line:03c5f8f </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-tourist"></a>

## spawned_tourist

<div class="yarn-node" data-title="spawned_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: tourists </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I got a map to find more dwarves.</span> <span class="yarn-meta">#line:0738670 </span>
<span class="yarn-line">The cathedral towers look very tall.</span> <span class="yarn-meta">#line:0eaaf50 </span>
<span class="yarn-line">The big painting goes all around us.</span> <span class="yarn-meta">#line:0606b35 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-local"></a>

## spawned_local

<div class="yarn-node" data-title="spawned_local">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: locals </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">The market square is busy and bright.</span> <span class="yarn-meta">#line:052bf17 </span>
<span class="yarn-line">The hall has a round high roof.</span> <span class="yarn-meta">#line:05c1526 </span>
<span class="yarn-line">The keymaster guards the elevator door.</span> <span class="yarn-meta">#line:0209b93 </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-reader"></a>

## spawned_reader

<div class="yarn-node" data-title="spawned_reader">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: kids </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I read a book by Olga.</span> <span class="yarn-meta">#line:0246124 </span>
<span class="yarn-line">Olga won a big prize.</span> <span class="yarn-meta">#line:0eeaf85 </span>
<span class="yarn-line">I want to write stories one day.</span> <span class="yarn-meta">#line:08c1def </span>

</code>
</pre>
</div>


