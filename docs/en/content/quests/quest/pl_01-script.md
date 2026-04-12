---
title: Discover Warszawa (pl_01) - Script
hide:
---

# Discover Warszawa (pl_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/pl-01-discover-warszawa/32/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=1983275331#gid=1983275331)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/PL_01%20Warsaw/PL_01%20Warsaw%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// pl_01 | Warsaw</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;declare $mermaid_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $mermaid_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chopin_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $chopin_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $wars_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $sawa_met = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $wars_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $sigismund_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $sigismund_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $parliament_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $parliament_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $sword_found = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $sword_returned = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $palace_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $palace_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $stadium_started = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $stadium_done = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $quiz_score = 0&gt;&gt;</span>
Welcome to Warsaw, the capital of Poland.
Go first to the Mermaid of Warsaw by the river.

</code>
</pre>
</div>

<a id="ys-node-quest-end"></a>

## quest_end

<div class="yarn-node" data-title="quest_end">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">type: panel_endgame</span>
<span class="yarn-header-dim">---</span>
Great work. You explored Warsaw from the river to the stadium.
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
The Mermaid started your trip and got her sword back.
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
Chopin showed you where to go next.
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa&gt;&gt;</span>
Wars and Sawa told you their river story.
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
King Sigismund sent you to Parliament.
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
At Parliament, you fixed the flag and got the sword.
<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
At the Palace, you helped Maria find her wallet.
<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
At the Stadium, you helped make the city happy again.
You gave the Mermaid her sword, opened seven chests, and learned about Warsaw.
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
<span class="yarn-header-dim">---</span>
Draw your favorite place in Warsaw and the chest you opened there.
Then draw the Polish flag with white on top and red below.
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-guide-intro"></a>

## GUIDE_INTRO

<div class="yarn-node" data-title="GUIDE_INTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: intro</span>
<span class="yarn-header-dim">---</span>
Warsaw's story starts with the Mermaid, not with me.
Talk to the people in each place to keep going.

</code>
</pre>
</div>

<a id="ys-node-mermaid-square"></a>

## MERMAID_SQUARE

<div class="yarn-node" data-title="MERMAID_SQUARE">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 1 | MERMAID OF WARSAW</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$mermaid_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$mermaid_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-1"></a>

## npc_1

<div class="yarn-node" data-title="npc_1">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $mermaid_done&gt;&gt;</span>
		You already helped me, thank you! The chest is waiting for you here.
	<span class="yarn-cmd">&lt;&lt;jump chest_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $mermaid_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_1&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
		I am the Mermaid of Warsaw. I am a symbol of the city.
		Antura stole my sword while I was trying to stop him.
		Help me here at the square by sorting the ways people move around Warsaw.
		People travel by tram, bus, train, car, and bike.
		When you are done, go to the Chopin Monument. I saw Antura run that way.
	<span class="yarn-cmd">&lt;&lt;set $mermaid_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_1_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-1-done"></a>

## task_1_done

<div class="yarn-node" data-title="task_1_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
Let us sort the ways people move around Warsaw!
<span class="yarn-choice">-&gt; I need help. What should I sort?</span>
		Think about tram, bus, car, and bike.
		Put the transport cards in the right order.
<span class="yarn-choice">-&gt; I am ready to sort! #highlight</span>
	<span class="yarn-cmd">&lt;&lt;activity order activity_1_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-1-done"></a>

## activity_1_done

<div class="yarn-node" data-title="activity_1_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
		Many people ride the tram in Warsaw.
	<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
		Many people ride the bus too.
	<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
		Some people ride a bike around the city.
	<span class="yarn-cmd">&lt;&lt;set $mermaid_done = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_1&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
		Good try! Come back and sort the transport words again.
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-1"></a>

## chest_1

<div class="yarn-node" data-title="chest_1">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_1_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_1 false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
The Mermaid chest is open now.
Take the card and go south to the Chopin Monument in the park. Keep looking for my sword.

</code>
</pre>
</div>

<a id="ys-node-chopin-monument"></a>

## CHOPIN_MONUMENT

