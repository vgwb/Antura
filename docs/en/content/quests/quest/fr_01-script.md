---
title: Paris! (fr_01) - Script
hide:
---

# Paris! (fr_01) - Script
> [!note] Educators & Designers: help improving this quest!
> **Comments and feedback**: [discuss in the Forum](https://antura.discourse.group/t/fr-01-paris/23/1)  
> **Improve script translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1FPFOy8CHor5ArSg57xMuPAG7WM27-ecDOiU-OmtHgjw/edit?gid=755037318#gid=755037318)  
> **Improve Cards translations**: [comment the Google Sheet](https://docs.google.com/spreadsheets/d/1M3uOeqkbE4uyDs5us5vO-nAFT8Aq0LGBxjjT_CSScWw/edit?gid=415931977#gid=415931977)  
> **Improve the script**: [propose an edit here](https://github.com/vgwb/Antura/blob/main/Assets/_discover/_quests/FR_01%20Paris/FR_01%20Paris%20-%20Yarn%20Script.yarn)  

<a id="ys-node-quest-start"></a>

## quest_start

<div class="yarn-node" data-title="quest_start">
<pre class="yarn-code" style="--node-color:red"><code>
<span class="yarn-header-dim">// fr_01 | Paris</span>
<span class="yarn-header-dim">// </span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">type: panel</span>
<span class="yarn-header-dim">color: red</span>
<span class="yarn-header-dim">actor: NARRATOR</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;set $TOTAL_COINS = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $BAGUETTE_STEP = 0&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;declare $met_tutor = false&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;area area_init&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">We are in Paris, the capital of France.</span> <span class="yarn-meta">#line:start </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Today we will explore the Eiffel Tower</span> <span class="yarn-meta">#line:start_1</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">and Notre-Dame.</span> <span class="yarn-meta">#line:start_1a</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">But first, let's eat a baguette!</span> <span class="yarn-meta">#line:start_1b</span>
<span class="yarn-cmd">&lt;&lt;target tutor&gt;&gt;</span>
<span class="yarn-line">Are you ready? Let's start!</span> <span class="yarn-meta">#line:start_2</span>

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
<span class="yarn-line">Congratulations!</span> <span class="yarn-meta">#line:0d11596 </span>
<span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">Did you like Paris?</span> <span class="yarn-meta">#line:0d11596a</span>
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
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Do you want to draw the Eiffel Tower?</span> <span class="yarn-meta">#line:002620f</span>
<span class="yarn-cmd">&lt;&lt;quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-tutor"></a>

## talk_tutor

<div class="yarn-node" data-title="talk_tutor">
<pre class="yarn-code" style="--node-color:blue"><code>
<span class="yarn-header-dim">actor:</span>
<span class="yarn-header-dim">color: blue</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;if $met_tutor == false&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;set $met_tutor = true&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card capital_paris&gt;&gt;</span>
<span class="yarn-line">    Hello! Have you ever been here in Paris?</span> <span class="yarn-meta">#line:talk_tutor_0</span>
<span class="yarn-line">    Yes!</span> <span class="yarn-meta">#line:talk_tutor_0b</span>
<span class="yarn-line">        Great! Let's see if you remember these places.</span> <span class="yarn-meta">#line:talk_tutor_0c</span>
<span class="yarn-line">    No.</span> <span class="yarn-meta">#line:talk_tutor_0d</span>
<span class="yarn-line">        I hope you can come here one day!</span> <span class="yarn-meta">#line:talk_tutor_0e</span>
<span class="yarn-line">    I saw Antura go to the baker. Let's go there!</span> <span class="yarn-meta">#line:talk_tutor</span>
    <span class="yarn-cmd">&lt;&lt;area area_bakery&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump spawned_man&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-baker"></a>

## talk_baker

<div class="yarn-node" data-title="talk_baker">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;if $BAGUETTE_STEP == 1&gt;&gt;</span>
<span class="yarn-line">    Great! We have flour.</span> <span class="yarn-meta">#line:baker_r1</span>
    <span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">    Now I need the salt.</span> <span class="yarn-meta">#line:06cccc0 </span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_notredame&gt;&gt;</span>
<span class="yarn-line">    Go to Notre-Dame.</span> <span class="yarn-meta">#line:baker_r2 #task:go_notredame</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_notredame&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_notredame&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
<span class="yarn-line">    Salt! Well done.</span> <span class="yarn-meta">#line:baker_r3</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_louvre&gt;&gt;</span>
<span class="yarn-line">    Antura took the water and went to the Louvre.</span> <span class="yarn-meta">#line:baker_r4 #task:go_louvre</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_louvre&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_louvre&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
<span class="yarn-line">    Perfect! We have water.</span> <span class="yarn-meta">#line:baker_r5</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_arc&gt;&gt;</span>
<span class="yarn-line">    Maybe the yeast is at the Arc de Triomphe.</span> <span class="yarn-meta">#line:baker_r6 #task:go_arc</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_arc&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_arc&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 4&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;jump baker_finish&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card person_baker&gt;&gt;</span>
<span class="yarn-line">    Hello! I am a baker. I make bread every day.</span> <span class="yarn-meta">#line:baker_0</span>
    <span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">    Today I want to make a baguette...</span> <span class="yarn-meta">#line:baker_1</span>
<span class="yarn-line">    But I lost the ingredients!</span> <span class="yarn-meta">#line:baker_2</span>
    <span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">    A big blue dog stole my flour!</span> <span class="yarn-meta">#line:baker_3</span>
<span class="yarn-line">    Can you help me find them?</span> <span class="yarn-meta">#line:baker_4</span>
    <span class="yarn-cmd">&lt;&lt;camera_focus camera_eiffell&gt;&gt;</span>
<span class="yarn-line">    Go to the Eiffel Tower.</span> <span class="yarn-meta">#line:baker_5 #task:go_eiffell</span>
    <span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 0&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;area area_all&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;task_start go_eiffell&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baker-finish"></a>

## baker_finish

<div class="yarn-node" data-title="baker_finish">
<pre class="yarn-code" style="--node-color:green"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: green</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card food_baguette&gt;&gt;</span>
<span class="yarn-line">You found all the ingredients!</span> <span class="yarn-meta">#line:baker_finish_0</span>
<span class="yarn-line">Now I can make a baguette.</span> <span class="yarn-meta">#line:baker_finish_1</span>
<span class="yarn-line">Let's see if you remember what you learned in Paris.</span> <span class="yarn-meta">#line:076b3e3 </span>
<span class="yarn-cmd">&lt;&lt;activity match_paris_final final_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-final-activity-done"></a>

## final_activity_done

<div class="yarn-node" data-title="final_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You solved the puzzle.</span> <span class="yarn-meta">#line:puzzle_done</span>
<span class="yarn-cmd">&lt;&lt;jump quest_end&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-flour"></a>

## baguette_flour

<div class="yarn-node" data-title="baguette_flour">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_flour&gt;&gt;</span>
<span class="yarn-line">You found flour!</span> <span class="yarn-meta">#line:06022b0 </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 1&gt;&gt;</span>
<span class="yarn-line">Go back to the baker.</span> <span class="yarn-meta">#line:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-salt"></a>

## baguette_salt

<div class="yarn-node" data-title="baguette_salt">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_salt&gt;&gt;</span>
<span class="yarn-line">You found salt.</span> <span class="yarn-meta">#line:00f1d2f </span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 2&gt;&gt;</span>
<span class="yarn-line">Go back to the baker.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker </span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-water"></a>

## baguette_water

<div class="yarn-node" data-title="baguette_water">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_water&gt;&gt;</span>
<span class="yarn-line">This is water.</span> <span class="yarn-meta">#line:0c4d1f6</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 3&gt;&gt;</span>
<span class="yarn-line">Go back to the baker.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-baguette-yeast"></a>

## baguette_yeast

<div class="yarn-node" data-title="baguette_yeast">
<pre class="yarn-code" style="--node-color:yellow"><code>
<span class="yarn-header-dim">group: bakery</span>
<span class="yarn-header-dim">color: yellow</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card food_yeast&gt;&gt;</span>
<span class="yarn-line">You found yeast!</span> <span class="yarn-meta">#line:025865d</span>
<span class="yarn-cmd">&lt;&lt;set $BAGUETTE_STEP = 4&gt;&gt;</span>
<span class="yarn-line">Go back to the baker.</span> <span class="yarn-meta">#shadow:go_back_baker #task:go_baker</span>
<span class="yarn-cmd">&lt;&lt;target baker&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;task_start go_baker&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-ticket"></a>

## talk_eiffell_ticket

<div class="yarn-node" data-title="talk_eiffell_ticket">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">////////// EIFFEL TOWER: pay 5 coins -&gt; roof -&gt; chest flour</span>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-line">Good morning. What do you want?</span> <span class="yarn-meta">#line:09e454b </span>
<span class="yarn-line">A ticket to go up the Eiffel Tower.</span> <span class="yarn-meta">#line:0141851 </span>
    <span class="yarn-cmd">&lt;&lt;if HasCompletedTask("collect_coins")&gt;&gt;</span>
<span class="yarn-line">        Select the money to pay.</span> <span class="yarn-meta">#line:0f44ea7 </span>
        <span class="yarn-cmd">&lt;&lt;activity money_elevator ticket_payment_done&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;elseif GetCurrentTask() == "collect_coins"&gt;&gt;</span>
<span class="yarn-line">        The ticket costs 5 coins.</span> <span class="yarn-meta">#line:069cbb3</span>
<span class="yarn-line">        Look around and pick up coins.</span> <span class="yarn-meta">#shadow:0097a65</span>
    <span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">        The ticket costs 5 coins.</span> <span class="yarn-meta">#shadow:069cbb3</span>
<span class="yarn-line">        Look around and pick up coins.</span> <span class="yarn-meta">#line:0097a65 #task:collect_coins</span>
        <span class="yarn-cmd">&lt;&lt;task_start collect_coins coins_collected&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>
<span class="yarn-line">A baguette.</span> <span class="yarn-meta">#line:03dc852 </span>
<span class="yarn-line">   There is a bakery near here. But it opens later.</span> <span class="yarn-meta">#line:0cbdcce </span>
<span class="yarn-line">Just to look around.</span> <span class="yarn-meta">#line:0718e4a </span>
<span class="yarn-line">   Enjoy your visit!</span> <span class="yarn-meta">#line:006fcf2 </span>

</code>
</pre>
</div>

<a id="ys-node-ticket-payment-done"></a>

## ticket_payment_done

<div class="yarn-node" data-title="ticket_payment_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">tags:</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_ticket&gt;&gt;</span>
<span class="yarn-line">Here is your ticket.</span> <span class="yarn-meta">#line:04e74ad </span>
<span class="yarn-cmd">&lt;&lt;action activate_elevator&gt;&gt;</span>
<span class="yarn-line">I saw Antura going up to the top of the tower.</span> <span class="yarn-meta">#line:089abda</span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_roof&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">Take the elevator or use the stairs!</span> <span class="yarn-meta">#line:0585a5e </span>

</code>
</pre>
</div>

<a id="ys-node-coins-collected"></a>

## coins_collected

<div class="yarn-node" data-title="coins_collected">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">You have enough coins now to buy the ticket.</span> <span class="yarn-meta">#line:0ba42cd </span>
<span class="yarn-cmd">&lt;&lt;target npc_eiffell_ticket&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-guide"></a>

## talk_eiffell_guide

<div class="yarn-node" data-title="talk_eiffell_guide">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>

<span class="yarn-header-dim">---</span>
<span class="yarn-line">Hello. What do you want to know?</span> <span class="yarn-meta">#line:0070084 </span>
<span class="yarn-line">What is the Eiffel Tower?</span> <span class="yarn-meta">#line:0d91dc0 </span>
<span class="yarn-line">    A tall iron tower, about 300 meters high.</span> <span class="yarn-meta">#line:0f17af0 </span>
<span class="yarn-line">    It is a special symbol of Paris!</span> <span class="yarn-meta">#line:07a113f </span>
<span class="yarn-line">Where are we?</span> <span class="yarn-meta">#line:09dd1da </span>
<span class="yarn-line">    We are in Paris.</span> <span class="yarn-meta">#line:02b627d </span>
<span class="yarn-line">Is this place real?</span> <span class="yarn-meta">#line:08bede4 </span>
<span class="yarn-line">    Yes! Why do you ask?</span> <span class="yarn-meta">#line:08654e6 </span>
<span class="yarn-line">    Well... it looks like a video game.</span> <span class="yarn-meta">#line:0bc62a3 </span>
<span class="yarn-line">Nothing. Bye.</span> <span class="yarn-meta">#line:0fe0732 #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-talk-eiffell-roof"></a>

## talk_eiffell_roof

<div class="yarn-node" data-title="talk_eiffell_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: toureiffel</span>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower&gt;&gt;</span>
<span class="yarn-line">Welcome to the top of the Eiffel Tower!</span> <span class="yarn-meta">#line:0da46e8 </span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_map&gt;&gt;</span>
<span class="yarn-line">The Eiffel Tower is 300 meters tall.</span> <span class="yarn-meta">#line:08c1973 </span>
<span class="yarn-cmd">&lt;&lt;card gustave_eiffel&gt;&gt;</span>
<span class="yarn-line">Gustave Eiffel built it in 1887.</span> <span class="yarn-meta">#line:09e5c3b </span>
<span class="yarn-cmd">&lt;&lt;card iron_material&gt;&gt;</span>
<span class="yarn-line">It is made of iron!</span> <span class="yarn-meta">#line:0d59ade </span>
<span class="yarn-cmd">&lt;&lt;card worlds_fair_1889&gt;&gt;</span>
<span class="yarn-line">It was built for a big fair long ago.</span> <span class="yarn-meta">#line:0d59ade_fair</span>
&lt;&lt;if GetActivityResult("memory_eiffell") &lt; 1 &gt;&gt;
<span class="yarn-line">    To open the chest, solve the puzzle!</span> <span class="yarn-meta">#line:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity memory_eiffell eiffell_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-eiffell-activity-done"></a>

## eiffell_activity_done

<div class="yarn-node" data-title="eiffell_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: </span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You solved the puzzle.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">The chest is now unlocked.</span> <span class="yarn-meta">#line:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_flour true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_flour&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame"></a>

## talk_notre_dame

<div class="yarn-node" data-title="talk_notre_dame">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 1&gt;&gt;
<span class="yarn-line">    Come back later.</span> <span class="yarn-meta">#line:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 1&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris&gt;&gt;</span>
<span class="yarn-line">    This is Notre-Dame Cathedral.</span> <span class="yarn-meta">#line:06f3fa2 </span>
    <span class="yarn-cmd">&lt;&lt;card cathedral&gt;&gt;</span>
<span class="yarn-line">    A cathedral is a very big church.</span> <span class="yarn-meta">#line:06f3fa2_cathedral</span>
    <span class="yarn-cmd">&lt;&lt;card church&gt;&gt;</span>
<span class="yarn-line">    A church is a place where people go to pray.</span> <span class="yarn-meta">#line:fr01_notredame_base_3</span>
<span class="yarn-line">    It is a very old church. It was built long ago.</span> <span class="yarn-meta">#line:0ac5a72 </span>
<span class="yarn-line">    Go up to the roof through that portal!</span> <span class="yarn-meta">#line:083dfcc</span>
    <span class="yarn-cmd">&lt;&lt;action activate_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    We already solved this part.</span> <span class="yarn-meta">#line:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-notre-dame-roof"></a>

## talk_notre_dame_roof

<div class="yarn-node" data-title="talk_notre_dame_roof">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card notre_dame_de_paris_fire&gt;&gt;</span>
<span class="yarn-line">There was a big fire in 2019, but it is fixed now.</span> <span class="yarn-meta">#line:09a0ead </span>
<span class="yarn-line">To open the chest, solve the puzzle!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
<span class="yarn-cmd">&lt;&lt;activity memory_notredame notredame_activity_done&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-notredame-activity-done"></a>

## notredame_activity_done

<div class="yarn-node" data-title="notredame_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You solved the puzzle.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">The chest is now unlocked.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_salt true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_salt&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-gargoyle"></a>

## gargoyle

<div class="yarn-node" data-title="gargoyle">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: notredame</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card gargoyle zoom&gt;&gt;</span>
<span class="yarn-line">Look at that statue! Is it scary?</span> <span class="yarn-meta">#line:0f7f9d8 </span>

</code>
</pre>
</div>

<a id="ys-node-talk-louvre"></a>

## talk_louvre

<div class="yarn-node" data-title="talk_louvre">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// LOUVRE: puzzle -&gt; chest unlock -&gt; water</span>
<span class="yarn-header-dim">group: louvre</span>
<span class="yarn-header-dim">actor: SENIOR_F</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 2&gt;&gt;
<span class="yarn-line">    Come back later.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 2&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card louvre&gt;&gt;</span>
<span class="yarn-line">    This is the Louvre, a famous museum.</span> <span class="yarn-meta">#line:louvre_0</span>
<span class="yarn-line">    To open the chest, solve the puzzle!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity activity_louvre louvre_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    We already solved this part.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-louvre-activity-done"></a>

## louvre_activity_done

<div class="yarn-node" data-title="louvre_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You solved the puzzle.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">The chest is now unlocked.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_water true&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-talk-arc"></a>

## talk_arc

<div class="yarn-node" data-title="talk_arc">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">///// ARC DE TRIOMPHE: puzzle -&gt; chest unlock -&gt; yeast</span>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;target off&gt;&gt;</span>
&lt;&lt;if $BAGUETTE_STEP &lt; 3&gt;&gt;
<span class="yarn-line">    Come back later.</span> <span class="yarn-meta">#shadow:come_back_later</span>
<span class="yarn-cmd">&lt;&lt;elseif $BAGUETTE_STEP == 3&gt;&gt;</span>
    <span class="yarn-cmd">&lt;&lt;card arc_de_triomphe&gt;&gt;</span>
<span class="yarn-line">    This is the Arc de Triomphe.</span> <span class="yarn-meta">#line:arc_0</span>
<span class="yarn-line">    It honors people who fought for France.</span> <span class="yarn-meta">#line:arc_0a</span>
<span class="yarn-line">    To open the chest, solve the puzzle!</span> <span class="yarn-meta">#shadow:solve_puzzle</span>
    <span class="yarn-cmd">&lt;&lt;activity jigsaw_arc arc_activity_done&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;else&gt;&gt;</span>
<span class="yarn-line">    We already solved this part.</span> <span class="yarn-meta">#shadow:already_solved</span>
<span class="yarn-cmd">&lt;&lt;endif&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-arc-activity-done"></a>

## arc_activity_done

<div class="yarn-node" data-title="arc_activity_done">
<pre class="yarn-code"><code>
<span class="yarn-header-dim">group: arc_de_triomphe</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Great! You solved the puzzle.</span> <span class="yarn-meta">#shadow:puzzle_done</span>
<span class="yarn-line">The chest is now unlocked.</span> <span class="yarn-meta">#shadow:chest_unlocked</span>
<span class="yarn-line">It is on the roof. use the teleporter to go there.</span> <span class="yarn-meta">#line:0d46853 </span>
<span class="yarn-cmd">&lt;&lt;SetActive chest_yeast true&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;action activate_arc_teleporter&gt;&gt;</span>
<span class="yarn-cmd">&lt;&lt;target chest_yeast&gt;&gt;</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-eiffell-tourist"></a>

## spawned_eiffell_tourist

<div class="yarn-node" data-title="spawned_eiffell_tourist">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">///////// NPCs SPAWNED IN THE SCENE //////////</span>
<span class="yarn-header-dim">// these npc are spawn automatically in the scene</span>
<span class="yarn-header-dim">// use these to add random facts. everythime you meet them</span>
<span class="yarn-header-dim">// they will say one of these lines randomly</span>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">I would like to go up the Eiffel Tower.</span> <span class="yarn-meta">#line:0aee9bb </span>
<span class="yarn-line">You need a ticket to go up.</span> <span class="yarn-meta">#line:09be864 </span>
<span class="yarn-line">There was a big fair in Paris in 1889.</span> <span class="yarn-meta">#line:0a3f4e1 </span>
<span class="yarn-line">    It was to celebrate a big birthday for France.</span> <span class="yarn-meta">#line:01fa210 </span>
<span class="yarn-line">    The Eiffel Tower was built for that big party.</span> <span class="yarn-meta">#line:0d6f3c4 </span>
<span class="yarn-line">I love Paris!</span> <span class="yarn-meta">#line:0bda18a </span>

</code>
</pre>
</div>

<a id="ys-node-spawned-man"></a>

## spawned_man

<div class="yarn-node" data-title="spawned_man">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Do you have questions?</span> <span class="yarn-meta">#line:07b94e9 </span>
<span class="yarn-line">Have you seen Antura?</span> <span class="yarn-meta">#line:0f18ad3 </span>
<span class="yarn-line">    No. Who is Antura?</span> <span class="yarn-meta">#line:0f9dd62 </span>
<span class="yarn-line">What are you doing?</span> <span class="yarn-meta">#line:002796f </span>
<span class="yarn-line">    I am going to buy bread at the bakery.</span> <span class="yarn-meta">#line:05a38a8 </span>
<span class="yarn-line">Where do you come from?</span> <span class="yarn-meta">#line:05eabcf </span>
<span class="yarn-line">    I was not born in this country.</span> <span class="yarn-meta">#line:0635a6a </span>
<span class="yarn-line">Goodbye</span> <span class="yarn-meta">#line:0ee51fc #highlight</span>

</code>
</pre>
</div>

<a id="ys-node-spawned-kid-f"></a>

## spawned_kid_f

<div class="yarn-node" data-title="spawned_kid_f">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: KID_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: kids</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Bonjour!</span> <span class="yarn-meta">#line:041403d </span>
<span class="yarn-line">Ca va?</span> <span class="yarn-meta">#line:04986a3 </span>

</code>
</pre>
</div>

<a id="ys-node-npc-notredame-roof"></a>

## npc_notredame_roof

<div class="yarn-node" data-title="npc_notredame_roof">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: notredame_roof</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">From the roof you can see much of Paris.</span> <span class="yarn-meta">#line:fr01_notredame_roof_1</span>
<span class="yarn-line">Stone creatures called gargoyles sit up here.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2</span>
<span class="yarn-line">These stone arms help hold up the walls.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2b #card:flying_buttress</span>
<span class="yarn-line">The big round window is called a rose window.</span> <span class="yarn-meta">#line:fr01_notredame_roof_2c #card:rose_window</span>

</code>
</pre>
</div>

<a id="ys-node-npc-eiffell-elevator"></a>

## npc_eiffell_elevator

<div class="yarn-node" data-title="npc_eiffell_elevator">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: GUIDE_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: eiffel_tower</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card eiffel_tower_elevators&gt;&gt;</span>
<span class="yarn-line">The elevator helps people go up the tower.</span> <span class="yarn-meta">#line:fr01_eiffel_elevator_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-paris-region"></a>

## npc_paris_region

<div class="yarn-node" data-title="npc_paris_region">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: ADULT_F</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: generic</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-cmd">&lt;&lt;card ile_de_france&gt;&gt;</span>
<span class="yarn-line">Paris is in a place called Île-de-France.</span> <span class="yarn-meta">#line:fr01_region_1</span>

</code>
</pre>
</div>

<a id="ys-node-npc-bakery"></a>

## npc_bakery

<div class="yarn-node" data-title="npc_bakery">
<pre class="yarn-code" style="--node-color:purple"><code>
<span class="yarn-header-dim">actor: SENIOR_M</span>
<span class="yarn-header-dim">color: purple</span>
<span class="yarn-header-dim">spawn_group: bakery</span>
<span class="yarn-header-dim">---</span>
<span class="yarn-line">Fresh bread smell makes people happy.</span> <span class="yarn-meta">#line:fr01_bakery_1</span>
<span class="yarn-line">A baguette needs flour, water, yeast, and salt.</span> <span class="yarn-meta">#line:fr01_bakery_2</span>
<span class="yarn-line">Yeast helps bread rise.</span> <span class="yarn-meta">#line:fr01_bakery_2a</span>
<span class="yarn-line">Bakers wake up very early to start making bread.</span> <span class="yarn-meta">#line:fr01_bakery_3</span>

</code>
</pre>
</div>