<div class="yarn-node" data-title="CHOPIN_MONUMENT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 2 | CHOPIN MONUMENT</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$mermaid_done&gt;&gt;</span>
		Help the Mermaid first, then come to my monument.
<span class="yarn-cmd">&lt;&lt;elseif !$chopin_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$chopin_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-2"></a>

## npc_2

<div class="yarn-node" data-title="npc_2">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $chopin_done&gt;&gt;</span>
		The melody is restored. The chest is yours.
	<span class="yarn-cmd">&lt;&lt;jump chest_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $chopin_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_2&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card chopin_monument&gt;&gt;</span>
		I am Fryderyk Chopin. My music is part of Warsaw.
		Antura scattered the notes of my melody around the monument.
		Come back and play the melody on the piano to restore it.
	<span class="yarn-cmd">&lt;&lt;set $chopin_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_2_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-2-done"></a>

## task_2_done

<div class="yarn-node" data-title="task_2_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
The notes are scattered around my monument.
Can you play the melody and bring them back?
<span class="yarn-choice">-&gt; How do I play the melody?</span>
		Listen carefully and watch the keys light up.
		Then tap them in the same order.
<span class="yarn-choice">-&gt; I am ready to play! #highlight</span>
	<span class="yarn-cmd">&lt;&lt;activity piano activity_2_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-2-done"></a>

## activity_2_done

<div class="yarn-node" data-title="activity_2_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
		I was a Polish composer. This is a famous place in Warsaw.
	<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_monument_warsaw&gt;&gt;</span>
		I saw Antura run past Copernicus and toward the river with the Mermaid's sword.
	<span class="yarn-cmd">&lt;&lt;set $chopin_done = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_2&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
		Good try! Come back and play the melody again.
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-2"></a>

## chest_2

<div class="yarn-node" data-title="chest_2">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_2_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_2 false&gt;&gt;</span>
The Chopin chest is open now.
Go north-east and help Wars and Sawa by the Wisla.

</code>
</pre>
</div>

<a id="ys-node-wars-and-sawa"></a>

## WARS_AND_SAWA

<div class="yarn-node" data-title="WARS_AND_SAWA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 3 | WARS AND SAWA BY THE WISLA</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$chopin_done&gt;&gt;</span>
		Chopin needs help first. Then come to the river.
<span class="yarn-cmd">&lt;&lt;elseif !$wars_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$sawa_met&gt;&gt;</span>
		Sawa is waiting by the river bend. Speak with her, then come back to Wars.
<span class="yarn-cmd">&lt;&lt;elseif !$wars_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-3"></a>

## npc_3

<div class="yarn-node" data-title="npc_3">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $wars_done&gt;&gt;</span>
		We are together again. Thank you! The chest is open.
	<span class="yarn-cmd">&lt;&lt;jump chest_3&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $sawa_met&gt;&gt;</span>
		Sawa is back! Talk to us both to hear the rest of our story.
	<span class="yarn-cmd">&lt;&lt;jump task_3_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $wars_started&gt;&gt;</span>
		Sawa is still at the river bend. Please find her.
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_3&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card wars_and_sawa_statue&gt;&gt;</span>
		I am Wars. Antura caused chaos here too, and Sawa got separated from me.
		Please find her at the bend of the Wisła and bring her back to me.
	<span class="yarn-cmd">&lt;&lt;set $wars_started = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-sawa-by-wisla"></a>

## SAWA_BY_WISLA

<div class="yarn-node" data-title="SAWA_BY_WISLA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$wars_started&gt;&gt;</span>
		Wars is looking for me. Please speak with him first.
<span class="yarn-cmd">&lt;&lt;elseif !$sawa_met&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card wars_and_sawa&gt;&gt;</span>
		I am Sawa. The Wisła is the longest river in Poland and this is our home.
		Antura ran through here too and separated me from Wars.
		Please walk back to Wars. We will tell you the rest of our story together.
	<span class="yarn-cmd">&lt;&lt;set $sawa_met = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
		Return to Wars. We are both ready.
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-wars-sawa-legend"></a>

## WARS_SAWA_LEGEND

<div class="yarn-node" data-title="WARS_SAWA_LEGEND">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;jump task_3_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-3-done"></a>

## task_3_done

<div class="yarn-node" data-title="task_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
We are together again. Thank you.
Our story is one of the old stories of Warsaw.
<span class="yarn-cmd">&lt;&lt;jump activity_3_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-3-done"></a>

## activity_3_done

<div class="yarn-node" data-title="activity_3_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa&gt;&gt;</span>
The Wisla flows through Warsaw, and our story is part of the city.
Antura passed toward King Sigismund's Column in the old town.
<span class="yarn-cmd">&lt;&lt;set $wars_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump chest_3&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-3"></a>

## chest_3

<div class="yarn-node" data-title="chest_3">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_3_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_3 false&gt;&gt;</span>
Our chest is open now.
Go west to the old town square and speak with King Sigismund. He may know where the sword was left.

</code>
</pre>
</div>

<a id="ys-node-sigismund-column"></a>

## SIGISMUND_COLUMN

<div class="yarn-node" data-title="SIGISMUND_COLUMN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 4 | KING SIGISMUND'S COLUMN</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$wars_done&gt;&gt;</span>
		Help Wars and Sawa first, then come to my square.
<span class="yarn-cmd">&lt;&lt;elseif !$sigismund_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$sigismund_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-4"></a>

## npc_4

<div class="yarn-node" data-title="npc_4">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $sigismund_done&gt;&gt;</span>
		My crown is restored. The chest is yours.
	<span class="yarn-cmd">&lt;&lt;jump chest_4&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $sigismund_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_4&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
		I am King Sigismund. My column stands in the old town near the Royal Castle.
		I moved the capital to Warsaw long ago. Today, Parliament leads Poland.
		Antura knocked three pieces of my crown off. Find them and bring them back.
	<span class="yarn-cmd">&lt;&lt;set $sigismund_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_4_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-4-done"></a>

## task_4_done

<div class="yarn-node" data-title="task_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
Have you found all three crown pieces?
<span class="yarn-choice">-&gt; Not yet. Where should I look?</span>
		Search the corners of the old town square near my column.
		The three golden pieces are glowing.
<span class="yarn-choice">-&gt; Yes, I have all three! #highlight</span>
		Excellent. My crown is complete again.
		Now I can tell you where the sword went next.
	<span class="yarn-cmd">&lt;&lt;jump activity_4_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-4-done"></a>

## activity_4_done

<div class="yarn-node" data-title="activity_4_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card person_king_sigismund&gt;&gt;</span>
People remember me because the capital moved to Warsaw.
I heard the Mermaid's sword was left at the Houses of Parliament.
<span class="yarn-cmd">&lt;&lt;set $sigismund_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump chest_4&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-4"></a>

## chest_4

<div class="yarn-node" data-title="chest_4">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_4_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_4 false&gt;&gt;</span>
My chest is open now.
Continue east to the Houses of Parliament.

</code>
</pre>
</div>

<a id="ys-node-president-parliament"></a>

## PRESIDENT_PARLIAMENT

<div class="yarn-node" data-title="PRESIDENT_PARLIAMENT">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 5 | HOUSES OF PARLIAMENT</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$sigismund_done&gt;&gt;</span>
		Please visit King Sigismund first.
<span class="yarn-cmd">&lt;&lt;elseif !$parliament_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$parliament_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-5"></a>

## npc_5

<div class="yarn-node" data-title="npc_5">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $parliament_done&gt;&gt;</span>
		You fixed the flag and have the sword. The chest is yours.
	<span class="yarn-cmd">&lt;&lt;jump chest_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $parliament_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_5&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
		Welcome to the Polish Houses of Parliament. Important laws are made here.
		I also work near the Presidential Palace, and yes, I have the Mermaid's sword.
		But first, the flag pieces were scattered. Sort the white and red parts back in the right order.
	<span class="yarn-cmd">&lt;&lt;set $parliament_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_5_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-5-done"></a>

## task_5_done

<div class="yarn-node" data-title="task_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
The flag pieces are scattered. Put them in the correct order!
<span class="yarn-choice">-&gt; I need a hint.</span>
		White is on top like snow. Red is below like a red carpet.
		Put them in that order.
<span class="yarn-choice">-&gt; Let us sort the flag now! #highlight</span>
	<span class="yarn-cmd">&lt;&lt;activity order activity_5_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-5-done"></a>

## activity_5_done

<div class="yarn-node" data-title="activity_5_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
		The Constitution of 3 May is an important day in Poland.
	<span class="yarn-cmd">&lt;&lt;card mermaids_sword&gt;&gt;</span>
		Here is the Mermaid's sword. Keep it safe and finish helping the city before you return it.
	<span class="yarn-cmd">&lt;&lt;set $sword_found = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $parliament_done = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_5&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
		Not quite right. The Polish flag has white on top and red below. Try again!
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-5"></a>

## chest_5

<div class="yarn-node" data-title="chest_5">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_5_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_5 false&gt;&gt;</span>
The Parliament chest is open now.
You have the sword now. Continue to the Palace of Culture and Science.

</code>
</pre>
</div>

<a id="ys-node-palace-culture-maria"></a>

## PALACE_CULTURE_MARIA

<div class="yarn-node" data-title="PALACE_CULTURE_MARIA">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 6 | PALACE OF CULTURE AND SCIENCE</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$parliament_done&gt;&gt;</span>
		Visit Parliament first, then come to the city center.
<span class="yarn-cmd">&lt;&lt;elseif !$palace_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$palace_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-6"></a>

## npc_6

<div class="yarn-node" data-title="npc_6">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $palace_done&gt;&gt;</span>
		You found my wallet. The chest is open. Thank you!
	<span class="yarn-cmd">&lt;&lt;jump chest_6&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $palace_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_6&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
		I am Maria Skłodowska-Curie. I am a Polish scientist and this grand building fits my story.
		Antura scattered six złoty coins around here and my wallet is missing!
		Follow the trail of coins to find it. In Poland, the złoty is the currency we use.
	<span class="yarn-cmd">&lt;&lt;set $palace_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_6_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-6-done"></a>

## task_6_done

<div class="yarn-node" data-title="task_6_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
Did you follow all six coins and find my wallet?
<span class="yarn-choice">-&gt; Not yet. Where should I look?</span>
		Follow the złoty coins from this spot. Each coin leads to the next.
		The last coin is right next to my wallet.
<span class="yarn-choice">-&gt; Yes, I found your wallet! #highlight</span>
		You found my wallet. Thank you so much!
	<span class="yarn-cmd">&lt;&lt;jump activity_6_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-6-done"></a>

## activity_6_done

<div class="yarn-node" data-title="activity_6_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
I was a Polish scientist.
<span class="yarn-cmd">&lt;&lt;card zoty_coins&gt;&gt;</span>
The złoty is the currency used in Poland.
<span class="yarn-cmd">&lt;&lt;set $palace_done = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump chest_6&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-6"></a>

## chest_6

<div class="yarn-node" data-title="chest_6">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_6_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_6 false&gt;&gt;</span>
The Palace chest is open now.
Cross the river and go to the National Stadium.

</code>
</pre>
</div>

<a id="ys-node-national-stadium"></a>

## NATIONAL_STADIUM

<div class="yarn-node" data-title="NATIONAL_STADIUM">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// PLACE 7 | NATIONAL STADIUM</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$palace_done&gt;&gt;</span>
		Visit the Palace of Culture first, then come to the stadium.
<span class="yarn-cmd">&lt;&lt;elseif !$stadium_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif !$stadium_done&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-7"></a>

## npc_7

<div class="yarn-node" data-title="npc_7">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $stadium_done&gt;&gt;</span>
		You scored all the goals! The chest is yours.
	<span class="yarn-cmd">&lt;&lt;jump chest_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $stadium_started&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;detour info_7&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
		I am Robert Lewandowski. Welcome to the National Stadium!
		Antura made a big mess here and everyone is sad.
		Score five goals to cheer up the crowd, then prove you know your sport words.
	<span class="yarn-cmd">&lt;&lt;card robert_lewandowski&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $stadium_started = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump task_7_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-task-7-done"></a>

## task_7_done

<div class="yarn-node" data-title="task_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
Great goals! Now show me how well you know your football words.
<span class="yarn-choice">-&gt; I need a hint.</span>
		Think about the ball, the field, the players, and the goal.
<span class="yarn-choice">-&gt; Take the sport quiz! #highlight</span>
	<span class="yarn-cmd">&lt;&lt;activity quiz activity_7_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-7-done"></a>

## activity_7_done

<div class="yarn-node" data-title="activity_7_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if GetActivityResult("") &gt; 0&gt;&gt;
	<span class="yarn-cmd">&lt;&lt;card football_soccer&gt;&gt;</span>
		Football is one of the most popular sports played here.
	<span class="yarn-cmd">&lt;&lt;card ball&gt;&gt;</span>
		The ball is what players kick across the field.
	<span class="yarn-cmd">&lt;&lt;card goal&gt;&gt;</span>
		The goal is where players try to score.
	<span class="yarn-cmd">&lt;&lt;card soccer_field&gt;&gt;</span>
		The soccer field is the place where the match is played.
	<span class="yarn-cmd">&lt;&lt;card mazurek_dabrowskiego&gt;&gt;</span>
		Mazurek Dąbrowskiego is Poland's national anthem.
	<span class="yarn-cmd">&lt;&lt;card independence_day_poland&gt;&gt;</span>
		Poland celebrates Independence Day on 11 November.
	<span class="yarn-cmd">&lt;&lt;set $stadium_done = true&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump chest_7&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
		Good effort! Come back and try the sport quiz again.
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-chest-7"></a>

## chest_7

<div class="yarn-node" data-title="chest_7">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;trigger chest_7_open&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;SetInteractable chest_7 false&gt;&gt;</span>
The stadium chest is open now.
Return to the Mermaid and give her back the sword.

</code>
</pre>
</div>

<a id="ys-node-mermaid-return"></a>

## MERMAID_RETURN

<div class="yarn-node" data-title="MERMAID_RETURN">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// FINALE | RETURN TO THE MERMAID</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$sword_found&gt;&gt;</span>
		Please follow Antura's trail and bring my sword back when the city is calm again.
<span class="yarn-cmd">&lt;&lt;elseif !$stadium_done&gt;&gt;</span>
		You found my sword. Thank you. Keep it safe a little longer and finish helping the other places first.
<span class="yarn-cmd">&lt;&lt;elseif !$sword_returned&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump npc_final&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump GUIDE_OUTRO&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-npc-final"></a>

## npc_final

<div class="yarn-node" data-title="npc_final">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mermaids_sword&gt;&gt;</span>
You found my sword. Thank you.
I am a symbol of Warsaw, and I protect the city.
Now the city is calm again, and I can take it back.
<span class="yarn-cmd">&lt;&lt;set $sword_returned = true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;jump activity_final_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-activity-final-done"></a>

## activity_final_done

<div class="yarn-node" data-title="activity_final_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">---</span>
Stay with me for the final quiz and the ending.
<span class="yarn-cmd">&lt;&lt;jump GUIDE_OUTRO&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-guide-outro"></a>

## GUIDE_OUTRO

<div class="yarn-node" data-title="GUIDE_OUTRO">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">---</span>
You finished the trip, returned the sword, and helped the city.
Now let us see what you remember.
<span class="yarn-cmd">&lt;&lt;jump FINAL_QUIZ&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-quiz"></a>

## FINAL_QUIZ

<div class="yarn-node" data-title="FINAL_QUIZ">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">type: quiz</span>
<span class="yarn-header-dim">group: finale</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if !$sword_returned&gt;&gt;</span>
		Return the sword to the Mermaid before taking the final quiz.
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;set $quiz_score = 0&gt;&gt;</span>
		Final question one: which colors are on the Polish flag?
<span class="yarn-choice">		-&gt; White and red.</span>
		<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
				Correct.
<span class="yarn-choice">		-&gt; Blue and red.</span>
				Not this time. The Polish flag is white and red.
<span class="yarn-choice">		-&gt; Green and white.</span>
				Not this time. The Polish flag is white and red.

		Final question two: who was Chopin?
<span class="yarn-choice">		-&gt; A composer.</span>
		<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
				Correct.
<span class="yarn-choice">		-&gt; A football player.</span>
				Not this time. Chopin was a composer.
<span class="yarn-choice">		-&gt; A king.</span>
				Not this time. Chopin was a composer.

		Final question three: what is the Wisla?
<span class="yarn-choice">		-&gt; A river.</span>
		<span class="yarn-cmd">&lt;&lt;set $quiz_score = $quiz_score + 1&gt;&gt;</span>
				Correct.
<span class="yarn-choice">		-&gt; A palace.</span>
				Not this time. The Wisla is a river.
<span class="yarn-choice">		-&gt; A stadium.</span>
				Not this time. The Wisla is a river.

	&lt;&lt;if $quiz_score &gt;= 2&gt;&gt;
				Well done. You know a lot about Warsaw.
	<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
				Good try. You can explore the city again and learn more.
	<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
	<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-facts-transport"></a>

## FACTS_TRANSPORT

<div class="yarn-node" data-title="FACTS_TRANSPORT">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card tram&gt;&gt;</span>
Trams are a common way to move around Warsaw.
<span class="yarn-cmd">&lt;&lt;card bus&gt;&gt;</span>
Buses help people travel across the city too.
<span class="yarn-cmd">&lt;&lt;card car&gt;&gt;</span>
Cars also move through the city streets every day.
<span class="yarn-cmd">&lt;&lt;card bike&gt;&gt;</span>
Bikes are a simple way to move from place to place.

</code>
</pre>
</div>

<a id="ys-node-facts-history"></a>

## FACTS_HISTORY

<div class="yarn-node" data-title="FACTS_HISTORY">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
King Sigismund's Column is a famous place in Warsaw.
<span class="yarn-cmd">&lt;&lt;card royal_castle_warsaw&gt;&gt;</span>
The Royal Castle stands close to the old town square.
<span class="yarn-cmd">&lt;&lt;card constitution_of_3_may&gt;&gt;</span>
The Constitution of 3 May is an important day in Polish history.

</code>
</pre>
</div>

<a id="ys-node-facts-science"></a>

## FACTS_SCIENCE

<div class="yarn-node" data-title="FACTS_SCIENCE">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
Maria Skłodowska-Curie was a famous scientist from Poland.
<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
The Palace of Culture and Science is a famous building in Warsaw.
<span class="yarn-cmd">&lt;&lt;card nicolaus_copernicus_monument_warsaw&gt;&gt;</span>
Copernicus is another famous person you can learn about in Warsaw.

</code>
</pre>
</div>

<a id="ys-node-facts-symbols"></a>

## FACTS_SYMBOLS

<div class="yarn-node" data-title="FACTS_SYMBOLS">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: Warsaw</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
The Mermaid is one of the main symbols of Warsaw.
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa&gt;&gt;</span>
Wars and Sawa are part of an old story about the city.
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
Warsaw has symbols, history, and important buildings.

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-local"></a>

## spawned_warsaw_local

<div class="yarn-node" data-title="spawned_warsaw_local">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_locals</span>
<span class="yarn-header-dim">---</span>
=&gt; The Wisla River flows through Warsaw.
=&gt; The Mermaid is a symbol of the city.
=&gt; I use the tram to move around Warsaw.
=&gt; The stadium is across the river.

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-guide"></a>

## spawned_warsaw_guide

<div class="yarn-node" data-title="spawned_warsaw_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">spawn_group: warsaw_guides</span>
<span class="yarn-header-dim">---</span>
=&gt; Chopin was a famous Polish composer.
=&gt; King Sigismund moved the capital to Warsaw.
=&gt; The Palace of Culture stands in the city center.
=&gt; Parliament is where laws are discussed.

</code>
</pre>
</div>

<a id="ys-node-spawned-warsaw-student"></a>

## spawned_warsaw_student

<div class="yarn-node" data-title="spawned_warsaw_student">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">spawn_group: warsaw_students</span>
<span class="yarn-header-dim">---</span>
=&gt; The Polish flag is white and red.
=&gt; Maria Skłodowska-Curie was a scientist.
=&gt; We play football at the National Stadium.
=&gt; Independence Day is on 11 November.

</code>
</pre>
</div>

<a id="ys-node-info-1"></a>

## info_1

<div class="yarn-node" data-title="info_1">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">// -----------------------------------------------------------------------------</span>
<span class="yarn-header-dim">// INFO DETOURS (shown once on first NPC visit via &lt;&lt;detour info_X&gt;&gt;)</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: mermaid</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card mermaid_of_warsaw&gt;&gt;</span>
The Mermaid of Warsaw is called the Syrenka.
She is half woman, half fish, and carries a sword and a shield.
You can find her on the city coat of arms and all over Warsaw.

</code>
</pre>
</div>

<a id="ys-node-info-2"></a>

## info_2

<div class="yarn-node" data-title="info_2">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: chopin</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card chopin_monument&gt;&gt;</span>
This monument stands in Łazienki Park in Warsaw.
Every summer, free piano concerts are held here on Sundays.
<span class="yarn-cmd">&lt;&lt;card fryderyk_chopin&gt;&gt;</span>
Fryderyk Chopin was born near Warsaw in 1810.
He is one of the most famous composers in the world.

</code>
</pre>
</div>

<a id="ys-node-info-3"></a>

## info_3

<div class="yarn-node" data-title="info_3">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: wars_sawa</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card wars_and_sawa_statue&gt;&gt;</span>
Wars and Sawa are characters from an old legend about Warsaw.
Their names are said to be hidden inside the name "Warszawa."
The Wisła flows through Warsaw all the way to the Baltic Sea.

</code>
</pre>
</div>

<a id="ys-node-info-4"></a>

## info_4

<div class="yarn-node" data-title="info_4">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: sigismund</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card king_sigismunds_column&gt;&gt;</span>
King Sigismund III Vasa moved the capital of Poland from Kraków to Warsaw in 1596.
His column on Castle Square is one of the oldest secular monuments in Warsaw.
<span class="yarn-cmd">&lt;&lt;card royal_castle_warsaw&gt;&gt;</span>
The Royal Castle stands right next to the column.
It was destroyed in World War II and fully rebuilt by 1984.

</code>
</pre>
</div>

<a id="ys-node-info-5"></a>

## info_5

<div class="yarn-node" data-title="info_5">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: parliament</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card polish_houses_of_parliament&gt;&gt;</span>
The Polish parliament is called the Sejm and the Senate.
It meets in Warsaw and votes on the laws of Poland.
<span class="yarn-cmd">&lt;&lt;card presidential_palace&gt;&gt;</span>
The Presidential Palace is nearby on Krakowskie Przedmieście.
It is where the President of Poland works.

</code>
</pre>
</div>

<a id="ys-node-info-6"></a>

## info_6

<div class="yarn-node" data-title="info_6">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: palace</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card palace_of_culture_and_science&gt;&gt;</span>
The Palace of Culture and Science is the tallest building in Poland.
It was built in the 1950s and has a viewing terrace at the top.
<span class="yarn-cmd">&lt;&lt;card maria_skodowskacurie&gt;&gt;</span>
Maria Skłodowska-Curie was born in Warsaw in 1867.
She was the first woman to win a Nobel Prize, and she won it twice.

</code>
</pre>
</div>

<a id="ys-node-info-7"></a>

## info_7

<div class="yarn-node" data-title="info_7">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">group: stadium</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card national_stadium_warsaw&gt;&gt;</span>
The PGE Narodowy stands on the right bank of the Wisła.
It opened in 2012 and hosted matches during UEFA Euro 2012.
<span class="yarn-cmd">&lt;&lt;card robert_lewandowski&gt;&gt;</span>
Robert Lewandowski is one of Poland's most famous footballers.
He has scored hundreds of goals for club and country.

</code>
</pre>
</div>


